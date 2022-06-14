using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.TP;
using System;
using TGC.Monogame.TP.Src.MyContentManagers;

namespace TGC.Monogame.TP.Src.ModelObjects 
{
    public enum EngineState {
        Start, RunningFast, RunningSlow, Stopped, Crashed
    }
 
    public class CarSoundEffects
    {
        private EngineState EngineState = EngineState.Stopped;
        private float[] Speed = new float[2]{ 0f, 0f };
        //private float[] Acceleration = new float[2]{ 0f, 0f };
        private bool[] Crash = new bool[2]{ false, false };
        private float SoundEffectTime = 0f;

        private static SoundEffect FastEngineSound;
        private static SoundEffect SlowEngineSound;
        private static SoundEffect StartEngineSound;
        private static SoundEffect StopEngineSound;
        private static SoundEffect CrashSound;
        private SoundEffectInstance EngineInstance;
        private const float CAR_SPEED_CRASH = 200f;

        public void Initialize() {
            EngineInstance = StartEngineSound.CreateInstance();
        }

        public void Start() {
            if(EngineState != EngineState.Stopped)
                EngineInstance.Resume();
        }

        public void Stop() {
            EngineInstance.Pause();
        }

        public void Reset(){
            
        }

        public static void Load() {
            FastEngineSound = MyContentManager.SoundEffects.Load("fast engine");
            SlowEngineSound = MyContentManager.SoundEffects.Load("slow engine");
            StartEngineSound = MyContentManager.SoundEffects.Load("start engine 2");
            StopEngineSound = MyContentManager.SoundEffects.Load("stop engine");
            CrashSound = MyContentManager.SoundEffects.Load("car collision");
        }

        public void PlayCrashSound(){
            CrashSound.CreateInstance().Play();
        }

        public void Update(CarObject car) {
            Speed[0] = Speed[1];
            Speed[1] = MathF.Abs(car.Speed);
            /*
            Crash[0] = Crash[1];
            Crash[1] = car.HasCrashed;*/

            /*
            if(Crash[1] && !Crash[0] && Speed[1] > CAR_SPEED_CRASH){
                CrashSound.CreateInstance().Play();
            }*/

            switch(EngineState){
                case EngineState.Stopped:
                    if(Speed[1] > 0){
                        EngineState = EngineState.Start;
                        SwitchSoundInstance(ref EngineInstance, StartEngineSound);
                    }
                    break;
                case EngineState.Start:
                    if(Speed[1] >= Speed[0])
                        SoundEffectTime += TGCGame.GetElapsedTime();
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