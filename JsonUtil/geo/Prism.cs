using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;

namespace JsonUtil
{
    public class Prism : Geo
    {
        public List<Point> baseSurface { set; get; }
        public Point height { set; get; }

        public Extrusion ToRhinoPrism()
        {
            PolylineCurve curve = Util.ToRhinoPolylineCurve(baseSurface);
            return Extrusion.Create(curve, height.z, true);
        }
    }
}