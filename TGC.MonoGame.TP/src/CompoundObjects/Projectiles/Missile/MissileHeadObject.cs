using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile
{
    public class MissileHeadObject : SphereObject <MissileHeadObject>
    {
        // private const float MISSILE_HEAD_FORWARD_DISTANCE = 3f;
        private float ModelSize;
        private bool Visible = true;
        public void SetIsVisible(bool visible){ this.Visible = visible; }
        protected override bool IsVisible() { return Visible; }
        public MissileHeadObject(float modelSize) :
            base(new Vector3(0f, 0f, 0f), new Vector3(0.5f, 1, 0.5f) * modelSize, 0f, Color.Red)
        {
            ModelSize = modelSize;
        }
        public void Update(Vector3 position, Vector3 forward, Matrix rotationMatrix)
        {
            position = new Vector3(position.X, position.Y, position.Z) - ModelSize * forward / 2;
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= rotationMatrix;
            World *= Matrix.CreateTranslation(position);
        }
    }
}