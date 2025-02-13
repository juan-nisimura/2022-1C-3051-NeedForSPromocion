using System;
using Microsoft.Xna.Framework;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.MyContentManagers;
using TGC.MonoGame.TP;

namespace TGC.Monogame.TP.Src.PowerUpObjects.PowerUpModels
{
    public class MissilePowerUpModel : PowerUpModel
    {
        protected MissileBodyObject MissileBody { get; set; }
        protected MissileHeadObject MissileHead { get; set; }
        protected MissileTriangleObject[] MissileTriangles { get; set; }
        private const int TRIANGLES_QUANTITY = 8;
        private const float TRIANGLE_RELATIVE_SIZE = 0.8f;
        public const float MISSILE_MODEL_SIZE = 1f;
        public static PowerUpModel PowerUpModel = new MissilePowerUpModel(Vector3.Zero);
        public static new PowerUpModel GetModel() { 
            return PowerUpModel; 
        }
        public MissilePowerUpModel(Vector3 position){
            MissileBody = new MissileBodyObject(MISSILE_MODEL_SIZE);
            MissileHead = new MissileHeadObject(MISSILE_MODEL_SIZE);
            MissileTriangles = new MissileTriangleObject[] {
                new MissileTriangleObject(new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green, MISSILE_MODEL_SIZE),
                new MissileTriangleObject(new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green, MISSILE_MODEL_SIZE),
                new MissileTriangleObject(new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green, MISSILE_MODEL_SIZE),
                new MissileTriangleObject(new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green, MISSILE_MODEL_SIZE),

                new MissileTriangleObject(new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green, MISSILE_MODEL_SIZE),
                new MissileTriangleObject(new Vector3(0f, 1f, -0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green, MISSILE_MODEL_SIZE),
                new MissileTriangleObject(new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green, MISSILE_MODEL_SIZE),
                new MissileTriangleObject(new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green, MISSILE_MODEL_SIZE)
            };
            RotationMatrix = Matrix.CreateRotationX(-MathF.PI / 15);
            Position = position;
            MissileBody.Initialize();
            MissileHead.Initialize();
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Initialize();
        }
        
        public override void Update(){
            RotationMatrix *= Matrix.CreateRotationY(ROTATION_SPEED * TGCGame.GetElapsedTime());
            var forward = Vector3.Normalize(RotationMatrix.Forward);

            MissileBody.Update(Position, forward, RotationMatrix);
            MissileHead.Update(Position, forward, RotationMatrix);
            for (int i = 0; i < TRIANGLES_QUANTITY; i++) MissileTriangles[i].Update(Position, forward, RotationMatrix);
            Time += TGCGame.GetElapsedTime();
        }

        public override void Draw(Matrix view, Matrix projection)
        {
            var effect = MyContentManager.Effects.Get("PowerUpModelShader");
            effect.Parameters["Time"]?.SetValue(Time);
            effect.Parameters["Center"]?.SetValue(Position);
            MissileBody.Draw(view, projection, effect);
            MissileHead.Draw(view, projection, effect);
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Draw(view, projection, effect);
        }
    }
}