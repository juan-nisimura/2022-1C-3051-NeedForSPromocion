using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    abstract class DefaultObject 
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        protected Model Model;
        protected Effect Effect;
        protected Matrix World;
        // protected Matrix View;
        // protected Matrix Projection;
        protected String ModelDirectory = "RacingCarA/RacingCar";
        protected String ShaderDirectory = "BasicShader";
        public void Initialize(){
            this.ResetWorld();
        }
        public void Load(ContentManager content){

            // Cargo el modelo
            Model = content.Load<Model>(ContentFolder3D + ModelDirectory);

            // Cargo efecto
            Effect = content.Load<Effect>(ContentFolderEffects + ShaderDirectory);

            // Asigno el efecto que cargue a cada parte del mesh.
            foreach (var mesh in Model.Meshes)
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = Effect;
        }

/*
        public DefaultObject SetView(Matrix view){
            View = view;
            return this;
        }

        public DefaultObject SetProjection(Matrix projection){
            Projection = projection;
            return this;
        }
*/
        public DefaultObject TransformWorld(Matrix transform){
            World *= transform;
            return this;
        }

        public DefaultObject ResetWorld(){
            World = Matrix.Identity;
            return this;
        }
        abstract public void Update(GameTime gameTime);
        public void Draw(Matrix view, Matrix projection){

            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            Effect.Parameters["View"].SetValue(view);
            Effect.Parameters["Projection"].SetValue(projection);

            foreach (var mesh in Model.Meshes)
            {
                var meshWorld = mesh.ParentBone.Transform * World;
                Effect.Parameters["World"].SetValue(meshWorld);
                mesh.Draw();
            }
        }
    }
}