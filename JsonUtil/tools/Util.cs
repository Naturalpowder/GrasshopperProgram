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

        public static PolylineCurve ToRhinoPolylineCurve(List<Point3d> points, int[] face)
        {
            List<Point3d> list = new List<Point3d>();
            for (int i = 0; i < face.Length + 1; i++)
            {
                list.Add(points[face[i % face.Length]]);
            }
            return new PolylineCurve(list);
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

        public static geometry.breps.Surface GetSurfaces(List<geometry.Point> points, int[] face)
        {
            return new geometry.breps.Surface(new List<geometry.Point>(face.Select(e => points[e])));
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

        public static int[] GetFaceIndex(List<geometry.Point> points, BrepFace face)
        {
            Curve curve = face.OuterLoop.To3dCurve();
            int[] index = Util.GetFaceIndex(points, ToPoints(curve));
            return index;
        }
    }
}
