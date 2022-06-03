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

        public static void Load(ContentManager content, string shaderDirectory, string textureDirectory){
            // Cargo efecto
            Load(content, shaderDirectory);
            
            // Cargo la textura
            Textures.Add(typeof(T), content.Load<Texture>(ContentFolderTextures + textureDirectory));
        }

        public override void Update(GameTime gameTime){   
        }

        public override void Draw(Matrix view, Matrix projection){
            Effects[typeof(T)].Parameters["World"]?.SetValue(World);
            Effects[typeof(T)].Parameters["View"]?.SetValue(view);
            Effects[typeof(T)].Parameters["Projection"]?.SetValue(projection);
            Effects[typeof(T)].Parameters["DiffuseColor"]?.SetValue(DiffuseColor);
            Effects[typeof(T)].Parameters["Texture"]?.SetValue(getTexture());
            DrawPrimitive();
        }
        /*public void DrawBlinnPhong(Effect effect, Matrix view, Matrix projection)
        {
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;
            effect.Parameters["baseTexture"]?.SetValue(getTexture());
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(World)));
            effect.Parameters["WorldViewProjection"].SetValue(World * view * projection);
            DrawPrimitive();
        }*/

        protected abstract void DrawPrimitive();
    }
}
