using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game_forest_Match3
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            GameScreen form = new GameScreen();
            form.FormClosed += delegate { this.Show(); };
            form.Show();
            Hide();
        }

        private void MainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
