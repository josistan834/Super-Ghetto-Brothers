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
    public partial class GameScreen : UserControl
    {
        //Variables
        int level = 1;
        int count, p1X, p1Y, p1Width, p1Height, gX, gY, gW, gH, p1YStored, trueX, pX, pY, pW, pH;
        public static int lives;
        bool leftArrowDown, rightArrowDown, upArrowDown, grounded, jump;

        private void KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;

            }
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;

            }
        }

        //Objects
        SolidBrush brush = new SolidBrush(Color.White);
        Image player1Image = Properties.Resources.Bros_1_png;
        Image player2Image = Properties.Resources.Bros_2_png;
        Image groundImage = Properties.Resources.GroundBrick_1_png;
        List<Ground> floorTiles = new List<Ground>();
        List<Platform> platforms = new List<Platform>();
        Rectangle p1Bot = new Rectangle();
        Rectangle p1Top = new Rectangle();

        public GameScreen()
        {
            InitializeComponent();
            lives = 3;
            start();


        }

        private void start()
        {
            this.BackColor = Color.Black;
            p1Width = 45;
            p1Height = 90;
            gW = 30;
            gH = 30;
            p1X = this.Width - 650;
            p1Y = this.Height - p1Height - gH - 100;
            gX = 0;
            gY = this.Height - 30;
            grounded = false;
            trueX = 0;
            pX = 200;
            pY = this.Height - 160;
            pW = 30;
            pH = 30;
            count = 0;
        }

        public void dead()
        {
            // f is the form that this control is on - ("this" is the current User Control) 
            Form f = this.FindForm();
            f.Controls.Remove(this);
            // Create an instance of the SecondScreen 
            GameOver go = new GameOver();
            // Add the User Control to the Form 
            f.Controls.Add(go);
        }
       
        private void GameScreen_Load(object sender, EventArgs e)
        {
            
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (leftArrowDown)
            {
                if (p1X > 100)
                {
                    p1X -= 5;
                    if (p1Width > 0)
                    {
                        p1Width *= -1;
                        p1X -= p1Width / 2;
                    }
                }
                else
                {
                    trueX--;
                    foreach (Ground b in floorTiles)
                    {
                        b.x++;
                    }
                    foreach (Platform b in platforms)
                    {
                        b.x++;
                    }
                }
               
                
            }
            if (rightArrowDown)
            {
                if (p1X < this.Width-200)
                {
                    p1X += 5;
                    if (p1Width < 0)
                    {
                        p1Width *= -1;
                        p1X -= p1Width / 2;
                    }
                }
                else
                {
                    trueX++;
                    foreach (Ground b in floorTiles)
                    {
                        b.x--;
                    }
                    foreach (Platform b in platforms)
                    {
                        b.x--;
                    }
                }

            }
            if (upArrowDown)
            {
                if (grounded)
                {
                    jump = true;
                    p1YStored = p1Y;

                }
                
                if (p1Y + 150 > p1YStored && jump)
                {
                    if (p1YStored - p1Y > 90)
                    {
                        p1Y -= 8;
                    }
                    else if (p1YStored - p1Y > 50)
                    {
                        p1Y -= 12;
                    }
                    else
                    {
                        p1Y -= 16;
                    }
                }
                else if (p1Y + 150 <= p1YStored)
                {
                    jump = false;
                }
            }

            
            if (count > 200 && this.BackColor != Color.LightCyan)
            {
                this.BackColor = Color.LightCyan;
            }
            if (floorTiles.Count < 50)
            {
                Ground newG = new Ground(gX, gY, gW, gH);
                gX += gW;
                floorTiles.Add(newG);
            }
            if (trueX == 0 && platforms.Count < 5)
            {
                Platform newP = new Platform(pX, pY, pW, pH, platforms.Count);
                pX += gW;
                platforms.Add(newP);
            }
            //Remove boxes from left but probaby wont use so going back is possible.
            //int index = floorTiles.FindIndex(b => b.x < -30);

            //if (index >= 0)

            //{
            //    //TODO - remove box if it has gone of screen
            //    floorTiles.RemoveAt(index);

            //}

            p1Bot = new Rectangle(p1X, p1Y+p1Height, 30, 1);
            p1Top = new Rectangle(p1X, p1Y, 30, 1);

            foreach (Ground b in floorTiles)

            {
                Rectangle coFloor = new Rectangle(b.x, b.y, b.width, b.height);
                if (p1Bot.IntersectsWith(coFloor))

                {
                    grounded = true;
                    break;
                }
                else
                {
                    grounded = false;    
                }
            }
            foreach (Platform p in platforms)
            {
                Rectangle coPlat = new Rectangle(p.x, p.y,p.width, p.height);
                //delete block on hit (needs fix)
                //if (p1Top.IntersectsWith(coPlat))
                //{
                //    platforms.RemoveAt(0);
                //    break;
                //}
                if (p1Bot.IntersectsWith(coPlat))

                {
                    grounded = true;
                    break;
                }
            }
            if (!grounded)
            {
                if (p1YStored - p1Y > 90)
                {
                    p1Y += 4;
                }
                else if (p1YStored - p1Y > 50)
                {
                    p1Y += 6;
                }
                else
                {
                    p1Y += 8;
                }
              
            }
            if (p1Y > this.Height)
            {
                lives--;
                dead();
            }
            count++;
            Refresh();
        }
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            if (count < 200)
            {
                e.Graphics.DrawString($"{lives}X Lives \n Level {level}", this.Font, brush, this.Width /2 - 50, this.Height / 2);
            }
            else if (count > 200)
            {
                foreach (Ground g in floorTiles)
                {
                    e.Graphics.DrawImage(groundImage, g.x, g.y, g.width, g.height);   
                }
                foreach (Platform p in platforms)
                {
                    e.Graphics.DrawImage(groundImage, p.x, p.y, p.width, p.height);
                }
                e.Graphics.DrawImage(player1Image, p1X, p1Y, p1Width, p1Height);
                
            } 
        }
    }
}
