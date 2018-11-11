using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders.Textures
{
    class CharmLoader : TextureLoader
    {
        static CharmLoader Instance;

        private CharmLoader(ContentManager content) : base(content, "/charms")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if(Instance == null)
                Instance = new CharmLoader(content);
        }

        public CharmLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
