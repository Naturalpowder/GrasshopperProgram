using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geometry
{
    public class Info
    {
        public int userID { set; get; }

        public List<String> colors { set; get; }

        public Info()
        {
            colors = new List<string>();
            userID = 0;
        }

    }
}
