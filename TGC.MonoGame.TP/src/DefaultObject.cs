using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.Monogame.TP.Src   
{
    public abstract class DefaultObject <T>
    {
        public const float GRAVITY = 400f;
        protected static Dictionary<Type, Effect> Effects = new Dictionary<Type, Effect>(); 
        protected static Dictionary<Type, Texture> Textures = new Dictionary<Type, Texture>();
        
        protected Matrix World;
        protected Matrix ScaleMatrix;
        protected Matrix TranslateMatrix;
        protected Matrix RotationMatrix;

        protected Vector3 DiffuseColor;
        public static int ObjectCount { get; set; } = 0;

        abstract protected bool IsVisible();

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

        public float Lerp(float v1, float v2, float factorDeInterpolacion) {
            return v1 + (v2 - v1) * factorDeInterpolacion;
        }

        public Matrix GetWorld(){
            return World;
        }
        abstract public void Update();
        public abstract void Draw(Matrix view, Matrix projection);
    }
}