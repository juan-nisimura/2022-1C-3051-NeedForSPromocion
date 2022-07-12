using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.Screens;

namespace TGC.Monogame.TP.Src.CompoundObjects.Tree
{
    class TreeTrunkObject : CylinderObject <TreeTrunkObject>
    {        
        protected BoundingBox BoundingBox { get; }

        protected override bool IsVisible() 
        {
            return LevelScreen.GetBoundingFrustum().Intersects(BoundingBox);
        }
        
        public TreeTrunkObject(Vector3 position, Vector3 size, float rotation, Color color)
            : base(position, size, 0, rotation, color){
            BoundingBox = new BoundingBox(position - size / 2, position + size / 2);
        }
        
    }
}
