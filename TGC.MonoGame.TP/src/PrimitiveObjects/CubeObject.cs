using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Src.Geometries.Textures;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class CubeObject <T> : DefaultPrimitiveObject <T>
    {
        protected BoxPrimitive BoxPrimitive { get; }
        public CubeObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color){
            BoxPrimitive = new BoxPrimitive(graphicsDevice, Vector3.One, (Texture2D) getTexture());
            ScaleMatrix = Matrix.CreateScale(size);
            RotationMatrix = Matrix.Identity;
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive() { BoxPrimitive.Draw(getEffect()); }
    }
}
