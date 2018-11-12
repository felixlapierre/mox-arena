using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.ContentLoaders
{
    class FontLoader : ContentLoader<SpriteFont>
    {
        private static FontLoader Instance;

        private FontLoader(ContentManager content) : base(content, "/fonts")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if(Instance == null)
                Instance = new FontLoader(content);
        }

        public static FontLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
