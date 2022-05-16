using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Src;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Missile;

namespace TGC.Monogame.TP.Src.ModelObjects
{
    class PlayerCarObject : CarObject
    {
        private Boolean SpeedBoostActive {get; set;}
        private float SpeedBoostTime {get; set;}=0;
        private Boolean MachineGunActive {get; set;}
        private float MachineGunTime {get; set;}=0;
        private BulletObject bullet  {get; set;}
        //private List<Matrix> bulletsMatrix  {get; set;}
        private BulletObject[] MGBullets {get;set;}
        private int index {get;set;}=1;
        private Boolean MisileActive {get; set;}
        private float MisileTime {get; set;}=0;
        public PlayerCarObject(GraphicsDevice graphicsDevice, Vector3 position, Color color)
             : base(graphicsDevice, position, color)
        {
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix View, Matrix Projection){
            // Capturo el estado del teclado
            var keyboardState = Keyboard.GetState();

            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Si el auto esta tocando el piso
            if(Position.Y == 0){

                // Calculo la aceleracion y la velocidad
                //checkeo si queda tiempo de boostSpeed
                if( SpeedBoostTime > 0){
                    if (keyboardState.IsKeyDown(Keys.W)) {
                        Acceleration = ForwardAcceleration*5;
                        Speed = Math.Min(Speed + Acceleration * elapsedTime, MaxSpeed*2.5f);
                    }
                    else if (keyboardState.IsKeyDown(Keys.S)) {
                        Acceleration = - BackwardAcceleration*5;
                        Speed = Math.Max(Speed + Acceleration * elapsedTime, -MaxReverseSpeed*2);
                    }
                    else if (Speed > 0){
                        Acceleration = - StopAcceleration;
                        Speed = Math.Max(Speed + Acceleration * elapsedTime, 0);
                    }
                    else {
                        Acceleration = StopAcceleration;
                        Speed = Math.Min(Speed + Acceleration * elapsedTime, 0);
                    }
                    SpeedBoostTime-=elapsedTime;
                }else{
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
                }


                

                if(MachineGunTime > 0){
                    if (keyboardState.IsKeyDown(Keys.RightShift)) {
                        MGBullets = new BulletObject[]{
                            new BulletObject(graphicsDevice,Position,10f,Rotation)
                        };
                        //BulletObject bullet = new BulletObject(graphicsDevice,Position,10f,Rotation);

                        
                        //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Update(gameTime, Position, Rotation);
                        //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Draw(View, Projection);
                    }
                    MachineGunTime-=elapsedTime;
                }
                if(MGBullets != null){
                for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Initialize();
                for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Update(gameTime);
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
                VerticalSpeed = VerticalSpeed - Gravity * elapsedTime;
            }

            base.Update(gameTime);
        }

        /*public void SetSpeedBoostActive(Boolean isActive){
            SpeedBoostActive = isActive;
        }*/
        public void SetSpeedBoostTime(){
            SpeedBoostTime = 2f;
        }
        public void SetMachineGunTime(){
            MachineGunTime = 10f;
        }
        public void SetMisileTime(){
            MisileTime = 5f;
        }
        public BulletObject[] GetMGBullets(){
            return MGBullets;
        }
        
    }
}