using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.MyContentManagers;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels
{
    public class BulletPowerUpModel : PowerUpModel
    {
        protected BulletBodyObject BulletBody { get; set; }
        protected BulletHeadObject BulletHead { get; set; }
        public const float BULLET_MODEL_SIZE = 1f;
        public static PowerUpModel PowerUpModel = new BulletPowerUpModel(Vector3.Zero);
        public static new PowerUpModel GetModel() { 
            PowerUpModel.SetTime(0);
            return PowerUpModel; 
        }

        public BulletPowerUpModel(Vector3 position){
            BulletBody = new BulletBodyObject(BULLET_MODEL_SIZE);
            BulletHead = new BulletHeadObject(BULLET_MODEL_SIZE);
            RotationMatrix = Matrix.CreateRotationX(-MathF.PI / 15);
            Position = position;
            BulletBody.Initialize();
            BulletHead.Initialize();
        }
        
        public override void Update(){
            RotationMatrix *= Matrix.CreateRotationY(ROTATION_SPEED * TGCGame.GetElapsedTime());
            var forward = Vector3.Normalize(RotationMatrix.Forward);
            BulletBody.Update(Position, forward, RotationMatrix);
            BulletHead.Update(Position, forward, RotationMatrix);
            Time += TGCGame.GetElapsedTime();
        }

        public override void Draw(Matrix view, Matrix projection){
            var effect = MyContentManager.Effects.Get("PowerUpModelShader");
            effect.Parameters["Time"]?.SetValue(Time);
            effect.Parameters["Center"]?.SetValue(Position);
            BulletBody.Draw(view, projection, effect);
            BulletHead.Draw(view, projection, effect);
        }
    }
}