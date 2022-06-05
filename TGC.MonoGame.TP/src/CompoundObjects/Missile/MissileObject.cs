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
        private BoundingSphere BoundingSphere { get; set; }
        private float BoundingSphereRadius { get; set; }
        private float BoundingSphereCenter { get; set; }
        private const int TRIANGLES_QUANTITY = 8;
        private const float TRIANGLE_RELATIVE_SIZE = 0.8f;
        public const float MISSILE_MODEL_SIZE = 10f;
        private const float MISSILE_SPEED = 500f;
        private const float MAX_ACTIVE_TIME = 10f;
        private const float MISSILE_DAMAGE = 20f;
        private Vector3 Position;
        private Vector3 Forward;
        private bool IsActive = false;
        private float ActiveTime = 0f;
        private CarObject[] Enemies;
        public MissileObject(GraphicsDevice graphicsDevice){
            MissileBody = new MissileBodyObject(graphicsDevice);
            MissileHead = new MissileHeadObject(graphicsDevice);
            MissileTriangles = new MissileTriangleObject[] {
                new MissileTriangleObject(graphicsDevice, new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(graphicsDevice, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(graphicsDevice, new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(graphicsDevice, new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),

                new MissileTriangleObject(graphicsDevice, new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(graphicsDevice, new Vector3(0f, 1f, -0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(graphicsDevice, new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(graphicsDevice, new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green)
            };
            BoundingSphereRadius = MISSILE_MODEL_SIZE * 0.5f;
            BoundingSphere = new BoundingSphere(new Vector3(0f, MISSILE_MODEL_SIZE, MISSILE_MODEL_SIZE/2), BoundingSphereRadius);
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
                Position = Position - MISSILE_SPEED * Forward * TGCGame.GetElapsedTime();

                MissileBody.Update(Position, Forward, RotationMatrix);
                MissileHead.Update(Position, Forward, RotationMatrix);
                for (int i = 0; i < TRIANGLES_QUANTITY; i++) MissileTriangles[i].Update(Position, Forward, RotationMatrix);

                // Tiene que cambiar con la rotación?
                BoundingSphere = new BoundingSphere(Position, BoundingSphereRadius);

                ActiveTime += TGCGame.GetElapsedTime();

                IsActive = ActiveTime < MAX_ACTIVE_TIME;

                // Chequeo si colisionó con el auto
                for(int i = 0;i < TGCGame.PLAYERS_QUANTITY - 1;i++){
                    if(Enemies[i].ObjectBox.Intersects(BoundingSphere)){
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
            Forward = forward;
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