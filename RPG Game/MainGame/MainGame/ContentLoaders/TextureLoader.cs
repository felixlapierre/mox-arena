using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders
{
    class TextureLoader : ContentLoader<Texture2D>
    {
        private static TextureLoader Instance;

        protected TextureLoader(ContentManager content, string root) : base(content, "/graphics" + root)
        {
            //Leave root blank to load textures in graphics folder
        }
        
        public static void Initialize(ContentManager content)
        {
            if(Instance == null)
                Instance = new TextureLoader(content, "");
        }

        public static TextureLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
