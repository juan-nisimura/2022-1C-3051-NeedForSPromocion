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
        
        public override void Initialize()
        {
            
        }

        public override void Update()
        {
            if (TGCGame.ControllerKeyP.Update().IsKeyToPressed()){
                TGCGame.SwitchActiveScreen(() => LevelScreen.GetInstance());
            }
        }

        public override void DrawText()
        {
            DrawCenterTextY("Pause", 80, 3);
            DrawCenterTextY("P:     Pausar/Despausar", 180, 1);
            DrawCenterTextY("W:     Avanzar         ", 210, 1);
            DrawCenterTextY("A y D: Girar           ", 240, 1);
            DrawCenterTextY("S:     Retroceder      ", 270, 1);
            DrawCenterTextY("SPACE: Saltar          ", 300, 1);
            DrawCenterTextY("F:     Activar Poder   ", 330, 1);
            DrawCenterTextY("G:     Modo GOD        ", 360, 1);
        }
    }
}