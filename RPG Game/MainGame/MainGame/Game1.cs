using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

namespace MainGame
{ 
    /// </summary>
    public enum WeaponType
    {
        Ranged,
        Melee,
        Blank
    }

    public enum EffectType
    {
        Speed,
        Spin,
        Poison,
        Invulnerable,
        Stun,
        Shockwave,
        Noclip,
        Pacify,
        Unhittable,
        Grapple
    }

    public enum ItemType
    {
        Weapon,
        Shield,
        Charm
    }

    public enum GameState
    {
        MainMenu,
        Sandbox,
        Battle,
        Paused,
        PlayerPaused,
        Trade,
        SaveGame,
        LoadGame,
        Editor,
        GameOver
    }

    public enum GameMode
    {
        Arena,
        Adventure
    }

    public enum PathfinderType
    {
        StraightLine,
        BasicPathfinder,
        BasicRanged
    }

    public enum ShieldType
    {
        Blank,
        Blocking,
        Speed,
        RandomDash,
        Close
    }

    public enum CharmType
    {
        Blank,
        LowerCooldown,
        HigherCooldown,
        Burst,
        Speed,
        Lifesteal
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Game
        GameState gameState;
        GameMode gameMode = GameMode.Arena;
        int pauseDurationLeft;
        const int BetweenLevelPause = 1500;
        int level;
        Texture2D pixel;
        Vector2 offset = new Vector2(0, 0);
        #endregion

        #region Constants
        const int WindowWidth = 1000; //Usually 1000, for testing 300
        const int WindowHeight = 750; //Usually 750, for testing 300
        const int ActionBarHeight = 50;
        const int TileSize = 50;
        const int SafeZoneSize = 10; //In tiles. Area in which enemies cannot spawn

        const int NumberOfEnemies = 3;
        const int NumberOfRocks = 30;

        const int ObstacleCollisionLeniency = 10;
        const int enemyCollisionRectangleLeniency = 5;

        const int PlayerHitPoints = 100;
        const float enemyBaseSpeed = 0.15f;
        const float enemyRandomSpeed = 0.5f;
        const float playerBaseSpeed = 0.35f;

        const int NumberOfSaves = 5;

        const int LevelEditorPanSpeed = 5;
        const int SpawnerIDCutoff = 100;

        const bool GodMode = false;
        #endregion

        #region Player
        Player player1;
        Texture2D player1Sprite;
        Vector2 playerStartingLocation;

        Weapon weapon1;
        Weapon weapon2;
        Shield shield1;
        Charm charm1;

        Weapon oldWeapon1;
        Weapon oldWeapon2;
        Shield oldShield1;
        Charm oldCharm1;

        float Health;
        float oldHealth;

        int screenOffset = 0;
        #endregion

        #region Enemies
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

        #endregion

        #region Weapons
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
        #endregion

        #region Shields
        public ShieldFactory shieldFactory;
        Texture2D shieldBasic1;
        Texture2D shieldBasic2;
        Texture2D speedBoost1;
        Texture2D thunderStone;
        Texture2D elvenTrinket;
        #endregion

        #region Charms
        public CharmFactory charmFactory;
        Texture2D charmSprite;
        #endregion

        #region Projectiles
        List<Projectile> friendlyProjectiles = new List<Projectile>();
        List<Projectile> enemyProjectiles = new List<Projectile>();
        LineDrawer lineDrawer;
        Texture2D blueShockwaveBullet;
        Texture2D redShockwaveBullet;
        #endregion

        #region Tiles
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
        #endregion

        #region User interface
        //Font
        SpriteFont font;
        SpriteFont font20;

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

        //Health bar
        Texture2D healthBarSprite;
        #endregion

        #region Main Menu
        List<GridEntity> menuBackground = new List<GridEntity>();

        Texture2D titleSprite;
        Texture2D newGameSprite;
        Texture2D loadGameSprite;
        Texture2D sandboxSprite;
        Texture2D adventureSprite;

        StaticEntity title;
        StaticEntity newGameButton;
        StaticEntity loadGameButton;
        StaticEntity sandboxButton;
        StaticEntity adventureButton;
        #endregion

        #region Sandbox
        StaticEntity background;
        List<ItemBox> itemBoxes;
        List<ItemBox> equippedItemBoxes;
        List<StaticEntity> itemBoxBackgrounds;
        List<StaticEntity> equippedItemBoxBackgrounds;
        StaticEntity playButton;
        StaticEntity startButton;
        StaticEntity backButton;

        Texture2D playButtonSprite;
        Texture2D startButtonSprite;
        Texture2D backButtonSprite;

        string hoveredItemName = "";
        string hoveredItemType = "";
        string hoveredItemDamage = "";
        string hoveredItemCooldown = "";
        string hoveredItemEffects = "";
        string hoveredItemTooltip = "";
        #endregion

        #region Trade Screen
        bool saveGameOverrideEnabled;
        List<ItemBox> tradeItemBoxes;
        List<StaticEntity> tradeItemBoxBackgrounds;
        StaticEntity resetButton;
        StaticEntity saveButton;
        HealthBar playerTradeMenuHealth;
        StaticEntity healingButton;
        bool playerIsHealing;
        #endregion

        #region Keyboard/Mouse Input Variables
        bool escapeButtonPreviouslyPressed = false;
        bool shiftButtonPreviouslyPressed = false;
        bool leftMousePreviouslyPressed = false;
        bool rightMousePreviouslyPressed = false;
        int previousScrollWheelValue = 0;
        #endregion

        #region Saving and Loading
        int fileSelected;

        Texture2D blankButtonSprite;
        Texture2D confirmButtonSprite;

        List<StaticEntity> fileButtons;
        List<String> levelData;
        List<ItemBox> weapon1ItemBoxes;
        List<ItemBox> weapon2ItemBoxes;
        List<ItemBox> shield1ItemBoxes;
        List<ItemBox> charm1ItemBoxes;
        StaticEntity confirmButton;
        #endregion

        #region Adventure Mode
        bool looseCamera;
        int BackgroundType;
        List<Spawner> spawners;
        #endregion

        #region Level Editor
        bool selectingObjects;
        Texture2D valueEditor;
        List<StaticEntity> valueEditors = new List<StaticEntity>();
        int idValue = 1;
        int delayValue = 0;
        int distanceValue = 300;
        int quantityValue = 1;

        List<GridEntity> objectSelection = new List<GridEntity>();
        GridEntity selectedObject = new GridEntity();

        Spawner basicSpawner = new Spawner();
        Spawner createdSpawner = new Spawner();

        int editorZSelected = 1;

        string editorHoveredID = "";
        string editorHoveredZ = "";

        List<StaticEntity> editorMenuInfoBackgrounds = new List<StaticEntity>();
        StaticEntity editorSaveButton;
        #endregion

        #region Game Over Screen
        StaticEntity gameOverButton;
        #endregion

        //Random number generaor support
        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight + ActionBarHeight;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            gameState = GameState.MainMenu;
            escapeButtonPreviouslyPressed = false;
            shiftButtonPreviouslyPressed = false;
            level = 1;
            pixel = Content.Load<Texture2D>("graphics/pixel");
            lineDrawer = new LineDrawer(pixel);

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
                weaponMaul1, weaponMaulLarge,  weaponHammer1, weaponDagger1, weaponSpear1, weaponSpear2, weaponSpear3, weaponShuriken1,
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
            
            #region Main Menu
            tileBrick1Sprite = Content.Load<Texture2D>(@"graphics\TileBrick1");
            titleSprite = Content.Load<Texture2D>(@"graphics/TitleSprite");
            newGameSprite = Content.Load<Texture2D>(@"graphics/NewGameButton");
            loadGameSprite = Content.Load<Texture2D>(@"graphics/LoadGameButton");
            sandboxSprite = Content.Load<Texture2D>(@"graphics/SandboxButton");
            adventureSprite = Content.Load<Texture2D>(@"graphics/AchievementsButton");

            title = new StaticEntity("Title Card", new Vector2(WindowWidth / 2, TileSize * 2), titleSprite);
            newGameButton = new StaticEntity("New Game Button", new Vector2(WindowWidth / 2, TileSize * 11 / 2), newGameSprite);
            loadGameButton = new StaticEntity("Load Game Button", new Vector2(WindowWidth / 2, TileSize * 8), loadGameSprite);
            sandboxButton = new StaticEntity("Sandbox Button", new Vector2(WindowWidth / 2, TileSize * 21 / 2), sandboxSprite);
            adventureButton = new StaticEntity("Achievements Button", new Vector2(WindowWidth / 2, TileSize * 13), adventureSprite);

            TilesWide = (int)Math.Ceiling((decimal)WindowWidth / TileSize);
            TilesHigh = (int)Math.Ceiling((decimal)WindowHeight / TileSize);

            for (int i = 0; i < TilesWide; i++)
            {
                for (int j = 0; j < TilesHigh + 1; j++) //The +1 allows us to cover the action bar
                {
                    Vector2 tileLocation = new Vector2(i * TileSize + TileSize / 2, j * TileSize + TileSize / 2);
                    GridEntity newTile = new GridEntity("Tile", i, j, 0, tileLocation, TileSize, tileBrick1Sprite);
                    menuBackground.Add(newTile);
                }
            }
            #endregion

            #region Sandbox
            actionBarBackground = Content.Load<Texture2D>("graphics/actionBarBackground");

            background = new StaticEntity("Background", new Vector2(WindowWidth / 2, WindowHeight / 2), actionBarBackground);
            itemBoxes = new List<ItemBox>();
            itemBoxBackgrounds = new List<StaticEntity>();
            equippedItemBoxes = new List<ItemBox>();
            equippedItemBoxBackgrounds = new List<StaticEntity>();

            playButtonSprite = Content.Load<Texture2D>("graphics/continueButton");
            startButtonSprite = Content.Load<Texture2D>("graphics/startButton");
            backButtonSprite = Content.Load<Texture2D>("graphics/backButton");

            playButton = new StaticEntity("Play Button", new Vector2(WindowWidth - TileSize * 5 / 2, WindowHeight - TileSize), playButtonSprite);
            startButton = new StaticEntity("Start Button", new Vector2(WindowWidth / 2, WindowHeight / 2), startButtonSprite);
            backButton = new StaticEntity("Back Button", new Vector2(TileSize * 2, WindowHeight - TileSize), backButtonSprite);

            weapon1 = weaponFactory.CreateSword();
            weapon2 = weaponFactory.CreateBow();
            shield1 = shieldFactory.CreateBasicShield();
            charm1 = charmFactory.CreateEmptyCharm();

            #region Weapon Item Boxes
            int weaponBoxY = (int)(TileSize * 1.5);
            int weaponBoxX = (int)(TileSize * 1.5);

            Weapon sword = weaponFactory.CreateSword();
            Vector2 itemBoxSwordLocation = new Vector2(weaponBoxX, weaponBoxY);
            itemBoxes.Add(new ItemBox("Sword", itemBoxSwordLocation, sword.Sprite, sword));
            itemBoxBackgrounds.Add(new StaticEntity("SwordBackground", itemBoxSwordLocation, actionBarBackground));

            Weapon broadSword = weaponFactory.CreateBroadsword();
            Vector2 itemBoxBroadswordLocation = new Vector2(weaponBoxX + TileSize, weaponBoxY);
            itemBoxes.Add(new ItemBox("Broadsword", itemBoxBroadswordLocation, broadSword.Sprite, broadSword));
            itemBoxBackgrounds.Add(new StaticEntity("BroadswordBackground", itemBoxBroadswordLocation, actionBarBackground));

            Weapon bow = weaponFactory.CreateBow();
            Vector2 itemBoxBowLocation = new Vector2(weaponBoxX + TileSize * 2, weaponBoxY);
            itemBoxes.Add(new ItemBox("Bow", itemBoxBowLocation, bow.Sprite, bow));
            itemBoxBackgrounds.Add(new StaticEntity("BowBackground", itemBoxBowLocation, actionBarBackground));

            Weapon spear = weaponFactory.CreateSpear();
            Vector2 itemBoxSpearLocation = new Vector2(weaponBoxX + TileSize * 3, weaponBoxY);
            itemBoxes.Add(new ItemBox("Spear", itemBoxSpearLocation, spear.Sprite, spear));
            itemBoxBackgrounds.Add(new StaticEntity("SpearBackground", itemBoxSpearLocation, actionBarBackground));

            Weapon throwingAxe = weaponFactory.CreateThrowingAxe();
            Vector2 itemBoxThrowingaxeLocation = new Vector2(weaponBoxX + TileSize * 4, weaponBoxY);
            itemBoxes.Add(new ItemBox("ThrowingAxe", itemBoxThrowingaxeLocation, throwingAxe.Sprite, throwingAxe));
            itemBoxBackgrounds.Add(new StaticEntity("ThrowingAxeBackground", itemBoxThrowingaxeLocation, actionBarBackground));

            Weapon throwingDagger = weaponFactory.CreateThrowingDagger();
            Vector2 itemBoxThrowingDaggerLocation = new Vector2(weaponBoxX + TileSize * 5, weaponBoxY);
            itemBoxes.Add(new ItemBox("Throwing Dagger", itemBoxThrowingDaggerLocation, throwingDagger.Sprite, throwingDagger));
            itemBoxBackgrounds.Add(new StaticEntity("ThrowingDaggerBackground", itemBoxThrowingDaggerLocation, actionBarBackground));

            Weapon iceBow = weaponFactory.CreateIceBow();
            Vector2 itemBoxIcebowLocation = new Vector2(weaponBoxX + TileSize * 6, weaponBoxY);
            itemBoxes.Add(new ItemBox("Icebow", itemBoxIcebowLocation, iceBow.Sprite, iceBow));
            itemBoxBackgrounds.Add(new StaticEntity("IcebowBackground", itemBoxIcebowLocation, actionBarBackground));

            Weapon dwarvenAxe = weaponFactory.CreateDwarvenAxe();
            Vector2 itemBoxDwarvenAxeLocation = new Vector2(weaponBoxX + TileSize * 7, weaponBoxY);
            itemBoxes.Add(new ItemBox("DwarvenAxe", itemBoxDwarvenAxeLocation, dwarvenAxe.Sprite, dwarvenAxe));
            itemBoxBackgrounds.Add(new StaticEntity("DwarvenAxeBackground", itemBoxDwarvenAxeLocation, actionBarBackground));

            Weapon maul = weaponFactory.CreateMaul();
            Vector2 itemBoxMaulLocation = new Vector2(weaponBoxX + TileSize * 8, weaponBoxY);
            itemBoxes.Add(new ItemBox("Maul", itemBoxMaulLocation, maul.Sprite, maul));
            itemBoxBackgrounds.Add(new StaticEntity("MaulBackground", itemBoxMaulLocation, actionBarBackground));

            Weapon hammer = weaponFactory.CreateHammer();
            Vector2 itemBoxHammerLocation = new Vector2(weaponBoxX + TileSize * 9, weaponBoxY);
            itemBoxes.Add(new ItemBox("Maul", itemBoxHammerLocation, hammer.Sprite, hammer));
            itemBoxBackgrounds.Add(new StaticEntity("MaulBackground", itemBoxHammerLocation, actionBarBackground));

            Weapon throwingSpear = weaponFactory.CreateThrowingSpear();
            Vector2 itemBoxThrowingSpearLocation = new Vector2(weaponBoxX + TileSize * 10, weaponBoxY);
            itemBoxes.Add(new ItemBox("ThrowingSpear", itemBoxThrowingSpearLocation, throwingSpear.Sprite, throwingSpear));
            itemBoxBackgrounds.Add(new StaticEntity("ThrowingSpearBackground", itemBoxThrowingSpearLocation, actionBarBackground));

            Weapon jungleSpear = weaponFactory.CreateJungleSpear();
            Vector2 itemBoxJungleSpearLocation = new Vector2(weaponBoxX + TileSize * 11, weaponBoxY);
            itemBoxes.Add(new ItemBox("JungleSpear", itemBoxJungleSpearLocation, jungleSpear.Sprite, jungleSpear));
            itemBoxBackgrounds.Add(new StaticEntity("JungleSpearBackground", itemBoxJungleSpearLocation, actionBarBackground));

            Weapon fireball = weaponFactory.CreateFireball();
            Vector2 itemBoxFireballLocation = new Vector2(weaponBoxX + TileSize * 12, weaponBoxY);
            itemBoxes.Add(new ItemBox("Fireball", itemBoxFireballLocation, fireball.Sprite, fireball));
            itemBoxBackgrounds.Add(new StaticEntity("FireballBackground", itemBoxFireballLocation, actionBarBackground));

            Weapon superAxe = weaponFactory.CreateMaverick();
            Vector2 itemBoxSuperAxeLocation = new Vector2(weaponBoxX + TileSize * 13, weaponBoxY);
            itemBoxes.Add(new ItemBox("Superaxe", itemBoxSuperAxeLocation, superAxe.Sprite, superAxe));
            itemBoxBackgrounds.Add(new StaticEntity("SuperaxeBackground", itemBoxSuperAxeLocation, actionBarBackground));

            Weapon firebolt = weaponFactory.CreateFirebolt();
            Vector2 itemBoxFireboltLocation = new Vector2(weaponBoxX + TileSize * 14, weaponBoxY);
            itemBoxes.Add(new ItemBox("Firebolt", itemBoxFireboltLocation, firebolt.Sprite, firebolt));
            itemBoxBackgrounds.Add(new StaticEntity("FireboltBackground", itemBoxFireboltLocation, actionBarBackground));

            Weapon firework = weaponFactory.CreateFirework();
            Vector2 itemBoxFireworkLocation = new Vector2(weaponBoxX + TileSize * 15, weaponBoxY);
            itemBoxes.Add(new ItemBox("Firework", itemBoxFireworkLocation, firework.Sprite, firework));
            itemBoxBackgrounds.Add(new StaticEntity("FireworkBackground", itemBoxFireworkLocation, actionBarBackground));

            Weapon shuriken1 = weaponFactory.CreateBoteng();
            Vector2 itemBoxShuriken1Location = new Vector2(weaponBoxX + TileSize * 16, weaponBoxY);
            itemBoxes.Add(new ItemBox("Shuriken1", itemBoxShuriken1Location, shuriken1.Sprite, shuriken1));
            itemBoxBackgrounds.Add(new StaticEntity("Shuriken1Background", itemBoxShuriken1Location, actionBarBackground));

            Weapon shuriken2 = weaponFactory.CreateHira();
            Vector2 itemBoxShuriken2Location = new Vector2(weaponBoxX + TileSize * 17, weaponBoxY);
            itemBoxes.Add(new ItemBox("Shuriken2", itemBoxShuriken2Location, shuriken2.Sprite, shuriken2));
            itemBoxBackgrounds.Add(new StaticEntity("Shuriken2Background", itemBoxShuriken2Location, actionBarBackground));

            Weapon shuriken3 = weaponFactory.CreateTaago();
            Vector2 itemBoxShuriken3Location = new Vector2(weaponBoxX + TileSize * 18, weaponBoxY);
            itemBoxes.Add(new ItemBox("Shuriken3", itemBoxShuriken3Location, shuriken3.Sprite, shuriken3));
            itemBoxBackgrounds.Add(new StaticEntity("Shuriken3Background", itemBoxShuriken3Location, actionBarBackground));

            Weapon plasmaBolt = weaponFactory.CreatePlasmaBolt();
            Vector2 itemBoxPlasmaBoltLocation = new Vector2(weaponBoxX + TileSize * 15, weaponBoxY + TileSize);
            itemBoxes.Add(new ItemBox("plasmaBolt", itemBoxPlasmaBoltLocation, plasmaBolt.Sprite, plasmaBolt));
            itemBoxBackgrounds.Add(new StaticEntity("plasmaBoltBackground", itemBoxPlasmaBoltLocation, actionBarBackground));

            Weapon grapple = weaponFactory.CreateGrapple();
            Vector2 itemBoxGrappleLocation = new Vector2(weaponBoxX + TileSize * 16, weaponBoxY + TileSize);
            itemBoxes.Add(new ItemBox("Grapple", itemBoxGrappleLocation, grapple.Sprite, grapple));
            itemBoxBackgrounds.Add(new StaticEntity("grappleBackground", itemBoxGrappleLocation, actionBarBackground));
            #endregion

            #region Shield Item Boxes
            int shieldBoxY = (int)(TileSize * 3.5);
            int shieldBoxX = (int)(TileSize * 1.5);

            Shield basicShield = shieldFactory.CreateBasicShield();
            Vector2 itemBoxBasicShieldLocation = new Vector2(shieldBoxX, shieldBoxY);
            itemBoxes.Add(new ItemBox("BasicShield", itemBoxBasicShieldLocation, basicShield.Sprite, basicShield));
            itemBoxBackgrounds.Add(new StaticEntity("BasicShieldBackground", itemBoxBasicShieldLocation, actionBarBackground));

            Shield speedBoost = shieldFactory.CreateSpeedboost();
            Vector2 itemBoxSpeedboostLocation = new Vector2(shieldBoxX + TileSize, shieldBoxY);
            itemBoxes.Add(new ItemBox("Speedboost", itemBoxSpeedboostLocation, speedBoost.Sprite, speedBoost));
            itemBoxBackgrounds.Add(new StaticEntity("BasicShieldBackground", itemBoxSpeedboostLocation, actionBarBackground));

            Shield thunderStoneItem = shieldFactory.CreateThunderStone();
            Vector2 itemBoxThunderStoneLocation = new Vector2(shieldBoxX + TileSize * 2, shieldBoxY);
            itemBoxes.Add(new ItemBox("ThunderStone", itemBoxThunderStoneLocation, thunderStoneItem.Sprite, thunderStoneItem));
            itemBoxBackgrounds.Add(new StaticEntity("ThunderStoneBackground", itemBoxThunderStoneLocation, actionBarBackground));

            Shield towerShield = shieldFactory.CreateTowerShield();
            Vector2 itemBoxTowerShieldLocation = new Vector2(shieldBoxX + TileSize * 3, shieldBoxY);
            itemBoxes.Add(new ItemBox("TowerShield", itemBoxTowerShieldLocation, towerShield.Sprite, towerShield));
            itemBoxBackgrounds.Add(new StaticEntity("TowerShieldBackground", itemBoxTowerShieldLocation, actionBarBackground));

            Shield elvenTrinketItem = shieldFactory.CreateElvenTrinket();
            Vector2 itemBoxElvenTrinketLocation = new Vector2(shieldBoxX + TileSize * 4, shieldBoxY);
            itemBoxes.Add(new ItemBox("ElvenTrinket", itemBoxElvenTrinketLocation, elvenTrinketItem.Sprite, elvenTrinketItem));
            itemBoxBackgrounds.Add(new StaticEntity("ElvenTrinketBackground", itemBoxElvenTrinketLocation, actionBarBackground));

            Shield bullrushItem = shieldFactory.CreateBullrush();
            Vector2 itemBoxBullrushLocation = new Vector2(shieldBoxX + TileSize * 5, shieldBoxY);
            itemBoxes.Add(new ItemBox("Bullrush", itemBoxBullrushLocation, bullrushItem.Sprite, bullrushItem));
            itemBoxBackgrounds.Add(new StaticEntity("BullrushBackground", itemBoxBullrushLocation, actionBarBackground));
            #endregion

            #region Charm Item Boxes
            int charmBoxY = (int)(TileSize * 5.5);
            int charmBoxX = (int)(TileSize * 1.5);

            Charm burstCharm = charmFactory.CreateBurstCharm();
            Vector2 itemBoxBurstCharmLocation = new Vector2(charmBoxX, charmBoxY);
            itemBoxes.Add(new ItemBox("Burst Charm", itemBoxBurstCharmLocation, burstCharm.Sprite, burstCharm));
            itemBoxBackgrounds.Add(new StaticEntity("Burst Charm Background", itemBoxBurstCharmLocation, actionBarBackground));

            Charm cooldownCharm = charmFactory.CreateLowerCooldown();
            Vector2 itemBoxCoolCharmLocation = new Vector2(charmBoxX + TileSize, charmBoxY);
            itemBoxes.Add(new ItemBox("Cool Charm", itemBoxCoolCharmLocation, cooldownCharm.Sprite, cooldownCharm));
            itemBoxBackgrounds.Add(new StaticEntity("Cool Charm Background", itemBoxCoolCharmLocation, actionBarBackground));

            Charm damageCharm = charmFactory.CreateHigherCooldown();
            Vector2 itemBoxDamageCharmLocation = new Vector2(charmBoxX + TileSize * 2, charmBoxY);
            itemBoxes.Add(new ItemBox("Damage Charm", itemBoxDamageCharmLocation, damageCharm.Sprite, damageCharm));
            itemBoxBackgrounds.Add(new StaticEntity("Damage Charm Background", itemBoxDamageCharmLocation, actionBarBackground));

            Charm speedCharm = charmFactory.CreateSpeedCharm();
            Vector2 itemBoxSpeedCharmLocation = new Vector2(charmBoxX + TileSize * 3, charmBoxY);
            itemBoxes.Add(new ItemBox("speedCharm", itemBoxSpeedCharmLocation, speedCharm.Sprite, speedCharm));
            itemBoxBackgrounds.Add(new StaticEntity("speedCharm Background", itemBoxSpeedCharmLocation, actionBarBackground));

            Charm vampiricCharm = charmFactory.CreateLifestealCharm();
            Vector2 itemBoxVampiricCharmLocation = new Vector2(charmBoxX + TileSize * 4, charmBoxY);
            itemBoxes.Add(new ItemBox("vampiricCharm", itemBoxVampiricCharmLocation, vampiricCharm.Sprite, vampiricCharm));
            itemBoxBackgrounds.Add(new StaticEntity("vampiricCharm Background", itemBoxVampiricCharmLocation, actionBarBackground));
            #endregion

            #region Equipped Item Boxes
            int equipBoxX = (int)(TileSize * 1.5);
            int equipBoxY = (int)(WindowHeight - TileSize * 2.5);

            Vector2 box1Location = new Vector2(equipBoxX, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Weapon1", box1Location, weapon1.Sprite, weapon1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Weapon1", box1Location, actionBarBackground));

            Vector2 box2Location = new Vector2(equipBoxX + TileSize * 1, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Weapon2", box2Location, weapon2.Sprite, weapon2));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Weapon2", box2Location, actionBarBackground));

            Vector2 box3Location = new Vector2(equipBoxX + TileSize * 2, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Shield1", box3Location, shield1.Sprite, shield1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Shield1", box3Location, actionBarBackground));

            Vector2 box4Location = new Vector2(equipBoxX + TileSize * 3, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Charm1", box4Location, charm1.Sprite, charm1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Charm1", box4Location, actionBarBackground));
            #endregion

            #endregion
            
            #region Create Player
            //Health bar sprite
            healthBarSprite = Content.Load<Texture2D>("graphics/HealthBar2");

            //Load sprite
            player1Sprite = Content.Load<Texture2D>(@"graphics\PlayerSprite1");
            playerStartingLocation = new Vector2(WindowWidth / 2, WindowHeight / 2);
            Health = PlayerHitPoints;
            player1 = new Player("Player", playerStartingLocation, player1Sprite, new Vector2(0, 0), PlayerHitPoints, healthBarSprite, weapon1, weapon2, shield1, charm1, playerBaseSpeed);
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

            pathfinder = new Pathfinding(TilesWide, TilesHigh, TileSize, weaponDagger1, PathfinderType.BasicPathfinder);
            enemyFactory = new EnemyFactory(weaponFactory, shieldFactory, charmFactory, pathfinder, healthBarSprite, enemySprite1, enemyGladiator1, enemyGladiator2, enemyGladiator2Large,
                enemyGoblin1, enemySkeleton1, enemySkeleton2, enemyFireSpider1, enemyBlueSpider1);

            #endregion

            #region Create User Interface
            font = Content.Load<SpriteFont>("font/font");
            font20 = Content.Load<SpriteFont>("font/font20");
            score = 0;

            Vector2 firstIconLocation = new Vector2((TilesWide - 0.5f) * TileSize, (TilesHigh + 0.5f) * TileSize);
            Texture2D firstIconSprite;
            firstIconSprite = charm1.Sprite;

            Vector2 secondIconLocation = new Vector2(firstIconLocation.X - TileSize, firstIconLocation.Y);
            Texture2D secondIconSprite = shield1.Sprite;

            Vector2 thirdIconLocation = new Vector2(secondIconLocation.X - TileSize, secondIconLocation.Y);
            Texture2D thirdIconSprite = weapon2.Sprite;


            Vector2 fourthIconLocation = new Vector2(thirdIconLocation.X - TileSize, thirdIconLocation.Y);
            Texture2D fourthIconSprite = weapon1.Sprite;

            charm1Icon = new StaticEntity("Charm 1 Icon", firstIconLocation, firstIconSprite);
            shield1Icon = new StaticEntity("Shield 1 Icon", secondIconLocation, secondIconSprite);
            weapon2Icon = new StaticEntity("Weapon 1 Icon", thirdIconLocation, thirdIconSprite);
            weapon1Icon = new StaticEntity("Weapon 2 Icon", fourthIconLocation, fourthIconSprite);

            icon1Background = new StaticEntity("Icon 1 Background", firstIconLocation, actionBarBackground);
            icon2Background = new StaticEntity("Icon 2 Background", secondIconLocation, actionBarBackground);
            icon3Background = new StaticEntity("Icon 3 Background", thirdIconLocation, actionBarBackground);
            icon4Background = new StaticEntity("Icon 4 Background", fourthIconLocation, actionBarBackground);
            #endregion

            #region Create Trade Window
            saveGameOverrideEnabled = false;
            tradeItemBoxes = new List<ItemBox>();
            tradeItemBoxBackgrounds = new List<StaticEntity>();

            playerTradeMenuHealth = new HealthBar("Trade Menu Health", new Vector2(TileSize, WindowHeight - (TileSize * 3 + 15) ), healthBarSprite);

            Vector2 firstBoxLocation = new Vector2(TileSize / 2 + TileSize * 1, TileSize * 3);
            Vector2 secondBoxLocation = new Vector2(TileSize / 2 + TileSize * 3, TileSize * 3);
            Vector2 thirdBoxLocation = new Vector2(TileSize / 2 + TileSize * 5, TileSize * 3);
            Vector2 fourthBoxLocation = new Vector2(TileSize / 2 + TileSize * 7, TileSize * 3);
            Vector2 fifthBoxLocation = new Vector2(TileSize / 2 + TileSize * 9, TileSize * 3);

            tradeItemBoxes.Add(new ItemBox("item1", firstBoxLocation, actionBarBackground, weaponFactory.CreateSword()));
            tradeItemBoxes.Add(new ItemBox("item2", secondBoxLocation, actionBarBackground, weaponFactory.CreateSword()));
            tradeItemBoxes.Add(new ItemBox("item3", thirdBoxLocation, actionBarBackground, weaponFactory.CreateSword()));
            healingButton = new StaticEntity("Healing", fourthBoxLocation, Content.Load<Texture2D>("graphics/Potions"));
            resetButton = new StaticEntity("Reset Button", fifthBoxLocation, Content.Load<Texture2D>("graphics/resetButton"));
            Texture2D saveButtonSprite = Content.Load<Texture2D>("graphics/saveButton");
            saveButton = new StaticEntity("Save Button", new Vector2(WindowWidth - TileSize * 3, TileSize * 2), saveButtonSprite);

            tradeItemBoxBackgrounds.Add(new StaticEntity("item1Background", firstBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item2Background", secondBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item3Background", thirdBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item4Background", fourthBoxLocation, actionBarBackground));
            playerIsHealing = false;

            #endregion

            #region Create Save/Load Screen
            fileSelected = -1;
            blankButtonSprite = Content.Load<Texture2D>("graphics/blankButton");
            confirmButtonSprite = Content.Load<Texture2D>("graphics/confirmButton");
            fileButtons = new List<StaticEntity>();
            levelData = new List<String>();
            weapon1ItemBoxes = new List<ItemBox>();
            weapon2ItemBoxes = new List<ItemBox>();
            shield1ItemBoxes = new List<ItemBox>();
            charm1ItemBoxes = new List<ItemBox>();
            for (int i = 0; i < NumberOfSaves; i++)
            {
                fileButtons.Add(new StaticEntity("Button " + i.ToString(), new Vector2(TileSize * 2, TileSize * 3 + i * TileSize * 2), blankButtonSprite));
                levelData.Add("");
                weapon1ItemBoxes.Add(new ItemBox("Weapon1 File " + i.ToString(), new Vector2(TileSize * 7, TileSize * 3 + i * TileSize * 2)));
                weapon2ItemBoxes.Add(new ItemBox("Weapon2 File " + i.ToString(), new Vector2(TileSize * 8, TileSize * 3 + i * TileSize * 2)));
                shield1ItemBoxes.Add(new ItemBox("Shield1 File " + i.ToString(), new Vector2(TileSize * 9, TileSize * 3 + i * TileSize * 2)));
                charm1ItemBoxes.Add(new ItemBox("Charm1 File " + i.ToString(), new Vector2(TileSize * 10, TileSize * 3 + i * TileSize * 2)));
            }
            confirmButton = new StaticEntity("Confirm Button", new Vector2(TileSize * 5, WindowHeight - TileSize), confirmButtonSprite);
            #endregion

            #region Adventure Mode
            looseCamera = true;
            spawners = new List<Spawner>();
            #endregion

            #region Level editor
            selectedObject = GetObstacleFromID(1,2,0,0);
            objectSelection.Add(GetObstacleFromID(1, 3, 0, 1));
            objectSelection.Add(GetObstacleFromID(2, 4, 0, 1));
            objectSelection.Add(GetObstacleFromID(3, 5, 0, 1));
            objectSelection.Add(GetObstacleFromID(4, 6, 0, 1));

            Texture2D valueEditor = Content.Load<Texture2D>(@"graphics\ValueEditor1");

            valueEditors.Add(new StaticEntity("Editor1", new Vector2(WindowWidth - valueEditor.Width / 2, WindowHeight / 2), valueEditor));
            valueEditors.Add(new StaticEntity("Editor2", new Vector2(WindowWidth - valueEditor.Width / 2, WindowHeight / 2 + valueEditor.Height), valueEditor));
            valueEditors.Add(new StaticEntity("Editor3", new Vector2(WindowWidth - valueEditor.Width / 2, WindowHeight / 2 + valueEditor.Height * 2), valueEditor));
            valueEditors.Add(new StaticEntity("Editor4", new Vector2(WindowWidth - valueEditor.Width / 2, WindowHeight / 2 + valueEditor.Height * 3), valueEditor));

            basicSpawner = new Spawner("Spawner",7, 0, 0, new Vector2(7 * TileSize + TileSize / 2, TileSize / 2), charmSprite, 101, 200, 1, 0);
            createdSpawner = basicSpawner.Copy();

            editorMenuInfoBackgrounds.Add(new StaticEntity("TopLeft", new Vector2(0, 0), actionBarBackground));
            editorMenuInfoBackgrounds.Add(new StaticEntity("Left", new Vector2(0, 0), actionBarBackground));
            editorSaveButton = new StaticEntity("Save Button", new Vector2(WindowWidth - saveButtonSprite.Width, WindowHeight - saveButtonSprite.Height), saveButtonSprite);
            #endregion

            #region Game Over Screen
            gameOverButton = new StaticEntity("BackToMainMenu", new Vector2(WindowWidth / 2, WindowHeight / 2), playButtonSprite);
            #endregion

            //DEBUG:
            foreach (GridEntity obstacle in obstacles)
                Debug.WriteLine(obstacle.XPosition.ToString() + " " + obstacle.YPosition.ToString());
            Debug.WriteLine("Player: " + (player1.Location.X / TileSize ).ToString() + " " + (player1.Location.Y / TileSize).ToString());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            Vector2 trueMousePosition = new Vector2(mouse.X - offset.X, mouse.Y - offset.Y);

            #region Main Menu Update Logic
            if (gameState == GameState.MainMenu)
            {
                if (newGameButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
                {
                    gameMode = GameMode.Arena;
                    weapon1 = weaponFactory.CreateSword();
                    weapon2 = weaponFactory.CreateBow();
                    shield1 = shieldFactory.CreateBasicShield();
                    charm1 = charmFactory.CreateEmptyCharm();
                    InitiateCombat(gameTime);
                }
                else if (loadGameButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
                {
                    gameMode = GameMode.Arena;
                    gameState = GameState.LoadGame;
                    fileSelected = -1;
                    for (int i = 0; i < NumberOfSaves; i++)
                        CheckGameData(i, levelData, weapon1ItemBoxes, weapon2ItemBoxes, shield1ItemBoxes, charm1ItemBoxes);

                }
                else if (sandboxButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
                {
                    gameState = GameState.Sandbox;
                }
                else if (adventureButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
                {
                    gameMode = GameMode.Adventure;
                    if (!keyboard.IsKeyDown(Keys.LeftShift))
                    {
                        gameState = GameState.Battle;
                        InitiateCombat(gameTime);
                    }
                    else
                    {
                        gameState = GameState.Editor;
                        leftMousePreviouslyPressed = true;
                        InitiateLevelEditor();
                    }
                    
                }
            }
            #endregion

            #region Sandbox Update Logic
            if (gameState == GameState.Sandbox)
            {
                DoItemBoxUpdate(mouse, itemBoxes, itemBoxBackgrounds, equippedItemBoxes);
                if (backButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.MainMenu;
                }
            }
            #endregion

            #region Trade menu update logic
            if (gameState == GameState.Trade)
            {
                if (keyboard.IsKeyDown(Keys.Home))
                    saveGameOverrideEnabled = true;
                if (resetButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    weapon1 = oldWeapon1;
                    weapon2 = oldWeapon2;
                    shield1 = oldShield1;
                    charm1 = oldCharm1;
                    playerIsHealing = false;
                }
                if (healingButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && oldCharm1.Type != CharmType.Lifesteal)
                {
                    weapon1 = oldWeapon1;
                    weapon2 = oldWeapon2;
                    shield1 = oldShield1;
                    charm1 = oldCharm1;
                    playerIsHealing = true;
                }
                if (((level - 1) % 10 == 0 || saveGameOverrideEnabled) && saveButton.CollisionRectangle.Contains(mouse.Position)
                    && mouse.LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.SaveGame;
                    fileSelected = -1;
                    for (int i = 0; i < NumberOfSaves; i++)
                        CheckGameData(i, levelData, weapon1ItemBoxes, weapon2ItemBoxes, shield1ItemBoxes, charm1ItemBoxes);
                }
                DoItemBoxUpdate(mouse, tradeItemBoxes, tradeItemBoxBackgrounds, equippedItemBoxes);

                //playerTradeMenuHealth.Update(gameTime, player1); Health bar does not need to update as it is not moving
                if (playerIsHealing)
                {
                    Health = oldHealth + player1.MaxHitPoints / 2;
                    if (Health > player1.MaxHitPoints)
                        Health = player1.MaxHitPoints;
                }
                else
                    Health = oldHealth;

                player1.HitPoints = Health;

            }
            #endregion

            #region Initiate combat logic
            if (gameState == GameState.Trade || gameState == GameState.Sandbox)
            {
                if (playButton.CollisionRectangle.Contains(new Vector2(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
                {
                    InitiateCombat(gameTime);
                }
            }
            #endregion

            #region Battle Update Logic
            else if (gameState == GameState.Battle)
            {

                #region Player pausing logic
                if (keyboard.IsKeyDown(Keys.Escape) && !escapeButtonPreviouslyPressed)
                    gameState = GameState.PlayerPaused;
                #endregion

                #region Next level logic
                if (enemies.Count == 0 && gameMode == GameMode.Arena)
                {
                    //Initialize trade window
                    //Add the item boxes that are to be filled from enemies to a list
                    //Call AddItemFromEnemy with that list
                    //Get all the items from deadenemies and add them at random to the item boxes
                    List<ItemBox> enemyFilledBoxes = new List<ItemBox>();
                    enemyFilledBoxes.Add(tradeItemBoxes.ElementAt(0));
                    enemyFilledBoxes.Add(tradeItemBoxes.ElementAt(1));
                    AddItemFromEnemy(enemyFilledBoxes, deadEnemies);
                    AddRandomItem(tradeItemBoxes.ElementAt(2), weaponFactory, shieldFactory);

                    deadEnemies.Clear();
                    friendlyProjectiles.Clear();
                    enemyProjectiles.Clear();

                    player1.XLocation = WindowWidth / 2;
                    player1.YLocation = WindowHeight / 2;

                    player1.Weapon1.CooldownRemaining = 0;
                    player1.Weapon2.CooldownRemaining = 0;
                    player1.Shield1.CooldownRemaining = 0;

                    player1.ActiveEffects.Clear();

                    gameState = GameState.Trade;
                    saveGameOverrideEnabled = false;

                    oldWeapon1 = weapon1;
                    oldWeapon2 = weapon2;
                    oldShield1 = shield1;
                    oldCharm1 = charm1;
                    Health = player1.HitPoints;
                    oldHealth = player1.HitPoints;
                    playerIsHealing = false;

                    level += 1;

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

                if (player1.InvincibilityFramesRemaining <= 0 && GodMode == false)
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
                if (gameMode == GameMode.Adventure)
                {
                    if (keyboard.IsKeyDown(Keys.LeftShift) && !shiftButtonPreviouslyPressed)
                    {
                        if (looseCamera == false)
                            looseCamera = true;
                        else
                            looseCamera = false;
                    }
                    foreach (Spawner spawner in spawners)
                    {
                        spawner.Update(gameTime,player1.Location, enemies, enemyFactory);
                    }

                }
                #endregion

                #region Game Over
                if (player1.HitPoints <= 0)
                {
                    gameState = GameState.GameOver;

                }
                #endregion
            }
            #endregion

            #region Automatic Pausing logic
            else if (gameState == GameState.Paused)
            {
                if (pauseDurationLeft > 0)
                {
                    pauseDurationLeft -= gameTime.ElapsedGameTime.Milliseconds;
                    if (pauseDurationLeft <= 0)
                        gameState = GameState.Battle;
                }
            }
            #endregion

            #region Player pausing logic
            else if (gameState == GameState.PlayerPaused)
            {
                if (keyboard.IsKeyDown(Keys.Escape) && !escapeButtonPreviouslyPressed)
                {
                    gameState = GameState.Paused;
                    pauseDurationLeft = BetweenLevelPause;
                }
            }
            #endregion

            #region Save/Load Game Update Logic
            if (gameState == GameState.SaveGame)
            {
                foreach (StaticEntity button in fileButtons)
                {
                    if (button.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        fileSelected = fileButtons.IndexOf(button);
                    }
                }
                if (backButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.Trade;
                }
                else if (confirmButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    SaveGame(fileSelected);
                    gameState = GameState.Trade;
                }
            }
            if (gameState == GameState.LoadGame)
            {
                foreach (StaticEntity button in fileButtons)
                {
                    if (button.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        fileSelected = fileButtons.IndexOf(button);
                    }
                }
                if (backButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.MainMenu;
                }
                else if (confirmButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    if (LoadGame(fileSelected) == true)
                        gameState = GameState.Trade;
                }
                
            }
            #endregion

            #region Level Editor Update Logic
            if (gameState == GameState.Editor)
            {
                #region Screen panning
                if (keyboard.IsKeyDown(Keys.W))
                    offset.Y += LevelEditorPanSpeed;
                if (keyboard.IsKeyDown(Keys.A))
                    offset.X += LevelEditorPanSpeed;
                if (keyboard.IsKeyDown(Keys.D))
                    offset.X -= LevelEditorPanSpeed;
                if (keyboard.IsKeyDown(Keys.S))
                    offset.Y -= LevelEditorPanSpeed;
                if (keyboard.IsKeyDown(Keys.Escape))
                    selectingObjects = true;
                else
                    selectingObjects = false;
                #endregion

                #region Adjusting values
                int changeInScroll = 0;
                if (mouse.ScrollWheelValue < previousScrollWheelValue)
                    changeInScroll = -1;
                else if (mouse.ScrollWheelValue > previousScrollWheelValue)
                    changeInScroll = 1;
                if (changeInScroll != 0)
                {
                    if (selectingObjects && selectedObject.ID > SpawnerIDCutoff)
                    {
                        if (valueEditors.ElementAt(0).CollisionRectangle.Contains(mouse.Position))
                            idValue += changeInScroll;
                        else if (valueEditors.ElementAt(1).CollisionRectangle.Contains(mouse.Position))
                            distanceValue += changeInScroll * 50;
                        else if (valueEditors.ElementAt(2).CollisionRectangle.Contains(mouse.Position))
                            quantityValue += changeInScroll;
                        else if (valueEditors.ElementAt(3).CollisionRectangle.Contains(mouse.Position))
                            delayValue += changeInScroll * 500;
                    }
                    else
                        editorZSelected += changeInScroll;
                }
                #endregion

                #region Object info
                editorHoveredID = "";
                editorHoveredZ = "";
                foreach (GridEntity obstacle in obstacles)
                {
                    if (obstacle.CollisionRectangle.Contains(trueMousePosition))
                    {
                        editorHoveredID = obstacle.ID.ToString();
                        editorHoveredZ = obstacle.ZPosition.ToString();
                    }
                }
                foreach (Spawner spawner in spawners)
                {
                    if (spawner.CollisionRectangle.Contains(trueMousePosition))
                    {
                        editorHoveredID = spawner.ID.ToString();
                        editorHoveredZ = spawner.ZPosition.ToString();
                    }
                }
                #endregion

                if (mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
                {
                    if (selectingObjects)
                    {
                        foreach (GridEntity obj in objectSelection)
                        {
                            if (obj.CollisionRectangle.Contains(mouse.Position))
                            {
                                selectedObject.ID = obj.ID;
                                selectedObject.Sprite = obj.Sprite;
                                selectedObject.UpdateDrawRectangle();
                            }
                        }
                        if (basicSpawner.CollisionRectangle.Contains(mouse.Position))
                            selectedObject.ID = basicSpawner.ID;
                        if (editorSaveButton.CollisionRectangle.Contains(mouse.Position))
                            SaveLevel("Level" + level);
                    }
                    else
                    {
                        Vector2 gridMouse = ConvertToGrid(trueMousePosition);
                        DeleteGridEntityAtLocation(obstacles, trueMousePosition);
                        DeleteSpawnerAtLocation(spawners, trueMousePosition);
                        if (selectedObject.ID < SpawnerIDCutoff)
                            obstacles.Add(GetObstacleFromID(selectedObject.ID, (int)gridMouse.X, (int)gridMouse.Y, editorZSelected));
                        else
                        {
                            createdSpawner.XPosition = (int)ConvertToGrid(trueMousePosition).X;
                            createdSpawner.YPosition = (int)ConvertToGrid(trueMousePosition).Y;
                            createdSpawner.Location = new Vector2(createdSpawner.XPosition * TileSize + TileSize / 2, createdSpawner.YPosition * TileSize + TileSize / 2);
                            createdSpawner.UpdateDrawRectangle();
                            createdSpawner.EnemyID = idValue;
                            createdSpawner.ID = idValue + SpawnerIDCutoff;
                            createdSpawner.DistanceActivated = distanceValue;
                            createdSpawner.Quantity = quantityValue;
                            createdSpawner.Delay = delayValue;
                            spawners.Add(createdSpawner.Copy());
                        }
                    }
                }
                if (mouse.RightButton == ButtonState.Pressed && !rightMousePreviouslyPressed)
                {
                    DeleteGridEntityAtLocation(obstacles, trueMousePosition);
                    DeleteSpawnerAtLocation(spawners, trueMousePosition);
                }


            }
            #endregion

            #region Update keyboard states
            if (keyboard.IsKeyDown(Keys.Escape))
            escapeButtonPreviouslyPressed = true;
            else
                escapeButtonPreviouslyPressed = false;

            if (keyboard.IsKeyDown(Keys.LeftShift))
                shiftButtonPreviouslyPressed = true;
            else
                shiftButtonPreviouslyPressed = false;

            if (mouse.LeftButton == ButtonState.Pressed)
                leftMousePreviouslyPressed = true;
            else
                leftMousePreviouslyPressed = false;

            if (mouse.RightButton == ButtonState.Pressed)
                rightMousePreviouslyPressed = true;
            else
                rightMousePreviouslyPressed = false;
            #endregion

            #region Game Over 
            if (gameState == GameState.GameOver)
            {
                if (gameOverButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    gameState = GameState.MainMenu;
                    Health = PlayerHitPoints;
                    level = 1;
                    score = 0;
                    obstacles.Clear();
                    enemies.Clear();
                    weapon1 = weaponFactory.CreateSword();
                    weapon2 = weaponFactory.CreateBow();
                    shield1 = shieldFactory.CreateBasicShield();
                    charm1 = charmFactory.CreateEmptyCharm();
                    player1 = new Player("Player", playerStartingLocation, player1Sprite, new Vector2(0, 0), PlayerHitPoints, healthBarSprite, weapon1, weapon2, shield1, charm1, playerBaseSpeed);
                }
            }
            #endregion
            previousScrollWheelValue = mouse.ScrollWheelValue;

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            MouseState mouse = Mouse.GetState();

            Vector2 trueMousePosition = new Vector2(mouse.X - offset.X, mouse.Y - offset.Y);

            #region Main Menu
            if (gameState == GameState.MainMenu)
            {
                foreach (GridEntity tile in menuBackground)
                    tile.Draw(spriteBatch);
                title.Draw(spriteBatch);
                newGameButton.Draw(spriteBatch);
                loadGameButton.Draw(spriteBatch);
                sandboxButton.Draw(spriteBatch);
                adventureButton.Draw(spriteBatch);
            }
            #endregion

            #region Sandbox and Trade Menu
            if (gameState == GameState.Sandbox || gameState == GameState.Trade)
            {
                background.Draw(spriteBatch, new Rectangle(0, 0, WindowWidth, WindowHeight));
                
                #region Item Boxes
                foreach (StaticEntity equipItemBoxBackground in equippedItemBoxBackgrounds)
                    equipItemBoxBackground.Draw(spriteBatch, Color.DimGray);

                foreach (ItemBox itemBox in equippedItemBoxes)
                {
                    if (itemBox.Sprite != null)
                    {
                        if (itemBox.IsHovered)
                            itemBox.Draw(spriteBatch, Color.LightSteelBlue);
                        else
                            itemBox.Draw(spriteBatch);
                    }
                }
                #endregion

                #region Tooltip text
                if (hoveredItemName != "")
                {
                    spriteBatch.DrawString(font, hoveredItemName, new Vector2(TileSize, TileSize * 7), Color.Black);

                    spriteBatch.DrawString(font, hoveredItemType, new Vector2(TileSize, TileSize * 7 + font.LineSpacing), Color.Black);

                    if (hoveredItemDamage != "")
                        spriteBatch.DrawString(font, "Damage: " + hoveredItemDamage, new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 2), Color.Black);

                    spriteBatch.DrawString(font, "Effects: " + hoveredItemEffects, new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 3), Color.Black);
                    if (hoveredItemCooldown != "")
                        spriteBatch.DrawString(font, "Cooldown: " + hoveredItemCooldown + " second(s)", new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 4), Color.Black);
                    spriteBatch.DrawString(font, hoveredItemTooltip, new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 5), Color.Black);
                }
                #endregion

                if (playButton.CollisionRectangle.Contains(mouse.Position))
                    playButton.Draw(spriteBatch, Color.LightSteelBlue);
                else
                    playButton.Draw(spriteBatch);
            }
            #endregion

            #region Sandbox
            if (gameState == GameState.Sandbox)
            {
                #region General text

                spriteBatch.DrawString(font, "Left click on a weapon to equip it in the first slot.", new Vector2(TileSize / 2, 0), Color.Black);
                spriteBatch.DrawString(font, "Right click on a weapon to equip in in the second slot.", new Vector2(TileSize / 2, font.LineSpacing), Color.Black);
                spriteBatch.DrawString(font, "Click on a piece of equipment to put it in the third slot.", new Vector2(TileSize / 2, TileSize * 2 + font.LineSpacing), Color.Black);

                spriteBatch.DrawString(font, "Controls:", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7), Color.Black);
                spriteBatch.DrawString(font, "Left click to use the weapon in Slot One.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing), Color.Black);
                spriteBatch.DrawString(font, "Right click to use the weapon in Slot Two.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing * 2), Color.Black);
                spriteBatch.DrawString(font, "Spacebar to use equipment.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing * 3), Color.Black);
                spriteBatch.DrawString(font, "Press Escape to pause and unpause the game.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing * 4), Color.Black);
                #endregion

                #region Item boxes
                foreach (StaticEntity itemBoxBackground in itemBoxBackgrounds)
                    itemBoxBackground.Draw(spriteBatch, Color.DimGray);

                foreach (ItemBox itemBox in itemBoxes)
                {
                    if (itemBox.Sprite != null)
                    {
                        if (itemBox.IsHovered)
                            itemBox.Draw(spriteBatch, Color.LightSteelBlue);
                        else
                            itemBox.Draw(spriteBatch);
                    }
                }

                #endregion
                
            }
            #endregion

            #region Trade Menu
            if (gameState == GameState.Trade)
            {
                #region Text
                spriteBatch.DrawString(font, "The arena allows you to choose one item to take from your defeated foes.", new Vector2(TileSize / 2, TileSize / 2), Color.Black);
                spriteBatch.DrawString(font, "If you are injured, you may choose to take a healing potion instead.", new Vector2(TileSize / 2, TileSize / 2 + font.LineSpacing), Color.Black);
                spriteBatch.DrawString(font, "Left click on a piece of equipment to select it. Right click if you'd like to equip a weapon in slot 2.", new Vector2(TileSize / 2, TileSize / 2 + font.LineSpacing * 3), Color.Black);
                Color healTextColor = Color.Black;
                if (oldCharm1.Type == CharmType.Lifesteal)
                    healTextColor = Color.Crimson;
                spriteBatch.DrawString(font, "Heal", new Vector2(TileSize * 7 + (TileSize - font.MeasureString("Heal").X) / 2, TileSize * 3 - (TileSize / 2 + font.LineSpacing)), healTextColor);
                spriteBatch.DrawString(font, "Undo", new Vector2(TileSize * 9 + (TileSize - font.MeasureString("Undo").X) / 2, TileSize * 3 - (TileSize / 2 + font.LineSpacing)), Color.Black);
                spriteBatch.DrawString(font, "Level: " + level.ToString(), new Vector2(WindowWidth - font.MeasureString("Level:     ").X, 0), Color.Black);

                #endregion

                #region Item boxes
                foreach (StaticEntity itemBoxBackground in tradeItemBoxBackgrounds)
                    itemBoxBackground.Draw(spriteBatch, Color.DimGray);

                foreach (ItemBox itemBox in tradeItemBoxes)
                {
                    if (itemBox.Sprite != null && itemBox.IsActive)
                    {
                        if (itemBox.IsHovered)
                            itemBox.Draw(spriteBatch, Color.LightSteelBlue);
                        else
                            itemBox.Draw(spriteBatch);
                    }
                }

                #endregion

                #region Buttons

                if (resetButton.CollisionRectangle.Contains(mouse.Position))
                    resetButton.Draw(spriteBatch,Color.LightSteelBlue);
                else
                    resetButton.Draw(spriteBatch);

                if (oldCharm1.Type == CharmType.Lifesteal)
                    healingButton.Draw(spriteBatch, Color.DarkGray);
                else if (healingButton.CollisionRectangle.Contains(mouse.Position))
                    healingButton.Draw(spriteBatch, Color.LightSteelBlue);
                else
                    healingButton.Draw(spriteBatch);
                if ((level - 1) % 10 == 0 || saveGameOverrideEnabled)
                {
                    if (saveButton.CollisionRectangle.Contains(mouse.Position))
                        saveButton.Draw(spriteBatch, Color.LightSteelBlue);
                    else
                        saveButton.Draw(spriteBatch);
                }
                playerTradeMenuHealth.Draw(spriteBatch, player1, new Vector2(0,0));
                #endregion

            }
            #endregion

            #region Battle
            if (gameState == GameState.Battle || gameState == GameState.Paused || gameState == GameState.PlayerPaused 
                || gameState == GameState.Editor)
            {
                if (gameMode == GameMode.Adventure && looseCamera && gameState != GameState.Editor)
                    offset = -player1.Location + new Vector2(WindowWidth / 2, WindowHeight / 2);

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

                if (gameState != GameState.Editor)
                {
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
                    spriteBatch.DrawString(font, "Level: " + level.ToString(), new Vector2(WindowWidth - font.MeasureString("Level:     ").X, 0), Color.Black);
                    #endregion
                }
            }
            #endregion

            #region Pausing
            if (gameState == GameState.Paused)
            {
                Vector2 stringSize = font.MeasureString("Get Ready!");
                spriteBatch.DrawString(font, "Get Ready!", new Vector2(WindowWidth / 2 - stringSize.X / 2, 0), Color.Black);
            }
            else if (gameState == GameState.PlayerPaused)
            {
                Vector2 stringSize = font.MeasureString("Game is Paused");
                spriteBatch.DrawString(font, "Game is Paused", new Vector2(WindowWidth / 2 - stringSize.X / 2, 0), Color.Black);
            }
            #endregion

            #region Save and Load
            if (gameState == GameState.SaveGame || gameState == GameState.LoadGame)
            {
                background.Draw(spriteBatch, new Rectangle(0, 0, WindowWidth, WindowHeight));

                if (fileSelected != -1)
                {
                    if (confirmButton.CollisionRectangle.Contains(mouse.Position))
                        confirmButton.Draw(spriteBatch, Color.LightBlue);
                    else
                        confirmButton.Draw(spriteBatch);
                }

                foreach (StaticEntity button in fileButtons)
                {
                    if (fileButtons.IndexOf(button) == fileSelected)
                        button.Draw(spriteBatch, Color.SteelBlue);
                    else if (button.CollisionRectangle.Contains(mouse.Position))
                        button.Draw(spriteBatch, Color.LightBlue);
                    else
                        button.Draw(spriteBatch);
                }

                for (int i = 0; i < levelData.Count; i++)
                    spriteBatch.DrawString(font, levelData.ElementAt(i), new Vector2(TileSize * 3.1f, TileSize * 2.7f + TileSize * 2 * i), Color.Black);
                foreach (ItemBox box in weapon1ItemBoxes)
                    box.Draw(spriteBatch);
                foreach (ItemBox box in weapon2ItemBoxes)
                    box.Draw(spriteBatch);
                foreach (ItemBox box in shield1ItemBoxes)
                    box.Draw(spriteBatch);
                foreach (ItemBox box in charm1ItemBoxes)
                    box.Draw(spriteBatch);
            }
            if (gameState == GameState.SaveGame)
                spriteBatch.DrawString(font20, "Save Game", new Vector2(TileSize, TileSize), Color.Black);
            else if (gameState == GameState.LoadGame)
                spriteBatch.DrawString(font20, "Load Game", new Vector2(TileSize, TileSize), Color.Black);

            #endregion

            #region Save, Load, Sandbox
            if (gameState == GameState.SaveGame || gameState == GameState.LoadGame || gameState == GameState.Sandbox)
            {
                if (backButton.CollisionRectangle.Contains(mouse.Position))
                    backButton.Draw(spriteBatch, Color.LightBlue);
                else
                    backButton.Draw(spriteBatch);
            }
            #endregion

            #region Editor
            if (gameState == GameState.Editor)
            {
                Vector2 mouseGridPosition = ConvertToGrid(trueMousePosition);

                #region Info
                editorMenuInfoBackgrounds.ElementAt(0).Draw(spriteBatch, new Rectangle(0, 0, 35, font.LineSpacing * 3));

                spriteBatch.DrawString(font, "X " + mouseGridPosition.X, new Vector2(0, 0), Color.Black);
                spriteBatch.DrawString(font, "Y " + mouseGridPosition.Y, new Vector2(0, font.LineSpacing), Color.Black);
                spriteBatch.DrawString(font, "Z " + editorZSelected, new Vector2(0, 2 * font.LineSpacing), Color.Black);

                if (editorHoveredID != "")
                {
                    editorMenuInfoBackgrounds.ElementAt(1).Draw(spriteBatch, new Rectangle(0, WindowHeight / 2, 100, 100));
                    spriteBatch.DrawString(font, "Hovered:", new Vector2(0, WindowHeight / 2), Color.Black);
                    spriteBatch.DrawString(font, "ID: " + editorHoveredID, new Vector2(0, WindowHeight / 2 + font.LineSpacing), Color.Black);
                    spriteBatch.DrawString(font, "Z: " + editorHoveredZ, new Vector2(0, WindowHeight / 2 + font.LineSpacing * 2), Color.Black);
                }
                #endregion

                selectedObject.Draw(spriteBatch);
                
                if (selectingObjects)
                {
                    if (selectedObject.ID > SpawnerIDCutoff)
                    {
                        foreach (StaticEntity entity in valueEditors)
                            entity.Draw(spriteBatch);
                        spriteBatch.DrawString(font, "Enemy ID: " + idValue, valueEditors.ElementAt(0).CollisionRectangle.Location.ToVector2(), Color.Black);
                        spriteBatch.DrawString(font, "Distance: " + distanceValue, valueEditors.ElementAt(1).CollisionRectangle.Location.ToVector2(), Color.Black);
                        spriteBatch.DrawString(font, "Quantity: " + quantityValue, valueEditors.ElementAt(2).CollisionRectangle.Location.ToVector2(), Color.Black);
                        spriteBatch.DrawString(font, "Delay:    " + delayValue, valueEditors.ElementAt(3).CollisionRectangle.Location.ToVector2(), Color.Black);
                    }

                    editorSaveButton.Draw(spriteBatch);

                    foreach (GridEntity obj in objectSelection)
                    {
                        obj.Draw(spriteBatch);
                    }
                    basicSpawner.Draw(spriteBatch);
                }
                foreach (Spawner spawner in spawners)
                    spawner.Draw(spriteBatch, offset);
            }
            #endregion

            #region Game Over
            if (gameState == GameState.GameOver)
            {
                spriteBatch.DrawString(font20, "Game Over!", new Vector2(WindowWidth / 2 - gameOverButton.CollisionRectangle.Width / 2, 300), Color.White);
                gameOverButton.Draw(spriteBatch);
            }
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void CreateTiles()
        {
            for (int i = 0; i < TilesWide; i++)
            {
                for (int j = 0; j < TilesHigh; j++)
                {
                    Texture2D currentSprite = PickRandomTexture(tileSprites);

                    Vector2 tileLocation = new Vector2(i * TileSize + TileSize / 2, j * TileSize + TileSize / 2);
                    GridEntity newTile = new GridEntity("Tile", i, j, 0, tileLocation, TileSize, currentSprite);
                    tiles.Add(newTile);
                }
            }
        }

        public void DeleteGridEntityAtLocation(List<GridEntity> list, Vector2 location)
        {
            List<GridEntity> objToBeDeleted = new List<GridEntity>();
            foreach (GridEntity entity in list)
            {
                if (entity.CollisionRectangle.Contains(location))
                    objToBeDeleted.Add(entity);
            }
            foreach (GridEntity entity in objToBeDeleted)
                obstacles.Remove(entity);
        }

        public void DeleteSpawnerAtLocation(List<Spawner> list, Vector2 location)
        {
            List<Spawner> objToBeDeleted = new List<Spawner>();
            foreach (Spawner entity in list)
            {
                if (entity.CollisionRectangle.Contains(location))
                    objToBeDeleted.Add(entity);
            }
            foreach (Spawner entity in objToBeDeleted)
                spawners.Remove(entity);
        }

        public void DynamicToStaticCollision(DynamicEntity dynamicEntity, StaticEntity staticEntity)
        {
            if (dynamicEntity.CollisionRectangle.Intersects(staticEntity.CollisionRectangle) && dynamicEntity.Noclip == false)
            {
                //Collision from top
                if (dynamicEntity.PreviousCollisionRectangle.Bottom <= staticEntity.CollisionRectangle.Top)
                {
                    dynamicEntity.YLocation = staticEntity.CollisionRectangle.Top - (float)dynamicEntity.CollisionRectangle.Height / 2;
                }
                //Collision from bottom
                else if (dynamicEntity.PreviousCollisionRectangle.Top >= staticEntity.CollisionRectangle.Bottom)
                {
                    dynamicEntity.YLocation = staticEntity.CollisionRectangle.Bottom + (float)dynamicEntity.CollisionRectangle.Height / 2;
                }
                //Collision from left
                if (dynamicEntity.PreviousCollisionRectangle.Right <= staticEntity.CollisionRectangle.Left)
                {
                    dynamicEntity.XLocation = staticEntity.CollisionRectangle.Left - (float)dynamicEntity.CollisionRectangle.Width / 2;
                }
                //Collision from right
                else if (dynamicEntity.PreviousCollisionRectangle.Left >= staticEntity.CollisionRectangle.Right)
                {
                    dynamicEntity.XLocation = staticEntity.CollisionRectangle.Right + (float)dynamicEntity.CollisionRectangle.Width / 2;
                }
                dynamicEntity.UpdateRectangle();
            }
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

        public void EdgeCollision(DynamicEntity dynamicEntity)
        {
            int width = TilesWide * TileSize;
            int height = TilesHigh * TileSize;
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

        public bool CheckIfOffScreen(StaticEntity staticEntity)
        {
            bool isOffScreen = false;
            if (staticEntity.CollisionRectangle.Right < 0 || staticEntity.CollisionRectangle.Left > TilesWide * TileSize)
                isOffScreen = true;
            if (staticEntity.CollisionRectangle.Bottom < 0 || staticEntity.CollisionRectangle.Top > TilesHigh * TileSize)
                isOffScreen = true;
            return isOffScreen;
        }

        public void UpdateProjectiles(GameTime gameTime, List<Projectile> projectiles, List<GridEntity> obstacles)
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

        public Vector2 GetEnemyLocation()
        {
            /// Generate a random position on the grid for the enemy. Repeat the process
            /// until a position that is not occupied by an obstacle is obtained.
            bool positionValid = true;
            int enemyX = 0;
            int enemyY = 0;
            Rectangle illegalArea = new Rectangle(WindowWidth / TileSize / 2 - SafeZoneSize / 2, WindowHeight / TileSize / 2 - SafeZoneSize / 2, SafeZoneSize, SafeZoneSize);
            do
            {
                positionValid = true;
                enemyX = random.Next(0, WindowWidth / TileSize);
                enemyY = random.Next(0, WindowHeight / TileSize);
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
            Vector2 enemyLocation = new Vector2(enemyX * TileSize + TileSize / 2, enemyY * TileSize + TileSize / 2);
            return enemyLocation;
        }

        public Texture2D PickRandomTexture(List<Texture2D> textures)
        {
            Texture2D chosenTexture = textures.ElementAt(0);
            int index = random.Next(textures.Count);
            return textures.ElementAt(index);
        }

        public void ItemBoxHovered(ItemBox itemBox, MouseState mouse)
        {
            if (itemBox.IsActive == false)
                return;
            //Set the equipped item to the item in the box if the player is clicking on it
            if (itemBox.Type == ItemType.Shield && itemBox.Shield != null)
            {
                Shield shield = itemBox.Shield;
                hoveredItemName = shield.Name;
                hoveredItemType = "Equipment";
                hoveredItemTooltip = shield.Tooltip;
                if (shield.ActiveEffects.Count > 0)
                {
                    foreach (Effect effect in shield.ActiveEffects)
                    {
                        hoveredItemEffects += effect.Type.ToString() + " ";
                        if (effect.Strength > 0 && effect.Type != EffectType.Spin)
                            hoveredItemEffects += effect.Strength.ToString() + " ";
                        hoveredItemEffects += "for " + (effect.Duration / 1000f).ToString() + " sec, ";
                    }
                }
                else
                    hoveredItemEffects = "None";
                hoveredItemCooldown = ((float)shield.MaximumCooldown / 1000f).ToString();


                if (mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed)
                {
                    shield1 = shield.Copy();
                    if (oldShield1 != null)
                    {
                        weapon1 = oldWeapon1;
                        weapon2 = oldWeapon2;
                        charm1 = oldCharm1;
                        playerIsHealing = false;
                    }
                }
            }
            else if (itemBox.Type == ItemType.Weapon && itemBox.Weapon != null)
            {
                Weapon weapon = itemBox.Weapon;
                hoveredItemName = weapon.Name;
                hoveredItemType = weapon.Type.ToString() + " weapon";
                hoveredItemTooltip = weapon.Tooltip;
                if (weapon.Type == WeaponType.Ranged)
                    hoveredItemDamage = weapon.ProjectileDamage.ToString();
                else if (weapon.Type == WeaponType.Melee)
                    hoveredItemDamage = weapon.Damage.ToString();
                if (weapon.OnHitEffects.Count > 0)
                {
                    foreach (Effect effect in weapon.OnHitEffects)
                        hoveredItemEffects += effect.Type.ToString() + " " + effect.Strength.ToString() + " for " + (effect.Duration / 1000f).ToString() + " sec, ";
                }
                else
                    hoveredItemEffects = "None";
                hoveredItemCooldown = ((float)weapon.Cooldown / 1000f).ToString();

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    weapon1 = itemBox.Weapon.Copy();
                    if (oldWeapon1 != null)
                    {
                        weapon2 = oldWeapon2;
                        shield1 = oldShield1;
                        charm1 = oldCharm1;
                        playerIsHealing = false;
                    }
                }
                else if (mouse.RightButton == ButtonState.Pressed)
                {
                    weapon2 = itemBox.Weapon.Copy();
                    if (oldWeapon2 != null)
                    {
                        weapon1 = oldWeapon1;
                        shield1 = oldShield1;
                        charm1 = oldCharm1;
                        playerIsHealing = false;
                    }
                }
            }
            else if (itemBox.Type == ItemType.Charm && itemBox.Charm != null)
            {
                Charm charm = itemBox.Charm;
                hoveredItemName = charm.Name;
                hoveredItemType = "Charm";
                hoveredItemDamage = "";
                hoveredItemTooltip = charm.Tooltip;
                hoveredItemEffects = charm.Type.ToString();
                hoveredItemCooldown = "";

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (gameState == GameState.Trade)
                    {
                        weapon1 = oldWeapon1;
                        weapon2 = oldWeapon2;
                        shield1 = oldShield1;
                        playerIsHealing = false;
                    }
                    charm1 = itemBox.Charm.Copy();
                }
            }
        }

        public void DoItemBoxUpdate(MouseState mouse, List<ItemBox> itemBoxes, List<StaticEntity> itemBoxBackgrounds, List<ItemBox> equippedItemBoxes)
        {
            foreach (StaticEntity itemBoxBackground in itemBoxBackgrounds)
            {
                if (itemBoxBackgrounds.Count == itemBoxes.Count || itemBoxBackgrounds.IndexOf(itemBoxBackground) < itemBoxes.Count)
                { 
                    ItemBox correspondingBox = itemBoxes.ElementAt(itemBoxBackgrounds.IndexOf(itemBoxBackground));
                    if (itemBoxBackground.CollisionRectangle.Contains(mouse.Position.X, mouse.Position.Y))
                        correspondingBox.IsHovered = true;
                    else
                        correspondingBox.IsHovered = false;
                }
            }
            foreach (StaticEntity itemBoxBackground in equippedItemBoxBackgrounds)
            {
                ItemBox correspondingBox = equippedItemBoxes.ElementAt(equippedItemBoxBackgrounds.IndexOf(itemBoxBackground));
                if (itemBoxBackground.CollisionRectangle.Contains(mouse.Position.X, mouse.Position.Y))
                    correspondingBox.IsHovered = true;
                else
                    correspondingBox.IsHovered = false;
            }
            hoveredItemName = "";
            hoveredItemType = "";
            hoveredItemEffects = "";
            hoveredItemDamage = "";
            hoveredItemCooldown = "";
            foreach (ItemBox itemBox in itemBoxes)
            {
                if (itemBox.IsHovered)
                {
                    ItemBoxHovered(itemBox, mouse);
                }
            }
            foreach (ItemBox itemBox in equippedItemBoxes)
            {
                if (itemBox.IsHovered)
                    ItemBoxHovered(itemBox, mouse);
            }
            equippedItemBoxes.ElementAt(0).ReplaceItem(weapon1);
            equippedItemBoxes.ElementAt(1).ReplaceItem(weapon2);
            equippedItemBoxes.ElementAt(2).ReplaceItem(shield1);
            equippedItemBoxes.ElementAt(3).ReplaceItem(charm1);
            foreach (ItemBox box in equippedItemBoxes)
                box.Update();
        }

        public void AddRandomItem(ItemBox itemBox, WeaponFactory weaponFactory, ShieldFactory shieldFactory)
        {
            int selector = random.Next(1, 13);
            if (selector < 7) //Select weapon
            {
                selector = random.Next(0, 21);
                itemBox.ReplaceItem(GetWeaponFromID(selector));
            }
            else if (selector < 12) // Select shield
            {
                selector = random.Next(0, 5);
                itemBox.ReplaceItem(GetShieldFromID(selector));
            }
            else
            {
                selector = random.Next(0, 5);
                itemBox.ReplaceItem(GetCharmFromID(selector));
            }
        }

        public void AddItemFromEnemy(List<ItemBox> itemBoxes, List<Enemy> enemies)
        {
            int totalItems = 0;
            int idChosen = 0;
            List<Weapon> weapons = new List<Weapon>();
            List<Shield> shields = new List<Shield>();
            List<Charm> charms = new List<Charm>();

            foreach (Enemy enemy in enemies)
            {
                if (enemy.Weapon1.Type != WeaponType.Blank)
                    weapons.Add(GetWeaponFromID(enemy.Weapon1.ID));
                if (enemy.Weapon2.Type != WeaponType.Blank)
                    weapons.Add(GetWeaponFromID(enemy.Weapon2.ID));
                if (enemy.Shield1.Type != ShieldType.Blank)
                    shields.Add(GetShieldFromID(enemy.Shield1.ID));
                if (enemy.Charm1.Type != CharmType.Blank)
                    charms.Add(GetCharmFromID(enemy.Charm1.ID));
            }

            totalItems = weapons.Count + shields.Count + charms.Count;
            List<int> chosenItems = new List<int>();
            foreach (ItemBox itemBox in itemBoxes)
            {
                itemBox.IsActive = true;
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
                    itemBox.IsActive = false;
                else if (idChosen < weapons.Count)
                    itemBox.ReplaceItem(weapons.ElementAt(idChosen));
                else if (idChosen < weapons.Count + shields.Count)
                    itemBox.ReplaceItem(shields.ElementAt(idChosen - weapons.Count));
                else
                    itemBox.ReplaceItem(charms.ElementAt(idChosen - (weapons.Count + shields.Count)));
            }
        }

        public void CreateEnemies(EnemyFactory enemyFactory, List<Enemy> enemies)
        {
            List<Enemy> choices = new List<Enemy>();
            Vector2 zero = new Vector2(0, 0);
            switch (level)
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

        public void PickRandomEnemies(List<Enemy> mainEnemyList, List<Enemy> choices, int numberOfEnemies)
        {            
            for (int i = 0; i < numberOfEnemies; i++)
            {
                int enemyID = random.Next(0, choices.Count);
                Enemy enemy = choices.ElementAt(enemyID).Copy();
                enemy.Location = GetEnemyLocation();
                mainEnemyList.Add(enemy);
            }
        }

        public void CreateEnemiesOld(EnemyFactory enemyFactory, List<Enemy> enemies)
        {
            //Determine the current difficulty level
            int difficultyLevel = 1 + (int)Math.Ceiling(((decimal)level / 4));
            int numberOfEnemies = 1;
            if (level < 3)
                numberOfEnemies = 1;
            else if (level >= 3 && level < 8)
                numberOfEnemies = 2;
            else if (level >= 8 && level < 15)
                numberOfEnemies = 3;
            else if (level >= 15 && level < 23)
                numberOfEnemies = 4;
            else
                numberOfEnemies = 5;

            //Generate a location
            Vector2 location;

            if (level == 10)
            {
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                return;
            }
            if (level == 20)
            {
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                return;
            }
            if (level == 30)
            {
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                location = GetEnemyLocation();
                enemies.Add(enemyFactory.CreateGiant(location));
                return;
            }

            for (int i= 1; i <= numberOfEnemies; i++)
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

        public void InitiateCombat(GameTime gameTime)
        {
            //Initialize a combat
            player1 = new Player("Player1", playerStartingLocation, player1Sprite, new Vector2(0, 0), PlayerHitPoints, healthBarSprite, weapon1.Copy(), weapon2.Copy() , shield1.Copy(), charm1.Copy(), playerBaseSpeed);

            weapon1Icon.Sprite = weapon1.Sprite;
            weapon1Icon.UpdateDrawRectangle();
            weapon1.IsActive = false;

            weapon2Icon.Sprite = weapon2.Sprite;
            weapon2Icon.UpdateDrawRectangle();
            weapon2.IsActive = false;

            shield1Icon.Sprite = shield1.Sprite;
            shield1Icon.UpdateDrawRectangle();
            shield1.IsActive = false;

            charm1Icon.Sprite = charm1.Sprite;
            charm1Icon.UpdateDrawRectangle();
            player1.ApplyCharmEffects();

            player1.HitPoints = Health;

            if (gameMode == GameMode.Arena)
            {
                #region Create obstacles
                obstacles.Clear();
                for (int i = 1; i <= NumberOfRocks; i++)
                {
                    //Choose a random x and y location that is inside the screen
                    bool locationIsValid = false;
                    int x = 0;
                    int y = 0;
                    while (!locationIsValid)
                    {
                        x = random.Next(0, WindowWidth / TileSize);
                        y = random.Next(0, WindowHeight / TileSize);
                        locationIsValid = true;
                        foreach (GridEntity obstacle in obstacles)
                        {
                            if (obstacle.XPosition == x && obstacle.YPosition == y)
                                locationIsValid = false;
                        }
                        if (x == (WindowWidth / TileSize) / 2 ||
                        x == (WindowWidth / TileSize) / 2 - 1 ||
                        y == (WindowHeight / TileSize) / 2 ||
                        y == (WindowHeight / TileSize) / 2 - 1)
                            locationIsValid = false;
                    }

                    Vector2 obsLocation = new Vector2(x * TileSize + TileSize / 2, y * TileSize + TileSize / 2);
                    //Create the obstacle
                    obstacles.Add(GetObstacleFromID(1, x, y, 1));
                }
                #endregion

                //Create Enemies
                CreateEnemies(enemyFactory, enemies);
            }
            else if (gameMode == GameMode.Adventure)
            {
                obstacles.Clear();
                spawners.Clear();
                OpenLevel("Level1");
            }
            pathfinder.MapTilesHigh = TilesHigh;
            pathfinder.MapTilesWide = TilesWide;
            gameState = GameState.Paused;
            pauseDurationLeft = BetweenLevelPause;
            //Update once to properly adjust health bars
            player1.HealthBar.Update(gameTime, player1);
            foreach (Enemy enemy in enemies)
                enemy.Update(gameTime);
        }

        public Weapon GetWeaponFromID(int id)
        {
            switch (id)
            {
                case -1:
                    return weaponFactory.CreateMaverick();
                case 0:
                    return weaponFactory.CreateSword();
                case 1:
                    return weaponFactory.CreateBroadsword();
                case 2:
                    return weaponFactory.CreateBow();
                case 3:
                    return weaponFactory.CreateIceBow();
                case 4:
                    return weaponFactory.CreateThrowingAxe();
                case 5:
                    return weaponFactory.CreateDwarvenAxe();
                case 6:
                    return weaponFactory.CreateMaul();
                case 7:
                    return weaponFactory.CreateHammer();
                case 8:
                    return weaponFactory.CreateSpear();
                case 9:
                    return weaponFactory.CreateThrowingSpear();
                case 10:
                    return weaponFactory.CreateJungleSpear();
                case 11:
                    return weaponFactory.CreateThrowingDagger();
                case 12:
                    return weaponFactory.CreateFireball();
                case 13:
                    return weaponFactory.CreateFirebolt();
                case 14:
                    return weaponFactory.CreateFirework();
                case 15:
                    return weaponFactory.CreateHelsingor();
                case 16:
                    return weaponFactory.CreateBoteng();
                case 17:
                    return weaponFactory.CreateHira();
                case 18:
                    return weaponFactory.CreateTaago();
                case 19:
                    return weaponFactory.CreatePlasmaBolt();
                case 20:
                    return weaponFactory.CreateGrapple();
                default:
                    return weaponFactory.CreateSword();
            }
        }

        public Shield GetShieldFromID(int id)
        {
            switch (id)
            {
                case 0:
                    return shieldFactory.CreateSpeedboost();
                case 1:
                    return shieldFactory.CreateBasicShield();
                case 2:
                    return shieldFactory.CreateTowerShield();
                case 3:
                    return shieldFactory.CreateThunderStone();
                case 4:
                    return shieldFactory.CreateElvenTrinket();
                default:
                    return shieldFactory.CreateBasicShield();

            }
        }

        public Charm GetCharmFromID(int id)
        {
            switch (id)
            {
                case 0:
                    return charmFactory.CreateEmptyCharm();
                case 1:
                    return charmFactory.CreateLowerCooldown();
                case 2:
                    return charmFactory.CreateHigherCooldown();
                case 3:
                    return charmFactory.CreateBurstCharm();
                case 4:
                    return charmFactory.CreateSpeedCharm();
                case 5:
                    return charmFactory.CreateLifestealCharm();
                default:
                    return charmFactory.CreateEmptyCharm();

            }
        }

        public GridEntity GetObstacleFromID(int id, int x, int y, int z)
        {
            GridEntity obstacle = new GridEntity();
            Vector2 location = new Vector2(x * TileSize + TileSize / 2, y * TileSize + TileSize / 2);
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

        public void SaveGame(int slot)
        {
            StreamWriter outputFile;
            outputFile = File.CreateText("SaveGame" + slot.ToString());

            outputFile.WriteLine(level);
            outputFile.WriteLine(player1.HitPoints);
            outputFile.WriteLine(player1.Weapon1.ID);
            outputFile.WriteLine(player1.Weapon2.ID);
            outputFile.WriteLine(player1.Shield1.ID);
            outputFile.WriteLine(player1.Charm1.ID);

            foreach (ItemBox box in tradeItemBoxes)
            {
                if (box.IsActive == false)
                {
                    outputFile.WriteLine(-1);
                    outputFile.WriteLine(-1);
                }
                else
                {
                    switch (box.Type)
                    {
                        case ItemType.Weapon:
                            outputFile.WriteLine(0);
                            outputFile.WriteLine(box.Weapon.ID);
                            break;
                        case ItemType.Shield:
                            outputFile.WriteLine(1);
                            outputFile.WriteLine(box.Shield.ID);
                            break;
                        case ItemType.Charm:
                            outputFile.WriteLine(2);
                            outputFile.WriteLine(box.Charm.ID);
                            break;
                    }
                }

            }

            outputFile.Close();
        }

        public bool LoadGame(int slot)
        {
            StreamReader inputFile;
            bool loadSuccessful = true;
            try
            {
                inputFile = File.OpenText("SaveGame" + slot.ToString());

                level = int.Parse(inputFile.ReadLine());
                Health = int.Parse(inputFile.ReadLine());
                oldHealth = Health;
                player1.HitPoints = Health;
                weapon1 = GetWeaponFromID(int.Parse(inputFile.ReadLine()));
                oldWeapon1 = weapon1;
                player1.Weapon1 = weapon1;
                weapon2 = GetWeaponFromID(int.Parse(inputFile.ReadLine()));
                oldWeapon2 = weapon2;
                player1.Weapon2 = weapon2;
                shield1 = GetShieldFromID(int.Parse(inputFile.ReadLine()));
                oldShield1 = shield1;
                player1.Shield1 = shield1;
                charm1 = GetCharmFromID(int.Parse(inputFile.ReadLine()));
                oldCharm1 = charm1;
                player1.Charm1 = charm1;
                for (int i = 0; i < 3; i++)
                {
                    switch (int.Parse(inputFile.ReadLine()))
                    {
                        case -1:
                            tradeItemBoxes.ElementAt(i).IsActive = false;
                            inputFile.ReadLine();
                            break;
                        case 0:
                            tradeItemBoxes.ElementAt(i).ReplaceItem(GetWeaponFromID(int.Parse(inputFile.ReadLine())));
                            break;
                        case 1:
                            tradeItemBoxes.ElementAt(i).ReplaceItem(GetShieldFromID(int.Parse(inputFile.ReadLine())));
                            break;
                        case 2:
                            tradeItemBoxes.ElementAt(i).ReplaceItem(GetCharmFromID(int.Parse(inputFile.ReadLine())));
                            break;
                    }
                }

                inputFile.Close();
            }
            catch
            {
                loadSuccessful = false;
            }

            return loadSuccessful;
        }

        public bool CheckGameData(int slot, List<String> levelData, List<ItemBox> weapon1Box, List<ItemBox> weapon2Box, List<ItemBox> shield1Box, List<ItemBox> charm1Box)
        {
            StreamReader inputFile;
            bool checkSuccessful = true;
            try
            {
                inputFile = File.OpenText("SaveGame" + slot.ToString());

                levelData.RemoveAt(slot);
                levelData.Insert(slot, "Level " + inputFile.ReadLine() + "    " + inputFile.ReadLine() + " hitpoints.");
                weapon1Box.ElementAt(slot).ReplaceItem(GetWeaponFromID(int.Parse(inputFile.ReadLine())));
                weapon2Box.ElementAt(slot).ReplaceItem(GetWeaponFromID(int.Parse(inputFile.ReadLine())));
                shield1Box.ElementAt(slot).ReplaceItem(GetShieldFromID(int.Parse(inputFile.ReadLine())));
                charm1Box.ElementAt(slot).ReplaceItem(GetCharmFromID(int.Parse(inputFile.ReadLine())));

                inputFile.Close();
            }
            catch
            {
                checkSuccessful = false;
            }
            return checkSuccessful;
        }

        public void InitiateLevelEditor()
        {
            OpenLevel("Level" + level);
        }

        public void OpenLevel(string levelName)
        {
            StreamReader inputFile;
            try
            {
                inputFile = File.OpenText(levelName);
                //Read the level parameters
                TilesHigh = int.Parse(inputFile.ReadLine());
                TilesWide = int.Parse(inputFile.ReadLine());
                BackgroundType = int.Parse(inputFile.ReadLine());
                //For the rest of the file, load tile entities
                while (!inputFile.EndOfStream)
                {
                    int infoRead = 0;
                    List<string> obstacleData = new List<string>();
                    for (int i = 0; i <= 6; i++)
                        obstacleData.Add("");
                    string nextLine = inputFile.ReadLine();
                    for (int i = 0; i < nextLine.Length; i++)
                    {
                        if (infoRead <= 6)
                        {
                            char c = nextLine.ElementAt(i);
                            if (Char.IsWhiteSpace(c))
                                infoRead += 1;
                            else
                            {
                                string s = obstacleData.ElementAt(infoRead);
                                s += c;
                                obstacleData.RemoveAt(infoRead);
                                obstacleData.Insert(infoRead, s);
                            }
                        }
                    }
                    int id = int.Parse(obstacleData.ElementAt(0));
                    int x = int.Parse(obstacleData.ElementAt(1));
                    int y = int.Parse(obstacleData.ElementAt(2));
                    int z = int.Parse(obstacleData.ElementAt(3));
                    if (id < SpawnerIDCutoff)
                        obstacles.Add(GetObstacleFromID(id, x, y, z));
                    else
                    {
                        int distance = int.Parse(obstacleData.ElementAt(4));
                        int quantity = int.Parse(obstacleData.ElementAt(5));
                        int delay = int.Parse(obstacleData.ElementAt(6));
                        spawners.Add(new Spawner("spawner", x, y, z, new Vector2(x * TileSize + TileSize / 2, y * TileSize + TileSize / 2), charmSprite, id - SpawnerIDCutoff, distance, quantity, delay));
                    }

                }
                inputFile.Close();
            }
            catch
            {
                
            }
            tiles.Clear();
            CreateTiles();
        }

        public void SaveLevel(string levelName)
        {
            StreamWriter outputFile;
            outputFile = File.CreateText(levelName);

            outputFile.WriteLine(TilesHigh);
            outputFile.WriteLine(TilesWide);
            outputFile.WriteLine(BackgroundType);
            foreach (GridEntity obstacle in obstacles)
            {
                string text = "";
                text += obstacle.ID;
                text += " ";
                text += obstacle.XPosition;
                text += " ";
                text += obstacle.YPosition;
                text += " ";
                text += obstacle.ZPosition;
                outputFile.WriteLine(text);
            }
            foreach (Spawner spawner in spawners)
            {
                string text = "";
                text += spawner.EnemyID + SpawnerIDCutoff;
                text += " ";
                text += spawner.XPosition;
                text += " ";
                text += spawner.YPosition;
                text += " ";
                text += spawner.ZPosition;
                text += " ";
                text += spawner.DistanceActivated;
                text += " ";
                text += spawner.Quantity;
                text += " ";
                text += spawner.Delay;
                outputFile.WriteLine(text);
            }
            outputFile.Close();
        }
        
        public Vector2 ConvertToGrid(Vector2 location)
        {
            Vector2 gridPosition = new Vector2(0, 0);
            gridPosition.X = (float)Math.Floor(location.X / TileSize);
            gridPosition.Y = (float)Math.Floor(location.Y / TileSize);

            return gridPosition;
        }
    }
}