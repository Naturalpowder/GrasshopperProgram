using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;


namespace geometry.breps
{
    public class Box : Geo
    {
        public Point x { get; set; }
        public Point y { get; set; }
        public Point z { get; set; }
        public Point origin { get; set; }

        public Box(Rhino.Geometry.Box box)
        {
            x = new Point(box.X.Max - box.X.Min, 0, 0);
            y = new Point(0, box.Y.Max - box.Y.Min, 0);
            z = new Point(0, 0, box.Z.Max - box.Z.Min);
            origin = new Point(box.X.Min, box.Y.Min, box.Z.Min);
            initial();
        }

        public Rhino.Geometry.Box ToRhinoBox()
        {
            Point3d p = Point3d.Add(origin.ToRhinoPoint(), x.ToRhinoPoint());
            p = Point3d.Add(p, y.ToRhinoPoint());
            p = Point3d.Add(p, z.ToRhinoPoint());
            BoundingBox boundingBox = new BoundingBox(origin.ToRhinoPoint(), p);
            return new Rhino.Geometry.Box(boundingBox);
        }
    }
}