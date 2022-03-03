using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;
using JsonUtil;
using geometry.face;

namespace geometry.breps
{
    public class Surface : Geo
    {
        public List<Point> points { set; get; }
        public List<IndexFace> faces { set; get; }

        public Surface()
        {

        }

        public Surface(Rhino.Geometry.Brep surface)
        {
            points = Util.ToPoints(surface);
            List<BrepLoop> loops = new List<BrepLoop>(surface.Loops);
            faces = new List<IndexFace>
            {
                Util.GetFaceIndex(loops, points)
            };
            initial();
        }

        public Brep ToRhinoSurface()
        {
            return Util.ToRhinoSurface(points, faces[0]);
        }
    }
}