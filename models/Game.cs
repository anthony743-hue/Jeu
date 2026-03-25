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
        public List<GameEvent> events { get; set; }
        public void AddEvent(GamePoint p, string type, int indexPlayer)
        {
            GameEvent monEvenement = new GameEvent
            {                 
                GameID = this.GameID,                     
                PlayerID = indexPlayer,                     // ID du joueur (ex: Bleu)                 // Type d'action (ex: Tir de canon)
                PositionA = p,
                AddedDate = DateTime.Now
            };
            if( String.equals(type, "PlacePoint", StringComparaison.OrdinalIgnoreCase) ){7                monEvenement.DetailTypeID = EventType.CanonShotSuccess;
                monEvenement.DetailTypeID = EventType.PlacePoint;
            } else if( String.equals(type, "ShotSuccess", StringComparaison.OrdinalIgnoreCase) ) {
                monEvenement.DetailTypeID = EventType.CanonShotSuccess;
            } else if( String.equals(type, "ShotMiss", StringComparaison.OrdinalIgnoreCase) ){
                monEvenement.DetailTypeID = EventType.CanonShotMiss;
            } else {
                Console.WriteLine("Type d'action inconnnue");
            }
        }
        public void AddEvent(GamePoint p1, GamePoint p2, string type, int indexPlayer)
        {
            GameEvent monEvenement = new GameEvent
            {                 // Identifiant de l'événement
                GameID = this.GameID,                     // ID de la partie actuelle
                PlayerID = indexPlayer,                     // ID du joueur (ex: Bleu)                 // Type d'action (ex: Tir de canon)
                PositionA = p1,
                PositionB = p2,
                AddedDate = DateTime.Now
            };
            if( String.equals(type, "Line", StringComparaison.OrdinalIgnoreCase) ){
                monEvenement.DetailTypeID = EventType.CreateLine;
            }  else {
                Console.WriteLine("Type d'action inconnnue");
            }
        }
    }
}