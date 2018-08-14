using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AppServer
{

    public interface IDevice
    {
        object ConnectDevice(string serialPort);
        string GetSerialPort();
    }
}
