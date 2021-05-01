using System;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Newtonsoft.Json;
using geometry;

namespace JsonUtil
{
    public class ToJson
    {
        public static String ToJsonInfo(GH_Structure<IGH_GeometricGoo> tree)
        {
            GHHelper gHHelper = new GHHelper();
            foreach (List<IGH_GeometricGoo> goos in tree.Branches)
            {
                Group group = new Group();
                foreach (IGH_GeometricGoo goo in goos)
                {
                    if (goo is GH_Box box1)
                    {
                        geometry.breps.Box box = new geometry.breps.Box(box1.Value);
                        group.geos.Add(box);
                    }
                    else if (goo is GH_Mesh mesh1)
                    {
                        geometry.breps.Mesh mesh = new geometry.breps.Mesh(mesh1.Value);
                        group.geos.Add(mesh);
                    }
                    else if (goo is GH_Circle circle1)
                    {
                        geometry.vectors.Circle circle = new geometry.vectors.Circle(circle1.Value);
                        group.geos.Add(circle);
                    }
                    else if (goo is GH_Point point)
                    {
                        geometry.vectors.Vector vector = new geometry.vectors.Vector(point.Value);
                        group.geos.Add(vector);
                    }
                    else if (goo is GH_Surface surface1)
                    {
                        geometry.breps.Surface surface = new geometry.breps.Surface(surface1.Value);
                        group.geos.Add(surface);
                    }
                    else if (goo is GH_Brep brep)
                    {
                        geometry.breps.PolySurface polySurface = new geometry.breps.PolySurface(brep.Value);
                        group.geos.Add(polySurface);
                    }
                }
                gHHelper.groups.Add(group);
            }
            String json = JsonConvert.SerializeObject(gHHelper);
            return json;
        }
    }
}