using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class RampObject : DefaultPrimitiveObject <RampObject>
    {
        protected RampPrimitive RampPrimitive { get; }

        public RampObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation, Color color){
            RampPrimitive = new RampPrimitive(graphicsDevice);
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.CreateRotationY(rotation);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive() { RampPrimitive.Draw(getEffect()); }
    }
}
