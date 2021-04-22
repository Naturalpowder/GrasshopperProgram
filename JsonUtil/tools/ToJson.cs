using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel.Types;
using Newtonsoft.Json;
using Rhino.Geometry;

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
                    Box box = new Box(((GH_Box)goo).Value);
                    group.geos.Add(box);
                }
                else if (goo is GH_Mesh)
                {
                    Mesh mesh = new Mesh(((GH_Mesh)goo).Value);
                    group.geos.Add(mesh);
                }
                else if (goo is GH_Circle)
                {
                    Circle circle = new Circle(((GH_Circle)goo).Value);
                    group.geos.Add(circle);
                }
                else if (goo is GH_Vector)
                {
                    Vector vector = new Vector(((GH_Vector)goo).Value);
                    group.geos.Add(vector);
                }
                else if (goo is GH_Surface)
                {
                    Surface surface = new Surface(((GH_Surface)goo).Value);
                    group.geos.Add(surface);
                }
                else if (goo is GH_Brep)
                {
                    Brep value = ((GH_Brep)goo).Value;
                    //Prism prism = new Prism(((GH_Surface)goo).Value);
                    //group.geos.Add(prism);
                }
            }
            String json = JsonConvert.SerializeObject(gHHelper);
            return json;
        }
    }
}
