using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using MainGame.ContentLoaders.Textures;

namespace MainGame.ContentLoaders
{
    public class ContentLoaderInitializer
    {
        public static void InitializeAllContentLoaders(ContentManager content)
        {
            FontLoader.Initialize(content);
            CharmLoader.Initialize(content);
            CreatureLoader.Initialize(content);
            ProjectileLoader.Initialize(content);
            ShieldLoader.Initialize(content);
            TileLoader.Initialize(content);
            UserInterfaceLoader.Initialize(content);
            WeaponLoader.Initialize(content);
        }
    }
}
