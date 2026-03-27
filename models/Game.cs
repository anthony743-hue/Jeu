using entity;
using System;
using System.Drawing;
using GamePoint = utils.Point;
using System.Collections.Generic;

namespace models
{
    public class Game
    {
        public int GameID { get; set; }
        public string GameName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Notes { get; set; }
        public GameConfig config { get; set; }
        public List<GameEvent> events { get; private set; }
        public Game(){
            events = new List<GameEvent>();
        }
        public void AddEvent(GamePoint p, string type, int indexPlayer)
        {
            GameEvent monEvenement = new GameEvent
            {                 
                GameID = this.GameID,                     
                PlayerID = indexPlayer,                     // ID du joueur (ex: Bleu)                 // Type d'action (ex: Tir de canon)
                PositionA = p,
                AddedDate = DateTime.Now
            };
            if( string.Equals(type, "PlacePoint", StringComparison.OrdinalIgnoreCase) ){               
                monEvenement.GameEventTypeID = (int)EventType.PlacePoint;
            } else if( string.Equals(type, "ShotSuccess", StringComparison.OrdinalIgnoreCase) ) {
                monEvenement.GameEventTypeID = (int)EventType.CanonShotSuccess;
            } else if( string.Equals(type, "ShotMiss", StringComparison.OrdinalIgnoreCase) ){
                monEvenement.GameEventTypeID = (int)EventType.CanonShotMiss;
            } else {
                Console.WriteLine("Type d'action inconnnue");
            }
            events.Add(monEvenement);
        }
        public void AddEvent(GamePoint p1, GamePoint p2, int indexPlayer)
        {
            GameEvent monEvenement = new GameEvent
            {                 // Identifiant de l'événement
                GameID = this.GameID,                     // ID de la partie actuelle
                PlayerID = indexPlayer,                     // ID du joueur (ex: Bleu)                 // Type d'action (ex: Tir de canon)
                PositionA = p1,
                PositionB = p2,
                GameEventTypeID = (int) EventType.CreateLine,
                AddedDate = DateTime.Now
            };
            events.Add(monEvenement);
        }
    }
}