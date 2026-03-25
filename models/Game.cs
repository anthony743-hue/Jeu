using entity;
using System;
using System.Drawing;
using GamePoint = utils.Point

namespace models
{
    public class Game
    {
        public int GameID { get; set; }
        public string GameName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Notes { get; set; }
        public GameConfig config { get; set; }
        public List<GameEvent> events { get; set; }
        public void AddEvent(GamePoint p, int type)
        {
            GameEvent monEvenement = new GameEvent
            {
                GameEventsID = 1,                 // Identifiant de l'événement
                GameID = this.GameID,                     // ID de la partie actuelle
                PlayerID = 0,                     // ID du joueur (ex: Bleu)                 // Type d'action (ex: Tir de canon)
                PositionA = p,
                AddedDate = DateTime.Now
            };
        }
        public void AddEvent(Point p1, Point p2)
        {

        }
    }
}