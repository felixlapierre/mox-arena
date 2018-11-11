using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            #region Weapons
            Texture2D weaponSword1;
            Texture2D weaponSword2;
            Texture2D weaponBow1;
            Texture2D weaponBow2;
            Texture2D projectileBow1;
            Texture2D weaponAxe1;
            Texture2D weaponAxe2;
            Texture2D weaponDagger1;
            Texture2D weaponSpear1;
            Texture2D weaponSpear2;
            Texture2D weaponSpear3;
            Texture2D weaponMaul1;
            Texture2D weaponMaulLarge;
            Texture2D weaponHammer1;
            Texture2D weaponShuriken1;
            Texture2D weaponShuriken2;
            Texture2D weaponShuriken3;
            Texture2D weaponGrapple1;
            #endregion

            #region Shields
            Texture2D shieldBasic1;
            Texture2D shieldBasic2;
            Texture2D speedBoost1;
            Texture2D thunderStone;
            Texture2D elvenTrinket;
            #endregion

            #region Charms
            Texture2D charmSprite;
            #endregion

            #region Load Weapons
            weaponSword1 = Content.Load<Texture2D>("graphics/weapons/sword1");
            weaponSword2 = Content.Load<Texture2D>("graphics/WeaponSword2");
            weaponBow1 = Content.Load<Texture2D>("graphics/WeaponBow1");
            weaponBow2 = Content.Load<Texture2D>("graphics/WeaponBow2");
            projectileBow1 = Content.Load<Texture2D>("graphics/ProjectileBow1");
            weaponAxe1 = Content.Load<Texture2D>("graphics/WeaponAxe1");
            weaponAxe2 = Content.Load<Texture2D>("graphics/WeaponAxe2");
            weaponMaul1 = Content.Load<Texture2D>("graphics/WeaponMaul1");
            weaponMaulLarge = Content.Load<Texture2D>("graphics/WeaponMaulLarge");
            weaponHammer1 = Content.Load<Texture2D>("graphics/WeaponHammer1");
            weaponDagger1 = Content.Load<Texture2D>("graphics/WeaponDagger1");
            weaponSpear1 = Content.Load<Texture2D>("graphics/WeaponSpear1");
            weaponSpear2 = Content.Load<Texture2D>("graphics/WeaponSpear2");
            weaponSpear3 = Content.Load<Texture2D>("graphics/WeaponSpear3");
            weaponShuriken1 = Content.Load<Texture2D>("graphics/shuriken1");
            weaponShuriken2 = Content.Load<Texture2D>("graphics/shuriken2");
            weaponShuriken3 = Content.Load<Texture2D>("graphics/shuriken3");
            weaponGrapple1 = Content.Load<Texture2D>("graphics/WeaponGrapple1");

            Texture2D redShockwaveBullet = Content.Load<Texture2D>("graphics/redShockwaveBullet");
            Texture2D blueShockwaveBullet = Content.Load<Texture2D>("graphics/blueShockwaveBullet");

            Weapons = new WeaponFactory(weaponSword1, weaponBow1, weaponBow2, projectileBow1, weaponSword2, weaponAxe1, weaponAxe2,
                weaponMaul1, weaponMaulLarge, weaponHammer1, weaponDagger1, weaponSpear1, weaponSpear2, weaponSpear3, weaponShuriken1,
                weaponShuriken2, weaponShuriken3, redShockwaveBullet, blueShockwaveBullet, weaponGrapple1);
            #endregion

            #region Load Shields
            shieldBasic1 = Content.Load<Texture2D>("graphics/Shield1");
            shieldBasic2 = Content.Load<Texture2D>("graphics/Shield2");
            speedBoost1 = Content.Load<Texture2D>("graphics/speedBoost1");
            thunderStone = Content.Load<Texture2D>("graphics/thunderStone");
            elvenTrinket = Content.Load<Texture2D>("graphics/ElvenTrinket");

            Shields = new ShieldFactory(shieldBasic1, shieldBasic2, speedBoost1, thunderStone, blueShockwaveBullet, elvenTrinket);
            #endregion

            #region Load Charms
            charmSprite = Content.Load<Texture2D>("graphics/charmSprite");
            Charms = new CharmFactory(charmSprite);
            #endregion
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
