using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Ghetto_Brothers
{
    public class Goonba
    {
        //Variables
        public int x, y, width, height, LX, RX, speed;
        public bool dead;

        //Method to recieve properties of goonba
        public Goonba(int _x, int _y, int _width, int _height, int _RX, int _LX, bool _dead)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            RX = _RX;
            LX = _LX;
            dead = _dead;

        }

        //Behaviour to move goonbas
        public void moveGoon(Goonba g, bool dir)
        {
            if (dir == false)
            {
                g.x -= 5;
            }
            else
            {
                g.x += 5;
            }
        }

    }
}
