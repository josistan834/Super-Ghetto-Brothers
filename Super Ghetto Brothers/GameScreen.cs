using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Media;
using WMPLib;

namespace Super_Ghetto_Brothers
{
    public partial class GameScreen : UserControl
    {
        //All variables in the main game
        #region variables
        int level = 1;
        public static int WIDTH;
        int count, p1X, p1Y, p1Width, p1Height, gX, gY, gW, gH, p1YStored, pX, pY, pW, pH, pSpawned, pState, iStore;
        int baX, baY, baWidth, baHeight, baSpawned, koX, koY, koWidth, koHeight, koSpawned, backX;
        public static int lives, trueX, coins, oldCoins;
        public static bool WIN = false;
        bool leftArrowDown, rightArrowDown, upArrowDown, grounded, jump, right, left;
        bool baDead, baDir, isMyst;
        #endregion

        //Register user input on press and release
        #region userInput
        private void KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (count > 200)
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
           
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            if (count > 200)
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
            
        }
        #endregion

        //Create all the objects for sounds, images, and rectangles
        #region Objects
        SolidBrush brush = new SolidBrush(Color.White);
        Pen pen = new Pen(Color.White);
        Image player1Image = Properties.Resources.Bros_1_png;
        Image player2Image = Properties.Resources.Bros_2_png;
        Image groundImage = Properties.Resources.GroundBrick_1_png;
        Image goonImage = Properties.Resources.Goonba_1_png;
        Image koopaImage1 = Properties.Resources.Koopa_Trooper_1_png;
        Image koopaImage2 = Properties.Resources.Koopa_Trooper_2_png;
        Image mBox1 = Properties.Resources.PowerBrick_1_png;
        Image mBox2 = Properties.Resources.PowerBrick_2_png;
        Image flag = Properties.Resources.Flag_1_png;
        Image back = Properties.Resources.Backimage;
        List<Ground> floorTiles = new List<Ground>();
        List<Platform> platforms = new List<Platform>();
        List<Goonba> goons = new List<Goonba>();
        List<Koopa> koopas = new List<Koopa>();
        Rectangle p1Bot = new Rectangle();
        Rectangle p1Top = new Rectangle();
        Rectangle p1L = new Rectangle();
        Rectangle p1R = new Rectangle();
        Random mysteryChance = new Random();
        SoundPlayer stomp = new SoundPlayer(Properties.Resources.stomp);
        SoundPlayer death = new SoundPlayer(Properties.Resources.dead);
        SoundPlayer winSound = new SoundPlayer(Properties.Resources.Stage_Win__Super_Mario____Sound_Effect__HD_);
        WindowsMediaPlayer music = new WindowsMediaPlayer();
        #endregion

        //All of the created methods to do repetative tasks
        #region CustomMethods

        //Initialize the userControl
        public GameScreen() 
        {
            InitializeComponent();
        }

        //Runs when you are hit by a enemy
        public void dead() 
        {
            if(count > 200 && iStore <=0)
            {
                if (pState == 1)
                {
                    death.PlaySync();
                    count = 0;
                    lives--;
                    gameTimer.Stop();
                    // f is the form that this control is on - ("this" is the current User Control) 
                    Form f = this.FindForm();
                    f.Controls.Remove(this);
                    // Create an instance of the SecondScreen 
                    GameOver go = new GameOver();
                    // Add the User Control to the Form 
                    f.Controls.Add(go);
                    go.Location = new Point((f.Width - go.Width) / 2, (f.Height - go.Height) / 2);
                }
                else
                {
                    pState = 1;
                    iStore = 100;
                }
            }
           
        }

        //Runs when you touch the flag
        public void win()
        {
            winSound.PlaySync();
            WIN = true;
            gameTimer.Stop();
            // f is the form that this control is on - ("this" is the current User Control) 
            Form f = this.FindForm();
            f.Controls.Remove(this);
            // Create an instance of the SecondScreen 
            GameOver go = new GameOver();
            // Add the User Control to the Form 
            f.Controls.Add(go);
            go.Location = new Point((f.Width - go.Width) / 2, (f.Height - go.Height) / 2);
        }
       
        //Runs when the userControl is added to the form
        private void GameScreen_Load(object sender, EventArgs e) 
        {
            this.Focus();
            WIN = false;
            iStore = 0;
            pState = 1;
            isMyst = false;
            this.BackColor = Color.Black;
            WIDTH = this.Width;
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
            pSpawned = 0;
            koX = 900;
            koHeight = 90;
            koWidth = 45;
            koSpawned = 0;
            koY = this.Height - koHeight - gH;
            gameTimer.Start();
        }

        //Bounce off enemys when called
        private void bounce()
        {
            stomp.Play();
            //Basically teleports you up
            for (int i = 0; i <10; i++)
            {
                p1Y-=10;
            }
        }
        #endregion

        //Everything that happens every tick of the game will run here (game engine)
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //Code to move the player and object respectively based on user input
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
                        trueX-=5;
                        backX++;
                        foreach (Goonba g in goons)
                        {
                            g.RX+=5;
                            g.LX+=5;
                        }

                            foreach (Ground b in floorTiles)
                        {
                            b.x+=5;
                        }
                        foreach (Platform b in platforms)
                        {
                            b.x+=5;
                        }
                        foreach (Koopa b in koopas)
                        {
                            b.x += 5;
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
                        trueX+=5;
                        backX--;
                        foreach (Goonba g in goons)
                        {
                            g.RX-=5;
                            g.LX-=5;
                        }
                        foreach (Ground b in floorTiles)
                        {
                            b.x-=5;
                        }
                        foreach (Platform b in platforms)
                        {
                            b.x-=5;
                        }
                        foreach (Koopa b in koopas)
                        {
                            b.x -= 5;
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

            //Invinvibility Frames
            if (iStore > 0) { iStore--; }

            //Coins to Life
            if (coins >= 100)
            {
                if (coins-oldCoins >= 100)
                {
                    lives++;
                    oldCoins = coins;
                }  
            }

            //Change color at start
            if (count > 200 && this.BackColor != Color.DeepSkyBlue)
            {
                this.BackColor = Color.DeepSkyBlue;
            } 

            //Creates the floor before the games begins
            #region Floor generation
            while (floorTiles.Count < 50)
            {
                Ground newG = new Ground(gX, gY, gW, gH);
                gX += gW;
                floorTiles.Add(newG);
            }
            while (floorTiles.Count < 80)
            {
                Ground newG = new Ground(gX+200, gY, gW, gH);
                gX += gW;
                floorTiles.Add(newG);
            }
            while (floorTiles.Count < 130)
            {
                Ground newG = new Ground(gX + 400, gY, gW, gH);
                gX += gW;
                floorTiles.Add(newG);
            }
            #endregion

            //Creates platforms before the games begin also uses a random to genereate some mystery blocks
            #region Platform generation
            while (pSpawned < 5)
            {
                if (mysteryChance.Next(0, 5) == 1) { isMyst = true; } else { isMyst = false; }
                Platform newP = new Platform(pX + (gW * pSpawned), pY, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
                
                
            }
            while (pSpawned < 7)
            {
                if (mysteryChance.Next(0, 5) == 1) { isMyst = true; } else { isMyst = false; }
                Platform newP = new Platform(pX + (gW * pSpawned) +200, pY, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 11)
            {
                if (mysteryChance.Next(0, 5) == 1) { isMyst = true; } else { isMyst = false; }
                Platform newP = new Platform(pX + (gW * pSpawned) + 600, pY, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            //Note: ground platforms, no randoms.
            while (pSpawned < 16)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 820, this.Height-60, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 20)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 700, this.Height - 90, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 25)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 900, this.Height - 60, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 29)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 750, this.Height - 90, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 36)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 2420, this.Height - 60, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 42)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 2240, this.Height - 90, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 47)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 2090, this.Height - 120, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 51)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 1970, this.Height - 150, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 54)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 1880, this.Height - 180, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 56)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 1820, this.Height - 210, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 57)
            {
                isMyst = false;
                Platform newP = new Platform(pX + (gW * pSpawned) + 1790, this.Height - 240, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 62)
            {
                if (mysteryChance.Next(0, 5) == 1) { isMyst = true; } else { isMyst = false; }
                Platform newP = new Platform(pX + (gW * pSpawned), pY, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 66)
            {
                if (mysteryChance.Next(0, 5) == 1) { isMyst = true; } else { isMyst = false; }
                Platform newP = new Platform(pX + (gW * pSpawned) + 200, pY-60, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 71)
            {
                if (mysteryChance.Next(0, 5) == 1) { isMyst = true; } else { isMyst = false; }
                Platform newP = new Platform(pX + (gW * pSpawned) + 800, pY - 60, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            while (pSpawned < 75)
            {
                if (mysteryChance.Next(0, 5) == 1) { isMyst = true; } else { isMyst = false; }
                Platform newP = new Platform(pX + (gW * pSpawned) + 980, pY, pW, pH, isMyst);
                platforms.Add(newP);
                pSpawned++;
            }
            #endregion

            //Spawns the specified enemy at specified loaction uses the spawned int so dead enemies dont come back
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
            if (baSpawned < 3)
            {
                baX = 1050;
                Goonba newG = new Goonba(baX, baY, baWidth, baHeight, 1200, 1030, baDead);
                goons.Add(newG);
                baSpawned++;
            }
            if (baSpawned < 4)
            {
                baX = 2050;
                Goonba newG = new Goonba(baX, baY, baWidth, baHeight, 2200, 2030, baDead);
                goons.Add(newG);
                baSpawned++;
            }
            if (baSpawned < 5)
            {
                baX = 2950;
                Goonba newG = new Goonba(baX, baY, baWidth, baHeight, 3000, 2930, baDead);
                goons.Add(newG);
                baSpawned++;
            }
            if (baSpawned < 6)
            {
                baX = 3150;
                Goonba newG = new Goonba(baX, baY, baWidth, baHeight, 3200, 3130, baDead);
                goons.Add(newG);
                baSpawned++;
            }

            if (koSpawned < 1)
            {
                koSpawned++;
                Koopa newK = new Koopa(koX, koY, koWidth, koHeight, 1, false);
                koopas.Add(newK);
            }
            if (koSpawned < 2)
            {
                koSpawned++;
                Koopa newK = new Koopa(koX + 1600, koY, koWidth, koHeight, 1, false);
                koopas.Add(newK);
            }
            if (koSpawned < 3)
            {
                koSpawned++;
                Koopa newK = new Koopa(koX + 2000, koY, koWidth, koHeight, 1, false);
                koopas.Add(newK);
            }
            #endregion

            //Collisions with player and other objects
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
                    foreach (Platform b in platforms)
                    {
                        Rectangle coPlat2 = new Rectangle(b.x, b.y, b.width, b.height);
                        if (p1R.IntersectsWith(coPlat2) || p1L.IntersectsWith(coPlat2))

                        {
                            if (p1Width > 0)
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

                    break;
                }
                if (p1Top.IntersectsWith(coPlat))
                {
                    jump = false;
                    if (p.isMyst && p.state !=2)
                    {
                        p.state = 2;
                        p.randItem(mysteryChance.Next(0, 10));
                        coins += p.coins;
                        if(p.item == "star")
                        {
                            pState = 2;
                        }
                        if (p.item == "life")
                        {
                            lives++;
                        }
                    }
                    break;
                }
                if (p1R.IntersectsWith(coPlat) || p1L.IntersectsWith(coPlat))

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
                    bounce();
                    g.dead = true;
                }
                if (p1L.IntersectsWith(goon) || p1R.IntersectsWith(goon) || p1Top.IntersectsWith(goon))
                {
                    dead();
                }

            }
            foreach (Koopa k in koopas)
            {
                Rectangle koop = new Rectangle();
                k.attack();
                if (k.facingR)
                {
                    if (k.state == 2)
                    {
                        koop = new Rectangle(k.x, k.y + (k.height/2), k.width, k.height/2);
                    }
                    else
                    {
                        koop = new Rectangle(k.x, k.y, k.width, k.height);
                    }


                }
                else
                {
                    if (k.state == 2)
                    {
                        koop = new Rectangle(k.x - k.width, k.y + (k.height / 2), k.width, k.height/2);
                    }
                    else
                    {
                        koop = new Rectangle(k.x - k.width, k.y, k.width, k.height);
                    }
                }
                if (p1Bot.IntersectsWith(koop))
                {
                    if(k.state == 1)
                    {
                        k.state = 2;
                        bounce();
                        break;
                    }
                    else
                    {
                        k.dead = true;
                        bounce();
                        break;
                    }
                    
                }
                if (p1L.IntersectsWith(koop) || p1R.IntersectsWith(koop) || p1Top.IntersectsWith(koop) && k.state == 1)
                {
                    dead();
                }

            }
            Bullet.shoot();
            foreach (Bullet b in Koopa.bullets)
            {
                Rectangle bull = new Rectangle(b.x, b.y, b.width, b.height);
                if (p1Bot.IntersectsWith(bull))
                {
                    b.dead = true;
                    bounce();
                }
                    if (p1L.IntersectsWith(bull) || p1R.IntersectsWith(bull) || p1Top.IntersectsWith(bull))
                {
                    dead();
                }
            }

                #endregion

            //If an enemy is dead, find and remove it from the list
            #region kill/break
            int index = goons.FindIndex(g => g.dead == true);
            if (index >= 0)
            {
                goons.RemoveAt(index);
            }
            int index2 = Koopa.bullets.FindIndex(g => g.dead == true);
            if (index2 >= 0)
            {
                Koopa.bullets.RemoveAt(index2);
            }
            int index3 = koopas.FindIndex(k => k.dead == true);
            if (index3 >= 0)
            {
                koopas.RemoveAt(index3);
            }
            #endregion

            //If touching the location of the flag, win.
            if (p1X >= floorTiles[0].x + 3800 && p1X <= floorTiles[0].x + 3805 && p1Y >= this.Height-250)
            {
                win();
            }

            //Add one to the counter used for timing logic
            count++;

            //Refresh the gamescreen
            Refresh();
        }

        //Draw the objects to the screen
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //Showing the level screen for 200 ticks
            if (count < 200)
            {
                e.Graphics.DrawString($"{lives}X Lives \n Level {level}", this.Font, brush, this.Width /2 - 50, this.Height / 2);
            }
            //Display all game objects while the game is running
            else if (count > 200)
            {
                e.Graphics.DrawImage(back, backX-25, -500, 1500, 1500);
                foreach (Ground g in floorTiles)
                {
                    e.Graphics.DrawImage(groundImage, g.x, g.y, g.width, g.height);   
                }
                foreach (Platform p in platforms)
                {
                    if (!p.isMyst)
                    {
                        e.Graphics.DrawImage(groundImage, p.x, p.y, p.width, p.height);
                    }
                    else
                    {
                        if (p.state != 2)
                        {
                            e.Graphics.DrawImage(mBox1, p.x, p.y, p.width, p.height);
                        }
                        else
                        {
                            e.Graphics.DrawImage(mBox2, p.x, p.y, p.width, p.height);
                        }
                        
                    }
                }
                foreach (Koopa k in koopas)
                {
                    if (k.facingR)
                    {
                        if (k.state == 2)
                        {
                            e.Graphics.DrawImage(koopaImage2, k.x, k.y, k.width, k.height);
                        }
                        else
                        {
                            e.Graphics.DrawImage(koopaImage1, k.x, k.y, k.width, k.height);
                        }

                       
                    }
                    else
                    {
                        if (k.state == 2)
                        {
                            e.Graphics.DrawImage(koopaImage2, k.x, k.y, k.width * -1, k.height);
                        }
                        else
                        {
                            e.Graphics.DrawImage(koopaImage1, k.x, k.y, k.width*-1, k.height);
                        }
                        
                        
                    }
                    
                }
                foreach (Goonba b in goons)
                {
                    e.Graphics.DrawImage(goonImage, b.x, b.y, b.width, b.height);
                }
                foreach (Bullet b in Koopa.bullets)
                {
                    e.Graphics.FillRectangle(brush,b.x, b.y, b.width, b.height);

                }
                e.Graphics.DrawString($"Coins: {coins}", this.Font, brush, 10, 50);
                e.Graphics.DrawString($"Lives: {lives}", this.Font, brush, 10, 70);
                e.Graphics.DrawString($"Score: {Convert.ToInt16(count/10)}", this.Font, brush, 10, 90);
                e.Graphics.DrawImage(flag, floorTiles[0].x + 3800, this.Height - 270, 80, 250);
                if (pState == 1)
                {
                    e.Graphics.DrawImage(player1Image, p1X, p1Y, p1Width, p1Height);
                }
                else
                {
                    e.Graphics.DrawImage(player2Image, p1X, p1Y, p1Width, p1Height);
                }
                
                //TESTING BOXES
                //e.Graphics.DrawRectangle(pen, p1Top);
                //e.Graphics.DrawRectangle(pen, p1Bot);
                //e.Graphics.DrawRectangle(pen, p1L);
                //e.Graphics.DrawRectangle(pen, p1R);


            } 
        }
    }
}
