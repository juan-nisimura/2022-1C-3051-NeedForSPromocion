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
        public static void Load(){
            DefaultLoad("CombatVehicle/Weapons", "WeaponShader");
        }

        public override void Update(){
        }

        public void FollowCar(Matrix carWorld){
            World = ScaleMatrix * RotationMatrix * carWorld * TranslateMatrix;
        }
    }
}