using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUps
{
    public class SpeedBoostPowerUp : PowerUp
    {
        private bool HasBeenTriggered = false;
        public override bool CanBeTriggered() {
            return !HasBeenTriggered;
        }

        public override void TriggerEffect(CarObject car) {
            if(CanBeTriggered()){
                car.SetSpeedBoostTime();
                HasBeenTriggered = true;
                UpdateCarPowerUp(car);
            }
        }
        public override void Update(CarObject car) { }
        public override void StopTriggerEffect(CarObject car) { }
    }
}