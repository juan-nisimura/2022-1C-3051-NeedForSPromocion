using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Missile;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.PowerUps
{
    public class MissileLauncherPowerUp : PowerUp
    {
        private bool HasBeenTriggered = false;
        public override bool CanBeTriggered() {
            return !HasBeenTriggered;
        }

        public override void TriggerEffect(GraphicsDevice graphicsDevice, CarObject car) {
            if(CanBeTriggered())
            {
                var MissilePosicion = car.Position;
                var MissileRotation = car.Rotation;
                car.ShootMissile();
                HasBeenTriggered = true;
            } 
        }
    }
}