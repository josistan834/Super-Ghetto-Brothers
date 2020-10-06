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

        //On start click set the initial global variables then start
        private void plr1Button_Click(object sender, EventArgs e)
        {
            Form1.players = 1;
            GameScreen.lives = 3;
            GameScreen.coins = GameScreen.oldCoins = 0;
            startGame();
        }

        //On exit click close the program
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //On start click switch to game screen
        public void startGame()
        {
            // f is the form that this control is on - ("this" is the current User Control) 
            Form f = this.FindForm();
            f.Controls.Remove(this);
            // Create an instance of the GameScreen 
            GameScreen gs = new GameScreen();
            // Add the User Control to the Form 
            f.Controls.Add(gs);
            gs.Location = new Point((f.Width - gs.Width) / 2, (f.Height - gs.Height) / 2);
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {
            plr1Button.Focus();
        }
    }
}
