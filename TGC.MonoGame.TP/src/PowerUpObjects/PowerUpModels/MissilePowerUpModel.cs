using System;
using Microsoft.Xna.Framework;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
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
        public const float MISSILE_MODEL_SIZE = 10f;
        private Vector3 Position;

        public MissilePowerUpModel(Vector3 position){
            MissileBody = new MissileBodyObject();
            MissileHead = new MissileHeadObject();
            MissileTriangles = new MissileTriangleObject[] {
                new MissileTriangleObject(new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),
                new MissileTriangleObject(new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, Color.Green),

                new MissileTriangleObject(new Vector3(0f, 0f, 0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(new Vector3(0f, 1f, -0.25f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, -1f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(new Vector3(0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green),
                new MissileTriangleObject(new Vector3(-0.25f, 0f, 0f) * MISSILE_MODEL_SIZE, new Vector3(0f, 1f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(-1f, 0f, 0f) * MISSILE_MODEL_SIZE * TRIANGLE_RELATIVE_SIZE, new Vector3(0f, 0f, 0f), Color.Green)
            };
            RotationMatrix = Matrix.CreateRotationX(-MathF.PI / 7);
            Position = position;
        }

        public override void Initialize(){
            MissileBody.Initialize();
            MissileHead.Initialize();
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Initialize();
        }
        
        public override void Update(){
            Console.WriteLine("{0}", Position);
            RotationMatrix *= Matrix.CreateRotationY(ROTATION_SPEED * TGCGame.GetElapsedTime());
            var forward = Vector3.Normalize(RotationMatrix.Forward);

            MissileBody.Update(Position, forward, RotationMatrix);
            MissileHead.Update(Position, forward, RotationMatrix);
            for (int i = 0; i < TRIANGLES_QUANTITY; i++) MissileTriangles[i].Update(Position, forward, RotationMatrix);
        }

        public override void Draw(Matrix view, Matrix projection){
            MissileBody.Draw(view, projection);
            MissileHead.Draw(view, projection);
            for (int i = 0; i < TRIANGLES_QUANTITY; i++)   MissileTriangles[i].Draw(view, projection);
        }
    }
}