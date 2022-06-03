using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    public class BulletObject : DefaultObject <MissileObject>
    {
        protected BulletBodyObject BulletBody { get; set; }
        protected BulletHeadObject BulletHead { get; set; }
        public Vector3 bulletPosition { get; set; }
        public float bulletRotationY { get; set; }
        
        public BulletObject(GraphicsDevice graphicsDevice, Vector3 position, float size, float rotationY){
            bulletPosition = position;
            bulletRotationY = rotationY;
            BulletBody = new BulletBodyObject(graphicsDevice, position + new Vector3(0f, size/2, 0f), new Vector3(size/2, size, size/2),MathHelper.PiOver2, rotationY, Color.Black);
            BulletHead = new BulletHeadObject(graphicsDevice, position + new Vector3(0f, size, size/2), new Vector3(0.5f, 1f, 0.5f) * size,rotationY, Color.Red);
        }

        public new void Initialize(){
            BulletBody.Initialize();
            BulletHead.Initialize();
        }
        
        public static void Load(ContentManager content){
            BulletBodyObject.Load(content, "BasicShader");
            BulletHeadObject.Load(content, "BasicShader");
        }
        public override void Update(GameTime gameTime){
            BulletBody.Update(gameTime);
            BulletHead.Update(gameTime);
        }
        public void Update(GameTime gameTime, Vector3 Position, float Rotation,float Speed){
            /*var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationY(Rotation);
            Position = new Vector3(Position.X - 100 * elapsedTime, 0, Position.Z - 100 * elapsedTime);
            World *= Matrix.CreateTranslation(Position);*/
            BulletBody.Update(gameTime,  Position,  Rotation, Speed);
            BulletHead.Update(gameTime,  Position,  Rotation, Speed);
        }

        public override void Draw(Matrix view, Matrix projection){
            BulletBody.Draw(view, projection);
            BulletHead.Draw(view, projection);
        }
    }
}