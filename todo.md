# 🎮 Projet : Jeu du Morpion Canon

## 📚 Phase 1 : Apprentissage Fondamental

### Concepts à maîtriser
- [ ] **C#** : syntaxe, POO, événements
- [ ] **WinForms** : formulaires, contrôles, événements souris
- [ ] **PostgreSQL** : requêtes, relations, gestion de base de données
- [ ] **Connexion C# ↔ PostgreSQL** : ADO.NET ou Entity Framework

---

## 🏗️ Phase 2 : Architecture du Projet

### Structure des données

```
📁 Projet/
├── 📁 Models/
│   ├── Joueur.cs
│   └── Point.cs
├── 📁 DAO/
│   ├── JoueurDAO.cs
│   └── PartieDAO.cs
├── 📁 UI/
│   └── GameForm.cs
└── 📁 Database/
    └── init_db.sql
```

### Entités principales

#### **Classe Joueur**
```csharp
public class Joueur
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public int Score { get; set; }
    public int Symbole { get; set; } // 1 = X, 2 = O
}
```

#### **Classe Point**
```csharp
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
    public int JoueurId { get; set; }
}
```

#### **Classe Game**
```csharp
public class Game
{
    public List<Point> Points { get; set; }
    public Joueur Joueur1 { get; set; }
    public Joueur Joueur2 { get; set; }
    public Joueur JoueurCourant { get; set; }
    
    public bool Placer(int x, int y) { /* ... */ }
    public bool VerifierVictoire(Point dernierPoint) { /* ... */ }
}
```

---

## 🎯 Phase 3 : Logique du Jeu

### 1️⃣ Placement du Point

#### Option A : Placement à la souris
- **Timer** : 15-30 secondes maximum
- **Curseur** : position de la souris = position du point (grille alignée)
- **Validation** : clic pour confirmer le placement

#### Option B : Placement aux boutons
- **Flèches clavier** : déplacement du curseur
- **Entrée** : confirmer

---

### 2️⃣ Tir du Canon (Inspiré de Clash of Clans)

#### Paramètres de tir

| Paramètre | Plage | Description |
|-----------|-------|-------------|
| **Puissance** | 1-9 | Affecte la distance et l'arc de la trajectoire |
| **Angle** | 0-180° | Contrôlé par flèches ↑↓ ou souris |
| **Animation** | Canon → Parabole | Visualiser la trajectoire avant tir |

#### Mécanique

```
┌─────────────────────────────────┐
│                                 │
│     Canon (joueur 1)            │
│        🔫  →                    │
│                                 │
│  ⊕ ⊕ ⊕  (équipe adverse)        │
│  ⊕ ⊕ ⊕                          │
│  ⊕ ⊕ ⊕                          │
└─────────────────────────────────┘

Projectile touche → Point supprimé (list.RemoveAt(index))
```

---

### 3️⃣ Vérification de Victoire

**Condition** : ≥ 5 points alignés (horizontal, vertical ou diagonal)

#### Algorithme d'optimisation

```csharp
public bool VerifierVictoire(Point dernierPoint)
{
    // Vérifier seulement autour du dernier point placé
    // 4 directions : →, ↓, ↘, ↗
    
    int[][] directions = new int[][]
    {
        new int[] { 1, 0 },   // Horizontal
        new int[] { 0, 1 },   // Vertical
        new int[] { 1, 1 },   // Diagonale ↘
        new int[] { 1, -1 }   // Diagonale ↗
    };
    
    foreach (var dir in directions)
    {
        int compte = 1; // Compte le point actuel
        
        // Compte dans la direction positive
        for (int i = 1; i <= 4; i++)
        {
            Point p = new Point 
            { 
                X = dernierPoint.X + dir[0] * i,
                Y = dernierPoint.Y + dir[1] * i
            };
            if (EstValide(p) && EstDuJoueur(p))
                compte++;
            else
                break;
        }
        
        // Compte dans la direction négative
        for (int i = 1; i <= 4; i++)
        {
            Point p = new Point 
            { 
                X = dernierPoint.X - dir[0] * i,
                Y = dernierPoint.Y - dir[1] * i
            };
            if (EstValide(p) && EstDuJoueur(p))
                compte++;
            else
                break;
        }
        
        if (compte >= 5)
            return true;
    }
    
    return false;
}
```

---

## 💡 Astuces Markdown Bonus

### 1. **Listes à cocher**
```markdown
- [ ] Tâche non complétée
- [x] Tâche complétée
```

### 2. **Tableaux**
```markdown
| Col 1 | Col 2 | Col 3 |
|-------|-------|-------|
| Données | Données | Données |
```

### 3. **Bloc de code avec langage**
````markdown
```csharp
// Votre code C#
```

```sql
-- Votre requête SQL
```
````

### 4. **Emphase imbriquée**
- `***Gras et italique***` = ***Gras et italique***
- `~~Barré~~` = ~~Barré~~

### 5. **Citations (blockquote)**
```markdown
> Citation importante
> 
> Plusieurs lignes
```

### 6. **Listes imbriquées (bon format)**
```markdown
- Item 1
  - Sous-item 1.1
    - Sous-sous-item
  - Sous-item 1.2
- Item 2
```

### 7. **Liens et références**
```markdown
[Texte du lien](https://exemple.com)
[Lien vers section](#astuces-markdown-bonus)
```

### 8. **Emojis pour structurer**
```markdown
# 🎮 Titre principal
## 📚 Apprentissage
### ⚙️ Configuration
- ✅ Complété
- ⚠️ En cours
- ❌ Non fait
```

### 9. **Ligne horizontale**
```markdown
---
***
___
```

### 10. **Bloc d'alerte (sintaxe GitHub)**
```markdown
> [!NOTE]
> Information importante

> [!WARNING]
> Attention !

> [!TIP]
> Conseil utile
```

---

## 📋 Checklist du Projet

- [ ] Phase 1 : Apprendre C# et WinForms
- [ ] Phase 2 : Créer la structure de la BDD
- [ ] Phase 2 : Implémenter les modèles
- [ ] Phase 2 : Créer les DAO
- [ ] Phase 3 : Interface de jeu (formulaire principal)
- [ ] Phase 3 : Système de placement
- [ ] Phase 3 : Animation du canon
- [ ] Phase 3 : Logique de collision
- [ ] Phase 3 : Vérification de victoire
- [ ] Phase 4 : Peaufinage et tests

---

## 🚀 Prochaines Étapes

1. Installer **Visual Studio Community**
2. Créer un **nouveau projet WinForms**
3. Configurer la **connexion PostgreSQL**
4. Créer les **scripts SQL** initiaux
5. Commencer par les **classes métier** (Models)

Bonne chance ! 🎮✨