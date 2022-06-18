using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bridge
{
    class BridgeRampObject : RampObject <BridgeRampObject>
    {
        public BridgeRampObject(Vector3 position, Vector3 size, float rotation, Color color, int connectedBoxesTotalQuantity, Vector3 IAMapBoxPosition)
            : base(position, size, rotation, color, connectedBoxesTotalQuantity, IAMapBoxPosition)
        {
        }
    }
}
