using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.SpeedBoost
{
    public class SpeedBoostBodyObject : CylinderObject <SpeedBoostBodyObject>
    {
        protected override bool IsVisible() { return true; }
        public SpeedBoostBodyObject(float modelSize)
            : base(new Vector3(0f, 0f, 0f), new Vector3(modelSize/1.5f, modelSize, modelSize/1.5f), MathHelper.PiOver2, 0f, Color.Red){
        }
        public void Update(Vector3 position, Vector3 forward, Matrix rotationMatrix){
            position = new Vector3(position.X, position.Y, position.Z);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= rotationMatrix;
            World *= Matrix.CreateTranslation(position);
        }
    }
}