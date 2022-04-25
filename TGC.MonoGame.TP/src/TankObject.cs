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
            World *= Matrix.CreateScale(0.05f);
            World *= Matrix.CreateTranslation(-100f,0f,-100f);
        }
        public new void Load(ContentManager content){
            ModelDirectory = "RacingCarA/RacingCar";
            DiffuseColor = Color.DarkGreen.ToVector3();
            base.Load(content);
        }

        public override void Update(GameTime gameTime){
            
        }
    }
}