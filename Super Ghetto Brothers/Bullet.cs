using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Super_Ghetto_Brothers
{
    public class Bullet
    {
        public int x, y, width, height;
        public bool dead, facingR;
        public Bullet(int _x, int _y, int _width, int _height, bool _dead, bool _facingR)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            dead = _dead;
            facingR = _facingR;
        }
        public static void shoot()
        {
            foreach(Bullet b in Koopa.bullets)
            {
                if(b.x > -5 && b.x < GameScreen.WIDTH)
                {
                    if (b.facingR)
                    {
                        b.x+=10;
                    }
                    else
                    {
                        b.x-=10;
                    }
                }
                else
                {
                    b.dead = true;
                }
               
            }
        }
    }
}
