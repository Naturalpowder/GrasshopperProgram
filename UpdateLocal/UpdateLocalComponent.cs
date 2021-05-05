using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using JsonUtil;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;
using System.Linq;
using System.Threading;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace UpdateLocal
{
    public class UpdateLocalComponent : GH_Component
    {
        private bool expiredGlobal = true;
        private int count = 0;
        private GH_Structure<IGH_Goo> data;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public UpdateLocalComponent()
          : base("UpdateLocal", "Update",
              "get local json geometry calculated results",
              "GrasshopperProgram", "Serialization")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("inputPath", "jsonPath", "the json file path on the disk", GH_ParamAccess.item);
            pManager.AddTextParameter("outputPath", "outputPath", "the path of saving calculated results", GH_ParamAccess.item);
            pManager.AddBooleanParameter("isExpired", "expired", "to judge whether the program will be expired", GH_ParamAccess.item);
            pManager.AddIntegerParameter("SleepTime", "pause", "set the time of thread sleeping", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("print", "out", "console info", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            try
            {
                bool expire = false;
                DA.GetData(2, ref expire);
                if (expiredGlobal && expire)
                {
                    ExpireSolution(true);
                    TextWriter writer = Console.Out;
                    using (StringWriter stringWriter = new StringWriter())
                    {
                        Console.SetOut(stringWriter);
                        Draw(DA);
                        DA.SetData(0, stringWriter.ToString());
                    }
                    Console.SetOut(writer);
                }
                else
                {
                    expiredGlobal = true;
                    Setup(DA);
                    DA.SetData(0, "Now the program is in the end!");
                }
                Sleep(DA);
            }
            catch (Exception e)
            {
                DA.SetData(0, e.StackTrace + "\n" + e.Message);
            }
        }

        private static void Sleep(IGH_DataAccess DA)
        {
            int time = 0;
            DA.GetData(3, ref time);
            if (time > 0)
                Thread.Sleep(time);
        }

        private void Setup(IGH_DataAccess DA)
        {
            String filePath = "";
            DA.GetData(0, ref filePath);
            data = new ReadJson(filePath).Get();
        }

        private void Draw(IGH_DataAccess DA)
        {
            String outputPath = "";
            DA.GetData(1, ref outputPath);
            GH_Document doc = OnPingDocument();
            if (count == data.Branches.Count)
                Calculate(doc, new List<IGH_Goo>(data.Branches[0]));
            else
                Calculate(doc, new List<IGH_Goo>(data.Branches[count]));
            //存储result不等于"No Message!"时的结果
            String result = GetTextResult(doc);
            Console.WriteLine(result);
            ManageCirculation(data);
        }

        private void ManageCirculation(GH_Structure<IGH_Goo> structure)
        {
            if (count == structure.Branches.Count)
            {
                expiredGlobal = false;
                count = 0;
            }
            else
                count++;
        }

        private void Calculate(GH_Document doc, List<IGH_Goo> list)
        {
            Param_Brep brep = (Param_Brep)GetObject(doc, "GeoFromJson");
            brep.ClearData();
            brep.AddVolatileDataList(new GH_Path(0), list);
            brep.ExpireSolution(true);
        }

        private String GetTextResult(GH_Document doc)
        {
            GH_Panel panel = (GH_Panel)GetObject(doc, "Result");
            panel.CollectData();
            IGH_StructureEnumerator enumerator = panel.VolatileData.AllData(true);
            String info = "No Message!";
            if (new List<IGH_Goo>(enumerator).Count > 0)
            {
                info = String.Join("\n", new List<IGH_Goo>(enumerator).Select(e => e.ToString()));
            }
            return info;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.Update;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ad3d492b-58d2-49a0-871b-1a9ef6da2434"); }
        }

        public IGH_DocumentObject GetObject(GH_Document doc, String key)
        {
            List<IGH_DocumentObject> select = new List<IGH_DocumentObject>(doc.Objects).FindAll(o => o.NickName.Equals(key));
            if (select.Count > 0)
                return select[0];
            else
                return null;
        }
    }
}
