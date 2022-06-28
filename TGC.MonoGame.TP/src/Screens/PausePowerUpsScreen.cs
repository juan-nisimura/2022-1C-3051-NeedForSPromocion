using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class PausePowerUpsScreen : TextScreen
    {
        protected override String SongName() { return "Riders On The Storm Fredwreck Remix"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new PausePowerUpsScreen();
        public static Screen GetInstance() { return Instance; }
        
        public override void Initialize()
        {
            
        }

        public override void Update()
        {
            if (TGCGame.ControllerKeyP.Update().IsKeyToPressed()){
                TGCGame.SwitchActiveScreen(() => LevelScreen.GetInstance());
            }

            if (TGCGame.ControllerKeyA.Update().IsKeyToPressed()){
                TGCGame.SwitchActiveScreen(() => PauseInstructionsScreen.GetInstance());
            }
        }

        public override void DrawText()
        {
            DrawCenterTextY("CUBOS DE PODER", 60, 3);
            DrawCenterTextY("En el mapa se encuentran unos cubos misteriosos de color amarillo.", 180, 0.8f);
            DrawCenterTextY("Los llamamos CUBOS DE PODER: Recolectalos para obtener un poder!  ", 210, 0.8f);
            DrawCenterTextY("Existen los siguientes poderes:                                   ", 240, 0.8f);
            DrawCenterTextY("  *  Ametralladora: Dispara varias balas en direccion recta.      ", 270, 0.8f);
            DrawCenterTextY("  *  Lanzacohetes: Dispara un misil teledirigido.                 ", 300, 0.8f);
            DrawCenterTextY("  *  Nitro: Satisface tus ANSIAS DE VELOCIDAD por un corto tiempo.", 330, 0.8f);
        
            DrawLeftArrow();
        }
    }
}