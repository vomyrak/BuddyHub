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
        [BsonElement("vid")]
        public string Vid { get; set; }
        [BsonElement("pid")]
        public string Pid { get; set; }
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
        public List<Function> FunctionArray { get; set; }

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
    public class Function
    {
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("param")]
        public List<double> Param { get; set; }
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
