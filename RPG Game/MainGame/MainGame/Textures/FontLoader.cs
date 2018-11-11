using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Textures
{
    class FontLoader : ContentLoader<SpriteFont>
    {
        public FontLoader(ContentManager content) : base(content, "fonts")
        {

        }
    }
}
