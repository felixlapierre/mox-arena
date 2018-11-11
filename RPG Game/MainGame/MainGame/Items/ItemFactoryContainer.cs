using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Items.Charms;
using MainGame.Items.Weapons;
using MainGame.Items.Shields;

namespace MainGame.Items
{
    public class ItemFactoryContainer
    {
        public static WeaponFactory Weapons { get; set; }
        public static ShieldFactory Shields { get; set; }
        public static CharmFactory Charms { get; set; }

        public static bool IsInitialized = false;

        public static void Initialize(ContentManager Content)
        {
            if (IsInitialized)
                return;
            IsInitialized = true;

            Weapons = new WeaponFactory();

            Shields = new ShieldFactory();

            Charms = new CharmFactory();
        }

        public static Weapon GetWeaponFromID(int id)
        {
            switch (id)
            {
                case -1:
                    return Weapons.CreateMaverick();
                case 0:
                    return Weapons.CreateSword();
                case 1:
                    return Weapons.CreateBroadsword();
                case 2:
                    return Weapons.CreateBow();
                case 3:
                    return Weapons.CreateIceBow();
                case 4:
                    return Weapons.CreateThrowingAxe();
                case 5:
                    return Weapons.CreateDwarvenAxe();
                case 6:
                    return Weapons.CreateMaul();
                case 7:
                    return Weapons.CreateHammer();
                case 8:
                    return Weapons.CreateSpear();
                case 9:
                    return Weapons.CreateThrowingSpear();
                case 10:
                    return Weapons.CreateJungleSpear();
                case 11:
                    return Weapons.CreateThrowingDagger();
                case 12:
                    return Weapons.CreateFireball();
                case 13:
                    return Weapons.CreateFirebolt();
                case 14:
                    return Weapons.CreateFirework();
                case 15:
                    return Weapons.CreateHelsingor();
                case 16:
                    return Weapons.CreateBoteng();
                case 17:
                    return Weapons.CreateHira();
                case 18:
                    return Weapons.CreateTaago();
                case 19:
                    return Weapons.CreatePlasmaBolt();
                case 20:
                    return Weapons.CreateGrapple();
                default:
                    return Weapons.CreateSword();
            }
        }

        public static Shield GetShieldFromID(int id)
        {
            switch (id)
            {
                case 0:
                    return Shields.CreateSpeedboost();
                case 1:
                    return Shields.CreateBasicShield();
                case 2:
                    return Shields.CreateTowerShield();
                case 3:
                    return Shields.CreateThunderStone();
                case 4:
                    return Shields.CreateElvenTrinket();
                default:
                    return Shields.CreateBasicShield();

            }
        }

        public static Charm GetCharmFromID(int id)
        {
            switch (id)
            {
                case 0:
                    return Charms.CreateEmptyCharm();
                case 1:
                    return Charms.CreateLowerCooldown();
                case 2:
                    return Charms.CreateHigherCooldown();
                case 3:
                    return Charms.CreateBurstCharm();
                case 4:
                    return Charms.CreateSpeedCharm();
                case 5:
                    return Charms.CreateLifestealCharm();
                default:
                    return Charms.CreateEmptyCharm();

            }
        }
    }
}
