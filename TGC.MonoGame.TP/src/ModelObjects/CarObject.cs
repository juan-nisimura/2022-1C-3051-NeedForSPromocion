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
    public class CarObject : DefaultModelObject <CarObject>
    {
        protected float MaxSpeed { get; set; } = 3000f;
        protected float MaxReverseSpeed { get; set; } = 1500f;
        protected float ForwardAcceleration { get; set; } = 3500f;
        protected float BackwardAcceleration { get; set; } = 3500f;
        protected float StopAcceleration { get; set; } = 2500f;
        protected float MaxTurningAcceleration { get; set; } = MathF.PI * 5;
        protected float MaxTurningSpeed { get; set; } = MathF.PI / 1.25f;
        protected float JumpInitialSpeed { get; set; } = 100f;

        public float Speed { get; set; } = 0;
        public float Acceleration { get; set; } = 0;
        public float Rotation { get; set; } = 0;
        public float TurningSpeed { get; set; } = 0;
        protected float TurningAcceleration { get; set; } = 0;
        protected float VerticalSpeed { get; set; } = 0;

        protected float WheelAngle { get; set; } = 0;
        public Vector3 Position { get; set; }
        protected WeaponObject Weapon { get; set; } = new WeaponObject();

        protected float SpeedBoostTime {get; set;}=0;
        protected float MachineGunTime {get; set;}=0;
        protected float MissileTime {get; set;}=0;
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
        public bool HasCrashed = false;
        public static float HIPOTENUSA_AL_VERTICE;
        public static float ANGULO_AL_VERTICE;
        public float GroundLevel = 0;
        public bool OnTheGround = true;
        public Vector3 Size;

        public CarObject(Vector3 position, Color color){
            Position = position;
            DiffuseColor = color.ToVector3();

            // Create an OBB for a model
            // First, get an AABB from the model
            var temporaryCubeAABB = BoundingVolumesExtensions.CreateAABBFrom(getModel());
            // Scale it to match the model's transform
            temporaryCubeAABB = BoundingVolumesExtensions.Scale(temporaryCubeAABB, 0.05f);

            Size = temporaryCubeAABB.Max - temporaryCubeAABB.Min;

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

            var temporalCarObject = new CarObject(new Vector3(0f, 0f, 0f), Color.Green);

            ANGULO_AL_VERTICE = MathF.Atan(temporalCarObject.Size.X / temporalCarObject.Size.Z);
            HIPOTENUSA_AL_VERTICE = MathF.Sqrt(MathF.Pow(temporalCarObject.Size.X / 2, 2) + MathF.Pow(temporalCarObject.Size.Z / 2, 2));
        }

        public void RotateX(Vector3 normal) {
            var angulo = MathF.Acos(Convert.ToSingle(Vector3.Dot(Vector3.Normalize(new Vector3(normal.X, 0f, normal.Z)), normal)));
            RotationMatrix = Matrix.CreateRotationX(angulo - MathF.PI / 2) * Matrix.CreateRotationY(Rotation);
        }

        public override void Update(GameTime gameTime){
            
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            RotationMatrix = Matrix.CreateRotationY(Rotation);

            // Calculo la nueva posicion
            var x = Position.X - Speed * World.Forward.X * elapsedTime;
            var z = Position.Z - Speed * World.Forward.Z * elapsedTime;
            var y = Math.Max(Position.Y - Speed * World.Forward.Y * elapsedTime + VerticalSpeed * elapsedTime, GroundLevel);

            Position = new Vector3(x, y, z);

            OnTheGround = Position.Y - GroundLevel < 0.5f;

            // Calculo el nuevo ángulo de las ruedas
            WheelAngle += Speed * elapsedTime / 125f;

            TranslateMatrix = Matrix.CreateTranslation(Position);

            var forward = RotationMatrix.Forward;

            var differentialAngleX = HeightMap.GetDifferentialAngle(Position, forward);
            var differentialAngleZ = HeightMap.GetDifferentialAngle(Position, new Vector3(forward.Z, 0f, -forward.X));

            if(MathF.Abs(differentialAngleX) < MathF.PI / 6)
                RotationMatrix = Matrix.CreateRotationX(differentialAngleX) * RotationMatrix;
            if(MathF.Abs(differentialAngleZ) < MathF.PI / 6)
                RotationMatrix = Matrix.CreateRotationZ(-differentialAngleZ) * RotationMatrix;

            // Rotate the box
            ObjectBox.Orientation = RotationMatrix;

            World = ScaleMatrix * RotationMatrix * TranslateMatrix;

            Weapon.FollowCar(World);
        }

        public new void Draw(Matrix view, Matrix projection)
        {
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;

            getEffect().Parameters["View"]?.SetValue(view);
            getEffect().Parameters["Projection"]?.SetValue(projection);
            getEffect().Parameters["ModelTexture"]?.SetValue(getTexture());

            CarBodyObject.Draw(getEffect(), getModel().Meshes["Car"], World);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelA"], World, WheelAngle, TurningSpeed * MathF.Sign(Speed));
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelB"], World, WheelAngle, TurningSpeed * MathF.Sign(Speed));
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelC"], World, WheelAngle, 0);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelD"], World, WheelAngle, 0);
            Weapon.Draw(view, projection);
        }
        public new void DrawBloom(Effect effect, Matrix view, Matrix projection)
        {
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;

            effect.CurrentTechnique = effect.Techniques["BloomPass"];
            effect.Parameters["baseTexture"].SetValue(getTexture());
            CarBodyObject.Draw(getEffect(), getModel().Meshes["Car"], World);
            Weapon.Draw(view, projection);
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public void Crash()
        {
            Speed = -Speed/4;
        }

        public void SetSpeedBoostTime(){
            SpeedBoostTime = 2f;
        }
        public void SetMachineGunTime(){
            MachineGunTime = 5f;
        }
        public void SetMisileTime(){
            MissileTime = 5f;
        }
    }
}