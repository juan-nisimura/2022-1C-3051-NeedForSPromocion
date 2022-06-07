using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using TGC.Monogame.TP.Src.CompoundObjects.SpeedBoost;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels
{
    public class SpeedBoostPowerUpModel : PowerUpModel
    {
        protected SpeedBoostBodyObject SpeedBoostBody { get; set; }
        protected SpeedBoostHeadObject SpeedBoostHead { get; set; }
        public const float BULLET_MODEL_SIZE = 1f;
        public static PowerUpModel PowerUpModel = new SpeedBoostPowerUpModel(Vector3.Zero);
        public static new PowerUpModel GetModel() { return PowerUpModel; }

        public SpeedBoostPowerUpModel(Vector3 position){
            SpeedBoostBody = new SpeedBoostBodyObject(BULLET_MODEL_SIZE);
            SpeedBoostHead = new SpeedBoostHeadObject(BULLET_MODEL_SIZE);
            RotationMatrix = Matrix.CreateRotationX(-MathF.PI / 5);
            Position = position;
            SpeedBoostBody.Initialize();
            SpeedBoostHead.Initialize();
        }

        public static void Load(ContentManager content){
            SpeedBoostBodyObject.Load(content, "BasicShader");
            SpeedBoostHeadObject.Load(content, "BasicShader");
        }
        
        public override void Update(){
            RotationMatrix *= Matrix.CreateRotationY(ROTATION_SPEED * TGCGame.GetElapsedTime());
            var forward = Vector3.Normalize(RotationMatrix.Forward);

            SpeedBoostBody.Update(Position, forward, RotationMatrix);
            SpeedBoostHead.Update(Position, forward, RotationMatrix);
        }

        public override void Draw(Matrix view, Matrix projection){
            SpeedBoostBody.Draw(view, projection);
            SpeedBoostHead.Draw(view, projection);
        }
    }
}