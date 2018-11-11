using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainGame.Items;
using MainGame.Screens.Trade_Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame.Screens
{
    public class MainMenuScreen : Screen
    {
        #region Properties
        List<GridEntity> menuBackground = new List<GridEntity>();

        Texture2D titleSprite;
        Texture2D newGameSprite;
        Texture2D loadGameSprite;
        Texture2D sandboxSprite;
        Texture2D adventureSprite;

        Texture2D tileBrick1Sprite;

        StaticEntity title;
        StaticEntity newGameButton;
        StaticEntity loadGameButton;
        StaticEntity sandboxButton;
        StaticEntity adventureButton;

        ContentManager Content { get; set; }

        //TODO: Get rid of this when buttons have been refactored
        bool leftMousePreviouslyPressed = false;
        #endregion

        #region Constructors
        public MainMenuScreen(OnScreenChanged screenChanged, ContentManager content) : base(screenChanged)
        {
            Content = content;

            tileBrick1Sprite = content.Load<Texture2D>(@"graphics\TileBrick1");
            titleSprite = content.Load<Texture2D>(@"graphics/TitleSprite");
            newGameSprite = content.Load<Texture2D>(@"graphics/NewGameButton");
            loadGameSprite = content.Load<Texture2D>(@"graphics/LoadGameButton");
            sandboxSprite = content.Load<Texture2D>(@"graphics/SandboxButton");
            adventureSprite = content.Load<Texture2D>(@"graphics/AchievementsButton");

            title = new StaticEntity("Title Card", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.TILE_SIZE * 2), titleSprite);
            newGameButton = new StaticEntity("New Game Button", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.TILE_SIZE * 11 / 2), newGameSprite);
            loadGameButton = new StaticEntity("Load Game Button", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.TILE_SIZE * 8), loadGameSprite);
            sandboxButton = new StaticEntity("Sandbox Button", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.TILE_SIZE * 21 / 2), sandboxSprite);
            adventureButton = new StaticEntity("Achievements Button", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.TILE_SIZE * 13), adventureSprite);

            for (int i = 0; i < GameConstants.TILES_WIDE; i++)
            {
                for (int j = 0; j < GameConstants.TILES_HIGH + 1; j++) //The +1 allows us to cover the action bar
                {
                    Vector2 tileLocation = new Vector2(i * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2, j * GameConstants.TILE_SIZE + GameConstants.TILE_SIZE / 2);
                    GridEntity newTile = new GridEntity("Tile", i, j, 0, tileLocation, GameConstants.TILE_SIZE, tileBrick1Sprite);
                    menuBackground.Add(newTile);
                }
            }
        }
        #endregion

        #region Public Methods
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (GridEntity tile in menuBackground)
                tile.Draw(spriteBatch);
            title.Draw(spriteBatch);
            newGameButton.Draw(spriteBatch);
            loadGameButton.Draw(spriteBatch);
            sandboxButton.Draw(spriteBatch);
            adventureButton.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            if (newGameButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            {                
                TradeScreenContents playerStartingGear = new TradeScreenContents(GameConstants.PLAYER_MAX_HIT_POINTS, 1,
                    ItemFactoryContainer.Weapons.CreateSword(), ItemFactoryContainer.Weapons.CreateBow(),
                    ItemFactoryContainer.Shields.CreateBasicShield(), ItemFactoryContainer.Charms.CreateEmptyCharm());

                ScreenChanged(new CombatScreen(ScreenChanged, Content, playerStartingGear));
            }
            else if (loadGameButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            {
                ScreenChanged(new LoadGameScreen(ScreenChanged, Content));

            }
            else if (sandboxButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            {
                ScreenChanged(new SandboxScreen(ScreenChanged, Content));
            }
            else if (adventureButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            {
                //TODO: Support adventure mode and level editor
                //gameMode = GameMode.Adventure;
                //if (!keyboard.IsKeyDown(Keys.LeftShift))
                //{
                //    gameState = GameState.Battle;
                //    InitiateCombat(gameTime);
                //}
                //else
                //{
                //    gameState = GameState.Editor;
                //    leftMousePreviouslyPressed = true;
                //    InitiateLevelEditor();
                //}

            }

            leftMousePreviouslyPressed = mouse.LeftButton == ButtonState.Pressed;
        }
        #endregion
    }
}
