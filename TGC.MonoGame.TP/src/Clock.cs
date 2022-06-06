using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TGC.MonoGame.TP;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    public class Clock
    {
        private float END_GAME_TOTAL_TIME = 100f;
        private float totalTime = 0;
        private SpriteFont Font;

        public void Load(ContentManager content)
        {
            Font = content.Load<SpriteFont>("SpriteFonts/" + "DS-Digital/DS-Digital");
        }
        public void Update() {
            totalTime += TGCGame.GetElapsedTime();
        }

        public bool NoTimeLeft() {
            return totalTime >= END_GAME_TOTAL_TIME;
        }

        public void Reset() {
            totalTime = 0;
        }
        public void Draw(Matrix view, Matrix projection)
        {
            var msg = "TIEMPO RESTANTE " + (END_GAME_TOTAL_TIME - totalTime).ToString("0.00");
            var W = TGCGame.GetGraphicsDevice().Viewport.Width;
            var H = TGCGame.GetGraphicsDevice().Viewport.Height;
            var escala = 1;
            var size = Font.MeasureString(msg) * escala;
            var Y = 50f;
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - size.X) / 2, Y, 0));
            TGCGame.GetSpriteBatch().DrawString(Font, msg, new Vector2(0, 0), Color.Red);
            TGCGame.GetSpriteBatch().End();
        }
    }
}