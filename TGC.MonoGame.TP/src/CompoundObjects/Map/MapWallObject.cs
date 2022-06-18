using Microsoft.Xna.Framework;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Map
{
    class MapWallObject : BoxObject <MapWallObject>
    {

        public MapWallObject(Vector3 position, Vector3 size, Color color) 
        : base(position, size, color, 0, Vector3.Zero)
        {
        }
    }
}
