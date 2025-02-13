using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TGC.MonoGame.TP.Src.Geometries;
using TGC.Monogame.TP.Src.ModelObjects;
using TGC.MonoGame.Samples.Collisions;
using System;
using TGC.MonoGame.TP;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Bullet;
using TGC.Monogame.TP.Src.CompoundObjects.Projectiles.Missile;
using TGC.Monogame.TP.Src.IALogicalMaps;
using TGC.Monogame.TP.Src.Screens;

namespace TGC.Monogame.TP.Src.PrimitiveObjects
{
    class RampObject <T> : DefaultPrimitiveObject <T>
    {
        protected RampPrimitive RampPrimitive { get; }
        protected Vector3 Position;
        protected Vector3 Size;
        protected float Rotation;

        protected BoundingBox BoundingBox;
        protected Plane Plane;
        
        public IAMapBox IAMapBox;

        protected override bool IsVisible() 
        {
            return LevelScreen.GetBoundingFrustum().Intersects(BoundingBox);
        }

        public RampObject(Vector3 position, Vector3 size, float rotation, Color color, int connectedBoxesTotalQuantity, Vector3 IAMapBoxPosition){
            RampPrimitive = new RampPrimitive(TGCGame.GetGraphicsDevice());
            ScaleMatrix = Matrix.CreateScale(size);
            TranslateMatrix = Matrix.CreateTranslation(position);
            RotationMatrix = Matrix.CreateRotationY(rotation);
            DiffuseColor = color.ToVector3();

            Size = size;
            Position = position;
            Rotation = rotation;

            if(rotation == MathF.PI / 2 || rotation == -MathF.PI / 2)
                BoundingBox = new BoundingBox(position - new Vector3(size.Z, size.Y, size.X)/2, position + new Vector3(size.Z, size.Y, size.X)/2);
            else{
                BoundingBox = new BoundingBox(position - size/2, position + size/2);
            }
               
            Plane = Plane.Transform(new Plane(new Vector3(size.X, -size.Y, -size.Z)/2,
                                              new Vector3(-size.X, size.Y, size.Z)/2,
                                              new Vector3(size.X, -size.Y, size.Z)/2), 
                                            RotationMatrix);
            Plane = Plane.Transform(Plane, TranslateMatrix);

            IAMapBox = new IAMapBox(BoundingBox, position + new Vector3(0f, size.Y / 2, 0f) + IAMapBoxPosition, connectedBoxesTotalQuantity);
        }

        public override void Update(){
        }

        public void SolveBulletCollision(BulletObject bullet){
            if(BoundingBox.Intersects(bullet.ImpactSphere) && bullet.ImpactSphere.Intersects(Plane) == PlaneIntersectionType.Back)
                bullet.HitObstacle();
        }

        public void SolveMissileCollision(MissileObject missile){
            if(BoundingBox.Intersects(missile.ImpactSphere) && missile.ImpactSphere.Intersects(Plane) == PlaneIntersectionType.Back)
                missile.HitObstacle();
        }

        public bool SolveHorizontalCollision(CarObject car){
            
            // Chequeo si colisionó con el auto
            if(car.ObjectBox.Intersects(BoundingBox)){

                if(car.ObjectBox.Intersects(Plane) != PlaneIntersectionType.Front){
                    DiffuseColor = Color.Blue.ToVector3();
                    return false;
                }
                
                DiffuseColor = Color.Red.ToVector3();

                // Get the cylinder center at the same Y-level as the box
                var sameLevelCenter = car.ObjectBox.Center;
                sameLevelCenter.Y = Position.Y;

                // Find the closest horizontal point from the box
                var closestPoint = BoundingVolumesExtensions.ClosestPoint(BoundingBox, sameLevelCenter);

                // Calculate our normal vector from the "Same Level Center" of the cylinder to the closest point
                // This happens in a 2D fashion as we are on the same Y-Plane
                var normalVector = sameLevelCenter - closestPoint;
                var normalVectorLength = normalVector.Length();

                if(normalVectorLength == 0)
                    return false;

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

        protected override void DrawPrimitive(Effect effect) { RampPrimitive.Draw(effect); }

        public void UpdateHeightMap(int x, int z, int level) {
            if(HeightMap.Ray.Intersects(BoundingBox) != null){
                var intersects = HeightMap.Ray.Intersects(Plane);

                if(intersects != null) {  
                    HeightMap.SetHeightIfGreater(x, z, HeightMap.Ray.Position.Y - intersects.GetValueOrDefault(), level);
                }
            }
        }

        public void UpdateIALogicalMap(int x, int z, int level) {
            var intersects = IALogicalMap.Ray.Intersects(IAMapBox.BoundingBox);
            if(intersects != null) {  
                IALogicalMap.SetIAMapBoxIfGreater(x, z, IAMapBox, level);
            }
        }

        internal bool SolveVerticalCollision(CarObject car)
        {
            if(car.ObjectBox.Intersects(BoundingBox)){
                var posibleNewGroundLevel = HeightMap.GetHeight(car.Position.X, car.Position.Z, HeightMap.GetActualLevel(car.Position.Y));
                if(posibleNewGroundLevel - car.GroundLevel < 15f)
                    car.GroundLevel = posibleNewGroundLevel;
                //else
                    //car.GroundLevel = 0;
            }
            return false;
        }
    }
}
