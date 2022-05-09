using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.Monogame.TP.Src.PrimitiveObjects  
{
    class MeshObject
    {
        public static void Draw(Effect effect, ModelMesh mesh, Matrix world, Matrix MeshTransform){
            var meshWorld = MeshTransform * mesh.ParentBone.Transform * world;
            effect.Parameters["World"].SetValue(meshWorld);
            mesh.Draw();
        }

        public static void Draw(Effect effect, ModelMesh mesh, Matrix world){
            var meshWorld = mesh.ParentBone.Transform * world;
            effect.Parameters["World"].SetValue(meshWorld);
            mesh.Draw();
        }
    }
}