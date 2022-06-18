using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.Monogame.TP.Src.IALogicalMaps
{
    public class IAMapBox
    {
        private IAMapBox Raiz;
        public BoundingBox BoundingBox;
        public Vector3 Position;
        public IAMapBox[] ConnectedBoxes;
        private float Height;
        private int ConnectedBoxesQuantity = 0;
        public void AddIAMapBox(IAMapBox mapBox) {
            ConnectedBoxes[ConnectedBoxesQuantity] = mapBox;
            ConnectedBoxesQuantity++;
        }

        public IAMapBox GetRaiz(){
            return Raiz;
        }
        public void SetRaiz(IAMapBox raiz){
            this.Raiz = raiz;
        }

        public IAMapBox AddIAMapBoxes(IAMapBox[] mapBoxes) {
            for(int i = 0; i < mapBoxes.Length; i++)
                AddIAMapBox(mapBoxes[i]);
            return this;
        }

        public IAMapBox(BoundingBox boundingBox, Vector3 position, int connectedBoxesMaxQuantity) {
            this.BoundingBox = boundingBox;
            this.Position = position;
            this.Height = BoundingBox.Max.Y;
            this.ConnectedBoxes = new IAMapBox[connectedBoxesMaxQuantity];
            IALogicalMap.AddBox(this);
        }

        public float GetHeight() {
            return Height;
        }

        public void SetConnectedBoxes(IAMapBox[] connectedBoxes) {
            this.ConnectedBoxes = connectedBoxes;
        }
    }
}