using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.Monogame.TP.Src.PrimitiveObjects  
{
    class WheelObject : MeshObject
    {
        public static void Draw(Effect effect, ModelMesh mesh, Matrix world, float wheelAngle, float turningSpeed){
            MeshObject.Draw(effect, mesh, world, Matrix.CreateRotationX(wheelAngle) * Matrix.CreateRotationY(turningSpeed / 11f));
        }
    }
}