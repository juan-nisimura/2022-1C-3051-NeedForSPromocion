using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace TGC.Monogame.TP.Src.ModelObjects   
{
    class IACarObject : CarObject
    {
        public IACarObject(GraphicsDevice graphicsDevice, Vector3 position, Color color)
             : base(graphicsDevice, position, color)
        {
        }
    }
}