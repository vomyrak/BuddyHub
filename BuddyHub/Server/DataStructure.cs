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
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.IO;

namespace AppServer
{
    /// <summary>
    /// Specify the format of the content of the array of the device information
    /// </summary>
    public class DeviceInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("assembly")]
        public string Assembly { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
        [BsonElement("device_name")]
        public string DisplayName { get; set; }
        [BsonElement("function")]
        public List<USBDeviceMethod> FunctionArray { get; set; }
        [BsonElement("apiType")]
        public string ApiType { get; set; }
        [BsonElement("vid")]
        public string Vid { get; set; }
        [BsonElement("pid")]
        public string Pid { get; set; }
        [BsonElement("methods")]
        public List<RemoteDeviceMethod> Methods { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void ToFile(string fileName)
        {
            File.WriteAllText(fileName, this.ToString());
        }

        public static DeviceInfo LoadLocalDeviceInfo(string fileName)
        {
            DeviceInfo deviceInfo = null;
            if (File.Exists(fileName))
            {
                deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(File.ReadAllText(fileName));
            }
            return deviceInfo;
        }
    }



    /// <summary>
    /// Specify the format of the content of the array of the key "function"
    /// </summary>
    public class USBDeviceMethod
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("param")]
        public List<double> Param { get; set; }
        [BsonElement("buttonIndex")]
        public int ButtonIndex { get; set; }
    }

    public class RemoteDeviceMethod
    {
        [BsonElement("method")]
        public string Method { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("http_method")]
        public string HttpMethod { get; set; }
        [BsonElement("link")]
        public string Link { get; set; }
        [BsonElement("data")]
        public string Data { get; set; }
        [BsonElement("headers")]
        public string Headers { get; set; }
        [BsonElement("callback_function")]
        public string CallbackFunction { get; set; }
        [BsonElement("text_input_field")]
        public string TextInputField { get; set; }
        [BsonElement("params")]
        public List<string> Params { get; set; }
        [BsonElement("buttonIndex")]
        public int ButtonIndex { get; set; }
    }

    /// <summary>
    /// Specify the format of authentication json file
    /// </summary>
    class Auth
    {
        public string User { get; set; }
        public string Pass { get; set; }
        public string Dns { get; set; }
    }
}
