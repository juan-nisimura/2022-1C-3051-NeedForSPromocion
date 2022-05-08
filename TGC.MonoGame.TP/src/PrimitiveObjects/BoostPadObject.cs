using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    class BoostPadObject : QuadObject <BoostPadObject>
    {
        public BoostPadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation)
            : base(graphicsDevice, position, size, rotation, Color.GreenYellow){
        }
    }
}
