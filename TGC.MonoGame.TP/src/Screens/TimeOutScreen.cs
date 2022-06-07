using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class TimeOutScreen : TextScreen
    {
        protected override String SongName() { return "Riders On The Storm Fredwreck Remix"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new TimeOutScreen();
        public static Screen GetInstance() { return Instance; }
        
        public override void Initialize()
        {
            
        }

        public override void DrawText()
        {
            DrawCenterTextY("Time Out", 100, 3);
            DrawCenterTextY("Presione ENTER para volver a jugar", 200, 1);
        }
    }
}