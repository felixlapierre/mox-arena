using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders.Textures
{
    class WeaponLoader : TextureLoader
    {
        private static WeaponLoader Instance;

        private WeaponLoader(ContentManager content) : base(content, "/weapons")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if(Instance == null)
                Instance = new WeaponLoader(content);
        }

        public static WeaponLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
