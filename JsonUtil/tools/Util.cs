using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;
using geometry;

namespace JsonUtil
{
    public class Util
    {

        public static PolylineCurve ToRhinoPolylineCurve(List<geometry.Point> baseSurface)
        {
            List<Point3d> pts = baseSurface.Select(e => e.ToRhinoPoint()).ToList();
            pts.Add(pts[0]);
            PolylineCurve curve = new PolylineCurve(pts);
            return curve;
        }
    }
}
