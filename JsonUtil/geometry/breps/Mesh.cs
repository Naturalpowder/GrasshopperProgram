using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;
using geometry.face;

namespace geometry.breps
{
    public class Mesh : Geo
    {
        public List<Point> points { set; get; }
        public List<IndexFace> faces { set; get; }

        public Mesh()
        {

        }

        public Mesh(Rhino.Geometry.Mesh mesh)
        {
            points = new List<Point>(mesh.Vertices.ToList().Select(e => new Point(e.X, e.Y, e.Z)));
            List<MeshFace> meshFaces = mesh.Faces.ToList();
            faces = new List<IndexFace>();
            foreach (MeshFace face in meshFaces)
            {
                if (face.IsTriangle)
                {
                    faces.Add(new IndexFace(new List<int[]>(), new int[] { face.A, face.B, face.C }));
                }
                else if (face.IsQuad)
                {
                    faces.Add(new IndexFace(new List<int[]>(), new int[] { face.A, face.B, face.C, face.D }));
                }
            }
            initial();
        }

        public Rhino.Geometry.Mesh ToRhinoMesh()
        {
            Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();
            List<Point3d> pts = points.Select(e => e.ToRhinoPoint()).ToList();
            mesh.Vertices.AddVertices(pts);
            List<MeshFace> fs = new List<MeshFace>();
            foreach (IndexFace indexFace in faces)
            {
                int[] face = indexFace.outFace;
                if (face.Length == 3)
                {
                    fs.Add(new MeshFace(face[0], face[1], face[2]));
                }
                else if (face.Length == 4)
                {
                    fs.Add(new MeshFace(face[0], face[1], face[2], face[3]));
                }
            }
            mesh.Faces.AddFaces(fs);
            return mesh;
        }
    }
}