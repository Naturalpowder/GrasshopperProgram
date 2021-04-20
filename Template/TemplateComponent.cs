using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Grasshopper;
using Rhino.Geometry;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using JsonUtil;
using SocketUtil;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Template
{
    public class TemplateComponent : GH_Component
    {
        private int iter = 0;
        private int maxRefresh = 100;
        private Boolean stop = false;
        private List<Z_GHGroup> geometries;
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public TemplateComponent()
          : base("SolverTemplate", "Solver",
              "A Template for running C# scripts",
              "CSharpProject", "Template")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("num", "run", "numDescription", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Port", "port", "used for Socket", GH_ParamAccess.item);
            pManager.AddTextParameter("Host", "host", "used for Socket", GH_ParamAccess.item);
            pManager.AddTextParameter("Return data", "data", "get Final Calculation Result(number/text)", GH_ParamAccess.item);
            pManager.AddIntegerParameter("maxIteration", "maxIter", "the max ieration count of calculation", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("info", "out", "Debug.Log()", GH_ParamAccess.item);
            pManager.AddGeometryParameter("breps", "geometry", "Json Geometry", GH_ParamAccess.tree);
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
            DA.GetData(4, ref maxRefresh);
            if (toggle && iter < maxRefresh)
            {
                iter++;
                ExpireSolution(true);
                Draw(DA);
            }
            else if (!toggle)
            {
                iter = 0;
                stop = false;
                Setup(DA);
            }
        }

        private void Setup(IGH_DataAccess DA)
        {
            try
            {
                DA.SetData(0, String.Join("\n", OnPingDocument().Objects.Select(e => e.ToString())));
            }
            catch (Exception e)
            {
                DA.SetData(0, e.StackTrace + "\n" + e.Message);
            }
        }

        private void Draw(IGH_DataAccess DA)
        {
            Server server = null;
            try
            {
                server = Manage(DA, server);
            }
            catch (Exception e)
            {
                DA.SetData(0, e.StackTrace + "\n" + e.Message);
            }
            finally
            {
                if (server != null)
                    server.Close();
            }
        }

        private Server Manage(IGH_DataAccess DA, Server server)
        {
            GH_Document doc = OnPingDocument();
            ReceiveMessage(DA, ref server);
            Param_Brep brep = Calculate(doc, DA);
            String info = null;
            String typeReturn = "";
            DA.GetData(3, ref typeReturn);
            if (typeReturn.Equals("number"))
                info = SetNumberResult(DA, doc);
            else if (typeReturn.Equals("text"))
                info = SetTextResult(DA, doc);
            SendMessage(info, server);
            return server;
        }

        private Param_Brep Calculate(GH_Document doc, IGH_DataAccess DA)
        {
            Param_Brep brep = (Param_Brep)GetObject(doc, "GeoFromJson");
            brep.ClearData();
            brep.AddVolatileDataTree(ToGHStructure());
            brep.ExpireSolution(true);
            return brep;
        }

        private void ReceiveMessage(IGH_DataAccess DA, ref Server server)
        {
            if (!stop)
            {
                String host = "";
                int port = 0;
                DA.GetData(1, ref port);
                DA.GetData(2, ref host);
                server = new Server(host, port);
                string json = server.ReceiveMessage();
                if (json.Equals("stop"))
                {
                    stop = true;
                    geometries = null;
                }
                else
                {
                    geometries = ReadJson.Get(json);
                    GH_Structure<IGH_Goo> tree = ToGHStructure();
                    DA.SetDataTree(1, tree);
                }
            }
        }

        private GH_Structure<IGH_Goo> ToGHStructure()
        {
            GH_Structure<IGH_Goo> structure = new GH_Structure<IGH_Goo>();
            if (geometries != null)
            {
                for (int i = 0; i < geometries.Count; i++)
                {
                    GH_Path path = new GH_Path(i);
                    foreach (IGH_Goo item in geometries[i].Data)
                    {
                        structure.Append(item, path);
                    }
                }
            }
            return structure;
        }

        private String SetTextResult(IGH_DataAccess DA, GH_Document doc)
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

        private String SetNumberResult(IGH_DataAccess DA, GH_Document doc)
        {
            Param_Number result = (Param_Number)GetObject(doc, "Result");
            result.CollectData();
            IGH_StructureEnumerator enumerator = result.VolatileData.AllData(true);
            String info = "No Message!";
            if (new List<IGH_Goo>(enumerator).Count > 0)
            {
                info = new List<IGH_Goo>(enumerator)[0].ToString();
            }
            return info;
        }

        private void SendMessage(String info, Server server)
        {
            if (server != null)
            {
                server.SendMessage(info);
            }
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
                return Properties.Resources.Info;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("cb47a7c0-b1db-435e-8444-ff48b693d790"); }
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