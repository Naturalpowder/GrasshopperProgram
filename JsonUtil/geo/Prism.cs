using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;

namespace JsonUtil
{
    public class Prism : Geo
    {
        public List<Point> baseSurface { set; get; }
        public Point height { set; get; }

        public Prism(Extrusion extrusion)
        {
            Brep brep;
            baseSurface = new List<Point>(extrusion.Profile3d(0, 0).ToPolyline(.1, .1, .1, Double.MaxValue).ToPolyline().Select(e => new Point(e.X, e.Y, e.Z)));
            Point3d start = extrusion.PathStart;
            Point3d end = extrusion.PathEnd;
            height = new Point(end.X - start.X, end.Y - start.Y, end.Z - start.Z);
        }

        public Extrusion ToRhinoPrism()
        {
            PolylineCurve curve = Util.ToRhinoPolylineCurve(baseSurface);
            return Extrusion.Create(curve, height.z, true);
        }
    }
}