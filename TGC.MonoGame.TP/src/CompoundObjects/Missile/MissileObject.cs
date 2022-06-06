using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.ModelObjects;
using System;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    public class MissileObject : DefaultObject <MissileObject>
    {
        protected MissileBodyObject MissileBody { get; set; }
        protected MissileHeadObject MissileHead { get; set; }
        protected MissileTriangleObject[] MissileTriangles { get; set; }
        private BoundingSphere ImpactSphere { get; set; }
        private float ImpactSphereRadius { get; set; }
        private BoundingSphere DetectionSphere { get; set; }
        private float DETECTION_SPHERE_RADIUS = 100f;
        private const int TRIANGLES_QUANTITY = 8;
        private const float TRIANGLE_RELATIVE_SIZE = 0.8f;
        public const float MISSILE_MODEL_SIZE = 10f;
        private const float MISSILE_SPEED = 200f;
        private const float MAX_ACTIVE_TIME = 10f;
        private const float MISSILE_DAMAGE = 20f;
        private const float TURNING_LERP = 5f;
        private Vector3 Position;
        private Vector3 Forward;
        private bool IsActive = false;
        private float ActiveTime = 0f;
        private CarObject[] Enemies;
        public MissileObject(){
            MissileBody = new MissileBodyObject();
            MissileHead = new MissileHeadObject();
            MissileTriangles = new MissileTriangleObject[] {
                new MissileTriangleObject(new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),

                new MissileTriangleObject(new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(new Vector3(0f, 1f, -0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green)
            };
            ImpactSphereRadius = MISSILE_MODEL_SIZE * 0.5f;
            // ImpactSphere = new BoundingSphere(new Vector3(0f, MISSILE_MODEL_SIZE, MISSILE_MODEL_SIZE/2), ImpactSphereRadius);
            ImpactSphere = new BoundingSphere(new Vector3(0f, 0f, 0f), ImpactSphereRadius);
            DetectionSphere = new BoundingSphere(new Vector3(0f, 0f, 0f), DETECTION_SPHERE_RADIUS);
        }

        public void Initialize(CarObject[] enemies){
            Enemies = enemies;
            MissileBody.Initialize();
            MissileHead.Initialize();
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Initialize();
        }
        
        public static void Load(ContentManager content){
            MissileBodyObject.Load(content, "BasicShader");
            MissileHeadObject.Load(content, "BasicShader");
            MissileTriangleObject.Load(content, "BasicShader");
        }
        
        public override void Update(){
            if(IsActive){

                // Chequeo si detectó el auto
                for(int i = 0;i < TGCGame.PLAYERS_QUANTITY - 1;i++){
                    if(Enemies[i].ObjectBox.Intersects(DetectionSphere)){
                        // Si detectó el auto, el misil se dirige hacia él

                        // Calculo el nuevo vector forward
                        var vectorToEnemy = Vector3.Normalize(Position - Enemies[i].GetPosition());
                        var oldForward = Forward;

                        Forward.X = Lerp(Forward.X, vectorToEnemy.X, TURNING_LERP * TGCGame.GetElapsedTime());
                        Forward.Y = Lerp(Forward.Y, vectorToEnemy.Y, TURNING_LERP * TGCGame.GetElapsedTime());
                        Forward.Z = Lerp(Forward.Z, vectorToEnemy.Z, TURNING_LERP * TGCGame.GetElapsedTime());

                        Forward = Vector3.Normalize(Forward);                        

                        // Calculo la nueva matriz de rotación del misil
                        var anguloXZ = MathF.Atan2(Forward.Z * oldForward.X - Forward.X * oldForward.Z, Forward.X * oldForward.X + Forward.Z * oldForward.Z);
                        var anguloYZ = MathF.Atan2(Forward.Z * oldForward.Y - Forward.Y * oldForward.Z, Forward.Y * oldForward.Y + Forward.Z * oldForward.Z);
                        RotationMatrix *= Matrix.CreateRotationY(-anguloXZ);
                        RotationMatrix *= Matrix.CreateRotationX(anguloYZ);
                    }
                }

                Position = Position - MISSILE_SPEED * Forward * TGCGame.GetElapsedTime();

                MissileBody.Update(Position, Forward, RotationMatrix);
                MissileHead.Update(Position, Forward, RotationMatrix);
                for (int i = 0; i < TRIANGLES_QUANTITY; i++) MissileTriangles[i].Update(Position, Forward, RotationMatrix);

                // Tiene que cambiar con la rotación?
                ImpactSphere = new BoundingSphere(Position, ImpactSphereRadius);
                DetectionSphere = new BoundingSphere(Position, DETECTION_SPHERE_RADIUS);

                ActiveTime += TGCGame.GetElapsedTime();

                IsActive = ActiveTime < MAX_ACTIVE_TIME;

                // Chequeo si impactó con el auto
                for(int i = 0;i < TGCGame.PLAYERS_QUANTITY - 1;i++){
                    if(Enemies[i].ObjectBox.Intersects(ImpactSphere)){
                        // Si colisionó con el auto, el auto recibe daño de bala
                        IsActive = false;
                        Enemies[i].TakeDamage(MISSILE_DAMAGE);
                    }
                }
            }
        }

        public void Activate(Vector3 position, Vector3 forward, Matrix rotationMatrix) {
            IsActive = true;
            ActiveTime = 0f;
            Position = position;
            RotationMatrix = rotationMatrix;
            Forward = Vector3.Normalize(forward);
        }

        public override void Draw(Matrix view, Matrix projection){
            if(IsActive){
                MissileBody.Draw(view, projection);
                MissileHead.Draw(view, projection);
                for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Draw(view, projection);
            }
        }
    }
}