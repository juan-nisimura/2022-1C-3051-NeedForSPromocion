using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bridge
{
    class BridgeRampObject : RampObject <BridgeRampObject>
    {
        public BridgeRampObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation, Color color)
            : base(graphicsDevice, position, size, rotation, color)
        {
        }
    }
}
