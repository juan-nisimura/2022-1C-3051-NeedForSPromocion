using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    class MissileObject : DefaultObject <MissileObject>
    {
        protected CylinderObject Cylinder { get; set; }
        protected SphereObject Sphere { get; set; }
        protected TriangleObject[] Triangles { get; set; }
        private const int TRIANGLES_QUANTITY = 8;
        private const float TRIANGLE_RELATIVE_SIZE = 0.4f;
        
        public MissileObject(GraphicsDevice graphicsDevice, Vector3 position, float size){
            Cylinder = new CylinderObject(graphicsDevice, position + new Vector3(0f, size/2, 0f), new Vector3(size/2, size, size/2), 0, Color.Gray);
            Sphere = new SphereObject(graphicsDevice, position + new Vector3(0f, size, 0f), new Vector3(0.5f, 1, 0.5f) * size, Color.Red);
            Triangles = new TriangleObject[] {
                new TriangleObject(graphicsDevice, position + new Vector3(0f, 0f, 0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new TriangleObject(graphicsDevice, position + new Vector3(0f, 0f, -0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new TriangleObject(graphicsDevice, position + new Vector3(0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new TriangleObject(graphicsDevice, position + new Vector3(-0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, Color.Green),

                new TriangleObject(graphicsDevice, position + new Vector3(0f, 0f, 0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 1f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new TriangleObject(graphicsDevice, position + new Vector3(0f, 0f, -0.25f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, -1f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new TriangleObject(graphicsDevice, position + new Vector3(0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new TriangleObject(graphicsDevice, position + new Vector3(-0.25f, 0f, 0f) * size, new Vector3(0f, 1f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(-1f, 0f, 0f) * size * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
            };
        }

        public new void Initialize(){
            Cylinder.Initialize();
            Sphere.Initialize();
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   Triangles[i].Initialize();
        }
        
        public static void Load(ContentManager content, string shaderDirectory){
            
        }

        public override void Update(GameTime gameTime){   
        
        }

        public override void Draw(Matrix view, Matrix projection){
            Cylinder.Draw(view, projection);
            Sphere.Draw(view, projection);
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   Triangles[i].Draw(view, projection);
        }
    }
}