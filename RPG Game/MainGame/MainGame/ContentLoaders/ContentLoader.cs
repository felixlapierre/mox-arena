using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.ContentLoaders
{
    public class ContentLoader<T>
    {
        #region Properties
        private ContentManager Content { get; set; }
        #endregion

        #region Public Methods
        public ContentLoader(ContentManager content, string root)
        {
            Content = new ContentManager(content.ServiceProvider, "Content" + root);
        }

        public T Get(string name)
        {
            return Content.Load<T>(name);
        }

        public void Unload()
        {
            Content.Unload();
        }
        #endregion
    }
}
