using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders.Textures
{
    class ProjectileLoader : TextureLoader
    {
        private static ProjectileLoader Instance;

        private ProjectileLoader(ContentManager content) : base(content, "/projectiles")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if(Instance == null)
                Instance = new ProjectileLoader(content);
        }

        public static ProjectileLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
