using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.SpeedBoost
{
    public class SpeedBoostHeadObject : CylinderObject <SpeedBoostHeadObject>
    {
        protected override bool IsVisible() { return true; }
        private const float SPEED_BOOST_HEAD_FORWARD_DISTANCE = 0.80f;

        private float ModelSize;

        public SpeedBoostHeadObject(float modelSize) :
            base(new Vector3(0f, 0f, 0f), new Vector3(1/1.4f, 0.3f, 1/1.4f) * modelSize, 0, 0, Color.Blue){
            ModelSize = modelSize;
        }

        public void Update(Vector3 position, Vector3 forward, Matrix rotationMatrix){
            position = new Vector3(position.X, position.Y, position.Z) - SPEED_BOOST_HEAD_FORWARD_DISTANCE * ModelSize * forward;
            World = ScaleMatrix;
            World *= rotationMatrix;
            World *= Matrix.CreateTranslation(position);
        }
    }
}
