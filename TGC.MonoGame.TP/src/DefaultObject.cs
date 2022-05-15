using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.Monogame.TP.Src   
{
    abstract class DefaultObject <T>
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";
        protected static Dictionary<Type, Effect> Effects = new Dictionary<Type, Effect>(); 
        protected static Dictionary<Type, Texture> Textures = new Dictionary<Type, Texture>();
        
        protected Matrix World;
        protected Matrix ScaleMatrix;
        protected Matrix TranslateMatrix;
        protected Matrix RotationMatrix;

        protected Vector3 DiffuseColor;
        public static int ObjectCount { get; set; } = 0;

        public static Effect getEffect(){
            return Effects[typeof(T)];
        }

        public static Texture getTexture(){
            return Textures[typeof(T)];
        }

        public void Initialize(){
            this.ResetWorld();
            ObjectCount++; 
        }

        public DefaultObject<T> TransformWorld(Matrix transform){
            World *= transform;
            return this;
        }

        public DefaultObject<T> ResetWorld(){
            World = Matrix.Identity;
            return this;
        }
        abstract public void Update(GameTime gameTime);
        public abstract void Draw(Matrix view, Matrix projection);
    }
}