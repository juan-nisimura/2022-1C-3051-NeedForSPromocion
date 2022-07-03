using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile
{
    public class MissileBodyObject : CylinderObject <MissileBodyObject>
    {
        private bool Visible = true;
        public void SetIsVisible(bool visible){ this.Visible = visible; }
        protected override bool IsVisible() { return Visible; }
        public MissileBodyObject(float modelSize)
            : base(new Vector3(0f, 0f, 0f), new Vector3(modelSize/2, modelSize, modelSize/2), MathHelper.PiOver2, 0f, Color.Gray)
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
