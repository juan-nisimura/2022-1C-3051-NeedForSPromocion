using System;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class PauseInstructionsScreen : TextScreen
    {
        protected override String SongName() { return "Riders On The Storm Fredwreck Remix"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new PauseInstructionsScreen();
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
                TGCGame.SwitchActiveScreen(() => PauseControlsScreen.GetInstance());
            }

            if (TGCGame.ControllerKeyD.Update().IsKeyToPressed()){
                TGCGame.SwitchActiveScreen(() => PausePowerUpsScreen.GetInstance());
            }
        }

        public override void DrawText()
        {
            DrawCenterTextY("INSTRUCCIONES", 60, 3);
            DrawCenterTextY("Objetivo: Derrota a todos los coches enemigos.                    ", 200, 0.8f);
            DrawCenterTextY("Un coche es derrotado cuando se agotan sus Puntos de Vida (PV).   ", 240, 0.8f);
            DrawCenterTextY("Un coche pierde PV si recibe un golpe fuerte o un impacto de arma.", 280, 0.8f);
            DrawCenterTextY("Si te derrotan o se acaba el tiempo, pierdes!                     ", 320, 0.8f);
        }
    }
}