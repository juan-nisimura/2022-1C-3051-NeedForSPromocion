using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.Samples.Collisions;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class BoxObject <T> : CubeObject <T>
    {

        private BoundingBox BoundingBox; 
        private float MaxHeight;
        public BoxObject(Vector3 position, Vector3 size, Color color) : base(position, size, color)
        {
            MaxHeight = position.Y + size.Y / 2;
            BoundingBox = new BoundingBox(position - size/2, position + size/2);
        }

        public void Update(CarObject car){
        }

        public void UpdateHeightMap(int x, int z) {
            var intersects = HeightMap.Ray.Intersects(BoundingBox);
            if(intersects != null) {  
                HeightMap.SetHeightIfGreater(x, z, HeightMap.Ray.Position.Y - intersects.GetValueOrDefault());
            }
        }

        public void SolveBulletCollision(BulletObject bullet){
            if(bullet.ImpactSphere.Intersects(BoundingBox))
                bullet.HitObstacle();
        }
        public void SolveMissileCollision(MissileObject missile){
            if(missile.ImpactSphere.Intersects(BoundingBox))
                missile.HitObstacle();
        }

        public bool SolveHorizontalCollision(CarObject car){
            // Chequeo si colisionó con el auto
            if(car.ObjectBox.Intersects(BoundingBox)){

                // Si colisionó con el auto, el auto es empujado
                
                // Si el auto roza la parte de arriba del bloque, no es empujado
                if(car.ObjectBox.Center.Y + 7f > MaxHeight)
                    return false;

                // Get the cylinder center at the same Y-level as the box
                var sameLevelCenter = car.ObjectBox.Center;
                sameLevelCenter.Y = BoundingVolumesExtensions.GetCenter(BoundingBox).Y;

                // Find the closest horizontal point from the box
                var closestPoint = BoundingVolumesExtensions.ClosestPoint(BoundingBox, sameLevelCenter);

                // Calculate our normal vector from the "Same Level Center" of the cylinder to the closest point
                // This happens in a 2D fashion as we are on the same Y-Plane
                var normalVector = sameLevelCenter - closestPoint;
                var normalVectorLength = normalVector.Length();

                // Calculo la distancia al centro del auto en la direccion del vector normal
                var normalVectorNormalized = Vector3.Normalize(normalVector);

                var forward = car.ObjectBox.Orientation.Forward;
                forward = new Vector3(forward.X, 0f, forward.Z);
                
                var angulo = MathF.Acos(Convert.ToSingle(Vector3.Dot(Vector3.Normalize(forward), normalVectorNormalized)));
                angulo = MathF.PI / 2 - MathF.Abs(MathF.Abs(angulo) - MathF.PI / 2);
                
                float distanciaAlCentroDelAuto = CarObject.HIPOTENUSA_AL_VERTICE * MathF.Cos(MathF.Abs(angulo - CarObject.ANGULO_AL_VERTICE));
                
                
                // La penetración es la diferencia entre la distancia al centro del auto y la longitud del vector normal
                // Utilizo 0.1 como mínimo para evitar loops infinitos
                var penetration = MathF.Max(distanciaAlCentroDelAuto - normalVectorLength, 0.1f);

                // Empuja el centro del auto fuera del Box
                car.ObjectBox.Center += (normalVectorNormalized * penetration);
                car.Position = car.ObjectBox.Center;
                car.HasCrashed = true;

                return true;
            } 
                
            return false;
        }

        protected override void DrawPrimitive() { BoxPrimitive.Draw(getEffect()); }

        internal bool SolveVerticalCollision(CarObject car)
        {
            if(car.ObjectBox.Intersects(BoundingBox)){
                var posibleNewGroundLevel = HeightMap.GetHeight(car.Position.X, car.Position.Z);
                if(posibleNewGroundLevel - car.GroundLevel < 1f){
                    car.GroundLevel = posibleNewGroundLevel;
                }
                    
                else
                    car.GroundLevel = 0;
            }
            
            return false;
        }
    }
}
