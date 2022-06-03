using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class WinScreen : TextScreen
    {   
        protected override String SongName() { return "Main Menu"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new WinScreen();
        public static Screen GetInstance() { return Instance; }
        
        public override void Initialize(GraphicsDevice graphicsDevice)
        {
            
        }

        public override void DrawText(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            DrawCenterTextY("You Win", 100, 3, spriteBatch, graphicsDevice);
            DrawCenterTextY("Presione ENTER para volver a jugar", 200, 1, spriteBatch, graphicsDevice);
        }
    }
}