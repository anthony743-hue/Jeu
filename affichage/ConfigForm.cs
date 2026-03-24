using System;
using System.Windows.Forms;
using System.Drawing;

namespace affichage
{
    public class ConfigForm : Form
    {
        private const int MIN_DIMENSION = 5;
        private const int MAX_DIMENSION = 50;
        private const int DEFAULT_DIMENSION = 15;

        public int SelectedWidth { get; private set; } = DEFAULT_DIMENSION;
        public int SelectedHeight { get; private set; } = DEFAULT_DIMENSION;

        private TextBox txtDimension;
        private Label lblError;
        private TrackBar trackDimension;
        private Label lblDimensionValue;

        public ConfigForm()
        {
            InitializeForm();
            CreateControls();
            ApplyStyles();
        }

        private void InitializeForm()
        {
            this.Text = "Configuration du Plateau";
            this.Size = new Size(450, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 245);
            this.Font = new Font("Segoe UI", 10);
        }

        private void CreateControls()
        {
            // Titre principal
            Label lblTitle = new Label
            {
                Text = "Configuration du Jeu",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 100),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(0, 10, 0, 0)
            };

            // Panel de configuration
            Panel panelConfig = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = Color.FromArgb(240, 240, 245)
            };

            // Label pour la dimension
            Label lblDimension = new Label
            {
                Text = "Dimension du Plateau:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 100),
                AutoSize = true,
                Location = new Point(0, 0)
            };

            // TextBox pour saisir la dimension
            txtDimension = new TextBox
            {
                Text = DEFAULT_DIMENSION.ToString(),
                Font = new Font("Segoe UI", 12),
                Width = 100,
                Height = 35,
                Location = new Point(0, 25),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtDimension.TextChanged += TxtDimension_TextChanged;

            // Label pour afficher la valeur
            lblDimensionValue = new Label
            {
                Text = $"{DEFAULT_DIMENSION} × {DEFAULT_DIMENSION}",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(100, 100, 150),
                AutoSize = true,
                Location = new Point(110, 30)
            };

            // TrackBar pour ajuster visuellement
            trackDimension = new TrackBar
            {
                Minimum = MIN_DIMENSION,
                Maximum = MAX_DIMENSION,
                Value = DEFAULT_DIMENSION,
                Width = 300,
                Location = new Point(0, 65)
            };
            trackDimension.ValueChanged += TrackDimension_ValueChanged;

            // Label pour la plage
            Label lblRange = new Label
            {
                Text = $"Minimum: {MIN_DIMENSION}  |  Maximum: {MAX_DIMENSION}",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(120, 120, 150),
                AutoSize = true,
                Location = new Point(0, 95)
            };

            // Label d'erreur
            lblError = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Red,
                AutoSize = true,
                Location = new Point(0, 130),
                MaximumSize = new Size(400, 50)
            };

            // Panel des boutons
            Panel panelButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(30, 10, 30, 20),
                BackColor = Color.FromArgb(230, 230, 240)
            };

            Button btnStart = new Button
            {
                Text = "▶ Lancer le Jeu",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Width = 150,
                Height = 40,
                Location = new Point(110, 10),
                BackColor = Color.FromArgb(100, 150, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.MouseEnter += (s, e) => btnStart.BackColor = Color.FromArgb(70, 120, 180);
            btnStart.MouseLeave += (s, e) => btnStart.BackColor = Color.FromArgb(100, 150, 200);
            btnStart.Click += BtnStart_Click;

            Button btnCancel = new Button
            {
                Text = "Annuler",
                Font = new Font("Segoe UI", 11),
                Width = 100,
                Height = 40,
                Location = new Point(270, 10),
                BackColor = Color.FromArgb(180, 180, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = Color.FromArgb(150, 150, 150);
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = Color.FromArgb(180, 180, 180);
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Ajout des contrôles
            panelConfig.Controls.Add(lblTitle);
            panelConfig.Controls.Add(lblDimension);
            panelConfig.Controls.Add(txtDimension);
            panelConfig.Controls.Add(lblDimensionValue);
            panelConfig.Controls.Add(trackDimension);
            panelConfig.Controls.Add(lblRange);
            panelConfig.Controls.Add(lblError);

            panelButtons.Controls.Add(btnStart);
            panelButtons.Controls.Add(btnCancel);

            this.Controls.Add(panelConfig);
            this.Controls.Add(panelButtons);
        }

        private void TxtDimension_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtDimension.Text, out int dim))
            {
                if (dim >= MIN_DIMENSION && dim <= MAX_DIMENSION)
                {
                    trackDimension.Value = dim;
                    lblDimensionValue.Text = $"{dim} × {dim}";
                    lblError.Text = "";
                }
                else
                {
                    lblError.Text = $"⚠ La dimension doit être entre {MIN_DIMENSION} et {MAX_DIMENSION}";
                }
            }
            else if (!string.IsNullOrEmpty(txtDimension.Text))
            {
                lblError.Text = "⚠ Veuillez entrer un nombre valide";
            }
        }

        private void TrackDimension_ValueChanged(object sender, EventArgs e)
        {
            txtDimension.TextChanged -= TxtDimension_TextChanged;
            txtDimension.Text = trackDimension.Value.ToString();
            txtDimension.TextChanged += TxtDimension_TextChanged;
            lblDimensionValue.Text = $"{trackDimension.Value} × {trackDimension.Value}";
            lblError.Text = "";
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtDimension.Text, out int dim) && dim >= MIN_DIMENSION && dim <= MAX_DIMENSION)
            {
                SelectedWidth = SelectedHeight = dim;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblError.Text = $"⚠ Veuillez entrer une dimension valide ({MIN_DIMENSION}-{MAX_DIMENSION})";
            }
        }

        private void ApplyStyles()
        {
            // Style cohérent pour tous les TextBox
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox tb)
                    tb.BackColor = Color.White;
            }
        }
    }
}