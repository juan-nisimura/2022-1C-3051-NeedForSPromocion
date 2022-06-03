using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bridge
{
    class BridgeFloorObject : BoxObject <BridgeFloorObject>
    {
        public BridgeFloorObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color) 
        : base(graphicsDevice, position, size, color)
        {
        }
        public void DrawBlinnPhong(Effect effect, Matrix view, Matrix projection)
        {
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;
            effect.CurrentTechnique = effect.Techniques["BridgeFloorColorDrawing"];
            effect.Parameters["rampTexture"]?.SetValue(getTexture());
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(World)));
            effect.Parameters["WorldViewProjection"].SetValue(World * view * projection);
            DrawPrimitive();
        }
    }
}