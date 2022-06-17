using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.Monogame.TP.Src   
{
    public abstract class HeightMap
    {
        public static float[,,] Bitmap = new float[1421, 1421, 2];

        public static Ray Ray = new Ray(new Vector3(0f, 1000f, 0f), new Vector3(0f, -1f, 0f));

        public static void SetHeight(int positionX, int positionZ, float height, int level){
            Bitmap[positionX + 710, positionZ + 710, level] = height;
        }

        public static float GetHeight(int positionX, int positionZ, int level) {
            return Bitmap[positionX + 710, positionZ + 710, level];
        }

        public static float GetHeight(float positionX, float positionZ, int level) {
            return Bitmap[(int) MathF.Round(positionX) + 710, (int) MathF.Round(positionZ) + 710, level];
        }

        public static void MoveRay(int x, int z) {
            Ray.Position = new Vector3(x, 1000f, z);
        }

        public static void SetHeightIfGreater(int x, int z, float height, int level)
        {
            SetHeight(x, z, MathF.Max(GetHeight(x, z, level), height), level);
        }

        public static float GetDifferential(Vector3 position, Vector3 forward, int level)
        {
            Vector3 normalized = Vector3.Normalize(forward) * 10f;
            return GetHeight(position.X + normalized.X, position.Z + normalized.Z, level) -
                    GetHeight(position.X, position.Z, level);
        }

        public static float GetDifferentialAngle(Vector3 position, Vector3 forward, int level)
        {
            return MathF.Atan(GetDifferential(position, forward, level)/10f);
        }

        public static int GetActualLevel(float actualHeight){
            if(actualHeight > 25f)
                return 1;
            return 0;
        }
    }
}