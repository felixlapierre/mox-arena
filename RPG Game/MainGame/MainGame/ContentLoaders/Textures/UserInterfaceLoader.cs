using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace MainGame.ContentLoaders.Textures
{
    class UserInterfaceLoader : TextureLoader
    {
        private static UserInterfaceLoader Instance;

        private UserInterfaceLoader(ContentManager content) : base(content, "/userinterface")
        {

        }

        public static void Initialize(ContentManager content)
        {
            if (Instance == null)
                Instance = new UserInterfaceLoader(content);
        }

        public static UserInterfaceLoader GetInstance()
        {
            if (Instance == null)
                throw new LoaderNotInitializedException();
            return Instance;
        }
    }
}
