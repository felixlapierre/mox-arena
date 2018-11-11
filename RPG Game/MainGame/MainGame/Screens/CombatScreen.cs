using MainGame.Items;
using MainGame.Screens.Trade_Screen;
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

        int Level;

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

        WeaponFactory weaponFactory;
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

        ShieldFactory shieldFactory;
        Texture2D shieldBasic1;
        Texture2D shieldBasic2;
        Texture2D speedBoost1;
        Texture2D thunderStone;
        Texture2D elvenTrinket;

        CharmFactory charmFactory;
        Texture2D charmSprite;

        List<Projectile> friendlyProjectiles = new List<Projectile>();
        List<Projectile> enemyProjectiles = new List<Projectile>();
        LineDrawer lineDrawer;
        Texture2D blueShockwaveBullet;
        Texture2D redShockwaveBullet;

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
        bool playerPaused = false;
        int unpauseTimer = 0;

        #endregion

        #region Constructor
        public CombatScreen(OnScreenChanged screenChanged, ContentManager content, TradeScreenContents tradeContents) : base(screenChanged)
        {
            Content = content;

            Level = tradeContents.Level;

            actionBarBackground = Content.Load<Texture2D>("graphics/actionBarBackground");

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

            Texture2D pixel = Content.Load<Texture2D>("graphics/pixel");
            lineDrawer = new LineDrawer(pixel);
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

            //TODO: Fix the ugly code where we set the player health afterwards
            player1 = new Player("Player", playerStartingLocation, player1Sprite, new Vector2(0, 0), GameConstants.PLAYER_MAX_HIT_POINTS, healthBarSprite, tradeContents.Weapon1.Copy(), tradeContents.Weapon2.Copy(), tradeContents.Shield1.Copy(), tradeContents.Charm1.Copy(), GameConstants.PLAYER_BASE_SPEED);
            player1.HitPoints = tradeContents.Health;
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

            pathfinder = new Pathfinding(GameConstants.TILES_WIDE, GameConstants.TILES_HIGH, GameConstants.TILE_SIZE, weaponDagger1, PathfinderType.BasicPathfinder);
            enemyFactory = new EnemyFactory(weaponFactory, shieldFactory, charmFactory, pathfinder, healthBarSprite, enemySprite1, enemyGladiator1, enemyGladiator2, enemyGladiator2Large,
                enemyGoblin1, enemySkeleton1, enemySkeleton2, enemyFireSpider1, enemyBlueSpider1);

            #endregion

            #region Create User Interface
            font = Content.Load<SpriteFont>("font/font");
            score = 0;

            Vector2 firstIconLocation = new Vector2((GameConstants.TILES_WIDE - 0.5f) * GameConstants.TILE_SIZE, (GameConstants.TILES_HIGH + 0.5f) * GameConstants.TILE_SIZE);
            Texture2D firstIconSprite;
            firstIconSprite = player1.Charm1.Sprite;

            Vector2 secondIconLocation = new Vector2(firstIconLocation.X - GameConstants.TILE_SIZE, firstIconLocation.Y);
            Texture2D secondIconSprite = player1.Shield1.Sprite;

            Vector2 thirdIconLocation = new Vector2(secondIconLocation.X - GameConstants.TILE_SIZE, secondIconLocation.Y);
            Texture2D thirdIconSprite = player1.Weapon2.Sprite;


            Vector2 fourthIconLocation = new Vector2(thirdIconLocation.X - GameConstants.TILE_SIZE, thirdIconLocation.Y);
            Texture2D fourthIconSprite = player1.Weapon1.Sprite;

            charm1Icon = new StaticEntity("Charm 1 Icon", firstIconLocation, firstIconSprite);
            shield1Icon = new StaticEntity("Shield 1 Icon", secondIconLocation, secondIconSprite);
            weapon2Icon = new StaticEntity("Weapon 1 Icon", thirdIconLocation, thirdIconSprite);
            weapon1Icon = new StaticEntity("Weapon 2 Icon", fourthIconLocation, fourthIconSprite);

            icon1Background = new StaticEntity("Icon 1 Background", firstIconLocation, actionBarBackground);
            icon2Background = new StaticEntity("Icon 2 Background", secondIconLocation, actionBarBackground);
            icon3Background = new StaticEntity("Icon 3 Background", thirdIconLocation, actionBarBackground);
            icon4Background = new StaticEntity("Icon 4 Background", fourthIconLocation, actionBarBackground);
            #endregion

            CreateObstacles();

            //Create Enemies
            CreateEnemies(enemyFactory, enemies);
            
            pathfinder.MapTilesHigh = GameConstants.TILES_HIGH;
            pathfinder.MapTilesWide = GameConstants.TILES_WIDE;
            unpauseTimer = GameConstants.PAUSE_DELAY;

            //Update once to properly adjust health bars
            player1.HealthBar.Update(player1);
            foreach (Enemy enemy in enemies)
                enemy.HealthBar.Update(enemy);

            //TODO: Move this to the Player class where it belongs - right now this causes a bug where charm effects on weapons
            //stack every level. Bug is funny so i'll leave it until charms are refactored.
            player1.ApplyCharmEffects();
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
                if(playerPaused)
                {
                    playerPaused = false;
                    unpauseTimer = GameConstants.PAUSE_DELAY;
                } else
                {
                    playerPaused = true;
                }
            }
            escapeButtonPreviouslyPressed = keyboard.IsKeyDown(Keys.Escape);

            if (playerPaused)
            {
                return;
            }
            if(unpauseTimer > 0)
            {
                unpauseTimer -= gameTime.ElapsedGameTime.Milliseconds;
                return;
            }

            #endregion

            #region Next Level logic
            if (enemies.Count == 0)
            {
                //TODO: Initialize trade window
                float health = player1.HitPoints;
                int level = Level + 1;

                Item item1 = AddItemFromEnemy(deadEnemies);
                Item item2 = AddItemFromEnemy(deadEnemies);
                Item item3 = AddRandomItem();

                TradeScreenContents tradeContents = new TradeScreenContents(player1.HitPoints, level, player1.Weapon1, player1.Weapon2, player1.Shield1, player1.Charm1,
                    item1, item2, item3);

                ScreenChanged(new TradeScreen(ScreenChanged, Content, tradeContents));
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
                ScreenChanged(new GameOverScreen(ScreenChanged, Content));
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
            charm1Icon.Draw(spriteBatch, player1.Charm1.Color);
            #endregion

            #region Text
            spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(0, 0), Color.Black);

            if (unpauseTimer > 0 && !playerPaused)
            {
                Vector2 stringSize = font.MeasureString("Get Ready!");
                spriteBatch.DrawString(font, "Get Ready!", new Vector2(GameConstants.WINDOW_WIDTH / 2 - stringSize.X / 2, 0), Color.Black);
            }
            else if (playerPaused)
            {
                Vector2 stringSize = font.MeasureString("Game is Paused");
                spriteBatch.DrawString(font, "Game is Paused", new Vector2(GameConstants.WINDOW_WIDTH / 2 - stringSize.X / 2, 0), Color.Black);
            }
            #endregion
        }

        #region Private Methods
        private void CreateTiles()
        {
            for (int i = 0; i < GameConstants.TILES_WIDE; i++)
            {
                for (int j = 0; j < GameConstants.TILES_HIGH; j++)
                {
                    Texture2D currentSprite = PickRandomTexture(tileSprites);

                    Vector2 tileLocation = new Vector2(i * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2, j * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2);
                    GridEntity newTile = new GridEntity("Tile", i, j, 0, tileLocation, GameConstants.TILE_SIZE, currentSprite);
                    tiles.Add(newTile);
                }
            }
        }

        private void CreateObstacles()
        {
            for (int i = 1; i <= GameConstants.NUMBER_OF_ROCKS; i++)
            {
                //Choose a random x and y location that is inside the screen
                bool locationIsValid = false;
                int x = 0;
                int y = 0;
                while (!locationIsValid)
                {
                    x = random.Next(0, GameConstants.WINDOW_WIDTH / GameConstants.TILE_SIZE);
                    y = random.Next(0, GameConstants.WINDOW_HEIGHT / GameConstants.TILE_SIZE);
                    locationIsValid = true;
                    foreach (GridEntity obstacle in obstacles)
                    {
                        if (obstacle.XPosition == x && obstacle.YPosition == y)
                            locationIsValid = false;
                    }
                    if (x == (GameConstants.WINDOW_WIDTH / GameConstants.TILE_SIZE) / 2 ||
                    x == (GameConstants.WINDOW_WIDTH / GameConstants.TILE_SIZE) / 2 - 1 ||
                    y == (GameConstants.WINDOW_HEIGHT / GameConstants.TILE_SIZE) / 2 ||
                    y == (GameConstants.WINDOW_HEIGHT / GameConstants.TILE_SIZE) / 2 - 1)
                        locationIsValid = false;
                }

                Vector2 obsLocation = new Vector2(x * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2,
                    y * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2);
                //Create the obstacle
                obstacles.Add(GetObstacleFromID(1, x, y, 1));
            }
        }

        private void CreateEnemies(EnemyFactory enemyFactory, List<Enemy> enemies)
        {
            List<Enemy> choices = new List<Enemy>();
            Vector2 zero = new Vector2(0, 0);
            switch (Level)
            {
                #region Levels 1-10
                case 1:
                    enemies.Add(enemyFactory.CreateSwordsman(GetEnemyLocation()));
                    break;
                case 2:
                    enemies.Add(enemyFactory.CreateSkeletonArcher(GetEnemyLocation()));
                    break;
                case 3:
                    choices.Add(enemyFactory.CreateSwordsman(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateSkeletonArcher(GetEnemyLocation()));
                    PickRandomEnemies(enemies, choices, 2);
                    break;
                case 4:
                    enemies.Add(enemyFactory.CreateGoblinMauler(GetEnemyLocation()));
                    break;
                case 5:
                    choices.Add(enemyFactory.CreateSwordsman(zero));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    PickRandomEnemies(enemies, choices, 2);
                    break;
                case 6:
                    enemies.Add(enemyFactory.CreateSpearman(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    PickRandomEnemies(enemies, choices, 1);
                    break;
                case 7:
                    enemies.Add(enemyFactory.CreateSpearman(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreateSpearThrower(GetEnemyLocation()));
                    break;
                case 8:
                case 9:
                    choices.Add(enemyFactory.CreateSwordsman(zero));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateSpearThrower(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    break;
                case 10:
                    enemies.Add(enemyFactory.CreateGiant(GetEnemyLocation()));
                    break;
                #endregion

                #region Levels 11-20
                case 11:
                    enemies.Add(enemyFactory.CreateHeavy(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    PickRandomEnemies(enemies, choices, 1);
                    enemies.ElementAt(0).Charm1 = charmFactory.CreateSpeedCharm();
                    enemies.ElementAt(0).ApplyCharmEffects();
                    break;
                case 12:
                case 13:
                case 14:
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateSpearThrower(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    enemies.ElementAt(0).Charm1 = charmFactory.CreateSpeedCharm();
                    enemies.ElementAt(0).ApplyCharmEffects();
                    break;
                case 15:
                    enemies.Add(enemyFactory.CreateFireSpider(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreateFireSpider(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreateFireSpider(GetEnemyLocation()));
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateSpearThrower(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    enemies.ElementAt(0).Charm1 = charmFactory.CreateSpeedCharm();
                    enemies.ElementAt(0).ApplyCharmEffects();
                    break;
                case 20:
                    enemies.Add(enemyFactory.CreateBotengNinja(GetEnemyLocation()));
                    enemies.ElementAt(0).Charm1 = charmFactory.CreateSpeedCharm();
                    break;
                #endregion

                #region Levels 21-30
                case 21:
                case 22:
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateSpearThrower(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    break;
                case 23:
                case 24:
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateSpearThrower(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    foreach (Enemy createdEnemy in enemies)
                    {
                        createdEnemy.Charm1 = charmFactory.CreateSpeedCharm();
                        createdEnemy.ApplyCharmEffects();
                    }
                    break;
                case 25:
                    enemies.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    break;
                case 26:
                case 27:
                case 28:
                case 29:
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateSpearThrower(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    PickRandomEnemies(enemies, choices, 4);
                    break;
                case 30:
                    enemies.Add(enemyFactory.CreateHiraNinja(GetEnemyLocation()));
                    enemies.ElementAt(0).Charm1 = charmFactory.CreateHigherCooldown();
                    break;
                #endregion

                #region Levels 31-40
                case 31:
                    enemies.Add(enemyFactory.CreateJungleSpearman(GetEnemyLocation()));
                    break;
                case 32:
                    enemies.Add(enemyFactory.CreateJungleSpearman(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateJungleSpearman(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    choices.Add(enemyFactory.CreateBarbarian(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    break;
                case 33:
                case 34:
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateJungleSpearman(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    choices.Add(enemyFactory.CreateBarbarian(zero));
                    PickRandomEnemies(enemies, choices, 4);
                    break;
                case 35:
                    enemies.Add(enemyFactory.CreateHiraNinja(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreateTaagoNinja(GetEnemyLocation()));
                    break;
                case 36:
                case 37:
                case 38:
                case 39:
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateSkeletonArcher(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateSpearman(zero));
                    choices.Add(enemyFactory.CreateJungleSpearman(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    choices.Add(enemyFactory.CreateGiant(zero));
                    choices.Add(enemyFactory.CreateBarbarian(zero));
                    PickRandomEnemies(enemies, choices, 4);
                    break;
                case 40:
                    enemies.Add(enemyFactory.CreateGiantGrappler(GetEnemyLocation()));
                    break;
                #endregion

                #region Levels 41-50
                //Adds jungle spear, poison dagger, ice bow
                case 41:
                    enemies.Add(enemyFactory.CreateFrostArcher(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreateSkeletonArcher(GetEnemyLocation()));
                    enemies.Add(enemyFactory.CreateFrostArcher(GetEnemyLocation()));
                    break;
                case 42:
                case 43:
                    enemies.Add(enemyFactory.CreateFrostArcher(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    choices.Add(enemyFactory.CreateFrostArcher(GetEnemyLocation()));
                    PickRandomEnemies(enemies, choices, 2);
                    enemies.ElementAt(0).Charm1 = charmFactory.CreateSpeedCharm();
                    enemies.ElementAt(0).ApplyCharmEffects();
                    enemies.ElementAt(1).Charm1 = charmFactory.CreateLifestealCharm();
                    enemies.ElementAt(1).ApplyCharmEffects();
                    break;
                case 44:
                    enemies.Add(enemyFactory.CreateFrostArcher(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateFrostArcher(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    choices.Add(enemyFactory.CreateHeavy(zero));
                    choices.Add(enemyFactory.CreateGoblinMauler(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    enemies.ElementAt(0).Charm1 = charmFactory.CreateBurstCharm();
                    enemies.ElementAt(0).ApplyCharmEffects();
                    break;
                case 45:
                    enemies.Add(enemyFactory.CreateDaggerTosser(GetEnemyLocation()));
                    enemies.ElementAt(0).Shield1 = shieldFactory.CreateBasicShield();
                    break;
                case 46:
                case 47:
                case 48:
                case 49:
                    choices.Add(enemyFactory.CreateFrostArcher(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateDaggerTosser(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreatePlasmaSpider(GetEnemyLocation()));
                    choices.Add(enemyFactory.CreateFireSpider(zero));
                    PickRandomEnemies(enemies, choices, 3);
                    if (random.Next(2) == 1)
                        enemies.ElementAt(0).Charm1 = charmFactory.CreateBurstCharm();
                    else
                        enemies.ElementAt(0).Charm1 = charmFactory.CreateHigherCooldown();
                    enemies.ElementAt(0).ApplyCharmEffects();
                    break;
                case 50:
                    Enemy enemy = enemyFactory.CreateSwordsman(GetEnemyLocation());
                    enemy.HitPoints = 100;
                    enemy.MaxHitPoints = 100;
                    enemy.Charm1 = charmFactory.CreateLifestealCharm();
                    enemy.ApplyCharmEffects();
                    enemy.Weapon1 = weaponFactory.CreateHelsingor();
                    enemy.Weapon2 = weaponFactory.CreateIceBow();
                    enemy.Shield1 = shieldFactory.CreateBasicShield();
                    enemies.Add(enemy);
                    break;
                #endregion

                default:
                    CreateEnemiesOld(enemyFactory, enemies);
                    break;
            }
        }

        public void CreateEnemiesOld(EnemyFactory enemyFactory, List<Enemy> enemies)
        {
            //Determine the current difficulty Level
            int difficultyLevel = 1 + (int)Math.Ceiling(((decimal)Level / 4));
            int numberOfEnemies = 1;
            if (Level < 3)
                numberOfEnemies = 1;
            else if (Level >= 3 && Level < 8)
                numberOfEnemies = 2;
            else if (Level >= 8 && Level < 15)
                numberOfEnemies = 3;
            else if (Level >= 15 && Level < 23)
                numberOfEnemies = 4;
            else
                numberOfEnemies = 5;

            //Generate a location
            Vector2 location;

            if (Level == 10)
            {
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                return;
            }
            if (Level == 20)
            {
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                return;
            }
            if (Level == 30)
            {
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                return;
            }

            for (int i = 1; i <= numberOfEnemies; i++)
            {
                //Main loop that creates an enemy
                location = GetEnemyLocation();
                //Generate a random number to decide which enemy it will be
                int maxEnemyID = 2;
                if (difficultyLevel <= 8)
                    maxEnemyID += difficultyLevel;
                else
                    maxEnemyID = 15;

                int enemyID = random.Next(1, maxEnemyID);

                //Choose the enemy based on the number generated
                switch (enemyID)
                {
                    case 1:
                        enemies.Add(enemyFactory.CreateSwordsman(location));
                        break;
                    case 2:
                        enemies.Add(enemyFactory.CreateGoblinMauler(location));
                        break;
                    case 3:
                        enemies.Add(enemyFactory.CreateSkeletonArcher(location));
                        break;
                    case 4:
                        enemies.Add(enemyFactory.CreateSpearman(location));
                        break;
                    case 5:
                        enemies.Add(enemyFactory.CreateSpearThrower(location));
                        break;
                    case 6:
                        enemies.Add(enemyFactory.CreateHeavy(location));
                        break;
                    case 7:
                        enemies.Add(enemyFactory.CreateBarbarian(location));
                        break;
                    case 8:
                        enemies.Add(enemyFactory.CreateGoblinArcanist(location));
                        break;
                    case 9:
                        enemies.Add(enemyFactory.CreateGiant(location));
                        break;
                    case 10:
                        enemies.Add(enemyFactory.CreateFireSpider(location));
                        break;
                    case 11:
                        enemies.Add(enemyFactory.CreatePlasmaSpider(location));
                        break;
                    case 12:
                        enemies.Add(enemyFactory.CreateHiraNinja(location));
                        break;
                    case 13:
                        enemies.Add(enemyFactory.CreateBotengNinja(location));
                        break;
                    case 14:
                        enemies.Add(enemyFactory.CreateTaagoNinja(location));
                        break;
                    case 15:
                        enemies.Add(enemyFactory.CreateGiantGrappler(location));
                        break;
                    default:
                        enemies.Add(enemyFactory.CreateSwordsman(location));
                        break;
                }
            }
        }

        private GridEntity GetObstacleFromID(int id, int x, int y, int z)
        {
            GridEntity obstacle = new GridEntity();
            Vector2 location = new Vector2(x * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2, 
                y * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2);
            switch (id)
            {
                case 1:
                    obstacle = new GridEntity("Rock", x, y, z, location, 10, wallRock1Sprite);
                    break;
                case 2:
                    obstacle = new GridEntity("Stone", x, y, z, location, 10, tileStone1Sprite);
                    break;
                case 3:
                    obstacle = new GridEntity("ClayTile", x, y, z, location, 10, tileClay1Sprite);
                    break;
                case 4:
                    obstacle = new GridEntity("Pillar", x, y, z, location, 10, tilePillar1Sprite);
                    break;
                default:
                    obstacle = new GridEntity("Rock", x, y, z, location, wallRock1Sprite);
                    break;

            }
            obstacle.ID = id;
            return obstacle;
        }

        public Vector2 GetEnemyLocation()
        {
            /// Generate a random position on the grid for the enemy. Repeat the process
            /// until a position that is not occupied by an obstacle is obtained.
            bool positionValid = true;
            int enemyX = 0;
            int enemyY = 0;
            Rectangle illegalArea = new Rectangle(GameConstants.WINDOW_WIDTH / GameConstants.TILE_SIZE / 2 - GameConstants.SAFE_ZONE_SIZE / 2, GameConstants.WINDOW_HEIGHT / GameConstants.TILE_SIZE / 2 - GameConstants.SAFE_ZONE_SIZE / 2, GameConstants.SAFE_ZONE_SIZE, GameConstants.SAFE_ZONE_SIZE);
            do
            {
                positionValid = true;
                enemyX = random.Next(0, GameConstants.WINDOW_WIDTH / GameConstants.TILE_SIZE);
                enemyY = random.Next(0, GameConstants.WINDOW_HEIGHT / GameConstants.TILE_SIZE);
                foreach (GridEntity obstacle in obstacles)
                {
                    if (obstacle.XPosition == enemyX && obstacle.YPosition == enemyY)
                    {
                        positionValid = false;
                    }
                }
                if (illegalArea.Contains(new Vector2(enemyX, enemyY)))
                    positionValid = false;
            } while (positionValid == false);

            //Determine the location of the creature based on its position, then create the creature
            Vector2 enemyLocation = new Vector2(enemyX * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2, enemyY * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2);
            return enemyLocation;
        }

        private Texture2D PickRandomTexture(List<Texture2D> textures)
        {
            Texture2D chosenTexture = textures.ElementAt(0);
            int index = random.Next(textures.Count);
            return textures.ElementAt(index);
        }

        public void PickRandomEnemies(List<Enemy> mainEnemyList, List<Enemy> choices, int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int enemyID = random.Next(0, choices.Count);
                Enemy enemy = choices.ElementAt(enemyID).Copy();
                enemy.Location = GetEnemyLocation();
                enemy.UpdateDrawRectangleAnimated();
                mainEnemyList.Add(enemy);
            }
        }

        public Item AddRandomItem()
        {
            int selector = random.Next(1, 13);
            if (selector < 7) //Select weapon
            {
                selector = random.Next(0, 21);
                return ItemFactoryContainer.GetWeaponFromID(selector);
            }
            else if (selector < 12) // Select shield
            {
                selector = random.Next(0, 5);
                return ItemFactoryContainer.GetShieldFromID(selector);
            }
            else
            {
                selector = random.Next(0, 5);
                return ItemFactoryContainer.GetCharmFromID(selector);
            }
        }

        public Item AddItemFromEnemy(List<Enemy> enemies)
        {
            int totalItems = 0;
            int idChosen = 0;
            List<Weapon> weapons = new List<Weapon>();
            List<Shield> shields = new List<Shield>();
            List<Charm> charms = new List<Charm>();

            foreach (Enemy enemy in enemies)
            {
                if (enemy.Weapon1.Type != WeaponType.Blank)
                    weapons.Add(ItemFactoryContainer.GetWeaponFromID(enemy.Weapon1.ID));
                if (enemy.Weapon2.Type != WeaponType.Blank)
                    weapons.Add(ItemFactoryContainer.GetWeaponFromID(enemy.Weapon2.ID));
                if (enemy.Shield1.Type != ShieldType.Blank)
                    shields.Add(ItemFactoryContainer.GetShieldFromID(enemy.Shield1.ID));
                if (enemy.Charm1.Type != CharmType.Blank)
                    charms.Add(ItemFactoryContainer.GetCharmFromID(enemy.Charm1.ID));
            }

            totalItems = weapons.Count + shields.Count + charms.Count;
            List<int> chosenItems = new List<int>();
                bool chosenIdValid = false;
                while (chosenIdValid != true)
                {
                    idChosen = random.Next(totalItems);
                    if (!chosenItems.Contains(idChosen))
                    {
                        chosenIdValid = true;
                        chosenItems.Add(idChosen);
                    }
                    else if (totalItems <= chosenItems.Count)
                    {
                        idChosen = -1;
                        chosenIdValid = true;
                    }

                }
                if (idChosen == -1)
                    return null;
                else if (idChosen < weapons.Count)
                    return (weapons.ElementAt(idChosen));
                else if (idChosen < weapons.Count + shields.Count)
                    return (shields.ElementAt(idChosen - weapons.Count));
                else
                    return (charms.ElementAt(idChosen - (weapons.Count + shields.Count)));
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
            if (staticEntity.CollisionRectangle.Right < 0 || staticEntity.CollisionRectangle.Left > GameConstants.TILES_WIDE * GameConstants.TILE_SIZE)
                isOffScreen = true;
            if (staticEntity.CollisionRectangle.Bottom < 0 || staticEntity.CollisionRectangle.Top > GameConstants.TILES_HIGH * GameConstants.TILE_SIZE)
                isOffScreen = true;
            return isOffScreen;
        }

        private void EdgeCollision(DynamicEntity dynamicEntity)
        {
            int width = GameConstants.TILES_WIDE * GameConstants.TILE_SIZE;
            int height = GameConstants.TILES_HIGH * GameConstants.TILE_SIZE;
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

        private void DynamicToStaticCollision(Creature creature, StaticEntity staticEntity)
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
