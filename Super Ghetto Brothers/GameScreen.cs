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
        #region variables
        int level = 1;
        int count, p1X, p1Y, p1Width, p1Height, gX, gY, gW, gH, p1YStored, pX, pY, pW, pH;
        int baX, baY, baWidth, baHeight, baSpawned;
        public static int lives, trueX;
        bool leftArrowDown, rightArrowDown, upArrowDown, grounded, jump, right, left;
        bool baDead, baDir;
        #endregion

        #region userInput
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
        #endregion

        #region Objects
        SolidBrush brush = new SolidBrush(Color.White);
        Pen pen = new Pen(Color.White);
        Image player1Image = Properties.Resources.Bros_1_png;
        Image player2Image = Properties.Resources.Bros_2_png;
        Image groundImage = Properties.Resources.GroundBrick_1_png;
        Image goonImage = Properties.Resources.Goonba_1_png;
        List<Ground> floorTiles = new List<Ground>();
        List<Platform> platforms = new List<Platform>();
        List<Goonba> goons = new List<Goonba>();
        Rectangle p1Bot = new Rectangle();
        Rectangle p1Top = new Rectangle();
        Rectangle p1L = new Rectangle();
        Rectangle p1R = new Rectangle();
        #endregion 
        public GameScreen() //lading user control
        {
            InitializeComponent();
            lives = 3;
        }

        public void dead() //when you die
        {
            count = 0;
            gameTimer.Stop();
            // f is the form that this control is on - ("this" is the current User Control) 
            Form f = this.FindForm();
            f.Controls.Remove(this);
            // Create an instance of the SecondScreen 
            GameOver go = new GameOver();
            // Add the User Control to the Form 
            f.Controls.Add(go);
        }
       
        private void GameScreen_Load(object sender, EventArgs e) //loading game
        {
            this.BackColor = Color.Black;
            baDir = true; //true = right
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
            baX = 200;
            baY = this.Height - 70;
            baWidth = 40;
            baHeight = 40;
            baDead = false;
            baSpawned = 0;
            gameTimer.Start();
        }

        private void gameTimer_Tick(object sender, EventArgs e) //The game loop
        {
            #region Movement
            if (leftArrowDown)
            {
                if (left)
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
                        foreach (Goonba g in goons)
                        {
                            g.RX++;
                            g.LX++;
                        }

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
               
                
            }
            if (rightArrowDown)
            {
                if (right)
                {


                    if (p1X < this.Width - 200)
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
                        foreach (Goonba g in goons)
                        {
                            g.RX--;
                            g.LX--;
                        }
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
            left = right = true;
            #endregion


            if (count > 200 && this.BackColor != Color.DeepSkyBlue)
            {
                this.BackColor = Color.DeepSkyBlue;
            } //Change color at start

            
            if (floorTiles.Count < 50)
            {
                Ground newG = new Ground(gX, gY, gW, gH);
                gX += gW;
                floorTiles.Add(newG);
            } //Generate ground

            #region Platform generation
            if (trueX == 0 && platforms.Count < 5)
            {
                Platform newP = new Platform(pX, pY, pW, pH, platforms.Count);
                pX += gW;
                platforms.Add(newP);
            }
            #endregion

            #region Enemy Spawner
            if (baSpawned < 1)
            {
                Goonba newG = new Goonba(baX, baY, baWidth, baHeight, 400, 200, baDead);
                goons.Add(newG);
                baSpawned++;
            }
            if (baSpawned < 2)
            {
                baX = 650;
                Goonba newG = new Goonba(baX, baY, baWidth, baHeight, 800, 630, baDead);
                goons.Add(newG);
                baSpawned++;
            }
            #endregion

            #region Collisions
            p1Bot = new Rectangle(p1X + (p1Width / 2), p1Y+p1Height, 15, 1);
            p1Top = new Rectangle(p1X + (p1Width/2), p1Y, 15, 1);
            p1L = new Rectangle(p1X + (p1Width / 3), p1Y + (p1Height / 2) - 20, 1, 50);
            p1R = new Rectangle(p1X + p1Width - (p1Width/3), p1Y + (p1Height/2) - 20, 1, 50);

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
                //Note: Consider adding block break if time.
                Rectangle coPlat = new Rectangle(p.x, p.y,p.width, p.height);
                if (p1Bot.IntersectsWith(coPlat))

                {
                    grounded = true;
                    break;
                }
                if (p1Top.IntersectsWith(coPlat))

                {
                    jump = false;
                    break;
                }
                if (p1R.IntersectsWith(coPlat))

                {
                    if (p1Width >0)
                    {
                        right = false;
                    }
                    else
                    {
                        left = false;
                    }
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

            foreach (Goonba g in goons)
            {
                Rectangle goon = new Rectangle(g.x, g.y, g.width, g.height);
                if (baDir)
                {
                    if (g.x > g.RX)
                    {
                        baDir = !baDir;
                    }
                    else
                    {
                        g.moveGoon(g, baDir);
                    }
                }
                else
                {
                    if (g.x < g.LX)
                    {
                        baDir = !baDir;
                    }
                    else
                    {
                        g.moveGoon(g, baDir);
                    }
                }
                if (p1Bot.IntersectsWith(goon))
                {
                    g.dead = true;
                }
                if (p1L.IntersectsWith(goon) || p1R.IntersectsWith(goon) || p1Top.IntersectsWith(goon))
                {
                    dead();
                }

            }
            #endregion

            #region kill/break
            int index = goons.FindIndex(g => g.dead == true);
            if (index >= 0)
            {
                goons.RemoveAt(index);
            }
            #endregion

            count++;
            Refresh();
            Console.WriteLine(trueX);
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
                foreach (Goonba b in goons)
                {
                    e.Graphics.DrawImage(goonImage, b.x, b.y, b.width, b.height);
                    e.Graphics.DrawRectangle(pen, b.LX, b.y, b.RX - b.LX, 5);
                }
                e.Graphics.DrawImage(player1Image, p1X, p1Y, p1Width, p1Height);
                e.Graphics.DrawRectangle(pen, p1Top);
                e.Graphics.DrawRectangle(pen, p1Bot);
                e.Graphics.DrawRectangle(pen, p1L);
                e.Graphics.DrawRectangle(pen, p1R);
                

            } 
        }
    }
}
