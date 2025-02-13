using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.Screens;
using TGC.MonoGame.TP;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    public abstract class TriangleObject<T> : DefaultPrimitiveObject <T>
    {
        protected TrianglePrimitive TrianglePrimitive { get; }
        public TriangleObject(Vector3 position, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color color){
            TrianglePrimitive = new TrianglePrimitive(TGCGame.GetGraphicsDevice(), vertex1, vertex2, vertex3, color);
            ScaleMatrix = Matrix.Identity;
            RotationMatrix = Matrix.Identity;
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }

        protected override void DrawPrimitive(Effect effect) { TrianglePrimitive.Draw(effect); }
    }
}
