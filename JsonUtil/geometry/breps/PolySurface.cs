using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using JsonUtil;
using geometry.face;

namespace geometry.breps
{
    public class PolySurface : Geo
    {
        public List<Point> points;
        public List<IndexFace> faces;

        public PolySurface()
        {

        }

        public PolySurface(Brep brep)
        {
            points = Util.ToPoints(brep);
            faces = new List<IndexFace>();
            foreach (BrepFace face in brep.Faces)
            {
                IndexFace index = Util.GetFaceIndex(points, face);
                faces.Add(index);
            }
            initial();
        }

        public Brep ToRhinoPolySurface()
        {
            List<Brep> surfaces = new List<Brep>(faces.Select(e => Util.ToRhinoSurface(points, e)));
            if (surfaces.Count == 1) return surfaces[0];
            Brep[] breps = Brep.CreateSolid(surfaces, .001);
            if (breps.Length == 0) return new Brep();
            return breps[0];
        }
    }
}