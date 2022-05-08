using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.Monogame.TP.Src.PrimitiveObjects  
{
    class WheelObject : CylinderObject
    {
        public WheelObject(GraphicsDevice graphicsDevice, Vector3 position)
            : base (graphicsDevice, position, new Vector3(1f,1f,1f), 0, Color.White){
            ScaleMatrix = Matrix.CreateScale(90f, 20f, 90f);
            RotationMatrix = Matrix.CreateRotationZ(MathF.PI/2);
        }

        public new void Initialize(){
            base.Initialize();
        }

        public override void Update(GameTime gameTime){
        }

        public void FollowCar(Matrix carWorld, float turningSpeed){
            World = ScaleMatrix * RotationMatrix * Matrix.CreateRotationY(turningSpeed / 11f) * TranslateMatrix * carWorld;
        }
    }
}