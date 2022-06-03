using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class PauseScreen : TextScreen
    {
        protected override String SongName() { return "Riders On The Storm Fredwreck Remix"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new PauseScreen();
        public static Screen GetInstance() { return Instance; }
        
        public override void Initialize(GraphicsDevice graphicsDevice)
        {
            
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (TGCGame.ControllerKeyP.Update().IsKeyToPressed()){
                TGCGame.SwitchActiveScreen(() => LevelScreen.GetInstance());
            }
        }

        public override void DrawText(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            DrawCenterTextY("Pause", 100, 3, spriteBatch, graphicsDevice);
            DrawCenterTextY("Presione P para seguir jugando", 200, 1, spriteBatch, graphicsDevice);
        }
    }
}