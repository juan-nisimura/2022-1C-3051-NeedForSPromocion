using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.HUD;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUps;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.Screens;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects
{
    public class PowerUpObject : CubeObject <PowerUpObject>
    {
        private float Rotation;
        private Vector3 Position;
        private BoundingSphere BoundingSphere;
        public Boolean IsAvailable = true;
        private float RespawnActualTime;
        private float Time;
        //private Matrix BouncingTranslation = Matrix.Identity;
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
            Time = 0;
        }

        public Vector3 GetPosition(){
            return Position;
        }

        public void Reset() {
            IsAvailable = true;
            Time = 0;
        }
        protected override bool IsVisible() 
        {
            return LevelScreen.GetBoundingFrustum().Intersects(BoundingSphere);
        }
        
        public void Update(CarObject[] cars){
        
            var BouncingTranslation = Matrix.CreateTranslation(0f, MathF.Abs(30 * MathF.Cos(Time * 5)) / (1 + MathF.Pow(Time * 3, 2)), 0f);
            
            // Si el powerup está disponible
            if(IsAvailable){
                Time += TGCGame.GetElapsedTime();
                
                for(int i= 0; i < TGCGame.PLAYERS_QUANTITY; i++){
                    // Chequeo si colisionó con el auto
                    if(cars[i].ObjectBox.Intersects(BoundingSphere)){
                        // Si colisionó con el auto, el auto obtiene un powerup
                        IsAvailable = false;
                        RespawnActualTime = 0;
                        Time = 0;

                        switch(RandomPowerUp.Next(3)){
                            case 1:
                                cars[i].SetPowerUp(new MachineGunPowerUp());
                                cars[i].SetPowerUpHUDModel(BulletPowerUpModel.GetModel());
                                //PowerUpHUDCircleObject.SetPowerUpModel(BulletPowerUpModel.GetModel());
                                break;
                            case 2:
                                cars[i].SetPowerUp(new MissileLauncherPowerUp());
                                cars[i].SetPowerUpHUDModel(MissilePowerUpModel.GetModel());
                                //PowerUpHUDCircleObject.SetPowerUpModel(MissilePowerUpModel.GetModel());
                                break;
                            default:
                                cars[i].SetPowerUp(new SpeedBoostPowerUp());
                                cars[i].SetPowerUpHUDModel(SpeedBoostPowerUpModel.GetModel());
                                //PowerUpHUDCircleObject.SetPowerUpModel(SpeedBoostPowerUpModel.GetModel());
                                break;
                        }

                        break;
                    }
                }
            } else {
                RespawnActualTime += TGCGame.GetElapsedTime();
                IsAvailable = RespawnActualTime > RespawnCooldown;
            }

            // Actualizo la matrix de mundo
            Rotation += Convert.ToSingle(TGCGame.GetElapsedTime());
            World = ScaleMatrix * Matrix.CreateRotationY(Rotation) * TranslateMatrix * BouncingTranslation;
        }

        protected override void DrawPrimitive(Effect effect) {
            if(IsAvailable)   
                BoxPrimitive.Draw(effect);
        }
    }
}