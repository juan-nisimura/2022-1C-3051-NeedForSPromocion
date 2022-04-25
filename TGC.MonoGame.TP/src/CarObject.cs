using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
namespace TGC.Monogame.TP.Src   
{
    class CarObject : DefaultObject
    {
        protected float MaxSpeed { get; set; } = 3000f;
        protected float MaxReverseSpeed { get; set; } = 1500f;
        protected float ForwardAcceleration { get; set; } = 3500f;
        protected float BackwardAcceleration { get; set; } = 3500f;
        protected float StopAcceleration { get; set; } = 2500f;
        protected float MaxTurningAcceleration { get; set; } = MathF.PI * 5;
        protected float MaxTurningSpeed { get; set; } = MathF.PI / 1.25f;
        protected float Gravity { get; set; } = 400f;
        protected float JumpInitialSpeed { get; set; } = 100f;

        protected float Speed { get; set; } = 0;
        protected float Acceleration { get; set; } = 0;
        protected float Rotation { get; set; } = 0;
        protected float TurningSpeed { get; set; } = 0;
        protected float TurningAcceleration { get; set; } = 0;
        protected float VerticalSpeed { get; set; } = 0;
        protected Vector3 Position { get; set; }

        protected WeaponObject Weapon { get; set; } = new WeaponObject();

        public CarObject(Vector3 position, Color color){
            Position = position;
            DiffuseColor = color.ToVector3();
        }

        public new void Initialize(){
            base.Initialize();
            ScaleMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f);
            Weapon.Initialize();
        }
        public new void Load(ContentManager content){
            ModelDirectory = "RacingCarA/RacingCar";
            base.Load(content);
            Weapon.Load(content);
        }

        public override void Update(GameTime gameTime){
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            World = ScaleMatrix;
            World *= Matrix.CreateRotationY(Rotation);

            // Calculo la nueva posicion
            Position = new Vector3(Position.X - Speed * World.Forward.X * elapsedTime, Math.Max(Position.Y + VerticalSpeed * elapsedTime, 0), Position.Z - Speed * World.Forward.Z * elapsedTime);

            World *= Matrix.CreateTranslation(Position);

            Weapon.FollowCar(World);
        }

        public new void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
            Weapon.Draw(view, projection);
        }

        public Vector3 GetPosition()
        {
            return Position;
        }
    }
}