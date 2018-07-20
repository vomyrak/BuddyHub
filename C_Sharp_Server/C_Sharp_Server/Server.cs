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

namespace C_Sharp_Server
{
    class Server
    {
        static void Main(string[] args)
        {
            


            // Host server
            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/");
            ws.Run();
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
            ws.Stop();

        }
        public static string SendResponse(HttpListenerRequest request)
        {
            string[] parsedRequest = request.RawUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);
            if (parsedRequest.Length == 0) return "";
            else {
                try
                {
                    switch (parsedRequest[0])
                    {
                        case "test":
                          return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
                        case "robotic_arm":
                            string[] data = ConnectToDatabase();
                            TestRoboticArm(data[0], data[1]);
                            return "";
                        default:
                           return "";
                          
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                   return "";
                }
            }
        }
        private static void TestRoboticArm(string servo, string param)
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

        private static string[] ConnectToDatabase()
        {
            // Generate database connection string from auth.json
            string path = "C:\\Users\\Vomyrak\\Documents\\VS Projects\\WSUROP18\\C_Sharp_Server\\C_Sharp_Server\\auth.json";
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
            var coll = db.GetCollection<OutputData>("outputData");
            var outputDevice = coll
                .Find<OutputData>(FilterDefinition<OutputData>.Empty)
                .Limit(1)
                .ToListAsync<OutputData>()
                .Result;
            string[] result = new string[2];
            result[0] = outputDevice[0].functionArray[0].name;
            result[1] = Convert.ToString(outputDevice[0].functionArray[0].param[0]);
            return result;
        }
    }

    class OutputData
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string device { get; set; }
        [BsonElement("function")]
        public List<Function> functionArray { get; set; }
    }

    class Function
    {
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("param")]
        public List<double> param { get; set; }
    }

    class Auth {
        public string user { get; set; }
        public string pass { get; set; }
        public string dns { get; set; }
    }


}
