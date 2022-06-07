using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUps
{
    public class MachineGunPowerUp : PowerUp
    {
        private const float SHOOT_DELAY = 0.2f;
        private float LastShotTime = 0f;
        private float TimeLeft = 5f;
        public override bool CanBeTriggered() {
            return TimeLeft >= 0f;
        }

        public override void TriggerEffect(CarObject car) {
            if(CanBeTriggered()){
                if (LastShotTime >= SHOOT_DELAY) {
                    var BulletPosicion = car.Position;
                    var BulletRotation = car.Rotation;
                    car.ShootBullet();
                    LastShotTime = 0f;
                }
                LastShotTime += TGCGame.GetElapsedTime();
                TimeLeft -= TGCGame.GetElapsedTime();
            }
        }
    }
}