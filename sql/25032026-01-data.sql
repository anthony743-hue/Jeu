-- Insertion du placement de point
INSERT INTO GameEventType (GameEventTypeID, GameEventName, Description)
VALUES (1, 'PlacePoint', 'Placement standard d''un point sur la grille');

-- Insertion du tir réussi
INSERT INTO GameEventType (GameEventTypeID, GameEventName, Description)
VALUES (2, 'CanonShotSuccess', 'Tir de canon ayant supprimé un point adverse');

-- Insertion du tir manqué
INSERT INTO GameEventType (GameEventTypeID, GameEventName, Description)
VALUES (3, 'CanonShotMiss', 'Tir de canon n''ayant touché aucune cible');

-- Insertion de la création de ligne
INSERT INTO GameEventType (GameEventTypeID, GameEventName, Description)
VALUES (4, 'CreateLine', 'Formation d''une ligne reliant deux points (score)');