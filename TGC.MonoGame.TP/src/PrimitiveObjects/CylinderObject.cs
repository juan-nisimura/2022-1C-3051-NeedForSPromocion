using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class CylinderObject<T> : DefaultPrimitiveObject <T>
    {
        protected CylinderPrimitive CylinderPrimitive { get; }
        public CylinderObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation, Color color){
            CylinderPrimitive = new CylinderPrimitive(graphicsDevice);
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.CreateRotationY(rotation);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive() { CylinderPrimitive.Draw(getEffect()); }
    }
}
