using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src   
{
    public class Clock
    {
        private float END_GAME_TOTAL_TIME = 300f;
        private float totalTime = 0;
        public void Update() {
            totalTime += TGCGame.GetElapsedTime();
        }

        public bool NoTimeLeft() {
            return totalTime >= END_GAME_TOTAL_TIME;
        }

        public void Reset() {
            totalTime = 0;
        }        
    }
}