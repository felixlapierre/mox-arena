using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders.Textures
{
    class TileLoader : TextureLoader
    {
        private static TileLoader Instance;

        private TileLoader(ContentManager content) : base(content, "/tiles")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if (Instance == null)
                Instance = new TileLoader(content);
        }

        public static TileLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
