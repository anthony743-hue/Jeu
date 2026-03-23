using models;
using utils;

namespace entity
{
    class Game
    {
        private int DEFAULT_WIDTH { get; set; } = 9;
        private int DEFAULT_HEIGTH { get; set; } = 9;
        private int DEFAULT_NB_PLAYER { get; } = 2;
        public int[][] matrix { get; set; }
        private bool[][] colored;
        private Player[] joueurs { get; set; } = new Player[2];
        private int[] scores { get; }
        private int indexPlayer { get; set; } = 0;
        private List<Point[]> points;
        private static readonly int[,] Directions = new int[,]{
            { -1,  0 },  // Haut
            {  0, -1 },  // Gauche
            { -1, -1 },  // HautGauche
            { -1,  1 },  // HautDroite
        };

        public Game(Player[] ls, int w, int h)
        {
            joueurs[0] = ls[0];
            joueurs[1] = ls[1];
            scores = new int[DEFAULT_NB_PLAYER];
            matrix = new int[h][];
            points = new List<Point[]>();
            for (int i = 0; i < h; i++)
            {
                matrix[i] = new int[w];
                colored[i] = new bool[w];
                for (int j = 0; j < w; j++)
                {
                    matrix[i][j] = -1;
                    colored[i][j] = false;
                }
            }
        }

        public void addPoint(Point p)
        {
            int x = p.X, y = p.Y;
            matrix[y][x] = indexPlayer;
        }

        public void removePoint(Point p)
        {
            int x = p.X, y = p.Y;
            matrix[y][x] = -1;
        }

        public void nextPlayer()
        {
            indexPlayer = (indexPlayer + 1) % DEFAULT_NB_PLAYER;
        }

        public void ifScoring(Point p)
        {
            int PlusX = 0, PlusY = 0;
            Point pt1 = new Point(), pt2 = new Point();
            int sum = 0;
            int temp = 0;
            for (int i = 0; i < Directions.GetLength(0); i++)
            {
                PlusX = Directions[i, 1]; PlusY = Directions[i, 0];
                getScoreInLine(pt1, p.X, p.Y,PlusX, PlusY);
                
                PlusX = -PlusX; PlusY = -PlusY;
                
                getScoreInLine(pt2, p.X, p.Y, PlusX, PlusY);
                temp = (int) Point.getDistance(pt1, pt2);
                if( temp >= 5){
                    sum+= temp - 5 + 1;
                }
            }
            scores[indexPlayer]+= sum;
        }

        private void getScoreInLine(Point p, int PosX, int PosY, int PlusX, int PlusY)
        {
            int NextX = 0, NextY = 0;
            p.X = PosX;
            p.Y = PosY;
            while (isSecure(p.X, p.Y) && ! colored[p.Y][p.X]){
                NextX = p.X + PlusX;
                NextY = p.Y + PlusY;
                if (!isSecure(NextX, NextY) || !isSamePlayerPoint(p.X, p.Y, NextX, NextY)){
                    break;
                }
                p.X = NextX;
                p.Y = NextY;
            }
        }

        private bool isSecure(int PosX, int PosY)
        {
            bool boundLimits_O = 0 <= PosX && PosX < DEFAULT_WIDTH;
            bool boundLimits_1 = 0 <= PosY && PosY < DEFAULT_HEIGTH;
            return boundLimits_1 && boundLimits_O;
        }

        private bool isSamePlayerPoint(int PosX, int PosY, int NextX, int NextY)
        {
            return matrix[PosY][PosX] == matrix[NextY][NextX];
        }
    }
}