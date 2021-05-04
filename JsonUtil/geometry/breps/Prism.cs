using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;
using JsonUtil;

namespace geometry.breps
{
    public class Prism : Geo
    {
        public List<Point> baseSurface { set; get; }
        public Point height { set; get; }

        public Prism()
        {

        }

        public Prism(Rhino.Geometry.Extrusion extrusion)
        {
            initial();
        }

        public Extrusion ToRhinoPrism()
        {
            PolylineCurve curve = Util.ToRhinoPolylineCurve(baseSurface);
            return Extrusion.Create(curve, height.z, true);
        }
    }
}