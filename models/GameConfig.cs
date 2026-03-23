namespace models
{
    public class GameConfig
    {
        public int GameConfigID { get; set; }
        public int GameID { get; set; }
        public int BoardDimensionM { get; set; }
        public int TimePerPoint { get; set; }
        public int TimePerTurn { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}