using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;

namespace geometry
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

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                  IsEqual(x, point.x) &&
                   IsEqual(y, point.y) &&
                   IsEqual(z, point.z);
        }

        public bool IsEqual(double a, double b)
        {
            return Math.Abs(a - b) < .001;
        }

        public override int GetHashCode()
        {
            int hashCode = 373119288;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            hashCode = hashCode * -1521134295 + z.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return "[ " + x + " , " + y + " , " + z + " ]";
        }
    }
}
