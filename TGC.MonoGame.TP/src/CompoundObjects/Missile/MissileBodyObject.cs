using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class MissileBodyObject : CylinderObject <MissileBodyObject>
    {
        private Vector3 forward {get;set;}
        private float counter {get;set;}=-1f;
        public MissileBodyObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotationX, float rotationY, Color color)
            : base(graphicsDevice, position, size, rotationX, rotationY, color){

        }
        public void Update(GameTime gameTime, Vector3 Position, float Rotation, float size)
        {
            forward += World.Forward * counter;
            counter +=  2000000f;
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Position = new Vector3(Position.X+forward.X, Position.Y+5, Position.Z+forward.Z);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationY(Rotation);
            
            World *= Matrix.CreateTranslation(new Vector3(Position.X, Position.Y+5, Position.Z));
        }

    }
}
