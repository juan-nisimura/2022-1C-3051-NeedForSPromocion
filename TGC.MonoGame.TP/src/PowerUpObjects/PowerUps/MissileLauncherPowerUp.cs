using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUps
{
    public class MissileLauncherPowerUp : PowerUp
    {
        private bool HasBeenTriggered = false;
        public override bool CanBeTriggered() {
            return !HasBeenTriggered;
        }

        public override void TriggerEffect(CarObject car) {
            if(CanBeTriggered())
            {
                var MissilePosicion = car.Position;
                var MissileRotation = car.Rotation;
                car.ShootMissile();
                HasBeenTriggered = true;
                UpdateCarPowerUp(car);
            } 
        }
        public override void Update(CarObject car) { }
        public override void StopTriggerEffect(CarObject car) { }
    }
}