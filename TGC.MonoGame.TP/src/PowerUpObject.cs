using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    class PowerUpObject : BoxObject
    {
        private float Rotation;

        public PowerUpObject(GraphicsDevice graphicsDevice, Vector3 position) : base(graphicsDevice, position, new Vector3(10f,10f,10f), Color.Blue)
        {
        }
        public new void Initialize(){
            base.Initialize();
            Rotation = 0;
        }

        public override void Update(GameTime gameTime){
            Rotation += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            World = ScaleMatrix * Matrix.CreateRotationY(Rotation) * TranslateMatrix;
        }
    }
}