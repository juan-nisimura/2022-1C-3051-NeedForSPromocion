using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class MissileBodyObject : CylinderObject <MissileBodyObject>
    {
        private Vector3 forward {get;set;}
        private float counter {get;set;}=1;
        public MissileBodyObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotationX, float rotationY, Color color)
            : base(graphicsDevice, position, size, rotationX, rotationY, color){
        }
        public void Update(GameTime gameTime, Vector3 Position, float Rotation){
            forward += World.Forward * counter * counter;
            counter+=1500f;
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Position = new Vector3(Position.X+forward.X, 10, Position.Z+forward.Z);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationY(Rotation);
            
            World *= Matrix.CreateTranslation(new Vector3(Position.X, 10, Position.Z));
        }

        /*
        //buen efecto para el cohete
        public void Update(GameTime gameTime, Vector3 Position, float Rotation){
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Position = new Vector3(Position.X+forward.X, 10, Position.Z+forward.Z);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationY(Rotation);
            forward += World.Forward * counter;
            counter*=2;
            World *= Matrix.CreateTranslation(new Vector3(Position.X, 10, Position.Z));
        }*/

    }
}
