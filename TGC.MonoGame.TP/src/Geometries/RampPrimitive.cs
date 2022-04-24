#region File Description

//-----------------------------------------------------------------------------
// RampPrimitive.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion Using Statements

namespace TGC.MonoGame.TP.Src.Geometries
{
    /// <summary>
    ///     Geometric primitive class for drawing cubes.
    /// </summary>
    public class RampPrimitive : GeometricPrimitive
    {
        public RampPrimitive(GraphicsDevice graphicsDevice) : this(graphicsDevice, 1, Color.White, Color.White,
            Color.White, Color.White, Color.White)
        {
        }

        public RampPrimitive(GraphicsDevice graphicsDevice, float size, Color color) : this(graphicsDevice, size, color,
            color, color, color, color)
        {
        }

        /// <summary>
        ///     Constructs a new cube primitive, with the specified size.
        /// </summary>
        public RampPrimitive(GraphicsDevice graphicsDevice, float size, Color color1, Color color2, Color color3,
            Color color4, Color color5)
        {
            // A ramp has five faces, each one pointing in a different direction.
            Vector3[] squareNormals =
            {
                // back normal
                -Vector3.UnitX,
                // bottom normal
                -Vector3.UnitY
            };

            Vector3 rightTriangleNormal = -Vector3.UnitZ;
            Vector3 leftTriangleNormal = Vector3.UnitZ;
            Vector3 rampNormal = new Vector3(1f,1f,0f);

            Color[] colors =
            {
                color1, color2, color3, color4, color5
            };

            var i = 0;
            // Create bottom and back face.
            foreach (var normal in squareNormals)
            {
                // Get two vectors perpendicular to the face normal and to each other.
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                // Six indices (two triangles) per face.
                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 1);
                AddIndex(CurrentVertex + 2);

                AddIndex(CurrentVertex + 0);
                AddIndex(CurrentVertex + 2);
                AddIndex(CurrentVertex + 3);

                // Four vertices per face.
                AddVertex((normal - side1 - side2) * size / 2, colors[i], normal);
                AddVertex((normal - side1 + side2) * size / 2, colors[i], normal);
                AddVertex((normal + side1 + side2) * size / 2, colors[i], normal);
                AddVertex((normal + side1 - side2) * size / 2, colors[i], normal);

                i++;
            }

            // Create right triangle face.
            // Three indices (one triangle) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            // Three vertices per face.
            AddVertex((rightTriangleNormal + new Vector3(-1f, -1f, 0f)) * size / 2, colors[i], rightTriangleNormal);
            AddVertex((rightTriangleNormal + new Vector3(1f, -1f, 0f)) * size / 2, colors[i], rightTriangleNormal);
            AddVertex((rightTriangleNormal + new Vector3(-1f, 1f, 0f)) * size / 2, colors[i], rightTriangleNormal);

            i++;

            // Create left triangle face.
            // Three indices (one triangle) per face.
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            // Three vertices per face.
            AddVertex((leftTriangleNormal + new Vector3(-1f, -1f, 0f)) * size / 2, colors[i], leftTriangleNormal);
            AddVertex((leftTriangleNormal + new Vector3(-1f, 1f, 0f)) * size / 2, colors[i], leftTriangleNormal);
            AddVertex((leftTriangleNormal + new Vector3(1f, -1f, 0f)) * size / 2, colors[i], leftTriangleNormal);

            i++;

            // Create ramp face.
            // Six indices (two triangles) per face.
            
            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 1);
            AddIndex(CurrentVertex + 2);

            AddIndex(CurrentVertex + 0);
            AddIndex(CurrentVertex + 2);
            AddIndex(CurrentVertex + 3);

            // Four vertices per face.
            AddVertex(new Vector3(1f, -1f, -1f) * size / 2, colors[i], rampNormal);
            AddVertex(new Vector3(1f, -1f, 1f) * size / 2, colors[i], rampNormal);
            AddVertex(new Vector3(-1f, 1f, 1f) * size / 2, colors[i], rampNormal);
            AddVertex(new Vector3(-1f, 1f, -1f) * size / 2, colors[i], rampNormal);

            i++;

            InitializePrimitive(graphicsDevice);
        }
    }
}