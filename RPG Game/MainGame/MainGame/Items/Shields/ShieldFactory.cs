using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Items.Shields
{
    public class ShieldFactory
    {
        #region Properties
        Texture2D spriteShield1;
        Texture2D spriteShield2;
        Texture2D speedBoost1;
        Texture2D thunderStone;
        Texture2D blueShockwaveSprite;
        Texture2D elvenTrinket;
        #endregion

        #region Constructors
        public ShieldFactory(Texture2D spriteShield1, Texture2D spriteShield2, Texture2D speedBoost1, Texture2D thunderStone,
            Texture2D blueShockwaveSprite, Texture2D elvenTrinket)
        {
            this.spriteShield1 = spriteShield1;
            this.spriteShield2 = spriteShield2;
            this.speedBoost1 = speedBoost1;
            this.thunderStone = thunderStone;
            this.blueShockwaveSprite = blueShockwaveSprite;
            this.elvenTrinket = elvenTrinket;
        }
        #endregion

        public Shield CreateSpeedboost()
        {
            string name = "Boots of Speed";
            Texture2D sprite = speedBoost1;
            List<Effect> activeEffects = new List<Effect>();
            activeEffects.Add(new Effect(EffectType.Speed, 250, 2.0f));
            activeEffects.Add(new Effect(EffectType.Spin, 250, (float)Math.PI / 8));
            int cooldown = 1000;
            Shield shield = new Shield(name, sprite, activeEffects, cooldown);
            shield.ID = 0;
            return shield;
        }

        public Shield CreateBasicShield()
        {
            string name = "Iron Shield";
            Texture2D sprite = spriteShield1;
            List<Effect> activeEffects = new List<Effect>();
            activeEffects.Add(new Effect(EffectType.Unhittable, 500, 0f));
            int cooldown = 3000;
            Shield shield = new Shield(name, sprite, activeEffects, cooldown);
            shield.ID = 1;
            return shield;
        }

        public Shield CreateTowerShield()
        {
            string name = "Tower Shield";
            Texture2D sprite = spriteShield2;
            List<Effect> activeEffects = new List<Effect>();
            activeEffects.Add(new Effect(EffectType.Unhittable, 210, 0f));
            activeEffects.Add(new Effect(EffectType.Pacify, 210, 0f));
            activeEffects.Add(new Effect(EffectType.Speed, 210, 0.35f));
            int cooldown = 200;
            Shield shield = new Shield(name, sprite, activeEffects, cooldown);
            shield.ID = 2;
            shield.Tooltip = "You cannot attack while using this shield.";
            return shield;
        }

        public Shield CreateThunderStone()
        {
            string name = "Thunder Stone";
            Texture2D sprite = thunderStone;
            List<Effect> activeEffects = new List<Effect>();
            activeEffects.Add(new Effect(EffectType.Shockwave, 10, 1.0f, blueShockwaveSprite, 5, 400));
            int cooldown = 6000;
            Shield shield = new Shield(name, sprite, activeEffects, cooldown);
            shield.ID = 3;
            shield.Type = ShieldType.Close;
            shield.Tooltip = "Knocks all nearby enemies away from you.";
            return shield;
        }

        public Shield CreateSuperShield()
        {
            string name = "Odin's Shield";
            Texture2D sprite = spriteShield1;
            List<Effect> activeEffects = new List<Effect>();
            activeEffects.Add(new Effect(EffectType.Unhittable, 2000, 0f));
            int cooldown = 4000;
            return new Shield(name, sprite, activeEffects, cooldown);
        }

        public Shield CreateElvenTrinket()
        {
            string name = "Elven Trinket";
            Texture2D sprite = elvenTrinket;
            List<Effect> activeEffects = new List<Effect>();
            activeEffects.Add(new Effect(EffectType.Unhittable, 250, 0f));
            activeEffects.Add(new Effect(EffectType.Speed, 250, 2.0f));
            int cooldown = 1500;
            Shield shield = new Shield(name, sprite, activeEffects, cooldown);
            shield.Type = ShieldType.RandomDash;
            shield.ID = 4;
            return shield;
        }

        public Shield CreateBullrush()
        {
            string name = "Bullrush";
            Texture2D sprite = elvenTrinket;
            List<Effect> activeEffects = new List<Effect>();
            activeEffects.Add(new Effect(EffectType.Unhittable, 1000, 0f));
            activeEffects.Add(new Effect(EffectType.Speed, 1000, 1.5f));
            int cooldown = 6000;
            Shield shield = new Shield(name, sprite, activeEffects, cooldown);
            shield.Type = ShieldType.RandomDash;
            shield.ID = 4;
            return shield;
        }


    }

}
