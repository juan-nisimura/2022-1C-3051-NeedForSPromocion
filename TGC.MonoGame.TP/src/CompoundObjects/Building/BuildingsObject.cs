using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        public BuildingsObject(GraphicsDevice graphicsDevice){

            Boxes = new BuildingBoxObject[] {
                new BuildingBoxObject(graphicsDevice, new Vector3(555f, 20f, 555f), new Vector3(290f, 40f, 290f), Color.Brown),
                new BuildingBoxObject(graphicsDevice, new Vector3(-555f, 20f, 555f), new Vector3(290f, 40f, 290f), Color.Brown),
                new BuildingBoxObject(graphicsDevice, new Vector3(-555f, 20f, -555f), new Vector3(290f, 40f, 290f), Color.Brown),
                new BuildingBoxObject(graphicsDevice, new Vector3(555f, 20f, -555f), new Vector3(290f, 40f, 290f), Color.Brown),
                
                new BuildingBoxObject(graphicsDevice, new Vector3(405f, 20f, 550f), new Vector3(10f, 40f, 100f), Color.Brown),
                new BuildingBoxObject(graphicsDevice, new Vector3(550f, 20f, 405f), new Vector3(100f, 40f, 10f), Color.Brown),

                new BuildingBoxObject(graphicsDevice, new Vector3(-405f, 20f, 550f), new Vector3(10f, 40f, 100f), Color.Brown),
                new BuildingBoxObject(graphicsDevice, new Vector3(-550f, 20f, 405f), new Vector3(100f, 40f, 10f), Color.Brown),

                new BuildingBoxObject(graphicsDevice, new Vector3(405f, 20f, -550f), new Vector3(10f, 40f, 100f), Color.Brown),
                new BuildingBoxObject(graphicsDevice, new Vector3(550f, 20f, -405f), new Vector3(100f, 40f, 10f), Color.Brown),

                new BuildingBoxObject(graphicsDevice, new Vector3(-405f, 20f, -550f), new Vector3(10f, 40f, 100f), Color.Brown),
                new BuildingBoxObject(graphicsDevice, new Vector3(-550f, 20f, -405f), new Vector3(100f, 40f, 10f), Color.Brown),
            };
            
            Ramps = new BuildingRampObject[] {  
                new BuildingRampObject(graphicsDevice, new Vector3(550f, 20f, 350f), new Vector3(100f, 40f, 100f), MathF.PI / 2, Color.Yellow),
                new BuildingRampObject(graphicsDevice, new Vector3(350f, 20f, 550f), new Vector3(100f, 40f, 100f), MathF.PI, Color.Yellow),

                new BuildingRampObject(graphicsDevice, new Vector3(550f, 20f, -350f), new Vector3(100f, 40f, 100f), - MathF.PI / 2, Color.Yellow),
                new BuildingRampObject(graphicsDevice, new Vector3(350f, 20f, -550f), new Vector3(100f, 40f, 100f), MathF.PI, Color.Yellow),

                new BuildingRampObject(graphicsDevice, new Vector3(-550f, 20f, -350f), new Vector3(100f, 40f, 100f), - MathF.PI / 2, Color.Yellow),
                new BuildingRampObject(graphicsDevice, new Vector3(-350f, 20f, -550f), new Vector3(100f, 40f, 100f), 0, Color.Yellow),

                new BuildingRampObject(graphicsDevice, new Vector3(-550f, 20f, 350f), new Vector3(100f, 40f, 100f), MathF.PI / 2, Color.Yellow),
                new BuildingRampObject(graphicsDevice, new Vector3(-350f, 20f, 550f), new Vector3(100f, 40f, 100f), 0, Color.Yellow),
            };

            Walls = new BuildingWallObject[] {
                new BuildingWallObject(graphicsDevice, new Vector3(405f, 25f, 450f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(455f, 25f, 405f), new Vector3(90f, 50f, 10f), Color.Gray),  
                new BuildingWallObject(graphicsDevice, new Vector3(405f, 25f, 650f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(650f, 25f, 405f), new Vector3(100f, 50f, 10f), Color.Gray),
                
                new BuildingWallObject(graphicsDevice, new Vector3(-405f, 25f, -450f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(-455f, 25f, -405f), new Vector3(90f, 50f, 10f), Color.Gray),  
                new BuildingWallObject(graphicsDevice, new Vector3(-405f, 25f, -650f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(-650f, 25f, -405f), new Vector3(100f, 50f, 10f), Color.Gray),
                
                new BuildingWallObject(graphicsDevice, new Vector3(-405f, 25f, 450f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(-455f, 25f, 405f), new Vector3(90f, 50f, 10f), Color.Gray),  
                new BuildingWallObject(graphicsDevice, new Vector3(-405f, 25f, 650f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(-650f, 25f, 405f), new Vector3(100f, 50f, 10f), Color.Gray),
                
                new BuildingWallObject(graphicsDevice, new Vector3(405f, 25f, -450f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(455f, 25f, -405f), new Vector3(90f, 50f, 10f), Color.Gray),  
                new BuildingWallObject(graphicsDevice, new Vector3(405f, 25f, -650f), new Vector3(10f, 50f, 100f), Color.Gray),
                new BuildingWallObject(graphicsDevice, new Vector3(650f, 25f, -405f), new Vector3(100f, 50f, 10f), Color.Gray),
            };
        }

        public new void Initialize(){
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].Initialize();
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].Initialize();
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].Initialize();
        }

        public override void Update(GameTime gameTime){
        }

        public void Update(GameTime gameTime, CarObject car){
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].Update(gameTime, car);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].Update(gameTime, car);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].Update(gameTime, car);
        }
        
        public static void Load(ContentManager content){
            BuildingBoxObject.Load(content, "BoxTextureShader", "brick_moss_001_diff_4k");
            BuildingWallObject.Load(content, "BoxTextureShader", "large_red_bricks_diff_4k");
            BuildingRampObject.Load(content, "RampTextureShader", "brick_moss_001_diff_4k");
        }

        public override void Draw(Matrix view, Matrix projection){
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].Draw(view, projection);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].Draw(view, projection);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].Draw(view, projection);
        }

        public bool SolveHorizontalCollision(GameTime gameTime, CarObject car){
            bool collided = false;
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].SolveHorizontalCollision(gameTime, car);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  collided = collided || Ramps[i].SolveHorizontalCollision(gameTime, car);
            for (int i = 0; i < WALLS_QUANTITY; i++)  collided = collided || Walls[i].SolveHorizontalCollision(gameTime, car);
            return collided;
        }

        internal bool SolveVerticalCollision(GameTime gameTime, CarObject car)
        {
            bool collided = false;
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].SolveVerticalCollision(gameTime, car);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  collided = collided || Ramps[i].SolveVerticalCollision(gameTime, car);
            for (int i = 0; i < WALLS_QUANTITY; i++)  collided = collided || Walls[i].SolveVerticalCollision(gameTime, car);
            return collided;
        }

        internal void UpdateHeightMap(int x, int z)
        {
            for (int i = 0; i < BOXES_QUANTITY; i++)  Boxes[i].UpdateHeightMap(x, z);
            for (int i = 0; i < RAMPS_QUANTITY; i++)  Ramps[i].UpdateHeightMap(x, z);
            for (int i = 0; i < WALLS_QUANTITY; i++)  Walls[i].UpdateHeightMap(x, z);
        }
    }
}
