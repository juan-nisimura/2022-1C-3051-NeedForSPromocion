using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.PowerUpObjects;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.ModelObjects   
{
    public class IACarObject : CarObject
    {
        private const float ANGULO_BUSQUEDA_IA = MathF.PI / 20;
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

        public Vector3 RotateInPlaneXZ(Vector3 vector, float angle){
            return new Vector3(vector.X * MathF.Cos(angle) - vector.Z * MathF.Sin(angle), vector.Y, vector.X * MathF.Sin(angle) + vector.Z * MathF.Cos(angle));
        }

        public override void Update()
        {
            if(IsDead())
                return;

            AccelerateForward = true;
            AccelerateBackward = false;
            Jump = false;

            Vector3 normalizedForward = Vector3.Normalize(World.Forward);

            Vector3 targetPosition;

            CarObject healthiestEnemy = Enemies[0];
            float highestHealth = Enemies[0].Health; 

            for(int i = 1; i < TGCGame.PLAYERS_QUANTITY - 1; i++) {
                if(Enemies[i].Health > highestHealth) {
                    highestHealth = Enemies[i].Health;
                    healthiestEnemy = Enemies[i];
                }
            }

            float enemyDistance = (healthiestEnemy.Position - Position).Length();

            if(PowerUp.CanBeTriggered() || SpeedBoostTime > 0 && enemyDistance < 100f)
                targetPosition = healthiestEnemy.Position;
            else
                targetPosition = NearestPowerUpPosition();
                

            targetPosition = IALogicalMap.GetTargetPositionInAdjacetCell(this.Position, targetPosition);

            var targetDistance = targetPosition - Position;
            var anguloXZ = MathF.Atan2(World.Forward.Z * targetDistance.X - World.Forward.X * targetDistance.Z, World.Forward.X * targetDistance.X + World.Forward.Z * targetDistance.Z);

            TurnLeft = anguloXZ < 0.0785;
            TurnRight = anguloXZ > 0.0785;

            AccelerateForward = targetDistance.Length() > 100 || MathF.Abs(anguloXZ) < MathF.PI / 2.8 || Speed < MaxSpeed * 0.3;

            // Calculo diferenciales hacia delante del auto, para detectar obstaculos
            var forwardPosition = Position;
            var forwardDistance = - normalizedForward * 3;
            for(int i = 0; i < 20; i++){
                if(HeightMap.GetDifferential(forwardPosition, forwardDistance, HeightMap.GetActualLevel(Position.Y)) > 10f){
                    AccelerateForward = false;
                    AccelerateBackward = true;
                    break;
                }
                forwardPosition += forwardDistance;
            }

            // Si el auto encontró un obstaculo hacia delante, busco otro camino cambiando el ángulo
            var forwardLeftPosition = Position;
            var forwardRightPosition = Position;
            var normalizedLeftForward = RotateInPlaneXZ(normalizedForward, ANGULO_BUSQUEDA_IA);
            var normalizedRightForward = RotateInPlaneXZ(normalizedForward, -ANGULO_BUSQUEDA_IA);
            var forwardRightDistance = - normalizedRightForward * 4;
            var forwardLeftDistance = - normalizedLeftForward * 4;
            
            for(int i = 0; i < 20; i++){
                if(HeightMap.GetDifferential(forwardLeftPosition, forwardLeftDistance, HeightMap.GetActualLevel(Position.Y)) > 6f){
                    TurnLeft = true;
                    if(i < 5){
                        AccelerateForward = false;
                        AccelerateBackward = true;
                    }
                    break;
                }
                if(HeightMap.GetDifferential(forwardRightPosition, forwardRightDistance, HeightMap.GetActualLevel(Position.Y)) > 6f){
                    TurnRight = true;
                    if(i < 5){
                        AccelerateForward = false;
                        AccelerateBackward = true;
                    }
                    break;
                }
                forwardLeftPosition += forwardLeftDistance;
                forwardRightPosition += forwardRightDistance;
            }

            PreviousUsePowerUp = UsePowerUp;
            UsePowerUp = healthiestEnemy.ObjectBox.Intersects(new BoundingSphere(Position - normalizedForward * MathF.Min(enemyDistance, 250f), 8f));
            
            AccelerateForward = AccelerateForward || (enemyDistance < 100 && UsePowerUp);

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