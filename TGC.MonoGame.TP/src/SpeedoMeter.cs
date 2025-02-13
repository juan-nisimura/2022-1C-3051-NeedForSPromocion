using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.MyContentManagers;

namespace TGC.Monogame.TP.Src   
{
    public class SpeedoMeter
    {
        private SpriteFont Font;
        private Texture2D Texture;
        private Texture2D Needle;
        private float Speed;

        public void Load()
        {
            Texture = (Texture2D) MyContentManager.Textures.Load("speedo_tex");
            Needle = (Texture2D) MyContentManager.Textures.Load("speedoneedle");
            Font = MyContentManager.SpriteFonts.Load("DS-Digital/DS-Digital");
        }
        public void Update(float carSpeed) {
            Speed = carSpeed/750*28;
        }
        public void Draw(Matrix view, Matrix projection)
        {
            var msg = Speed.ToString("0");
            var SpeedIndicator = Math.Abs(Speed); 
            var W = TGCGame.GetGraphicsDevice().Viewport.Width;
            var H = TGCGame.GetGraphicsDevice().Viewport.Height;
            var escala = W / 1400f;
            var escalaTex = H / 1400f;
            Vector2 position = new Vector2(W-Texture.Width*escalaTex,H - Texture.Height* escalaTex);

            var size = Font.MeasureString(msg) * escala;
            // var Y = 0f;
            
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escalaTex) * Matrix.CreateTranslation(0, 0, 0));
            TGCGame.GetSpriteBatch().Draw(Texture, position/escalaTex, Color.White);
            TGCGame.GetSpriteBatch().End();
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escalaTex) * Matrix.CreateRotationZ((float)Math.PI/2+SpeedIndicator / 60) * Matrix.CreateTranslation(position.X+Texture.Width*escalaTex/2,position.Y+ Texture.Height * escalaTex/2, 0));
            TGCGame.GetSpriteBatch().Draw(Needle, new Vector2(0,0), Color.White);
            TGCGame.GetSpriteBatch().End();
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((Texture.Width * escalaTex - size.X) /2, (Texture.Height * escalaTex - size.Y) *3/4, 0));
            TGCGame.GetSpriteBatch().DrawString(Font, msg, position/escala, Color.White);
            TGCGame.GetSpriteBatch().End();
        }
    }
}