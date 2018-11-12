using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainGame.Items;
using MainGame.ContentLoaders.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MainGame.Items.Shields;
using MainGame.Items.Weapons;
using MainGame.Items.Charms;

namespace MainGame.Creatures
{
    public class EnemyFactory
    {
        #region Properties

        CreatureLoader creatureLoader;

        WeaponFactory weaponFactory;
        ShieldFactory shieldFactory;
        CharmFactory charmFactory;

        Pathfinding pathfinder;

        Texture2D healthBarSprite;

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
        public EnemyFactory(Pathfinding pathfinder)
        {
            creatureLoader = CreatureLoader.GetInstance();

            weaponFactory = ItemFactoryContainer.Weapons;
            shieldFactory = ItemFactoryContainer.Shields;
            charmFactory = ItemFactoryContainer.Charms;

            this.pathfinder = pathfinder;

            healthBarSprite = creatureLoader.Get("healthBar1");
            
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
            Texture2D sprite = creatureLoader.Get("human1");
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
            Texture2D sprite = creatureLoader.Get("skeleton1");
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
            Texture2D sprite = creatureLoader.Get("goblin1");
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
            Texture2D sprite = creatureLoader.Get("gladiator1");
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
            Texture2D sprite = creatureLoader.Get("gladiator1");
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
            Texture2D sprite = creatureLoader.Get("gladiator2");
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
            Texture2D sprite = creatureLoader.Get("gladiator2");
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
            Texture2D sprite = creatureLoader.Get("gladiator2large");
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
            Texture2D sprite = creatureLoader.Get("goblin1");
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
            Texture2D sprite = creatureLoader.Get("human");
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
            Texture2D sprite = creatureLoader.Get("human");
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
            Texture2D sprite = creatureLoader.Get("human");
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
            Texture2D sprite = creatureLoader.Get("fireSpider1");
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
            Texture2D sprite = creatureLoader.Get("blueSpider1");
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
            Texture2D sprite = creatureLoader.Get("gladiator2Large");
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
            Texture2D sprite = creatureLoader.Get("skeleton2");
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
            Texture2D sprite = creatureLoader.Get("gladiator1");
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
            Texture2D sprite = creatureLoader.Get("gladiator1");
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

}
