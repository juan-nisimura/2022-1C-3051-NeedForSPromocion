using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.Monogame.TP.Src.PrimitiveObjects  
{
    class CarBodyObject : MeshObject
    {
        public static void DrawBlinnPhong(Effect effect, ModelMesh mesh, Matrix world, float wheelAngle, float turningSpeed, Matrix view, Matrix projection)
        {
            MeshObject.DrawMeshBlinnPhong(effect, mesh, world, Matrix.CreateRotationX(wheelAngle) * Matrix.CreateRotationY(turningSpeed / 8f), view, projection);
        }
        public static void DrawBlinnPhong(Effect effect, ModelMesh mesh, Matrix world, Matrix view, Matrix projection)
        {
            MeshObject.DrawMeshBlinnPhong(effect, mesh, world, view, projection);
        }
    }
}