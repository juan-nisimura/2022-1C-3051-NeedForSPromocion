using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.Screens;
using TGC.MonoGame.TP;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.MonoGame.TP.Src.Geometries.Textures;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    public class QuadObject <T> : DefaultPrimitiveObject <T>
    {
        protected QuadPrimitive QuadPrimitive;
        protected BoundingBox BoundingBox;

        public QuadObject(Vector3 position, Vector3 size, float rotation, Color color){
            QuadPrimitive = new QuadPrimitive(TGCGame.GetGraphicsDevice());
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.CreateRotationY(rotation);
            DiffuseColor = color.ToVector3();
            BoundingBox = new BoundingBox(position - size, position + size);
        }

        protected override bool IsVisible() 
        {
            return LevelScreen.GetBoundingFrustum().Intersects(BoundingBox);
        }

        protected override void DrawPrimitive(Effect effect) { QuadPrimitive.Draw(effect); }
    }
}
