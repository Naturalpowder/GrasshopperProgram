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


        public Surface(Rhino.Geometry.Brep surface)
        {
            baseSurface = new List<Point>(surface.Vertices.ToList().Select(e => new Point(e.Location.X, e.Location.Y, e.Location.Z)));
        }

        public Brep ToRhinoSurface()
        {
            PolylineCurve curve = Util.ToRhinoPolylineCurve(baseSurface);
            return Brep.CreatePlanarBreps(curve, .1)[0];
        }
    }
}