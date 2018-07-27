using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace CSharpServer
{
    interface IDevice : IDisposable
    {
        object ConnectDevice();
        //string GetDevicdHID();
        

    }
}
