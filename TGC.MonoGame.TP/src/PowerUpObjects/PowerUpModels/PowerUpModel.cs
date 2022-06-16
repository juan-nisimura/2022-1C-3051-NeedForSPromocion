using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.MyContentManagers;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels
{
    public abstract class PowerUpModel
    {
        protected const float ROTATION_SPEED = 1f;
        public Vector3 Position;
        protected Matrix RotationMatrix;
        protected float Time = 0;
        public static Effect Effect;

        public static void Load(){
            Effect = MyContentManager.Effects.Load("PowerUpModelShader");
        }

        public abstract void Update();
        public abstract void Draw(Matrix view, Matrix projection);

        public static PowerUpModel GetModel(){
            return NullPowerUpModel.GetModel();
        }

        public void SetTime(float time) {
            Time = time;
        }
    }
}