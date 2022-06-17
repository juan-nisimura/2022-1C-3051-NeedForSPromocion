using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bridge
{
    class BridgeObject : DefaultObject <BridgeObject>
    {
        const int RAMPS_QUANTITY = 2;
        const int FLOORS_QUANTITY = 2;
        const int COLUMNS_QUANTITY = 8;
        protected BridgeBlockObject Block { get; set; }
        protected BridgeColumnObject[] Columns { get; set; }
        protected BridgeRampObject[] Ramps { get; set; }
        protected BridgeFloorObject[] Floors { get; set; }
        public BridgeObject(){

            Ramps = new BridgeRampObject[] { 
                new BridgeRampObject(new Vector3(370f, 15f, -90f), new Vector3(100f, 30f, 80f), MathF.PI / 2, Color.Yellow),
                new BridgeRampObject(new Vector3(-370f, 15f, 90f), new Vector3(100f, 30f, 80f), - MathF.PI / 2, Color.Yellow),
            };

            Floors = new BridgeFloorObject[] {
                new BridgeFloorObject(new Vector3(235f, 29f, 0f), new Vector3(350f, 2f, 80f), Color.Brown),
                new BridgeFloorObject(new Vector3(-235f, 29f, 0f), new Vector3(350f, 2f, 80f), Color.Brown),
            };

            Block = new BridgeBlockObject(new Vector3(0f, 15f, 0f), new Vector3(120f, 30f, 80f), Color.Gray);

            Columns = new BridgeColumnObject[]{
                new BridgeColumnObject(new Vector3(150f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new BridgeColumnObject(new Vector3(150f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new BridgeColumnObject(new Vector3(300f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new BridgeColumnObject(new Vector3(300f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new BridgeColumnObject(new Vector3(-150f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new BridgeColumnObject(new Vector3(-150f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new BridgeColumnObject(new Vector3(-300f, 30f, -45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
                new BridgeColumnObject(new Vector3(-300f, 30f, 45f), new Vector3(10f, 60f, 10f), MathF.PI, Color.Beige),
            };
        }

        public new void Initialize(){
            Block.Initialize();
            for (int i = 0; i < RAMPS_QUANTITY; i++)    Ramps[i].Initialize();
            for (int i = 0; i < FLOORS_QUANTITY; i++)   Floors[i].Initialize();
            for (int i = 0; i < COLUMNS_QUANTITY; i++)  Columns[i].Initialize();
        }

        public override void Update(){   
        }
        
        public static void Load(){
            BridgeColumnObject.Load("TreeTrunkShader", "bark brown/textures/bark_brown_01_diff_4k");
            BridgeRampObject.Load("RampTextureShader", "weathered_brown_planks_diff_4k");
            BridgeBlockObject.Load("BoxTextureShader", "rock_01_diff_4k");
            BridgeFloorObject.Load("BridgeFloorTextureShader", "weathered_brown_planks_diff_4k");
        }

        public override void Draw(Matrix view, Matrix projection){
            Block.Draw(view, projection);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].Draw(view, projection);
            for (int i = 0; i < FLOORS_QUANTITY; i++)  Floors[i].Draw(view, projection);
            for (int i = 0; i < COLUMNS_QUANTITY; i++)  Columns[i].Draw(view, projection);
        }

        public void SolveBulletCollision(BulletObject bullet){
            Block.SolveBulletCollision(bullet);
            for (int i = 0; i < RAMPS_QUANTITY; i++)    Ramps[i].SolveBulletCollision(bullet);
            for (int i = 0; i < FLOORS_QUANTITY; i++)   Floors[i].SolveBulletCollision(bullet);
            for (int i = 0; i < COLUMNS_QUANTITY; i++)  Columns[i].SolveBulletCollision(bullet);
        }

        public void SolveMissileCollision(MissileObject missile){
            Block.SolveMissileCollision(missile);
            for (int i = 0; i < RAMPS_QUANTITY; i++)    Ramps[i].SolveMissileCollision(missile);
            for (int i = 0; i < FLOORS_QUANTITY; i++)   Floors[i].SolveMissileCollision(missile);
            for (int i = 0; i < COLUMNS_QUANTITY; i++)  Columns[i].SolveMissileCollision(missile);
        }

        public bool SolveHorizontalCollision(CarObject car){
            bool collided = false;
            collided = Block.SolveHorizontalCollision(car);
            for (int i = 0; i < RAMPS_QUANTITY; i++)    collided = collided || Ramps[i].SolveHorizontalCollision(car);
            for (int i = 0; i < FLOORS_QUANTITY; i++)   collided = collided || Floors[i].SolveHorizontalCollision(car);
            for (int i = 0; i < COLUMNS_QUANTITY; i++)  collided = collided || Columns[i].SolveHorizontalCollision(car);
            return collided;
        }

        internal bool SolveVerticalCollision(CarObject car)
        {
            bool collided = false;
            collided = Block.SolveVerticalCollision(car);
            for (int i = 0; i < RAMPS_QUANTITY; i++)    collided = collided || Ramps[i].SolveVerticalCollision(car);
            for (int i = 0; i < FLOORS_QUANTITY; i++)   collided = collided || Floors[i].SolveVerticalCollision(car);
            // for (int i = 0; i < COLUMNS_QUANTITY; i++)  collided = collided || Columns[i].SolveVerticalCollision(car);
            return collided;
        }

        internal void UpdateHeightMap(int x, int z)
        {
            Block.UpdateHeightMap(x, z);
            for (int i = 0; i < RAMPS_QUANTITY; i++)    Ramps[i].UpdateHeightMap(x, z);
            for (int i = 0; i < FLOORS_QUANTITY; i++)   Floors[i].UpdateHeightMap(x, z);
            // for (int i = 0; i < COLUMNS_QUANTITY; i++)  Columns[i].UpdateHeightMap(x, z);
        }
    }
}
