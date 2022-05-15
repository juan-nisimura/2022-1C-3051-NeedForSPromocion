using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
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

        public float Speed { get; set; } = 0;
        public float Acceleration { get; set; } = 0;
        public float Rotation { get; set; } = 0;
        public float TurningSpeed { get; set; } = 0;
        protected float TurningAcceleration { get; set; } = 0;
        protected float VerticalSpeed { get; set; } = 0;
        public Vector3 Position { get; set; }

        protected WeaponObject Weapon { get; set; } = new WeaponObject();
        protected WheelObject[] FrontWheels { get; set; }
        protected WheelObject[] BackWheels { get; set; }


        //colisions
        // The World Matrix for the Chair Oriented Bounding Box
        private Matrix ObjectOBBWorld { get; set; }

        // The OrientedBoundingBox of the Chair
        public OrientedBoundingBox ObjectBox { get; set; }
        
        // The Y Angle for the Chair
        private float ObjectAngle { get; set; } = 0;

        public CarObject(GraphicsDevice graphicsDevice, Vector3 position, Color color){
            Position = position;
            DiffuseColor = color.ToVector3();
            FrontWheels = new WheelObject[] {
                new WheelObject(graphicsDevice, new Vector3(105f,45f,145f)),
                new WheelObject(graphicsDevice, new Vector3(-105f,45f,145f)),
            };
            BackWheels = new WheelObject[] {
                new WheelObject(graphicsDevice, new Vector3(105f,45f,-145f)),
                new WheelObject(graphicsDevice, new Vector3(-105f,45f,-145f)),
            };
        }

        public new void Initialize(){
            base.Initialize();
            ScaleMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f);
            Weapon.Initialize();
            for(int i = 0;i < FrontWheels.Length;i++)    FrontWheels[i].Initialize();
            for(int i = 0;i < BackWheels.Length;i++)     BackWheels[i].Initialize();
        }
        public new void Load(ContentManager content){
            
            ModelDirectory = "RacingCarA/RacingCar";
            base.Load(content);

            // Create an OBB for a model
            // First, get an AABB from the model
            var temporaryCubeAABB = BoundingVolumesExtensions.CreateAABBFrom(Model);
            // Scale it to match the model's transform
            temporaryCubeAABB = BoundingVolumesExtensions.Scale(temporaryCubeAABB, 0.05f);
            // Create an Oriented Bounding Box from the AABB
            ObjectBox = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
            // Move the center
            ObjectBox.Center = Position;
            // Then set its orientation!
            // Hacerlo que funcione cuando se inclina
            ObjectBox.Orientation = Matrix.CreateRotationY(ObjectAngle);


            Weapon.Load(content);
            for(int i = 0;i < FrontWheels.Length;i++)    FrontWheels[i].Load(content);
            for(int i = 0;i < BackWheels.Length;i++)     BackWheels[i].Load(content);
        }

        public override void Update(GameTime gameTime){
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            World = ScaleMatrix;
            World *= Matrix.CreateRotationY(Rotation);

            // Rotate the box
            //ObjectAngle += 0.01f;
            //ObjectBox.Orientation = Matrix.CreateRotationY(Rotation);

            // Calculo la nueva posicion
            Position = new Vector3(Position.X - Speed * World.Forward.X * elapsedTime, Math.Max(Position.Y + VerticalSpeed * elapsedTime, 0), Position.Z - Speed * World.Forward.Z * elapsedTime);

            World *= Matrix.CreateTranslation(Position);

            

            Weapon.FollowCar(World);
            for(int i = 0;i < FrontWheels.Length;i++)    FrontWheels[i].FollowCar(World, TurningSpeed);
            for(int i = 0;i < BackWheels.Length;i++)     BackWheels[i].FollowCar(World, 0);
        }

        public new void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
            Weapon.Draw(view, projection);
            for(int i = 0;i < FrontWheels.Length;i++)    FrontWheels[i].Draw(view, projection);
            for(int i = 0;i < BackWheels.Length;i++)     BackWheels[i].Draw(view, projection);
        }

        public Vector3 GetPosition()
        {
            return Position;
        }
    }
}