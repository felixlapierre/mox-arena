using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MainGame.Textures
{
    class TextureLoader : ContentLoader<Texture2D>
    {
        public TextureLoader(ContentManager content, string root) : base(content, "graphics/" + root)
        {
            //Leave root blank to load textures in graphics folder
        }
    }
}
