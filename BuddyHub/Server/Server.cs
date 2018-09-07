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
using System.Net;
using System.Diagnostics;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using UCProtocol;
using UCUtility;
 
namespace AppServer
{

    public class Server
    {

        
        private static WebServer ws;
        private static WebServer internalServer;
        public static HttpClient client;
        public Dictionary<string, ControllerDevice> ConnectedDeviceList { get; set; }



        /// <summary>
        /// constructor of server class
        /// </summary>
        public Server(string[] hostAddress, string[] internalAddress)
        {
            ConnectedDeviceList = new Dictionary<string, ControllerDevice>();
            ws = new WebServer(this.SendResponse, hostAddress);
            internalServer = new WebServer(InternalResponse, internalAddress);
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ObtainRemoteDeviceInfo();
        }


        private void UpdateUSBDeviceList(HashSet<string> deviceIdSet)
        {
            foreach (string deviceId in deviceIdSet)
            {
                ParseDeviceId(deviceId, out string vid, out int vidIndex, out string pid, out int pidIndex);
                if (vidIndex < 0 || pidIndex < 0) { } // Invalid device
                else RegisterUSBDeviceById(deviceId, vid, pid);
            }
        }

        /// <summary>
        /// Parse deviceId for identification
        /// https://stackoverflow.com/questions/8303069/how-to-get-the-vid-and-pid-of-all-usb-devices-connected-to-my-system-in-net
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="vid"></param>
        /// <param name="pid"></param>
        private void ParseDeviceId(string deviceId, out string vid, out int vidIndex, out string pid, out int pidIndex)
        {
            vidIndex = deviceId.IndexOf("VID_");
            string startingAtVid = deviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"                    
            vid = startingAtVid.Substring(0, 4); // vid is four characters long

            pidIndex = deviceId.IndexOf("PID_");
            string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"                    
            pid = startingAtPid.Substring(0, 4); // pid is four characters long
        }

        private void RegisterUSBDeviceById(string deviceId, string vid, string pid)
        {
            foreach (string device in ConnectedDeviceList.Keys.ToList())
            {
                if (ConnectedDeviceList[device].DeviceId == deviceId) return;
            }
            DeviceInfo newDeviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(QueryDeviceInfo(vid, pid));
            if (newDeviceInfo != null)
            {
                newDeviceInfo.ToFile("newDevice.txt");
                AddDevice(newDeviceInfo.Device, new ControllerDevice(newDeviceInfo, deviceId));
                BindDevice(newDeviceInfo.Device, newDeviceInfo.Assembly);
            }
        }

        /// <summary>
        /// Start running the server
        /// </summary>
        public void Run()
        {
            ws.Run();
            internalServer.Run();
        }

        /// <summary>
        /// Add ControllerDevice object to collection
        /// </summary>
        /// <param name="name">Device name</param>
        /// <param name="device">ControllerDevice object</param>
        public void AddDevice(string name, ControllerDevice device)
        {
            ConnectedDeviceList[name] = device;
        }

        public void DeleteDevice(string name)
        {
            ConnectedDeviceList.Remove(name);
        }


        /// <summary>
        /// Link device library and instantiate device object
        /// </summary>
        /// <param name="deviceName">The friendly name of device</param>
        public void BindDevice(string deviceName, string assemblyName)
        {
            Assembly dll = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\" + assemblyName + ".dll");
            ConnectedDeviceList[deviceName].Library = dll;
            IDevice newDevice = null;
            foreach (Type type in dll.GetTypes())
            {
                if (type.GetInterface("IDevice") != null)
                {
                    newDevice = Activator.CreateInstance(type) as IDevice;
                }
            }
            string portName = newDevice.GetSerialPort();
            ConnectedDeviceList[deviceName].DeviceObject = newDevice.ConnectDevice(portName);
        }


        /// <summary>
        /// Query the existence of device within database via device ids
        /// </summary>
        /// <param name="vid">Vid of USB devices</param>
        /// <param name="pid">Pid of USB devices</param>
        /// <returns></returns>
        public string QueryDeviceInfo(string vid, string pid)
        {
            // Generate database connection string from auth.json
            string path;
            if (Debugger.IsAttached)
            {
                path = "..\\..\\..\\auth.json";
            }
            else path = "auth.json";
            string json = "";
            using (StreamReader sr = File.OpenText(path))
            {
                json = sr.ReadToEnd();
            }
            Auth auth = JsonConvert.DeserializeObject<Auth>(json);
            var connectionString = new MongoUrlBuilder
            {
                Server = new MongoServerAddress(auth.Dns, 27017),
                Username = auth.User,
                Password = auth.Pass
            };
            // Establish connection
            var mongo = new MongoClient(connectionString.ToMongoUrl());
            var db = mongo.GetDatabase("uc");
            var coll = db.GetCollection<DeviceInfo>("outputData");
            var outputDevice = coll
                .Find(new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.Vid, vid)
                    & new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.Pid, pid)
                    & new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.ApiType, "LocalLib"))
                .ToListAsync()
                .Result;
            return outputDevice[0].ToString();
        }

        public List<DeviceInfo> QueryRemoteDeviceInfo()
        {
            // Generate database connection string from auth.json
            string path;
            if (Debugger.IsAttached)
            {
                path = "..\\..\\..\\auth.json";
            }
            else path = "auth.json";
            string json = "";
            using (StreamReader sr = File.OpenText(path))
            {
                json = sr.ReadToEnd();
            }
            Auth auth = JsonConvert.DeserializeObject<Auth>(json);
            var connectionString = new MongoUrlBuilder
            {
                Server = new MongoServerAddress(auth.Dns, 27017),
                Username = auth.User,
                Password = auth.Pass
            };
            // Establish connection
            var mongo = new MongoClient(connectionString.ToMongoUrl());
            var db = mongo.GetDatabase("uc");
            var coll = db.GetCollection<DeviceInfo>("outputDevices");
            var outputDevice = coll
                .Find(new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.ApiType, "Http"))
                .ToListAsync()
                .Result;
            return outputDevice;

        }

        /// <summary>
        /// Get metainfo associated with a function via known function name
        /// </summary>
        /// <param name="controllerDevice">Device object corresponding to the function</param>
        /// <param name="funcName">Known function name</param>
        /// <returns></returns>
        public MethodInfo GetMethodInfo(ControllerDevice controllerDevice, string funcName)
        {
            Assembly dll = controllerDevice.Library;
            dynamic deviceInstance = controllerDevice.DeviceObject;
            //Type deviceType = dll.GetType(controllerDevice.DeviceInfo.Device);
            Type deviceType = dll.GetType(dll.GetName().Name + "." + controllerDevice.DeviceInfo.Device);
            
            return deviceType.GetMethod(funcName);

        }
        
        public string InternalResponse(HttpListenerRequest request)
        {
            Console.WriteLine(request.Url);
            string rawUrl = request.RawUrl.Replace("%20", " ");
            string[] parsedRequest = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            int fieldSize = parsedRequest.Length;
            if (parsedRequest.Length == 0) return "Invalid Notification Received!";
            else if (request.HttpMethod == "POST")
            {
                int notification = Int16.Parse(parsedRequest[0]);
                switch (notification)
                {
                    case (int)Notif.DeviceChanged:
                        byte[] buffer = new byte[5000];
                        request.InputStream.Read(buffer, 0, 5000);
                        string content = System.Text.Encoding.UTF8.GetString(TrimNull(buffer));
                        HashSet<string> deviceIdSet = JsonConvert.DeserializeObject<HashSet<string>>(content);
                        UpdateUSBDeviceList(deviceIdSet);
                        return "OK";
                    default:
                        return "Invalid Notification Received!";
                }
            }
            else
            {
                return "Invalid Notification Received!";
            }
        }

        /// <summary>
        /// Send response to the client according to request information
        /// </summary>
        /// <param name="request">Http request from client</param>
        /// <returns>A string corresponding to the status of the operation</returns>
        public string SendResponse(HttpListenerRequest request)
        {
            Console.WriteLine(request.Url);
            string rawUrl = request.RawUrl.Replace("%20", " ");
            string[] parsedRequest = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            
            int fieldSize = parsedRequest.Length;
            if (parsedRequest.Length == 0) return "";
            else if (request.HttpMethod == "POST")
            {
                string deviceRequested = parsedRequest[0];
                ControllerDevice device = IsDeviceConnected(deviceRequested);
                if (device == null)
                {
                    return "Device Not Found";
                    throw new InvalidDeviceException("Device Not Found!");
                }
                else
                {
                    if (device.DeviceInfo.ApiType == "LocalLib")
                    {
                        int indexRequested = Int32.Parse(parsedRequest[1]);
                        IEnumerable<USBDeviceMethod> resultList =
                            from result in device.DeviceInfo.FunctionArray
                            where result.ButtonIndex == indexRequested
                            select result;

                        string functionRequested = resultList.First().Name;
                        MethodInfo method = GetMethodInfo(device, functionRequested);
                        if (method == null)
                        {
                            return "Method Not Found";
                            throw new InvalidMethodException("Method Not Found!");
                        }
                        else
                        {
                            Task.Run(() =>
                            {
                                bool lockTaken = false;
                                try
                                {
                                    Monitor.TryEnter(device._lock, ref lockTaken);
                                    if (lockTaken)
                                    {

                                        string[] parameters = new string[fieldSize - 2];
                                        Array.Copy(parsedRequest, 2, parameters, 0, fieldSize - 2);
                                        method.Invoke(device.DeviceObject, parameters);
                                    }
                                }
                                finally
                                {
                                    if (lockTaken)
                                    {
                                        Monitor.Exit(device._lock);
                                    }
                                }
                            });
                            return "OK";
                        }
                    }
                    else if (device.DeviceInfo.ApiType == "Http")
                    {
                        int indexRequested = Int32.Parse(parsedRequest[1]);
                            IEnumerable<RemoteDeviceMethod> resultList =
                            from result in device.DeviceInfo.Methods
                            where result.ButtonIndex == indexRequested
                            select result;

                        string functionRequested = resultList.First().Method;
                        DeviceInfo deviceInfo = device.DeviceInfo;
                        foreach (RemoteDeviceMethod method in deviceInfo.Methods)
                        {
                            if (method.Method == functionRequested)
                            {
                                HttpRequestMessage message;
                                if (device.DeviceInfo.Device == "Alexa")
                                {
                                    byte[] buffer = new byte[5000];
                                    request.InputStream.Read(buffer, 0, 5000);
                                    string content = System.Text.Encoding.UTF8.GetString(TrimNull(buffer));
                                    content = JsonConvert.DeserializeObject<Dictionary<string, string>>(method.Data)["input"] + content;
                                    content = JsonConvert.SerializeObject(new Dictionary<string, string> { ["input"] = content });
                                    message = FormRequestMessage(
                                        method.HttpMethod,
                                        method.Link,
                                        content
                                    );
                                    var response = SendToRemoteServerAsync(message).Result;
                                    if (!response.IsSuccessStatusCode)
                                    {
                                        string errorPhrase = response.ReasonPhrase;
                                        //return errorPhrase;
                                    }

                                    string result = response.Content.ReadAsStringAsync().Result;

                                    result = method.Link.Substring(0, method.Link.Length - 4) + result;
                                    Task.Run(() =>
                                    {
                                        bool lockTaken = false;
                                        try
                                        {

                                            Monitor.TryEnter(device._lock, ref lockTaken);
                                            if (lockTaken)
                                            {
                                                AudioPlayer.Play(result);
                                            }
                                        }
                                        finally
                                        {
                                            if (lockTaken)
                                            {
                                                Monitor.Exit(device._lock);
                                            }
                                        }
                                    });
                                }
                                else
                                {
                                    Task.Run(() =>
                                    {
                                        bool lockTaken = false;
                                        try
                                        {
                                            Monitor.TryEnter(device._lock, ref lockTaken);
                                            if (lockTaken)
                                            {
                                                message = FormRequestMessage(
                                                    method.HttpMethod,
                                                    method.Link,
                                                    method.Data,
                                                    "application/json"
                                                );
                                                var result = SendToRemoteServerAsync(message);
                                            }
                                        }
                                        finally
                                        {
                                            if (lockTaken)
                                            {
                                                Monitor.Exit(device._lock);
                                            }
                                        }
                                    });
                                }
                            }
                            //else throw new InvalidMethodException("Method not found!");

                        }

                        return "OK";
                    }
                    else
                    {
                        return "Invalid Device API Type";
                        throw new InvalidDeviceException("Invalid device API type");
                    }
                    
                }
            }
            

            else
            {
                
                return "Error";
            }

        }

        /// <summary>
        /// convert buffer array to an array of correct size by trimming zero values
        /// </summary>
        /// <param name="input">buffer array</param>
        /// <returns></returns>
        private byte[] TrimNull(byte[] input)
        {
            int i = input.Length - 1;
            while (input[i] == 0)
                --i;
            byte[] temp = new byte[i + 1];
            Array.Copy(input, temp, i + 1);
            return temp;
        }


        /// <summary>
        /// Post data to remote server
        /// </summary>
        /// <param name="host">Remote host address</param>
        /// <param name="message">Message to be post</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendToRemoteServerAsync(HttpRequestMessage message)
        {
            try
            {
                var response = await client.SendAsync(message);
                var responseString = await response.Content.ReadAsStringAsync();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// check whether a device is being connected
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        private ControllerDevice IsDeviceConnected(string deviceName)
        {
            foreach (ControllerDevice device in ConnectedDeviceList.Values)
            {
                if (device.DeviceInfo.Device == deviceName)
                {
                    return device;
                }
            }
            return null;
        }

        /// <summary>
        /// retrieve information of and add all smart devices from database
        /// </summary>
        public void ObtainRemoteDeviceInfo()
        {
            List<DeviceInfo> remoteDeviceList = QueryRemoteDeviceInfo();
            foreach (DeviceInfo deviceInfo in remoteDeviceList)
            {
                AddDevice(deviceInfo.Device, new ControllerDevice(deviceInfo, deviceInfo.Device));
            }

        }

        /// <summary>
        /// create http request message
        /// </summary>
        /// <param name="method">http method</param>
        /// <param name="url">url address</param>
        /// <param name="data">message content if applicable</param>
        /// <param name="mediaType">transmitted media type</param>
        /// <returns>a http request message</returns>
        public HttpRequestMessage FormRequestMessage(string method, string url, string data, string mediaType = "application/json")
        {
            HttpRequestMessage message = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(url),
                Content = new StringContent(data)
            };
            message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mediaType);
            return message; 
        }
    }
    
}
