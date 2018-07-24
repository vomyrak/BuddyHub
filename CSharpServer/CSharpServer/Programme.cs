using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Dynamic;

namespace CSharpServer
{
    class Programme
    {
        
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
        }

        

    }

    class Server
    {
        private static List<DeviceInfo> connectedDeviceList;
        private static WebServer ws;

        /// <summary>
        /// Constructor Server class
        /// </summary>
        public Server()
        {
            connectedDeviceList = new List<DeviceInfo>();
            ws = new WebServer(this.SendResponse, "http://localhost:8080/");
        }

        /// <summary>
        /// Start running the server
        /// </summary>
        public void Run()
        {
            ws.Run();
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
            ws.Stop();
        }

        
        /// <summary>
        /// Query the existence of device within database
        /// </summary>
        /// <param name="deviceName">The name of the device to look for</param>
        public void QueryDeviceInfo(string deviceName)
        {
            // Generate database connection string from auth.json
            string path;
            if (Debugger.IsAttached)
                path = "..\\..\\auth.json";
            else
                path = "auth.json";

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

        /// <summary>
        /// Send response to the client according to request information
        /// </summary>
        /// <param name="request">Http request from client</param>
        /// <returns>A string corresponding to the status of the operation</returns>
        public string SendResponse(HttpListenerRequest request)
        {
            // Parse the request string and return requested result as a string
            string[] parsedRequest = request.RawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parsedRequest.Length == 0) return "";
            else
            {
                try
                {
                    string deviceName = parsedRequest[0];
                    if (deviceName == "test") return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
                    else
                    {
                        QueryDeviceInfo(deviceName);
                        if (connectedDeviceList[connectedDeviceList.Count - 1].device == deviceName)
                        {
                            //Type thisType = typeof(Server);
                            //MethodInfo thisMethod = thisType.GetMethod(connectedDeviceList[connectedDeviceList.Count - 1].functionArray[0].name);
                            //thisMethod.Invoke(this, new string[2] { "0", "1300" });

                            var dll = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\Lynxmotion.dll");
                            Type AL5C = dll.GetType(dll.GetName().Name + ".AL5C");
                            if (AL5C != null)
                            {
                                Type SSC32 = dll.GetType(dll.GetName().Name + ".SSC32");
                                dynamic[] SSC32s = (dynamic[])SSC32.GetMethod("EnumerateConnectedSSC32").Invoke(null, new object[] { 9600 });
                                   
                                if (SSC32s.Length > 0)
                                {
                                    dynamic al5c = Activator.CreateInstance(AL5C, new object[] { SSC32s[0].PortName });
                                    al5c.setElbow_PW(short.Parse("1500"));
                                    al5c.updateServos();

                                }
                            }
               
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
        //public void TestRoboticArm(string servo, string param)
        //{
           
        //    AL5C al5c;
        //    SSC32ENumerationResult[] SSC32s = AL5C.EnumerateConnectedSSC32(9600);
        //    if (SSC32s.Length > 0)
        //    {
        //        al5c = new AL5C(SSC32s[0].PortName);
        //        short survoIndex = short.Parse(servo);
        //        short pwmVal = short.Parse(param);
        //        al5c.setElbow_PW(pwmVal);
        //        al5c.updateServos();
        //    }

        //}
    }


    /// <summary>
    /// Specify the format of the content of the array of the device information
    /// </summary>
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
