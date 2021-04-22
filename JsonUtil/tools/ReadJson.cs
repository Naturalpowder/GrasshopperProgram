using System;
using System.IO;
using Newtonsoft.Json;
using Rhino.Geometry;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;

namespace JsonUtil
{
    public class ReadJson
    {
        String filePath { get; set; }


        public ReadJson(String filePath)
        {
            this.filePath = filePath;
        }

        public static List<Z_GHGroup> Get(string json)
        {
            List<Z_GHGroup> list = new List<Z_GHGroup>();
            GHHelper results = JsonConvert.DeserializeObject<GHHelper>(json, new GeoJsonConverter<Geo>());
            foreach (Group group in results.groups)
            {
                Z_GHGroup gHGroup = new Z_GHGroup();
                foreach (Geo geo in group.geos)
                {
                    if (geo is Box myBox)
                    {

                        Rhino.Geometry.Box box = myBox.ToRhinoBox();
                        gHGroup.Data.Add(new GH_Box(box));
                    }
                    else if (geo is Vector myVector)
                    {
                        Vector3d vector = myVector.ToRhinoVector();
                        gHGroup.Data.Add(new GH_Vector(vector));
                    }
                    else if (geo is Circle myCircle)
                    {
                        Rhino.Geometry.Circle circle = myCircle.ToRhinoCircle();
                        gHGroup.Data.Add(new GH_Circle(circle));
                    }
                    else if (geo is Surface mySurface)
                    {
                        Brep surface = mySurface.ToRhinoSurface();
                        gHGroup.Data.Add(new GH_Surface(surface));
                    }
                    else if (geo is Prism prism)
                    {
                        Extrusion extrusion = prism.ToRhinoPrism();
                        gHGroup.Data.Add(new GH_Brep(extrusion.ToBrep()));
                    }
                    else if (geo is Mesh myMesh)
                    {
                        Rhino.Geometry.Mesh mesh = myMesh.ToRhinoMesh();
                        gHGroup.Data.Add(new GH_Mesh(mesh));
                    }
                }
                list.Add(gHGroup);
            }
            return list;
        }

        public List<Z_GHGroup> Get()
        {
            var json = File.ReadAllText(filePath);
            return Get(json);
        }

        public static void Main(String[] args)
        {
            ReadJson readJson = new ReadJson("../../../data/geos.json");
            readJson.Get();
            Console.ReadLine();
        }
    }
}