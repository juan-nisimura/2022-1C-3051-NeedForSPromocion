using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Map
{
    class FloorObject : QuadObject <FloorObject>
    {
        public FloorObject(Vector3 position, Vector3 size, float rotation)
            : base(position, size, rotation, Color.Black){            
        }
    }
}
