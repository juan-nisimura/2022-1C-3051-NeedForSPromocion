using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public abstract class TextScreen : Screen
    {
        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            LevelScreen.GetInstance().Draw(spriteBatch, graphicsDevice);
            DrawText(spriteBatch, graphicsDevice);
        }
        
        public override void Load(GraphicsDevice graphicsDevice, ContentManager content) 
        {
            Song = content.Load<Song>(ContentFolderMusic + SongName());
            MediaPlayer.IsRepeating = true;
            Font = content.Load<SpriteFont>(ContentFolderSpriteFonts + FontName());
        }

        public override void Update(GraphicsDevice graphicsDevice){
            if (TGCGame.ControllerKeyEnter.Update().IsKeyToPressed()){
                LevelScreen.GetInstance().Reset();
                TGCGame.SwitchActiveScreen(() => LevelScreen.GetInstance());
            }
        }
        public override void Start() {
            MediaPlayer.Play(Song);
        }

        public override void Stop() {
            MediaPlayer.Stop();
        }

        public override void Reset(){

        }

        public abstract void DrawText(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);

        public void DrawCenterTextY(string msg, float Y, float escala, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            var W = graphicsDevice.Viewport.Width;
            var H = graphicsDevice.Viewport.Height;
            var size = Font.MeasureString(msg) * escala;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - size.X) / 2, Y, 0));
            spriteBatch.DrawString(Font, msg, new Vector2(0, 0), Color.YellowGreen);
            spriteBatch.End();
        }
    }
}