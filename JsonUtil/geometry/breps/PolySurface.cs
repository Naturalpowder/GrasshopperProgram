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
                int[] index = Util.GetFaceIndex(points, face);
                faces.Add(index);
            }
            initial();
        }

        public Brep ToRhinoPolySurface()
        {
            List<Brep> surfaces = new List<Brep>(faces.Select(e => Util.GetSurfaces(points, e).ToRhinoSurface()));
            if (surfaces.Count == 1) return surfaces[0];
            return Brep.CreateSolid(surfaces, .01)[0];
        }
    }
}