using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class MainMenuScreen : TextScreen
    {
        protected override String SongName() { return "Main Menu"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new MainMenuScreen();
        public static Screen GetInstance() { return Instance; }

        public override void Initialize(GraphicsDevice graphicsDevice)
        {
        
        }

        public override void DrawText(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            DrawCenterTextY("Need For Spromocion", 100, 2.5f, spriteBatch, graphicsDevice);
            DrawCenterTextY("Presione ENTER para jugar", 200, 1, spriteBatch, graphicsDevice);
        }
    }
}