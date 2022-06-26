using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.MyContentManagers;

namespace TGC.Monogame.TP.Src.Screens 
{
    public abstract class TextScreen : Screen
    {
        public override void Draw()
        {
            #region Pass 1

            // Set the main render target as our render target
            TGCGame.GetGraphicsDevice().SetRenderTarget(LevelScreen.GetLevelScreenInstance().BlurMainRenderTarget);
            TGCGame.GetGraphicsDevice().Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);
            LevelScreen.GetInstance().Draw();

            #endregion
            #region Pass 2

            // Set the depth configuration as none, as we don't use depth in this pass
            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.None;

            // Set the render target as null, we are drawing into the screen now!
            TGCGame.GetGraphicsDevice().SetRenderTarget(null);
            TGCGame.GetGraphicsDevice().Clear(Color.Black);

            // Set the technique to our blur technique
            // Then draw a texture into a full-screen quad
            // using our rendertarget as texture

            LevelScreen.GetLevelScreenInstance().BlurEffect.CurrentTechnique = LevelScreen.GetLevelScreenInstance().BlurEffect.Techniques["Blur"];
            LevelScreen.GetLevelScreenInstance().BlurEffect.Parameters["baseTexture"].SetValue(LevelScreen.GetLevelScreenInstance().BlurMainRenderTarget);
            LevelScreen.GetLevelScreenInstance().FullScreenQuad.Draw(LevelScreen.GetLevelScreenInstance().BlurEffect);
            
            DrawText();
            #endregion
        }
        
        public override void Load() 
        {
            Song = MyContentManager.Songs.Load(SongName());
            MediaPlayer.IsRepeating = true;
            Font = MyContentManager.SpriteFonts.Load(FontName());
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
            TGCGame.GetSpriteBatch().DrawString(Font, msg, new Vector2(0, 0), Color.White);
            TGCGame.GetSpriteBatch().End();
        }
    }
}