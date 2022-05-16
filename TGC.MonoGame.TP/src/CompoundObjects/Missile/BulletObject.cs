using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TGC.Monogame.TP.Src.CompoundObjects.Missile
{
    class BulletObject : DefaultObject <MissileObject>
    {
        protected MissileBodyObject BulletBody { get; set; }
        protected MissileHeadObject BulletHead { get; set; }
        
        public BulletObject(GraphicsDevice graphicsDevice, Vector3 position, float size, float rotationY){
            BulletBody = new MissileBodyObject(graphicsDevice, position + new Vector3(0f, size/2, 0f), new Vector3(size/2, size, size/2),MathHelper.PiOver2, rotationY, Color.Gray);
            BulletHead = new MissileHeadObject(graphicsDevice, position + new Vector3(0f, size/2, size/2), new Vector3(0.5f, 0.5f, 1f) * size,rotationY, Color.Red);
        }

        public new void Initialize(){
            BulletBody.Initialize();
            BulletHead.Initialize();
        }
        
        public static void Load(ContentManager content){
            MissileBodyObject.Load(content, "BasicShader");
            MissileHeadObject.Load(content, "BasicShader");
        }
        public override void Update(GameTime gameTime){
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            World *= Matrix.CreateTranslation(Vector3.UnitX*elapsedTime);
        }
        public void Update(GameTime gameTime, Vector3 Position, float Rotation){
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            World = ScaleMatrix;
            World *= Matrix.CreateRotationY(Rotation);
            Position = new Vector3(Position.X - 100 * elapsedTime, 0, Position.Z - 100 * elapsedTime);
            World *= Matrix.CreateTranslation(Position);
            BulletBody.Update(gameTime);
            BulletHead.Update(gameTime);
        }

        public override void Draw(Matrix view, Matrix projection){
            BulletBody.Draw(view, projection);
            BulletHead.Draw(view, projection);
        }
    }
}