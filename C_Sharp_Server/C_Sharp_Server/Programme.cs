using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.Net.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Lynxmotion;
using System.Reflection;

namespace CSharpServer
{
    class Programme
    {
        
        static void Main(string[] args)
        {

            List<DeviceInfo> connectedDeviceList = new List<DeviceInfo>();
            WebServer ws = new WebServer(connectedDeviceList, Server.SendResponse, "http://localhost:8080/");
            ws.Run();
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
            ws.Stop();

        }

        static class Server
        {
            public static void QueryDeviceInfo(string deviceName, List<DeviceInfo> connectedDeviceList)
            {
                // Generate database connection string from auth.json
                string path = "D:\\Visual Studio\\Projects\\WSUROP\\C_Sharp_Server\\C_Sharp_Server\\auth.json";
                string json = "";
                using (StreamReader sr = File.OpenText(path))
                {
                    json = sr.ReadToEnd();
                }
                Auth auth = JsonConvert.DeserializeObject<Auth>(json);
                var connectionString = new MongoUrlBuilder();
                connectionString.Server = new MongoServerAddress(auth.dns, 27017);
                connectionString.Username = auth.user;
                connectionString.Password = auth.pass;
                // Establish connection
                var mongo = new MongoClient(connectionString.ToMongoUrl());
                var db = mongo.GetDatabase("uc");
                var coll = db.GetCollection<DeviceInfo>("outputData");
                var outputDevice = coll
                    .Find(new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.device, deviceName))
                    .ToListAsync()
                    .Result;
                if (outputDevice.Count != 0)
                {
                    connectedDeviceList.Add(outputDevice[0]);
                }
            }

            public static string SendResponse(HttpListenerRequest request, List<DeviceInfo> connectedDeviceList)
            {
                // Parse the request string and return requested result as a string
                string[] parsedRequest = request.RawUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);
                if (parsedRequest.Length == 0) return "";
                else
                {
                    try
                    {
                        string deviceName = parsedRequest[0];
                        if (deviceName == "test") return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
                        else
                        {
                            QueryDeviceInfo(deviceName, connectedDeviceList);
                            if (connectedDeviceList[connectedDeviceList.Count - 1].device == "robotic_arm")
                            {
                                Type thisType = typeof(Server);
                                MethodInfo thisMethod = thisType.GetMethod(connectedDeviceList[connectedDeviceList.Count - 1].functionArray[0].name);
                                thisMethod.Invoke(null, new string[2] { "0", "1" });

                                TestRoboticArm("0", connectedDeviceList[connectedDeviceList.Count - 1].functionArray[0].param[0].ToString());
                            }
                            return "success";
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return "";
                    }
                }
            }


            /// <summary>
            /// Test function for controlling the robotic arm
            /// </summary>
            /// <param name="servo">The index of the servo to be controlled</param>
            /// <param name="param">Parameter passed to the function</param>
            public static void TestRoboticArm(string servo, string param)
            {
                Lynxmotion.AL5C al5c;
                Lynxmotion.SSC32ENumerationResult[] SSC32s = AL5C.EnumerateConnectedSSC32(9600);
                if (SSC32s.Length > 0)
                {
                    al5c = new AL5C(SSC32s[0].PortName);
                    short survoIndex = short.Parse(servo);
                    short pwmVal = short.Parse(param);
                    al5c.setElbow_PW(pwmVal);
                }

            }
        }

    }

    
    

    class DeviceInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        [BsonElement("device")]
        public string device { get; set; }
        [BsonElement("function")]
        public List<Function> functionArray { get; set; }

    }

    /// <summary>
    /// Specify the format of the content of the array of the key "function"
    /// </summary>
    class Function
    {
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("param")]
        public List<double> param { get; set; }
    }

    /// <summary>
    /// Specify the format of authentication json file
    /// </summary>
    class Auth {
        public string user { get; set; }
        public string pass { get; set; }
        public string dns { get; set; }
    }




}
