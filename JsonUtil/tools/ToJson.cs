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
                    Geo geo;
                    if (goo is GH_Box box)
                    {
                        geo = new geometry.breps.Box(box.Value);
                    }
                    else if (goo is GH_Mesh mesh)
                    {
                        geo = new geometry.breps.Mesh(mesh.Value);
                    }
                    else if (goo is GH_Circle circle)
                    {
                        geo = new geometry.vectors.Circle(circle.Value);
                    }
                    else if (goo is GH_Point point)
                    {
                        geo = new geometry.vectors.Vector(point.Value);
                    }
                    else if (goo is GH_Surface surface)
                    {
                        if (surface.Value.Loops.Count == 1)
                            geo = new geometry.breps.Surface(surface.Value);
                        else
                            geo = new geometry.hole.SurfaceWithHole(surface.Value);
                    }
                    else if (goo is GH_Brep brep)
                    {
                        geo = new geometry.breps.PolySurface(brep.Value);
                    }
                    else
                        geo = new geometry.breps.Box();
                    group.geos.Add(geo);
                }
                gHHelper.groups.Add(group);
            }
            String json = JsonConvert.SerializeObject(gHHelper);
            return json;
        }
    }
}