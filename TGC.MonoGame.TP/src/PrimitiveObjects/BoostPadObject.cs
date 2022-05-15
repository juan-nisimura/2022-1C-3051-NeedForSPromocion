using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.Samples.Collisions;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    class BoostPadObject : QuadObject <BoostPadObject>
    {
        private OrientedBoundingBox OrientedBoundingBox;
        bool isAvailable = true;

        public BoostPadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation)
            : base(graphicsDevice, position, size, rotation, Color.GreenYellow){

            OrientedBoundingBox = new OrientedBoundingBox(position, size);
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(rotation);
        }

        public void Update(GameTime gameTime, CarObject car){
        

            // Chequeo si colisionó con el auto
            if(car.ObjectBox.Intersects(OrientedBoundingBox)){
                // Si colisionó con el auto, el auto obtiene un speed boost
                isAvailable = false;
            } else {
                isAvailable = true;
            }
        }

        protected override void DrawPrimitive() {
            if(isAvailable)   
                QuadPrimitive.Draw(getEffect());
        }
    }
}
