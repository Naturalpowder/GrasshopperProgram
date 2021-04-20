using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;

namespace JsonUtil
{
    public class Surface : Geo
    {
        public List<Point> baseSurface { set; get; }

        public Brep toRhinoSurface()
        {
            PolylineCurve curve = Util.ToRhinoPolylineCurve(baseSurface);
            return Brep.CreatePlanarBreps(curve, .1)[0];
        }
    }
}