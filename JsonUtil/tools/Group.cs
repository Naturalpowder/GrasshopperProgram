using System;
using System.Collections.Generic;
using System.Text;

namespace geometry
{
    public class Group
    {
        public List<Geo> geos { set; get; }

        public Group()
        {
            geos = new List<Geo>();
        }
    }
}
