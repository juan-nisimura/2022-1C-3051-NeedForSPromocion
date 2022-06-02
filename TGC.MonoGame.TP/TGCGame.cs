using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP.Src;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Tree;
using TGC.Monogame.TP.Src.CompoundObjects.Missile;
using TGC.Monogame.TP.Src.CompoundObjects.Bridge;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.Monogame.TP.Src.CompoundObjects.Map;
using TGC.Monogame.TP.Src.CompoundObjects.Mount;
using TGC.Monogame.TP.Src.CompoundObjects.Building;
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
        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        private GraphicsDeviceManager Graphics { get; }
        
        private SpriteBatch SpriteBatch { get; set; }

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

        private KeyController ControllerKeyR { get; set; } 

        /*
        private Boolean GodModeIsActive { get; set; } = false;
        private KeyController ControllerKeyG { get; set; }
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private CameraObject Camera { get; set; }
        private PlayerCarObject Car { get; set; }
        private IACarObject IACar { get; set; }

        private BridgeObject Bridge { get; set; }

        private MapWallObject[] MapWalls { get; set; }
        private BuildingsObject Buildings { get; set; }

        private PowerUpObject[] PowerUps { get; set; }
        private MountObject[] Mounts { get; set; }
        private BoostPadObject[] BoostPads { get; set; }
        private TreeObject[] Trees { get; set; }
        private FloorObject Floor { get; set; }
        private Boolean TouchingObject { get; set; }
        private MissileObject[] Missiles { get; set; }
        private BulletObject[] MGBullets {get; set;}
        private List<BulletObject> MGBulletsList {get; set;}
        private BulletObject bullet2 {get;set;}
        private SpherePrimitive Sphere { get; set; }
*/
        private Screen ActiveScreen { get; set; } = MainMenuScreen.GetInstance();

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            /*
            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 500);

            Camera = new CameraObject();
            
            BoostPads = new BoostPadObject[]{
                new BoostPadObject(GraphicsDevice, new Vector3(0,0.1f,575f),new Vector3(22.5f,1f,27.5f), - MathF.PI / 2),
                new BoostPadObject(GraphicsDevice, new Vector3(0,0.1f,525f),new Vector3(22.5f,1f,27.5f), - MathF.PI / 2),

                new BoostPadObject(GraphicsDevice, new Vector3(0,0.1f,-575f),new Vector3(22.5f,1f,27.5f), MathF.PI / 2),
                new BoostPadObject(GraphicsDevice, new Vector3(0,0.1f,-525f),new Vector3(22.5f,1f,27.5f), MathF.PI / 2),

                new BoostPadObject(GraphicsDevice, new Vector3(575,0.1f,0f),new Vector3(22.5f,1f,27.5f), 0),
                new BoostPadObject(GraphicsDevice, new Vector3(525,0.1f,0f),new Vector3(22.5f,1f,27.5f), 0),

                new BoostPadObject(GraphicsDevice, new Vector3(-575,0.1f,0f),new Vector3(22.5f,1f,27.5f), MathF.PI),
                new BoostPadObject(GraphicsDevice, new Vector3(-525,0.1f,0f),new Vector3(22.5f,1f,27.5f), MathF.PI),
            };
            
            Missiles = new MissileObject[] {
                //new MissileObject(GraphicsDevice, new Vector3(-100f, 0f, -100f), 40f),
            };

            
            //bullet2 = new BulletObject(GraphicsDevice,new Vector3(-100f,20f,-150f),10f);


            Floor = new FloorObject(GraphicsDevice, new Vector3(0f,0f,0f),new Vector3(700f,1f,700f),0);           

            //bullet2.Initialize();
            Floor.Initialize();

            ControllerKeyG = new KeyController(Keys.G);
            */
            ControllerKeyR = new KeyController(Keys.R);

            MainMenuScreen.GetInstance().Initialize(GraphicsDevice);
            LevelScreen.GetInstance().Initialize(GraphicsDevice);

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

            /*
            // Cargo los efectos, modelos y texturas
            CarObject.Load(Content);

            TreeObject.Load(Content);
            BoostPadObject.Load(Content, "BoostPadShader");

            MapWallObject.Load(Content, "BoxTextureShader", "large_red_bricks_diff_4k");
            BridgeObject.Load(Content);
            BuildingsObject.Load(Content);
            MissileObject.Load(Content);
            FloorObject.Load(Content, "FloorShader", "brown_mud_leaves_01_diff_4k");
            MountObject.Load(Content);
            PowerUpObject.Load(Content, "BasicShader", "Floor");

            //MGBulletsList = new List<BulletObject>();
            Car = new PlayerCarObject(new Vector3(-100f,0,-100f), Color.Blue);
            Car.Initialize();

            IACar = new IACarObject(new Vector3(-100f,0,-50f), Color.Red);
            IACar.Initialize();

            base.LoadContent();
            
            Mounts = new MountObject[]{
                new MountObject(GraphicsDevice, new Vector3(235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                
                new MountObject(GraphicsDevice, new Vector3(-235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(0f,2.5f,-250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(0f,2.5f,250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(-235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(GraphicsDevice, new Vector3(235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
            };
            
            Trees = new TreeObject[]{
                new TreeObject(GraphicsDevice, new Vector3(100f,0f,400f), 40f),
                new TreeObject(GraphicsDevice, new Vector3(-100f,0f,400f), 40f),
                new TreeObject(GraphicsDevice, new Vector3(100f,0f,-400f), 40f),
                new TreeObject(GraphicsDevice, new Vector3(-100f,0f,-400f), 40f),
                new TreeObject(GraphicsDevice, new Vector3(-300f,0f,-300f), 40f),
                new TreeObject(GraphicsDevice, new Vector3(-300f,0f,300f), 40f),
                new TreeObject(GraphicsDevice, new Vector3(300f,0f,-300f), 40f),
                new TreeObject(GraphicsDevice, new Vector3(300f,0f,300f), 40f),
            };

            MapWalls = new MapWallObject[] {
                new MapWallObject(GraphicsDevice, new Vector3(705f, 32.5f, 0f), new Vector3(10f, 65f, 1420f), Color.White),
                new MapWallObject(GraphicsDevice, new Vector3(-705f, 32.5f, 0f), new Vector3(10f, 65f, 1420f), Color.White),
                new MapWallObject(GraphicsDevice, new Vector3(0f, 32.5f, 705f), new Vector3(1400f, 65f, 10f), Color.White),
                new MapWallObject(GraphicsDevice, new Vector3(0f, 32.5f, -705f), new Vector3(1400f, 65f, 10f), Color.White),

            };

            Buildings = new BuildingsObject(GraphicsDevice);

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
            
            Bridge = new BridgeObject(GraphicsDevice);

            Bridge.Initialize();
            Buildings.Initialize();

            for (int i = 0; i < PowerUps.Length; i++)   PowerUps[i].Initialize();
            for (int i = 0; i < Mounts.Length; i++)     Mounts[i].Initialize();
            for (int i = 0; i < Trees.Length; i++)      Trees[i].Initialize();
            for (int i = 0; i < BoostPads.Length; i++)      BoostPads[i].Initialize();
            for (int i = 0; i < Missiles.Length; i++)       Missiles[i].Initialize();
            for (int i = 0; i < MapWalls.Length; i++)   MapWalls[i].Initialize();

            // Inicializo el HeightMap
            for(int x =-710; x <= 710; x++) {
                for(int z = -710; z <= 710; z++) {
                    HeightMap.SetHeight(x, z, 0);
                    HeightMap.MoveRay(x, z);
                    Bridge.UpdateHeightMap(x, z);
                    Buildings.UpdateHeightMap(x, z);
                    for (int i = 0; i < MapWalls.Length; i++)   MapWalls[i].UpdateHeightMap(x, z);
                    for (int i = 0; i < Mounts.Length; i++)     Mounts[i].UpdateHeightMap(x, z);
                }
            }*/
            base.LoadContent();

            MainMenuScreen.GetInstance().Load(GraphicsDevice, Content);
            LevelScreen.GetInstance().Load(GraphicsDevice, Content);

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
            
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Capturar Input teclado
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Salgo del juego.
                Exit();

            if (ControllerKeyR.Update().IsKeyToPressed()){
                ActiveScreen.Stop();
                if(ActiveScreen == MainMenuScreen.GetInstance())
                    ActiveScreen = LevelScreen.GetInstance();
                else
                    ActiveScreen = MainMenuScreen.GetInstance();
                ActiveScreen.Reset();
                ActiveScreen.Start();
            }

            /*
            if (ControllerKeyG.Update().IsKeyToPressed()){
                GodModeIsActive = !GodModeIsActive;
            }

            if(GodModeIsActive){
                View = Camera.MoveCameraByKeyboard(gameTime).GetView();
                return;
            }

            Car.Update(gameTime, GraphicsDevice, View, Projection);
            IACar.Update(gameTime);
            Floor.Update(gameTime);
            for (int i = 0; i < PowerUps.Length; i++)       PowerUps[i].Update(gameTime, Car);
            for (int i = 0; i < BoostPads.Length; i++)      BoostPads[i].Update(gameTime, Car);
            for (int i = 0; i < Missiles.Length; i++)       Missiles[i].Update(gameTime);

            Buildings.Update(gameTime, Car);
            Bridge.Update(gameTime, Car);
            
            for (int i = 0; i < MapWalls.Length; i++)       MapWalls[i].Update(gameTime, Car);
            for (int i = 0; i < Mounts.Length; i++)         Mounts[i].Update(gameTime, Car);
            for (int i = 0; i < Trees.Length; i++)          Trees[i].Update(gameTime);

            SolveCollisions(gameTime, Car);

            //MGBulletsList = Car.GetMGBulletsList();
            //MGBullets = Car.GetMGBullets();
            //MGBullets = new BulletObject[]{
            //    new BulletObject(GraphicsDevice,new Vector3(-100f,20f,-150f),10f),
            //    new BulletObject(GraphicsDevice,new Vector3(-100f,20f,-200f),10f),
            //    new BulletObject(GraphicsDevice,new Vector3(-100f,20f,-250f),10f)
            //};
            //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Initialize();
            //for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Update(gameTime);
            //bullet2.Update(gameTime);

            
            var keyboardState = Keyboard.GetState();
            //TouchSpeedBoost = Car.ObjectBox.Intersects(SpeedBoost.ObjectBox);
            if (keyboardState.IsKeyDown(Keys.LeftShift)) {
                //Car.SetSpeedBoostActive(true);
                Car.SetSpeedBoostTime();
            }else{
                //Car.SetSpeedBoostActive(false);
            }

            //TouchMachineGunBoost = Car.ObjectBox.Intersects(MachineGun.ObjectBox);
            if (keyboardState.IsKeyDown(Keys.LeftControl)) {
                //Car.SetSpeedBoostActive(true);
                Car.SetMachineGunTime();
            }else{
                //Car.SetSpeedBoostActive(false);
            }

            if (keyboardState.IsKeyDown(Keys.LeftAlt)) {
                //Car.SetSpeedBoostActive(true);
                Car.SetMisileTime();
            }else{
                //Car.SetSpeedBoostActive(false);
            }

            View = Camera.FollowCamera(Car.GetPosition()).GetView();
            */

            ActiveScreen.Update(gameTime, GraphicsDevice);

            base.Update(gameTime);
        }

        /*
        protected void SolveCollisions(GameTime gameTime, CarObject car) {
            var collided = true;
            car.HasCrashed = false;
            if(HeightMap.GetHeight(car.Position.X, car.Position.Z) == 0)
                Car.GroundLevel = 0;
            while(collided){
                collided = false;
                Buildings.SolveHorizontalCollision(gameTime, car);
                Bridge.SolveHorizontalCollision(gameTime, car);
                for (int i = 0; i < Mounts.Length; i++)         collided = collided || Mounts[i].SolveHorizontalCollision(gameTime, car);
                for (int i = 0; i < MapWalls.Length; i++)       collided = collided || MapWalls[i].SolveHorizontalCollision(gameTime, car);
                for (int i = 0; i < Trees.Length; i++)          collided = collided || Trees[i].SolveHorizontalCollision(gameTime, car);
                for (int i = 0; i < Mounts.Length; i++)         collided = collided || Mounts[i].SolveVerticalCollision(gameTime, car);
                for (int i = 0; i < MapWalls.Length; i++)       collided = collided || MapWalls[i].SolveVerticalCollision(gameTime, car);
                Buildings.SolveVerticalCollision(gameTime, car);
                Bridge.SolveVerticalCollision(gameTime, car);
            }
            if(car.HasCrashed)  car.Crash();
        }*/

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logica de renderizado del juego.
            GraphicsDevice.Clear(Color.LightBlue);

            /*
            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.  
            Car.Draw(View, Projection);
            IACar.Draw(View, Projection);
            
            Floor.Draw(View, Projection);
            Bridge.Draw(View, Projection);
            Buildings.Draw(View, Projection);
            for (int i = 0; i < PowerUps.Length; i++)       PowerUps[i].Draw(View, Projection);
            for (int i = 0; i < Mounts.Length; i++)         Mounts[i].Draw(View, Projection);
            for (int i = 0; i < BoostPads.Length; i++)      BoostPads[i].Draw(View, Projection);
            for (int i = 0; i < Trees.Length; i++)          Trees[i].Draw(View, Projection);
            for (int i = 0; i < MapWalls.Length; i++)       MapWalls[i].Draw(View, Projection);
            for (int i = 0; i < Missiles.Length; i++)   Missiles[i].Draw(View, Projection);

            if(Car.GetMGBulletsList()!=null ){Car.GetMGBulletsList().ForEach(bullet => bullet.Draw(View, Projection));}
            if(Car.GetMissileList()!=null ){Car.GetMissileList().ForEach(missile => missile.Draw(View, Projection));}
            //if(MGBullets != null){
            //    for (int i = 0; i < MGBullets.Length; i++)   MGBullets[i].Draw(View, Projection);
            //}
            
            //bullet2.Draw(View, Projection);*/

            ActiveScreen.Draw(gameTime, SpriteBatch, GraphicsDevice);
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
    }
}