using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    public class Effect
    {
        #region Properties
        EffectType type;
        int duration;
        float strength;
        bool isActive;
        int damage;
        int secondaryDuration;
        Texture2D sprite;
        Creature owner;

        //Accessors
        public EffectType Type
        {
            get { return type; }
        }
        public int Duration
        {
            get { return duration; }
        }
        public float Strength
        {
            get { return strength; }
        }
        public bool IsActive
        {
            get { return isActive; }
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public int SecondaryDuration
        {
            get { return secondaryDuration; }
        }
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        public Creature Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        #endregion

        #region Constructors
        public Effect()
        {

        }

        public Effect(EffectType type, int duration, float strength)
        {
            this.type = type;
            this.duration = duration;
            this.strength = strength;
            isActive = true;
        }

        /// <summary>
        /// This constructor is to only be used to create shockwave effects 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        /// <param name="strength"></param>
        /// <param name="sprite"></param>
        /// <param name="damage"></param>
        public Effect(EffectType type, int duration, float strength, Texture2D sprite, int damage, int secondaryDuration)
        {
            this.type = type;
            this.duration = duration;
            this.strength = strength;
            isActive = true;
            this.sprite = sprite;
            this.damage = damage;
            this.secondaryDuration = secondaryDuration;
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            if (duration != -1)
            {
                duration -= gameTime.ElapsedGameTime.Milliseconds;
                if (duration <= 0)
                    isActive = false;
            }
        }

        public void Destroy()
        {
            isActive = false;
        }

        public Effect Copy()
        {
            return new Effect(type, duration, strength, sprite, damage, secondaryDuration);
        }
    }
}
