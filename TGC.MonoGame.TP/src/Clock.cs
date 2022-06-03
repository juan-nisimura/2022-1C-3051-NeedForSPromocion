using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.Monogame.TP.Src   
{
    public class Clock
    {
        private float END_GAME_TOTAL_TIME = 300f;
        private float totalTime = 0;
        public void Update(GameTime gameTime) {
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            totalTime += elapsedTime;
        }

        public bool NoTimeLeft() {
            return totalTime >= END_GAME_TOTAL_TIME;
        }

        public void Reset() {
            totalTime = 0;
        }

        
    }
}