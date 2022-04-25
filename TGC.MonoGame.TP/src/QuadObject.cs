using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Src.Geometries.Textures;

namespace TGC.Monogame.TP.Src   
{
    class QuadObject : DefaultObject
    {
        protected QuadPrimitive QuadPrimitive { get; set; }

        public QuadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation, Color color){
            QuadPrimitive = new QuadPrimitive(graphicsDevice);
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.CreateRotationY(rotation);
            DiffuseColor = color.ToVector3();
        }

        public new void Initialize(){
            base.Initialize();
            World *= ScaleMatrix * RotationMatrix * TranslateMatrix;
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
            QuadPrimitive.Draw(Effect);
        }
    }
}
