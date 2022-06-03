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
        public static void DrawMeshBlinnPhong(Effect effect, ModelMesh mesh, Matrix world, Matrix MeshTransform, Matrix view, Matrix projection)
        {
            var meshWorld = MeshTransform * mesh.ParentBone.Transform * world;
            effect.Parameters["World"].SetValue(meshWorld);
            effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(meshWorld)));
            effect.Parameters["WorldViewProjection"].SetValue(meshWorld * view * projection);
            mesh.Draw();
        }
        public static void DrawMeshBlinnPhong(Effect effect, ModelMesh mesh, Matrix world, Matrix view, Matrix projection)
        {
            var meshWorld = mesh.ParentBone.Transform * world;
            effect.Parameters["World"].SetValue(meshWorld);
            effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(meshWorld)));
            effect.Parameters["WorldViewProjection"].SetValue(meshWorld * view * projection);
            mesh.Draw();
        }

        public static void Draw(Effect effect, ModelMesh mesh, Matrix world){
            var meshWorld = mesh.ParentBone.Transform * world;
            effect.Parameters["World"].SetValue(meshWorld);
            mesh.Draw();
        }
    }
}