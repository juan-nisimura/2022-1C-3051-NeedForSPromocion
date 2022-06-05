using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    public abstract class DefaultPrimitiveObject <T> : DefaultObject <T>
    {
        public new void Initialize(){
            base.Initialize();
            World *= ScaleMatrix * RotationMatrix * TranslateMatrix;
        }
        public static void Load(ContentManager content, string shaderDirectory){
            // Cargo efecto
            Effects.Add(typeof(T), content.Load<Effect>(ContentFolderEffects + shaderDirectory));
        }

        public static void Load(ContentManager content, string shaderDirectory, string textureDirectory){
            // Cargo efecto
            Load(content, shaderDirectory);
            
            // Cargo la textura
            Textures.Add(typeof(T), content.Load<Texture>(ContentFolderTextures + textureDirectory));
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
