using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;


namespace JsonUtil
{
   public class Box : Geo
    {
        public Point x { get; set; }
        public Point y { get; set; }
        public Point z { get; set; }
        public Point origin { get; set; }

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