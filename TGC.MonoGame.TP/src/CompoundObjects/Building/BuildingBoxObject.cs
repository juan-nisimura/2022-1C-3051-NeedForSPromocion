using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Building
{
    class BuildingBoxObject : BoxObject <BuildingBoxObject>
    {
        public BuildingBoxObject(Vector3 position, Vector3 size, Color color, int connectedBoxesTotalQuantity) 
        : base(position, size, color, connectedBoxesTotalQuantity, Vector3.Zero)
        {
        }
    }
}
