using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.Screens 
{
    public abstract class TextScreen : Screen
    {
        public override void Draw()
        {
            LevelScreen.GetInstance().Draw();
            DrawText();
        }
        
        public override void Load(ContentManager content) 
        {
            Song = content.Load<Song>(ContentFolderMusic + SongName());
            MediaPlayer.IsRepeating = true;
            Font = content.Load<SpriteFont>(ContentFolderSpriteFonts + FontName());
        }

        public override void Update(){
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

        public abstract void DrawText();

        public void DrawCenterTextY(string msg, float Y, float escala)
        {
            var W = TGCGame.GetGraphicsDevice().Viewport.Width;
            var H = TGCGame.GetGraphicsDevice().Viewport.Height;
            var size = Font.MeasureString(msg) * escala;
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - size.X) / 2, Y, 0));
            TGCGame.GetSpriteBatch().DrawString(Font, msg, new Vector2(0, 0), Color.YellowGreen);
            TGCGame.GetSpriteBatch().End();
        }
    }
}