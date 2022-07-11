using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TGC.MonoGame.TP;
using Microsoft.Xna.Framework.Content;
using TGC.Monogame.TP.Src.MyContentManagers;

namespace TGC.Monogame.TP.Src   
{
    public class Clock
    {
        private const float GAME_TOTAL_TIME = 360f;
        private float totalTime = GAME_TOTAL_TIME;
        private SpriteFont Font;

        public void Load()
        {
            Font = MyContentManager.SpriteFonts.Load("DS-Digital/DS-Digital");
        }

        public void Update() {
            totalTime -= TGCGame.GetElapsedTime();
        }

        public bool NoTimeLeft() {
            return totalTime < 0;
        }

        public void Reset() {
            totalTime = GAME_TOTAL_TIME;
        }
        public void Draw(Matrix view, Matrix projection)
        {
            var minutos = MathF.Floor(totalTime / 60);
            var segundos = MathF.Floor(totalTime) - minutos * 60; 
            var msg = minutos.ToString("00") + ":" + segundos.ToString("00");
            var W = TGCGame.GetGraphicsDevice().Viewport.Width;
            var H = TGCGame.GetGraphicsDevice().Viewport.Height;
            var escala = 1;
            var size = Font.MeasureString(msg) * escala;
            var Y = 25f;
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - (Font.MeasureString("TIEMPO RESTANTE").X)) / 2, Y, 0));
            TGCGame.GetSpriteBatch().DrawString(Font, "TIEMPO RESTANTE", new Vector2(0, 0), Color.Red);
            TGCGame.GetSpriteBatch().DrawString(Font, msg, new Vector2((Font.MeasureString("TIEMPO RESTANTE").X * escala - size.X) / 2, 30), Color.Red);
            TGCGame.GetSpriteBatch().End();
        }
    }
}