using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PowerUpObjects;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.ModelObjects   
{
    public class IACarObject : CarObject
    {
        public PowerUpObject[] MapPowerUps;
        public int MapPowerUpsQuantity;
        private bool AccelerateForward, TurnLeft, TurnRight, 
                    AccelerateBackward, Jump, UsePowerUp, PreviousUsePowerUp;
        public IACarObject(Vector3 position, Color color, PowerUpObject[] mapPowerUps)
             : base(position, color)
        {
            this.MapPowerUps = mapPowerUps;
            this.MapPowerUpsQuantity = mapPowerUps.Length;
        }

        public override void SetPowerUpHUDModel(PowerUpModel powerUpModel){

        }

        private Vector3 NearestPowerUpPosition(){
            PowerUpObject nearestPowerUp = MapPowerUps[0];
            float distance;
            float bestDistance = 1000000;
            int i = 0;

            // Busco el primer powerup disponible
            for(; i < MapPowerUpsQuantity; i++){
                if(MapPowerUps[i].IsAvailable){
                    nearestPowerUp = MapPowerUps[i];
                    bestDistance = Vector3.Distance(this.Position, MapPowerUps[i].GetPosition());
                    break;
                }
            }

            // Busco si hay alguno más cerca
            for(; i < MapPowerUpsQuantity; i++){
                if(MapPowerUps[i].IsAvailable){
                    distance = Vector3.Distance(this.Position, MapPowerUps[i].GetPosition());
                    if(distance < bestDistance) {
                        nearestPowerUp = MapPowerUps[i];
                        bestDistance = distance;
                    }
                }
            }

            return nearestPowerUp.GetPosition();
        }

        public override void Update()
        {
            // Capturo el estado del teclado
            AccelerateForward = true;
            AccelerateBackward = false;
            Jump = false;

            Vector3 targetPosition;

            if(PowerUp.CanBeTriggered())
                targetPosition = Enemies[0].Position;
            else
                targetPosition = NearestPowerUpPosition();

            var targetDistance = targetPosition - Position;
            var anguloXZ = MathF.Atan2(World.Forward.Z * targetDistance.X - World.Forward.X * targetDistance.Z, World.Forward.X * targetDistance.X + World.Forward.Z * targetDistance.Z);

            TurnLeft = anguloXZ < 0;
            TurnRight = anguloXZ > 0;
            
            var enemyDistance = (Enemies[0].Position - Position).Length();

            PreviousUsePowerUp = UsePowerUp;
            UsePowerUp = Enemies[0].ObjectBox.Intersects(new BoundingSphere(Position - Vector3.Normalize(World.Forward) * enemyDistance, 10f));

            // Si el auto esta tocando el piso
            if(OnTheGround){

                // Calculo la aceleracion y la velocidad
                // checkeo si queda tiempo de boostSpeed
                if( SpeedBoostTime > 0){
                    if (AccelerateForward) {
                        Acceleration = ForwardAcceleration*5;
                        Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), MaxSpeed*2.5f);
                    }
                    else if (AccelerateBackward) {
                        Acceleration = - BackwardAcceleration*5;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), -MaxReverseSpeed*2);
                    }
                    else if (Speed > 0){
                        Acceleration = - StopAcceleration;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                    else {
                        Acceleration = StopAcceleration;
                        Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                    SpeedBoostTime -= TGCGame.GetElapsedTime();
                }else{
                    if (AccelerateForward) {
                    Acceleration = ForwardAcceleration;
                    Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), MaxSpeed);
                    }
                    else if (AccelerateBackward) {
                        Acceleration = - BackwardAcceleration;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), -MaxReverseSpeed);
                    }
                    else if (Speed > 0){
                        Acceleration = - StopAcceleration;
                        Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                    else {
                        Acceleration = StopAcceleration;
                        Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
                    }
                }

                // Calculo aceleracion y velocidad de giro
                if (TurnLeft && Speed != 0){
                    TurningAcceleration = MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), -MaxTurningSpeed, MaxTurningSpeed);
                } 
                else if (TurnRight && Speed != 0){
                    TurningAcceleration = -MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), -MaxTurningSpeed, MaxTurningSpeed);
                }
                else if (TurningSpeed > 0){
                    TurningAcceleration = -MaxTurningAcceleration;
                    TurningSpeed = Math.Max(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
                }
                else{
                    TurningAcceleration = MaxTurningAcceleration;
                    TurningSpeed = Math.Min(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
                }

                // Calculo rotacion del auto
                Rotation += TurningSpeed * TGCGame.GetElapsedTime();

                // Calculo velocidad vertical
                if(Jump)
                    VerticalSpeed = JumpInitialSpeed;
                else
                    VerticalSpeed = 0;
            }
            // Si el auto esta en el aire
            else{
                // Calculo velocidad vertical
                VerticalSpeed = VerticalSpeed - GRAVITY * TGCGame.GetElapsedTime();
            }

            





            /*
            if (Speed > 0){
                Acceleration = - StopAcceleration;
                Speed = Math.Max(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
            }
            else {
                Acceleration = StopAcceleration;
                Speed = Math.Min(Speed + Acceleration * TGCGame.GetElapsedTime(), 0);
            }
            if (TurningSpeed > 0){
                TurningAcceleration = -MaxTurningAcceleration;
                TurningSpeed = Math.Max(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
            }else{
                TurningAcceleration = MaxTurningAcceleration;
                TurningSpeed = Math.Min(TurningSpeed + TurningAcceleration * TGCGame.GetElapsedTime(), 0);
            }

            // Calculo rotacion del auto
            Rotation += TurningSpeed * TGCGame.GetElapsedTime();

            */

            // Esto calcula la posicion del auto
            base.Update();
            
            // Lógica para disparar el power up
            
            if(!PreviousUsePowerUp && UsePowerUp)       PowerUp.TriggerEffect(this);
            else if(PreviousUsePowerUp && !UsePowerUp)  PowerUp.StopTriggerEffect(this);
            PowerUp.Update(this);

            ObjectBox.Center = Position;
            ObjectBox.Orientation = RotationMatrix;
        }
    }
}