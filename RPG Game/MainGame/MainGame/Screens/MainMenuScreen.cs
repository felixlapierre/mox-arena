using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        #endregion

        #region Constructors
        public MainMenuScreen(OnScreenChanged screenChanged, ContentManager content) : base(screenChanged)
        {
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
            //if (newGameButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            //{
            //    gameMode = GameMode.Arena;
            //    weapon1 = weaponFactory.CreateSword();
            //    weapon2 = weaponFactory.CreateBow();
            //    shield1 = shieldFactory.CreateBasicShield();
            //    charm1 = charmFactory.CreateEmptyCharm();
            //    InitiateCombat(gameTime);
            //}
            //else if (loadGameButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            //{
            //    gameMode = GameMode.Arena;
            //    gameState = GameState.LoadGame;
            //    fileSelected = -1;
            //    for (int i = 0; i < NumberOfSaves; i++)
            //        CheckGameData(i, levelData, weapon1ItemBoxes, weapon2ItemBoxes, shield1ItemBoxes, charm1ItemBoxes);

            //}
            //else if (sandboxButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            //{
            //    gameState = GameState.Sandbox;
            //}
            //else if (adventureButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            //{
            //    gameMode = GameMode.Adventure;
            //    if (!keyboard.IsKeyDown(Keys.LeftShift))
            //    {
            //        gameState = GameState.Battle;
            //        InitiateCombat(gameTime);
            //    }
            //    else
            //    {
            //        gameState = GameState.Editor;
            //        leftMousePreviouslyPressed = true;
            //        InitiateLevelEditor();
            //    }

            //}
        }
        #endregion
    }
}
