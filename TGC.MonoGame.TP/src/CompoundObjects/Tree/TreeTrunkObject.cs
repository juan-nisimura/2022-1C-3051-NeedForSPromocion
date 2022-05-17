using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Tree
{
    class TreeTrunkObject : CylinderObject <TreeTrunkObject>
    {
        public TreeTrunkObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation, Color color)
            : base(graphicsDevice, position, size, 0, rotation, color){
        }
    }
}
