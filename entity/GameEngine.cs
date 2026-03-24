using models;
using utils;
using System.Collections.Generic;
using System;

namespace entity
{
    public class GameEngine
    {
        public int DEFAULT_WIDTH { get; set; }
        public int DEFAULT_HEIGTH { get; set; }
        public int MAX_POWER { get; set; } = 9;
        public int MIN_POWER { get; set; } = 1;
        public int[][] matrix { get; set; }
        public bool[][] colored;
        public Player[] joueurs { get; set; } = new Player[2];
        public int[] scores { get; }
        public int indexPlayer { get; set; } = 0;

        // Stockage des lignes validées : (Extrémité1, Extrémité2, IDJoueur)
        public List<Tuple<Point, Point, int>> ActiveLines { get; private set; }

        private static readonly int[,] Directions = new int[,]{
            { -1,  0 }, { 0, -1 }, { -1, -1 }, { -1,  1 }
        };

        public GameEngine(Player[] ls, int w, int h)
        {
            DEFAULT_WIDTH = w;
            DEFAULT_HEIGTH = h;
            joueurs = ls;
            scores = new int[2];
            ActiveLines = new List<Tuple<Point, Point, int>>();
            matrix = new int[h][];
            colored = new bool[h][];
            for (int i = 0; i < h; i++)
            {
                matrix[i] = new int[w];
                colored[i] = new bool[w];
                for (int j = 0; j < w; j++) matrix[i][j] = -1;
            }
        }

        public void addPoint(Point p)
        {
            if (isSecure(p.X, p.Y)) matrix[p.Y][p.X] = indexPlayer;
        }

        public void nextPlayer() { indexPlayer = (indexPlayer + 1) % 2; }

        public void ifScoring(Point p)
        {
            for (int i = 0; i < Directions.GetLength(0); i++)
            {
                Point pt1 = new Point(), pt2 = new Point();
                getScoreInLine(pt1, p.X, p.Y, Directions[i, 1], Directions[i, 0]);
                getScoreInLine(pt2, p.X, p.Y, -Directions[i, 1], -Directions[i, 0]);

                // CALCUL DE LA DISTANCE DE GRILLE (Chebyshev)
                // C'est le maximum entre l'écart en X et l'écart en Y
                int deltaX = Math.Abs(pt2.X - pt1.X);
                int deltaY = Math.Abs(pt2.Y - pt1.Y);
                int nbPas = Math.Max(deltaX, deltaY);

                // Un segment de 5 points a toujours 4 intervalles, 
                // que ce soit en ligne droite ou en diagonale.
                if (nbPas >= 4)
                {
                    // Vérification d'obstruction...
                    bool isObstructed = false;
                    foreach (var line in ActiveLines)
                    {
                        if (line.Item3 != indexPlayer && MethodUtils.AreSegmentsIntersecting(pt1, pt2, line.Item1, line.Item2))
                        {
                            isObstructed = true; break;
                        }
                    }

                    if (!isObstructed)
                    {
                        scores[indexPlayer] += (nbPas - 4 + 1);
                        ActiveLines.Add(new Tuple<Point, Point, int>(new Point(pt1.X, pt1.Y), new Point(pt2.X, pt2.Y), indexPlayer));
                        MarkColored(pt1, pt2);
                    }
                }
            }
        }

        private void MarkColored(Point p1, Point p2)
        {
            int steps = (int)Math.Max(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y));
            for (int i = 0; i <= steps; i++)
            {
                float t = (steps == 0) ? 0 : (float)i / steps;
                int x = (int)Math.Round(p1.X + t * (p2.X - p1.X));
                int y = (int)Math.Round(p1.Y + t * (p2.Y - p1.Y));
                if (isSecure(x, y)) colored[y][x] = true;
            }
        }

        private void getScoreInLine(Point p, int PosX, int PosY, int PlusX, int PlusY)
        {
            p.X = PosX; p.Y = PosY;
            while (true)
            {
                int nX = p.X + PlusX, nY = p.Y + PlusY;
                if (!isSecure(nX, nY) || matrix[nY][nX] != indexPlayer) break;
                p.X = nX; p.Y = nY;
            }
        }

        public bool isSecure(int x, int y) => x >= 0 && x < DEFAULT_WIDTH && y >= 0 && y < DEFAULT_HEIGTH;

        public int fireCanon(int row, int power)
        {
            int range = (int)Math.Round((power - 1) * (DEFAULT_WIDTH - 1) / 8.0);
            if (matrix[row][range] != -1 && matrix[row][range] != indexPlayer)
            {
                if (!colored[row][range])
                {
                    matrix[row][range] = -1; 
                    return range; 
                }
            }
            return -1; // Aucun point touché
        }
    }
}