using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP.Src;
using TGC.MonoGame.TP.Src.Geometries.Textures;

namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

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

        private Boolean GodModeIsActive { get; set; } = false;
        private KeyController ControllerKeyG { get; set; }
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private CameraObject Camera { get; set; }
        private CarObject Car { get; set; }
        private TankObject Tank { get; set; }
        private BoxObject[] Boxes { get; set; }
        private PowerUpObject[] PowerUps { get; set; }
        private RampObject[] Ramps { get; set; }
        private CylinderObject[] Cylinders { get; set; }
        private MountObject[] Mounts { get; set; }
        private QuadPrimitive Floor { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 500);

            Camera = new CameraObject();

            Car = new CarObject();
            Car.Initialize();

            Tank = new TankObject();
            Tank.Initialize();

            Boxes = new BoxObject[] {
                new BoxObject(GraphicsDevice, new Vector3(705f, 25f, 0f), new Vector3(10f, 50f, 1420f), Color.White),
                new BoxObject(GraphicsDevice, new Vector3(-705f, 25f, 0f), new Vector3(10f, 50f, 1420f), Color.White),
                new BoxObject(GraphicsDevice, new Vector3(0f, 25f, 705f), new Vector3(1400f, 50f, 10f), Color.White),
                new BoxObject(GraphicsDevice, new Vector3(0f, 25f, -705f), new Vector3(1400f, 50f, 10f), Color.White),

                new BoxObject(GraphicsDevice, new Vector3(235f, 29f, 0f), new Vector3(350f, 2f, 80f), Color.Brown),
                new BoxObject(GraphicsDevice, new Vector3(0f, 15f, 0f), new Vector3(120f, 30f, 80f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(-235f, 29f, 0f), new Vector3(350f, 2f, 80f), Color.Brown),

                new BoxObject(GraphicsDevice, new Vector3(550f, 20f, 550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
                new BoxObject(GraphicsDevice, new Vector3(-550f, 20f, 550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
                new BoxObject(GraphicsDevice, new Vector3(-550f, 20f, -550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
                new BoxObject(GraphicsDevice, new Vector3(550f, 20f, -550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
            
                new BoxObject(GraphicsDevice, new Vector3(405f, 45f, 450f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(455f, 45f, 405f), new Vector3(90f, 10f, 10f), Color.Gray),  
                new BoxObject(GraphicsDevice, new Vector3(405f, 45f, 650f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(650f, 45f, 405f), new Vector3(100f, 10f, 10f), Color.Gray),
            
                new BoxObject(GraphicsDevice, new Vector3(-405f, 45f, 450f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(-455f, 45f, 405f), new Vector3(90f, 10f, 10f), Color.Gray),  
                new BoxObject(GraphicsDevice, new Vector3(-405f, 45f, 650f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(-650f, 45f, 405f), new Vector3(100f, 10f, 10f), Color.Gray),
            
                new BoxObject(GraphicsDevice, new Vector3(-405f, 45f, -450f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(-455f, 45f, -405f), new Vector3(90f, 10f, 10f), Color.Gray),  
                new BoxObject(GraphicsDevice, new Vector3(-405f, 45f, -650f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(-650f, 45f, -405f), new Vector3(100f, 10f, 10f), Color.Gray),

                new BoxObject(GraphicsDevice, new Vector3(405f, 45f, -450f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(455f, 45f, -405f), new Vector3(90f, 10f, 10f), Color.Gray),  
                new BoxObject(GraphicsDevice, new Vector3(405f, 45f, -650f), new Vector3(10f, 10f, 100f), Color.Gray),
                new BoxObject(GraphicsDevice, new Vector3(650f, 45f, -405f), new Vector3(100f, 10f, 10f), Color.Gray),           
            };

            PowerUps = new PowerUpObject[] {
                new PowerUpObject(GraphicsDevice, new Vector3(0f,30f,0f)),

                new PowerUpObject(GraphicsDevice, new Vector3(235f,0f,0f)),
                new PowerUpObject(GraphicsDevice, new Vector3(-235f,0f,0f)),

                new PowerUpObject(GraphicsDevice, new Vector3(235f,5f,-400f)),
                new PowerUpObject(GraphicsDevice, new Vector3(-235f,5f,-400f)),
                new PowerUpObject(GraphicsDevice, new Vector3(0f,5f,-250f)),
                
                new PowerUpObject(GraphicsDevice, new Vector3(235f,5f,400f)),
                new PowerUpObject(GraphicsDevice, new Vector3(-235f,5f,400f)),
                new PowerUpObject(GraphicsDevice, new Vector3(0f,5f,250f)),

                new PowerUpObject(GraphicsDevice, new Vector3(550f, 40f, 550f)),
                new PowerUpObject(GraphicsDevice, new Vector3(-550f, 40f, -550f)),
                new PowerUpObject(GraphicsDevice, new Vector3(-550f, 40f, 550f)),
                new PowerUpObject(GraphicsDevice, new Vector3(550f, 40f, -550f))
            };

            Ramps = new RampObject[] {
                new RampObject(GraphicsDevice, new Vector3(370f, 15f, -90f), new Vector3(100f, 30f, 80f), MathF.PI / 2, Color.Yellow),
                new RampObject(GraphicsDevice, new Vector3(-370f, 15f, 90f), new Vector3(100f, 30f, 80f), - MathF.PI / 2, Color.Yellow),

                new RampObject(GraphicsDevice, new Vector3(550f, 20f, 350f), new Vector3(100f, 40f, 100f), MathF.PI / 2, Color.Yellow),
                new RampObject(GraphicsDevice, new Vector3(350f, 20f, 550f), new Vector3(100f, 40f, 100f), MathF.PI, Color.Yellow),

                new RampObject(GraphicsDevice, new Vector3(550f, 20f, -350f), new Vector3(100f, 40f, 100f), - MathF.PI / 2, Color.Yellow),
                new RampObject(GraphicsDevice, new Vector3(350f, 20f, -550f), new Vector3(100f, 40f, 100f), MathF.PI, Color.Yellow),

                new RampObject(GraphicsDevice, new Vector3(-550f, 20f, -350f), new Vector3(100f, 40f, 100f), - MathF.PI / 2, Color.Yellow),
                new RampObject(GraphicsDevice, new Vector3(-350f, 20f, -550f), new Vector3(100f, 40f, 100f), 0, Color.Yellow),

                new RampObject(GraphicsDevice, new Vector3(-550f, 20f, 350f), new Vector3(100f, 40f, 100f), MathF.PI / 2, Color.Yellow),
                new RampObject(GraphicsDevice, new Vector3(-350f, 20f, 550f), new Vector3(100f, 40f, 100f), 0, Color.Yellow),



/*
                new BoxObject(GraphicsDevice, new Vector3(550f, 20f, 550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
                new BoxObject(GraphicsDevice, new Vector3(-550f, 20f, 550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
                new BoxObject(GraphicsDevice, new Vector3(-550f, 20f, -550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
                new BoxObject(GraphicsDevice, new Vector3(550f, 20f, -550f), new Vector3(300f, 40f, 300f), Color.Chocolate),
*/
/*
                new RampObject(GraphicsDevice, new Vector3(0f, 27.5f, 160f), new Vector3(200f, 0f, 50f), MathF.PI, Color.DarkSlateGray),
                new RampObject(GraphicsDevice, new Vector3(-170f, 0f, 160f), new Vector3(140f, 55f, 50f), MathF.PI, Color.DarkGray),
                new RampObject(GraphicsDevice, new Vector3(170f, 0f, 160f), new Vector3(140f, 55f, 50f), 0, Color.DarkGray),
                new RampObject(GraphicsDevice, new Vector3(-50f, 42.5f, 184.5f), new Vector3(100f, 30f, 1f), MathF.PI, Color.OrangeRed),
                new RampObject(GraphicsDevice, new Vector3(50f, 42.5f, 184.5f), new Vector3(100f, 30f, 1f), 0, Color.OrangeRed),
                new RampObject(GraphicsDevice, new Vector3(-50f, 42.5f, 135.5f), new Vector3(100f, 30f, 1f), MathF.PI, Color.OrangeRed),
                new RampObject(GraphicsDevice, new Vector3(50f, 42.5f, 135.5f), new Vector3(100f, 30f, 1f), 0, Color.OrangeRed),
 */         };

            Cylinders = new CylinderObject[]{
                new CylinderObject(GraphicsDevice, new Vector3(150f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new CylinderObject(GraphicsDevice, new Vector3(150f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new CylinderObject(GraphicsDevice, new Vector3(300f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new CylinderObject(GraphicsDevice, new Vector3(300f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new CylinderObject(GraphicsDevice, new Vector3(-150f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new CylinderObject(GraphicsDevice, new Vector3(-150f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new CylinderObject(GraphicsDevice, new Vector3(-300f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new CylinderObject(GraphicsDevice, new Vector3(-300f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
            };
            
            Mounts = new MountObject[]{
                new MountObject(GraphicsDevice, new Vector3(235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                
                new MountObject(GraphicsDevice, new Vector3(-235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(0f,2.5f,-250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(0f,2.5f,250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(-235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
            };

            Floor = new QuadPrimitive(GraphicsDevice);           

            for (int i = 0; i < Boxes.Length; i++)
            {
                Boxes[i].Initialize();
            }

            for (int i = 0; i < PowerUps.Length; i++)
            {
                PowerUps[i].Initialize();
            }

            for (int i = 0; i < Ramps.Length; i++)
            {
                Ramps[i].Initialize();
            }

            for (int i = 0; i < Cylinders.Length; i++)
            {
                Cylinders[i].Initialize();
            }

            for (int i = 0; i < Mounts.Length; i++)
            {
                Mounts[i].Initialize();
            }

            ControllerKeyG = new KeyController(Keys.G);

            Console.WriteLine("Cantidad de objectos = {0}", DefaultObject.ObjectCount);
            
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

            // Cargo el auto
            Car.Load(Content);
            Tank.Load(Content);
            for (int i = 0; i < Boxes.Length; i++)
            {
                Boxes[i].Load(Content);
            }
            for (int i = 0; i < PowerUps.Length; i++)
            {
                PowerUps[i].Load(Content);
            }
            for (int i = 0; i < Ramps.Length; i++)
            {
                Ramps[i].Load(Content);
            }
            for (int i = 0; i < Cylinders.Length; i++)
            {
                Cylinders[i].Load(Content);
            }
            for (int i = 0; i < Mounts.Length; i++)
            {
                Mounts[i].Load(Content);
            }

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de actualizacion del juego.

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            if (ControllerKeyG.Update().IsKeyToPressed()){
                GodModeIsActive = !GodModeIsActive;
            }

            if(GodModeIsActive){
                View = Camera.MoveCameraByKeyboard(gameTime).GetView();
                return;
            }

            Car.Update(gameTime);
            Tank.Update(gameTime);
            for (int i = 0; i < Boxes.Length; i++)
            {
                Boxes[i].Update(gameTime);
            }
            for (int i = 0; i < PowerUps.Length; i++)
            {
                PowerUps[i].Update(gameTime);
            }
            for (int i = 0; i < Ramps.Length; i++)
            {
                Ramps[i].Update(gameTime);
            }
            for (int i = 0; i < Cylinders.Length; i++)
            {
                Cylinders[i].Update(gameTime);
            }
            for (int i = 0; i < Mounts.Length; i++)
            {
                Mounts[i].Update(gameTime);
            }

            View = Camera.FollowCamera(Car.Position).GetView();//  Matrix.CreateLookAt(Car.Position + new Vector3(-100f, 150f, -100f), Car.Position, new Vector3(1f, 1.5f, 1f));

            base.Update(gameTime);
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de renderizado del juego.
            GraphicsDevice.Clear(Color.LightBlue);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.  
            Car.Draw(View, Projection);
            Tank.Draw(View, Projection);
            for (int i = 0; i < Boxes.Length; i++)
            {
                Boxes[i].Draw(View, Projection);
            }
            for (int i = 0; i < PowerUps.Length; i++)
            {
                PowerUps[i].Draw(View, Projection);
            }
            for (int i = 0; i < Ramps.Length; i++)
            {
                Ramps[i].Draw(View, Projection);
            }
            for (int i = 0; i < Cylinders.Length; i++)
            {
                Cylinders[i].Draw(View, Projection);
            }
            for (int i = 0; i < Mounts.Length; i++)
            {
                Mounts[i].Draw(View, Projection);
            }
            Floor.Draw(Matrix.CreateScale(700f), View, Projection);
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
    }
}