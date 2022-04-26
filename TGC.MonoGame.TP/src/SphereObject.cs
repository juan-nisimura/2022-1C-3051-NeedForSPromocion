using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src   
{
    class SphereObject : DefaultObject
    {
        protected SpherePrimitive SpherePrimitive { get; set; }
        public SphereObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color){
            SpherePrimitive = new SpherePrimitive(graphicsDevice);
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            DiffuseColor = color.ToVector3();
        }

        public new void Initialize(){
            base.Initialize();
            World *= ScaleMatrix * TranslateMatrix;
        }
        public new void Load(ContentManager content){
            // Cargo efecto
            Effect = content.Load<Effect>(ContentFolderEffects + ShaderDirectory);
        }

        public override void Update(GameTime gameTime){   
        }

        public new void Draw(Matrix view, Matrix projection){
            Effect.Parameters["World"].SetValue(World);
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);
            Effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
            SpherePrimitive.Draw(Effect);
        }
    }
}
