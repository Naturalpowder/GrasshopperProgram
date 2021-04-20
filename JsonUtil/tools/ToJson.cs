using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;
using Newtonsoft.Json;

namespace JsonUtil
{
    public class ToJson
    {
        public static String ToJsonInfo(List<IGH_Goo> geos)
        {
            GHHelper gHHelper = new GHHelper();
            Group group = new Group();
            gHHelper.groups.Add(group);
            foreach (IGH_Goo goo in geos)
            {
                if (goo is GH_Box)
                {
                    Box box = new Box(new Rhino.Geometry.Box(((GH_Box)goo).Boundingbox));
                    group.geos.Add(box);
                }
            }
            String json = JsonConvert.SerializeObject(gHHelper);
            return json;
        }
    }
}
