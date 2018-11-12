using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MainGame.ContentLoaders;
using MainGame.ContentLoaders.Textures;

namespace MainGame.Screens
{
    class GameOverScreen : Screen
    {
        StaticEntity gameOverButton;
        Texture2D playButtonSprite;
        SpriteFont font20;

        public GameOverScreen(OnScreenChanged screenChanged) : base(screenChanged)
        {
            FontLoader fontLoader = FontLoader.GetInstance();
            UserInterfaceLoader uiLoader = UserInterfaceLoader.GetInstance();

            font20 = fontLoader.Get("font");
            playButtonSprite = uiLoader.Get("continue");
            gameOverButton = new StaticEntity("BackToMainMenu", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2), playButtonSprite);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font20, "Game Over!", new Vector2(GameConstants.WINDOW_WIDTH / 2 - gameOverButton.CollisionRectangle.Width / 2, 300), Color.White);
            gameOverButton.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            if (gameOverButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                ScreenChanged(new MainMenuScreen(ScreenChanged));
            }
        }
    }
}
