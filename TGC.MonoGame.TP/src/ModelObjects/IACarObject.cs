using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.ModelObjects   
{
    public class IACarObject : CarObject
    {
        public IACarObject(Vector3 position, Color color)
             : base(position, color)
        {
        }
        public override void Update()
        {
            if (Speed > 0){
                Acceleration = - StopAcceleration;
                Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
            }
            else {
                Acceleration = StopAcceleration;
                Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
            }
            if (TurningSpeed > 0){
                TurningAcceleration = -MaxTurningAcceleration;
                TurningSpeed = Math.Max(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
            }else{
                TurningAcceleration = MaxTurningAcceleration;
                TurningSpeed = Math.Min(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
            }

            // Calculo rotacion del auto
            Rotation += TurningSpeed * TGCGame.GetElapsedTime();

            // Esto calcula la posicion del auto
            base.Update();
            
            ObjectBox.Center = Position;
            // Hacer que funcione cuando se incline
            ObjectBox.Orientation = Matrix.CreateRotationY(Rotation);
        }
    }
}