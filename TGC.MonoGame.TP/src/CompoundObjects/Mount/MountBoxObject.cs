using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Mount
{
    class MountBoxObject : BoxObject <MountBoxObject>
    {

        public MountBoxObject(Vector3 position, Vector3 size, Color color) 
        : base(position, size, color)
        {
        }
    }
}
