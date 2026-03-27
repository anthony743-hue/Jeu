SELECT 

FROM Game g
JOIN GameEvent ge ON ge.gameid = g.gameid
JOIN GameEventType gte ON ge.GameEventTypeID = gte.GameEventTypeID;


Retour ->
    PlacePoint -> Revenir en arriere l'efface
        |
        V
    LigneSeTrace -> 
        Revenir en arriere efface le point qui a permis cela    
        False Colored et Supprime le point dans ActivesLines

    shotSuccess ->
        Efface le point atteint

    shotMiss ->
        {```}