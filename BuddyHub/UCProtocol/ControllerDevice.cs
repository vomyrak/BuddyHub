//  BuddyHub Universal Controller
//
//  Created by Husheng Deng, 2018
//  https://github.com/vomyrak/BuddyHub

//  This library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.

//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.

//  You should have received a copy of the GNU General Public License
//  along with this library.  If not, see <http://www.gnu.org/licenses/>.
//
//  All trademarks, service marks, trade names, product names are the property of their respective owners.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;

namespace UCProtocol
{

    public class ControllerDevice
    {
        public DeviceInfo DeviceInfo { get; set; }
        public dynamic Library { get; set; }
        public dynamic DeviceObject { get; set; }
        public string DeviceId { get; set; }
        public Dictionary<string, MethodInfo> MethodList { get; set; }
        public readonly object _lock = new object();
        public ControllerDevice() { }

        /// <summary>
        /// instantiate controllerDevice with deviceInfo object and its deviceId
        /// </summary>
        /// <param name="deviceInfo">deviceInfo object</param>
        /// <param name="deviceId">deviceId</param>
        public ControllerDevice(DeviceInfo deviceInfo, string deviceId)
        {
            DeviceInfo = deviceInfo;
            DeviceId = deviceId;
            MethodList = new Dictionary<string, MethodInfo>();
            
        }

        /// <summary>
        /// bind device methodInfo to device itself
        /// </summary>
        /// <param name="name">device name</param>
        /// <param name="method">methodInfo object</param>
        public void BindMethodInfo(string name, MethodInfo method)
        {
            MethodList.Add(name, method);
        }

        /// <summary>
        /// bind all device MethodInfo to device itself
        /// </summary>
        /// <param name="name">device name</param>
        /// <param name="method">list of methodInfo</param>
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
