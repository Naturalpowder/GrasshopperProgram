using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using JsonUtil;
using geometry.breps;

namespace geometry.hole
{
    class SurfaceWithHole : Geo
    {
        public List<Point> points;
        public IndexFace face;

        public SurfaceWithHole()
        {

        }

        public SurfaceWithHole(Rhino.Geometry.Brep surface)
        {
            points = Util.ToPoints(surface);
            List<BrepLoop> loops = new List<BrepLoop>(surface.Loops);
            List<int[]> innerFaces = new List<int[]>();
            foreach (BrepLoop loop in loops)
            {
                Curve curve = loop.To3dCurve();
                List<Point> temp = Util.ToPoints(curve);
                int[] index = Util.GetFaceIndex(points, temp);
                innerFaces.Add(index.Skip(0).Take(index.Length - 1).ToArray());
            }
            int[] outFace = innerFaces[0];
            innerFaces.Remove(outFace);
            face = new IndexFace(innerFaces, outFace);
            initial();
        }

        public Brep ToRhinoSurface()
        {
            List<Point3d> pts = points.Select(e => e.ToRhinoPoint()).ToList();
            List<int[]> faces = face.innerFaces;
            faces.Add(face.outFace);
            List<PolylineCurve> curves = new List<PolylineCurve>(faces.Select(e => Util.ToRhinoPolylineCurve(pts, e)));
            Brep outBrep = Brep.CreatePlanarBreps(curves[curves.Count - 1], .1)[0];
            if (curves.Count > 1)
            {
                outBrep.Faces[0].TryGetPlane(out Plane plane, .01);
                curves.Remove(curves[curves.Count - 1]);
                Brep[] inBreps = Brep.CreatePlanarBreps(curves, .1);
                foreach (Brep e in inBreps)
                {
                    outBrep = Brep.CreatePlanarDifference(outBrep, e, plane, .1)[0];
                }
            }
            return outBrep;
        }
    }
}