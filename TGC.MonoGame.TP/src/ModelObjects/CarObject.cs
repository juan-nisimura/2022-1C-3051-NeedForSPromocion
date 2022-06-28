using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.Samples.Collisions;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.HUD;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUps;
using Microsoft.Xna.Framework.Audio;
using TGC.Monogame.TP.Src.MyContentManagers;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels;

namespace TGC.Monogame.TP.Src.ModelObjects

{
    public abstract class CarObject : DefaultModelObject <CarObject>
    {
        public static float  FAST_SPEED = 2000f;
        protected static float MaxSpeed { get; set; } = 3000f;
        protected static float MaxReverseSpeed { get; set; } = 1500f;
        protected static float ForwardAcceleration { get; set; } = 3500f;
        protected static float BackwardAcceleration { get; set; } = 3500f;
        protected static float StopAcceleration { get; set; } = 2500f;
        protected static float MaxTurningAcceleration { get; set; } = MathF.PI * 5;
        protected static float MaxTurningSpeed { get; set; } = MathF.PI / 1.25f;
        protected static float JumpInitialSpeed { get; set; } = 100f;
        protected Vector3 InitialPosition { get; set; }
 
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

        public const int BULLETS_POOL_SIZE = 20;
        public const int MISSILES_POOL_SIZE = 2;

        public BulletObject[] BulletsPool = new BulletObject[BULLETS_POOL_SIZE];
        public MissileObject[] MissilesPool = new MissileObject[MISSILES_POOL_SIZE];

        protected int NextBulletIndex = 0;
        protected int NextMissileIndex = 0;
        public CarObject[] Enemies;

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
        protected Vector3 ForceSpeedDirection = Vector3.Zero;
        protected float ForceSpeedModule = 0f;
        public HealthBarObject HealthBar;
        public CarSoundEffects CarSoundEffects;

        protected PowerUp PowerUp = new NullPowerUp();
        public static float MAX_HEALTH = 100;
        public float Health = MAX_HEALTH;

        protected static SoundEffect SpeedBoostSound;

        public CarObject(Vector3 position, Color color){
            InitialPosition = position;
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

        public void Initialize(CarObject[] enemies){
            base.Initialize();
            ScaleMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f);
            Weapon.Initialize();
            HealthBar = new HealthBarObject();
            HealthBar.Initialize();
            CarSoundEffects = new CarSoundEffects();
            CarSoundEffects.Initialize();
            Enemies = enemies;
            for(int i = 0; i < BULLETS_POOL_SIZE; i++){
                BulletsPool[i] = new BulletObject(i);
                BulletsPool[i].Initialize(Enemies);
            }  

            for(int i = 0; i < MISSILES_POOL_SIZE; i++){
                MissilesPool[i] = new MissileObject();
                MissilesPool[i].Initialize(Enemies);
            }
            Update();
        }

        public void Reset(){
            Health = MAX_HEALTH;
            Speed = 0f;
            Acceleration = 0f;
            VerticalSpeed = 0f;
            Position = InitialPosition;
            CarSoundEffects.Reset();
            Rotation = 0;
            TurningSpeed = 0;
            TurningAcceleration = 0;
            WheelAngle = 0;
            SpeedBoostTime = 0;
            ForceSpeedDirection = Vector3.Zero;
            ForceSpeedModule = 0;
            PowerUp = new NullPowerUp();
            PowerUpHUDCircleObject.SetPowerUpModel(new NullPowerUpModel());
            for(int i = 0; i < BULLETS_POOL_SIZE; i++)
                BulletsPool[i].IsActive = false;
            for(int i = 0; i < MISSILES_POOL_SIZE; i++)
                MissilesPool[i].IsActive = false;
        }

        public void Stop(){
            CarSoundEffects.Stop();
        }

        public void Start(){
            if(IsDead())
                return;
            CarSoundEffects.Start();
        }

        public static void Load(){
            DefaultLoad("RacingCarA/RacingCar", "CarShader");
            WeaponObject.Load();
            CarSoundEffects.Load();
            PowerUpHUDCircleObject.Load("PowerUpHUDCircleShader");

            var temporalCarObject = new PlayerCarObject(new Vector3(0f, 0f, 0f), Color.Green);

            ANGULO_AL_VERTICE = MathF.Atan(temporalCarObject.Size.X / temporalCarObject.Size.Z);
            HIPOTENUSA_AL_VERTICE = MathF.Sqrt(MathF.Pow(temporalCarObject.Size.X / 2, 2) + MathF.Pow(temporalCarObject.Size.Z / 2, 2));
        
            SpeedBoostSound = MyContentManager.SoundEffects.Load("boost engine");
        }

        public bool IsDead() {
            return Health <= 0;
        }

        public void RotateX(Vector3 normal) {
            var angulo = MathF.Acos(Convert.ToSingle(Vector3.Dot(Vector3.Normalize(new Vector3(normal.X, 0f, normal.Z)), normal)));
            RotationMatrix = Matrix.CreateRotationX(angulo - MathF.PI / 2) * Matrix.CreateRotationY(Rotation);
        }

        public override void Update(){

            RotationMatrix = Matrix.CreateRotationY(Rotation);

            ForceSpeedModule = MathF.Max(ForceSpeedModule - 100f * TGCGame.GetElapsedTime(), 0f);
            var forceSpeed = ForceSpeedModule * ForceSpeedDirection;

            // Calculo la nueva posicion
            var x = Position.X + (- Speed * World.Forward.X + forceSpeed.X) * TGCGame.GetElapsedTime();
            var z = Position.Z + (- Speed * World.Forward.Z + forceSpeed.Z) * TGCGame.GetElapsedTime();
            var y = Math.Max(Position.Y - Speed * World.Forward.Y * TGCGame.GetElapsedTime() + VerticalSpeed * TGCGame.GetElapsedTime(), GroundLevel);

            Position = new Vector3(x, y, z);

            OnTheGround = Position.Y - GroundLevel < 0.5f;
            if(OnTheGround)     Position = new Vector3(x, GroundLevel, z);

            // Calculo el nuevo ángulo de las ruedas
            WheelAngle += Speed * TGCGame.GetElapsedTime() / 125f;

            TranslateMatrix = Matrix.CreateTranslation(Position);

            var forward = RotationMatrix.Forward;

            var differentialAngleX = HeightMap.GetDifferentialAngle(Position, forward, HeightMap.GetActualLevel(Position.Y));
            var differentialAngleZ = HeightMap.GetDifferentialAngle(Position, new Vector3(forward.Z, 0f, -forward.X), HeightMap.GetActualLevel(Position.Y));

            if(MathF.Abs(differentialAngleX) < MathF.PI / 6)
                RotationMatrix = Matrix.CreateRotationX(differentialAngleX) * RotationMatrix;
            if(MathF.Abs(differentialAngleZ) < MathF.PI / 6)
                RotationMatrix = Matrix.CreateRotationZ(-differentialAngleZ) * RotationMatrix;

            // Rotate the box
            ObjectBox.Orientation = RotationMatrix;

            World = ScaleMatrix * RotationMatrix * TranslateMatrix;

            Weapon.FollowCar(World);
            HealthBar.Update(this);
            CarSoundEffects.Update(this);

            for(int i = 0; i < BULLETS_POOL_SIZE; i++)  BulletsPool[i].Update();
            for(int i = 0; i < MISSILES_POOL_SIZE; i++) MissilesPool[i].Update();
        }

        public void Draw(Matrix view, Matrix projection, Effect carEffect)
        {
            if (IsDead())
                return;
            foreach (var meshPart in getModel().Meshes["Car"].MeshParts)
                meshPart.Effect = carEffect;
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
            HealthBar.Draw(view, projection);
            for (int i = 0; i < BULLETS_POOL_SIZE; i++) BulletsPool[i].Draw(view, projection);
            for (int i = 0; i < MISSILES_POOL_SIZE; i++) MissilesPool[i].Draw(view, projection);
        }
        public void DrawBloom( Matrix view, Matrix projection, Effect bloom)
        {

            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;

            bloom.CurrentTechnique = bloom.Techniques["BloomPass"];
            bloom.Parameters["baseTexture"].SetValue(getTexture());
            bloom.Parameters["View"].SetValue(view);
            bloom.Parameters["Projection"].SetValue(projection);
            //CarBodyObject.Draw(getEffect(), getModel().Meshes["Car"], World);
            var meshWorld = getModel().Meshes["Car"].ParentBone.Transform * World;
            bloom.Parameters["World"].SetValue(meshWorld);
            //bloom.Parameters["WorldViewProjection"].SetValue(meshWorld * view * projection);
            foreach (var meshPart in getModel().Meshes["Car"].MeshParts)
                meshPart.Effect = bloom;
            
            //getModel().Meshes["Car"].Draw();

            CarBodyObject.Draw(bloom, getModel().Meshes["Car"], World);
            Weapon.Draw(view, projection);
        }

        public void Draw(Matrix view, Matrix projection, Effect effect ,RenderTargetCube EnvironmentMapRenderTarget, Vector3 CameraPosition)
        {
            if(IsDead())
                return;
            foreach (var meshPart in getModel().Meshes["Car"].MeshParts)
                meshPart.Effect = effect;
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;

            getEffect().Parameters["View"]?.SetValue(view);
            getEffect().Parameters["Projection"]?.SetValue(projection);
            getEffect().Parameters["ModelTexture"]?.SetValue(getTexture());

            //enviroment
            getEffect().Parameters["environmentMap"].SetValue(EnvironmentMapRenderTarget);
            getEffect().Parameters["eyePosition"].SetValue(CameraPosition);
            getEffect().Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(World)));


            CarBodyObject.Draw(getEffect(), getModel().Meshes["Car"], World);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelA"], World, WheelAngle, TurningSpeed * MathF.Sign(Speed));
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelB"], World, WheelAngle, TurningSpeed * MathF.Sign(Speed));
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelC"], World, WheelAngle, 0);
            WheelObject.Draw(getEffect(), getModel().Meshes["WheelD"], World, WheelAngle, 0);
            Weapon.Draw(view, projection);
            HealthBar.Draw(view, projection);
            for(int i = 0; i < BULLETS_POOL_SIZE; i++)  BulletsPool[i].Draw(view, projection);
            for(int i = 0; i < MISSILES_POOL_SIZE; i++) MissilesPool[i].Draw(view, projection);
        }



        public Vector3 GetPosition()
        {
            return Position;
        }

        public void Crash()
        {
            Acceleration = 0;
            var crashDamage = MathF.Floor(MathF.Abs(Speed / 2000));
            TakeDamage(crashDamage);
            if(crashDamage > 0)
                CarSoundEffects.PlayCrashSound();
            Speed = -Speed/5;
        }

        public void ApplyForce(Vector3 force, float forceModule) { 
            this.ForceSpeedDirection = force;
            this.ForceSpeedModule = forceModule;
        }

        public bool SolveHorizontalCollision(CarObject enemyCar){
            
            if(IsDead() || enemyCar.IsDead())
                return false;

            // Chequeo si colisionó con el auto
            if(this.ObjectBox.Intersects(enemyCar.ObjectBox)){

                // SOLUCION RARA
                var thisNormalizedForward = Vector3.Normalize(new Vector3(this.World.Forward.X, 0, this.World.Forward.Z));
                var thisNormalizedSideward = new Vector3(thisNormalizedForward.Z, 0, -thisNormalizedForward.X);
                var enemyNormalizedForward = Vector3.Normalize(new Vector3(enemyCar.World.Forward.X, 0, enemyCar.World.Forward.Z));
                var forwardOBBExtents = new Vector3(Size.X, 10f, 0.1f);
                var sidewardOBBExtents = new Vector3(0.1f, 10f, Size.Z);

                var totalForce = Vector3.Zero;

                if(enemyCar.ObjectBox.Intersects(new OrientedBoundingBox(this.Position - thisNormalizedForward * Size.Z / 2, forwardOBBExtents))){
                    totalForce += -thisNormalizedForward;
                }
                if(enemyCar.ObjectBox.Intersects(new OrientedBoundingBox(this.Position + thisNormalizedForward * Size.Z / 2, forwardOBBExtents))){
                    totalForce += thisNormalizedForward;
                }
                if(enemyCar.ObjectBox.Intersects(new OrientedBoundingBox(this.Position - thisNormalizedSideward * Size.X / 2, sidewardOBBExtents))){
                    totalForce += -thisNormalizedSideward;
                }
                if(enemyCar.ObjectBox.Intersects(new OrientedBoundingBox(this.Position + thisNormalizedSideward * Size.X / 2, sidewardOBBExtents))){
                    totalForce += thisNormalizedSideward;
                }
                if(totalForce.Equals(Vector3.Zero))
                    return false;

                enemyCar.ApplyForce(Vector3.Normalize(totalForce), 60f);
                this.ApplyForce(-Vector3.Normalize(totalForce), 60f);

                var distance = this.Position - enemyCar.Position;
                var damageVector = Vector3.Dot(this.Speed * thisNormalizedForward, distance) / MathF.Pow(distance.Length(), 2) * distance ;
                damageVector += Vector3.Dot(enemyCar.Speed * enemyNormalizedForward, distance) / MathF.Pow(distance.Length(), 2) * distance ;
                var damage = MathF.Floor(MathF.Abs(damageVector.Length() / 3000f));

                enemyCar.TakeDamage(damage);
                this.TakeDamage(damage);
                
                enemyCar.Speed = enemyCar.Speed / 1.5f;
                this.Speed = this.Speed / 1.5f;

                enemyCar.HasCrashed = true;
                this.HasCrashed = true;

                return true;
            } 
                
            return false;
        }

        public void SetSpeedBoostTime(){
            if(SpeedBoostTime <= 0f)
                SpeedBoostSound.CreateInstance().Play();
            SpeedBoostTime = 2f;
        }

        public void SetPowerUp(PowerUp powerUp) {
            this.PowerUp = powerUp;
        }

        public abstract void SetPowerUpHUDModel(PowerUpModel powerUpModel);
        
        public void ShootBullet() {
            BulletsPool[GetNextBulletIndex()].Activate(Position, World.Forward, RotationMatrix);
        }

        protected int GetNextBulletIndex() {
            if(++NextBulletIndex >= BULLETS_POOL_SIZE)
                NextBulletIndex = 0;
            return NextBulletIndex;
        }

        public void ShootMissile() {
            MissilesPool[GetNextMissileIndex()].Activate(Position, World.Forward, RotationMatrix);
        }

        protected int GetNextMissileIndex() {
            if(++NextMissileIndex >= MISSILES_POOL_SIZE)
                NextMissileIndex = 0;
            return NextMissileIndex;
        }

        public void TakeDamage(float damage){
            Health -= damage;
        }
    }
}