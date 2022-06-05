using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    public class MissileHeadObject : SphereObject <MissileHeadObject>
    {
        private const float MISSILE_HEAD_FORWARD_DISTANCE = 100f;
        private const float MISSILE_DISTANCE_FROM_FLOOR = 10f;
        public MissileHeadObject(GraphicsDevice graphicsDevice) :
            base(graphicsDevice, new Vector3(0f, MissileObject.MISSILE_MODEL_SIZE, MissileObject.MISSILE_MODEL_SIZE/2), new Vector3(0.5f, 1, 0.5f) * MissileObject.MISSILE_MODEL_SIZE, 0f, Color.Red)
        {
        }
        public void Update(Vector3 position, Vector3 forward, Matrix rotationMatrix)
        {
            position = new Vector3(position.X, position.Y + MISSILE_DISTANCE_FROM_FLOOR, position.Z) - MISSILE_HEAD_FORWARD_DISTANCE * forward;
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= rotationMatrix;
            World *= Matrix.CreateTranslation(position);
        }
    }
}