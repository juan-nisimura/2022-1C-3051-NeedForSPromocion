using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.Monogame.TP.Src.PrimitiveObjects;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.CompoundObjects.Tree;
using TGC.Monogame.TP.Src.CompoundObjects.Missile;
using TGC.Monogame.TP.Src.CompoundObjects.Bridge;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.Monogame.TP.Src.CompoundObjects.Map;
using TGC.Monogame.TP.Src.CompoundObjects.Mount;
using TGC.Monogame.TP.Src.CompoundObjects.Building;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.PowerUps;
using TGC.Monogame.TP.Src.CompoundObjects.Bullet;

namespace TGC.Monogame.TP.Src.Screens 
{
    public class LevelScreen : Screen
    {        
        protected override String SongName() { return "Riders On The Storm Fredwreck Remix"; }
        protected override String FontName() { return "CascadiaCode/CascadiaCodePL"; }
        protected static Screen Instance { get; set; } = new LevelScreen();
        public static Screen GetInstance() { return Instance; }
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

        public override void Initialize(GraphicsDevice graphicsDevice) {

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            Projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1, 500);

            Camera = new CameraObject();

            BoostPads = new BoostPadObject[]{
                new BoostPadObject(graphicsDevice, new Vector3(0,0.1f,575f),new Vector3(22.5f,1f,27.5f), - MathF.PI / 2),
                new BoostPadObject(graphicsDevice, new Vector3(0,0.1f,525f),new Vector3(22.5f,1f,27.5f), - MathF.PI / 2),

                new BoostPadObject(graphicsDevice, new Vector3(0,0.1f,-575f),new Vector3(22.5f,1f,27.5f), MathF.PI / 2),
                new BoostPadObject(graphicsDevice, new Vector3(0,0.1f,-525f),new Vector3(22.5f,1f,27.5f), MathF.PI / 2),

                new BoostPadObject(graphicsDevice, new Vector3(575,0.1f,0f),new Vector3(22.5f,1f,27.5f), 0),
                new BoostPadObject(graphicsDevice, new Vector3(525,0.1f,0f),new Vector3(22.5f,1f,27.5f), 0),

                new BoostPadObject(graphicsDevice, new Vector3(-575,0.1f,0f),new Vector3(22.5f,1f,27.5f), MathF.PI),
                new BoostPadObject(graphicsDevice, new Vector3(-525,0.1f,0f),new Vector3(22.5f,1f,27.5f), MathF.PI),
            };
            
            Floor = new FloorObject(graphicsDevice, new Vector3(0f,0f,0f),new Vector3(700f,1f,700f),0);           

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

        public override void Load(GraphicsDevice graphicsDevice, ContentManager content) {

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
            FloorObject.Load(content, "FloorShader", "brown_mud_leaves_01_diff_4k");
            MountObject.Load(content);
            PowerUpObject.Load(content, "BasicShader", "Floor");

            Car = new PlayerCarObject(new Vector3(-100f,0,-100f), Color.Blue);
            IACar = new IACarObject(new Vector3(-100f,0,-50f), Color.Red);

            Car.Initialize(graphicsDevice, new CarObject[] { IACar });
            IACar.Initialize(graphicsDevice, new CarObject[] { Car });
            
            Mounts = new MountObject[]{
                new MountObject(graphicsDevice, new Vector3(235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                
                new MountObject(graphicsDevice, new Vector3(-235f,2.5f,-400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(graphicsDevice, new Vector3(0f,2.5f,-250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(graphicsDevice, new Vector3(0f,2.5f,250f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(graphicsDevice, new Vector3(-235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
                new MountObject(graphicsDevice, new Vector3(235f,2.5f,400f),new Vector3(60f,5f,60f),0,Color.White),
            };
            
            Trees = new TreeObject[]{
                new TreeObject(graphicsDevice, new Vector3(100f,0f,400f), 40f),
                new TreeObject(graphicsDevice, new Vector3(-100f,0f,400f), 40f),
                new TreeObject(graphicsDevice, new Vector3(100f,0f,-400f), 40f),
                new TreeObject(graphicsDevice, new Vector3(-100f,0f,-400f), 40f),
                new TreeObject(graphicsDevice, new Vector3(-300f,0f,-300f), 40f),
                new TreeObject(graphicsDevice, new Vector3(-300f,0f,300f), 40f),
                new TreeObject(graphicsDevice, new Vector3(300f,0f,-300f), 40f),
                new TreeObject(graphicsDevice, new Vector3(300f,0f,300f), 40f),
            };

            MapWalls = new MapWallObject[] {
                new MapWallObject(graphicsDevice, new Vector3(705f, 32.5f, 0f), new Vector3(10f, 65f, 1420f), Color.White),
                new MapWallObject(graphicsDevice, new Vector3(-705f, 32.5f, 0f), new Vector3(10f, 65f, 1420f), Color.White),
                new MapWallObject(graphicsDevice, new Vector3(0f, 32.5f, 705f), new Vector3(1400f, 65f, 10f), Color.White),
                new MapWallObject(graphicsDevice, new Vector3(0f, 32.5f, -705f), new Vector3(1400f, 65f, 10f), Color.White),
            };

            Buildings = new BuildingsObject(graphicsDevice);

            PowerUps = new PowerUpObject[] {
                new PowerUpObject(graphicsDevice, new Vector3(0f,30f,0f)),

                new PowerUpObject(graphicsDevice, new Vector3(235f,0f,0f)),
                new PowerUpObject(graphicsDevice, new Vector3(-235f,0f,0f)),

                new PowerUpObject(graphicsDevice, new Vector3(235f,5f,-400f)),
                new PowerUpObject(graphicsDevice, new Vector3(-235f,5f,-400f)),
                new PowerUpObject(graphicsDevice, new Vector3(0f,5f,-250f)),
                
                new PowerUpObject(graphicsDevice, new Vector3(235f,5f,400f)),
                new PowerUpObject(graphicsDevice, new Vector3(-235f,5f,400f)),
                new PowerUpObject(graphicsDevice, new Vector3(0f,5f,250f)),

                new PowerUpObject(graphicsDevice, new Vector3(550f, 40f, 550f)),
                new PowerUpObject(graphicsDevice, new Vector3(-550f, 40f, -550f)),
                new PowerUpObject(graphicsDevice, new Vector3(-550f, 40f, 550f)),
                new PowerUpObject(graphicsDevice, new Vector3(550f, 40f, -550f))
            };
            
            Bridge = new BridgeObject(graphicsDevice);

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

        public override void Update(GraphicsDevice graphicsDevice) {

            Clock.Update();

            // Si termina el juego de alguna forma, no hace los demás updates
            if(Clock.NoTimeLeft()){
                TGCGame.SwitchActiveScreen(() => TimeOutScreen.GetInstance());
            }

            if(Car.IsDead()){
                TGCGame.SwitchActiveScreen(() => LoseScreen.GetInstance());
            }

            if(IACar.IsDead()){
                TGCGame.SwitchActiveScreen(() => WinScreen.GetInstance());
            }

            if (TGCGame.ControllerKeyP.Update().IsKeyToPressed()){
                TGCGame.SwitchActiveScreen(() => PauseScreen.GetInstance());
            }

            if (ControllerKeyG.Update().IsKeyToPressed()){
                GodModeIsActive = !GodModeIsActive;
            }

            if(GodModeIsActive){
                View = Camera.MoveCameraByKeyboard().GetView();
                return;
            }

            Car.Update(graphicsDevice, View, Projection);
            IACar.Update();
            Floor.Update();
            for (int i = 0; i < PowerUps.Length; i++)       PowerUps[i].Update(Car);
            for (int i = 0; i < BoostPads.Length; i++)      BoostPads[i].Update(Car);

            Buildings.Update(Car);
            Bridge.Update(Car);
            
            for (int i = 0; i < MapWalls.Length; i++)       MapWalls[i].Update(Car);
            for (int i = 0; i < Mounts.Length; i++)         Mounts[i].Update(Car);
            for (int i = 0; i < Trees.Length; i++)          Trees[i].Update();

            SolveCollisions(Car);

            View = Camera.FollowCamera(Car.GetPosition()).GetView();
        }

        protected void SolveCollisions(CarObject car) {
            var collided = true;
            car.HasCrashed = false;
            if(HeightMap.GetHeight(car.Position.X, car.Position.Z) == 0)
                Car.GroundLevel = 0;
            while(collided){
                collided = false;
                Buildings.SolveHorizontalCollision(car);
                Bridge.SolveHorizontalCollision(car);
                for (int i = 0; i < Mounts.Length; i++)         collided = collided || Mounts[i].SolveHorizontalCollision(car);
                for (int i = 0; i < MapWalls.Length; i++)       collided = collided || MapWalls[i].SolveHorizontalCollision(car);
                for (int i = 0; i < Trees.Length; i++)          collided = collided || Trees[i].SolveHorizontalCollision(car);
                for (int i = 0; i < Mounts.Length; i++)         collided = collided || Mounts[i].SolveVerticalCollision(car);
                for (int i = 0; i < MapWalls.Length; i++)       collided = collided || MapWalls[i].SolveVerticalCollision(car);
                Buildings.SolveVerticalCollision(car);
                Bridge.SolveVerticalCollision(car);
            }
            if(car.HasCrashed)  car.Crash();
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.Clear(Color.LightBlue);
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

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
        }
    }
}