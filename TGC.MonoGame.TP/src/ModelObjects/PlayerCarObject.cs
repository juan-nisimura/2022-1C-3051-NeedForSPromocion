using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TGC.Monogame.TP.Src.ModelObjects
{
    class PlayerCarObject : CarObject
    {
        public PlayerCarObject(Vector3 position, Color color)
             : base(position, color)
        {
        }

        public override void Update(GameTime gameTime){
            // Capturo el estado del teclado
            var keyboardState = Keyboard.GetState();
    
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            Console.WriteLine("{0}", OnTheGround);

            // Si el auto esta tocando el piso
            if(OnTheGround){

                // Calculo la aceleracion y la velocidad
                if (keyboardState.IsKeyDown(Keys.W)) {
                    Acceleration = ForwardAcceleration;
                    Speed = Math.Min(Speed + Acceleration * elapsedTime, MaxSpeed);
                }
                else if (keyboardState.IsKeyDown(Keys.S)) {
                    Acceleration = - BackwardAcceleration;
                    Speed = Math.Max(Speed + Acceleration * elapsedTime, -MaxReverseSpeed);
                }
                else if (Speed > 0){
                    Acceleration = - StopAcceleration;
                    Speed = Math.Max(Speed + Acceleration * elapsedTime, 0);
                }
                else {
                    Acceleration = StopAcceleration;
                    Speed = Math.Min(Speed + Acceleration * elapsedTime, 0);
                }

                // Calculo aceleracion y velocidad de giro
                if (keyboardState.IsKeyDown(Keys.A) && Speed != 0){
                    TurningAcceleration = MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * elapsedTime, -MaxTurningSpeed, MaxTurningSpeed);
                } 
                else if (keyboardState.IsKeyDown(Keys.D) && Speed != 0){
                    TurningAcceleration = -MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * elapsedTime, -MaxTurningSpeed, MaxTurningSpeed);
                }
                else if (TurningSpeed > 0){
                    TurningAcceleration = -MaxTurningAcceleration;
                    TurningSpeed = Math.Max(TurningSpeed + TurningAcceleration * elapsedTime, 0);
                }
                else{
                    TurningAcceleration = MaxTurningAcceleration;
                    TurningSpeed = Math.Min(TurningSpeed + TurningAcceleration * elapsedTime, 0);
                }

                // Calculo rotacion del auto
                Rotation += TurningSpeed * elapsedTime;

                // Calculo velocidad vertical
                if(keyboardState.IsKeyDown(Keys.Space))
                    VerticalSpeed = JumpInitialSpeed;
                else
                    VerticalSpeed = 0;
            }
            // Si el auto esta en el aire
            else{
                // Calculo velocidad vertical
                VerticalSpeed = VerticalSpeed - GRAVITY * elapsedTime;
            }
            
            // Esto calcula la posición del auto
            base.Update(gameTime);
        
            ObjectBox.Center = Position;
            // Hacerlo que funcione cuando se incline
            ObjectBox.Orientation = Matrix.CreateRotationY(Rotation);
        }
    }
}