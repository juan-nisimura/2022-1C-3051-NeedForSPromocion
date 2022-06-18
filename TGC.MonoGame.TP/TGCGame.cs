using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP.Src;
using TGC.Monogame.TP.Src.MyContentManagers;
using TGC.Monogame.TP.Src.Screens;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        public static float ElapsedTime;
        public const int PLAYERS_QUANTITY = 4;

        private GraphicsDeviceManager Graphics { get; }
        private static SpriteBatch SpriteBatch { get; set; }
        private static GraphicsDevice StaticGraphicsDevice;

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            // Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        public static KeyController ControllerKeyP { get; set; } 

        public static KeyController ControllerKeyEnter { get; set; }

        public static Screen ActiveScreen { get; set; } = MainMenuScreen.GetInstance();

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            StaticGraphicsDevice = GraphicsDevice;
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            ControllerKeyP = new KeyController(Keys.P);
            ControllerKeyEnter = new KeyController(Keys.Enter);

            MainMenuScreen.GetInstance().Initialize();
            LevelScreen.GetInstance().Initialize();
            TimeOutScreen.GetInstance().Initialize();
            WinScreen.GetInstance().Initialize();
            LoseScreen.GetInstance().Initialize();
            PauseScreen.GetInstance().Initialize();

            base.Initialize();
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();

            MyContentManager.SetContentManager(Content);

            MainMenuScreen.GetInstance().Load();
            LevelScreen.GetInstance().Load();
            TimeOutScreen.GetInstance().Load();
            WinScreen.GetInstance().Load();
            LoseScreen.GetInstance().Load();
            PauseScreen.GetInstance().Load();
            
            ActiveScreen.Start();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.
            
            ElapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            ActiveScreen.Update();

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de renderizado del juego.

            ActiveScreen.Draw();
            base.Draw(gameTime);
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }

        public static void SwitchActiveScreen(Func<Screen> screenFunction) {
            ActiveScreen.Stop();
            ActiveScreen = screenFunction();
            //ActiveScreen.Reset();
            ActiveScreen.Start();
        }

        public static float GetElapsedTime() {
            return ElapsedTime;
        }
        public static GraphicsDevice GetGraphicsDevice() {
            return StaticGraphicsDevice;
        }
        public static SpriteBatch GetSpriteBatch() {
            return SpriteBatch;
        }
        
    }
}