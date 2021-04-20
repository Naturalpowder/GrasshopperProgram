using System;
using System.Collections.Generic;
using System.Text;

namespace JsonUtil
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
