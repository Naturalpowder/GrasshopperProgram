using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.IO;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Union
{
    public class UnionComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public UnionComponent()
          : base("Union", "UnionSolidUserVersion",
              "Union Solid",
              "GrasshopperProgram", "Bool")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("input", "input breps", "the breps will be unioned", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("output", "unioned breps", "the breps unioned", GH_ParamAccess.list);
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
                TextWriter writer = Console.Out;
                using (StringWriter stringWriter = new StringWriter())
                {
                    Console.SetOut(stringWriter);
                    Action(DA);
                    DA.SetData(1, stringWriter.ToString());
                }
                Console.SetOut(writer);
            }
            catch (Exception e)
            {
                DA.SetData(1, e.StackTrace + "\n" + e.Message);
            }
        }

        public void Action(IGH_DataAccess DA)
        {
            List<Brep> breps = new List<Brep>();
            DA.GetDataList(0, breps);
            Brep[] union = Brep.CreateBooleanUnion(breps, 0, false);
            DA.SetDataList(0, union);
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
                return Properties.Resources.Union;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9a154106-0425-4485-b36a-a55df6da5a92"); }
        }
    }
}
