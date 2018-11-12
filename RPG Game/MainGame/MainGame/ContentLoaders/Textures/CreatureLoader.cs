using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders.Textures
{
    class CreatureLoader : TextureLoader
    {
        private static CreatureLoader Instance;

        private CreatureLoader(ContentManager content) : base(content, "/creatures")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if (Instance == null)
                Instance = new CreatureLoader(content);
        }

        public static CreatureLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}

