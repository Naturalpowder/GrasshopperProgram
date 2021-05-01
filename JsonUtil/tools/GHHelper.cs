using System;
using System.Collections.Generic;
using System.Text;

namespace geometry
{
    public class GHHelper
    {
        public IList<Group> groups { set; get; }

        public GHHelper()
        {
            groups = new List<Group>();
        }
    }
}
