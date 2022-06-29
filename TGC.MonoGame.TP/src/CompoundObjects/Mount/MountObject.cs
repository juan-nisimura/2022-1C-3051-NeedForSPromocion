using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.Monogame.TP.Src.Screens;

namespace TGC.Monogame.TP.Src.CompoundObjects.Mount 
{
    class MountObject : DefaultObject <MountObject>
    {
        protected MountBoxObject Box { get; set; }
        protected MountRampObject[] Ramps { get; set; }
        protected BoundingBox VisibleBoundingBox;
        public MountObject(Vector3 position, Vector3 size, float rotation, Color color){
            Box = new MountBoxObject(position, size, color);
            Ramps = new MountRampObject[] {
                new MountRampObject(position + new Vector3(size.X * 6 / 10, 0f, 0f), new Vector3(size.X/5,size.Y,size.Z), rotation, color),
                new MountRampObject(position + new Vector3(-size.X * 6 / 10, 0f, 0f), new Vector3(size.X/5,size.Y,size.Z), rotation + MathF.PI, color),
                new MountRampObject(position + new Vector3(0f, 0f, size.Z * 6 / 10), new Vector3(size.X/5,size.Y,size.Z), rotation - MathF.PI/2, color),
                new MountRampObject(position + new Vector3(0f, 0f, -size.Z * 6 / 10), new Vector3(size.X/5,size.Y,size.Z), rotation + MathF.PI/2, color)
            };
            VisibleBoundingBox = new BoundingBox(position - size * 6 / 10, position + size * 6 / 10);
        }

        protected override bool IsVisible() 
        {
            return LevelScreen.GetBoundingFrustum().Intersects(VisibleBoundingBox);
        }

        public new void Initialize(){
            Box.Initialize();
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].Initialize();
        }

        public override void Update(){   
        }
        
        public static void Load(){
            MountBoxObject.Load("BoxTextureShader", "rock_01_diff_4k");
            MountRampObject.Load("RampTextureShader", "rock_01_diff_4k");
        }

        public void SolveBulletCollision(BulletObject bullet){                
            Box.SolveBulletCollision(bullet);
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].SolveBulletCollision(bullet);
        }

        public void SolveMissileCollision(MissileObject missile){                
            Box.SolveMissileCollision(missile);
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].SolveMissileCollision(missile);
        }

        public bool SolveHorizontalCollision(CarObject car){
            bool collided = false;
            collided = Box.SolveHorizontalCollision(car);
            for (int i = 0; i < Ramps.Length; i++)  collided = collided || Ramps[i].SolveHorizontalCollision(car);
            return collided;
        }

        public override void Draw(Matrix view, Matrix projection){
            if(IsVisible()){
                Box.Draw(view, projection);
                for (int i = 0; i < Ramps.Length; i++)  Ramps[i].Draw(view, projection);
            }
        }

        internal bool SolveVerticalCollision(CarObject car)
        {
            bool collided = false;
            collided = Box.SolveVerticalCollision(car);
            for (int i = 0; i < Ramps.Length; i++)  collided = collided || Ramps[i].SolveVerticalCollision(car);
            return collided;
        }

        internal void UpdateHeightMap(int x, int z, int level)
        {
            Box.UpdateHeightMap(x, z, level);
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].UpdateHeightMap(x, z, level);
        }

        internal void UpdateIALogicalMap(int x, int z, int level)
        {
            Box.UpdateIALogicalMap(x, z, level);
            for (int i = 0; i < Ramps.Length; i++)  Ramps[i].UpdateIALogicalMap(x, z, level);
        }
    }
}
