using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    class TreeObject : DefaultObject <TreeObject>
    {
        protected CylinderObject Cylinder { get; set; }
        protected SphereObject Sphere { get; set; }
        public TreeObject(GraphicsDevice graphicsDevice, Vector3 position, float size){
            Cylinder = new CylinderObject(graphicsDevice, position + new Vector3(0f, size/2, 0f), new Vector3(size/2, size, size/2), 0, Color.Brown);
            Sphere = new SphereObject(graphicsDevice, position + new Vector3(0f, size * 5/3, 0f), Vector3.One * size * 5 / 3, Color.ForestGreen);
        }

        public new void Initialize(){
            Cylinder.Initialize();
            Sphere.Initialize();
        }
        public static void Load(ContentManager content, string shaderDirectory){
        }

        public override void Update(GameTime gameTime){   
        }

        public override void Draw(Matrix view, Matrix projection){
            Cylinder.Draw(view, projection);
            Sphere.Draw(view, projection);
        }
    }
}
