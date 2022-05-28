using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Src;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Missile;

namespace TGC.Monogame.TP.Src.ModelObjects
{
    class PlayerCarObject : CarObject
    {
        //private Boolean SpeedBoostActive {get; set;}
        
        private Boolean MachineGunActive {get; set;}
        
        //private BulletObject bullet  {get; set;}
        //private List<Matrix> bulletsMatrix  {get; set;}
        private BulletObject[] MGBullets {get;set;}
        private Vector3 BulletPosicion {get;set;}
        private float BulletRotation {get;set;}
        private Vector3 MissilePosicion { get; set; }
        private float MissileRotation { get; set; }
        private int index {get;set;}=0;
        private List<BulletObject> MGBulletsList {get;set;}= new List<BulletObject>();
        private float MGShootDelay{get;set;}=0f;
        private List<MissileObject> MissileList {get;set;}= new List<MissileObject>();
        private float MissileShootDelay{get;set;}=0f;
        
        //private Boolean MisileActive {get; set;}

        public PlayerCarObject(Vector3 position, Color color)
             : base(position, color)
        {
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, Matrix View, Matrix Projection){
            // Capturo el estado del teclado
            var keyboardState = Keyboard.GetState();
    
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Si el auto esta tocando el piso
            if(OnTheGround){

                // Calculo la aceleracion y la velocidad
                //checkeo si queda tiempo de boostSpeed
                if( SpeedBoostTime > 0){
                    if (keyboardState.IsKeyDown(Keys.W)) {
                        Acceleration = ForwardAcceleration*5;
                        Speed = Math.Min(Speed + Acceleration * elapsedTime, MaxSpeed*2.5f);
                    }
                    else if (keyboardState.IsKeyDown(Keys.S)) {
                        Acceleration = - BackwardAcceleration*5;
                        Speed = Math.Max(Speed + Acceleration * elapsedTime, -MaxReverseSpeed*2);
                    }
                    else if (Speed > 0){
                        Acceleration = - StopAcceleration;
                        Speed = Math.Max(Speed + Acceleration * elapsedTime, 0);
                    }
                    else {
                        Acceleration = StopAcceleration;
                        Speed = Math.Min(Speed + Acceleration * elapsedTime, 0);
                    }
                    SpeedBoostTime-=elapsedTime;
                }else{
                    if (keyboardState.IsKeyDown(Keys.W)) {
                    Acceleration = ForwardAcceleration;
                    Speed = Math.Min(Speed + Acceleration * elapsedTime, MaxSpeed);
                    }
                    else if (keyboardState.IsKeyDown(Keys.S)) {
                        Acceleration = - BackwardAcceleration;
                        Speed = Math.Max(Speed + Acceleration * elapsedTime, -MaxReverseSpeed);
                    }
                    else if (Speed > 0){
                        Acceleration = - StopAcceleration;
                        Speed = Math.Max(Speed + Acceleration * elapsedTime, 0);
                    }
                    else {
                        Acceleration = StopAcceleration;
                        Speed = Math.Min(Speed + Acceleration * elapsedTime, 0);
                    }
                }


                

                

                
                // Calculo aceleracion y velocidad de giro
                if (keyboardState.IsKeyDown(Keys.A) && Speed != 0){
                    TurningAcceleration = MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * elapsedTime, -MaxTurningSpeed, MaxTurningSpeed);
                } 
                else if (keyboardState.IsKeyDown(Keys.D) && Speed != 0){
                    TurningAcceleration = -MaxTurningAcceleration * Speed / MaxSpeed;
                    TurningSpeed = Math.Clamp(TurningSpeed + TurningAcceleration * elapsedTime, -MaxTurningSpeed, MaxTurningSpeed);
                }
                else if (TurningSpeed > 0){
                    TurningAcceleration = -MaxTurningAcceleration;
                    TurningSpeed = Math.Max(TurningSpeed + TurningAcceleration * elapsedTime, 0);
                }
                else{
                    TurningAcceleration = MaxTurningAcceleration;
                    TurningSpeed = Math.Min(TurningSpeed + TurningAcceleration * elapsedTime, 0);
                }

                // Calculo rotacion del auto
                Rotation += TurningSpeed * elapsedTime;

                // Calculo velocidad vertical
                if(keyboardState.IsKeyDown(Keys.Space))
                    VerticalSpeed = JumpInitialSpeed;
                else
                    VerticalSpeed = 0;
            }
            // Si el auto esta en el aire
            else{
                // Calculo velocidad vertical
                VerticalSpeed = VerticalSpeed - GRAVITY * elapsedTime;
            }
            
            // Esto calcula la posición del auto
            base.Update(gameTime);

            if(MachineGunTime > 0){
                //MGBullets = new BulletObject[10];
                //MGBulletsList = new List<BulletObject>();
                
                    if (keyboardState.IsKeyDown(Keys.RightShift)&& MGShootDelay<=0) {
                        //MachineGunActive = true;
                        BulletPosicion = Position;
                        BulletRotation = Rotation;
                        //TO DO solo genera una sola bala, falta generar la lista dinamicamente
                        /*MGBullets = new BulletObject[]{
                            new BulletObject(graphicsDevice,BulletPosicion,5f,BulletRotation),
                            new BulletObject(graphicsDevice,BulletPosicion2,10f,BulletRotation)
                        };*/
                        
                        var bullet = new BulletObject(graphicsDevice,BulletPosicion,5f,BulletRotation);
                        MGBulletsList.Add(bullet);
                        bullet.Initialize();
                        MGShootDelay=0.2f;
                        
                        //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Update(gameTime, Position, Rotation);
                        //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Draw(View, Projection);
                    }
                    MGShootDelay-=elapsedTime;
                    MachineGunTime-=elapsedTime;
                    //MGBulletsList.ForEach(bullet => bullet.Initialize());
                    MGBulletsList.ForEach(bullet => bullet.Update(gameTime, bullet.bulletPosition, bullet.bulletRotationY,Speed));
            }else{
                MGBulletsList.Clear();
            }
                
                /*if(MGBullets != null){
                    if(MachineGunActive){
                        for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Initialize();
                        MachineGunActive = false;
                    }
                
                for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Update(gameTime, BulletPosicion, BulletRotation);
                }*/
            if(MissileTime > 0){
                //MGBullets = new BulletObject[10];
                //MGBulletsList = new List<BulletObject>();
                
                    if (keyboardState.IsKeyDown(Keys.RightControl)&& MissileShootDelay<=0) {
                        //MachineGunActive = true;
                        MissilePosicion = Position;
                        MissileRotation = Rotation;
                        //TO DO solo genera una sola bala, falta generar la lista dinamicamente
                        /*MGBullets = new BulletObject[]{
                            new BulletObject(graphicsDevice,BulletPosicion,5f,BulletRotation),
                            new BulletObject(graphicsDevice,BulletPosicion2,10f,BulletRotation)
                        };*/
                        
                        var misile = new MissileObject(graphicsDevice, MissilePosicion, 15f,MissileRotation);
                        MissileList.Add(misile);
                        misile.Initialize();
                        MissileShootDelay=1f;
                        
                        //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Update(gameTime, Position, Rotation);
                        //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Draw(View, Projection);
                    }
                    MissileShootDelay-=elapsedTime;
                    MissileTime-=elapsedTime;
                    //MGBulletsList.ForEach(bullet => bullet.Initialize());
                    MissileList.ForEach(missile => missile.Update(gameTime, missile.missilePosition, missile.missileRotationY));
            }else{
                MissileList.Clear();
            }
        
            ObjectBox.Center = Position;
            // Hacerlo que funcione cuando se incline
            //ObjectBox.Orientation = Matrix.CreateRotationY(Rotation);
        }

        /*public void SetSpeedBoostActive(Boolean isActive){
            SpeedBoostActive = isActive;
        }*/
        
        /*public BulletObject[] GetMGBullets(){
            return MGBullets;
        }*/
        public List<BulletObject> GetMGBulletsList(){
            return MGBulletsList;
        }
        public List<MissileObject> GetMissileList(){
            return MissileList;
        }
        
    }
}