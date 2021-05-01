using System;
using System.Collections.Generic;
using System.Text;
using Rhino.Geometry;

namespace geometry
{
    public abstract class Geo
    {
        public String color { set; get; }
        public String type { set; get; }
        public Info info { set; get; }

        public void initial()
        {
            color = "#ffffff";
            type = GetType().ToString();
            info = new Info();
        }
    }
}