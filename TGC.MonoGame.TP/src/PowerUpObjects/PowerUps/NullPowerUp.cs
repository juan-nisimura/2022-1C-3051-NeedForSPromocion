using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUps
{
    public class NullPowerUp : PowerUp
    {
        private static SoundEffect CarHornSound;
        public override bool CanBeTriggered() {
            return false;
        }
        public override void Update(CarObject car) { }
        public override void StopTriggerEffect(CarObject car) { }

        public static void Load(ContentManager content){
            CarHornSound = content.Load<SoundEffect>(TGCGame.ContentFolderSounds + "car horn");
        }

        public override void TriggerEffect(CarObject car) {
            CarHornSound.CreateInstance().Play();
        }
    }
}