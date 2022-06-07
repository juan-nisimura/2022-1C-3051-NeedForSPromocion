using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP.Src.HUD;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUps;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.ModelObjects
{
    public class PlayerCarObject : CarObject
    {
        PowerUpHUDCircleObject PowerUpHUDCircle;
        public PlayerCarObject(Vector3 position, Color color)
             : base(position, color)
        {
            
        }

        public new void Initialize(CarObject[] enemies){
            base.Initialize(enemies);
            PowerUpHUDCircle = new PowerUpHUDCircleObject();
        }

        public void Update(Matrix View, Matrix Projection){
            // Capturo el estado del teclado
            var keyboardState = Keyboard.GetState();

            // Si el auto esta tocando el piso
            if(OnTheGround){

                // Calculo la aceleracion y la velocidad
                // checkeo si queda tiempo de boostSpeed
                if( SpeedBoostTime > 0){
                    if (keyboardState.IsKeyDown(Keys.W)) {
                        Acceleration = ForwardAcceleration*5;
                        Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), MaxSpeed*2.5f);
                    }
                    else if (keyboardState.IsKeyDown(Keys.S)) {
                        Acceleration = - BackwardAcceleration*5;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), -MaxReverseSpeed*2);
                    }
                    else if (Speed > 0){
                        Acceleration = - StopAcceleration;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                    else {
                        Acceleration = StopAcceleration;
                        Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                    SpeedBoostTime -= TGCGame.GetElapsedTime();
                }else{
                    if (keyboardState.IsKeyDown(Keys.W)) {
                    Acceleration = ForwardAcceleration;
                    Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), MaxSpeed);
                    }
                    else if (keyboardState.IsKeyDown(Keys.S)) {
                        Acceleration = - BackwardAcceleration;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), -MaxReverseSpeed);
                    }
                    else if (Speed > 0){
                        Acceleration = - StopAcceleration;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                    else {
                        Acceleration = StopAcceleration;
                        Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                }

                // Calculo aceleracion y velocidad de giro
                if (keyboardState.IsKeyDown(Keys.A) && Speed != 0){
                    TurningAcceleration = MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), -MaxTurningSpeed, MaxTurningSpeed);
                } 
                else if (keyboardState.IsKeyDown(Keys.D) && Speed != 0){
                    TurningAcceleration = -MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), -MaxTurningSpeed, MaxTurningSpeed);
                }
                else if (TurningSpeed > 0){
                    TurningAcceleration = -MaxTurningAcceleration;
                    TurningSpeed = Math.Max(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
                }
                else{
                    TurningAcceleration = MaxTurningAcceleration;
                    TurningSpeed = Math.Min(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
                }

                // Calculo rotacion del auto
                Rotation += TurningSpeed * TGCGame.GetElapsedTime();

                // Calculo velocidad vertical
                if(keyboardState.IsKeyDown(Keys.Space))
                    VerticalSpeed = JumpInitialSpeed;
                else
                    VerticalSpeed = 0;
            }
            // Si el auto esta en el aire
            else{
                // Calculo velocidad vertical
                VerticalSpeed = VerticalSpeed - GRAVITY * TGCGame.GetElapsedTime();
            }
            
            // Esto calcula la posición del auto
            base.Update();

            if(keyboardState.IsKeyDown(Keys.F)){
                PowerUp.TriggerEffect(this);
            }
        
            ObjectBox.Center = Position;
            ObjectBox.Orientation = RotationMatrix;
            
            PowerUpHUDCircle.Update(this);
        }

        public new void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
        }

        public void DrawHUD(Matrix view, Matrix projection)
        {
            PowerUpHUDCircle.Draw(view, projection);
        }
    }
}