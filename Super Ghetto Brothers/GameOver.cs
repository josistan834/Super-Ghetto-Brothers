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
    public partial class GameOver : UserControl
    {
        public GameOver()
        {
            InitializeComponent();
           
        }

        private void GameOver_Load(object sender, EventArgs e)
        {
            if (GameScreen.lives > 0)
            {
                // f is the form that this control is on - ("this" is the current User Control) 
                Form f = this.FindForm();
                f.Controls.Remove(this);
                // Create an instance of the SecondScreen 
                GameScreen gs = new GameScreen();
                // Add the User Control to the Form 
                f.Controls.Add(gs);
            }
            else
            {
                continueButton.Show();
                exitButton.Show();
                label1.Show();
                continueButton.Focus();
            }
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            // f is the form that this control is on - ("this" is the current User Control) 
            Form f = this.FindForm();
            f.Controls.Remove(this);
            // Create an instance of the SecondScreen 
            MainScreen gs = new MainScreen();
            // Add the User Control to the Form 
            f.Controls.Add(gs);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
