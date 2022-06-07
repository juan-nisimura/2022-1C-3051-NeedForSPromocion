using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src   
{
    public class SpeedoMeter
    {
        private SpriteFont Font;
        private Texture2D Texture;
        private Texture2D Needle;
        private float Speed;

        public void Load(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Textures/" + "speedo_tex");
            Needle = content.Load<Texture2D>("Textures/" + "speedoneedle");
            Font = content.Load<SpriteFont>("SpriteFonts/" + "DS-Digital/DS-Digital");
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
            var escala = 0.5f;
            var escalaTex = 0.3f;
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