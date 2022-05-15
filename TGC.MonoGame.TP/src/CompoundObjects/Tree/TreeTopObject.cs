using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Tree
{
    class TreeTopObject : SphereObject <TreeTopObject>
    {
        private float Radius;
        public TreeTopObject(GraphicsDevice graphicsDevice, Vector3 position, float radius, Color color) :
            base(graphicsDevice, position, Vector3.One * radius, color){
                Radius = radius;
        }

        protected override void DrawPrimitive() { 
            getEffect().Parameters["Radius"]?.SetValue(Radius);
            SpherePrimitive.Draw(getEffect());
        }
    }
}
