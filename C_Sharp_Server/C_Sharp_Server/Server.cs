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

namespace C_Sharp_Server
{
    class Server
    {
        static void Main(string[] args)
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
            foreach (var device in outputDevice)
            {
                Console.WriteLine(device.device);
            }

            // Host server
            WebServer ws = new WebServer(SendResponse, "http://localhost:8080/test/");
            ws.Run();
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
            ws.Stop();

        }
        public static string SendResponse(HttpListenerRequest request)
        {
            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
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
