using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bullet
{
    public class BulletHeadObject : SphereObject <BulletHeadObject>
    {
        private const float BULLET_HEAD_FORWARD_DISTANCE = 10f;
        private const float BULLET_DISTANCE_FROM_FLOOR = 10f;

        public BulletHeadObject() :
            base(new Vector3(0f, BulletObject.BULLET_MODEL_SIZE, BulletObject.BULLET_MODEL_SIZE/2), new Vector3(0.5f, 1f, 0.5f) * BulletObject.BULLET_MODEL_SIZE, 0f, Color.Gold){
        }
        public void Update(Vector3 position, Vector3 forward, Matrix rotationMatrix){
            position = new Vector3(position.X, position.Y + BULLET_DISTANCE_FROM_FLOOR, position.Z) - BULLET_HEAD_FORWARD_DISTANCE * BulletObject.BULLET_MODEL_SIZE * forward;
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= rotationMatrix;
            World *= Matrix.CreateTranslation(position);
        }
    }
}
