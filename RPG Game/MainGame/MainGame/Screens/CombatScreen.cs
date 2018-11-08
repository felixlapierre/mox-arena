using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Screens
{
    class CombatScreen : Screen
    {
        #region Properties
        Player player1;
        Texture2D player1Sprite;
        Vector2 playerStartingLocation;

        Weapon weapon1;
        Weapon weapon2;
        Shield shield1;
        Charm charm1;

        float Health;

        EnemyFactory enemyFactory;
        List<Enemy> enemies = new List<Enemy>();
        List<Enemy> deadEnemies = new List<Enemy>();

        Texture2D enemySprite1;
        Texture2D enemyGladiator1;
        Texture2D enemyGladiator2;
        Texture2D enemyGladiator2Large;
        Texture2D enemySkeleton1;
        Texture2D enemySkeleton2;
        Texture2D enemyGoblin1;
        Texture2D enemyFireSpider1;
        Texture2D enemyBlueSpider1;

        Pathfinding pathfinder;

        public WeaponFactory weaponFactory;
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

        public ShieldFactory shieldFactory;
        Texture2D shieldBasic1;
        Texture2D shieldBasic2;
        Texture2D speedBoost1;
        Texture2D thunderStone;
        Texture2D elvenTrinket;

        public CharmFactory charmFactory;
        Texture2D charmSprite;

        List<Projectile> friendlyProjectiles = new List<Projectile>();
        List<Projectile> enemyProjectiles = new List<Projectile>();
        LineDrawer lineDrawer;
        Texture2D blueShockwaveBullet;
        Texture2D redShockwaveBullet;

        int TilesWide;
        int TilesHigh;

        List<GridEntity> tiles = new List<GridEntity>();
        List<GridEntity> obstacles = new List<GridEntity>();
        List<Texture2D> tileSprites = new List<Texture2D>();
        List<Texture2D> obstacleSprites = new List<Texture2D>();

        Texture2D tileGrass1Sprite;
        Texture2D tileGrass2Sprite;
        Texture2D tileGrass3Sprite;
        Texture2D tileSand1Sprite;
        Texture2D tileSand2Sprite;
        Texture2D tileSand3Sprite;
        Texture2D tileBrick1Sprite;
        Texture2D tileStone1Sprite;
        Texture2D tilePillar1Sprite;
        Texture2D tileClay1Sprite;

        Texture2D wallRock1Sprite;
        Texture2D wallRock2Sprite;

        bool escapeButtonPreviouslyPressed = false;
        bool shiftButtonPreviouslyPressed = false;
        bool leftMousePreviouslyPressed = false;
        bool rightMousePreviouslyPressed = false;
        int previousScrollWheelValue = 0;

        //Score
        int score;

        //Action bar
        StaticEntity weapon1Icon;
        StaticEntity weapon2Icon;
        StaticEntity shield1Icon;
        StaticEntity charm1Icon;
        StaticEntity icon1Background;
        StaticEntity icon2Background;
        StaticEntity icon3Background;
        StaticEntity icon4Background;
        Texture2D actionBarBackground;

        SpriteFont font;

        //Health bar
        Texture2D healthBarSprite;

        //Adventure mode stuff that needs to be in arena now
        Vector2 offset = new Vector2(0, 0);


        //TODO: Remove these or reconsider their implementation
        ContentManager Content { get; set; }
        Random random = new Random();

        #endregion

        #region Constructor
        public CombatScreen(OnScreenChanged screenChanged, ContentManager content) : base(screenChanged)
        {
            Content = content;

            #region Load Weapons
            weaponSword1 = Content.Load<Texture2D>("graphics/WeaponSword1");
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

            redShockwaveBullet = Content.Load<Texture2D>("graphics/redShockwaveBullet");
            blueShockwaveBullet = Content.Load<Texture2D>("graphics/blueShockwaveBullet");

            weaponFactory = new WeaponFactory(weaponSword1, weaponBow1, weaponBow2, projectileBow1, weaponSword2, weaponAxe1, weaponAxe2,
                weaponMaul1, weaponMaulLarge, weaponHammer1, weaponDagger1, weaponSpear1, weaponSpear2, weaponSpear3, weaponShuriken1,
                weaponShuriken2, weaponShuriken3, redShockwaveBullet, blueShockwaveBullet, weaponGrapple1);
            #endregion

            #region Load Shields
            shieldBasic1 = Content.Load<Texture2D>("graphics/Shield1");
            shieldBasic2 = Content.Load<Texture2D>("graphics/Shield2");
            speedBoost1 = Content.Load<Texture2D>("graphics/speedBoost1");
            thunderStone = Content.Load<Texture2D>("graphics/thunderStone");
            elvenTrinket = Content.Load<Texture2D>("graphics/ElvenTrinket");

            shieldFactory = new ShieldFactory(shieldBasic1, shieldBasic2, speedBoost1, thunderStone, blueShockwaveBullet, elvenTrinket);
            #endregion

            #region Load Charms
            charmSprite = Content.Load<Texture2D>("graphics/charmSprite");
            charmFactory = new CharmFactory(charmSprite);
            #endregion

            #region Create Player
            //Health bar sprite
            healthBarSprite = Content.Load<Texture2D>("graphics/HealthBar2");

            //Load sprite
            player1Sprite = Content.Load<Texture2D>(@"graphics\PlayerSprite1");
            playerStartingLocation = new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2);
            Health = GameConstants.PLAYER_MAX_HIT_POINTS;
            player1 = new Player("Player", playerStartingLocation, player1Sprite, new Vector2(0, 0), GameConstants.PLAYER_MAX_HIT_POINTS, healthBarSprite, weapon1, weapon2, shield1, charm1, GameConstants.PLAYER_BASE_SPEED);
            #endregion

            #region Create Background
            //Create tiles to fill screen

            tileGrass1Sprite = Content.Load<Texture2D>(@"graphics\TileGrass1");
            tileGrass2Sprite = Content.Load<Texture2D>(@"graphics\TileGrass2");
            tileGrass3Sprite = Content.Load<Texture2D>(@"graphics\TileGrass3");
            tileSand1Sprite = Content.Load<Texture2D>(@"graphics\TileSand1");
            tileSand2Sprite = Content.Load<Texture2D>(@"graphics\TileSand2");
            tileSand3Sprite = Content.Load<Texture2D>(@"graphics\TileSand3");
            tileClay1Sprite = Content.Load<Texture2D>(@"graphics\TileClay1");
            tileStone1Sprite = Content.Load<Texture2D>(@"graphics\TileStone1");
            tilePillar1Sprite = Content.Load<Texture2D>(@"graphics\TilePillar1");

            tileSprites.Add(tileSand1Sprite);
            tileSprites.Add(tileSand2Sprite);
            tileSprites.Add(tileSand3Sprite);
            CreateTiles();
            #endregion

            #region Obstacles
            wallRock1Sprite = Content.Load<Texture2D>(@"graphics\wallRock1");
            wallRock2Sprite = Content.Load<Texture2D>(@"graphics/wallRock2");

            obstacleSprites.Add(wallRock1Sprite);
            obstacleSprites.Add(wallRock2Sprite);
            #endregion

            #region Create Bad Guys

            enemySprite1 = Content.Load<Texture2D>("graphics/EnemySprite1");
            enemyGladiator1 = Content.Load<Texture2D>("graphics/EnemyGladiator1");
            enemyGladiator2 = Content.Load<Texture2D>("graphics/EnemyGladiator2");
            enemyGladiator2Large = Content.Load<Texture2D>("graphics/EnemyGladiator2Large");
            enemySkeleton1 = Content.Load<Texture2D>("graphics/EnemySkeleton1");
            enemySkeleton2 = Content.Load<Texture2D>("graphics/EnemySkeleton2");
            enemyGoblin1 = Content.Load<Texture2D>("graphics/EnemyGoblin1");
            enemyFireSpider1 = Content.Load<Texture2D>("graphics/FireSpider");
            enemyBlueSpider1 = Content.Load<Texture2D>("graphics/BlueSpider");

            pathfinder = new Pathfinding(TilesWide, TilesHigh, GameConstants.TILE_SIZE, weaponDagger1, PathfinderType.BasicPathfinder);
            enemyFactory = new EnemyFactory(weaponFactory, shieldFactory, charmFactory, pathfinder, healthBarSprite, enemySprite1, enemyGladiator1, enemyGladiator2, enemyGladiator2Large,
                enemyGoblin1, enemySkeleton1, enemySkeleton2, enemyFireSpider1, enemyBlueSpider1);

            #endregion

            font = Content.Load<SpriteFont>("font/font");
        }
        #endregion

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            offset = -player1.Location + new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2);

            #region Player pausing logic
            if (keyboard.IsKeyDown(Keys.Escape) && !escapeButtonPreviouslyPressed)
            {
                gameState = GameState.PlayerPaused;
            }
            #endregion

            #region Next level logic
            if (enemies.Count == 0)
            {
                ////TODO: Initialize trade window
                ////Add the item boxes that are to be filled from enemies to a list
                ////Call AddItemFromEnemy with that list
                ////Get all the items from deadenemies and add them at random to the item boxes
                
                //List<ItemBox> enemyFilledBoxes = new List<ItemBox>();
                //enemyFilledBoxes.Add(tradeItemBoxes.ElementAt(0));
                //enemyFilledBoxes.Add(tradeItemBoxes.ElementAt(1));
                //AddItemFromEnemy(enemyFilledBoxes, deadEnemies);
                //AddRandomItem(tradeItemBoxes.ElementAt(2), weaponFactory, shieldFactory);

                //deadEnemies.Clear();
                //friendlyProjectiles.Clear();
                //enemyProjectiles.Clear();

                //player1.XLocation = WindowWidth / 2;
                //player1.YLocation = WindowHeight / 2;

                //player1.Weapon1.CooldownRemaining = 0;
                //player1.Weapon2.CooldownRemaining = 0;
                //player1.Shield1.CooldownRemaining = 0;

                //player1.ActiveEffects.Clear();

                //gameState = GameState.Trade;
                //saveGameOverrideEnabled = false;

                //oldWeapon1 = weapon1;
                //oldWeapon2 = weapon2;
                //oldShield1 = shield1;
                //oldCharm1 = charm1;
                //Health = player1.HitPoints;
                //oldHealth = player1.HitPoints;
                //playerIsHealing = false;

                //level += 1;

            }

            #endregion

            #region Projectile Movement

            UpdateProjectiles(gameTime, friendlyProjectiles, obstacles);
            UpdateProjectiles(gameTime, enemyProjectiles, obstacles);

            #endregion

            #region Player Movement, Collision, Attack

            player1.Update(gameTime, keyboard, mouse, friendlyProjectiles, offset);
            player1.UpdateRectangle();
            EdgeCollision(player1);
            foreach (StaticEntity obstacle in obstacles)
            {
                DynamicToStaticCollision(player1, obstacle);
            }

            if (player1.InvincibilityFramesRemaining <= 0 && GameConstants.GOD_MODE == false)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (player1.CollisionRectangle.Intersects(enemy.Weapon1.CollisionRectangle)
                        && enemy.HitPoints > 0
                        && enemy.Weapon1.IsActive
                        && player1.Unhittable == false)
                    {
                        if (enemy.Weapon1.Damage > 0)
                        {
                            player1.HitByAttack(enemy.Weapon1);
                            if (enemy.Charm1.Type == CharmType.Lifesteal)
                                enemy.Heal(enemy.Weapon1.Damage * enemy.Charm1.Strength);
                        }
                    }
                    if (player1.CollisionRectangle.Intersects(enemy.Weapon2.CollisionRectangle)
                        && enemy.HitPoints > 0
                        && enemy.Weapon2.IsActive
                        && player1.Unhittable == false)
                    {
                        if (enemy.Weapon2.Damage > 0)
                        {
                            player1.HitByAttack(enemy.Weapon2);
                            if (enemy.Charm1.Type == CharmType.Lifesteal)
                                enemy.Heal(enemy.Weapon2.Damage * enemy.Charm1.Strength);
                        }
                    }
                }
                foreach (Projectile projectile in enemyProjectiles)
                {
                    if (player1.CollisionRectangle.Intersects(projectile.CollisionRectangle))
                    {
                        player1.HitByAttack(projectile);
                        projectile.CollideWithCreature();
                    }
                }
            }

            #endregion

            #region Creature Movement and Collision
            int enemiesActive = 0;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.HitPoints > 0)
                {
                    enemiesActive += 1;
                    enemy.Update(gameTime, player1, obstacles, enemyProjectiles, friendlyProjectiles);
                    enemy.UpdateRectangle();
                    foreach (StaticEntity obstacle in obstacles)
                    {
                        DynamicToStaticCollision(enemy, obstacle);
                    }
                    EdgeCollision(enemy);
                    //Collision with player weapons
                    if (enemy.InvincibilityFramesRemaining <= 0)
                    {
                        if (enemy.CollisionRectangle.Intersects(player1.Weapon1.CollisionRectangle)
                            && player1.HitPoints > 0
                            && player1.Weapon1.IsActive
                            && enemy.Unhittable == false)
                        {
                            if (player1.Weapon1.Damage > 0)
                            {
                                enemy.HitByAttack(player1.Weapon1);
                                if (player1.Charm1.Type == CharmType.Lifesteal)
                                    player1.Heal(player1.Weapon1.Damage * player1.Charm1.Strength);
                            }
                        }
                        if (enemy.CollisionRectangle.Intersects(player1.Weapon2.CollisionRectangle)
                            && player1.HitPoints > 0
                            && player1.Weapon2.IsActive
                            && enemy.Unhittable == false)
                        {
                            if (player1.Weapon2.Damage > 0)
                            {
                                enemy.HitByAttack(player1.Weapon2);
                                if (player1.Charm1.Type == CharmType.Lifesteal)
                                    player1.Heal(player1.Weapon1.Damage * player1.Charm1.Strength);
                            }
                        }
                        foreach (Projectile projectile in friendlyProjectiles)
                        {
                            if (enemy.CollisionRectangle.Intersects(projectile.CollisionRectangle) && projectile.IsActive)
                            {
                                enemy.HitByAttack(projectile);
                                projectile.CollideWithCreature();
                            }
                        }
                    }
                }
                else
                {
                    score += 1;
                    deadEnemies.Add(enemy);
                }
            }
            foreach (Enemy enemy in deadEnemies)
                enemies.Remove(enemy);
            #endregion

            #region Adventure Mode
            //if (gameMode == GameMode.Adventure)
            //{
            //    if (keyboard.IsKeyDown(Keys.LeftShift) && !shiftButtonPreviouslyPressed)
            //    {
            //        if (looseCamera == false)
            //            looseCamera = true;
            //        else
            //            looseCamera = false;
            //    }
            //    foreach (Spawner spawner in spawners)
            //    {
            //        spawner.Update(gameTime, player1.Location, enemies, enemyFactory);
            //    }

            //}
            #endregion

            #region Game Over
            if (player1.HitPoints <= 0)
            {
                gameState = GameState.GameOver;
            }
            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            offset = -player1.Location + new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2);

            #region Tiles, obstacles, enemies and projectiles

            foreach (StaticEntity tile in tiles)
                tile.Draw(spriteBatch, offset);

            foreach (StaticEntity obstacle in obstacles)
                obstacle.Draw(spriteBatch, offset);

            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch, offset);

            foreach (Enemy enemy in deadEnemies)
                enemy.Draw(spriteBatch, offset);

            foreach (Projectile projectile in friendlyProjectiles)
                projectile.Draw(spriteBatch, lineDrawer, offset);

            foreach (Projectile projectile in enemyProjectiles)
                projectile.Draw(spriteBatch, lineDrawer, offset);

            #endregion

            #region Player

            player1.Draw(spriteBatch, offset);
            player1.Weapon1.Draw(spriteBatch, offset);
            player1.Weapon2.Draw(spriteBatch, offset);
            #endregion

            #region Action Bar Icons
            icon1Background.Draw(spriteBatch);
            if (player1.Shield1.CooldownRemaining <= 0)
                icon2Background.Draw(spriteBatch);
            else
                icon2Background.Draw(spriteBatch, Color.DodgerBlue);

            if (player1.Weapon2.CooldownRemaining <= 0)
                icon3Background.Draw(spriteBatch);
            else
                icon3Background.Draw(spriteBatch, Color.DodgerBlue);

            if (player1.Weapon1.CooldownRemaining <= 0)
                icon4Background.Draw(spriteBatch);
            else
                icon4Background.Draw(spriteBatch, Color.DodgerBlue);

            weapon1Icon.Draw(spriteBatch);
            weapon2Icon.Draw(spriteBatch);
            shield1Icon.Draw(spriteBatch);
            charm1Icon.Draw(spriteBatch, charm1.Color);
            #endregion

            #region Text
            spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(0, 0), Color.Black);
            #endregion
        }

        #region Private Methods
        private void CreateTiles()
        {
            for (int i = 0; i < TilesWide; i++)
            {
                for (int j = 0; j < TilesHigh; j++)
                {
                    Texture2D currentSprite = PickRandomTexture(tileSprites);

                    Vector2 tileLocation = new Vector2(i * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2, j * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2);
                    GridEntity newTile = new GridEntity("Tile", i, j, 0, tileLocation, GameConstants.TILE_SIZE, currentSprite);
                    tiles.Add(newTile);
                }
            }
        }

        private Texture2D PickRandomTexture(List<Texture2D> textures)
        {
            Texture2D chosenTexture = textures.ElementAt(0);
            int index = random.Next(textures.Count);
            return textures.ElementAt(index);
        }

        private void UpdateProjectiles(GameTime gameTime, List<Projectile> projectiles, List<GridEntity> obstacles)
        {
            //Create a list to hold the projectiles that will be deleted
            List<Projectile> ProjectilesToBeDeleted = new List<Projectile>();
            List<Projectile> ProjectilesToBeAdded = new List<Projectile>();
            foreach (Projectile projectile in projectiles)
            {
                if (!projectile.IsActive)
                    ProjectilesToBeDeleted.Add(projectile);
                projectile.Update(gameTime, ProjectilesToBeAdded);
                if (CheckIfOffScreen(projectile))
                {
                    projectile.IsActive = false;
                }
                foreach (StaticEntity obstacle in obstacles)
                {
                    projectile.CollideWithWall(obstacle);
                }

            }
            //Delete all the projectiles!
            foreach (Projectile projectile in ProjectilesToBeDeleted)
            {
                projectiles.RemoveAt(projectiles.IndexOf(projectile));
            }
            //Add the new projectiles!
            foreach (Projectile projectile in ProjectilesToBeAdded)
            {
                projectiles.Add(projectile);
            }
        }

        public bool CheckIfOffScreen(StaticEntity staticEntity)
        {
            bool isOffScreen = false;
            if (staticEntity.CollisionRectangle.Right < 0 || staticEntity.CollisionRectangle.Left > TilesWide * GameConstants.TILE_SIZE)
                isOffScreen = true;
            if (staticEntity.CollisionRectangle.Bottom < 0 || staticEntity.CollisionRectangle.Top > TilesHigh * GameConstants.TILE_SIZE)
                isOffScreen = true;
            return isOffScreen;
        }

        private void EdgeCollision(DynamicEntity dynamicEntity)
        {
            int width = TilesWide * GameConstants.TILE_SIZE;
            int height = TilesHigh * GameConstants.TILE_SIZE;
            int halfEntityWidth = dynamicEntity.CollisionRectangle.Width / 2;
            int halfEntityHeight = dynamicEntity.CollisionRectangle.Height / 2;
            if (dynamicEntity.XLocation < halfEntityWidth)
                dynamicEntity.XLocation = halfEntityWidth;

            if (dynamicEntity.XLocation > width - halfEntityWidth)
                dynamicEntity.XLocation = width - halfEntityWidth;

            if (dynamicEntity.YLocation < halfEntityHeight)
                dynamicEntity.YLocation = halfEntityHeight;

            if (dynamicEntity.YLocation > height - halfEntityHeight)
                dynamicEntity.YLocation = height - halfEntityHeight;

            dynamicEntity.UpdateRectangle();
        }

        public void DynamicToStaticCollision(Creature creature, StaticEntity staticEntity)
        {
            if (creature.CollisionRectangle.Intersects(staticEntity.CollisionRectangle) && creature.Noclip == false)
            {
                //Collision from top
                if (creature.PreviousCollisionRectangle.Bottom <= staticEntity.CollisionRectangle.Top)
                {
                    creature.YLocation = staticEntity.CollisionRectangle.Top - (float)creature.CollisionRectangle.Height / 2;
                    creature.KnockbackVelocity = new Vector2(creature.KnockbackVelocity.X, 0);
                }
                //Collision from bottom
                else if (creature.PreviousCollisionRectangle.Top >= staticEntity.CollisionRectangle.Bottom)
                {
                    creature.YLocation = staticEntity.CollisionRectangle.Bottom + (float)creature.CollisionRectangle.Height / 2;
                    creature.KnockbackVelocity = new Vector2(creature.KnockbackVelocity.X, 0);
                }
                //Collision from left
                if (creature.PreviousCollisionRectangle.Right <= staticEntity.CollisionRectangle.Left)
                {
                    creature.XLocation = staticEntity.CollisionRectangle.Left - (float)creature.CollisionRectangle.Width / 2;
                    creature.KnockbackVelocity = new Vector2(0, creature.KnockbackVelocity.Y);
                }
                //Collision from right
                else if (creature.PreviousCollisionRectangle.Left >= staticEntity.CollisionRectangle.Right)
                {
                    creature.XLocation = staticEntity.CollisionRectangle.Right + (float)creature.CollisionRectangle.Width / 2;
                    creature.KnockbackVelocity = new Vector2(0, creature.KnockbackVelocity.Y);
                }
                creature.UpdateRectangle();
            }
        }
        #endregion
    }
}
