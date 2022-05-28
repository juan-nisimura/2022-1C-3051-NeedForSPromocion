using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class BulletHeadObject : SphereObject <MissileHeadObject>
    {
        private Vector3 forward {get;set;}
        private float counter {get;set;}=0;
        public BulletHeadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color) :
            base(graphicsDevice, position, size, color){
        }
        public BulletHeadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotationY, Color color) :
            base(graphicsDevice, position, size, rotationY,color){
        }
        public void Update(GameTime gameTime, Vector3 Position, float Rotation, float Speed){
            forward += World.Forward * -Speed / 1500;

            forward += World.Forward * counter;
            counter += Math.Max(50000000f, 0) ;

            //var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Position = new Vector3(Position.X+forward.X, Position.Y+5, Position.Z+forward.Z);
            World = ScaleMatrix;
            //World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationY(Rotation);

            
            World *= Matrix.CreateTranslation(new Vector3(Position.X, Position.Y+5, Position.Z));
        }
    }
}
