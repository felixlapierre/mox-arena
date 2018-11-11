using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Items.Weapons
{
    public class WeaponFactory
    {
        #region Properties
        Texture2D weaponSword1;
        Texture2D weaponBow1;
        Texture2D weaponBow2;
        Texture2D projectileBow1;
        Texture2D weaponSword2;
        Texture2D weaponAxe1;
        Texture2D weaponAxe2;
        Texture2D weaponMaul1;
        Texture2D weaponMaulLarge;
        Texture2D weaponHammer1;
        Texture2D weaponDagger1;
        Texture2D weaponSpear1;
        Texture2D weaponSpear2;
        Texture2D weaponSpear3;
        Texture2D weaponShuriken1;
        Texture2D weaponShuriken2;
        Texture2D weaponShuriken3;
        Texture2D redShockwaveBullet;
        Texture2D blueShockwaveBullet;
        Texture2D weaponGrapple1;
        #endregion

        #region Constructors
        public WeaponFactory(Texture2D weaponSword1, Texture2D weaponBow1, Texture2D weaponBow2, Texture2D projectileBow1, Texture2D weaponSword2,
            Texture2D weaponAxe1, Texture2D weaponAxe2, Texture2D weaponMaul1, Texture2D weaponMaulLarge, Texture2D weaponHammer1, Texture2D weaponDagger1,
            Texture2D weaponSpear1, Texture2D weaponSpear2, Texture2D weaponSpear3, Texture2D weaponShuriken1, Texture2D weaponShuriken2,
            Texture2D weaponShuriken3, Texture2D redShockwaveBullet, Texture2D blueShockwaveBullet, Texture2D weaponGrapple1)
        {
            this.weaponSword1 = weaponSword1;
            this.weaponBow1 = weaponBow1;
            this.weaponBow2 = weaponBow2;
            this.projectileBow1 = projectileBow1;
            this.weaponSword2 = weaponSword2;
            this.weaponAxe1 = weaponAxe1;
            this.weaponAxe2 = weaponAxe2;
            this.weaponMaul1 = weaponMaul1;
            this.weaponMaulLarge = weaponMaulLarge;
            this.weaponHammer1 = weaponHammer1;
            this.weaponDagger1 = weaponDagger1;
            this.weaponSpear1 = weaponSpear1;
            this.weaponSpear2 = weaponSpear2;
            this.weaponSpear3 = weaponSpear3;
            this.weaponShuriken1 = weaponShuriken1;
            this.weaponShuriken2 = weaponShuriken2;
            this.weaponShuriken3 = weaponShuriken3;
            this.redShockwaveBullet = redShockwaveBullet;
            this.blueShockwaveBullet = blueShockwaveBullet;
            this.weaponGrapple1 = weaponGrapple1;
        }
        #endregion

        public Weapon CreateSword()
        {
            string name = "Sword";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponSword2;
            float arc = (float)Math.PI / 8;
            float stabDistance = 75;
            int damage = 20;
            float knockback = 0.30f;
            int cooldown = 750;
            int timeDisplayed = 400;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects);
            weapon.ID = 0;
            return weapon;
        }

        public Weapon CreateBroadsword()
        {
            string name = "Broadsword";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponSword1;
            float arc = (float)Math.PI * 3 / 2;
            float stabDistance = 50;
            int damage = 15;
            float knockback = 0.70f;
            int cooldown = 500;
            int timeDisplayed = 200;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects);
            weapon.ID = 1;
            return weapon;
        }

        public Weapon CreateBow()
        {
            string name = "Bow";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponBow1;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1000;
            int timeDisplayed = 100;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = projectileBow1;
            int projectileDamage = 7;
            float projectileKnockback = 0.30f;
            int projectileLifetime = 700;
            float projectileSpeed = 1.0f;
            int projectileBounces = 0;
            int projectilePierces = 3;
            List<Effect> projectileActiveEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 2;
            return weapon;
        }

        public Weapon CreateIceBow()
        {
            string name = "IceBow";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponBow2;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 2000;
            int timeDisplayed = 100;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = projectileBow1;
            int projectileDamage = 10;
            float projectileKnockback = 0f;
            int projectileLifetime = 500;
            float projectileSpeed = 1.0f;
            int projectileBounces = 0;
            int projectilePierces = 3;
            List<Effect> projectileActiveEffects = new List<Effect>();
            onHitEffects.Add(new Effect(EffectType.Speed, 2000, 0.50f));

            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 3;
            return weapon;
        }

        public Weapon CreateThrowingAxe()
        {
            string name = "ThrowingAxe";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponAxe1;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 750;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = weaponAxe1;
            int projectileDamage = 10;
            float projectileKnockback = 0.30f;
            int projectileLifetime = 1000;
            float projectileSpeed = 0.5f;
            int projectileBounces = 5;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Spin, -1, (float)Math.PI / 5));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 4;
            weapon.Tooltip = "Bounces off of walls up to 5 times.";
            return weapon;
        }

        public Weapon CreateDwarvenAxe()
        {
            string name = "DwarvenAxe";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponAxe2;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            onHitEffects.Add(new Effect(EffectType.Speed, 1100, 0f));
            Texture2D projectileSprite = weaponAxe2;
            int projectileDamage = 7;
            float projectileKnockback = 0f;
            int projectileLifetime = 1000;
            float projectileSpeed = 0.5f;
            int projectileBounces = 5;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Spin, -1, (float)Math.PI / 5));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 5;
            weapon.Tooltip = "Bounces off walls up to 5 times.";
            return weapon;
        }

        public Weapon CreateMaul()
        {
            string name = "Maul";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponMaul1;
            float arc = (float)Math.PI / 8;
            float stabDistance = 70;
            int damage = 25;
            float knockback = 0.50f;
            int cooldown = 1000;
            int timeDisplayed = 1000;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects);
            weapon.ID = 6;
            return weapon;
        }

        public Weapon CreateHammer()
        {
            string name = "Hammer";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponHammer1;
            float arc = (float)Math.PI / 2;
            float stabDistance = 70;
            int damage = 25;
            float knockback = 0.55f;
            int cooldown = 1000;
            int timeDisplayed = 500;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            onHitEffects.Add(new Effect(EffectType.Stun, 750, 1f));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects);
            weapon.ID = 7;
            weapon.Tooltip = "High knockback";
            return weapon;
        }

        public Weapon CreateSpear()
        {
            string name = "Spear";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponSpear1;
            float arc = (float)Math.PI / 8;
            float stabDistance = 100;
            int damage = 15;
            float knockback = 0.40f;
            int cooldown = 1000;
            int timeDisplayed = 200;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects);
            weapon.ID = 8;
            return weapon;
        }

        public Weapon CreateThrowingSpear()
        {
            string name = "ThrowingSpear";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponSpear2;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1200;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = weaponSpear2;
            int projectileDamage = 15;
            float projectileKnockback = 0.30f;
            int projectileLifetime = 500;
            float projectileSpeed = 0.6f;
            int projectileBounces = 0;
            int projectilePierces = 5;
            List<Effect> projectileActiveEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 9;
            return weapon;
        }

        public Weapon CreateJungleSpear()
        {
            string name = "JungleSpear";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponSpear3;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 2000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            onHitEffects.Add(new Effect(EffectType.Poison, 5000, 4));
            Texture2D projectileSprite = weaponSpear3;
            int projectileDamage = 5;
            float projectileKnockback = 0.30f;
            int projectileLifetime = 500;
            float projectileSpeed = 0.6f;
            int projectileBounces = 0;
            int projectilePierces = 5;
            List<Effect> projectileActiveEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 10;
            return weapon;
        }

        public Weapon CreateThrowingDagger()
        {
            string name = "ThrowingDagger";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponDagger1;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            onHitEffects.Add(new Effect(EffectType.Poison, 5000, 5));
            Texture2D projectileSprite = weaponDagger1;
            int projectileDamage = 4;
            float projectileKnockback = 0.25f;
            int projectileLifetime = 700;
            float projectileSpeed = 0.6f;
            int projectileBounces = 0;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 11;
            return weapon;
        }

        public Weapon CreateMaverick()
        {
            string name = "Maverick";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponAxe1;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 500;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = weaponAxe1;
            int projectileDamage = 200;
            float projectileKnockback = 0.30f;
            int projectileLifetime = 1500;
            float projectileSpeed = 1f;
            int projectileBounces = -1;
            int projectilePierces = -1;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Spin, -1, (float)Math.PI / 5));

            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.Burst = 3;
            weapon.Spread = 16;
            weapon.SpreadAngle = (float)Math.PI * 2;
            weapon.SprayAngle = (float)Math.PI / 4;
            weapon.ID = -1;
            weapon.Tooltip = "ho";
            return weapon;
        }

        public Weapon CreateFireball()
        {
            string name = "FireBall";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = redShockwaveBullet;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 4000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = redShockwaveBullet;
            int projectileDamage = 20;
            float projectileKnockback = 0.25f;
            int projectileLifetime = 710;
            float projectileSpeed = 0.6f;
            int projectileBounces = 0;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 700, 0.25f, redShockwaveBullet, projectileDamage, 200));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 12;
            return weapon;
        }

        public Weapon CreateFirebolt()
        {
            string name = "FireBolt";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = redShockwaveBullet;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = redShockwaveBullet;
            int projectileDamage = 10;
            float projectileKnockback = 0.0f;
            int projectileLifetime = 400;
            float projectileSpeed = 0.8f;
            int projectileBounces = 0;
            int projectilePierces = 1;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 100, 0.0f, redShockwaveBullet, 7, 50));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 200, 0.0f, redShockwaveBullet, 7, 50));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 300, 0.0f, redShockwaveBullet, 7, 50));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 400, 0.0f, redShockwaveBullet, 7, 50));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 13;
            return weapon;
        }

        public Weapon CreateFirework()
        {
            string name = "Meteor";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = redShockwaveBullet;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 10000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = redShockwaveBullet;
            int projectileDamage = 30;
            float projectileKnockback = 0.30f;
            int projectileLifetime = 710;
            float projectileSpeed = 0.7f;
            int projectileBounces = -1;
            int projectilePierces = 10;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 100, 0.5f, redShockwaveBullet, 40, 100));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 250, 0.5f, redShockwaveBullet, 40, 200));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 400, 0.5f, redShockwaveBullet, 40, 300));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 600, 0.5f, redShockwaveBullet, 40, 600));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 633, 0.5f, redShockwaveBullet, 40, 500));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 666, 0.5f, redShockwaveBullet, 40, 400));
            projectileActiveEffects.Add(new Effect(EffectType.Shockwave, 700, 0.5f, redShockwaveBullet, 40, 300));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 14;
            return weapon;
        }

        public Weapon CreateHelsingor()
        {
            string name = "Helsingor";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponMaulLarge;
            float arc = (float)Math.PI / 8;
            float stabDistance = 70;
            int damage = 25;
            float knockback = 0.50f;
            int cooldown = 2000;
            int timeDisplayed = 1000;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            onHitEffects.Add(new Effect(EffectType.Poison, 4000, 5));
            onHitEffects.Add(new Effect(EffectType.Stun, 500, 1f));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects);
            weapon.ID = 15;
            return weapon;
        }

        public Weapon CreateBoteng()
        {
            string name = "Boteng Shuriken";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponShuriken1;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = weaponShuriken1;
            int projectileDamage = 9;
            float projectileKnockback = 0.15f;
            int projectileLifetime = 700;
            float projectileSpeed = 0.7f;
            int projectileBounces = 0;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Spin, -1, (float)Math.PI / 5));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.Burst = 2;
            weapon.BurstDelay = 200;
            weapon.Tooltip = "Shoots a burst of 3 shurikens.";
            weapon.ID = 16;
            return weapon;
        }

        public Weapon CreateHira()
        {
            string name = "Hira Shuriken";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponShuriken2;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1000;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = weaponShuriken2;
            int projectileDamage = 9;
            float projectileKnockback = 0.15f;
            int projectileLifetime = 700;
            float projectileSpeed = 0.7f;
            int projectileBounces = 0;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Spin, -1, (float)Math.PI / 5));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.Spread = 3;
            weapon.SpreadAngle = (float)Math.PI / 8;
            weapon.Tooltip = "Shoots 3 shurikens at once in a spread.";
            weapon.ID = 17;
            return weapon;
        }

        public Weapon CreateTaago()
        {
            string name = "Taago Shuriken";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponShuriken3;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 200;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = weaponShuriken3;
            int projectileDamage = 9;
            float projectileKnockback = 0.15f;
            int projectileLifetime = 700;
            float projectileSpeed = 0.7f;
            int projectileBounces = 0;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Spin, -1, (float)Math.PI / 5));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.SprayAngle = (float)Math.PI / 16 * 3;
            weapon.Tooltip = "Shurikens are shot rapidly but with little accuracy.";
            weapon.ID = 18;
            return weapon;
        }

        public Weapon CreatePlasmaBolt()
        {
            string name = "Plasma Bolt";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = blueShockwaveBullet;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 1500;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = blueShockwaveBullet;
            int projectileDamage = 15;
            float projectileKnockback = 0.0f;
            int projectileLifetime = 10000;
            float projectileSpeed = 0.2f;
            int projectileBounces = -1;
            int projectilePierces = -1;
            List<Effect> projectileActiveEffects = new List<Effect>();
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.Spread = 3;
            weapon.SpreadAngle = (float)Math.PI * 2 / 7;
            weapon.ID = 19;
            weapon.Tooltip = "Travels through obstacles.";
            return weapon;
        }

        public Weapon CreateGrapple()
        {
            string name = "Grapple";
            Vector2 location = new Vector2(0, 0);
            int collisionRectangleLeniency = 5;
            Texture2D sprite = weaponGrapple1;
            float arc = 0f;
            float stabDistance = 0f;
            int damage = 0;
            float knockback = 0;
            int cooldown = 2500;
            int timeDisplayed = 0;
            List<Effect> activeEffects = new List<Effect>();
            List<Effect> onHitEffects = new List<Effect>();
            Texture2D projectileSprite = weaponGrapple1;
            int projectileDamage = 10;
            float projectileKnockback = 0f;
            int projectileLifetime = 1000;
            float projectileSpeed = 0.7f;
            int projectileBounces = 0;
            int projectilePierces = 0;
            List<Effect> projectileActiveEffects = new List<Effect>();
            projectileActiveEffects.Add(new Effect(EffectType.Grapple, 1000, 0.6f));
            onHitEffects.Add(new Effect(EffectType.Stun, 600, 1.0f));
            Weapon weapon = new Weapon(name, location, collisionRectangleLeniency, sprite, arc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            weapon.ID = 20;
            weapon.Tooltip = "Pulls enemies towards you";
            return weapon;
        }

    }

}
