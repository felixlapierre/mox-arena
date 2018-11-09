using MainGame.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Screens.Trade_Screen
{
    class TradeScreenContents
    {
        public int Health;
        public int Level { get; set; }
        public Weapon Weapon1;
        public Weapon Weapon2;
        public Shield Shield1;
        public Charm Charm1;

        public Item Item1;
        public Item Item2;
        public Item Item3;

        public TradeScreenContents()
        {

        }

        public TradeScreenContents(int health, int level, Weapon weapon1, Weapon weapon2,
            Shield shield1, Charm charm1, Item item1, Item item2, Item item3)
        {
            Health = health;
            Level = level;
            Weapon1 = weapon1;
            Weapon2 = weapon2;
            Shield1 = shield1;
            Charm1 = charm1;

            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            
        }
    }
}
