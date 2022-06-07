using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.ModelObjects;
using Microsoft.Xna.Framework.Audio;
using System;

namespace TGC.Monogame.TP.Src.CompoundObjects.Projectiles
{
    public abstract class Projectile <T> : DefaultObject <T>
    {
        public BoundingSphere ImpactSphere { get; set; }
        protected float ImpactSphereRadius { get; set; }
        protected Vector3 Position;
        protected Vector3 Forward;
        public bool IsActive = false;
        protected float ActiveTime = 0f;
        protected CarObject[] Enemies;
        protected SoundEffect ObstacleHitSound;
        protected SoundEffect EnemyHitSound;
        protected SoundEffect ShootSound;

        public void Initialize(CarObject[] enemies){
            Enemies = enemies;
        }

        public void HitObstacle(){
            IsActive = false;
            ObstacleHitSound.CreateInstance().Play();
        }

        public void Activate(Vector3 position, Vector3 forward, Matrix rotationMatrix) {
            IsActive = true;
            ActiveTime = 0f;
            Position = position + new Vector3(0f, 5f, 0f);
            RotationMatrix = rotationMatrix;
            Forward = Vector3.Normalize(forward);
            ShootSound.CreateInstance().Play();
            ImpactSphere = new BoundingSphere(Position, ImpactSphereRadius);
        }

        public void SetTranslateMatrix(Matrix translateMatrix){
            this.TranslateMatrix = translateMatrix;
        }
    }
}