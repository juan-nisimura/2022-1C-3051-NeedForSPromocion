using Microsoft.Xna.Framework;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels
{
    public class NullPowerUpModel : PowerUpModel
    {
        public static PowerUpModel PowerUpModel = new NullPowerUpModel();
        public static new PowerUpModel GetModel() { return PowerUpModel; }
        public NullPowerUpModel(){
        }
        
        public override void Update(){
        
        }

        public override void Draw(Matrix view, Matrix projection){
            
        }
    }
}