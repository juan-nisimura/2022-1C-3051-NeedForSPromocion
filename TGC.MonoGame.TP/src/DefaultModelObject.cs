using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TGC.Monogame.TP.Src   
{
    public abstract class DefaultModelObject <T> : DefaultObject <T>
    {

        protected static Dictionary<Type, Model> Models = new Dictionary<Type, Model>();

        public static void DefaultLoad(ContentManager content, string modelDirectory, string shaderDirectory){

            // Cargo el modelo
            Models.Add(typeof(T), content.Load<Model>(ContentFolder3D + modelDirectory));

            // Cargo la textura
            Textures.Add(typeof(T), ((BasicEffect) getModel().Meshes.FirstOrDefault()?.MeshParts.FirstOrDefault()?.Effect)?.Texture);

            // Cargo efecto
            Effects.Add(typeof(T), content.Load<Effect>(ContentFolderEffects + shaderDirectory));

            // Asigno el efecto que cargue a cada parte del mesh.
            foreach (var mesh in getModel().Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = getEffect();
        }

        public static Model getModel() {
            return Models[typeof(T)];
        }

        public override void Draw(Matrix view, Matrix projection){

            Texture texture = getTexture();

            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            getEffect().Parameters["View"].SetValue(view);
            getEffect().Parameters["Projection"].SetValue(projection);
            getEffect().Parameters["ModelTexture"].SetValue(texture);

            foreach (var mesh in getModel().Meshes)
            {
                var meshWorld = mesh.ParentBone.Transform * World;
                getEffect().Parameters["World"].SetValue(meshWorld);
                mesh.Draw();
            }
        }
    }
}