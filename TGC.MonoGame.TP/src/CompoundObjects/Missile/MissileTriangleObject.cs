using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class MissileTriangleObject : TriangleObject <MissileTriangleObject>
    {
        public MissileTriangleObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color color)
            : base(graphicsDevice, position, vertex1, vertex2, vertex3, color){
        }
    }
}
