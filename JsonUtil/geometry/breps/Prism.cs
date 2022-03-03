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
        public Surface baseSurface { set; get; }
        public Point height { set; get; }

        public Prism()
        {

        }

        public Prism(Rhino.Geometry.Extrusion extrusion)
        {
            initial();
        }

        public Brep ToRhinoPrism()
        {
            BrepFace face = baseSurface.ToRhinoSurface().Faces[0];
            Point3d start = baseSurface.points[0].ToRhinoPoint();
            Point3d end = Point3d.Add(start, height.ToRhinoPoint());
            PolylineCurve line = new PolylineCurve(new List<Point3d>() { start, end });
            return face.CreateExtrusion(line, true);
        }
    }
}