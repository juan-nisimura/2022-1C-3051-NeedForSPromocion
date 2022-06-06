using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUps
{
    class PowerUpObject : CubeObject <PowerUpObject>
    {
        private float Rotation;
        private Vector3 Position;
        private BoundingSphere BoundingSphere;
        private Boolean isAvailable = true;
        private float RespawnActualTime;
        const float RespawnCooldown = 10;

        private static Random RandomPowerUp = new Random(); 

        public PowerUpObject(Vector3 position) : base(position, new Vector3(10f,10f,10f), Color.Yellow)
        {
            Position = position + new Vector3(0f, 10f, 0f);
            TranslateMatrix = Matrix.CreateTranslation(Position);
            BoundingSphere = new BoundingSphere(Position, 8f);
        }
        public new void Initialize(){
            base.Initialize();
            Rotation = 0;
        }

        public void Reset() {
            isAvailable = true;
        }

        public void Update(CarObject car){

            // Actualizo la matrix de mundo
            Rotation += Convert.ToSingle(TGCGame.GetElapsedTime());
            World = ScaleMatrix * Matrix.CreateRotationY(Rotation) * TranslateMatrix;
        
            // Si el powerup está disponible
            if(isAvailable){

                // Chequeo si colisionó con el auto
                if(car.ObjectBox.Intersects(BoundingSphere)){
                    // Si colisionó con el auto, el auto obtiene un powerup
                    isAvailable = false;
                    RespawnActualTime = 0;

                    switch(RandomPowerUp.Next(3)){
                        case 1:
                            car.SetPowerUp(new MachineGunPowerUp());
                            break;
                        case 2:
                            car.SetPowerUp(new MissileLauncherPowerUp());
                            break;
                        default:
                            car.SetPowerUp(new SpeedBoostPowerUp());
                            break;
                    }
                }

            } else {
                RespawnActualTime += TGCGame.GetElapsedTime();
                isAvailable = RespawnActualTime > RespawnCooldown;
            }
        }

        protected override void DrawPrimitive() {
            if(isAvailable)   
                BoxPrimitive.Draw(getEffect());
        }
    }
}