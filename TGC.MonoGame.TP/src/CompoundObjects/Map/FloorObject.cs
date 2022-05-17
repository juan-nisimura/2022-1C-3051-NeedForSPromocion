using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Map
{
    class FloorObject : QuadObject <FloorObject>
    {
        public FloorObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation)
            : base(graphicsDevice, position, size, rotation, Color.Black){            
        }
    }
}
