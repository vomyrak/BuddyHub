using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynxmotion;

namespace TestProject2
{
    class Program
    {
        static void Main(string[] args)
        {
            AL5C al5c;
            SSC32ENumerationResult[] SSC32s = AL5C.EnumerateConnectedSSC32(9600); 
        }
    }
}
