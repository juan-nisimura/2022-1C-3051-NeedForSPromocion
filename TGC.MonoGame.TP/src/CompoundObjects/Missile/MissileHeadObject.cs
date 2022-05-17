using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class MissileHeadObject : SphereObject <MissileHeadObject>
    {
        private Vector3 forward {get;set;}
        private float counter {get;set;}=0;
        public MissileHeadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color) :
            base(graphicsDevice, position, size, color){
        }
        public MissileHeadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotationY, Color color) :
            base(graphicsDevice, position, size, rotationY,color){
        }
        public void Update(GameTime gameTime, Vector3 Position, float Rotation){
            forward += World.Forward * counter * counter;
            counter+=500f;
            //var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Position = new Vector3(Position.X+forward.X, 10, Position.Z+forward.Z);
            World = ScaleMatrix;
            //World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationY(Rotation);

            
            World *= Matrix.CreateTranslation(new Vector3(Position.X, 10, Position.Z));
        }
    }
}
