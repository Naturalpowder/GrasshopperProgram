using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using JsonUtil;

namespace geometry.breps
{
    public class PolySurface : Geo
    {
        public List<Point> points;
        public List<int[]> faces;

        public PolySurface()
        {

        }

        public PolySurface(Brep brep)
        {
            points = Util.ToPoints(brep);
            faces = new List<int[]>();
            foreach (BrepFace face in brep.Faces)
            {
                List<Point> temp = Util.ToPoints(face.ToBrep());
                faces.Add(Util.GetFaceIndex(points, temp));
            }
            initial();
        }
    }
}
