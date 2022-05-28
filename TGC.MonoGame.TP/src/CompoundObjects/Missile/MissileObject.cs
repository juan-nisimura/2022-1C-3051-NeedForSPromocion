using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class MissileObject : DefaultObject <MissileObject>
    {
        protected MissileBodyObject MissileBody { get; set; }
        protected MissileHeadObject MissileHead { get; set; }
        protected MissileTriangleObject[] MissileTriangles { get; set; }
        private const int TRIANGLES_QUANTITY = 8;
        private const float TRIANGLE_RELATIVE_SIZE = 0.8f;
        public Vector3 missilePosition { get; set; }
        public float missileRotationY { get; set; }
        public Vector3[] trianglePositions { get; set; }
        private float missileSize { get; set; }

        public MissileObject(GraphicsDevice graphicsDevice, Vector3 position, float size)
        {
            missileSize = size;
            //missilePosition = new Vector3(position.X,position.Y,position.Z);
            MissileBody = new MissileBodyObject(graphicsDevice, position + new Vector3(0f, size*5 / 2, 0f), new Vector3(size / 2, size, size / 2), 0, 0, Color.Gray);
            MissileHead = new MissileHeadObject(graphicsDevice, position + new Vector3(0f, size, size / 2), new Vector3(0.5f, 1, 0.5f) * size, 0, Color.Red);

            MissileTriangles = new MissileTriangleObject[] {
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 0f, 0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 0f, -0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(-0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),

                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 0f, 0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 1f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 0f, -0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, -1f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(-0.5f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(-0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(-1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
            };
        }

        public MissileObject(GraphicsDevice graphicsDevice, Vector3 position, float size, float rotationY){
            missileSize = size;
            missilePosition = new Vector3(position.X, position.Y, position.Z);
            //missilePosition = position;
            missileRotationY = rotationY;
            MissileBody = new MissileBodyObject(graphicsDevice, position + new Vector3(0f, size/2, 0f), new Vector3(size/2, size, size/2), MathHelper.PiOver2 ,rotationY, Color.Gray);
            MissileHead = new MissileHeadObject(graphicsDevice, position + new Vector3(0f, size, size/2), new Vector3(0.5f, 1, 0.5f) * size,rotationY, Color.Red);
            trianglePositions = new Vector3[8] {
                position + new Vector3(0f, 0f, 0f) * size,
                position + new Vector3(0f, 0f, 0f) * size,
                position + new Vector3(0f, 0f, 0f) * size,
                position + new Vector3(0f, 0f, 0f) * size,

                position + new Vector3(0f, 0f, 0f) * size,
                position + new Vector3(0f, 0f, 0f) * size,
                position + new Vector3(0f, 0f, 0f) * size,
                position + new Vector3(0f, 0f, 0f) * size
            };
            MissileTriangles = new MissileTriangleObject[] {
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 0f, 0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 1f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f) * size * TRIANGLE_RELATIVE_SIZE, Color.Yellow),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, Color.Blue),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(-0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, Color.Red),

                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 0f, 0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 1f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.White),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0f, 1f, -0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, -1f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Black),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.LightBlue),
                new MissileTriangleObject(graphicsDevice, position + new Vector3(-0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(-1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Orange),
            
                };
        }

        public new void Initialize(){
            MissileBody.Initialize();
            MissileHead.Initialize();
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Initialize();
        }
        
        public static void Load(ContentManager content){
            MissileBodyObject.Load(content, "BasicShader");
            MissileHeadObject.Load(content, "BasicShader");
            MissileTriangleObject.Load(content, "BasicShader");
        }
        public override void Update(GameTime gameTime)
        {
            MissileBody.Update(gameTime);
            MissileHead.Update(gameTime);
        }
        public void Update(GameTime gameTime, Vector3 Position, float Rotation)
        {
            MissileBody.Update(gameTime, Position, Rotation, missileSize);
            MissileHead.Update(gameTime, Position, Rotation, missileSize);
            for (int i = 0; i < TRIANGLES_QUANTITY; i++) MissileTriangles[i].Update(gameTime, trianglePositions[i], Rotation, missileSize);
        }

        public override void Draw(Matrix view, Matrix projection){
            MissileBody.Draw(view, projection);
            MissileHead.Draw(view, projection);
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Draw(view, projection);
        }
    }
}