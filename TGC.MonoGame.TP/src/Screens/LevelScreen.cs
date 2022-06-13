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
        public PlayerCarObject Car { get; set; }
        public IACarObject IACar { get; set; }

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
        private FullScreenQuad FullScreenQuadBlur;
        private RenderTarget2D HorizontalRenderTarget;
        public RenderTarget2D MainRenderTarget;
        public Effect blurEffect { get; set; }
        public override void Initialize() {

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, TGCGame.GetGraphicsDevice().Viewport.AspectRatio, 1, 500);
            
            Camera = new CameraObject();

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
            IACar.Start();
        }

        public override void Stop() {
            MediaPlayer.Stop();
            Car.Stop();
            IACar.Stop();
        }

        public override void Reset(){
            Clock.Reset();
            Car.Reset();
            IACar.Reset();
            for (int i = 0; i < PowerUps.Length; i++)   PowerUps[i].Reset();
        }

        public override void Load(ContentManager content) {

            Song = content.Load<Song>(ContentFolderMusic + SongName());
            MediaPlayer.IsRepeating = true;

            // Cargo los efectos, modelos y texturas
            CarObject.Load(content);

            TreeObject.Load(content);
            BoostPadObject.Load(content, "BoostPadShader");
            HealthBarObject.Load(content, "HealthBarShader");

            MapWallObject.Load(content, "BoxTextureShader", "large_red_bricks_diff_4k");
            BridgeObject.Load(content);
            BuildingsObject.Load(content);
            MissileObject.Load(content);
            BulletObject.Load(content);
            FloorObject.Load(content, "FloorShader", "brown_mud_leaves_01_diff_4k");
            MountObject.Load(content);
            PowerUpObject.Load(content, "BasicShader", "Floor");
            NullPowerUp.Load(content);
            SpeedBoostPowerUpModel.Load(content);
            Clock.Load(content);
            SpeedoMeter.Load(content);
            
            blurEffect = content.Load<Effect>("Effects/" + "GaussianBlur");
            // Create a full screen quad to post-process
            FullScreenQuad = new FullScreenQuad(TGCGame.GetGraphicsDevice());

            // Create render targets. One can be used for simple gaussian blur
            // mainRenderTarget is also used as a render target in the separated filter
            // horizontalRenderTarget is used as the horizontal render target in the separated filter
            MainRenderTarget = new RenderTarget2D(TGCGame.GetGraphicsDevice(), TGCGame.GetGraphicsDevice().Viewport.Width,
                TGCGame.GetGraphicsDevice().Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0,
                RenderTargetUsage.DiscardContents);
            HorizontalRenderTarget = new RenderTarget2D(TGCGame.GetGraphicsDevice(), TGCGame.GetGraphicsDevice().Viewport.Width,
                TGCGame.GetGraphicsDevice().Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);

            blurEffect.Parameters["screenSize"]
                .SetValue(new Vector2(TGCGame.GetGraphicsDevice().Viewport.Width, TGCGame.GetGraphicsDevice().Viewport.Height));

            Car = new PlayerCarObject(new Vector3(-100f,0,-100f), Color.Blue);
            IACar = new IACarObject(new Vector3(-100f,0,-50f), Color.Red);

            Car.Initialize(new CarObject[] { IACar });
            IACar.Initialize(new CarObject[] { Car });
            
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

            Buildings = new BuildingsObject();

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
            
            Bridge = new BridgeObject();

            Bridge.Initialize();
            Buildings.Initialize();

            for (int i = 0; i < PowerUps.Length; i++)   PowerUps[i].Initialize();
            for (int i = 0; i < Mounts.Length; i++)     Mounts[i].Initialize();
            for (int i = 0; i < Trees.Length; i++)      Trees[i].Initialize();
            for (int i = 0; i < BoostPads.Length; i++)      BoostPads[i].Initialize();
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

            Song = content.Load<Song>(ContentFolderMusic + "Riders On The Storm Fredwreck Remix");
            MediaPlayer.IsRepeating = true;
            Reset();
        }

        public void UpdateMainMenu() {
            Timer -= TGCGame.GetElapsedTime();

            Car.Update(View, Projection);
            IACar.Update();
            Floor.Update();
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Update(Car);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Update(Car);

            Buildings.Update(Car);
            Bridge.Update(Car);

            for (int i = 0; i < MapWalls.Length; i++) MapWalls[i].Update(Car);
            for (int i = 0; i < Mounts.Length; i++) Mounts[i].Update(Car);
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
            IACar.Position= new Vector3((float)Math.Cos(Timer - 0.1) * radius, Math.Max(2, (float)Math.Cos(Timer * 4 - MathHelper.PiOver2 * 2) * 100), (float)Math.Sin(Timer - 0.1) * radius);
            IACar.Rotation = -Timer-MathHelper.Pi;
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

            if (IACar.IsDead())
            {
                TGCGame.SwitchActiveScreen(() => WinScreen.GetInstance());
            }

            if (TGCGame.ControllerKeyP.Update().IsKeyToPressed())
            {
                TGCGame.SwitchActiveScreen(() => PauseScreen.GetInstance());

            }

            if (ControllerKeyG.Update().IsKeyToPressed())
            {
                GodModeIsActive = !GodModeIsActive;
            }

            if (GodModeIsActive)
            {
                View = Camera.MoveCameraByKeyboard().GetView();
                return;
            }

            Car.Update(View, Projection);
            IACar.Update();
            Floor.Update();
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Update(Car);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Update(Car);
            Buildings.Update(Car);
            Bridge.Update(Car);

            for (int i = 0; i < MapWalls.Length; i++) MapWalls[i].Update(Car);
            for (int i = 0; i < Mounts.Length; i++) Mounts[i].Update(Car);
            for (int i = 0; i < Trees.Length; i++) Trees[i].Update();
            SpeedoMeter.Update(Car.Speed);


            View = Camera.FollowCamera(Car.GetPosition()).GetView();

            SolveCollisions(Car);
            // SolveCollisions(IACar);
        }

        protected void SolveCollisions(CarObject car) {
            var collided = true;
            car.HasCrashed = false;
            if(HeightMap.GetHeight(car.Position.X, car.Position.Z) == 0)
                Car.GroundLevel = 0;
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
            if(car.HasCrashed)  car.Crash();
            for (int i = 0; i < TGCGame.PLAYERS_QUANTITY - 1; i++)  collided = collided || car.Enemies[i].SolveHorizontalCollision(car);
            SolveBulletsCollisions(Car);
            SolveMissilesCollisions(Car);
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
            #region Pass 1
            TGCGame.GetGraphicsDevice().Clear(Color.LightBlue);
            TGCGame.GetGraphicsDevice().DepthStencilState = DepthStencilState.Default;
            // Use the default blend and depth configuration
            //TGCGame.GetGraphicsDevice().BlendState = BlendState.Opaque;
            TGCGame.GetGraphicsDevice().BlendState = BlendState.AlphaBlend;

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.  
            Car.Draw(View, Projection);
            IACar.Draw(View, Projection);
            Floor.Draw(View, Projection);
            Bridge.Draw(View, Projection);
            Buildings.Draw(View, Projection);
            for (int i = 0; i < PowerUps.Length; i++) PowerUps[i].Draw(View, Projection);
            for (int i = 0; i < Mounts.Length; i++) Mounts[i].Draw(View, Projection);
            for (int i = 0; i < BoostPads.Length; i++) BoostPads[i].Draw(View, Projection);
            for (int i = 0; i < Trees.Length; i++) Trees[i].Draw(View, Projection);
            for (int i = 0; i < MapWalls.Length; i++) MapWalls[i].Draw(View, Projection);
            Clock.Draw(View, Projection);
            SpeedoMeter.Draw(View, Projection);

            Car.DrawHUD(View, Projection);


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
            TGCGame.GetGraphicsDevice().SetRenderTarget(MainRenderTarget);
            TGCGame.GetGraphicsDevice().Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.  
            Car.Draw(View, Projection);
            IACar.Draw(View, Projection);
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

            blurEffect.CurrentTechnique = blurEffect.Techniques["Blur"];
            blurEffect.Parameters["baseTexture"].SetValue(MainRenderTarget);
            FullScreenQuad.Draw(blurEffect);
            #endregion
        }
        
    }
}