using Microsoft.Xna.Framework;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.PrimitiveObjects;

namespace TGC.Monogame.TP.Src.CompoundObjects.Map
{
    class FloorObject : QuadObject <FloorObject>
    {
        public IAMapBox IAMapBox = new IAMapBox(new BoundingBox(new Vector3(-710, 0 , -710), new Vector3(710, 0, 710)), Vector3.Zero, 10);
        
        private Plane Plane = new Plane(new Vector4(0f, 1f, 0f, 0f));
        public FloorObject(Vector3 position, Vector3 size, float rotation)
            : base(position, size, rotation, Color.Black){            
        }

        public void SolveBulletCollision(BulletObject bullet){
            if(bullet.ImpactSphere.Intersects(Plane) == PlaneIntersectionType.Back)
                bullet.HitObstacle();
        }

        public void SolveMissileCollision(MissileObject missile){
            if(missile.ImpactSphere.Intersects(Plane) == PlaneIntersectionType.Back)
                missile.HitObstacle();
        }
    }
}
