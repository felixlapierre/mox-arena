using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MainGame.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame
{
    public class Entity
    {
        #region Properties
        protected string name { get; set; }
        protected Vector2 location;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }
        public float XLocation
        {
            get { return location.X; }
            set { location.X = value; }
        }
        public float YLocation
        {
            get { return location.Y; }
            set { location.Y = value; }
        }
        #endregion Properties

        #region Constructors
        public Entity()
        {
            this.name = "DefaultName";
            this.location = new Vector2(0, 0);
        }

        public Entity(string name, Vector2 location)
        {
            this.name = name;
            this.location = location;
        }
        #endregion
    }

    public class StaticEntity : Entity
    {
        #region Properties
        protected Rectangle drawRectangle;
        public Rectangle DrawRectangle
        {
            get { return drawRectangle; }
        }

        protected Rectangle collisionRectangle;
        public Rectangle CollisionRectangle
        {
            get { return collisionRectangle; }
        }
        int collisionRectangleLeniency;

        protected Texture2D sprite;
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        protected float angle = 0f;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        protected Rectangle sourceRectangle;
        public Rectangle SourceRectangle
        {
            get { return sourceRectangle; }
            set { sourceRectangle = value; }
        }

        protected int animationFrame;
        public int AnimationFrame
        {
            get { return animationFrame; }
            set { animationFrame = value; }
        }
        #endregion

        #region Constructors
        public StaticEntity()
        {
            drawRectangle = new Rectangle();
            collisionRectangle = new Rectangle();
            sprite = null;
        }

        /// Each class has two constructors: one allows for the collision rectangle to be smaller than the draw rectangle
        /// by the amount specified in collisionRectangleLeniency

        public StaticEntity(string name, Vector2 location, Texture2D sprite) : base(name, location)
        {
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height); //Removed - sprite.Height / 2
            collisionRectangle = new Rectangle((int)location.X - sprite.Width / 2, (int)location.Y - sprite.Height / 2, sprite.Width, sprite.Height);
            collisionRectangleLeniency = 0;
            sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            animationFrame = 0;
            this.sprite = sprite;
        }

        public StaticEntity(string name, Vector2 location, int collisionRectangleLeniency, Texture2D sprite) : base(name, location)
        {
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height); //Removed - sprite.Height / 2
            int collisionRectangleWidth = sprite.Width - collisionRectangleLeniency;
            int collisionRectangleHeight = sprite.Height - collisionRectangleLeniency;
            this.collisionRectangleLeniency = collisionRectangleLeniency;
            collisionRectangle = new Rectangle((int)location.X - collisionRectangleWidth / 2, (int)location.Y - collisionRectangleHeight / 2, collisionRectangleWidth, collisionRectangleHeight);
            sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            animationFrame = 0;
            this.sprite = sprite;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch, Color color, Vector2 offset)
        {
            //sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Vector2 origin = new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2);
            //origin.X += sourceRectangle.Width * animationFrame;
            Rectangle currentSourceRectangle = new Rectangle(sourceRectangle.Width * animationFrame, sourceRectangle.Y, sourceRectangle.Width, sourceRectangle.Height);
            Rectangle currentDrawRectangle = new Rectangle(drawRectangle.X + (int)offset.X, drawRectangle.Y + (int)offset.Y, drawRectangle.Width, drawRectangle.Height);
            spriteBatch.Draw(sprite, currentDrawRectangle, currentSourceRectangle, color, angle, origin, SpriteEffects.None, 1);
        }

        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Draw(spriteBatch, color, new Vector2(0, 0));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Draw(spriteBatch, Color.White, offset);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Color.White, new Vector2(0, 0));
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Vector2 origin = new Vector2(0, 0);
            spriteBatch.Draw(sprite, destinationRectangle, sourceRectangle, Color.White, angle, origin, SpriteEffects.None, 1);
        }       

        public void UpdateDrawRectangle()
        {
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height);
            sourceRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
        }

        public void UpdateDrawRectangleAnimated()
        {
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, sourceRectangle.Width, sourceRectangle.Height);
            collisionRectangle = new Rectangle(drawRectangle.X, drawRectangle.Y, drawRectangle.Width - collisionRectangleLeniency, drawRectangle.Height - collisionRectangleLeniency);
        }
    }

    public class GridEntity : StaticEntity
    {
        #region Properties
        protected int id;
        protected int x;
        protected int y;
        protected int z;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int XPosition
        {
            get { return x; }
            set { x = value; }
        }
        public int YPosition
        {
            get { return y; }
            set { y = value; }
        }
        public int ZPosition
        {
            get { return z; }
            set { z = value; }
        }
        #endregion

        #region Constructors
        public GridEntity()
        {

        }

        public GridEntity(string name, int XPosition, int YPosition, int ZPosition, Vector2 location, Texture2D sprite) : base(name, location, sprite)
        {
            x = XPosition;
            y = YPosition;
            z = ZPosition;
        }

        public GridEntity(string name, int XPosition, int YPosition, int ZPosition, Vector2 location, int collisionRectangleLeniency, Texture2D sprite) : base(name, location, collisionRectangleLeniency, sprite)
        {
            x = XPosition;
            y = YPosition;
            z = ZPosition;
        }
        #endregion

        public GridEntity Copy()
        {
            GridEntity copy = new GridEntity(name, XPosition, YPosition, ZPosition, location, drawRectangle.Width - collisionRectangle.Width, sprite);
            copy.ID = id;
            return copy;
        }

    }

    public class Spawner : GridEntity
    {
        #region Properties
        int enemyID;
        float distanceActivated;
        int delay;
        int quantity;
        int quantityRemaining;
        int delayRemaining;
        public int EnemyID
        {
            get { return enemyID; }
            set { enemyID = value; }
        }
        public float DistanceActivated
        {
            get { return distanceActivated; }
            set { distanceActivated = value; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }
        #endregion

        #region Constructors
        public Spawner()
        {

        }

        public Spawner(string name, int XPosition, int YPosition, int ZPosition, Vector2 location, Texture2D sprite, int enemyID, float distanceActivated, int quantity, int delay) : base(name, XPosition, YPosition, ZPosition, location, sprite)
        {            
            this.enemyID = enemyID;
            id = enemyID + 100;
            this.distanceActivated = distanceActivated;
            this.quantity = quantity;
            quantityRemaining = quantity;
            this.delay = delay;
            delayRemaining = delay;
        }

        public void Update(GameTime gameTime, Vector2 playerLocation, List<Enemy> enemies, EnemyFactory enemyFactory)
        {
            float distanceToPlayer = (float)Math.Sqrt(Math.Pow(playerLocation.X - location.X,2) + Math.Pow(playerLocation.Y - location.Y,2));
            if (quantityRemaining > 0 && distanceToPlayer <= distanceActivated)
            {
                if (delayRemaining > 0)
                    delayRemaining -= gameTime.ElapsedGameTime.Milliseconds;
                else
                {
                    delayRemaining = delay;
                    quantityRemaining -= 1;
                    Enemy enemy = enemyFactory.GetEnemyFromID(enemyID, location);
                    enemies.Add(enemy);
                }
            }
        }

        public Spawner Copy()
        {
            Spawner spawner = new Spawner(name, XPosition, YPosition, ZPosition, location, sprite, enemyID, distanceActivated, quantity, delay);
            spawner.ID = enemyID + 100;
            return spawner;
        }
        #endregion
    }

    public class HealthBar : StaticEntity
    {
        #region Properties
        const int distanceAboveCreature = 5;
        #endregion

        #region Constructors
        public HealthBar()
        {

        }

        public HealthBar(string name, Vector2 location, Texture2D sprite) : base(name, location, sprite)
        {
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, 50, sprite.Height);
        }
        #endregion

        public void Update(GameTime gameTime, Creature creature)
        {
            location.X = creature.Location.X - drawRectangle.Width / 2;
            location.Y = creature.Location.Y - creature.DrawRectangle.Height / 2 - distanceAboveCreature;
            drawRectangle.X = (int)location.X;
            drawRectangle.Y = (int)location.Y;
        }

        public void Draw(SpriteBatch spriteBatch, Creature creature, Vector2 offset)
        {
            int percentRemaining = (int)Math.Ceiling((decimal)creature.HitPoints / (decimal)creature.MaxHitPoints * 100);
            int x = 50 - percentRemaining / 2;
            int y = 0;
            Rectangle currentDrawRectangle = new Rectangle(drawRectangle.X + (int)offset.X, drawRectangle.Y + (int)offset.Y, drawRectangle.Width, drawRectangle.Height);
            spriteBatch.Draw(sprite, currentDrawRectangle, new Rectangle(x, y, 50, drawRectangle.Height), Color.White);
        }
    }

    public class ItemBox : StaticEntity
    {
        #region Properties
        ItemType type;
        Weapon weapon;
        Shield shield;
        Charm charm;
        bool hovered;
        bool isActive = true;

        public ItemType Type
        {
            get { return type; }
        }
        public Weapon Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }
        public Shield Shield
        {
            get { return shield; }
            set { shield = value; }
        }
        public Charm Charm
        {
            get { return charm; }
            set { charm = value; }
        }
        public bool IsHovered
        {
            get { return hovered; }
            set { hovered = value; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        #endregion

        #region Constructors
        public ItemBox(string name, Vector2 location)
        {
            Name = name;
            Location = location;
            hovered = false;
            isActive = false;
        }
        public ItemBox(string name, Vector2 location, Texture2D sprite, Weapon weapon) : base(name, location, sprite)
        {
            type = ItemType.Weapon;
            this.weapon = weapon;
            hovered = false;
        }
        public ItemBox(string name, Vector2 location, Texture2D sprite, Shield shield) : base(name, location, sprite)
        {
            type = ItemType.Shield;
            this.shield = shield;
            hovered = false;
        }
        public ItemBox(string name, Vector2 location, Texture2D sprite, Charm charm) : base(name, location, sprite)
        {
            type = ItemType.Charm;
            this.charm = charm;
            hovered = false;
        }

        #endregion

        public void Update()
        {
            if (!isActive)
                return;
            if (type == ItemType.Weapon && weapon != null)
            {
                sprite = weapon.Sprite;
                drawRectangle = new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height);
            }
            else if (type == ItemType.Shield && shield != null)
            {
                sprite = shield.Sprite;
                drawRectangle = new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height);
            }
            else if (type == ItemType.Charm && shield != null)
            {
                sprite = charm.Sprite;
                drawRectangle = new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height);
            }

        }

        public void ReplaceItem(Weapon weapon)
        {
            type = ItemType.Weapon;
            this.weapon = weapon;
            sprite = weapon.Sprite;
            isActive = true;
            if (sprite != null)
                UpdateDrawRectangle();

        }
        public void ReplaceItem(Shield shield)
        {
            type = ItemType.Shield;
            this.shield = shield;
            sprite = shield.Sprite;
            isActive = true;
            if (sprite != null)
                UpdateDrawRectangle();

        }
        public void ReplaceItem(Charm charm)
        {
            type = ItemType.Charm;
            this.charm = charm.Copy();
            sprite = charm.Sprite;
            isActive = true;
            if (sprite != null)
                UpdateDrawRectangle();
        }

        public new void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (!isActive)
                return;
            if (type == ItemType.Charm)
                Draw(spriteBatch, charm.Color, offset);
            base.Draw(spriteBatch, offset);
        }
    }

    public class DynamicEntity : StaticEntity
    {
        #region Properties
        protected Vector2 velocity;
        public float XVelocity
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }
        public float YVelocity
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }

        protected int z;
        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        const int BaseAnimationInterval = 100;
        int currentAnimationInterval;

        bool noclip;
        public bool Noclip
        {
            get { return noclip; }
        }

        protected Rectangle previousCollisionRectangle;
        public Rectangle PreviousCollisionRectangle
        {
            get { return previousCollisionRectangle; }
        }

        List<Effect> activeEffects = new List<Effect>();
        public List<Effect> ActiveEffects
        {
            get { return activeEffects; }
        }
        float spinAngle = 0f;

        //Effects logic
        bool poisoned;
        bool slowed;
        bool invulnerable;
        bool unhittable;
        bool pacified;
        bool stunned;
        const int poisonTimerMax = 250;
        int poisonTimerRemaining;
        float poisonDamageThisTick;

        public bool Poisoned
        {
            get { return poisoned; }
            set { poisoned = value; }
        }
        public bool Slowed
        {
            get { return slowed; }
        }
        public float PoisonDamage
        {
            get { return poisonDamageThisTick; }
        }
        public bool Invulnerable
        {
            get { return invulnerable; }
        }
        public bool Unhittable
        {
            get { return unhittable; }
        }
        public bool Pacified
        {
            get { return pacified; }
        }
        public bool Stunned
        {
            get { return stunned; }
        }


        #endregion

        #region Constructors
        //Base constructor
        public DynamicEntity()
        {
            this.velocity = new Vector2(0, 0);
            activeEffects = new List<Effect>();
        }
        //Constructor that uses sprite to determine collision rectangle
        public DynamicEntity(string name, Vector2 location, Texture2D sprite, Vector2 velocity) : base(name, location, sprite)
        {
            this.velocity = velocity;
            activeEffects = new List<Effect>();
            currentAnimationInterval = BaseAnimationInterval;
        }
        //Constructor that allows leniency in collision rectangle
        public DynamicEntity(string name, Vector2 location, int collisionRectangleLeniency, Texture2D sprite, Vector2 velocity) : base(name, location, collisionRectangleLeniency, sprite)
        {
            this.velocity = velocity;
            activeEffects = new List<Effect>();
            currentAnimationInterval = BaseAnimationInterval;
        }
        #endregion

        public void SetVelocity(Vector2 newVelocity)
        {
            XVelocity = newVelocity.X;
            YVelocity = newVelocity.Y;
        }

        public void Update(GameTime gameTime)
        {

            #region Effects
            //Apply effects
            List<Effect> expiredEffects = new List<Effect>();
            //Speed
            float speedModifier = 1f;

            slowed = false;
            poisoned = false;
            invulnerable = false;
            unhittable = false;
            pacified = false;
            noclip = false;
            stunned = false;

            if (poisonTimerRemaining > 0)
                poisonTimerRemaining -= gameTime.ElapsedGameTime.Milliseconds;
            float poisonDamage = 0;
            foreach (Effect effect in activeEffects)
            {
                if (effect.IsActive)
                {
                    effect.Update(gameTime);
                    switch (effect.Type)
                    {
                        case EffectType.Speed:
                            speedModifier = speedModifier * effect.Strength;
                            break;
                        case EffectType.Spin:
                            spinAngle += effect.Strength;
                            break;
                        case EffectType.Poison:
                            poisoned = true;
                            if (poisonTimerRemaining <= 0 && poisonDamage < effect.Strength)
                                poisonDamage = effect.Strength;
                            break;
                        case EffectType.Invulnerable:
                            invulnerable = true;
                            break;
                        case EffectType.Noclip:
                            noclip = true;
                            break;
                        case EffectType.Unhittable:
                            unhittable = true;
                            break;
                        case EffectType.Pacify:
                            pacified = true;
                            break;
                        case EffectType.Stun:
                            stunned = true;
                            break;
                    }
                }
                else
                    expiredEffects.Add(effect);
            }
            if (poisonDamage > 0)
            {
                poisonTimerRemaining = poisonTimerMax;
                poisonDamageThisTick = poisonDamage * ((float)poisonTimerMax / (float)1000);
            }
            else
                poisonDamageThisTick = 0;
            if (speedModifier < 1)
                slowed = true;
            foreach (Effect effect in expiredEffects)
            {
                if (effect.Type == EffectType.Spin)
                    spinAngle = 0;
                ActiveEffects.Remove(effect);
            }
            #endregion

            //Update the location of the entity
            location.X += velocity.X * speedModifier * gameTime.ElapsedGameTime.Milliseconds;
            location.Y += velocity.Y * speedModifier * gameTime.ElapsedGameTime.Milliseconds;
            if (velocity == new Vector2(0, 0) || sourceRectangle.Width == sprite.Width)
                animationFrame = 0;
            else if (currentAnimationInterval <= 0)
            {
                currentAnimationInterval = (int)((float)BaseAnimationInterval / speedModifier);
                if (animationFrame < (sprite.Width / SourceRectangle.Width) - 1)
                    animationFrame += 1;
                else
                    animationFrame = 1;
            }
            else
            {
                currentAnimationInterval -= gameTime.ElapsedGameTime.Milliseconds;
            }

            #region Determine the angle of movement
            float pi = (float)Math.PI;
            if (velocity.X > 0)
            {
                if (velocity.Y < 0)
                    angle = pi / 2 + (float)Math.Atan2(velocity.Y, velocity.X); //pi / 4;
                if (velocity.Y == 0)
                    angle = pi / 2;
                if (velocity.Y > 0)
                    angle = pi / 2 + (float)Math.Atan2(velocity.Y, velocity.X);
            }
            else if (velocity.X == 0)
            {
                if (velocity.Y < 0)
                    angle = 0f;
                if (velocity.Y > 0)
                    angle = pi;
            }
            else if (velocity.X < 0)
            {
                if (velocity.Y > 0)
                    angle = pi / 2 + (float)Math.Atan2(velocity.Y, velocity.X);
                if (velocity.Y == 0)
                    angle = 3 * pi / 2;
                if (velocity.Y < 0)
                    angle = pi / 2 + (float)Math.Atan2(velocity.Y, velocity.X); //7 * pi / 4;
            }
            angle += spinAngle;
            #endregion

            //Update the draw and collision rectangles
            previousCollisionRectangle = CollisionRectangle;
            UpdateRectangle();
        }

        public void Update(GameTime gameTime, List<Projectile> createdProjectiles)
        {
            foreach (Effect effect in activeEffects)
            {
                if (effect.IsActive == false && effect.Type == EffectType.Shockwave)
                {
                    for (float i = 0; i <= Math.PI * 2; i += (float)Math.PI / 16)
                    {
                        List<Effect> activeEffects = new List<Effect>();
                        List<Effect> onHitEffects = new List<Effect>();
                        activeEffects.Add(new Effect(EffectType.Spin, 1000, (float)Math.PI / 15));
                        //Create velocity vector
                        Vector2 velocity = new Vector2(0, 0);
                        velocity.X = 0.40f * (float)Math.Cos(i);
                        velocity.Y = 0.40f * (float)Math.Sin(i);
                        createdProjectiles.Add(new Projectile("ShockwaveBullet", location, 5, effect.Sprite, velocity, null, effect.Damage, effect.SecondaryDuration, effect.Strength, -1, 20, activeEffects, onHitEffects));
                    }
                    effect.Destroy();
                }
            }
        }

        public void UpdateRectangle()
        {
            drawRectangle.X = (int)location.X;
            drawRectangle.Y = (int)location.Y;
            collisionRectangle.X = (int)location.X - collisionRectangle.Width / 2;
            collisionRectangle.Y = (int)location.Y - collisionRectangle.Height / 2;
        }
    }

    public class Creature : DynamicEntity
    {
        #region Properties
        const int InitialInvincibilityFrames = 500;
        int invincibilityFramesRemaining = 0;

        protected float hitPoints = 0;
        protected float maxHitPoints;

        public Vector2 knockbackVelocity = new Vector2(0, 0);
        const float KnockbackVelocityDecay = 0.01f;

        HealthBar healthBar;
        Vector2 desiredVelocity;
        float baseSpeed;
        Weapon weapon1;
        Weapon weapon2;
        Shield shield1;
        Charm charm1;

        //Accessors
        public float HitPoints
        {
            get { return hitPoints; }
            set { hitPoints = value; }
        }
        public float MaxHitPoints
        {
            get { return maxHitPoints; }
            set { maxHitPoints = value; }
        }
        public int InvincibilityFramesRemaining
        {
            get { return invincibilityFramesRemaining; }
        }
        public HealthBar HealthBar
        {
            get { return healthBar; }
        }
        public Vector2 DesiredVelocity
        {
            get { return desiredVelocity; }
            set { desiredVelocity = value; }
        }
        public Vector2 KnockbackVelocity
        {
            get { return knockbackVelocity; }
            set { knockbackVelocity = value; }
        }
        public float BaseSpeed
        {
            get { return baseSpeed; }
            set { baseSpeed = value; }
        }
        public Weapon Weapon1
        {
            get { return weapon1; }
            set { weapon1 = value; }
        }
        public Weapon Weapon2
        {
            get { return weapon2; }
            set { weapon2 = value; }
        }
        public Shield Shield1
        {
            get { return shield1; }
            set { shield1 = value; }
        }
        public Charm Charm1
        {
            get { return charm1; }
            set { charm1 = value; }
        }
        #endregion

        #region Constructors
        public Creature()
        {

        }
        public Creature(string name, Vector2 location, int collisionRectangleLeniency, Texture2D sprite, Vector2 velocity, float hitPoints, Texture2D healthBarSprite, Weapon weapon1, Weapon weapon2, Shield shield1, Charm charm1, float baseSpeed) : base(name, location, collisionRectangleLeniency, sprite, velocity)
        {
            this.hitPoints = hitPoints;
            maxHitPoints = hitPoints;
            healthBar = new HealthBar("Health Bar", location, healthBarSprite);
            Weapon1 = weapon1;
            Weapon2 = weapon2;
            Shield1 = shield1;
            Charm1 = charm1;
            this.baseSpeed = baseSpeed;
        }
        #endregion

        public new void Update(GameTime gameTime)
        {
            if (hitPoints <= 0)
                //Don't update if the creature is dead :(
                return;

            float totalSpeedModifier = 1.0f;

            desiredVelocity.X = desiredVelocity.X * totalSpeedModifier;
            desiredVelocity.Y = desiredVelocity.Y * totalSpeedModifier;
            SetVelocity(new Vector2(desiredVelocity.X, desiredVelocity.Y));

            base.Update(gameTime);

            if (invincibilityFramesRemaining > 0)
                invincibilityFramesRemaining -= gameTime.ElapsedGameTime.Milliseconds;
            if (knockbackVelocity.Length() > 0)
            {
                knockbackVelocity.X = UpdateKnockbackVelocity(knockbackVelocity.X);
                knockbackVelocity.Y = UpdateKnockbackVelocity(knockbackVelocity.Y);
            }
            if (PoisonDamage > 0 && !Invulnerable)
                hitPoints -= PoisonDamage;

            //Update health bar
            healthBar.Update(gameTime, this);
        }

        public new void Update(GameTime gameTime, List<Projectile> projectiles)
        {
            Update(gameTime);
            base.Update(gameTime, projectiles);
        }

        public new void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            Color color = Color.White;
            Random random = new Random();
            if (Invulnerable)
            {
                if (random.Next(0, 2) == 0)
                    color = Color.TransparentBlack;
            }
            else if (Unhittable)
            {
                if (random.Next(0, 2) == 0)
                    color = Color.TransparentBlack;
            }
            else if (Stunned)
            {
                if (random.Next(0, 2) == 0)
                    color = Color.Gold;
            }
            else if (invincibilityFramesRemaining > 0)
                color = Color.Firebrick;
            else if (Slowed)
            {
                if (Poisoned)
                    color = Color.Aquamarine;
                else
                    color = Color.DeepSkyBlue;
            }
            else if (Poisoned)
                color = Color.DarkSeaGreen;

            Draw(spriteBatch, color, offset);
            if (hitPoints > 0)
                healthBar.Draw(spriteBatch, this, offset);
        }

        private float UpdateKnockbackVelocity(float speed)
        {
            if (Math.Abs(speed) < KnockbackVelocityDecay)
                speed = 0;
            else if (speed < 0)
                speed += KnockbackVelocityDecay;
            else if (speed > 0)
                speed -= KnockbackVelocityDecay;
            return speed;
        }

        public void HitByAttack(Weapon weapon)
        {
            TakeDamage(weapon.Damage, weapon.UserLocation, weapon.Knockback);
            foreach (Effect effect in weapon.OnHitEffects)
            {
                ActiveEffects.Add(effect.Copy());
            }
        }

        public void HitByAttack(Projectile projectile)
        {
            if (Unhittable)
                return;
            //Calculate an attack location so that the creature will be pushed based on the
            //velocity of the projectile, not its location
            Vector2 attackLocation = new Vector2(location.X - projectile.XVelocity, location.Y - projectile.YVelocity);
            TakeDamage(projectile.Damage, attackLocation, projectile.Knockback);
            foreach (Effect effect in projectile.ActiveEffects)
            {
                if (effect.Type == EffectType.Grapple)
                {
                    float angleOfHit = (float)Math.Atan2(location.Y - projectile.Owner.Location.Y, location.X - projectile.Owner.Location.X) + (float)Math.PI;
                    knockbackVelocity = new Vector2(effect.Strength * (float)Math.Cos(angleOfHit), effect.Strength * (float)Math.Sin(angleOfHit));
                }
            }
            foreach (Effect effect in projectile.OnHitEffects)
            {
                ActiveEffects.Add(effect.Copy());
            }
        }

        private void TakeDamage(float damage, Vector2 attackLocation, float knockback)
        {
            if (invincibilityFramesRemaining <= 0 && !Invulnerable)
            {
                hitPoints -= damage;
                if (damage != 0)
                    invincibilityFramesRemaining = InitialInvincibilityFrames;
                float angleOfHit = (float)Math.Atan2(location.Y - attackLocation.Y, location.X - attackLocation.X);
                knockbackVelocity = new Vector2(knockback * (float)Math.Cos(angleOfHit), knockback * (float)Math.Sin(angleOfHit));
            }
        }

        public void Heal(float healAmount)
        {
            hitPoints += healAmount;
            if (hitPoints > MaxHitPoints)
                hitPoints = MaxHitPoints;
        }

        public void ApplyCharmEffects()
        {
            if (charm1 == null)
                return;

            switch (charm1.Type)
            {
                case CharmType.Burst:
                    weapon1.Burst += (int)Math.Floor(charm1.Strength);
                    if (weapon2 != null)
                        weapon2.Burst += (int)Math.Floor(charm1.Strength);
                    break;
                case CharmType.HigherCooldown:
                    weapon1.Damage = (int)((float)weapon1.Damage * charm1.Strength);
                    weapon1.ProjectileDamage = (int)((float)weapon1.ProjectileDamage * charm1.Strength);
                    weapon1.Cooldown = (int)((float)weapon1.Cooldown * charm1.Strength);
                    if (weapon2 != null)
                    {
                        weapon2.Damage = (int)((float)weapon2.Damage * charm1.Strength);
                        weapon2.ProjectileDamage = (int)((float)weapon2.ProjectileDamage * charm1.Strength);
                        weapon2.Cooldown = (int)((float)weapon2.Cooldown * charm1.Strength);
                    }
                    break;
                case CharmType.LowerCooldown:
                    weapon1.Damage = (int)((float)weapon1.Damage / charm1.Strength);
                    weapon1.ProjectileDamage = (int)((float)weapon1.ProjectileDamage / charm1.Strength);
                    weapon1.Cooldown = (int)((float)weapon1.Cooldown / charm1.Strength);
                    if (weapon2 != null)
                    {
                        weapon2.Damage = (int)((float)weapon2.Damage / charm1.Strength);
                        weapon2.ProjectileDamage = (int)((float)weapon2.ProjectileDamage / charm1.Strength);
                        weapon2.Cooldown = (int)((float)weapon2.Cooldown / charm1.Strength);
                    }
                    break;
                case CharmType.Speed:
                    baseSpeed = baseSpeed * charm1.Strength;
                    break;
                default:
                    break;
            }
        }

    }

    public class Player : Creature
    {
        #region Properties
        const int playerCollisionBoxLeniency = 5;
        #endregion

        #region Constructors
        public Player()
        {

        }

        public Player(string name, Vector2 location, Texture2D sprite, Vector2 velocity, float hitPoints, Texture2D healthBarSprite, Weapon weapon1, Weapon weapon2, Shield shield1, Charm charm1, float baseSpeed) : base(name, location, playerCollisionBoxLeniency, sprite, velocity, hitPoints, healthBarSprite, weapon1, weapon2, shield1, charm1, baseSpeed)
        {
            SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            UpdateDrawRectangleAnimated();
        }
        #endregion

        public void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse, List<Projectile> friendlyProjectiles, Vector2 offset)
        {
            if (hitPoints > 0)
            {
                #region Determine speed based on keyboard
                double xSpeed = 0;
                double ySpeed = 0;
                double Root2on2 = Math.Sqrt(2) / 2;
                if (!Stunned)
                {
                    if (keyboard.IsKeyDown(Keys.W))
                    {
                        ySpeed = -1;
                    }
                    else if (keyboard.IsKeyDown(Keys.S))
                    {
                        ySpeed = 1;
                    }
                    if (keyboard.IsKeyDown(Keys.D))
                    {
                        if (ySpeed == 0)
                        {
                            xSpeed = 1;
                        }

                        else
                        {
                            ySpeed = ySpeed * Root2on2;
                            xSpeed = Root2on2;
                        }
                    }
                    else if (keyboard.IsKeyDown(Keys.A))
                    {
                        if (ySpeed == 0)
                            xSpeed = -1;
                        else
                        {
                            ySpeed = ySpeed * Root2on2;
                            xSpeed = -1 * Root2on2;
                        }
                    }
                }
                DesiredVelocity = new Vector2((float)xSpeed * BaseSpeed, (float)(ySpeed) * BaseSpeed) + knockbackVelocity;
                #endregion

                Vector2 target = new Vector2(mouse.X - offset.X, mouse.Y - offset.Y);
                bool weapon1Used = false;
                bool weapon2Used = false;
                Shield1.IsActive = false;
                if (!Pacified && !Stunned)
                {
                    if (mouse.LeftButton == ButtonState.Pressed)
                        weapon1Used = true;
                    if (mouse.RightButton == ButtonState.Pressed)
                        weapon2Used = true;
                }

                if (keyboard.IsKeyDown(Keys.Space))
                {
                    Shield1.IsActive = true;
                    foreach (Effect effect in Shield1.ActiveEffects)
                    {
                        if (effect.Type == EffectType.Pacify && (Weapon1.TimeDisplayedRemaining > 0 || Weapon2.TimeDisplayedRemaining > 0))
                            Shield1.IsActive = false;
                    }
                }

                Weapon1.Update(gameTime, this, target, weapon1Used, friendlyProjectiles);
                Weapon2.Update(gameTime, this, target, weapon2Used, friendlyProjectiles);
                Shield1.Update(gameTime, this, target);
            }
            Update(gameTime, friendlyProjectiles);
        }

        public new void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);
            Weapon1.Draw(spriteBatch, offset);
            Weapon2.Draw(spriteBatch, offset);
        }
    }

    public class Enemy : Creature
    {
        #region Properties
        Pathfinding pathfinder;
        Path currentPath;
        int pathfindingInterval = 400;
        int randomPathfindingInterval = 200;
        int pathfindingIntervalRemaining = 0;
        float randomDashAngle;
        int maxAttackDelay;
        int remainingAttackDelay;

        public Pathfinding Pathfinder
        {
            get { return pathfinder; }
            set { pathfinder = value; }
        }
        public int RemainingAttackDelay;
        #endregion

        #region Constructors
        public Enemy()
        {

        }

        public Enemy(string name, Vector2 location, int collisionRectangleLeniency, Texture2D sprite, Vector2 velocity, float hitPoints, Texture2D healthBarSprite, Weapon weapon1, Weapon weapon2, Shield shield1, Charm charm1, float baseSpeed, Pathfinding pathfinder) : base(name, location, collisionRectangleLeniency, sprite, velocity, hitPoints, healthBarSprite, weapon1, weapon2, shield1, charm1, baseSpeed)
        {
            this.pathfinder = pathfinder;
            ApplyCharmEffects();
            maxAttackDelay = 500;
        }
        #endregion

        public void Update(GameTime gameTime, Player player1, List<GridEntity> obstacles, List<Projectile> projectiles, List<Projectile> hostileProjectiles)
        {
            Random random = new Random();
            bool randomDash = false;
            bool shielding = false;
            foreach (Effect effect in ActiveEffects)
            {
                if (effect.Type == EffectType.Unhittable)
                    shielding = true;
            }

            #region Shielding
            if (Shield1 != null)
            {
                Shield1.IsActive = false;
                if (Shield1.Type == ShieldType.Blocking || Shield1.Type == ShieldType.RandomDash)
                {
                    bool projectileDodge = false;
                    foreach (Projectile projectile in hostileProjectiles)
                    {
                        if (pathfinder.DistanceBetween(projectile.Location, location) < 75)
                            projectileDodge = true;
                    }
                    if ((pathfinder.DistanceBetween(player1.Weapon1.Location, location) < 100 && player1.Weapon1.IsActive)
                        || (pathfinder.DistanceBetween(player1.Weapon2.Location, location) < 100 && player1.Weapon2.IsActive)
                        || projectileDodge)
                    {
                        Shield1.IsActive = true;
                        if (Shield1.Type == ShieldType.RandomDash && !shielding)
                            randomDashAngle = (float)(Math.PI * 2 * random.NextDouble());
                    }
                }
                if (Shield1.Type == ShieldType.RandomDash && shielding)
                {
                    randomDash = true;
                }

                Shield1.Update(gameTime, this, player1.Location);
            }
            #endregion

            #region Pathfinding
            Vector2 target = new Vector2(0, 0);
            Vector2 velocityToTarget = new Vector2(0, 0);
            if (randomDash)
            {
                float angleToTarget = randomDashAngle;
                velocityToTarget = new Vector2((float)Math.Cos(angleToTarget) * BaseSpeed, (float)Math.Sin(angleToTarget) * BaseSpeed);
            }
            else if (pathfinder.Type != PathfinderType.StraightLine && !Stunned)
            {
                //Get path from pathfinder provided
                if (pathfindingIntervalRemaining > 0)
                    pathfindingIntervalRemaining -= gameTime.ElapsedGameTime.Milliseconds;
                else
                {
                    currentPath = pathfinder.CreatePath(location, player1.Location, obstacles);
                    pathfindingIntervalRemaining = pathfindingInterval + random.Next(0, randomPathfindingInterval);
                }

                //Determine the target towards which the creature will walk
                int tileSize = pathfinder.TileSize;

                //If the next node in the path has already been reached, delete it
                //To determine if it has been reached, the enemy's whole collision rectangle should be inside
                if (currentPath.Locations.Count > 0)
                {
                    float nextNodeCenterX = currentPath.nextLocationCoordinates(tileSize).X;
                    float nextNodeCenterY = currentPath.nextLocationCoordinates(tileSize).Y;
                    //Consider turning halfCollisionRectWidth and stuff into properties
                    int partialCollisionRectWidth = collisionRectangle.Width / 2;
                    int partialCollisionRectHeight = collisionRectangle.Height / 2;
                    if (XLocation < nextNodeCenterX + partialCollisionRectWidth &&
                        XLocation > nextNodeCenterX - partialCollisionRectWidth &&
                        YLocation < nextNodeCenterY + partialCollisionRectHeight &&
                        YLocation > nextNodeCenterY - partialCollisionRectHeight)
                    {
                        currentPath.Locations.RemoveAt(0);
                    }
                }

                //If the path is not empty, then the next node is the target
                if (currentPath.Locations.Count != 0)
                {
                    target = currentPath.nextLocationCoordinates(tileSize);
                    float angleToTarget = (float)Math.Atan2(target.Y - location.Y, target.X - location.X);
                    velocityToTarget = new Vector2((float)Math.Cos(angleToTarget) * BaseSpeed, (float)Math.Sin(angleToTarget) * BaseSpeed);
                }
            }
            else if (!Stunned)
            {
                target = player1.Location;
                float angleToTarget = (float)Math.Atan2(target.Y - location.Y, target.X - location.X);
                velocityToTarget = new Vector2((float)Math.Cos(angleToTarget) * BaseSpeed, (float)Math.Sin(angleToTarget) * BaseSpeed);
            }

            //Create velocity vector to target and move along it at base speed

            DesiredVelocity = velocityToTarget + knockbackVelocity; //Weirdness about knockback velocity accessibility level
            #endregion

            #region Attacking
            UseWeapon(gameTime, player1, obstacles, projectiles, Weapon1);
            UseWeapon(gameTime, player1, obstacles, projectiles, Weapon2);
            #endregion

            Update(gameTime, projectiles);
        }

        public void UseWeapon(GameTime gameTime, Player player1, List<GridEntity> obstacles, List<Projectile> projectiles, Weapon weapon)
        {
            bool weaponInUse = false;
            if (!Pacified && !Stunned && weapon.CooldownRemaining <= 0)
            {
                if (weapon.Type == WeaponType.Ranged &&
                    pathfinder.LineOfSight(location, player1.Location, obstacles, weapon.ProjectileSprite.Width * 2)
                    || weapon.Type == WeaponType.Ranged && weapon.ProjectileBounces == -1)
                    weaponInUse = true;
                else
                {
                    float distanceToPlayer;
                    float xDistance = player1.XLocation - XLocation;
                    float yDistance = player1.YLocation - YLocation;
                    distanceToPlayer = (float)Math.Sqrt((Math.Pow(xDistance, 2) + Math.Pow(yDistance, 2)));
                    if (weapon.Type == WeaponType.Melee && distanceToPlayer <= weapon.CollisionRectangle.Height + weapon.StabDistance)
                        weaponInUse = true;
                }
            }
            if (remainingAttackDelay > 0 && weaponInUse)
            {
                remainingAttackDelay -= gameTime.ElapsedGameTime.Milliseconds;
                if (remainingAttackDelay <= 0)
                    weaponInUse = true;
                else
                    weaponInUse = false;
            }            
            else if (weapon.CooldownRemaining <= 0 && weaponInUse && remainingAttackDelay <= 0)
            {
                Random random = new Random();
                remainingAttackDelay = random.Next(maxAttackDelay);
                weaponInUse = false;
            }
            
            weapon.Update(gameTime, this, player1.Location, weaponInUse, projectiles);
        }

        public new void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            base.Draw(spriteBatch, offset);
            Weapon1.Draw(spriteBatch, offset);
            if (pathfinder != null)
                pathfinder.Draw(spriteBatch);
        }

        public Enemy Copy()
        {
            Enemy copy = new Enemy(name, location, 5, sprite, velocity, hitPoints, HealthBar.Sprite, Weapon1.Copy(), Weapon2.Copy(), Shield1.Copy(), Charm1.Copy(), BaseSpeed, pathfinder.Copy());
            foreach (Effect effect in ActiveEffects)
                copy.ActiveEffects.Add(effect.Copy());
            copy.SourceRectangle = SourceRectangle;
            copy.UpdateDrawRectangleAnimated();
            return copy;
        }
    }

    public class EnemyFactory
    {
        #region Properties
        Texture2D healthBarSprite;
        Texture2D enemySprite1;
        Texture2D enemyGladiator1;
        Texture2D enemyGladiator2;
        Texture2D enemyGladiator2Large;
        Texture2D enemyGoblin1;
        Texture2D enemySkeleton1;
        Texture2D enemySkeleton2;
        Texture2D enemyFireSpider1;
        Texture2D enemyBlueSpider1;

        WeaponFactory weaponFactory;
        ShieldFactory shieldFactory;
        CharmFactory charmFactory;
        Pathfinding pathfinder;
        public Pathfinding Pathfinder
        {
            get { return pathfinder; }
            set { pathfinder = value; }
        }

        Weapon blankW = new Weapon();
        Shield blankS = new Shield();
        Charm blankC = new Charm();
        #endregion

        #region Constructors
        public EnemyFactory(WeaponFactory weaponFactory, ShieldFactory shieldFactory, CharmFactory charmFactory, Pathfinding pathfinder, Texture2D healthBarSprite,
            Texture2D enemySprite1, Texture2D enemyGladiator1, Texture2D enemyGladiator2, Texture2D enemyGladiator2Large,
            Texture2D enemyGoblin1, Texture2D enemySkeleton1, Texture2D enemySkeleton2, Texture2D enemyFireSpider1, Texture2D enemyBlueSpider1)
        {
            this.weaponFactory = weaponFactory;
            this.shieldFactory = shieldFactory;
            this.charmFactory = charmFactory;
            this.pathfinder = pathfinder;

            this.healthBarSprite = healthBarSprite;
            this.enemySprite1 = enemySprite1;
            this.enemyGladiator1 = enemyGladiator1;
            this.enemyGladiator2 = enemyGladiator2;
            this.enemyGladiator2Large = enemyGladiator2Large;
            this.enemyGoblin1 = enemyGoblin1;
            this.enemySkeleton1 = enemySkeleton1;
            this.enemySkeleton2 = enemySkeleton2;
            this.enemyFireSpider1 = enemyFireSpider1;
            this.enemyBlueSpider1 = enemyBlueSpider1;
        }
        #endregion

        public Enemy GetEnemyFromID(int id, Vector2 location)
        {
            Enemy enemy = new Enemy();
            switch (id)
            {
                case 1:
                default:
                    enemy = CreateSwordsman(location);
                    break;
                case 2:
                    enemy = CreateGoblinMauler(location);
                    break;
                case 3:
                    enemy = CreateSkeletonArcher(location);
                    break;
                case 4:
                    enemy = CreateBarbarian(location);
                    break;
                case 5:
                    enemy = CreateSpearman(location);
                    break;
                case 6:
                    enemy = CreateHeavy(location);
                    break;
                case 7:
                    enemy = CreateSpearThrower(location);
                    break;
                case 8:
                    enemy = CreateGiant(location);
                    break;
                case 9:
                    enemy = CreateGoblinArcanist(location);
                    break;
                case 10:
                    enemy = CreateBotengNinja(location);
                    break;
                case 11:
                    enemy = CreateHiraNinja(location);
                    break;
                case 12:
                    enemy = CreateTaagoNinja(location);
                    break;
                case 13:
                    enemy = CreateFireSpider(location);
                    break;
                case 14:
                    enemy = CreatePlasmaSpider(location);
                    break;
                case 15:
                    enemy = CreateGiantGrappler(location);
                    break;
                case 16:
                    enemy = CreateFrostArcher(location);
                    break;
                case 17:
                    enemy = CreateDaggerTosser(location);
                    break;
                case 18:
                    enemy = CreateJungleSpearman(location);
                    break;
            }
            return enemy;
        }

        public Enemy CreateSwordsman(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemySprite1;
            int hitPoints = 40;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateSword();
            enemy = new Enemy("Swordsman", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreateSkeletonArcher(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemySkeleton1;
            int hitPoints = 40;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateBow();
            enemy = new Enemy("Skeleton Archer", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreateGoblinMauler(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGoblin1;
            int hitPoints = 40;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateMaul();
            enemy = new Enemy("Goblin Mauler", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreateBarbarian(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateThrowingAxe();
            enemy = new Enemy("Barbarian", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            return enemy;
        }

        public Enemy CreateSpearman(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateSpear();
            enemy = new Enemy("Spearman", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreateHeavy(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator2;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateHammer();
            enemy = new Enemy("Heavy", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreateSpearThrower(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator2;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateThrowingSpear();
            enemy = new Enemy("Spear Thrower", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            return enemy;
        }

        public Enemy CreateGiant(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator2Large;
            int hitPoints = 100;
            float baseSpeed = 0.20f;
            Weapon weapon = weaponFactory.CreateHelsingor();
            Shield shield = shieldFactory.CreateBasicShield();
            enemy = new Enemy("Giant", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, shield, blankC, baseSpeed, pathfinder.Copy());
            enemy.Pathfinder.Type = PathfinderType.StraightLine;
            enemy.ActiveEffects.Add(new Effect(EffectType.Noclip, -1, 1.0f));
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreateGoblinArcanist(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGoblin1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateFirebolt();
            enemy = new Enemy("Arcanist", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            return enemy;
        }

        public Enemy CreateBotengNinja(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemySprite1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateBoteng();
            enemy = new Enemy("Boteng Ninja", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            enemy.Shield1 = shieldFactory.CreateElvenTrinket();
            return enemy;
        }

        public Enemy CreateHiraNinja(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemySprite1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateHira();
            enemy = new Enemy("Hira Ninja", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            enemy.Shield1 = shieldFactory.CreateElvenTrinket();
            return enemy;
        }

        public Enemy CreateTaagoNinja(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemySprite1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateTaago();
            enemy = new Enemy("Taago Ninja", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            enemy.Shield1 = shieldFactory.CreateElvenTrinket();
            return enemy;
        }

        public Enemy CreateFireSpider(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyFireSpider1;
            int hitPoints = 25;
            float baseSpeed = 0.00001f;
            Weapon weapon = weaponFactory.CreateFireball();
            Charm charm = charmFactory.CreateLowerCooldown();
            enemy = new Enemy("Fire Spider", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, charm, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreatePlasmaSpider(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyBlueSpider1;
            int hitPoints = 25;
            float baseSpeed = 0.000001f;
            Weapon weapon = weaponFactory.CreatePlasmaBolt();
            enemy = new Enemy("Plasma Spider", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            return enemy;
        }

        public Enemy CreateGiantGrappler(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator2Large;
            int hitPoints = 100;
            float baseSpeed = 0.20f;
            Weapon weapon1 = weaponFactory.CreateHelsingor();
            Weapon weapon2 = weaponFactory.CreateGrapple();
            Shield shield = shieldFactory.CreateBasicShield();
            enemy = new Enemy("Grappler", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon1, weapon2, shield, blankC, baseSpeed, pathfinder.Copy());
            enemy.Pathfinder.Type = PathfinderType.StraightLine;
            enemy.ActiveEffects.Add(new Effect(EffectType.Noclip, -1, 1.0f));
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Charm1 = charmFactory.CreateLowerCooldown();
            enemy.ApplyCharmEffects();
            return enemy;
        }

        public Enemy CreateFrostArcher(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemySkeleton2;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateIceBow();
            enemy = new Enemy("Frost Archer", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            return enemy;
        }

        public Enemy CreateDaggerTosser(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateThrowingDagger();
            enemy = new Enemy("Dagger Tosser", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            return enemy;
        }

        public Enemy CreateJungleSpearman(Vector2 location)
        {
            Enemy enemy;
            Texture2D sprite = enemyGladiator1;
            int hitPoints = 50;
            float baseSpeed = 0.15f;
            Weapon weapon = weaponFactory.CreateJungleSpear();
            enemy = new Enemy("Jungle Spearman", location, 5, sprite, new Vector2(0, 0), hitPoints, healthBarSprite, weapon, blankW, blankS, blankC, baseSpeed, pathfinder.Copy());
            enemy.SourceRectangle = new Rectangle(0, 0, sprite.Height, sprite.Height); //So that animations work - each individual frame is square
            enemy.UpdateDrawRectangleAnimated();
            enemy.Pathfinder.Type = PathfinderType.BasicRanged;
            return enemy;
        }
        
    }

    public class Weapon : DynamicEntity, Item
    {
        #region Properties
        int id;
        WeaponType weaponType;
        float slashArc;
        float stabDistance;
        float aimedAngle;
        float damage;
        int cooldown;
        int cooldownRemaining;
        int timeDisplayed;
        int timeDisplayedRemaining;
        float knockback;
        List<Effect> activeEffects = new List<Effect>();
        List<Effect> projectileActiveEffects = new List<Effect>();
        List<Effect> onHitEffects = new List<Effect>();
        string tooltip = "";

        //Ranged weapon only properties
        Texture2D projectileSprite;
        float projectileDamage;
        int projectileLifetime;
        float projectileKnockback;
        float projectileSpeed;
        int projectileBounces;
        int projectilePierces;

        Vector2 userLocation;
        bool isActive = false;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public WeaponType Type
        {
            get { return weaponType; }
        }
        public float StabDistance
        {
            get { return stabDistance; }
        }
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public int ProjectileLifetime
        {
            get { return projectileLifetime; }
        }
        public int Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
        }
        public int CooldownRemaining
        {
            get { return cooldownRemaining; }
            set { cooldownRemaining = value; }
        }
        public int TimeDisplayed
        {
            get { return timeDisplayed; }
        }
        public int TimeDisplayedRemaining
        {
            get { return timeDisplayedRemaining; }
        }
        public float Knockback
        {
            get { return knockback; }
        }
        public Vector2 UserLocation
        {
            get { return userLocation; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Texture2D ProjectileSprite
        {
            get { return projectileSprite; }
        }
        public float ProjectileDamage
        {
            get { return projectileDamage; }
            set { projectileDamage = value; }
        }
        public int ProjectileBounces
        {
            get { return projectileBounces; }
            set { projectileBounces = value; }
        }
        public List<Effect> OnHitEffects
        {
            get { return onHitEffects; }
        }
        public string Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }

        //These were going to be effects, but they really only apply to weapons
        int burst = 0;
        int burstRemaining = 0;
        int maximumBurstDelay = 150;
        int currentBurstDelay = 0;
        int spread = 1;
        float spreadAngle = (float)Math.PI / 2;
        float sprayAngle = 0;
        Random random = new Random();

        public int Burst
        {
            get { return burst; }
            set { burst = value; }
        }
        public int BurstDelay
        {
            get { return maximumBurstDelay; }
            set { maximumBurstDelay = value; }
        }
        public int Spread
        {
            get { return spread; }
            set { spread = value; }
        }
        public float SpreadAngle
        {
            get { return spreadAngle; }
            set { spreadAngle = value; }
        }
        public float SprayAngle
        {
            get { return sprayAngle; }
            set { sprayAngle = value; }
        }
        #endregion

        #region Constructors
        public Weapon()
        {
            damage = 0;
            weaponType = WeaponType.Blank;
            projectileDamage = 0;
            cooldown = 0;
        }

        /// <summary>
        /// Melee weapon constructor
        /// </summary>
        /// <param name="name">The name of the weapon</param>
        /// <param name="location">The initial location of the weapon</param>
        /// <param name="collisionRectangleLeniency">How much smaller than the draw rectangle the collision rectangle will be</param>
        /// <param name="sprite">The image that the weapon will use</param>
        /// <param name="arc">The angle of the arc, in radians, across which the weapon will swing</param>
        /// <param name="stabDistance">The distance away from the player the weapon will stab</param>
        /// <param name="damage">How much damage the weapon will do</param>
        /// <param name="knockback">The knockback velocity imparted upon an enemy hit by this weapon</param>
        /// <param name="cooldown">How many milliseconds the weapon will be unavailable after use</param>
        /// <param name="timeDisplayed">The amount of time for which the weapon is active and displayed, in milliseconds</param>
        public Weapon(string name, Vector2 location, int collisionRectangleLeniency, Texture2D sprite, float arc, float stabDistance, float damage, float knockback, int cooldown, int timeDisplayed, List<Effect> activeEffects, List<Effect> onHitEffects) : base(name, location, collisionRectangleLeniency, sprite, new Vector2(0, 0))
        {
            this.damage = damage;
            this.weaponType = WeaponType.Melee;
            this.cooldown = cooldown;
            this.timeDisplayed = timeDisplayed;
            this.slashArc = arc;
            this.stabDistance = stabDistance;
            this.knockback = knockback;
            this.activeEffects = activeEffects;
            this.onHitEffects = onHitEffects;
            projectileDamage = 0;
        }

        /// <summary>
        /// Ranged weapon constructor
        /// </summary>
        /// <param name="name">The name of the weapon</param>
        /// <param name="location">The initial location of the weapon</param>
        /// <param name="collisionRectangleLeniency">How much smaller than the draw rectangle the collision rectangle will be</param>
        /// <param name="sprite">The image that the weapon will use</param>
        /// <param name="arc">The angle of the arc, in radians, across which the weapon will swing</param>
        /// <param name="stabDistance">The distance away from the player the weapon will stab</param>
        /// <param name="damage">How much damage the weapon will do</param>
        /// <param name="knockback">The knockback velocity imparted upon an enemy hit by this weapon</param>
        /// <param name="cooldown">How many milliseconds the weapon will be unavailable after use</param>
        /// <param name="timeDisplayed">The amount of time for which the weapon is active and displayed, in milliseconds</param>
        /// <param name="projectileSprite">The image that will be used for the projectile generated by this weapon</param>
        /// <param name="projectileDamage">The damage that the projectile will deal when it collides with an enemy</param>
        /// <param name="projectileKnockback">The knockback velocity imparted upon an enemy hit by this weapon's projectile</param>
        /// <param name="projectileLifetime">How many milliseconds the projectile will exist before being destroyed</param>
        /// <param name="projectileSpeed">How fast the projectile will move. Player moves at 0.35f</param>
        /// <param name="projectileBounces">How many times the projectile will and bounce of obstacles before breaking</param>
        /// <param name="projectilePierces"> The amount of enemies that the projectile can hit before being destroyed</param>
        public Weapon(string name, Vector2 location, int collisionRectangleLeniency, Texture2D sprite, float arc, float stabDistance, float damage, float knockback, int cooldown, int timeDisplayed, List<Effect> activeEffects, List<Effect> onHitEffects, Texture2D projectileSprite, float projectileDamage, float projectileKnockback, int projectileLifetime, float projectileSpeed, int projectileBounces, int projectilePierces, List<Effect> projectileActiveEffects) : base(name, location, collisionRectangleLeniency, sprite, new Vector2(0, 0))
        {
            this.damage = damage;
            this.weaponType = WeaponType.Ranged;
            this.cooldown = cooldown;
            this.timeDisplayed = timeDisplayed;
            this.slashArc = arc;
            this.stabDistance = stabDistance;
            this.knockback = knockback;
            this.projectileSprite = projectileSprite;
            this.projectileDamage = projectileDamage;
            this.projectileKnockback = projectileKnockback;
            this.projectileLifetime = projectileLifetime;
            this.projectileSpeed = projectileSpeed;
            this.projectileBounces = projectileBounces;
            this.projectilePierces = projectilePierces;
            this.activeEffects = activeEffects;
            this.onHitEffects = onHitEffects;
            this.projectileActiveEffects = projectileActiveEffects;
        }

        #endregion

        public void Update(GameTime gameTime, Creature user, Vector2 target, bool isBeingUsed, List<Projectile> projectiles)
        {
            if (weaponType == WeaponType.Blank)
                return;
            //Save the location of the player
            userLocation = new Vector2(user.XLocation, user.YLocation);


            //Reduce the weapon's cooldown
            if (cooldownRemaining > 0)
            {
                cooldownRemaining -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //If the weapon is being displayed, then set its isActive property to true
            isActive = false;
            if (timeDisplayedRemaining > 0 && user.HitPoints > 0)
                isActive = true;
            timeDisplayedRemaining -= gameTime.ElapsedGameTime.Milliseconds;

            //Finally, if the weapon is in use (i.e. the player is holding the mouse button down) start the cooldown
            if (isBeingUsed)
            {
                if (cooldownRemaining <= 0)
                {
                    cooldownRemaining = cooldown;
                    timeDisplayedRemaining = timeDisplayed;

                    //Calculate angle of weapon based on location of target
                    Vector2 direction = new Vector2(target.X - userLocation.X, target.Y - userLocation.Y);
                    aimedAngle = (float)(Math.PI / 2 + Math.Atan2(direction.Y, direction.X));

                    if (weaponType == WeaponType.Ranged)
                    {
                        if (spread == 1)
                            CreateProjectile(projectiles, user);
                        if (burst > 0)
                        {
                            burstRemaining = burst;
                            currentBurstDelay = maximumBurstDelay;
                        }
                        if (spread > 1)
                        {
                            for (int i = 0; i < spread; i++)
                            {
                                aimedAngle = (float)(Math.PI / 2 + Math.Atan2(direction.Y, direction.X)) - spreadAngle / 2 + (spreadAngle / (spread - 1)) * i;
                                CreateProjectile(projectiles, user);
                            }
                        }
                    }
                }
            }
            //What about the burst wurst?
            if (burst > 0)
            {
                if (burstRemaining > 0)
                {
                    if (currentBurstDelay > 0)
                    {
                        currentBurstDelay -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                    else
                    {
                        //Calculate angle of weapon based on location of target
                        Vector2 direction = new Vector2(target.X - userLocation.X, target.Y - userLocation.Y);
                        aimedAngle = (float)(Math.PI / 2 + Math.Atan2(direction.Y, direction.X));
                        burstRemaining -= 1;
                        CreateProjectile(projectiles, user);
                        if (spread > 1)
                        {
                            for (int i = 0; i < spread; i++)
                            {
                                aimedAngle = (float)(Math.PI / 2 + Math.Atan2(direction.Y, direction.X)) - spreadAngle / 2 + (spreadAngle / (spread - 1)) * i;
                                CreateProjectile(projectiles, user);
                            }
                        }
                        if (burstRemaining > 0)
                            currentBurstDelay = maximumBurstDelay;
                    }
                }
            }

            if (timeDisplayedRemaining > 0)
            {
                float progressInSwing = (float)timeDisplayedRemaining / (float)timeDisplayed;
                float swingAngle = progressInSwing * slashArc - slashArc / 2;
                angle = aimedAngle + swingAngle;
                float currentStabDistance = stabDistance * (0.5f - (float)Math.Abs(0.5 - progressInSwing));
                location.X = userLocation.X + ((float)user.CollisionRectangle.Height / 2 + collisionRectangle.Height / 2 + currentStabDistance) * (float)Math.Cos(angle - Math.PI / 2);
                location.Y = userLocation.Y + ((float)user.CollisionRectangle.Height / 2 + collisionRectangle.Height / 2 + currentStabDistance) * (float)Math.Sin(angle - Math.PI / 2);
                UpdateRectangle();
            }



        }

        public void CreateProjectile(List<Projectile> projectiles, Creature user)
        {
            if (sprayAngle > 0)
            {
                if (random.Next(0, 2) == 0)
                    aimedAngle += sprayAngle * (float)random.NextDouble();
                else
                    aimedAngle -= sprayAngle * (float)random.NextDouble();
            }
            float projectileLocationX = userLocation.X + drawRectangle.Height * (float)Math.Cos(aimedAngle - Math.PI / 2);
            float projectileLocationY = userLocation.Y + drawRectangle.Height * (float)Math.Sin(aimedAngle - Math.PI / 2);
            Vector2 projectileLocation = new Vector2(projectileLocationX, projectileLocationY);
            Vector2 projectileVelocity = new Vector2(projectileSpeed * (float)Math.Cos(aimedAngle - Math.PI / 2), projectileSpeed * (float)Math.Sin(aimedAngle - Math.PI / 2));
            projectiles.Add(new Projectile(name, projectileLocation, 0, projectileSprite, projectileVelocity, user, projectileDamage, projectileLifetime, projectileKnockback, projectileBounces, projectilePierces, projectileActiveEffects, onHitEffects));
        }

        public new void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            if (timeDisplayedRemaining > 0)
            {
                base.Draw(spriteBatch, offset);
            }
        }

        public Weapon Copy()
        {
            Weapon copy = new Weapon();
            if (weaponType == WeaponType.Melee)
                copy = new Weapon(name, location, 5, sprite, slashArc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects);
            //Collision rectangle leniency set to 5 since that is a constant everywhere so far. Lazy coding, but it probably won't ever be changed
            else if (weaponType == WeaponType.Ranged)
                copy = new Weapon(name, location, 5, sprite, slashArc, stabDistance, damage, knockback, cooldown, timeDisplayed, activeEffects, onHitEffects, projectileSprite, projectileDamage, projectileKnockback, projectileLifetime, projectileSpeed, projectileBounces, projectilePierces, projectileActiveEffects);
            copy.Burst = Burst;
            copy.BurstDelay = BurstDelay;
            copy.Spread = Spread;
            copy.SpreadAngle = SpreadAngle;
            copy.SprayAngle = SprayAngle;
            copy.ID = ID;
            return copy;
        }

    }

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

    public class Projectile : DynamicEntity
    {
        #region Properties
        float damage;
        int lifetime;
        float knockback;
        int numberOfBounces;
        int bouncesRemaining;
        int piercesRemaining;
        bool isActive;
        List<Effect> onHitEffects = new List<Effect>();
        Creature owner;

        public float Damage
        {
            get { return damage; }
        }
        public int Lifetime
        {
            get { return lifetime; }
            set { lifetime = value; }
        }
        public float Knockback
        {
            get { return knockback; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public List<Effect> OnHitEffects
        {
            get { return onHitEffects; }
        }
        public Creature Owner
        {
            get { return owner; }
            set { owner = value; }
        }
        #endregion

        #region Constructors
        public Projectile()
        {

        }

        public Projectile(string name, Vector2 location, int collisionRectangleLeniency, Texture2D sprite, Vector2 velocity, Creature owner, float damage, int lifetime, float knockback, int numberOfBounces, int numberOfPierces, List<Effect> activeEffects, List<Effect> onHitEffects) : base(name, location, collisionRectangleLeniency, sprite, velocity)
        {
            this.owner = owner;
            this.damage = damage;
            this.lifetime = lifetime;
            this.knockback = knockback;
            this.numberOfBounces = numberOfBounces;
            bouncesRemaining = numberOfBounces;
            piercesRemaining = numberOfPierces;
            isActive = true;
            foreach (Effect effect in activeEffects)
            {
                ActiveEffects.Add(effect.Copy());
            }

            foreach (Effect effect in onHitEffects)
            {
                this.onHitEffects.Add(effect.Copy());
            }
        }
        #endregion

        public new void Update(GameTime gameTime)
        {
            lifetime -= gameTime.ElapsedGameTime.Milliseconds;
            if (lifetime <= 0)
                isActive = false;
            base.Update(gameTime);
        }

        public new void Update(GameTime gameTime, List<Projectile> projectiles)
        {
            Update(gameTime);
            if (isActive == false)
            {
                foreach (Effect effect in ActiveEffects)
                    effect.Destroy();
            }
            base.Update(gameTime, projectiles);
        }

        public void Draw(SpriteBatch spriteBatch, LineDrawer lineDrawer, Vector2 offset)
        {
            foreach (Effect effect in ActiveEffects)
            {
                if (effect.Type == EffectType.Grapple)
                {
                    lineDrawer.DrawLine(spriteBatch, Location + offset, owner.Location + offset , Color.Black);
                }
            }
            Draw(spriteBatch, offset);
        }

        public void Destroy()
        {
            //Setting isActive to false flags the projectile for deletion in the next update cycle
            isActive = false;
        }

        public void CollideWithWall(StaticEntity wall)
        {
            //A value of -1 signals that the projectile does not bounce, but passes straight through obstacles
            if (numberOfBounces == -1)
                return;

            if (this.CollisionRectangle.Intersects(wall.CollisionRectangle))
            {
                //Reduce number of bounces appropriately
                if (numberOfBounces > 0)
                    numberOfBounces -= 1;
                else
                {
                    Destroy();
                    return;
                }
                //Collision from top
                if (this.PreviousCollisionRectangle.Bottom <= wall.CollisionRectangle.Top)
                {
                    this.YLocation = wall.CollisionRectangle.Top - (float)this.CollisionRectangle.Height / 2;
                    velocity.Y = -velocity.Y;
                }
                //Collision from bottom
                else if (this.PreviousCollisionRectangle.Top >= wall.CollisionRectangle.Bottom)
                {
                    this.YLocation = wall.CollisionRectangle.Bottom + (float)this.CollisionRectangle.Height / 2;
                    velocity.Y = -velocity.Y;
                }
                //Collision from left
                if (this.PreviousCollisionRectangle.Right <= wall.CollisionRectangle.Left)
                {
                    this.XLocation = wall.CollisionRectangle.Left - (float)this.CollisionRectangle.Width / 2;
                    velocity.X = -velocity.X;
                }
                //Collision from right
                else if (this.PreviousCollisionRectangle.Left >= wall.CollisionRectangle.Right)
                {
                    this.XLocation = wall.CollisionRectangle.Right + (float)this.CollisionRectangle.Width / 2;
                    velocity.X = -velocity.X;
                }
                this.UpdateRectangle();
            }
        }

        public void CollideWithCreature()
        {
            if (piercesRemaining > 0)
            {
                piercesRemaining -= 1;
            }
            else
                Destroy();
        }


    }

    public class Shield : Item
    {
        #region Properties
        int id;
        string name;
        Texture2D sprite;
        List<Effect> activeEffects = new List<Effect>();
        int maximumCooldown;
        int cooldownRemaining;
        bool isActive;
        ShieldType type;
        string tooltip = "";

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
        }
        public List<Effect> ActiveEffects
        {
            get { return activeEffects; }
        }
        public Texture2D Sprite
        {
            get { return sprite; }
        }
        public int MaximumCooldown
        {
            get { return maximumCooldown; }
        }
        public int CooldownRemaining
        {
            get { return cooldownRemaining; }
            set { cooldownRemaining = value; }
        }
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public ShieldType Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }
        #endregion

        #region Constructors
        public Shield()
        {
            maximumCooldown = 0;
            type = ShieldType.Blank;
        }
        public Shield(string name, Texture2D sprite, List<Effect> activeEffects, int cooldown)
        {
            this.name = name;
            isActive = false;
            maximumCooldown = cooldown;
            cooldownRemaining = 0;
            this.sprite = sprite;
            foreach (Effect effect in activeEffects)
                this.activeEffects.Add(effect);
            type = ShieldType.Blocking;
        }
        #endregion

        public void Update(GameTime gameTime, Creature user, Vector2 target)
        {
            if (cooldownRemaining > 0)
                cooldownRemaining -= gameTime.ElapsedGameTime.Milliseconds;
            else if (isActive)
            {
                cooldownRemaining = maximumCooldown;
                foreach (Effect effect in activeEffects)
                    user.ActiveEffects.Add(effect.Copy());
            }
        }

        public Shield Copy()
        {
            Shield copy = new Shield(name, sprite, activeEffects, MaximumCooldown);
            copy.Type = type;
            copy.ID = ID;
            return copy;
        }
    }

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

    public class Charm : Item
    {
        #region Properties
        int id;
        string name;
        CharmType type;
        float strength;
        Texture2D sprite;
        Color color;
        string tooltip = "";

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public String Name
        {
            get { return name; }
        }
        public CharmType Type
        {
            get { return type; }
            set { type = value; }
        }
        public float Strength
        {
            get { return strength; }
            set { strength = value; }
        }
        public Texture2D Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public String Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }
        #endregion

        #region Constructors
        public Charm()
        {
            name = "";
            type = CharmType.Blank;
            color = Color.White;
        }
        public Charm(string name, CharmType type, float strength, Texture2D sprite, Color color)
        {
            this.name = name;
            this.type = type;
            this.strength = strength;
            this.sprite = sprite;
            this.color = color;
        }
        #endregion

        public Charm Copy()
        {
            Charm copy = new Charm(name, type, strength, sprite, color);
            copy.ID = ID;
            copy.Tooltip = tooltip;
            return copy;
        }
    }

    public class CharmFactory
    {
        #region Properties
        Texture2D charmSprite;
        #endregion

        #region Constructors
        public CharmFactory(Texture2D charmSprite)
        {
            this.charmSprite = charmSprite;
        }
        #endregion

        public Charm CreateEmptyCharm()
        {
            Charm charm = new Charm("Empty Charm", CharmType.Blank, 0f, charmSprite, Color.White);
            charm.ID = 0;
            charm.Tooltip = "This charm does nothing.";
            return charm;
        }

        public Charm CreateLowerCooldown()
        {
            Charm charm = new Charm("Overclock Charm", CharmType.LowerCooldown, 2f, charmSprite, Color.Orange);
            charm.ID = 1;
            charm.Tooltip = "Your weapons' cooldowns are half as long, but they deal half as much damage.";
            return charm;
        }

        public Charm CreateHigherCooldown()
        {
            Charm charm = new Charm("Impact Charm", CharmType.HigherCooldown, 2f, charmSprite, Color.Crimson);
            charm.ID = 2;
            charm.Tooltip = "Your weapons deal twice as much damage, but their cooldown is twice as long.";
            return charm;
        }

        public Charm CreateBurstCharm()
        {
            Charm charm = new Charm("Burst Charm", CharmType.Burst, 1.0f, charmSprite, Color.Blue);
            charm.ID = 3;
            charm.Tooltip = "Your ranged weapons have 1 extra burst.";
            return charm;
        }

        public Charm CreateSpeedCharm()
        {
            Charm charm = new Charm("Speed Charm", CharmType.Speed, 1.3f, charmSprite, Color.LimeGreen);
            charm.ID = 4;
            charm.Tooltip = "Your speed is increased by 30%.";
            return charm;
        }

        public Charm CreateLifestealCharm()
        {
            Charm charm = new Charm("Vampiric Charm", CharmType.Lifesteal, 0.4f, charmSprite, Color.DarkViolet);
            charm.ID = 5;
            charm.Tooltip = "Your melee attacks heal you for " + (charm.Strength * 100).ToString() + "% of damage dealt. You cannot take the Heal option in the Trade menu.";
            return charm;
        }

    }

    public class LineDrawer
    {
        #region Properties
        Texture2D pixel;
        #endregion

        #region Constructors
        public LineDrawer(Texture2D pixel)
        {
            this.pixel = pixel;
        }
        #endregion

        public void DrawLine(SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            spriteBatch.Draw(pixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
