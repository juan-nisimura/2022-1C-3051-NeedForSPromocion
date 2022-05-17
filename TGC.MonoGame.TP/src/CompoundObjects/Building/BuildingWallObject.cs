using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Building
{
    class BuildingWallObject : BoxObject <BuildingWallObject>
    {
        public BuildingWallObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color) 
        : base(graphicsDevice, position, size, color)
        {
        }
    }
}
