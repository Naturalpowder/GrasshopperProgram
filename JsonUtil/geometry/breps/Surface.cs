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

        public Surface()
        {

        }

        public Surface(IList<Point> points)
        {
            this.baseSurface = new List<Point>(points);
        }

        public Surface(Rhino.Geometry.Brep surface)
        {
            Curve curve = surface.Faces[0].OuterLoop.To3dCurve();
            baseSurface = Util.ToPoints(curve);
            initial();
        }

        public Brep ToRhinoSurface()
        {
            PolylineCurve curve = Util.ToRhinoPolylineCurve(baseSurface);
            return Brep.CreatePlanarBreps(curve, .1)[0];
        }
    }
}