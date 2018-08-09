using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.IO;

namespace CSharpServer
{
    /// <summary>
    /// Specify the format of the content of the array of the device information
    /// </summary>
    public class DeviceInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("device")]
        public string Device { get; set; }
        [BsonElement("icon")]
        public string Icon { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("isTextboxVisible")]
        public bool IsTextBoxVisible { get; set; }
        [BsonElement("isButtonVisible")]
        public List<bool> IsButtonVisible { get; set; }
        [BsonElement("buttonLabel")]
        public List<string> ButtonLabel { get; set; }
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
