/*using System;
using Microsoft.Xna.Framework;

namespace TGC.Monogame.TP.Src.IALogicalMaps   
{
    public abstract class IAMapLogicalMap
    {
        public static IAMapBox[,] Bitmap = new IAMapBox[1421, 1421];

        public static Ray Ray = new Ray(new Vector3(0f, 1000f, 0f), new Vector3(0f, -1f, 0f));

        public static void SetIAMapBox(int positionX, int positionZ, IAMapBox mapBox){
            Bitmap[positionX + 710, positionZ + 710] = mapBox;
        }

        public static IAMapBox GetIAMapBox(int positionX, int positionZ) {
            return Bitmap[positionX + 710, positionZ + 710];
        }

        public static IAMapBox GetIAMapBox(float positionX, float positionZ) {
            return Bitmap[(int) MathF.Round(positionX) + 710, (int) MathF.Round(positionZ) + 710];
        }

        public static void MoveRay(int x, int z) {
            Ray.Position = new Vector3(x, 1000f, z);
        }

        public static void SetIAMapBoxIfGreater(int x, int z, IAMapBox mapBox)
        {
            SetHeight(x, z, MathF.Max(GetHeight(x, z), height));
        }

        public static float GetDifferential(Vector3 position, Vector3 forward)
        {
            Vector3 normalized = Vector3.Normalize(forward) * 10f;
            return GetHeight(position.X + normalized.X, position.Z + normalized.Z) -
                    GetHeight(position.X, position.Z);
        }

        public static float GetDifferentialAngle(Vector3 position, Vector3 forward)
        {
            return MathF.Atan(GetDifferential(position, forward)/10f);
        }
    }
}*/