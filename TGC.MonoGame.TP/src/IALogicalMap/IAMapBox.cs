using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TGC.Monogame.TP.Src.IALogicalMaps
{
    public class IAMapBox
    {
        private BoundingBox Box;

        private IAMapBox[] ConnectedBoxes;

        public IAMapBox(BoundingBox boundingBox, IAMapBox[] connectedBoxes) {
            this.Box = boundingBox;
            this.ConnectedBoxes = connectedBoxes;
        }
    }
}