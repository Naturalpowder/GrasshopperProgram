using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace geometry.breps
{
    public class PolySurface : Geo
    {
        public List<Point> points;
        public List<int[]> faces;

        public PolySurface()
        {

        }

        public PolySurface(Rhino.Geometry.Brep brep)
        {
            points = ToPoints(brep);
            faces = new List<int[]>();
            foreach (BrepFace face in brep.Faces)
            {
                List<Point> temp = ToPoints(face.ToBrep());
                List<int> faceIndex = new List<int>();
                for (int i = 0; i < temp.Count; i++)
                {
                    int index = points.IndexOf(temp[i]);
                    if (index != -1)
                        faceIndex.Add(index);
                }
                faces.Add(faceIndex.ToArray());
            }
            initial();
        }

        private List<Point> ToPoints(Brep brep)
        {
            return new List<Point>(brep.Vertices.Select(e => new Point(e.Location.X, e.Location.Y, e.Location.Z)));
        }
    }
}
