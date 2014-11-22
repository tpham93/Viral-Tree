using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Utilities
{
    public struct Vector3i
    {
        public int x;
        public int y;
        public int z;

        public Vector3i(int w)
        {
            x = w;
            y = w;
            z = w;
        }

        public Vector3i(int x, int y, int z){
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return "[x: " + x + ", y:" + y + ", z:" + z+"]";
        }
    }
}
