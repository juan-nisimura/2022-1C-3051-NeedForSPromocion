using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class CubeObject <T> : DefaultPrimitiveObject <T>
    {
        protected CubePrimitive CubePrimitive { get; }
        public CubeObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color){
            CubePrimitive = new CubePrimitive(graphicsDevice);
            ScaleMatrix = Matrix.CreateScale(size);
            RotationMatrix = Matrix.Identity;
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive() { CubePrimitive.Draw(getEffect()); }
    }
}
