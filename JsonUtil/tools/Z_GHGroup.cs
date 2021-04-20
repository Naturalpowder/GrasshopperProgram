using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;

namespace JsonUtil
{
    public class Z_GHGroup
    {
        public List<IGH_Goo> Data { set; get; }

        public Z_GHGroup()
        {
            Data = new List<IGH_Goo>();
        }
    }
}