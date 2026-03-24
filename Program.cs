using System;
using System.Drawing;
using System.Windows.Forms;
using models;
using utils;
using entity;
using System.Collections.Generic;

using GdiPoint = System.Drawing.Point;
using GamePoint = utils.Point;

namespace JeuDePoints
{
    public class GameForm : Form
    {
        private GameEngine engine;
        private Panel gamePanel;

        private int laserX = -1; // Coordonnée X de l'impact (-1 = pas de laser)
        private int laserY = -1; // Coordonnée Y (la ligne du canon)
        private Timer laserTimer; // Le minuteur pour effacer le laser
        private int laserSide = 1; // 1 = Gauche (Joueur Bleu), -1 = Droite (Joueur Rouge)
        private Label lblStatus;
        private int cellSize = 30, canonRow = 0;

        public GameForm()
        {
            this.Text = "Points & Canon - Final";
            this.Size = new Size(1000, 850);
            this.DoubleBuffered = true;
            laserTimer = new Timer { Interval = 200 }; // SANS CECI -> CRASH
            laserTimer.Tick += (s, e) => { /* ... */ };
            InitGame();
        }

        private void InitGame()
        {
            Player[] ps = { new Player("Bleu"), new Player("Rouge") };
            engine = new GameEngine(ps, 15, 15);

            lblStatus = new Label { Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold), BackColor = Color.Gainsboro };
            gamePanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            gamePanel.Paint += (s, e) => Render(e.Graphics);
            gamePanel.MouseDoubleClick += (s, e) =>
            {
                int x = (e.X - 40 + cellSize / 2) / cellSize, y = (e.Y - 40 + cellSize / 2) / cellSize;
                if (engine.isSecure(x, y) && engine.matrix[y][x] == -1)
                {
                    GamePoint p = new GamePoint(x, y);
                    engine.addPoint(p);
                    engine.ifScoring(p);
                    engine.nextPlayer();
                    UpdateStatus();
                    gamePanel.Invalidate();
                }
            };

            this.KeyPreview = true;
            this.KeyDown += OnKeyDown;
            this.Controls.Add(gamePanel);
            this.Controls.Add(lblStatus);
            UpdateStatus();
        }

        private void OnKeyDown(object s, KeyEventArgs e)
        {
            int currentPlayer = engine.indexPlayer;

            // Déplacement du canon du joueur actuel
            if (e.KeyCode == Keys.Up && engine.canonRows[currentPlayer] > 0)
                engine.canonRows[currentPlayer]--;
            if (e.KeyCode == Keys.Down && engine.canonRows[currentPlayer] < engine.DEFAULT_HEIGTH - 1)
                engine.canonRows[currentPlayer]++;

            // Tir (uniquement avec Ctrl + Chiffre)
            if (e.Control)
            {
                int pwr = -1;
                if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9) pwr = e.KeyCode - Keys.D0;
                else if (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad9) pwr = e.KeyCode - Keys.NumPad1 + 1;

                if (pwr != -1)
                {
                    laserSide = (currentPlayer == 0) ? 1 : -1;
                    laserX = engine.fireCanon(pwr);

                    laserY = engine.canonRows[currentPlayer];
                    laserTimer.Start();
                    engine.nextPlayer();
                    UpdateStatus();
                }
            }
            gamePanel.Invalidate();
        }

        private void UpdateStatus()
        {
            lblStatus.Text = $"Tour: {engine.joueurs[engine.indexPlayer].PlayerName} | Score B: {engine.scores[0]} R: {engine.scores[1]}";
        }

        private void Render(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int offset = 40;

            // Grille
            Pen pGrid = new Pen(Color.Gainsboro, 1);
            for (int i = 0; i < engine.DEFAULT_WIDTH; i++) g.DrawLine(pGrid, offset + i * cellSize, offset, offset + i * cellSize, offset + (engine.DEFAULT_HEIGTH - 1) * cellSize);
            for (int j = 0; j < engine.DEFAULT_HEIGTH; j++) g.DrawLine(pGrid, offset, offset + j * cellSize, offset + (engine.DEFAULT_WIDTH - 1) * cellSize, offset + j * cellSize);

            // Lignes de score
            foreach (KeyValuePair<int, List<Tuple<GamePoint, GamePoint>>> entree in engine.ActivesLines)
            {
                foreach (var line in entree.Value)
                {
                    Pen pLine = new Pen((entree.Key == 0) ? Color.Blue : Color.Red, 3);
                    g.DrawLine(pLine, offset + line.Item1.X * cellSize, offset + line.Item1.Y * cellSize, offset + line.Item2.X * cellSize, offset + line.Item2.Y * cellSize);
                }
            }

            // Points
            for (int y = 0; y < engine.DEFAULT_HEIGTH; y++)
            {
                for (int x = 0; x < engine.DEFAULT_WIDTH; x++)
                {
                    if (engine.matrix[y][x] != -1)
                    {
                        Brush br = (engine.matrix[y][x] == 0) ? Brushes.Blue : Brushes.Red;
                        if (engine.colored[y][x]) g.DrawEllipse(new Pen(Color.Gold, 2), offset + x * cellSize - 9, offset + y * cellSize - 9, 18, 18);
                        g.FillEllipse(br, offset + x * cellSize - 7, offset + y * cellSize - 7, 14, 14);
                    }
                }
            }
            int gridRight = offset + (engine.DEFAULT_WIDTH - 1) * cellSize;
            Brush brushBleu = (engine.indexPlayer == 0) ? Brushes.Blue : Brushes.LightSkyBlue;
            g.FillRectangle(brushBleu, 5, offset + engine.canonRows[0] * cellSize - 8, 20, 16);

            Brush brushRouge = (engine.indexPlayer == 1) ? Brushes.Red : Brushes.LightPink;
            g.FillRectangle(brushRouge, gridRight + 15, offset + engine.canonRows[1] * cellSize - 8, 20, 16);
            if (laserX != -1)
            {
                Pen laserPen = new Pen(Color.OrangeRed, 4);
                int startX = (laserSide == 1) ? 25 : (offset + (engine.DEFAULT_WIDTH - 1) * cellSize + 15);

                int endX = offset + laserX * cellSize;
                int y = offset + laserY * cellSize;

                g.DrawLine(laserPen, startX, y, endX, y);
                laserX = -1;
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new GameForm());
        }
    }
}