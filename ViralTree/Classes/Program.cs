using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    class Program
    {
        static void Main(string[] args)
        {

            using (Game g = new Game())
                g.Run();
             
        }

    }
}
