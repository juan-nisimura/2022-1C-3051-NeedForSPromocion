
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace TGC.Monogame.TP.Src.Screens 
{
    public abstract class Screen
    {
        protected const String ContentFolderMusic = "Music/";
        protected const String ContentFolderSpriteFonts = "SpriteFonts/";
        protected Song Song { get; set; }
        protected SpriteFont Font { get; set; }
        protected abstract String SongName();
        protected abstract String FontName();
        public abstract void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
        public abstract void Update(GraphicsDevice graphicsDevice);
        public abstract void Initialize(GraphicsDevice graphicsDevice);
        public abstract void Load(GraphicsDevice graphicsDevice, ContentManager content);

        public abstract void Start();

        public abstract void Stop();

        public abstract void Reset();
    }
}