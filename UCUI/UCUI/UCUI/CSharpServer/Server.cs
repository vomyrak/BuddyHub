using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace CSharpServer
{
    public class Server
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

        }


        /// <summary>
        /// Query the existence of device within database
        /// </summary>
        /// <param name="deviceName">The name of the device to look for</param>
        public string QueryDeviceInfo(string deviceName)
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
            connectionString.Server = new MongoServerAddress(auth.Dns, 27017);
            connectionString.Username = auth.User;
            connectionString.Password = auth.Pass;
            // Establish connection
            var mongo = new MongoClient(connectionString.ToMongoUrl());
            var db = mongo.GetDatabase("uc");
            var coll = db.GetCollection<DeviceInfo>("outputData");
            var outputDevice = coll
                .Find(new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.Device, deviceName))
                .ToListAsync()
                .Result;
            return outputDevice[0].ToString();
        }

        public string QueryDeviceInfo(string vid, string pid)
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
            connectionString.Server = new MongoServerAddress(auth.Dns, 27017);
            connectionString.Username = auth.User;
            connectionString.Password = auth.Pass;
            // Establish connection
            var mongo = new MongoClient(connectionString.ToMongoUrl());
            var db = mongo.GetDatabase("uc");
            var coll = db.GetCollection<DeviceInfo>("outputData");
            var outputDevice = coll
                .Find(new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.Vid, vid) 
                    & new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.Pid, pid))
                .ToListAsync()
                .Result;
            return outputDevice[0].ToString();
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
                    int actionType = int.Parse(parsedRequest[0]);
                    switch (actionType)
                    {
                        case (int)Action.Test:
                            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
                        case (int)Action.CheckName:
                            return QueryDeviceInfo(parsedRequest[1]);
                        case (int)Action.CheckIds:
                            return QueryDeviceInfo(parsedRequest[1], parsedRequest[2]);
                        case (int)Action.CallFunction:
                            return parsedRequest[1] + "/" + parsedRequest[2] + "/" + parsedRequest[3];
                        default:
                            throw new InvalidActionException("Not a valid action to perform.");

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

}
