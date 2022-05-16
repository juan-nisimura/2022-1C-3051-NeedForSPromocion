using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Missile;


namespace TGC.Monogame.TP.Src.ModelObjects

{
    class CarObject : DefaultModelObject <CarObject>
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

        protected float WheelAngle { get; set; } = 0;
        protected Vector3 Position { get; set; }
        protected WeaponObject Weapon { get; set; } = new WeaponObject();
        //protected BulletObject[] MGBullets {get;set;}
        //protected int indexBullet {get; set;}=0;
        //protected List<BulletObject> MGBulletsList {get;set;}
        

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

            // Create an OBB for a model
            // First, get an AABB from the model
            var temporaryCubeAABB = BoundingVolumesExtensions.CreateAABBFrom(getModel());
            // Scale it to match the model's transform
            temporaryCubeAABB = BoundingVolumesExtensions.Scale(temporaryCubeAABB, 0.05f);
            // Create an Oriented Bounding Box from the AABB
            ObjectBox = OrientedBoundingBox.FromAABB(temporaryCubeAABB);
            // Move the center
            ObjectBox.Center = Position;
            // Then set its orientation!
            // Hacerlo que funcione cuando se inclina
            ObjectBox.Orientation = Matrix.CreateRotationY(ObjectAngle);
        }

        public new void Initialize(){
            base.Initialize();
            ScaleMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f);
            Weapon.Initialize();
            //MGBulletsList = new List<BulletObject>();
        }

        public static void Load(ContentManager content){
            DefaultLoad(content, "RacingCarA/RacingCar", "CarShader");
            WeaponObject.Load(content);
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

            // Calculo el nuevo ángulo de las ruedas
            WheelAngle += Speed * elapsedTime / 250f;

            World *= Matrix.CreateTranslation(Position);

            

            Weapon.FollowCar(World);
        }

        public new void Draw(Matrix view, Matrix projection)
        {
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            getEffect().Parameters["View"]?.SetValue(view);
            getEffect().Parameters["Projection"]?.SetValue(projection);
            getEffect().Parameters["ModelTexture"]?.SetValue(getTexture());

            CarBodyObject.Draw(getEffect(), getModel().Meshes["Car"], World);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelA"], World, WheelAngle, TurningSpeed);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelB"], World, WheelAngle, TurningSpeed);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelC"], World, WheelAngle, 0);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelD"], World, WheelAngle, 0);
            Weapon.Draw(view, projection);
        }

        public Vector3 GetPosition()
        {
            return Position;
        }
    }
}