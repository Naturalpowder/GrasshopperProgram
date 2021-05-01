using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;
using JsonUtil;

namespace geometry.breps
{
    public class Surface : Geo
    {
        public List<Point> baseSurface { set; get; }


        public Surface(Brep surface)
        {
            baseSurface = new List<Point>(surface.Vertices.ToList().Select(e => new Point(e.Location.X, e.Location.Y, e.Location.Z)));
            initial();
        }

        public Brep ToRhinoSurface()
        {
            PolylineCurve curve = Util.ToRhinoPolylineCurve(baseSurface);
            return Brep.CreatePlanarBreps(curve, .1)[0];
        }
    }
}