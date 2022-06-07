using TGC.Monogame.TP.Src.HUD;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUps
{
    public abstract class PowerUp
    {
        public abstract bool CanBeTriggered();
        public abstract void TriggerEffect(CarObject car);
        public void Draw(){

        }

        public void UpdateHUD(){
            if(!CanBeTriggered())    
                PowerUpHUDCircleObject.SetPowerUpModel(new NullPowerUpModel());
        }
    }
}