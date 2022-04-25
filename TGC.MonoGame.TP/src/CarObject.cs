using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    class CarObject : DefaultObject
    {
        private float MaxSpeed { get; set; } = 3000f;
        private float MaxReverseSpeed { get; set; } = 1500f;
        private float ForwardAcceleration { get; set; } = 3500f;
        private float BackwardAcceleration { get; set; } = 3500f;
        private float StopAcceleration { get; set; } = 2500f;
        private float MaxTurningAcceleration { get; set; } = MathF.PI * 5;
        private float MaxTurningSpeed { get; set; } = MathF.PI / 1.25f;
        private float Gravity { get; set; } = 400f;
        private float JumpInitialSpeed { get; set; } = 100f;

        private float Speed { get; set; } = 0;
        private float Acceleration { get; set; } = 0;
        private float Rotation { get; set; } = 0;
        private float TurningSpeed { get; set; } = 0;
        private float TurningAcceleration { get; set; } = 0;
        private float VerticalSpeed { get; set; } = 0;
        public Vector3 Position { get; set; } = new Vector3(-100f, 0f, 100f);

        public new void Initialize(){
            
            base.Initialize();

            World = Matrix.CreateScale(0.05f, 0.05f, 0.05f);
        }
        public new void Load(ContentManager content){
            ModelDirectory = "RacingCarA/RacingCar";
            DiffuseColor = Color.Blue.ToVector3();
            base.Load(content);
        }

        public override void Update(GameTime gameTime){
            // Capturo el estado del teclado
            var keyboardState = Keyboard.GetState();

            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Si el auto esta tocando el piso
            if(Position.Y == 0){

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
                VerticalSpeed = VerticalSpeed - Gravity * elapsedTime;
            }

            // Calculo la posicion vertical
            var verticalPosition = Math.Max(Position.Y + VerticalSpeed * elapsedTime, 0);

            World = Matrix.CreateScale(0.05f, 0.05f, 0.05f);
            World *= Matrix.CreateRotationY(Rotation);

            // Calculo la nueva posicion
            Position = new Vector3(Position.X - Speed * World.Forward.X * elapsedTime, verticalPosition, Position.Z - Speed * World.Forward.Z * elapsedTime);

            World *= Matrix.CreateTranslation(Position);
        }
    }
}