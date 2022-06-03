using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Bridge
{
    class BridgeBlockObject : BoxObject <BridgeBlockObject>
    {
        public BridgeBlockObject(GraphicsDevice graphicsDevice, Vector3 position, Vector3 size, Color color) 
        : base(graphicsDevice, position, size, color)
        {
        }
        public void DrawBlinnPhong(Effect effect, Matrix view, Matrix projection)
        {
            // Para dibujar el modelo necesitamos pasarle informacion que el efecto esta esperando.
            World = ScaleMatrix * RotationMatrix * TranslateMatrix;
            effect.CurrentTechnique = effect.Techniques["BoxColorDrawing"];
            effect.Parameters["boxTexture"]?.SetValue(getTexture());
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Invert(Matrix.Transpose(World)));
            //effect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(World));
            effect.Parameters["WorldViewProjection"].SetValue(World * view * projection);
            DrawPrimitive();
        }
    }
}