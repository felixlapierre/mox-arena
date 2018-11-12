using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using MainGame.ContentLoaders;
using MainGame.ContentLoaders.Textures;

namespace MainGame
{
    class Game2 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D axe1;
        public Game2()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            //ContentManager content2 = new ContentManager()

            graphics.PreferredBackBufferWidth = GameConstants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = GameConstants.WINDOW_HEIGHT + GameConstants.ACTION_BAR_HEIGHT;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ContentLoaders.ContentLoaderInitializer.InitializeAllContentLoaders(Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            axe1 = WeaponLoader.GetInstance().Get("axe1");

            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(axe1, new Rectangle(0, 0, 1000, 800), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
