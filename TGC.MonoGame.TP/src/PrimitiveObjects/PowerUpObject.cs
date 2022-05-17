using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class PowerUpObject : CubeObject <PowerUpObject>
    {
        private float Rotation;
        private Vector3 Position;
        private BoundingSphere BoundingSphere;
        private Boolean isAvailable = true;
        private float RespawnActualTime;
        const float RespawnCooldown = 10;

        //private PowerUp PowerUp;

        public PowerUpObject(GraphicsDevice graphicsDevice, Vector3 position) : base(graphicsDevice, position, new Vector3(10f,10f,10f), Color.Yellow)
        {
            Position = position + new Vector3(0f, 10f, 0f);
            TranslateMatrix = Matrix.CreateTranslation(Position);
            BoundingSphere = new BoundingSphere(Position, 8f);
        }
        public new void Initialize(){
            base.Initialize();
            Rotation = 0;
        }

        public void Update(GameTime gameTime, CarObject car){

            // Actualizo la matrix de mundo
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            Rotation += Convert.ToSingle(elapsedTime);
            World = ScaleMatrix * Matrix.CreateRotationY(Rotation) * TranslateMatrix;
        
            // Si el powerup está disponible
            if(isAvailable){

                // Chequeo si colisionó con el auto
                if(car.ObjectBox.Intersects(BoundingSphere)){
                    // Si colisionó con el auto, el auto obtiene un powerup
                    isAvailable = false;
                    RespawnActualTime = 0;
                    car.SetMachineGunTime();
                }

            } else {
                RespawnActualTime += elapsedTime;
                isAvailable = RespawnActualTime > RespawnCooldown;
            }
        }

        protected override void DrawPrimitive() {
            if(isAvailable)   
                BoxPrimitive.Draw(getEffect());
        }
    }
}