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
    class ContentLoader<T>
    {
        #region Properties
        private ContentManager Content { get; set; }
        private Dictionary<string, T> ContentMap { get; set; }
        private string Root { get; set; }
        #endregion

        #region Public Methods
        public ContentLoader(ContentManager content, string root)
        {
            Content = content;
            ContentMap = new Dictionary<string, T>();
            Root = root;
        }

        public T Get(string name)
        {
            if(ContentMap.ContainsKey(name))
            {
                return ContentMap[name];
            }
            else
            {
                T loadedContent = Content.Load<T>(Root + name);
                ContentMap[name] = loadedContent;
                return loadedContent;
            }
        }
        #endregion
    }
}
