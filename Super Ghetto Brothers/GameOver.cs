using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.SymbolStore;

namespace Super_Ghetto_Brothers
{
    public partial class GameOver : UserControl
    {
        public GameOver()
        {
            InitializeComponent();
           
        }

        //On load
        private void GameOver_Load(object sender, EventArgs e)
        {
            //If you have lives
            if (GameScreen.lives > 0)
            {
                //Lose life
                if (!GameScreen.WIN)
                {
                    // f is the form that this control is on - ("this" is the current User Control) 
                    Form f = this.FindForm();
                    f.Controls.Remove(this);
                    // Create an instance of the SecondScreen 
                    GameScreen gs = new GameScreen();
                    // Add the User Control to the Form 
                    f.Controls.Add(gs);
                    gs.Location = new Point((f.Width - gs.Width) / 2, (f.Height - gs.Height) / 2);
                }
                //Winning screen show
                else
                {
                    continueButton.Show();
                    exitButton.Show();
                    label1.ForeColor = Color.LimeGreen;
                    label1.Text = "YOU WIN!!!!";
                    label1.Show();
                    continueButton.Focus();
                }
               
            }

            //When you have no lives show gameover
            else
            {
                label1.ForeColor = Color.Red;
                label1.Text = "Game Over...";
                continueButton.Show();
                exitButton.Show();
                label1.Show();
                continueButton.Focus();
            }
        }

        //Return to the main screen
        private void continueButton_Click(object sender, EventArgs e)
        {
            // f is the form that this control is on - ("this" is the current User Control) 
            Form f = this.FindForm();
            f.Controls.Remove(this);
            // Create an instance of the SecondScreen 
            MainScreen ms = new MainScreen();
            // Add the User Control to the Form 
            f.Controls.Add(ms);
            ms.Location = new Point((f.Width - ms.Width) / 2, (f.Height - ms.Height) / 2);
        }

        //End the program on exic click
        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
