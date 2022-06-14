using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src.MyContentManagers
{
    public class MyContentDictionary <T> 
    {       
        private string ContentFolder;
        private ContentManager Content;
        private Dictionary<string, T> Elements = new Dictionary<string, T>();

        public MyContentDictionary(ContentManager content, string contentFolder){
            this.Content = content;
            this.ContentFolder = contentFolder;
        }

        public T Get(string path) {
            return Elements[path];
        }

        public T Load(string path) {
            if(!Elements.ContainsKey(path))
                Elements.Add(path, Content.Load<T>(ContentFolder + path));
            return Get(path);
        }
    }
}