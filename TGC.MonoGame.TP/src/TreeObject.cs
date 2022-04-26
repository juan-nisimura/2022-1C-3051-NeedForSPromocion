using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src   
{
    class TreeObject : DefaultObject
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
        public new void Load(ContentManager content){
            Cylinder.Load(content);
            Sphere.Load(content);
        }

        public override void Update(GameTime gameTime){   
        }

        public new void Draw(Matrix view, Matrix projection){
            Cylinder.Draw(view, projection);
            Sphere.Draw(view, projection);
        }
    }
}
