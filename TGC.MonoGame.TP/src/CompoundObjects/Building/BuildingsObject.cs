using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Building
{
    class BuildingsObject : DefaultObject <BuildingsObject>
    {
        const int RAMPS_QUANTITY = 8;
        const int WALLS_QUANTITY = 16;
        const int BOXES_QUANTITY = 12;
        protected BuildingBoxObject[] Boxes { get; set; }
        protected BuildingWallObject[] Walls { get; set; }
        protected BuildingRampObject[] Ramps { get; set; }
        public BuildingsObject(IAMapBox floorIAMapBox){

            Ramps = new BuildingRampObject[] {  
                new BuildingRampObject(new Vector3(550f, 20f, 350f), new Vector3(100f, 40f, 100f), MathF.PI / 2, Color.Yellow, 4, new Vector3(0f,0f,-40f)),
                new BuildingRampObject(new Vector3(350f, 20f, 550f), new Vector3(100f, 40f, 100f), MathF.PI, Color.Yellow, 4, new Vector3(-40f,0f,0f)),

                new BuildingRampObject(new Vector3(-550f, 20f, 350f), new Vector3(100f, 40f, 100f), MathF.PI / 2, Color.Yellow, 4, new Vector3(0f,0f,-40f)),
                new BuildingRampObject(new Vector3(-350f, 20f, 550f), new Vector3(100f, 40f, 100f), 0, Color.Yellow, 4, new Vector3(40f,0f,0f)),

                new BuildingRampObject(new Vector3(-550f, 20f, -350f), new Vector3(100f, 40f, 100f), - MathF.PI / 2, Color.Yellow, 4, new Vector3(0f,0f,40f)),
                new BuildingRampObject(new Vector3(-350f, 20f, -550f), new Vector3(100f, 40f, 100f), 0, Color.Yellow, 4, new Vector3(40f,0f,0f)),

                new BuildingRampObject(new Vector3(550f, 20f, -350f), new Vector3(100f, 40f, 100f), - MathF.PI / 2, Color.Yellow, 4, new Vector3(0f,0f,40f)),
                new BuildingRampObject(new Vector3(350f, 20f, -550f), new Vector3(100f, 40f, 100f), MathF.PI, Color.Yellow, 4, new Vector3(-40f,0f,0f)),
            };

            Boxes = new BuildingBoxObject[] {
                new BuildingBoxObject(new Vector3(555f, 20f, 555f), new Vector3(290f, 40f, 290f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(405f, 20f, 550f), new Vector3(10f, 40f, 100f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(550f, 20f, 405f), new Vector3(100f, 40f, 10f), Color.Brown, 2),

                new BuildingBoxObject(new Vector3(-555f, 20f, 555f), new Vector3(290f, 40f, 290f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(-405f, 20f, 550f), new Vector3(10f, 40f, 100f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(-550f, 20f, 405f), new Vector3(100f, 40f, 10f), Color.Brown, 2),

                new BuildingBoxObject(new Vector3(-555f, 20f, -555f), new Vector3(290f, 40f, 290f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(-405f, 20f, -550f), new Vector3(10f, 40f, 100f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(-550f, 20f, -405f), new Vector3(100f, 40f, 10f), Color.Brown, 2),

                new BuildingBoxObject(new Vector3(555f, 20f, -555f), new Vector3(290f, 40f, 290f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(405f, 20f, -550f), new Vector3(10f, 40f, 100f), Color.Brown, 2),
                new BuildingBoxObject(new Vector3(550f, 20f, -405f), new Vector3(100f, 40f, 10f), Color.Brown, 2),
            };

            Walls = new BuildingWallObject[] {
                new BuildingWallObject(new Vector3(405f, 25f, 450f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(455f, 25f, 405f), new Vector3(90f, 50f, 10f), Color.Gray, 1),  
                new BuildingWallObject(new Vector3(405f, 25f, 650f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(650f, 25f, 405f), new Vector3(100f, 50f, 10f), Color.Gray, 1),
                
                new BuildingWallObject(new Vector3(-405f, 25f, -450f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(-455f, 25f, -405f), new Vector3(90f, 50f, 10f), Color.Gray, 1),  
                new BuildingWallObject(new Vector3(-405f, 25f, -650f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(-650f, 25f, -405f), new Vector3(100f, 50f, 10f), Color.Gray, 1),
                
                new BuildingWallObject(new Vector3(-405f, 25f, 450f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(-455f, 25f, 405f), new Vector3(90f, 50f, 10f), Color.Gray, 1),  
                new BuildingWallObject(new Vector3(-405f, 25f, 650f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(-650f, 25f, 405f), new Vector3(100f, 50f, 10f), Color.Gray, 1),
                
                new BuildingWallObject(new Vector3(405f, 25f, -450f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(455f, 25f, -405f), new Vector3(90f, 50f, 10f), Color.Gray, 1),  
                new BuildingWallObject(new Vector3(405f, 25f, -650f), new Vector3(10f, 50f, 100f), Color.Gray, 1),
                new BuildingWallObject(new Vector3(650f, 25f, -405f), new Vector3(100f, 50f, 10f), Color.Gray, 1),
            };

            // Conecto los IAMapBoxes
            for(int i = 0; i < Ramps.Length; i++)
                floorIAMapBox.AddIAMapBox(Ramps[i].IAMapBox);

            for(int i = 0; i < Walls.Length; i++)
                Walls[i].IAMapBox.AddIAMapBox(floorIAMapBox);

            for(int i = 0; i < Ramps.Length; i++){
                var firstBoxIndex = (int) MathF.Floor(i / 2) * 3;   
                for(int j = firstBoxIndex; j < firstBoxIndex + 3; j++){
                    Ramps[i].IAMapBox.AddIAMapBox(Boxes[j].IAMapBox);
                } 
                Ramps[i].IAMapBox.AddIAMapBox(floorIAMapBox);
            }

            for(int i = 0; i < Boxes.Length; i++){
                var firstRampIndex = (int) MathF.Floor(i / 3) * 2;
                for(int j = firstRampIndex; j < firstRampIndex + 2; j++){
                    Boxes[i].IAMapBox.AddIAMapBox(Ramps[j].IAMapBox);
                }
            }            
        }

        public new void Initialize(){
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].Initialize();
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].Initialize();
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].Initialize();
        }

        public override void Update(){
        }

        public void SolveBulletCollision(BulletObject bullet){
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].SolveBulletCollision(bullet);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].SolveBulletCollision(bullet);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].SolveBulletCollision(bullet);
        }

        public void SolveMissileCollision(MissileObject missile){
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].SolveMissileCollision(missile);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].SolveMissileCollision(missile);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].SolveMissileCollision(missile);
        }
        
        public static void Load(){
            BuildingBoxObject.Load("BoxTextureShader", "brick_moss_001_diff_4k");
            BuildingWallObject.Load("BoxTextureShader", "large_red_bricks_diff_4k");
            BuildingRampObject.Load("RampTextureShader", "brick_moss_001_diff_4k");
        }

        public override void Draw(Matrix view, Matrix projection){
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].Draw(view, projection);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].Draw(view, projection);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].Draw(view, projection);
        }

        public void Draw(Matrix view, Matrix projection, Effect effect)
        {
            effect.CurrentTechnique = effect.Techniques["Box"];
            for (int i = 0; i < BOXES_QUANTITY; i++) Boxes[i].Draw(view, projection, effect);
            effect.CurrentTechnique = effect.Techniques["Ramp"];
            for (int i = 0; i < RAMPS_QUANTITY; i++) Ramps[i].Draw(view, projection, effect);
            effect.CurrentTechnique = effect.Techniques["Wall"];
            for (int i = 0; i < WALLS_QUANTITY; i++) Walls[i].Draw(view, projection, effect);
        }

        public bool SolveHorizontalCollision(CarObject car){
            bool collided = false;
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].SolveHorizontalCollision(car);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  collided = collided || Ramps[i].SolveHorizontalCollision(car);
            for (int i = 0; i < WALLS_QUANTITY; i++)  collided = collided || Walls[i].SolveHorizontalCollision(car);
            return collided;
        }

        internal bool SolveVerticalCollision(CarObject car)
        {
            bool collided = false;
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].SolveVerticalCollision(car);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  collided = collided || Ramps[i].SolveVerticalCollision(car);
            for (int i = 0; i < WALLS_QUANTITY; i++)  collided = collided || Walls[i].SolveVerticalCollision(car);
            return collided;
        }

        internal void UpdateHeightMap(int x, int z, int level)
        {
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].UpdateHeightMap(x, z, level);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].UpdateHeightMap(x, z, level);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].UpdateHeightMap(x, z, level);
        }

        internal void UpdateIALogicalMap(int x, int z, int level)
        {
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].UpdateIALogicalMap(x, z, level);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].UpdateIALogicalMap(x, z, level);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].UpdateIALogicalMap(x, z, level);
        }
    }
}
