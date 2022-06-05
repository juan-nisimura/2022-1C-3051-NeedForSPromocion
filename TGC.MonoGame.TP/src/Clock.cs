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
        public void Draw(Matrix view, Matrix projection, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            var msg = "TIEMPO RESTANTE " + (END_GAME_TOTAL_TIME - totalTime).ToString("0.00");
            var W = graphicsDevice.Viewport.Width;
            var H = graphicsDevice.Viewport.Height;
            var escala = 1;
            var size = Font.MeasureString(msg) * escala;
            var Y = 50f;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - size.X) / 2, Y, 0));
            spriteBatch.DrawString(Font, msg, new Vector2(0, 0), Color.Red);
            spriteBatch.End();
        }
    }
}