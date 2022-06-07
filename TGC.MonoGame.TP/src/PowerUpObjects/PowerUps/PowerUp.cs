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

        public abstract void Update(CarObject car);

        public abstract void StopTriggerEffect(CarObject car);

        public void UpdateCarPowerUp(CarObject car){
            if(!CanBeTriggered()){
                car.SetPowerUp(new NullPowerUp());
                PowerUpHUDCircleObject.SetPowerUpModel(new NullPowerUpModel());
            }
        }
    }
}