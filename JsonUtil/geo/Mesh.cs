using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;

namespace JsonUtil
{
    public class Mesh : Geo
    {
        public List<Point> points { set; get; }
        public List<int[]> faces { set; get; }

        public Rhino.Geometry.Mesh toRhinoMesh()
        {
            Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
            List<Point3d> pts = points.Select(e => e.ToRhinoPoint()).ToList();
            mesh.Vertices.AddVertices(pts);
            List<MeshFace> fs = faces.Select(e => new MeshFace(e[0], e[1], e[2])).ToList();
            mesh.Faces.AddFaces(fs);
            return mesh;
        }
    }
}
