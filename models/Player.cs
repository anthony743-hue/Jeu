using System;
using System.Collections.Generic;
using utils;

namespace models
{
    public class Player
    {
        public Player()
        {
            listePoints = new List<Point>();
        }
        public int PlayerID { get; set; }
        public string PlayerName { get; set; }
        public DateTime RegistrationDate { get; set; }
        private List<Point> listePoints { get; set; }
        public void addPoint(int x, int y)
        {
            listePoints.Add(new Point(x, y));
        }
        public void addPoint(Point p)
        {
            listePoints.Add(p);
        }
    }
}