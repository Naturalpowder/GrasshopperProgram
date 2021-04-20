using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;

namespace JsonUtil
{
    public class Circle : Geo
    {
        public Point origin { set; get; }
        public double radius { set; get; }

        public Rhino.Geometry.Circle ToRhinoCircle()
        {
            return new Rhino.Geometry.Circle(origin.ToRhinoPoint(), radius);
        }
    }
}