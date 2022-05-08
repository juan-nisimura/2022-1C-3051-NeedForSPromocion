using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    abstract class DefaultPrimitiveObject <T> : DefaultObject <T>
    {
        public new void Initialize(){
            base.Initialize();
            World *= ScaleMatrix * RotationMatrix * TranslateMatrix;
        }
        public static void Load(ContentManager content, string shaderDirectory){
            // Cargo efecto
            Effects.Add(typeof(T), content.Load<Effect>(ContentFolderEffects + shaderDirectory));
        }

        public override void Update(GameTime gameTime){   
        }

        public override void Draw(Matrix view, Matrix projection){
            Effects[typeof(T)].Parameters["World"].SetValue(World);
            Effects[typeof(T)].Parameters["View"].SetValue(view);
            Effects[typeof(T)].Parameters["Projection"].SetValue(projection);
            Effects[typeof(T)].Parameters["DiffuseColor"].SetValue(DiffuseColor);
            DrawPrimitive();
        }
    
        protected abstract void DrawPrimitive();
    }
}
