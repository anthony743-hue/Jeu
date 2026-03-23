using entity;

namespace models
{
    public class Game
    {
        public int GameID { get; set; }
        public string GameName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Notes { get; set; }

        public GameConfig config { get; set; }
    }
}