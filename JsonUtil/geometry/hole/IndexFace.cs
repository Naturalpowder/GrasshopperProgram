using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geometry.hole
{
    class IndexFace
    {
        public List<int[]> innerFaces { set; get; }
        public int[] outFace { set; get; }

        public IndexFace(List<int[]> innerFaces, int[] outFace)
        {
            this.innerFaces = innerFaces;
            this.outFace = outFace;
        }
    }
}
