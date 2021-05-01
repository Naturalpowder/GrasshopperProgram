using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using JsonUtil;
using Grasshopper.Kernel.Types;
using System.Linq;
using System.IO;
using Grasshopper.Kernel.Data;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Serialization
{
    public class SerializationComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public SerializationComponent()
          : base("Serialization", "toJson",
              "create json from grasshopper geometries",
              "GrasshopperProgram", "Serialization")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("geometry", "input", "Add geometries to json helper", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("json", "info", "Serialization info", GH_ParamAccess.item);
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
                using (StringWriter stringWriter = new StringWriter())
                {
                    Console.SetOut(stringWriter);
                    Action(DA);
                    DA.SetData(1, stringWriter.ToString());
                }
            }
            catch (Exception e)
            {
                DA.SetData(1, e.StackTrace + "\n" + e.Message);
            }
        }

        private static void Action(IGH_DataAccess DA)
        {
            DA.GetDataTree(0, out GH_Structure<IGH_GeometricGoo> tree);
            String json = ToJson.ToJsonInfo(tree);
            DA.SetData(0, json);
            Console.WriteLine(tree[0].GetType());
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
                return Properties.Resources.Serialization;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1456602e-a2b0-4503-95f4-54a2ce576401"); }
        }
    }
}
