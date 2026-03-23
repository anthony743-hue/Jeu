namespace utils
{
    class MethodUtils
    {
        public static int[] getCoords(int x1, int y1, int x2, int y2)
        {
            int[] coords = new int[2];
            int a = (y2 - y1) / (x2 - x1);
            int b = y1 - a * x1;
            return new int[] { a, b };
        }

        public static bool AreSegmentsIntersecting(Point p1, Point p2, Point p3, Point p4)
        {
            // Calcul des orientations nécessaires
            int o1 = GetOrientation(p1, p2, p3);
            int o2 = GetOrientation(p1, p2, p4);
            int o3 = GetOrientation(p3, p4, p1);
            int o4 = GetOrientation(p3, p4, p2);

            if (o1 != o2 && o3 != o4)
                return true;
            return false;
        }

        private static int GetOrientation(Point p, Point q, Point r)
        {
            int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);

            if (val == 0) return 0;     // Colinéaires
            return (val > 0) ? 1 : 2;  // 1: Horaire, 2: Anti-horaire
        }
    }
}