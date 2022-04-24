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
        private float MaxSpeed { get; set; }
        private float MaxReverseSpeed { get; set; }
        private float ForwardAcceleration { get; set; }
        private float BackwardAcceleration { get; set; }
        private float StopAcceleration { get; set; }
        private float MaxTurningAcceleration { get; set; }
        private float MaxTurningSpeed { get; set; }
        private float Gravity { get; set; }
        private float JumpInitialSpeed { get; set; }

        private float Speed { get; set; } 
        private float Acceleration { get; set; }
        private float Rotation { get; set; }
        private float TurningSpeed { get; set; }
        private float TurningAcceleration { get; set; }
        private float VerticalSpeed { get; set; }
        public Vector3 Position { get; set; }

        public new void Initialize(){
            
            base.Initialize();
            
            // Configuro constantes 
            MaxSpeed = 2500f;
            MaxReverseSpeed = 1500f;
            ForwardAcceleration = 1500f;
            BackwardAcceleration = 1500f;
            StopAcceleration = 1500f;
            MaxTurningSpeed = MathF.PI / 3f;
            MaxTurningAcceleration = MathF.PI;
            Gravity = 300f;
            JumpInitialSpeed = 150f;

            // Inicializo variables
            Speed = 0;
            Acceleration = 0;
            Rotation = 0;
            TurningSpeed = 0;
            TurningAcceleration = 0;
            VerticalSpeed = 0;
            Position = new Vector3(0f, 0f, 0f);
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