using System;
using System.Drawing;
using System.Windows.Forms;
using models;
using utils;
using entity;

using GdiPoint = System.Drawing.Point; 
using GamePoint = utils.Point;

namespace JeuDePoints
{
    public class GameForm : Form
    {
        private GameEngine engine;
        private Panel gamePanel;
        private Label lblStatus;
        private int cellSize = 30, canonRow = 0;

        public GameForm() {
            this.Text = "Points & Canon - Final";
            this.Size = new Size(1000, 850);
            this.DoubleBuffered = true;
            InitGame();
        }

        private void InitGame() {
            Player[] ps = { new Player("Bleu"), new Player("Rouge") };
            engine = new GameEngine(ps, 15, 15);
            
            lblStatus = new Label { Dock = DockStyle.Top, Height = 40, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Arial", 12, FontStyle.Bold), BackColor = Color.Gainsboro };
            gamePanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            
            gamePanel.Paint += (s, e) => Render(e.Graphics);
            gamePanel.MouseDoubleClick += (s, e) => {
                int x = (e.X - 40 + cellSize / 2) / cellSize, y = (e.Y - 40 + cellSize / 2) / cellSize;
                if (engine.isSecure(x, y) && engine.matrix[y][x] == -1) {
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

        private void OnKeyDown(object s, KeyEventArgs e) {
            if (e.KeyCode == Keys.Up && canonRow > 0) canonRow--;
            if (e.KeyCode == Keys.Down && canonRow < engine.DEFAULT_HEIGTH - 1) canonRow++;
            
            if (e.Control) {
                int pwr = -1;
                if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9) pwr = e.KeyCode - Keys.D0;
                else if (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad9) pwr = e.KeyCode - Keys.NumPad1 + 1;

                if (pwr != -1) { engine.fireCanon(canonRow, pwr); engine.nextPlayer(); UpdateStatus(); }
            }
            gamePanel.Invalidate();
        }

        private void UpdateStatus() {
            lblStatus.Text = $"Tour: {engine.joueurs[engine.indexPlayer].PlayerName} | Score B: {engine.scores[0]} R: {engine.scores[1]}";
        }

        private void Render(Graphics g) {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int offset = 40;

            // Grille
            Pen pGrid = new Pen(Color.Gainsboro, 1);
            for (int i = 0; i < engine.DEFAULT_WIDTH; i++) g.DrawLine(pGrid, offset + i * cellSize, offset, offset + i * cellSize, offset + (engine.DEFAULT_HEIGTH-1)*cellSize);
            for (int j = 0; j < engine.DEFAULT_HEIGTH; j++) g.DrawLine(pGrid, offset, offset + j * cellSize, offset + (engine.DEFAULT_WIDTH-1)*cellSize, offset + j * cellSize);

            // Lignes de score
            foreach (var line in engine.ActiveLines) {
                Pen pLine = new Pen((line.Item3 == 0) ? Color.Blue : Color.Red, 3);
                g.DrawLine(pLine, offset + line.Item1.X*cellSize, offset + line.Item1.Y*cellSize, offset + line.Item2.X*cellSize, offset + line.Item2.Y*cellSize);
            }

            // Points
            for (int y = 0; y < engine.DEFAULT_HEIGTH; y++) {
                for (int x = 0; x < engine.DEFAULT_WIDTH; x++) {
                    if (engine.matrix[y][x] != -1) {
                        Brush br = (engine.matrix[y][x] == 0) ? Brushes.Blue : Brushes.Red;
                        if (engine.colored[y][x]) g.DrawEllipse(new Pen(Color.Gold, 2), offset + x*cellSize - 9, offset + y*cellSize - 9, 18, 18);
                        g.FillEllipse(br, offset + x*cellSize - 7, offset + y*cellSize - 7, 14, 14);
                    }
                }
            }
            g.FillRectangle(Brushes.Black, 5, offset + canonRow * cellSize - 8, 20, 16);
        }

        [STAThread] public static void Main() { Application.EnableVisualStyles(); Application.Run(new GameForm()); }
    }
}