using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;

namespace JsonUtil
{
    public class Point
    {
        public double x { set; get; }
        public double y { set; get; }
        public double z { set; get; }


        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Point3d ToRhinoPoint()
        {
            return new Point3d(x, y, z);
        }
    }
}
