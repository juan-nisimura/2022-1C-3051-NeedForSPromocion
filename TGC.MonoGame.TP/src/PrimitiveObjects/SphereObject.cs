using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    public class SphereObject<T> : DefaultPrimitiveObject <T>
    {
        protected SpherePrimitive SpherePrimitive { get; }
        public SphereObject(Vector3 position, Vector3 size,Color color){
            SpherePrimitive = new SpherePrimitive(TGCGame.GetGraphicsDevice(), 1, 16, color);
            ScaleMatrix = Matrix.CreateScale(size);
            RotationMatrix = Matrix.Identity;
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }
        public SphereObject(Vector3 position, Vector3 size, float rotationY,Color color){
            SpherePrimitive = new SpherePrimitive(TGCGame.GetGraphicsDevice(), 1, 16, color);
            ScaleMatrix = Matrix.CreateScale(size);
            RotationMatrix = Matrix.CreateRotationY(rotationY);
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive(Effect effect) { SpherePrimitive.Draw(effect); }
    }
}
