using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    class MountObject : DefaultObject <MountObject>
    {
        protected BoxObject Box { get; set; }
        protected RampObject[] Ramps { get; set; }
        public MountObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation, Color color){
            Box = new BoxObject(graphicsDevice, position, size, color);
            Ramps = new RampObject[] {
                new RampObject(graphicsDevice, position + new Vector3(size.X * 6 / 10, 0f, 0f), new Vector3(size.X/5,size.Y,size.Z), rotation, color),
                new RampObject(graphicsDevice, position + new Vector3(-size.X * 6 / 10, 0f, 0f), new Vector3(size.X/5,size.Y,size.Z), rotation + MathF.PI, color),
                new RampObject(graphicsDevice, position + new Vector3(0f, 0f, size.Z * 6 / 10), new Vector3(size.X/5,size.Y,size.Z), rotation - MathF.PI/2, color),
                new RampObject(graphicsDevice, position + new Vector3(0f, 0f, -size.Z * 6 / 10), new Vector3(size.X/5,size.Y,size.Z), rotation + MathF.PI/2, color)
            };
        }

        public new void Initialize(){
            Box.Initialize();
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].Initialize();
        }

        public override void Update(GameTime gameTime){   
        }

        public void Update(GameTime gameTime, CarObject car){
            Box.Update(gameTime, car);
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].Update(gameTime, car);
        }
        
        public static void Load(ContentManager content, string shaderDirectory){
        }

        public bool SolveHorizontalCollision(GameTime gameTime, CarObject car){
            bool collided = false;
            collided = Box.SolveHorizontalCollision(gameTime, car);
            for (int i = 0; i < Ramps.Length; i++)  collided = collided || Ramps[i].SolveHorizontalCollision(gameTime, car);
            return collided;
        }

        public override void Draw(Matrix view, Matrix projection){
            Box.Draw(view, projection);
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].Draw(view, projection);
        }

        internal bool SolveVerticalCollision(GameTime gameTime, CarObject car)
        {
            bool collided = false;
            collided = Box.SolveVerticalCollision(gameTime, car);
            for (int i = 0; i < Ramps.Length; i++)  collided = collided || Ramps[i].SolveVerticalCollision(gameTime, car);
            return collided;
        }

        internal void UpdateHeightMap(int x, int z)
        {
            Box.UpdateHeightMap(x, z);
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].UpdateHeightMap(x, z);
        }
    }
}
