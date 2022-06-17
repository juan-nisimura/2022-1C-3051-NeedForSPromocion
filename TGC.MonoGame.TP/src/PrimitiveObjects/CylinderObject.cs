using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.Samples.Collisions;
using TGC.MonoGame.TP;
using TGC.MonoGame.TP.Src.Geometries;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    public class CylinderObject<T> : DefaultPrimitiveObject <T>
    {
        protected BoundingCylinder BoundingCylinder;
        protected CylinderPrimitive CylinderPrimitive { get; }
        public CylinderObject(Vector3 position, Vector3 size, float rotationX, float rotationY, Color color){
            CylinderPrimitive = new CylinderPrimitive(TGCGame.GetGraphicsDevice());
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.CreateRotationY(rotationY);
            DiffuseColor = color.ToVector3();
            BoundingCylinder = new BoundingCylinder(position, size.X / 2, size.Y/2);
        }
        protected override void DrawPrimitive(Effect effect) { CylinderPrimitive.Draw(effect); }
    
        public void UpdateHeightMap(int x, int z, int level) {
            
            if(BoundingCylinder.Intersects(HeightMap.Ray)){
                var height = BoundingCylinder.HalfHeight * 2;
                HeightMap.SetHeightIfGreater(x, z, height, level);
            }
        }

        public void SolveBulletCollision(BulletObject bullet){
            if(BoundingCylinder.Intersects(bullet.ImpactSphere))
                bullet.HitObstacle();
        }

        public void SolveMissileCollision(MissileObject missile){
            if(BoundingCylinder.Intersects(missile.ImpactSphere))
                missile.HitObstacle();
        }

        public bool SolveHorizontalCollision(CarObject car){
            // Chequeo si colisionó con el auto
            
            if(car.ObjectBox.Intersects(BoundingCylinder)){

                // Si colisionó con el auto, el auto es empujado
                
                // Si el auto roza la parte de arriba del bloque, no es empujado
                //if(car.ObjectBox.Center.Y + 8f > MaxHeight)
                   // return false;

                // Get the car center at the same Y-level as the box
                var sameLevelCenter = car.ObjectBox.Center;
                sameLevelCenter.Y = BoundingCylinder.Center.Y;

                // Find the closest horizontal point from the box
                //var closestPoint = BoundingVolumesExtensions.ClosestPoint(BoundingCylinder, sameLevelCenter);

                // Calculate our normal vector from the "Same Level Center" of the cylinder to the closest point
                // This happens in a 2D fashion as we are on the same Y-Plane
                var normalVector = sameLevelCenter - BoundingCylinder.Center;
                var normalVectorLength = normalVector.Length();

                // Calculo la distancia al centro del auto en la direccion del vector normal
                var normalVectorNormalized = Vector3.Normalize(normalVector);

                var forward = car.ObjectBox.Orientation.Forward;
                //forward = new Vector3(forward.Z, 0f, - forward.X);
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
    }
}
