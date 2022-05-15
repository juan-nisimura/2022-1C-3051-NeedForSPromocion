using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    class IACarObject : CarObject
    {
        public IACarObject(GraphicsDevice graphicsDevice, Vector3 position, Color color)
             : base(graphicsDevice, position, color)
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
            ObjectBox.Center = Position;
            ObjectBox.Orientation = Matrix.CreateRotationY(Rotation);

            base.Update(gameTime);
        }
    }
}