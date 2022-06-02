
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace TGC.Monogame.TP.Src.Screens 
{
    public abstract class Screen 
    {
        public static String ContentFolderMusic = "Music/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";

        protected Song Song { get; set; }

        public abstract void Reset();

        public abstract void Load(GraphicsDevice graphicsDevice, ContentManager content);

        public abstract void Update(GameTime gameTime, GraphicsDevice graphicsDevice);

        public abstract void Start();

        public abstract void Stop();

        public abstract void Initialize(GraphicsDevice graphicsDevice);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
    }
}