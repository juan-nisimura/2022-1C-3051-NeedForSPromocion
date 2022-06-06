using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.HUD
{
    public class PowerUpHUDCircleObject : QuadObject <PowerUpHUDCircleObject>
    {
        public PowerUpHUDCircleObject()
         : base(new Vector3(0f, 0f, 0f), new Vector3(10f, 0f, 10f), 0, Color.Gray){
            var cameraUpVector = Vector3.Normalize(new Vector3(1f, 1.5f, 1f));
            var upVector = Vector3.UnitY;
            var angulo = MathF.Acos(Convert.ToSingle(Vector3.Dot(cameraUpVector, upVector)));
            RotationMatrix = Matrix.CreateRotationX(-angulo) * Matrix.CreateRotationY(MathF.PI / 4);
        }

        public void Update(PlayerCarObject car) {
            TranslateMatrix = Matrix.CreateTranslation(car.GetPosition() + new Vector3(-100f, 50f, -20f));
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;  
        }

        public override void Draw(Matrix view, Matrix projection){
            getEffect().Parameters["World"].SetValue(World);
            getEffect().Parameters["View"].SetValue(view);
            getEffect().Parameters["Projection"].SetValue(projection);
            getEffect().Parameters["Texture"]?.SetValue(getTexture());
            getEffect().Parameters["HealthPercentage"]?.SetValue(1f);
            DrawPrimitive();
        }
    }
}