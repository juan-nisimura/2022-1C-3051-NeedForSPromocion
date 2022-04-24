using Microsoft.Xna.Framework.Input;

namespace TGC.Monogame.TP.Src
{
    public class KeyController
    {
        private enum KeyStates { 
            NotPressed,
            ToPressed,
            Pressed,
            ToNotPressed
        }

        private Keys Key;
        private KeyStates KeyState;
        public KeyController(Keys key){
            Key = key;
            KeyState = KeyStates.NotPressed;
        }

        public KeyController Update()
        {
            switch(KeyState)
            {
                case KeyStates.NotPressed:
                    if(Keyboard.GetState().IsKeyDown(Key))
                        KeyState = KeyStates.ToPressed;
                    break;
                case KeyStates.ToPressed:
                    KeyState = KeyStates.Pressed;
                    break;
                case KeyStates.Pressed:
                    if(!Keyboard.GetState().IsKeyDown(Key))
                        KeyState = KeyStates.ToNotPressed;
                    break;
                case KeyStates.ToNotPressed:
                    KeyState = KeyStates.NotPressed;
                    break;
            }

            return this;
        }

        public bool IsKeyToPressed(){
            return KeyState == KeyStates.ToPressed;
        }


    }
}