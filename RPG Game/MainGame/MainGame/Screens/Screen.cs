using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Screens
{
    public delegate void OnScreenChanged(Screen nextScreen);

    public abstract class Screen
    {
        protected OnScreenChanged ScreenChanged { get; set; }

        public Screen(OnScreenChanged screenChanged)
        {
            ScreenChanged = screenChanged;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
