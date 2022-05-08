using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class SphereObject : DefaultPrimitiveObject <SphereObject>
    {
        protected SpherePrimitive SpherePrimitive { get; }
        public SphereObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color){
            SpherePrimitive = new SpherePrimitive(graphicsDevice);
            ScaleMatrix = Matrix.CreateScale(size);
            RotationMatrix = Matrix.Identity;
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive() { SpherePrimitive.Draw(getEffect()); }
    }
}
