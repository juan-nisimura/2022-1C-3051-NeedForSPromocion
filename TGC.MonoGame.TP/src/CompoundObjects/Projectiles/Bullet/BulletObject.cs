using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.ModelObjects;
using Microsoft.Xna.Framework.Audio;
using TGC.Monogame.TP.Src.MyContentManagers;

namespace TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet
{
    public class BulletObject : Projectile <BulletObject>
    {
        protected BulletBodyObject BulletBody { get; set; }
        protected BulletHeadObject BulletHead { get; set; }
        private const float BULLET_SPEED = 300f;
        private const float MAX_ACTIVE_TIME = 10f;
        public const float BULLET_MODEL_SIZE = 3f;
        public const float BULLET_DAMAGE = 1f;
        private const int GROUND_BULLET_SOUNDS_QUANTITY = 5;
        private const int METAL_BULLET_SOUNDS_QUANTITY = 4;
        private const int BULLET_SHOOT_SOUNDS_QUANTITY = 3;
        private static SoundEffect[] GroundBulletSounds;
        private static SoundEffect[] MetalBulletSounds;
        private static SoundEffect[] BulletShootSounds;

        public BulletObject(int bullet_number){
            BulletBody = new BulletBodyObject(BULLET_MODEL_SIZE);
            BulletHead = new BulletHeadObject(BULLET_MODEL_SIZE);

            ImpactSphereRadius = BULLET_MODEL_SIZE * 0.5f;
            ImpactSphere = new BoundingSphere(new Vector3(0f, 0f, 0f), ImpactSphereRadius);
        
            ObstacleHitSound = GroundBulletSounds[bullet_number % GROUND_BULLET_SOUNDS_QUANTITY];
            EnemyHitSound = MetalBulletSounds[bullet_number % METAL_BULLET_SOUNDS_QUANTITY];
            ShootSound = BulletShootSounds[bullet_number % BULLET_SHOOT_SOUNDS_QUANTITY];
        }

        public new void Initialize(CarObject[] enemies) {
            base.Initialize(enemies);
            BulletBody.Initialize();
            BulletHead.Initialize();
        }
        
        public static void Load(){
            BulletBodyObject.Load("BasicShader");
            BulletHeadObject.Load("BasicShader");
            GroundBulletSounds = new SoundEffect[GROUND_BULLET_SOUNDS_QUANTITY] {
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_ground_1"),
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_ground_4"),
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_ground_3"),
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_ground_5"),
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_ground_2"),
            };
            MetalBulletSounds = new SoundEffect[METAL_BULLET_SOUNDS_QUANTITY] {
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_metal_1"),
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_metal_4"),
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_metal_2"),
                MyContentManager.SoundEffects.Load("bullet/bullet_hit_metal_3"),
            };
            BulletShootSounds = new SoundEffect[BULLET_SHOOT_SOUNDS_QUANTITY] {
                MyContentManager.SoundEffects.Load("bullet/bullet_shoot_2"),
                MyContentManager.SoundEffects.Load("bullet/bullet_shoot_3"),
                MyContentManager.SoundEffects.Load("bullet/bullet_shoot_4"),
            };
        }

        public override void Update(){
            if(IsActive){
                Position = Position - BULLET_SPEED * Forward * TGCGame.GetElapsedTime();

                BulletBody.Update(Position, Forward, RotationMatrix);
                BulletHead.Update(Position, Forward, RotationMatrix);
                
                ImpactSphere = new BoundingSphere(Position, ImpactSphereRadius);

                ActiveTime += TGCGame.GetElapsedTime();

                IsActive = ActiveTime < MAX_ACTIVE_TIME;

                // Chequeo si colisionó con el auto
                for(int i = 0;i < TGCGame.PLAYERS_QUANTITY - 1;i++){
                    if(Enemies[i].ObjectBox.Intersects(ImpactSphere)){
                        // Si colisionó con el auto, el auto recibe daño de bala
                        IsActive = false;
                        Enemies[i].TakeDamage(BULLET_DAMAGE);
                        EnemyHitSound.CreateInstance().Play();
                        return;
                    }
                }
            }
        }

        public override void Draw(Matrix view, Matrix projection){
            if(IsActive){
                BulletBody.Draw(view, projection);
                BulletHead.Draw(view, projection);
            }
        }
    }
}