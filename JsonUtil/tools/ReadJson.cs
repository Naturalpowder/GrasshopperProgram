using System;
using System.IO;
using Newtonsoft.Json;
using Rhino.Geometry;
using System.Collections.Generic;
using Grasshopper.Kernel.Types;
using geometry;
using geometry.vectors;
using geometry.breps;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;

namespace JsonUtil
{
    public class ReadJson
    {
        String filePath { get; set; }

        public ReadJson(String filePath)
        {
            this.filePath = filePath;
        }

        public static GH_Structure<IGH_Goo> Get(string json)
        {
            GH_Structure<IGH_Goo> structure = new GH_Structure<IGH_Goo>();
            GHHelper results = JsonConvert.DeserializeObject<GHHelper>(json, new GeoJsonConverter<Geo>());
            for (int i = 0; i < results.groups.Count; i++)
            {
                GH_Path path = new GH_Path(i);
                foreach (Geo geo in results.groups[i].geos)
                {
                    if (geo is geometry.breps.Box myBox)
                    {
                        Rhino.Geometry.Box box = myBox.ToRhinoBox();
                        structure.Append(new GH_Box(box), path);
                    }
                    else if (geo is Vector myVector)
                    {
                        Vector3d vector = myVector.ToRhinoVector();
                        structure.Append(new GH_Vector(vector), path);
                    }
                    else if (geo is geometry.vectors.Circle myCircle)
                    {
                        Rhino.Geometry.Circle circle = myCircle.ToRhinoCircle();
                        structure.Append(new GH_Circle(circle), path);
                    }
                    else if (geo is geometry.breps.Surface mySurface)
                    {
                        Brep surface = mySurface.ToRhinoSurface();
                        structure.Append(new GH_Surface(surface), path);
                    }
                    else if (geo is Prism prism)
                    {
                        Brep extrusion = prism.ToRhinoPrism();
                        structure.Append(new GH_Brep(extrusion), path);
                    }
                    else if (geo is geometry.breps.PolySurface polySurface)
                    {
                        Brep solid = polySurface.ToRhinoPolySurface();
                        structure.Append(new GH_Brep(solid), path);
                    }
                    else if (geo is geometry.breps.Mesh myMesh)
                    {
                        Rhino.Geometry.Mesh mesh = myMesh.ToRhinoMesh();
                        structure.Append(new GH_Mesh(mesh), path);
                    }
                }
            }
            return structure;
        }

        public GH_Structure<IGH_Goo> Get()
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