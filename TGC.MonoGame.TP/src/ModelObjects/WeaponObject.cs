using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TGC.Monogame.TP.Src.ModelObjects
{
    public class WeaponObject : DefaultModelObject <WeaponObject>
    {
        public new void Initialize(){
            base.Initialize();
            ScaleMatrix = Matrix.CreateScale(0.05f, 0.05f, 0.05f);
            RotationMatrix = Matrix.CreateRotationY(MathF.PI/2);
            TranslateMatrix = Matrix.CreateTranslation(0f, 10f, 0f);
        }
        public static void Load(ContentManager content){
            DefaultLoad(content, "CombatVehicle/Weapons", "BlinnPhong");
        }

        public override void Update(GameTime gameTime){
        }

        public void FollowCar(Matrix carWorld){
            World = ScaleMatrix * RotationMatrix * carWorld * TranslateMatrix;
        }
        public void DrawBlinnPhong(Effect effect, Matrix view, Matrix projection)
        {
            effect.CurrentTechnique = effect.Techniques["BasicColorDrawing"];
            Texture texture = getTexture();
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.

            effect.Parameters["ModelTexture"].SetValue(texture);

            foreach (var mesh in getModel().Meshes)
            {
                var meshWorld = mesh.ParentBone.Transform * World;
                effect.Parameters["World"].SetValue(meshWorld);
                effect.Parameters["WorldViewProjection"].SetValue(meshWorld * view * projection);
                effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(meshWorld)));
                mesh.Draw();
            }
        }
        public void DrawBloom(Effect effect, Matrix view, Matrix projection, Matrix world)
        {
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;
            effect.CurrentTechnique = effect.Techniques["WeaponBloomPass"];
            Texture texture = getTexture();
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.

            effect.Parameters["baseTexture"].SetValue(texture);

            foreach (var mesh in getModel().Meshes)
            {
                var meshWorld = mesh.ParentBone.Transform * World;
                effect.Parameters["WorldViewProjection"].SetValue(World * view * projection);
                mesh.Draw();
            }
        }

    }

}