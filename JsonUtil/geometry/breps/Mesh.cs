using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;
using System.Linq;

namespace geometry.breps
{
    public class Mesh : Geo
    {
        public List<Point> points { set; get; }
        public List<int[]> faces { set; get; }

        public Mesh(Rhino.Geometry.Mesh mesh)
        {
            points = new List<Point>(mesh.Vertices.ToList().Select(e => new Point(e.X, e.Y, e.Z)));
            List<MeshFace> meshFaces = mesh.Faces.ToList();
            faces = new List<int[]>();
            foreach (MeshFace face in meshFaces)
            {
                if (face.IsTriangle)
                {
                    faces.Add(new int[] { face.A, face.B, face.C });
                }
                else if (face.IsQuad)
                {
                    faces.Add(new int[] { face.A, face.B, face.C, face.D });
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
            foreach (int[] face in faces)
            {
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