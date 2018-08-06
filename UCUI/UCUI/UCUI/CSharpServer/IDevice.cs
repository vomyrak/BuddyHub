using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace CSharpServer
{

    public interface IDevice
    {
        object ConnectDevice(string serialPort);
        string GetSerialPort();
    }
}
