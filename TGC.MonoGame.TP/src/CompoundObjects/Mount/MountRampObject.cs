using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Mount
{
    class MountRampObject : RampObject <MountRampObject>
    {
        public MountRampObject(Vector3 position, Vector3 size, float rotation, Color color)
            : base(position, size, rotation, color, 0, Vector3.Zero)
        {
        }
    }
}