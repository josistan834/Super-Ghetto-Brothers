using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Super_Ghetto_Brothers
{
    public partial class MainScreen : UserControl
    {
        public MainScreen()
        {
            InitializeComponent();
            plr1Button.Focus();
        }

        private void plr1Button_Click(object sender, EventArgs e)
        {
            Form1.players = 1;
            GameScreen.lives = 3;
            startGame();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void startGame()
        {
            // f is the form that this control is on - ("this" is the current User Control) 
            Form f = this.FindForm();
            f.Controls.Remove(this);
            // Create an instance of the SecondScreen 
            GameScreen gs = new GameScreen();
            // Add the User Control to the Form 
            f.Controls.Add(gs);
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {
            plr1Button.Focus();
        }
    }
}
