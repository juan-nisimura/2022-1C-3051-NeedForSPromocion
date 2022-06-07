using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.Samples.Collisions;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    class BoostPadObject : QuadObject <BoostPadObject>
    {
        private OrientedBoundingBox OrientedBoundingBox;
        public BoostPadObject(Vector3 position, Vector3 size, float rotation)
            : base(position, size, rotation, Color.GreenYellow){

            OrientedBoundingBox = new OrientedBoundingBox(position, size);
            OrientedBoundingBox.Orientation = Matrix.CreateRotationY(rotation);
        }

        public void Update(CarObject car){
        
            // Chequeo si colisionó con el auto
            if(car.ObjectBox.Intersects(OrientedBoundingBox))
                // Si colisionó con el auto, el auto obtiene un speed boost
                car.SetSpeedBoostTime();    
        }
    }
}
