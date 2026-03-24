using System;
using System.Windows.Forms;

namespace affichage
{
    public class ConfigForm : Form
    {
        public int SelectedWidth { get; private set; } = 15;
        public int SelectedHeight { get; private set; } = 15;

        public ConfigForm()
        {
            this.Text = "Configuration du Plateau";
            this.Size = new Size(300, 200);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lbl = new Label { Text = "Dimension (ex: 15):", Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter };
            TextBox txtDim = new TextBox { Text = "15", Dock = DockStyle.Top };
            Button btnStart = new Button { Text = "Lancer le Jeu", Dock = DockStyle.Bottom };

            btnStart.Click += (s, e) =>
            {
                if (int.TryParse(txtDim.Text, out int dim) && dim > 4)
                {
                    SelectedWidth = SelectedHeight = dim;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            this.Controls.Add(txtDim);
            this.Controls.Add(lbl);
            this.Controls.Add(btnStart);
        }
    }
}