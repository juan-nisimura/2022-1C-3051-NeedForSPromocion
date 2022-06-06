using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    public class MissileTriangleObject : TriangleObject <MissileTriangleObject>
    {
        private const float MISSILE_TRIANGLE_FORWARD_DISTANCE = 5f;
        private const float MISSILE_DISTANCE_FROM_FLOOR = 10f;
        public MissileTriangleObject(Vector3 position, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color color)
            : base(position, vertex1, vertex2, vertex3, color){
        }

        public void Update(Vector3 position, Vector3 forward, Matrix rotationMatrix)
        {
            position = new Vector3(position.X, position.Y + MISSILE_DISTANCE_FROM_FLOOR, position.Z) + MISSILE_TRIANGLE_FORWARD_DISTANCE * forward;
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= rotationMatrix;
            World *= Matrix.CreateTranslation(position);
        }
    }
}
