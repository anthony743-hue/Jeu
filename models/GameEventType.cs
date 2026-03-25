using System;

namespace models
{
    public class GameEventType
    {
        public int DetailTypeID { get; set; }
        public string DetailTypeName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public enum EventType
    {
        PlacePoint = 1,        // Placement d'un point standard
        CanonShotSuccess = 2,  // Tir de canon ayant touché une cible
        CanonShotMiss = 3,     // Tir de canon dans le vide (optionnel)
        CreateLine = 4         // Formation d'une ligne de score (optionnel)
    }
}