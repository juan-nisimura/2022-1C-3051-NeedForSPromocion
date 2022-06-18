using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TGC.Monogame.TP.Src.IALogicalMaps
{
    public abstract class IALogicalMap
    {
        public static IAMapBox[] AllIABoxes = new IAMapBox[100];
        public static int BoxesQuantity = 0;
        public static IAMapBox[,,] Bitmap = new IAMapBox[1421, 1421, 2];
        public static Ray Ray = new Ray(new Vector3(0f, 1000f, 0f), new Vector3(0f, -1f, 0f));

        public static void AddBox(IAMapBox mapBox) {
            AllIABoxes[BoxesQuantity] = mapBox;
            BoxesQuantity ++;
        }

        public static Dictionary<IAMapBox, bool> GenerateBannedBoxDictionary() {
            var dictionary = new Dictionary<IAMapBox, bool>();
            for(int i = 0; i < BoxesQuantity; i++)
                dictionary.Add(AllIABoxes[i], false);
            return dictionary;
        }

        public static void SetIAMapBox(int positionX, int positionZ, IAMapBox mapBox, int level){
            Bitmap[positionX + 710, positionZ + 710, level] = mapBox;
        }

        public static IAMapBox GetIAMapBox(int positionX, int positionZ, int level) {
            return Bitmap[positionX + 710, positionZ + 710, level];
        }

        public static IAMapBox GetIAMapBox(float positionX, float positionZ, int level) {
            return Bitmap[(int) MathF.Round(positionX) + 710, (int) MathF.Round(positionZ) + 710, level];
        }

        public static IAMapBox GetIAMapBox(Vector3 position) {
            return GetIAMapBox(position.X, position.Z, GetActualLevel(position.Y));
        }

        public static void MoveRay(int x, int z) {
            Ray.Position = new Vector3(x, 1000f, z);
        }

        public static void SetIAMapBoxIfGreater(int x, int z, IAMapBox mapBox, int level)
        {
            if(GetIAMapBox(x, z, level).GetHeight() < mapBox.GetHeight())
                SetIAMapBox(x, z, mapBox, level);
        }

        public static int GetActualLevel(float actualHeight){
            if(actualHeight > 25f)
                return 1;
            return 0;
        }

        internal static Vector3 GetTargetPositionInAdjacetCell(Vector3 position, Vector3 targetPosition)
        {
            var targetIAMapBox = GetIAMapBox(targetPosition);
            var actualIAMapBox = GetIAMapBox(position);
            
            // Si están en la misma celda, devuelvo la posicion target
            if(targetIAMapBox == actualIAMapBox)
                return targetPosition;
            
            // Creo una lista de "banned" boxes (las que ya se recorrieron)
            var bannedIABoxes = GenerateBannedBoxDictionary();
            bannedIABoxes.Remove(actualIAMapBox);
            bannedIABoxes.Add(actualIAMapBox, true);
            
            // Uso una cola para buscar por el camino más corto
            Queue<IAMapBox> Cola = new Queue<IAMapBox>();
            
            // Encolo los elementos conectados a la celda actual
            for(int i = 0; i < actualIAMapBox.ConnectedBoxes.Length; i++){
                // Seteo a ellos mismos como la "raíz" (el box que se convertiría en target si es el del camino más corto)
                actualIAMapBox.ConnectedBoxes[i].SetRaiz(actualIAMapBox.ConnectedBoxes[i]);
                Cola.Enqueue(actualIAMapBox.ConnectedBoxes[i]);
            }

            IAMapBox queuedBox = actualIAMapBox;

            // Busqueda por niveles hasta que no queden elementos
            while(Cola.Count > 0) {
                queuedBox = Cola.Dequeue();
                
                // Si está baneado, salto al siguiente ciclo
                if(bannedIABoxes.GetValueOrDefault(queuedBox, true))
                    continue;

                // Si es el target, termino (devuelve la posicion de la raiz)
                if(targetIAMapBox == queuedBox)
                    return queuedBox.GetRaiz().Position;

                // Si no es el target ni está baneado, lo baneo y agrego sus hijos a la cola
                bannedIABoxes.Remove(queuedBox);
                bannedIABoxes.Add(queuedBox, true);
                for(int i = 0; i < queuedBox.ConnectedBoxes.Length; i++){
                    // Asigno la misma raiz que su padre
                    queuedBox.ConnectedBoxes[i].SetRaiz(queuedBox.GetRaiz());
                    Cola.Enqueue(queuedBox.ConnectedBoxes[i]);
                }
            }

            // Si no se encuentra, por defecto devuelve el targetPosition
            return targetPosition; 
        }
    }
}