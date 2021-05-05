using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Parameters;
using System.Threading;
using System.Collections;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Linq;
using JsonUtil;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace HelloGrasshopper
{
    public class HelloGrasshopperComponent : GH_Component
    {
        private int iter = 1;
        private static int maxRefresh = 50;
        private List<Object> geos;
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public HelloGrasshopperComponent()
          : base("HelloGrasshopper", "Hello",
              "Test",
              "HelloGrasshopper", "Primitive")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("num", "run", "numDescription", GH_ParamAccess.item);
            pManager.AddTextParameter("filePath", "file", "Read Json", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("point", "pt", "pointInfo", GH_ParamAccess.list);
            pManager.AddTextParameter("info", "out", "Debug.Log()", GH_ParamAccess.item);
            pManager.AddGeometryParameter("breps", "geometry", "Json Geometry", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool toggle = false;
            DA.GetData(0, ref toggle);
            if (toggle && iter < maxRefresh)
            {
                iter++;
                ExpireSolution(true);
            }
            else if (!toggle)
            {
                iter = 0;
                Setup(DA);
            }
            Draw(DA);
        }

        private void Setup(IGH_DataAccess DA)
        {
            String filePath = "";
            DA.GetData(1, ref filePath);
            geos = ReadJson(filePath);
        }

        private void Draw(IGH_DataAccess DA)
        {
            GH_Document doc = OnPingDocument();
            IGH_DocumentObject e = GetObject(doc, "result");
            if (e != null)
            {
                UpdateInfo(DA, doc, e);
            }
        }

        private void AddListener(IGH_DataAccess DA, IGH_DocumentObject e)
        {
            IGH_DocumentObject.SolutionExpiredEventHandler handler = new IGH_DocumentObject.SolutionExpiredEventHandler((sender, eve) =>
            {
                DA.SetData(1, iter + " : " + sender + "\n" + eve);
            });
            e.SolutionExpired += handler;
        }

        private List<Object> ReadJson(String filePath)
        {
            ReadJson readJson = new ReadJson(filePath);
            return readJson.Get().ToList<object>();
        }

        private void UpdateInfo(IGH_DataAccess DA, GH_Document doc, IGH_DocumentObject e)
        {
            GH_NumberSlider slider = (GH_NumberSlider)GetObject(doc, "Count");
            slider.SetSliderValue(Math.Min(iter, 50));
            slider.ExpireSolution(true);
            Thread.Sleep(1);
            Param_Point p = (Param_Point)e;
            p.CollectData();
            IGH_StructureEnumerator pts = p.VolatileData.AllData(true);
            List<Point3d> list = pts.Select(o => ((GH_Point)o).Value).ToList();
            DA.SetDataList(0, pts);
            AddListener(DA, slider);
            DA.SetData(1, "Print Info");
            DA.SetDataList(2, geos);
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
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7c48bc6f-6fe6-4769-8346-414c911c077f"); }
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
