using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    class IACarObject : CarObject
    {
        public IACarObject(GraphicsDevice graphicsDevice, Vector3 position, Color color)
             : base(graphicsDevice, position, color)
        {
        }
    }
}