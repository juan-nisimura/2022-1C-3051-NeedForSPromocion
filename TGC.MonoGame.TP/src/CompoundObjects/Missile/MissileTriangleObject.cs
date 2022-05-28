using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class MissileTriangleObject : TriangleObject <MissileTriangleObject>
    {
        private Vector3 forward { get; set; }
        private float counter { get; set; } = -1f;
        public MissileTriangleObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color color)
            : base(graphicsDevice, position, vertex1, vertex2, vertex3, color){
        }

        public void Update(GameTime gameTime, Vector3 Position, float Rotation, float size)
        {
            forward += World.Forward * counter ;
            counter += 1000000*size;//5000000f;
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            Position = new Vector3(Position.X+ forward.X, Position.Y+5, Position.Z + forward.Z);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationX(MathHelper.PiOver2);
            World *= Matrix.CreateRotationY(Rotation);

            World *= Matrix.CreateTranslation(new Vector3(Position.X, Position.Y+5, Position.Z));
        }
    }
}
