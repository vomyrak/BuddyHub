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
        public readonly object _lock;
        public Dictionary<string, MethodInfo> MethodList { get; set; }

        public ControllerDevice() { }
        public ControllerDevice(DeviceInfo deviceInfo, string deviceId)
        {
            DeviceInfo = deviceInfo;
            DeviceId = deviceId;
            _lock = new object();
            MethodList = new Dictionary<string, MethodInfo>();
        }

        public void BindMethodInfo(string name, MethodInfo method)
        {
            MethodList.Add(name, method);
        }

        public void BindMethodInfo(List<string> name, List<MethodInfo> method)
        {
            if (name.Count != method.Count)
                throw new IndexOutOfRangeException("Name and Method size not matched");
            else
            {
                for (int i = 0; i < name.Count; i++)
                {
                    BindMethodInfo(name[i], method[i]);
                }
            }
        }
    }

}
