namespace utils
{
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        // Constructeur
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point()
        {
            
        }

        public static float getDistance(Point p1, Point p2)
        {
            return (float) Math.Sqrt( Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) );
        }

        public override string ToString() => $"({X}, {Y})";
    }
}