using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.ModelObjects
{
    public class HealthBarObject : QuadObject <HealthBarObject>
    {
        private float HealthPercentage = 1;
        public HealthBarObject(GraphicsDevice graphicsDevice)
         : base(graphicsDevice, new Vector3(0f, 0f, 0f), new Vector3(15f, 0f, 1f), 0, Color.Green){
            var cameraUpVector = Vector3.Normalize(new Vector3(1f, 1.5f, 1f));
            var upVector = Vector3.UnitY;

            var angulo = MathF.Acos(Convert.ToSingle(Vector3.Dot(cameraUpVector, upVector)));
            RotationMatrix = Matrix.CreateRotationX(-angulo) * Matrix.CreateRotationY(MathF.PI / 4);
        }

        public void Update(GameTime gameTime, CarObject car){
            
            // Chequeo si colisionó con el auto
            HealthPercentage = car.Health / CarObject.MAX_HEALTH;

            TranslateMatrix = Matrix.CreateTranslation(car.Position + new Vector3(0f, 20f, 0f));
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;  
        }

        public override void Draw(Matrix view, Matrix projection){
            getEffect().Parameters["World"].SetValue(World);
            getEffect().Parameters["View"].SetValue(view);
            //getEffect().Parameters["View"].SetValue(Matrix.Identity);
            getEffect().Parameters["Projection"].SetValue(projection);
            //getEffect().Parameters["Projection"].SetValue(Matrix.Identity);
            //getEffect().Parameters["DiffuseColor"]?.SetValue(DiffuseColor);
            getEffect().Parameters["Texture"]?.SetValue(getTexture());
            getEffect().Parameters["HealthPercentage"]?.SetValue(HealthPercentage);
            DrawPrimitive();
        }
    }
}
