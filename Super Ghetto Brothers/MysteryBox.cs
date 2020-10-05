using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Super_Ghetto_Brothers
{
    class MysteryBox
    {
        public int x, y, width, height, num, coins, state;
        string item;
        Random coinsR = new Random();

        public MysteryBox(int _x, int _y, int _width, int _height, int _coins, int _state)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            coins = _coins;
            state = _state;
        }
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
                coins = coinsR.Next(0, 10);
            }
        }
    }
}
