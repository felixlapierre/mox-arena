using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders.Textures
{
    class ShieldLoader : TextureLoader
    {
        private static ShieldLoader Instance;

        private ShieldLoader(ContentManager content) : base(content, "/shields")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if(Instance == null)
                Instance = new ShieldLoader(content);
        }

        public static ShieldLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
