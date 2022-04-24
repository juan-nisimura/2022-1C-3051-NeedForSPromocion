using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    class TankObject : DefaultObject
    {
        public new void Initialize(){
            base.Initialize();
            World *= Matrix.CreateScale(0.005f);
            World *= Matrix.CreateTranslation(50f,0f,50f);
        }
        public new void Load(ContentManager content){
            ModelDirectory = "CombatVehicle/Vehicle";
            base.Load(content);
        }

        public override void Update(GameTime gameTime){
            
        }
    }
}