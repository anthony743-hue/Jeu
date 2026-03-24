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
        private int DEFAULT_NB_PLAYER = 2;
        public int[][] matrix { get; set; }
        public bool[][] colored;
        public Player[] joueurs { get; set; } = new Player[2];
        public int[] scores { get; }
        public int indexPlayer { get; set; } = 0;
        public int[] canonRows { get; set; }
        public Dictionary<int, List<Tuple<Point, Point>>> ActivesLines;
        private static readonly int[,] Directions = new int[,]{
            { -1,  0 }, { 0, -1 }, { -1, -1 }, { -1,  1 }
        };

        public GameEngine(Player[] ls, int w, int h)
        {
            DEFAULT_WIDTH = w;
            DEFAULT_HEIGTH = h;
            joueurs = ls;
            scores = new int[DEFAULT_NB_PLAYER];
            canonRows = new int[DEFAULT_NB_PLAYER];
            ActivesLines = new Dictionary<int, List<Tuple<Point, Point>>>();
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

        public void nextPlayer() { indexPlayer = (indexPlayer + 1) % DEFAULT_NB_PLAYER; }

        public void ifScoring(Point p)
        {
            int deltaX = 0, deltaY = 0, nbPas = 0;
            Point pt1 = new Point(), pt2 = new Point();
            for (int i = 0; i < Directions.GetLength(0); i++)
            {
                getScoreInLine(pt1, p.X, p.Y, Directions[i, 1], Directions[i, 0]);
                getScoreInLine(pt2, p.X, p.Y, -Directions[i, 1], -Directions[i, 0]);
                deltaX = Math.Abs(pt2.X - pt1.X);
                deltaY = Math.Abs(pt2.Y - pt1.Y);
                nbPas = Math.Max(deltaX, deltaY);
                if (nbPas >= 4)
                {
                    scores[indexPlayer] += (nbPas - 4 + 1);
                    if( ! ActivesLines.ContainsKey(indexPlayer))
                    {
                        ActivesLines.Add(indexPlayer, new List<Tuple<Point, Point>>());
                    }
                    ActivesLines[indexPlayer].Add(new Tuple<Point, Point>(new Point(pt1.X, pt1.Y), new Point(pt2.X, pt2.Y)));
                    MarkColored(pt1, pt2);
                }
            }
        }

        private bool isObstructed(Point pt1, Point pt2)
        {
            foreach (KeyValuePair<int, List<Tuple<Point, Point>>> entree in ActivesLines)
            {
                if (entree.Key != indexPlayer)
                {
                    foreach (var line in entree.Value)
                    {
                        if (MethodUtils.AreSegmentsIntersecting(pt1, pt2, line.Item1, line.Item2))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void MarkColored(Point p1, Point p2)
        {
            int steps = (int)Math.Max(Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y));
            int x = 0, y = 0;
            float t = 0;
            for (int i = 0; i <= steps; i++)
            {
                t = (steps == 0) ? 0 : (float)i / steps;
                x = (int)Math.Round(p1.X + t * (p2.X - p1.X));
                y = (int)Math.Round(p1.Y + t * (p2.Y - p1.Y));
                if (isSecure(x, y)) colored[y][x] = true;
            }
        }

        private void getScoreInLine(Point p, int PosX, int PosY, int PlusX, int PlusY)
        {
            p.X = PosX; p.Y = PosY;
            int nX = 0, nY = 0;
            Point pt2 = new Point();
            while (true)
            {
                nX = p.X + PlusX; nY = p.Y + PlusY;
                pt2.X = nX; pt2.Y = nY;
                if (!isSecure(nX, nY) || matrix[nY][nX] != indexPlayer || isObstructed(p, pt2)) break;
                p.X = nX; p.Y = nY;
            }
        }

        public bool isSecure(int x, int y)
        {
            return x >= 0 && x < DEFAULT_WIDTH && y >= 0 && y < DEFAULT_HEIGTH;
        }

        public int fireCanon(int power)
        {
            int row = canonRows[indexPlayer];
            int direction = (indexPlayer == 0) ? 1 : 0;

            float t = (power - 1) / 8.0f;
            int targetX = (int)Math.Round((1.0f - t) * (DEFAULT_WIDTH - 1)); ;

            if (isSecure(targetX, row))
            {
                if (matrix[row][targetX] != -1 && matrix[row][targetX] != indexPlayer && !colored[row][targetX])
                {
                    matrix[row][targetX] = -1;
                    return targetX;
                }
            }
            return -1;
        }
    }
}