using System;
using Microsoft.Xna.Framework;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels
{
    public abstract class PowerUpModel
    {
        protected const float ROTATION_SPEED = 1f;
        public Vector3 Position;
        protected Matrix RotationMatrix;

        public abstract void Initialize();
        
        public abstract void Update();

        public abstract void Draw(Matrix view, Matrix projection);
    }
}