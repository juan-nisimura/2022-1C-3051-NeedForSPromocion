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
        private MissileObject[] Missiles { get; set; }

        //post procesing
        private Effect CarEffect { get; set; }
        private Effect BloomEffect { get; set; }
        private Effect BlurEffect { get; set; }
        private Effect BlinnPhongEffect { get; set; }
        private Effect FloorShader { get; set; }
        private Vector3 LightPosition { get; set; }
        private float Timer { get; set; }

        private const int PassCount = 2;

        private RenderTarget2D FirstPassBloomRenderTarget;

        private FullScreenQuad FullScreenQuad;

        private RenderTarget2D MainSceneRenderTarget;

        private RenderTarget2D SecondPassBloomRenderTarget;
        private CylinderPrimitive HealthBar;

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

            HealthBar = new CylinderPrimitive(GraphicsDevice,10,1,32);
            


            Floor = new FloorObject(GraphicsDevice, new Vector3(0f,0f,0f),new Vector3(700f,1f,700f),0);           

            Floor.Initialize();

            ControllerKeyG = new KeyController(Keys.G);
            
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

            //cargo efectos y parametros para blinn phong
           
            BlinnPhongEffect = Content.Load<Effect>(ContentFolderEffects + "BlinnPhong");
            BlinnPhongEffect.Parameters["ambientColor"].SetValue(new Color(1f, 1f, 1f).ToVector3());
            BlinnPhongEffect.Parameters["diffuseColor"].SetValue(new Color(0.5f, 0.5f, 0.5f).ToVector3());
            BlinnPhongEffect.Parameters["specularColor"].SetValue(Color.White.ToVector3());
            BlinnPhongEffect.Parameters["KAmbient"].SetValue(0.3f);
            BlinnPhongEffect.Parameters["KDiffuse"].SetValue(0.5f);
            BlinnPhongEffect.Parameters["KSpecular"].SetValue(0.5f);
            BlinnPhongEffect.Parameters["shininess"].SetValue(3.0f);
            

            /*FloorShader = Content.Load<Effect>(ContentFolderEffects + "FloorShader");
            FloorShader.Parameters["ambientColor"].SetValue(new Color(1f, 1f, 1f).ToVector3());
            FloorShader.Parameters["diffuseColor"].SetValue(new Color(0.5f, 0.5f, 0.5f).ToVector3());
            FloorShader.Parameters["specularColor"].SetValue(Color.White.ToVector3());
            FloorShader.Parameters["KAmbient"].SetValue(0.1f);
            FloorShader.Parameters["KDiffuse"].SetValue(1f);
            FloorShader.Parameters["KSpecular"].SetValue(0.8f);
            FloorShader.Parameters["shininess"].SetValue(2.0f);
            FloorShader.Parameters["lightPosition"].SetValue(LightPosition);*/

            // Cargo los efectos, modelos y texturas
            CarObject.Load(Content);

            TreeObject.Load(Content);
            BoostPadObject.Load(Content, "BoostPadShader");

            MapWallObject.Load(Content, "BoxTextureShader", "large_red_bricks_diff_4k");
            BridgeObject.Load(Content);
            BuildingsObject.Load(Content);
            MissileObject.Load(Content);
            FloorObject.Load(Content, "BlinnPhong", "brown_mud_leaves_01_diff_4k");
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
            }

            //cargo efectos para bloom y blur
            CarEffect = Content.Load<Effect>(ContentFolderEffects + "CarShader");
            BloomEffect = Content.Load<Effect>(ContentFolderEffects + "Bloom");
            BlurEffect = Content.Load<Effect>(ContentFolderEffects + "GaussianBlur");
            BlurEffect.Parameters["screenSize"]
                .SetValue(new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));

            


            // Create a full screen quad to post-process
            FullScreenQuad = new FullScreenQuad(GraphicsDevice);

            // Create render targets. 
            // MainRenderTarget is used to store the scene color
            // BloomRenderTarget is used to store the bloom color and switches with MultipassBloomRenderTarget
            // depending on the pass count, to blur the bloom color
            MainSceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            FirstPassBloomRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            SecondPassBloomRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
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

            var LightPosition = new Vector3((float)Math.Cos(Timer) * 800f, Math.Max(100f, (float)Math.Cos(Timer) * 400f), (float)Math.Sin(Timer) * 800f);
            var eyePosition = new Vector3(Car.Position.X, 50f, Car.Position.Z);
            BlinnPhongEffect.Parameters["lightPosition"].SetValue(LightPosition);
            
            BlinnPhongEffect.Parameters["eyePosition"].SetValue(eyePosition);
            //FloorShader.Parameters["eyePosition"].SetValue(Vector3.Up * 10f);
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

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
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            
            #region Pass 1

            // Use the default blend and depth configuration
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

            // Set the main render target, here we'll draw the base scene
            GraphicsDevice.SetRenderTarget(MainSceneRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

            // Aca deberiamos poner toda la logica de renderizado del juego.
            GraphicsDevice.Clear(Color.LightBlue);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            BlinnPhongEffect.CurrentTechnique = BlinnPhongEffect.Techniques["CarColorDrawing"];
            Car.DrawBlinnPhong(BlinnPhongEffect, View, Projection);
            //IACar.Draw(CarEffect ,View, Projection);
            BlinnPhongEffect.CurrentTechnique = BlinnPhongEffect.Techniques["FloorColorDrawing"];
            Floor.DrawBlinnPhong(BlinnPhongEffect, View, Projection);
            Bridge.DrawBlinnPhong(BlinnPhongEffect,View, Projection);
            Buildings.Draw(View, Projection);
            for (int i = 0; i < PowerUps.Length; i++)       PowerUps[i].Draw(View, Projection);
            for (int i = 0; i < Mounts.Length; i++)         Mounts[i].Draw(View, Projection);
            for (int i = 0; i < BoostPads.Length; i++)      BoostPads[i].Draw(View, Projection);
            for (int i = 0; i < Trees.Length; i++)          Trees[i].Draw(View, Projection);
            for (int i = 0; i < MapWalls.Length; i++)       MapWalls[i].Draw(View, Projection);
            for (int i = 0; i < Missiles.Length; i++)   Missiles[i].Draw(View, Projection);

            if(Car.GetMGBulletsList()!=null ){Car.GetMGBulletsList().ForEach(bullet => bullet.Draw(View, Projection));}
            if(Car.GetMissileList()!=null ){Car.GetMissileList().ForEach(missile => missile.Draw(View, Projection));}
            #endregion

            #region Pass 2
            // Set the render target as our bloomRenderTarget, we are drawing the bloom color into this texture
            GraphicsDevice.SetRenderTarget(FirstPassBloomRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

            

            Car.DrawBloom(BloomEffect, View, Projection);

            #endregion

            #region Final Pass
            //no blur pass

            // Set the depth configuration as none, as we don't use depth in this pass
            GraphicsDevice.DepthStencilState = DepthStencilState.None;

            // Set the render target as null, we are drawing into the screen now!
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            // Set the technique to our blur technique
            // Then draw a texture into a full-screen quad
            // using our rendertarget as texture
            BloomEffect.CurrentTechnique = BloomEffect.Techniques["Integrate"];
            BloomEffect.Parameters["baseTexture"].SetValue(MainSceneRenderTarget);
            BloomEffect.Parameters["bloomTexture"].SetValue(FirstPassBloomRenderTarget);
            FullScreenQuad.Draw(BloomEffect);

            #endregion
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
            FullScreenQuad.Dispose();
            FirstPassBloomRenderTarget.Dispose();
            MainSceneRenderTarget.Dispose();
            SecondPassBloomRenderTarget.Dispose();
        }
    }
}