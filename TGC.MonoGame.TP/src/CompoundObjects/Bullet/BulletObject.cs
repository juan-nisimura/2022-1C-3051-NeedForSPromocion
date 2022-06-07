using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using System;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bullet
{
    public class BulletObject : DefaultObject <BulletObject>
    {
        protected BulletBodyObject BulletBody { get; set; }
        protected BulletHeadObject BulletHead { get; set; }
        private BoundingSphere BoundingSphere { get; set; }
        private float BoundingSphereRadius { get; set; }
        private float BoundingSphereCenter { get; set; }
        private const float BULLET_SPEED = 10000f;
        private const float MAX_ACTIVE_TIME = 10f;
        public const float BULLET_MODEL_SIZE = 3f;
        public const float BULLET_DAMAGE = 1f;
        private Vector3 Position;
        private Vector3 Forward;
        private bool IsActive = false;
        private float ActiveTime = 0f;
        private CarObject[] Enemies;
        public BulletObject(){
            BulletBody = new BulletBodyObject();
            BulletHead = new BulletHeadObject();

            BoundingSphereRadius = BULLET_MODEL_SIZE * 0.5f;
            BoundingSphere = new BoundingSphere(new Vector3(0f, BULLET_MODEL_SIZE, BULLET_MODEL_SIZE/2), BoundingSphereRadius);
        }

        public void Initialize(CarObject[] enemies){
            Enemies = enemies;
            BulletBody.Initialize();
            BulletHead.Initialize();
        }
        
        public static void Load(ContentManager content){
            BulletBodyObject.Load(content, "BasicShader");
            BulletHeadObject.Load(content, "BasicShader");
        }
        public override void Update(){
            if(IsActive){
                Position = Position - BULLET_SPEED * Forward * TGCGame.GetElapsedTime();

                BulletBody.Update(Position, Forward, RotationMatrix);
                BulletHead.Update(Position, Forward, RotationMatrix);
                // Tiene que cambiar con la rotación
                BoundingSphere = new BoundingSphere(Position, BoundingSphereRadius);

                ActiveTime += TGCGame.GetElapsedTime();

                IsActive = ActiveTime < MAX_ACTIVE_TIME;

                // Chequeo si colisionó con el auto
                for(int i = 0;i < TGCGame.PLAYERS_QUANTITY - 1;i++){
                    if(Enemies[i].ObjectBox.Intersects(BoundingSphere)){
                        // Si colisionó con el auto, el auto recibe daño de bala
                        IsActive = false;
                        Enemies[i].TakeDamage(BULLET_DAMAGE);
                        return;
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
                BulletBody.Draw(view, projection);
                BulletHead.Draw(view, projection);
            }
        }
    }
}