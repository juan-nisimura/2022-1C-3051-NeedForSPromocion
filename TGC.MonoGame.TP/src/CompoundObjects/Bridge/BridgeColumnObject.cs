using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bridge
{
    class BridgeColumnObject : CylinderObject <BridgeColumnObject>
    {
        public BridgeColumnObject(Vector3 position, Vector3 size, float rotation, Color color)
            : base(position, size, 0, rotation, color){
        }
    }
}