using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;

namespace geometry.vectors
{
    public class Vector : Geo
    {
        public Point origin { set; get; }
        public Point vector { set; get; }

        public Vector()
        {

        }

        public Vector(Rhino.Geometry.Point3d vector)
        {
            origin = new Point(0, 0, 0);
            this.vector = new Point(vector.X, vector.Y, vector.Z);
            initial();
        }

        public Vector3d ToRhinoVector()
        {
            return new Vector3d(vector.x, vector.y, vector.z);
        }
    }
}
