using System;
using utils;

namespace models
{
    public class GameEvent
    {
        public int GameEventsID { get; set; }
        public int GameID { get; set; }
        public int PlayerID { get; set; }
        public int GameEventTypeID { get; set; }
        public Point PositionA { get; set; }
        public Point PositionB { get; set; }
        public DateTime AddedDate { get; set; } 
    }
}