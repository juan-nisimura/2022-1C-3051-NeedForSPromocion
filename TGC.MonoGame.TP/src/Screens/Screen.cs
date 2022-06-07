
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.Screens 
{
    public abstract class Screen
    {
        protected FullScreenQuad FullScreenQuad;
        protected RenderTarget2D MenuRenderTarget;
        protected RenderTarget2D SceneRenderTarget;
        
        protected const String ContentFolderMusic = "Music/";
        protected const String ContentFolderSpriteFonts = "SpriteFonts/";
        protected Song Song { get; set; }
        protected SpriteFont Font { get; set; }
        protected abstract String SongName();
        protected abstract String FontName();
        public abstract void Draw();
        public abstract void Update();
        public abstract void Initialize();
        public abstract void Load(ContentManager content);

        public abstract void Start();

        public abstract void Stop();

        public abstract void Reset();
    }
}