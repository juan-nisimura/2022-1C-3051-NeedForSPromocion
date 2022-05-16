using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.ModelObjects;

namespace TGC.Monogame.TP.Src.PrimitiveObjects 
{
    class BoostPadObject : QuadObject <BoostPadObject>
    {
        private Vector3 Position;
        private BoundingSphere BoundingSphere;
        public BoostPadObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, float rotation)
            : base(graphicsDevice, position, size, rotation, Color.GreenYellow)
        {
            Position = position + new Vector3(0f, 10f, 0f);
            TranslateMatrix = Matrix.CreateTranslation(Position);
            BoundingSphere = new BoundingSphere(Position, 8f);
        }
        public void Update(GameTime gameTime, CarObject car){
            if(car.ObjectBox.Intersects(BoundingSphere)){
                car.SetSpeedBoostTime();
            }
        }
    }
}
