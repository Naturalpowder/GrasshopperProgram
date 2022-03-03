using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;
using geometry;
using geometry.face;

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

        public static PolylineCurve ToRhinoPolylineCurve(List<Point3d> points, int[] face)
        {
            List<Point3d> list = new List<Point3d>();
            for (int i = 0; i < face.Length + 1; i++)
            {
                list.Add(points[face[i % face.Length]]);
            }
            return new PolylineCurve(list);
        }

        public static Brep ToRhinoSurface(List<geometry.Point> points, IndexFace face)
        {
            List<Point3d> pts = points.Select(e => e.ToRhinoPoint()).ToList();
            List<int[]> faces = face.innerFaces;
            faces.Add(face.outFace);
            List<PolylineCurve> curves = new List<PolylineCurve>(faces.Select(e => Util.ToRhinoPolylineCurve(pts, e)));
            Brep outBrep = Brep.CreatePlanarBreps(curves[curves.Count - 1], .1)[0];
            if (curves.Count > 1)
            {
                outBrep.Faces[0].TryGetPlane(out Plane plane, .1);
                curves.Remove(curves[curves.Count - 1]);
                Brep[] inBreps = Brep.CreatePlanarBreps(curves, .1);
                foreach (Brep e in inBreps)
                {
                    outBrep = Brep.CreatePlanarDifference(outBrep, e, plane, .0001)[0];
                }
            }
            return outBrep;
        }

        public static List<geometry.Point> ToPoints(Brep brep)
        {
            return new List<geometry.Point>(brep.Vertices.Select(e => new geometry.Point(e.Location.X, e.Location.Y, e.Location.Z)));
        }

        public static List<geometry.Point> ToPoints(Curve curve)
        {
            Polyline pl = new Polyline();
            curve.TryGetPolyline(out pl);
            List<geometry.Point> points = new List<geometry.Point>(pl.Select(e => new geometry.Point(e.X, e.Y, e.Z)));
            return points;
        }

        public static IndexFace GetFaceIndex(List<BrepLoop> loops, List<geometry.Point> points)
        {
            List<int[]> innerFaces = new List<int[]>();
            foreach (BrepLoop loop in loops)
            {
                Curve curve = loop.To3dCurve();
                List<geometry.Point> temp = ToPoints(curve);
                int[] index = GetFaceIndex(points, temp);
                innerFaces.Add(index.Skip(0).Take(index.Length - 1).ToArray());
            }
            int[] outFace = innerFaces[0];
            innerFaces.Remove(outFace);
            return new IndexFace(innerFaces, outFace);
        }

        public static IndexFace GetFaceIndex(List<geometry.Point> points, BrepFace face)
        {
            List<BrepLoop> loops = face.Loops.ToList();
            return GetFaceIndex(loops, points);
        }

        public static int[] GetFaceIndex(List<geometry.Point> points, List<geometry.Point> temp)
        {
            List<int> faceIndex = new List<int>();
            for (int i = 0; i < temp.Count; i++)
            {
                int index = points.IndexOf(temp[i]);
                if (index != -1)
                    faceIndex.Add(index);
            }
            return faceIndex.ToArray();
        }
    }
}
