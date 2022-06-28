using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Tree;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.CompoundObjects.Bridge;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.Monogame.TP.Src.CompoundObjects.Map;
using TGC.Monogame.TP.Src.CompoundObjects.Mount;
using TGC.Monogame.TP.Src.CompoundObjects.Building;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.PowerUpObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.HUD;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels;
using TGC.Monogame.TP.Src.PowerUpObjects.PowerUps;
using TGC.Monogame.TP.Src.MyContentManagers;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.MonoGame.Samples.Cameras;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class LevelScreen : Screen
    {        
        protected override String SongName() { return "Riders On The Storm Fredwreck Remix"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new LevelScreen();
        public static Screen GetInstance() { return Instance; }
        public static LevelScreen GetLevelScreenInstance() { return (LevelScreen) Instance; }
        private Boolean GodModeIsActive { get; set; } = false;
        private KeyController ControllerKeyG { get; set; }
        private float Rotation { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }
        private CameraObject Camera { get; set; }
        public CarObject[] AllCars { get; set; }
        public PlayerCarObject Car { get; set; }
        public IACarObject[] IACars { get; set; }

        private BridgeObject Bridge { get; set; }

        private MapWallObject[] MapWalls { get; set; }
        private BuildingsObject Buildings { get; set; }

        private PowerUpObject[] PowerUps { get; set; }
        private MountObject[] Mounts { get; set; }
        private BoostPadObject[] BoostPads { get; set; }
        private TreeObject[] Trees { get; set; }
        private FloorObject Floor { get; set; }
        public Clock Clock = new Clock();
        private SpeedoMeter SpeedoMeter = new SpeedoMeter();
        private Boolean isStart { get; set; } = false;
        private float Timer { get; set; }

        //blur
        private FullScreenQuad BlurFullScreenQuad;
        private RenderTarget2D BlurHorizontalRenderTarget;
        public RenderTarget2D BlurMainRenderTarget;
        public Effect BlurEffect { get; set; }

        //enviroment map 
        private const int EnvironmentmapSize = 2048;
        private Effect EnviromentMap { get; set; }
        private RenderTargetCube EnvironmentMapRenderTarget { get; set; }
        private StaticCamera CubeMapCamera { get; set; }

        //faros-blinnphong
        private Effect Lights { get; set; }

        //blur
        private Effect Bloom { get; set; }
        private const int PassCount = 2;
        private RenderTarget2D BloomFirstPassRenderTarget;

        private FullScreenQuad BloomFullScreenQuad;

        private RenderTarget2D BloomMainSceneRenderTarget;

        private RenderTarget2D BloomSecondPassRenderTarget;

        //fixes
        Effect CarEffect { get; set; }

        public override void Initialize() {

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, TGCGame.GetGraphicsDevice().Viewport.AspectRatio, 1, 500);
            
            Camera = new CameraObject();

            CubeMapCamera = new StaticCamera(1f, new Vector3(-350f, 10f, -350f), Vector3.UnitX, Vector3.Up);
            CubeMapCamera.BuildProjection(1f, 50f, 400f, MathHelper.PiOver2);

            BoostPads = new BoostPadObject[]{
                new BoostPadObject(new Vector3(0,0.1f,575f),new Vector3(22.5f,1f,27.5f), - MathF.PI / 2),
                new BoostPadObject(new Vector3(0,0.1f,525f),new Vector3(22.5f,1f,27.5f), - MathF.PI / 2),

                new BoostPadObject(new Vector3(0,0.1f,-575f),new Vector3(22.5f,1f,27.5f), MathF.PI / 2),
                new BoostPadObject(new Vector3(0,0.1f,-525f),new Vector3(22.5f,1f,27.5f), MathF.PI / 2),

                new BoostPadObject(new Vector3(575,0.1f,0f),new Vector3(22.5f,1f,27.5f), 0),
                new BoostPadObject(new Vector3(525,0.1f,0f),new Vector3(22.5f,1f,27.5f), 0),

                new BoostPadObject(new Vector3(-575,0.1f,0f),new Vector3(22.5f,1f,27.5f), MathF.PI),
                new BoostPadObject(new Vector3(-525,0.1f,0f),new Vector3(22.5f,1f,27.5f), MathF.PI),
            };
            
            Floor = new FloorObject(new Vector3(0f,0f,0f),new Vector3(700f,1f,700f),0);           
            Floor.Initialize();

            ControllerKeyG = new KeyController(Keys.G);
        }

        public override void Start() {
            MediaPlayer.Play(Song);
            Car.Start();
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)   IACars[i].Start();
        }

        public override void Stop() {
            MediaPlayer.Stop();
            Car.Stop();
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)   IACars[i].Stop();
        }

        public override void Reset(){
            Clock.Reset();
            Car.Reset();
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)   IACars[i].Reset();
            for (int i = 0; i < PowerUps.Length; i++)               PowerUps[i].Reset();
        }

        public override void Load() {
            //careffect
            CarEffect = MyContentManager.Effects.Load("CarShader");
            //enviromentMap
            EnvironmentMapRenderTarget = new RenderTargetCube(TGCGame.GetGraphicsDevice(), EnvironmentmapSize, false,
                                         SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
            TGCGame.GetGraphicsDevice().BlendState = BlendState.Opaque;

            //carLigths
            Lights = MyContentManager.Effects.Load("CarLights");
            Lights.Parameters["ambientColor"].SetValue(Color.White.ToVector3());
            Lights.Parameters["diffuseColor"].SetValue(Color.White.ToVector3());
            Lights.Parameters["specularColor"].SetValue(Color.White.ToVector3());
            Lights.Parameters["KAmbient"].SetValue(0.5f);
            Lights.Parameters["KDiffuse"].SetValue(0.0f);
            Lights.Parameters["KSpecular"].SetValue(0.5f);
            Lights.Parameters["shininess"].SetValue(2.0f);

            //Bloom
            Bloom = MyContentManager.Effects.Load("Bloom");
            BloomMainSceneRenderTarget = new RenderTarget2D(TGCGame.GetGraphicsDevice(), TGCGame.GetGraphicsDevice().Viewport.Width,
                TGCGame.GetGraphicsDevice().Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            BloomFirstPassRenderTarget = new RenderTarget2D(TGCGame.GetGraphicsDevice(), TGCGame.GetGraphicsDevice().Viewport.Width,
                TGCGame.GetGraphicsDevice().Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            BloomSecondPassRenderTarget = new RenderTarget2D(TGCGame.GetGraphicsDevice(), TGCGame.GetGraphicsDevice().Viewport.Width,
                TGCGame.GetGraphicsDevice().Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);

            //Blur
            BlurEffect = MyContentManager.Effects.Load("GaussianBlur");
            // Create a full screen quad to post-process
            FullScreenQuad = new FullScreenQuad(TGCGame.GetGraphicsDevice());
            // Create render targets. One can be used for simple gaussian blur
            // mainRenderTarget is also used as a render target in the separated filter
            // horizontalRenderTarget is used as the horizontal render target in the separated filter
            BlurMainRenderTarget = new RenderTarget2D(TGCGame.GetGraphicsDevice(), TGCGame.GetGraphicsDevice().Viewport.Width,
                TGCGame.GetGraphicsDevice().Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            BlurHorizontalRenderTarget = new RenderTarget2D(TGCGame.GetGraphicsDevice(), TGCGame.GetGraphicsDevice().Viewport.Width,
                TGCGame.GetGraphicsDevice().Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);

            BlurEffect.Parameters["screenSize"]
                .SetValue(new Vector2(TGCGame.GetGraphicsDevice().Viewport.Width, TGCGame.GetGraphicsDevice().Viewport.Height));


            Font = MyContentManager.SpriteFonts.Load("CascadiaCode/CascadiaCodePL");
            Song = MyContentManager.Songs.Load(SongName());
            MediaPlayer.IsRepeating = true;

            // Cargo los efectos, modelos y texturas
            CarObject.Load();

            TreeObject.Load();
            BoostPadObject.Load("BoostPadShader");
            HealthBarObject.Load("HealthBarShader");

            MapWallObject.Load("BoxTextureShader", "large_red_bricks_diff_4k");
            BridgeObject.Load();
            BuildingsObject.Load();
            MissileObject.Load();
            BulletObject.Load();
            FloorObject.Load("FloorShader", "brown_mud_leaves_01_diff_4k");
            MountObject.Load();
            PowerUpObject.Load("BasicShader", "Floor");
            NullPowerUp.Load();
            PowerUpModel.Load();
            SpeedBoostPowerUpModel.Load();
            Clock.Load();
            SpeedoMeter.Load();
            
            

            
            Mounts = new MountObject[]{
                new MountObject(new Vector3(235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                
                new MountObject(new Vector3(-235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(new Vector3(0f,2.5f,-250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(new Vector3(0f,2.5f,250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(new Vector3(-235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(new Vector3(235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
            };
            
            Trees = new TreeObject[]{
                new TreeObject(new Vector3(100f,0f,400f), 40f),
                new TreeObject(new Vector3(-100f,0f,400f), 40f),
                new TreeObject(new Vector3(100f,0f,-400f), 40f),
                new TreeObject(new Vector3(-100f,0f,-400f), 40f),
                new TreeObject(new Vector3(-300f,0f,-300f), 40f),
                new TreeObject(new Vector3(-300f,0f,300f), 40f),
                new TreeObject(new Vector3(300f,0f,-300f), 40f),
                new TreeObject(new Vector3(300f,0f,300f), 40f),
            };

            MapWalls = new MapWallObject[] {
                new MapWallObject(new Vector3(705f, 32.5f, 0f), new Vector3(10f, 65f, 1420f), Color.White),
                new MapWallObject(new Vector3(-705f, 32.5f, 0f), new Vector3(10f, 65f, 1420f), Color.White),
                new MapWallObject(new Vector3(0f, 32.5f, 705f), new Vector3(1400f, 65f, 10f), Color.White),
                new MapWallObject(new Vector3(0f, 32.5f, -705f), new Vector3(1400f, 65f, 10f), Color.White),
            };

            Buildings = new BuildingsObject(Floor.IAMapBox);

            PowerUps = new PowerUpObject[] {
                new PowerUpObject(new Vector3(0f,30f,0f)),

                new PowerUpObject(new Vector3(235f,0f,0f)),
                new PowerUpObject(new Vector3(-235f,0f,0f)),

                new PowerUpObject(new Vector3(235f,5f,-400f)),
                new PowerUpObject(new Vector3(-235f,5f,-400f)),
                new PowerUpObject(new Vector3(0f,5f,-250f)),
                
                new PowerUpObject(new Vector3(235f,5f,400f)),
                new PowerUpObject(new Vector3(-235f,5f,400f)),
                new PowerUpObject(new Vector3(0f,5f,250f)),

                new PowerUpObject(new Vector3(550f, 40f, 550f)),
                new PowerUpObject(new Vector3(-550f, 40f, -550f)),
                new PowerUpObject(new Vector3(-550f, 40f, 550f)),
                new PowerUpObject(new Vector3(550f, 40f, -550f))
            };
            
            Bridge = new BridgeObject(Floor.IAMapBox);
            Bridge.Initialize();
            Buildings.Initialize();

            for (int i = 0; i < PowerUps.Length; i++)   PowerUps[i].Initialize();
            for (int i = 0; i < Mounts.Length; i++)     Mounts[i].Initialize();
            for (int i = 0; i < Trees.Length; i++)      Trees[i].Initialize();
            for (int i = 0; i < BoostPads.Length; i++)      BoostPads[i].Initialize();
            for (int i = 0; i < MapWalls.Length; i++)   MapWalls[i].Initialize();

            var CarsPositions = new Vector3[] {
                new Vector3(-350f, 0f, -350f),
                new Vector3(350f, 0f, -350f),
                new Vector3(-350f, 0f, 350f),
                new Vector3(350f, 0f, 350f),
            };

            Car = new PlayerCarObject(CarsPositions[0], Color.Blue);
            IACars = new IACarObject[TGCGame.PLAYERS_QUANTITY - 1];
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)   
                IACars[i] = new IACarObject(CarsPositions[i+1], Color.Red, PowerUps);

            AllCars = new CarObject[TGCGame.PLAYERS_QUANTITY];
            AllCars[0] = Car;
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)   
                AllCars[i + 1] = IACars[i];

            Car.Initialize(IACars);
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++) {
                var enemyCars = new CarObject[TGCGame.PLAYERS_QUANTITY - 1];
                for(int j = 0; j < TGCGame.PLAYERS_QUANTITY - 1; j++){
                    if(j != i)
                        enemyCars[j] = IACars[j];
                    else
                        enemyCars[j] = Car;
                }
                IACars[i].Initialize(enemyCars);
            }

            // Inicializo el HeightMap
            for(int level = 0; level <= 1; level++){
                for(int x =-710; x <= 710; x++) {
                    for(int z = -710; z <= 710; z++) {
                        HeightMap.SetHeight(x, z, 0, level);
                        HeightMap.MoveRay(x, z);
                        Bridge.UpdateHeightMap(x, z, level);
                        Buildings.UpdateHeightMap(x, z, level);
                        for (int i = 0; i < MapWalls.Length; i++)   MapWalls[i].UpdateHeightMap(x, z, level);
                        for (int i = 0; i < Mounts.Length; i++)     Mounts[i].UpdateHeightMap(x, z, level);
                        for (int i = 0; i < Trees.Length; i++)      Trees[i].UpdateHeightMap(x, z, level);
                    }
                }
            }

            // Inicializo el IALogicalMap
            for(int level = 0; level <= 1; level++){
                for(int x =-710; x <= 710; x++) {
                    for(int z = -710; z <= 710; z++) {
                        IALogicalMap.SetIAMapBox(x, z, Floor.IAMapBox, level);
                        IALogicalMap.MoveRay(x, z);
                        Bridge.UpdateIALogicalMap(x, z, level);
                        Buildings.UpdateIALogicalMap(x, z, level);
                        //for (int i = 0; i < MapWalls.Length; i++)   MapWalls[i].UpdateIALogicalMap(x, z, level);
                        //for (int i = 0; i < Mounts.Length; i++)     Mounts[i].UpdateIALogicalMap(x, z, level);
                        //for (int i = 0; i < Trees.Length; i++)      Trees[i].UpdateIALogicalMap(x, z, level);
                    }
                }
            }


            Song = MyContentManager.Songs.Load("Riders On The Storm Fredwreck Remix");
            MediaPlayer.IsRepeating = true;
            Reset();
        }

        public void UpdateMainMenu() {
            Timer -= TGCGame.GetElapsedTime();
            IACars[0].Update();
            Floor.Update();
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Update(AllCars);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Update(Car);

            for (int i = 0; i < Trees.Length; i++) Trees[i].Update();
            Camera.FollowCamera(new Vector3((float)Math.Cos(Timer) * 200f, 50f, (float)Math.Sin(Timer) * 200f));
            //View = Camera.FollowCamera(new Vector3((float)Math.Cos(Timer) * 200f, 50f, (float)Math.Sin(Timer) * 200f)).GetView();
            var radius = 600f+ Math.Max(0,(float)Math.Cos(Timer * 4 - MathHelper.PiOver2 * 2) *150f);
            var b = (float)Math.Sqrt(Timer);
            View = Matrix.CreateLookAt(
                //new Vector3((float)Math.Cos(Timer)* radius, 50f, (float)Math.Sin(Timer) * Timer * radius),
                new Vector3((float)Math.Cos(Timer) * radius, Math.Max(10 ,(float)Math.Cos(Timer * 4-MathHelper.PiOver2*2) * 100), (float)Math.Sin(Timer) * radius),
                new Vector3((float)Math.Cos(Timer-MathHelper.PiOver4) * radius, 25f, (float)Math.Sin(Timer - MathHelper.PiOver4) * radius),
                new Vector3(0.1f, 1f, 0f));
            IACars[0].Position= new Vector3((float)Math.Cos(Timer - 0.1) * radius, Math.Max(2, (float)Math.Cos(Timer * 4 - MathHelper.PiOver2 * 2) * 100), (float)Math.Sin(Timer - 0.1) * radius);
            IACars[0].Rotation = -Timer-MathHelper.Pi;
        }

        public override void Update() {
            Clock.Update();
            
            // Si termina el juego de alguna forma, no hace los demás updates
            if (Clock.NoTimeLeft())
            {
                TGCGame.SwitchActiveScreen(() => TimeOutScreen.GetInstance());
            }

            if (Car.IsDead())
            {
                TGCGame.SwitchActiveScreen(() => LoseScreen.GetInstance());
            }

            var Win = true;
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)   
                Win = Win && IACars[i].IsDead();

            if(Win)
                TGCGame.SwitchActiveScreen(() => WinScreen.GetInstance());

            if (TGCGame.ControllerKeyP.Update().IsKeyToPressed())
                TGCGame.SwitchActiveScreen(() => PauseControlsScreen.GetInstance());

            if (ControllerKeyG.Update().IsKeyToPressed())
                GodModeIsActive = !GodModeIsActive;

            if (GodModeIsActive)
            {
                View = Camera.MoveCameraByKeyboard().GetView();
                return;
            }

            Car.Update(View, Projection);
            CubeMapCamera.Position = new Vector3(Car.Position.X-5f, Car.Position.Y+25f, Car.Position.Z-5f);
            for (int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)   IACars[i].Update();
            
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Update(AllCars);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Update(Car);
            for (int i = 0; i < BoostPads.Length; i++){
                for(int j = 0; j < TGCGame.PLAYERS_QUANTITY - 1; j++)   BoostPads[i].Update(IACars[j]);
            } 

            SpeedoMeter.Update(Car.Speed);

            View = Camera.FollowCamera(Car.GetPosition()).GetView();

            SolveCollisions(Car);
            for(int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++){
                SolveCollisions(IACars[i]);
            }
        }

        protected void SolveCollisions(CarObject car) {
            var collided = true;
            car.HasCrashed = false;
            if(HeightMap.GetHeight(car.Position.X, car.Position.Z, HeightMap.GetActualLevel(car.Position.Y)) == 0)
                car.GroundLevel = 0;
            while(collided){
                collided = false;
                for (int i = 0; i < Mounts.Length; i++)         collided = collided || Mounts[i].SolveVerticalCollision(car);
                for (int i = 0; i < MapWalls.Length; i++)       collided = collided || MapWalls[i].SolveVerticalCollision(car);
                Buildings.SolveVerticalCollision(car);
                Bridge.SolveVerticalCollision(car);
                Buildings.SolveHorizontalCollision(car);
                Bridge.SolveHorizontalCollision(car);
                for (int i = 0; i < Mounts.Length; i++)         collided = collided || Mounts[i].SolveHorizontalCollision(car);
                for (int i = 0; i < MapWalls.Length; i++)       collided = collided || MapWalls[i].SolveHorizontalCollision(car);
                for (int i = 0; i < Trees.Length; i++)          collided = collided || Trees[i].SolveHorizontalCollision(car);
            }
            for (int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)  collided = collided || car.Enemies[i].SolveHorizontalCollision(car);
            if(car.HasCrashed)  car.Crash();
            SolveBulletsCollisions(car);
            SolveMissilesCollisions(car);
        }

        protected void SolveBulletsCollisions(CarObject car) {
            for(int b = 0; b < CarObject.BULLETS_POOL_SIZE; b++) {
                if(car.BulletsPool[b].IsActive){
                    Floor.SolveBulletCollision(car.BulletsPool[b]);
                    Buildings.SolveBulletCollision(car.BulletsPool[b]);
                    Bridge.SolveBulletCollision(car.BulletsPool[b]);
                    for (int i = 0; i < Mounts.Length; i++)     Mounts[i].SolveBulletCollision(car.BulletsPool[b]);
                    for (int i = 0; i < MapWalls.Length; i++)   MapWalls[i].SolveBulletCollision(car.BulletsPool[b]);
                    for (int i = 0; i < Trees.Length; i++)      Trees[i].SolveBulletCollision(car.BulletsPool[b]);
                }
            }
        }

        protected void SolveMissilesCollisions(CarObject car) {
            for(int m = 0; m < CarObject.MISSILES_POOL_SIZE; m++) {
                if(car.MissilesPool[m].IsActive){
                    Floor.SolveMissileCollision(car.MissilesPool[m]);
                    Buildings.SolveMissileCollision(car.MissilesPool[m]);
                    Bridge.SolveMissileCollision(car.MissilesPool[m]);
                    for (int i = 0; i < Mounts.Length; i++)     Mounts[i].SolveMissileCollision(car.MissilesPool[m]);
                    for (int i = 0; i < MapWalls.Length; i++)   MapWalls[i].SolveMissileCollision(car.MissilesPool[m]);
                    for (int i = 0; i < Trees.Length; i++)      Trees[i].SolveMissileCollision(car.MissilesPool[m]);
                }
            }
        }

        public override void Draw()
        {
            //enviromentMap
            #region Pass 1-6

            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.Default;
            // Draw to our cubemap from the robot position
            for (var face = CubeMapFace.PositiveX; face <= CubeMapFace.NegativeZ; face++)
            {

                // Set the render target as our cubemap face, we are drawing the scene in this texture
                TGCGame.GetGraphicsDevice().SetRenderTarget(EnvironmentMapRenderTarget, face);
                TGCGame.GetGraphicsDevice().Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                SetCubemapCameraForOrientation(face);
                CubeMapCamera.BuildView();

                // Draw our scene. Do not draw our tank as it would be occluded by itself 
                // (if it has backface culling on)
                //Floor.Draw(View, Projection);
                Bridge.Draw(CubeMapCamera.View, CubeMapCamera.Projection);
                Buildings.Draw(CubeMapCamera.View, CubeMapCamera.Projection);
                for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Draw(CubeMapCamera.View, CubeMapCamera.Projection);
                //for (int i = 0; i < Mounts.Length; i++) Mounts[i].Draw(CubeMapCamera.View, CubeMapCamera.Projection);
                for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Draw(CubeMapCamera.View, CubeMapCamera.Projection);
                for (int i = 0; i < Trees.Length; i++) Trees[i].Draw(CubeMapCamera.View, CubeMapCamera.Projection);
                for (int i = 0; i < MapWalls.Length; i++) MapWalls[i].Draw(CubeMapCamera.View, CubeMapCamera.Projection);
            }

            #endregion

            #region clear 
            TGCGame.GetGraphicsDevice().SetRenderTarget(null);
            TGCGame.GetGraphicsDevice().Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);
            #endregion

            #region DrawScene
            TGCGame.GetGraphicsDevice().Clear(Color.LightBlue);
            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.Default;
            // Use the default blend and depth configuration
            //TGCGame.GetGraphicsDevice().BlendState = BlendState.Opaque;
            TGCGame.GetGraphicsDevice().BlendState = BlendState.AlphaBlend;
            TGCGame.GetGraphicsDevice().SetRenderTarget(BloomMainSceneRenderTarget);
            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.  



            Vector3 cameraPosition = new Vector3(Camera.getPosition().X, 70f, Camera.getPosition().Z);
            //var LightPosition = new Vector3(Car.Position.X, Car.Position.Y + 20f, Car.Position.Z);
            Vector3 forward = Car.GetWorld().Forward;
            //var eyePosition = new Vector3(Car.Position.X, Car.Position.Y + 20f, Car.Position.Z)+ forward * 10000;
            var eyePosition = Car.Position + new Vector3(0f, 10f, 0f) - new Vector3(forward.X, 0f, forward.Z) * 5;
            var floorEyePosition = new Vector3(Car.Position.X, Car.Position.Y + 100f, Car.Position.Z) - new Vector3(forward.X, 0f, forward.Z) * 5000;
            var LightPosition = Car.Position + new Vector3(0f,10f,0f) + new Vector3(forward.X, 0f, forward.Z) * 5;

            var wallLigthPosition = Car.Position + new Vector3(0f, 10f, 0f) +new Vector3(forward.X, 0f, forward.Z) * 6;
            //var wallLigthPosition = new Vector3(0f, 10f, 0f);
            var wallEyePosition = Car.Position + new Vector3(0f, 10f, 0f) - new Vector3(forward.X, 0f, forward.Z) * 500; ;

            CarEffect.CurrentTechnique = CarEffect.Techniques["PlayerCar"];
            Car.Draw(View, Projection,CarEffect, EnvironmentMapRenderTarget, cameraPosition);
            CarEffect.CurrentTechnique = CarEffect.Techniques["AiCar"];
            for (int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++){
                IACars[i].Draw(View, Projection,CarEffect);
            }

            Lights.Parameters["ambientColor"].SetValue(Color.White.ToVector3());
            Lights.Parameters["diffuseColor"].SetValue(Color.White.ToVector3());
            Lights.Parameters["specularColor"].SetValue(Color.White.ToVector3());
            Lights.Parameters["carPosition"]?.SetValue(Car.Position);
            Lights.Parameters["lightPosition"]?.SetValue(LightPosition);
            Lights.Parameters["wallLigthPosition"]?.SetValue(wallLigthPosition);
            Lights.Parameters["eyePosition"]?.SetValue(eyePosition);
            Lights.Parameters["floorEyePosition"]?.SetValue(floorEyePosition);
            Lights.Parameters["KAmbient"].SetValue(0.6f);
            Lights.Parameters["KDiffuse"].SetValue(0.0f);
            Lights.Parameters["KSpecular"].SetValue(0.3f);
            Lights.Parameters["shininess"].SetValue(4.7f);

            Lights.CurrentTechnique = Lights.Techniques["Floor"];
            Floor.Draw(View, Projection,Lights);
            Bridge.Draw(View, Projection, Lights);
            Buildings.Draw(View, Projection, Lights);
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Draw(View, Projection);
            for (int i = 0; i < Mounts.Length; i++) Mounts[i].Draw(View, Projection);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Draw(View, Projection);
            for (int i = 0; i < Trees.Length; i++) Trees[i].Draw(View, Projection);
            Lights.CurrentTechnique = Lights.Techniques["Wall"];
            for (int i = 0; i < MapWalls.Length; i++) MapWalls[i].Draw(View, Projection, Lights);
            
            /*if (!GodModeIsActive)
            {
                Clock.Draw(View, Projection);
                SpeedoMeter.Draw(View, Projection);
                Car.DrawHUD(View, Projection);
            }*/

            var msg = "P: PAUSA";
            var W = TGCGame.GetGraphicsDevice().Viewport.Width;
            var H = TGCGame.GetGraphicsDevice().Viewport.Height;
            var escala = 1;
            var size = Font.MeasureString(msg) * escala;
            var Y = 25f;
            var X = 25f;
            TGCGame.GetSpriteBatch().Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation(W - size.X - X, Y, 0));
            TGCGame.GetSpriteBatch().DrawString(Font, msg, new Vector2(0, 0), Color.White);
            TGCGame.GetSpriteBatch().End();


            #endregion

            #region Bloom pass
            TGCGame.GetGraphicsDevice().SetRenderTarget(BloomFirstPassRenderTarget);
            TGCGame.GetGraphicsDevice().Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

            Bloom.CurrentTechnique = Bloom.Techniques["BloomPassPrimitive"];
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Draw(View, Projection, Bloom);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Draw(View, Projection);

            Bloom.CurrentTechnique = Bloom.Techniques["BloomPass"];

            Car.DrawBloom(View, Projection, Bloom);

            

            #endregion

            #region Multipass Bloom
            BlurEffect.CurrentTechnique = BlurEffect.Techniques["Blur"];

            var bloomTexture = BloomFirstPassRenderTarget;
            var finalBloomRenderTarget = BloomSecondPassRenderTarget;

            for (var index = 0; index < PassCount; index++)
            {
                //Exchange(ref SecondaPassBloomRenderTarget, ref FirstPassBloomRenderTarget);

                // Set the render target as null, we are drawing into the screen now!
                TGCGame.GetGraphicsDevice().SetRenderTarget(finalBloomRenderTarget);
                TGCGame.GetGraphicsDevice().Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);

                BlurEffect.Parameters["baseTexture"].SetValue(bloomTexture);
                FullScreenQuad.Draw(BlurEffect);

                if (index != PassCount - 1)
                {
                    var auxiliar = bloomTexture;
                    bloomTexture = finalBloomRenderTarget;
                    finalBloomRenderTarget = auxiliar;
                }
            }

            #endregion



            #region Final pass
            // Set the depth configuration as none, as we don't use depth in this pass
            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.None;

            // Set the render target as null, we are drawing into the screen now!
            TGCGame.GetGraphicsDevice().SetRenderTarget(null);
            TGCGame.GetGraphicsDevice().Clear(Color.Black);

            // Set the technique to our blur technique
            // Then draw a texture into a full-screen quad
            // using our rendertarget as texture
            Bloom.CurrentTechnique = Bloom.Techniques["Integrate"];
            Bloom.Parameters["baseTexture"].SetValue(BloomMainSceneRenderTarget);
            Bloom.Parameters["bloomTexture"].SetValue(BloomFirstPassRenderTarget);
            FullScreenQuad.Draw(Bloom);
            if (!GodModeIsActive)
            {
                Clock.Draw(View, Projection);
                SpeedoMeter.Draw(View, Projection);
                Car.DrawHUD(View, Projection);
            }
            #endregion
        }

        public void DrawMainMenu()
        {
            #region Pass 1
            TGCGame.GetGraphicsDevice().Clear(Color.LightBlue);
            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.Default;
            // Use the default blend and depth configuration
            TGCGame.GetGraphicsDevice().BlendState = BlendState.Opaque;

            // Set the main render target as our render target
            TGCGame.GetGraphicsDevice().SetRenderTarget(BlurMainRenderTarget);
            TGCGame.GetGraphicsDevice().Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.  
            Car.Draw(View, Projection);
            IACars[0].Draw(View, Projection);
            Floor.Draw(View, Projection);
            Bridge.Draw(View, Projection);
            Buildings.Draw(View, Projection);
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Draw(View, Projection);
            for (int i = 0; i < Mounts.Length; i++) Mounts[i].Draw(View, Projection);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Draw(View, Projection);
            for (int i = 0; i < Trees.Length; i++) Trees[i].Draw(View, Projection);
            for (int i = 0; i < MapWalls.Length; i++) MapWalls[i].Draw(View, Projection);

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

            BlurEffect.CurrentTechnique = BlurEffect.Techniques["Blur"];
            BlurEffect.Parameters["baseTexture"].SetValue(BlurMainRenderTarget);
            FullScreenQuad.Draw(BlurEffect);
            #endregion
        }

        private void SetCubemapCameraForOrientation(CubeMapFace face)
        {
            switch (face)
            {
                default:
                case CubeMapFace.PositiveX:
                    CubeMapCamera.FrontDirection = -Vector3.UnitX;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;

                case CubeMapFace.NegativeX:
                    CubeMapCamera.FrontDirection = Vector3.UnitX;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;

                case CubeMapFace.PositiveY:
                    CubeMapCamera.FrontDirection = Vector3.Down;
                    CubeMapCamera.UpDirection = Vector3.UnitZ;
                    break;

                case CubeMapFace.NegativeY:
                    CubeMapCamera.FrontDirection = Vector3.Up;
                    CubeMapCamera.UpDirection = -Vector3.UnitZ;
                    break;

                case CubeMapFace.PositiveZ:
                    CubeMapCamera.FrontDirection = -Vector3.UnitZ;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;

                case CubeMapFace.NegativeZ:
                    CubeMapCamera.FrontDirection = Vector3.UnitZ;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;
            }
        }

    }
}