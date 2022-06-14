using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.Monogame.TP.Src.MyContentManagers;

namespace TGC.Monogame.TP.Src   
{
    public abstract class DefaultPrimitiveObject <T> : DefaultObject <T>
    {
        public new void Initialize(){
            base.Initialize();
            World *= ScaleMatrix * RotationMatrix * TranslateMatrix;
        }
        public static void Load(string shaderDirectory){
            // Cargo efecto
            Effects.Add(typeof(T), MyContentManager.Effects.Load(shaderDirectory));
        }

        public static void Load(string shaderDirectory, string textureDirectory){
            // Cargo efecto
            Load(shaderDirectory);
            
            // Cargo la textura
            Textures.Add(typeof(T), MyContentManager.Textures.Load(textureDirectory));
        }

        public override void Update(){   
        }

        public override void Draw(Matrix view, Matrix projection){
            Effects[typeof(T)].Parameters["World"].SetValue(World);
            Effects[typeof(T)].Parameters["View"].SetValue(view);
            Effects[typeof(T)].Parameters["Projection"].SetValue(projection);
            Effects[typeof(T)].Parameters["DiffuseColor"]?.SetValue(DiffuseColor);
            Effects[typeof(T)].Parameters["Texture"]?.SetValue(getTexture());
            DrawPrimitive();
        }
    
        protected abstract void DrawPrimitive();
    }
}
