using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class TriangleObject : DefaultPrimitiveObject <TriangleObject>
    {
        protected TrianglePrimitive TrianglePrimitive { get; }
        public TriangleObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color color){
            TrianglePrimitive = new TrianglePrimitive(graphicsDevice, vertex1, vertex2, vertex3, color);
            ScaleMatrix = Matrix.Identity;
            RotationMatrix = Matrix.Identity;
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive() { TrianglePrimitive.Draw(getEffect()); }
    }
}
