using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Deserialization
{
    public class DeserializationInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Deserialization";
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
                return new Guid("c2a3ac2f-ae13-459f-867c-b83ac767ca5b");
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
