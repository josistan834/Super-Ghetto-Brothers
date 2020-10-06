using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Ghetto_Brothers
{
    public class Platform
    {
        //Variables
        public int x, y, width, height, coins, state;
        public bool isMyst;
        public string item;
        Random coinsR = new Random();

        //Method to recieve properties of platforms
        public Platform(int _x, int _y, int _width, int _height, bool _isMyst)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            isMyst = _isMyst;
        }

        //Behaviour for mystery boxes
        public void randItem(int rand)
        {
            if (rand == 1)
            {
                item = "star";
            }
            else if (rand == 2)
            {
                item = "life";
            }
            else
            {
                item = "coin";
            }
            if (item == "coin")
            {
                coins = coinsR.Next(1, 10);
            }
            else
            {
                coins = 0;
            }
        }
    }
}
