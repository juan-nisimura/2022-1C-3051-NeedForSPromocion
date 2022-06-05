
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src
{
    public class CameraObject
    {
        private Vector3 Position { get; set; } = new Vector3(0f,0f,0f);
        private static float CameraAbsoluteSpeed = 300f;
        private Vector3 Speed;

        public CameraObject MoveCameraByKeyboard()
        {
            // Capturo el estado del teclado
            var keyboardState = Keyboard.GetState();

            Speed = new Vector3(0f,0f,0f);

            // Calculo la aceleracion y la velocidad
            if (keyboardState.IsKeyDown(Keys.W)) {
                Speed += CameraAbsoluteSpeed * new Vector3(1f,0f,1f);
            }
            else if (keyboardState.IsKeyDown(Keys.S)) {
                Speed += CameraAbsoluteSpeed * new Vector3(-1f,0f,-1f);
            }
            
            if (keyboardState.IsKeyDown(Keys.D)) {
                Speed += CameraAbsoluteSpeed * new Vector3(-1f,0f,1f);
            }
            else if (keyboardState.IsKeyDown(Keys.A)) {
                Speed += CameraAbsoluteSpeed * new Vector3(1f,0f,-1f);
            }
            
            // Calculo la nueva posicion
            Position += Speed * TGCGame.GetElapsedTime();

            return this;
        }

        public Matrix GetView()
        {
            return Matrix.CreateLookAt(Position + new Vector3(-100f, 150f, -100f), Position, new Vector3(1f, 1.5f, 1f));
        }

        public CameraObject FollowCamera(Vector3 position)
        {
            Position = position;
            return this;
        }
    }
}