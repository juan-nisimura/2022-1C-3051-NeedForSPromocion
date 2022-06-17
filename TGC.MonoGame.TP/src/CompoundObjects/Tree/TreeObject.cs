using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.Monogame.TP.Src.ModelObjects;
using System;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles;

namespace TGC.Monogame.TP.Src.CompoundObjects.Tree
{
    class TreeObject : DefaultObject <TreeObject>
    {
        protected TreeTrunkObject TreeTrunk { get; set; }
        protected TreeTopObject TreeTop { get; set; }
        public TreeObject(Vector3 position, float size){
            TreeTrunk = new TreeTrunkObject(position + new Vector3(0f, size/2, 0f), new Vector3(size/2, size, size/2), 0, Color.Brown);
            TreeTop = new TreeTopObject(position + new Vector3(0f, size * 5/3, 0f), size * 5 / 3, Color.ForestGreen);
        }

        public new void Initialize(){
            TreeTrunk.Initialize();
            TreeTop.Initialize();
        }
        public static void Load(){
            TreeTrunkObject.Load("TreeTrunkShader", "bark brown/textures/bark_brown_01_diff_4k");
            //TreeTopObject.Load(content, "TreeTopShader", "forest leaves/textures/forest_leaves_03_diff_4k");
            TreeTopObject.Load("BasicShader", "Floor");
        }

        public override void Update(){   
        }

        public override void Draw(Matrix view, Matrix projection){
            TreeTrunk.Draw(view, projection);
            TreeTop.Draw(view, projection);
        }

        public bool SolveHorizontalCollision(CarObject car)
        {
            return TreeTrunk.SolveHorizontalCollision(car);
        }

        public void SolveBulletCollision(BulletObject bullet){
            TreeTrunk.SolveBulletCollision(bullet);
        }

        public void SolveMissileCollision(MissileObject missile){
            TreeTrunk.SolveMissileCollision(missile);
        }

        internal void UpdateHeightMap(int x, int z, int level)
        {
            TreeTrunk.UpdateHeightMap(x, z, level);
        }
    }
}
