using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.TP;
using System;

namespace TGC.Monogame.TP.Src.ModelObjects 
{
    public enum EngineState {
        Start, RunningFast, RunningSlow, Stopped//, Stopping
    }
 
    public class CarSoundEffects
    {
        private EngineState EngineState = EngineState.Stopped;
        private float[] Speed = new float[2]{ 0f, 0f };
        //private float[] Acceleration = new float[2]{ 0f, 0f };
        //private bool[] Crash = new bool[2]{ false, false };
        private float SoundEffectTime = 0f;

        private static SoundEffect FastEngineSound;
        private static SoundEffect SlowEngineSound;
        private static SoundEffect StartEngineSound;
        private static SoundEffect StopEngineSound;
        private SoundEffectInstance EngineInstance;

        public void Initialize() {
            EngineInstance = StartEngineSound.CreateInstance();
        }

        public void Start() {
            EngineInstance.Resume();
        }

        public void Stop() {
            EngineInstance.Pause();
        }

        public void Reset(){
            
        }

        public static void Load(ContentManager content) {
            FastEngineSound = content.Load<SoundEffect>(TGCGame.ContentFolderSounds + "fast engine");
            SlowEngineSound = content.Load<SoundEffect>(TGCGame.ContentFolderSounds + "slow engine");
            StartEngineSound = content.Load<SoundEffect>(TGCGame.ContentFolderSounds + "start engine 2");
            StopEngineSound = content.Load<SoundEffect>(TGCGame.ContentFolderSounds + "stop engine");
        }

        public void Update(GameTime gameTime, CarObject car) {
            Speed[0] = Speed[1];
            Speed[1] = MathF.Abs(car.Speed);
            //Acceleration[0] = Acceleration[1];
            //Acceleration[1] = car.Acceleration;
            //Crash[0] = Crash[1];
            // Crash[1] = car.Crash;

            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            switch(EngineState){
                case EngineState.Stopped:
                    if(Speed[1] > 0){
                        EngineState = EngineState.Start;
                        SwitchSoundInstance(ref EngineInstance, StartEngineSound);
                    }
                        
                    break;
                case EngineState.Start:
                    if(Speed[1] >= Speed[0])
                        SoundEffectTime += elapsedTime;
                    if(SoundEffectTime >= StartEngineSound.Duration.TotalSeconds || Speed[1] > CarObject.FAST_SPEED){
                        EngineState = EngineState.RunningSlow;
                        SwitchSoundInstance(ref EngineInstance, SlowEngineSound, true);
                    }
                    break;
                case EngineState.RunningSlow:
                    if(Speed[1] > CarObject.FAST_SPEED){
                        EngineState = EngineState.RunningFast;
                        SwitchSoundInstance(ref EngineInstance, FastEngineSound, true);
                    } else if (Speed[1] == 0){
                        EngineState = EngineState.Stopped;
                        SwitchSoundInstance(ref EngineInstance, StopEngineSound);
                    }
                    break;
                case EngineState.RunningFast:
                    if(Speed[1] < CarObject.FAST_SPEED){
                        EngineState = EngineState.RunningSlow;
                        SwitchSoundInstance(ref EngineInstance, SlowEngineSound, true);
                    }
                    break;
                    /*
                case EngineState.Stopping:
                    if()
                    break;*/
            }
        }

        private void SwitchSoundInstance(ref SoundEffectInstance instance, SoundEffect newSoundEffect, bool isLooped = false){
            SoundEffectTime = 0;
            instance.Stop();
            instance = newSoundEffect.CreateInstance();
            instance.IsLooped = isLooped;
            instance.Play();
        }
    }
}