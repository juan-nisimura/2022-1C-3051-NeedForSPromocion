using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace TGC.Monogame.TP.Src.ModelObjects   
{
    class IACarObject : CarObject
    {
        public IACarObject(Vector3 position, Color color)
             : base(position, color)
        {
        }
        public override void Update(GameTime gameTime)
        {
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            if (Speed > 0){
                Acceleration = - StopAcceleration;
                Speed = Math.Max(Speed + Acceleration * elapsedTime, 0);
            }
            else {
                Acceleration = StopAcceleration;
                Speed = Math.Min(Speed + Acceleration * elapsedTime, 0);
            }
            if (TurningSpeed > 0){
                TurningAcceleration = -MaxTurningAcceleration;
                TurningSpeed = Math.Max(TurningSpeed + TurningAcceleration * elapsedTime, 0);
            }else{
                TurningAcceleration = MaxTurningAcceleration;
                TurningSpeed = Math.Min(TurningSpeed + TurningAcceleration * elapsedTime, 0);
            }

            // Calculo rotacion del auto
            Rotation += TurningSpeed * elapsedTime;

            // Esto calcula la posicion del auto
            base.Update(gameTime);
            
            ObjectBox.Center = Position;
            // Hacer que funcione cuando se incline
            ObjectBox.Orientation = Matrix.CreateRotationY(Rotation);
        }
    }
}