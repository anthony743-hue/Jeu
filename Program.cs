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

        private int laserX = -1; 
        private int laserY = -1; 
        private Timer laserTimer; 
        private int laserSide = 1; 
        private Label lblStatus;
        private int cellSize = 30;

        // Constructeur acceptant les dimensions choisies
        public GameForm(int width, int height)
        {
            this.Text = "Points & Canon - Final";
            this.Size = new Size(1000, 850);
            this.DoubleBuffered = true;
            
            laserTimer = new Timer { Interval = 200 };
            laserTimer.Tick += (s, e) => { 
                laserX = -1; 
                gamePanel.Invalidate(); 
                laserTimer.Stop(); 
            };

            InitGame(width, height);
        }

        private void InitGame(int w, int h)
        {
            Player[] ps = { new Player("Bleu"), new Player("Rouge") };
            engine = new GameEngine(ps, w, h);

            lblStatus = new Label { Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold), BackColor = Color.Gainsboro };
            gamePanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            gamePanel.Paint += (s, e) => Render(e.Graphics);
            
            gamePanel.MouseDoubleClick += (s, e) =>
            {
                // Calcul des offsets dynamiques pour le clic
                int offsetX = (gamePanel.Width - (engine.DEFAULT_WIDTH - 1) * cellSize) / 2;
                int offsetY = (gamePanel.Height - (engine.DEFAULT_HEIGTH - 1) * cellSize) / 2;

                int x = (int)Math.Round((float)(e.X - offsetX) / cellSize);
                int y = (int)Math.Round((float)(e.Y - offsetY) / cellSize);

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

            if (e.KeyCode == Keys.Up && engine.canonRows[currentPlayer] > 0)
                engine.canonRows[currentPlayer]--;
            if (e.KeyCode == Keys.Down && engine.canonRows[currentPlayer] < engine.DEFAULT_HEIGTH - 1)
                engine.canonRows[currentPlayer]++;

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

            // CALCUL DU CENTRAGE
            int gridWidthPx = (engine.DEFAULT_WIDTH - 1) * cellSize;
            int gridHeightPx = (engine.DEFAULT_HEIGTH - 1) * cellSize;
            int offsetX = (gamePanel.Width - gridWidthPx) / 2;
            int offsetY = (gamePanel.Height - gridHeightPx) / 2;

            // Grille
            Pen pGrid = new Pen(Color.Gainsboro, 1);
            for (int i = 0; i < engine.DEFAULT_WIDTH; i++) 
                g.DrawLine(pGrid, offsetX + i * cellSize, offsetY, offsetX + i * cellSize, offsetY + gridHeightPx);
            for (int j = 0; j < engine.DEFAULT_HEIGTH; j++) 
                g.DrawLine(pGrid, offsetX, offsetY + j * cellSize, offsetX + gridWidthPx, offsetY + j * cellSize);

            // Lignes de score
            foreach (KeyValuePair<int, List<Tuple<GamePoint, GamePoint>>> entree in engine.ActivesLines)
            {
                foreach (var line in entree.Value)
                {
                    Pen pLine = new Pen((entree.Key == 0) ? Color.Blue : Color.Red, 3);
                    g.DrawLine(pLine, offsetX + line.Item1.X * cellSize, offsetY + line.Item1.Y * cellSize, offsetX + line.Item2.X * cellSize, offsetY + line.Item2.Y * cellSize);
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
                        if (engine.colored[y][x]) 
                            g.DrawEllipse(new Pen(Color.Gold, 2), offsetX + x * cellSize - 9, offsetY + y * cellSize - 9, 18, 18);
                        g.FillEllipse(br, offsetX + x * cellSize - 7, offsetY + y * cellSize - 7, 14, 14);
                    }
                }
            }

            // Canons
            int gridRight = offsetX + gridWidthPx;
            Brush brushBleu = (engine.indexPlayer == 0) ? Brushes.Blue : Brushes.LightSkyBlue;
            g.FillRectangle(brushBleu, offsetX - 30, offsetY + engine.canonRows[0] * cellSize - 8, 20, 16);

            Brush brushRouge = (engine.indexPlayer == 1) ? Brushes.Red : Brushes.LightPink;
            g.FillRectangle(brushRouge, gridRight + 10, offsetY + engine.canonRows[1] * cellSize - 8, 20, 16);

            // Laser
            if (laserX != -1)
            {
                Pen laserPen = new Pen(Color.OrangeRed, 4);
                int startX = (laserSide == 1) ? offsetX - 15 : gridRight + 15;
                int endX = offsetX + laserX * cellSize;
                int y = offsetY + laserY * cellSize;

                g.DrawLine(laserPen, startX, y, endX, y);
            }
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            
            // Lancement de la config avant le jeu
            affichage.ConfigForm config = new affichage.ConfigForm();
            if (config.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new GameForm(config.SelectedWidth, config.SelectedHeight));
            }
        }
    }
}