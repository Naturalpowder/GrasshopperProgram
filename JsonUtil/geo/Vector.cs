using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;

namespace JsonUtil
{
    public class Vector : Geo
    {
        public Point origin { set; get; }
        public Point vector { set; get; }

        public Vector3d ToRhinoVector()
        {
            return new Vector3d(vector.x, vector.y, vector.z);
        }
    }
}
