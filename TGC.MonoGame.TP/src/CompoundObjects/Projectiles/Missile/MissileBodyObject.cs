using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile
{
    public class MissileBodyObject : CylinderObject <MissileBodyObject>
    {
        public MissileBodyObject()
            : base(new Vector3(0f, 0f, 0f), new Vector3(MissileObject.MISSILE_MODEL_SIZE/2, MissileObject.MISSILE_MODEL_SIZE, MissileObject.MISSILE_MODEL_SIZE/2), MathHelper.PiOver2, 0f, Color.Gray)
        {
        }

        public void Update(Vector3 position, Vector3 forward, Matrix rotationMatrix)
        {
            position = new Vector3(position.X, position.Y, position.Z);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= rotationMatrix;
            World *= Matrix.CreateTranslation(position);
        }
    }
}
