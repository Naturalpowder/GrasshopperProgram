using Rhino.Geometry;

namespace geometry.vectors
{
    public class Circle : Geo
    {
        public Point origin { set; get; }
        public double radius { set; get; }

        public Circle(Rhino.Geometry.Circle circle)
        {
            this.radius = circle.Radius;
            this.origin = new Point(circle.Center.X, circle.Center.Y, circle.Center.Z);
            initial();
        }

        public Rhino.Geometry.Circle ToRhinoCircle()
        {
            return new Rhino.Geometry.Circle(origin.ToRhinoPoint(), radius);
        }
    }
}