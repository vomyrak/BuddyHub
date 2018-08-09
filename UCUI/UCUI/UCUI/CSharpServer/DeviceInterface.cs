using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;

namespace CSharpServer
{

    public class ControllerDevice
    {
        public DeviceInfo DeviceInfo { get; set; }
        public dynamic Library { get; set; }
        public dynamic DeviceObject { get; set; }
        public string DeviceId { get; set; }

        public ControllerDevice() { }
        public ControllerDevice(DeviceInfo deviceInfo, string deviceId)
        {
            DeviceInfo = deviceInfo;
            DeviceId = deviceId;
        }
    }


}
