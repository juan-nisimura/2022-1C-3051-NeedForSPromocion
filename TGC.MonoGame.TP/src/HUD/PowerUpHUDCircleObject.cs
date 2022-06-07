using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.HUD
{
    public class PowerUpHUDCircleObject : QuadObject <PowerUpHUDCircleObject>
    {
        private Vector3 CameraLookAtVector;
        private Vector3 CameraRelativePosition;
        private Vector3 HUDCircleRelativePosition = new Vector3(2f,0f,-10f);
        public static PowerUpModel PowerUpModel = new NullPowerUpModel();

        public PowerUpHUDCircleObject()
         : base(new Vector3(0f, 0f, 0f), new Vector3(2f, 0f, 2f), 0, Color.Gray){
            var cameraUpVector = Vector3.Normalize(new Vector3(1f, 1.5f, 1f));
            CameraLookAtVector = Vector3.Normalize(new Vector3(1f, -1.5f, 1f));
            CameraRelativePosition = new Vector3(-100f, 150f, -100f);
            var upVector = Vector3.UnitY;
            var angulo = MathF.Acos(Convert.ToSingle(Vector3.Dot(cameraUpVector, upVector)));
            RotationMatrix = Matrix.CreateRotationX(-angulo) * Matrix.CreateRotationY(MathF.PI / 4);
        }

        public void Update(PlayerCarObject car) {
            var position = car.GetPosition() + CameraRelativePosition + CameraLookAtVector * 20 + HUDCircleRelativePosition;
            TranslateMatrix = Matrix.CreateTranslation(position);
            PowerUpModel.Position = position;
            PowerUpModel.Update();
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;
        }

        public static void SetPowerUpModel(PowerUpModel powerUpModel) {
            PowerUpModel = powerUpModel;
        }

        public override void Draw(Matrix view, Matrix projection){      
            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.DepthRead;
            TGCGame.GetGraphicsDevice().BlendState = BlendState.AlphaBlend;
            getEffect().Parameters["World"].SetValue(World);
            getEffect().Parameters["View"].SetValue(view);
            getEffect().Parameters["Projection"].SetValue(projection);
            getEffect().Parameters["Texture"]?.SetValue(getTexture());
            DrawPrimitive();
            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.Default;
            TGCGame.GetGraphicsDevice().BlendState = BlendState.Opaque;
            PowerUpModel.Draw(view, projection);
        }
    }
}