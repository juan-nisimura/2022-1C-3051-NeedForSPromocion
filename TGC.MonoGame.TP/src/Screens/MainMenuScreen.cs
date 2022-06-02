using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class MainMenuScreen : Screen
    {
        public static MainMenuScreen Instance { get; set; } = new MainMenuScreen();
        private SpriteFont Font { get; set; }
        private string Instructions { get; set; }
        private Vector2 InstructionsSize { get; set; }

        public static Screen GetInstance() {
            return Instance;
        }

        public override void Initialize(GraphicsDevice graphicsDevice) {
            Instructions = "Y = Play, U = Pause, I = Resume, O = Stop.";
        }

        public override void Start() {
            MediaPlayer.Play(Song);
        }

        public override void Stop() {
            MediaPlayer.Stop();
        }

        public override void Reset(){
            
        }

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content) {
            Song = content.Load<Song>(ContentFolderMusic + "Main Menu");
            MediaPlayer.IsRepeating = true;
            Font = content.Load<SpriteFont>(ContentFolderSpriteFonts + "CascadiaCode/CascadiaCodePL");
            InstructionsSize = Font.MeasureString(Instructions);
            Reset();
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice) {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            spriteBatch.Begin();

            var songNamePosition = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2f, 20) -
                                   Font.MeasureString("Main Menu") / 2;
            spriteBatch.DrawString(Font, "Playing: " + "Main Menu", songNamePosition, Color.DarkMagenta);
            var instructionsPosition = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2f, 60) -
                                       InstructionsSize / 2;
            spriteBatch.DrawString(Font, Instructions, instructionsPosition, Color.DarkGreen);

            spriteBatch.End();

            DrawCenterTextY("Need For Spromotion", 100, 5, spriteBatch, graphicsDevice);
            DrawCenterTextY("Left y Right -> girar", 300, 1, spriteBatch, graphicsDevice);
            DrawCenterTextY("SpaceBar -> pausa", 400, 1, spriteBatch, graphicsDevice);
            DrawCenterTextY("G -> Modo God", 500, 1, spriteBatch, graphicsDevice);
            DrawCenterTextY("Presione SPACE para comenzar", 600, 1, spriteBatch, graphicsDevice);
        }

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