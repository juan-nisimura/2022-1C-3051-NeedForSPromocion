using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.PowerUps
{
    public abstract class PowerUp
    {
        public abstract bool CanBeTriggered();
        public abstract void TriggerEffect(CarObject car);
        public void Draw(){

        }
    }
}