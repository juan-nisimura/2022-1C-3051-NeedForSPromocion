using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.MyContentManagers;
using System;

namespace TGC.Monogame.TP.Src.Screens 
{
    public abstract class TextScreen : Screen
    {
        protected Texture2D LeftArrow, RightArrow;

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
            LeftArrow = (Texture2D) MyContentManager.Textures.Load("leftArrow");
            RightArrow = (Texture2D) MyContentManager.Textures.Load("rightArrow");
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

        protected void DrawRightArrow() {
            var W = TGCGame.GetGraphicsDevice().Viewport.Width;
            var H = TGCGame.GetGraphicsDevice().Viewport.Height;
            var escalaTex = new Vector3(0.02f, 0.09f, 1f);// 0.3f;
            Vector2 position = new Vector2((W - 10f) / escalaTex.X - RightArrow.Width, (H - 10f) / escalaTex.Y - RightArrow.Height);
            
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escalaTex) * Matrix.CreateTranslation(0, 0, 0));
            TGCGame.GetSpriteBatch().Draw(RightArrow, position, Color.White);
            TGCGame.GetSpriteBatch().End();

            //DrawCenterTextXY();
            DrawCenterTextY("                         D", 370, 2);
        }

        protected void DrawLeftArrow() {
            var W = TGCGame.GetGraphicsDevice().Viewport.Width;
            var H = TGCGame.GetGraphicsDevice().Viewport.Height;
            var escalaTex = new Vector3(0.02f, 0.09f, 1f);// 0.3f;

            Vector2 position = new Vector2(10f / escalaTex.X, (H - 10f) / escalaTex.Y - LeftArrow.Height );
            
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escalaTex) * Matrix.CreateTranslation(0, 0, 0));
            TGCGame.GetSpriteBatch().Draw(LeftArrow, position, Color.White);
            TGCGame.GetSpriteBatch().End();

            DrawCenterTextY("A                         ", 370, 2);
        }
    }
}