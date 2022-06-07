using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.PowerUps
{
    public class NullPowerUp : PowerUp
    {
        public override bool CanBeTriggered() {
            return false;
        }

        public override void TriggerEffect(CarObject car) {

        }
    }
}