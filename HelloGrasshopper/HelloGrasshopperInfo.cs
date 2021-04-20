using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace HelloGrasshopper
{
    public class HelloGrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "HelloGrasshopper";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("61918ea6-bf6d-4578-8ce1-b97c7430940c");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
