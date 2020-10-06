using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Super_Ghetto_Brothers
{
    public class Koopa
    {
        //Variables
        public int x, y, width, height, state;
        public bool dead, facingR;
        public static List<Bullet> bullets = new List<Bullet>();
        int count = 0;

        //Method to recieve properties of koopa
        public Koopa(int _x, int _y, int _width, int _height, int _state, bool _dead)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            dead = _dead;
            state = _state;
        }

        //Behaviour for koopa to attack
        public void attack()
        {
            count++;
            if (count%100 == 0 && state == 1)
            {
                facingR = !facingR;
                Bullet newB = new Bullet(x, y+(height/2), 5, 5, false, facingR);
                bullets.Add(newB);

            }
        }
    }
}
