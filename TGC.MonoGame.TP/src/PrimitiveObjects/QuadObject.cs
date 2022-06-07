using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.MonoGame.TP.Src.Geometries.Textures;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    public class QuadObject <T> : DefaultPrimitiveObject <T>
    {
        protected QuadPrimitive QuadPrimitive;

        public QuadObject(Vector3 position, Vector3 size, float rotation, Color color){
            QuadPrimitive = new QuadPrimitive(TGCGame.GetGraphicsDevice());
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.CreateRotationY(rotation);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive() { QuadPrimitive.Draw(getEffect()); }
    }
}
