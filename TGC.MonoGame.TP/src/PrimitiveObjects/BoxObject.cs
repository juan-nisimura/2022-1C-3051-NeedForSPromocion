using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class BoxObject : CubeObject <BoxObject>
    {

        public BoxObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color) : base(graphicsDevice, position, size, color)
        {
        }
    }
}
