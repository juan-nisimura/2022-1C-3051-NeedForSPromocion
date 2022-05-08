using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class PowerUpObject : CubeObject <PowerUpObject>
    {
        private float Rotation;

        public PowerUpObject(GraphicsDevice graphicsDevice, Vector3 position) : base(graphicsDevice, position, new Vector3(10f,10f,10f), Color.Yellow)
        {
            TranslateMatrix *= Matrix.CreateTranslation(0f, 10f, 0f);
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