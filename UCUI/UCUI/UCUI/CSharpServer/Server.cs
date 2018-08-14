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
using System.Reflection;
using System.Threading;
using System.Management;

namespace CSharpServer
{
    public enum Notif
    {
        Test,
        CheckName,
        CheckIds,
        InvokeMethod,
        PostToServer,
        DeviceDetected,
        DeviceDisconnected,
        GetControllerDevice,
        GetDeviceInfo
    }

    public class Server
    {
        /// <summary>
        /// Enumerations that defines action code for the server
        /// </summary>
        
        private static WebServer ws;
        public static HttpClient client;
        public Dictionary<string, ControllerDevice> ConnectedDeviceList { get; set; }
        public static event EventHandler<string> RaiseUINotifEvent;
        public static event EventHandler<DeviceInfo> RaiseDeviceInfoEvent;
        public static event EventHandler<ControllerDevice> RaiseControllerDeviceEvent;

        private const string INTERNAL_ADDRESS = "http://localhost:8192/";
        /// <summary>
        /// Constructor Server class
        /// </summary>
        public Server(string hostAddress)
        {
            ConnectedDeviceList = new Dictionary<string, ControllerDevice>();
            ws = new WebServer(this.SendResponse, hostAddress);
            client = new HttpClient();
        }

        /// <summary>
        /// Start running the server
        /// </summary>
        public void Run()
        {
            ws.Run();

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
        /// Query the existence of device within database
        /// </summary>
        /// <param name="deviceName">The name of the device to look for</param>
        /// <param name="server">The server object to query</param>
        public void CheckValidDevice(string deviceName)
        {
            DeviceInfo resultDeviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(QueryDeviceInfo(deviceName));
            if (resultDeviceInfo != null)
            {
                ConnectedDeviceList[deviceName] = new ControllerDevice
                {
                    DeviceInfo = resultDeviceInfo
                };
                BindDevice(deviceName, resultDeviceInfo.Assembly);
            }
            else throw new InvalidDeviceException("Device Not Recognised by System", 0);
        }
        /// <summary>
        /// Query the existence of device within database
        /// </summary>
        /// <param name="vid">vid of usb devices</param>
        /// <param name="pid">pid of usb devices</param>
        public void CheckValidDevice(string vid, string pid)
        {
            DeviceInfo resultDeviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(QueryDeviceInfo(vid, pid));
            if (resultDeviceInfo != null)
            {
                string deviceName = resultDeviceInfo.Device;
                ConnectedDeviceList[deviceName] = new ControllerDevice
                {
                    DeviceInfo = resultDeviceInfo
                };
                BindDevice(deviceName, resultDeviceInfo.Assembly);
            }
            else throw new InvalidDeviceException("Device Not Recognised by System", 0);
        }

        /// <summary>
        /// Link device library and instantiate device object
        /// </summary>
        /// <param name="deviceName">The friendly name of device</param>
        public void BindDevice(string deviceName, string assemblyName)
        {
            var dll = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\" + assemblyName + ".dll");
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
                .Find(new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.Device, deviceName))
                .ToListAsync()
                .Result;
            return outputDevice[0].ToString();
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
                path = "..\\..\\auth.json";
            else
                path = "auth.json";

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

        public string QueryRemoteDeviceInfo(string deviceName)
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
                .Find(new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.ApiType, "Http")
                    & new FilterDefinitionBuilder<DeviceInfo>().Eq(x => x.Device, deviceName))
                .ToListAsync()
                .Result;
            return outputDevice[0].ToString();

        }

        public List<DeviceInfo> QueryRemoteDeviceInfo()
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

        /// <summary>
        /// Send response to the client according to request information
        /// </summary>
        /// <param name="request">Http request from client</param>
        /// <returns>A string corresponding to the status of the operation</returns>
        public string SendResponse(HttpListenerRequest request)
        {

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
                    throw new InvalidDeviceException("Device Not Found!");
                }
                else
                {
                    if (device.DeviceInfo.ApiType == "LocalLib")
                    {
                        string functionRequested = parsedRequest[1];
                        MethodInfo method = GetMethodInfo(device, functionRequested);
                        if (method == null) throw new InvalidMethodException("Method Not Found!");
                        else
                        {
                            ThreadStart threadStart = new ThreadStart(() =>
                            {
                                string[] parameters = new string[fieldSize - 2];
                                Array.Copy(parsedRequest, 2, parameters, 0, fieldSize - 2);

                                method.Invoke(device.DeviceObject, parameters);
                                RaiseUINotifEvent?.Invoke(this, "method invoked");
                            });
                            Thread newThread = new Thread(threadStart);
                            newThread.Start();
                            return "";
                        }
                    }
                    else if (device.DeviceInfo.ApiType == "Http")
                    {
                        
                        string functionRequested = parsedRequest[1];
                        DeviceInfo deviceInfo = device.DeviceInfo;
                        foreach (RemoteDeviceMethod method in deviceInfo.Methods)
                        {
                            if (method.Method == functionRequested)
                            {
                                //HttpRequestMessage message = new HttpRequestMessage();
                                //message.Content = new StringContent(method.Data);
                                //message.Headers.Clear();
                                //message.Headers.Add("Content-Type", "application/json");
                                //message.Method = new HttpMethod(method.HttpMethod);
                                //message.RequestUri = new Uri(method.Link);
                                //var result = SendToRemoteServerAsync(message);
                                HttpRequestMessage message;
                                if (device.DeviceInfo.Device == "Alexa")
                                {

                                    byte[] buffer = new byte[5000];
                                    request.InputStream.Read(buffer, 0, 5000);
                                    string content = System.Text.Encoding.UTF8.GetString(TrimNull(buffer));
                                    message = FormRequestMessage(
                                        method.HttpMethod,
                                        method.Link,
                                        content
                                    );
                                    var result = SendToRemoteServerAsync(message);
                                }
                                else
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
                            else throw new InvalidMethodException("Method not found!");

                        }
                        
                        return "";
                    }
                    else throw new InvalidDeviceException("Invalid device API type");
                    
                }
            }
            // Parse the request string and return requested result as a string

            // POST request corresponds to internal signaling message from UI thread
            else if (request.HttpMethod == "GET")
            {
                try
                {
                
                    int actionType = int.Parse(parsedRequest[0]);
                    switch (actionType)
                    {
                        case (int)Notif.Test:
                            return string.Format("<HTML><BODY>My web page.<br>{0}</BODY></HTML>", DateTime.Now);
                        case (int)Notif.CheckName:
                            return QueryDeviceInfo(parsedRequest[1]);
                        case (int)Notif.CheckIds:
                            return QueryDeviceInfo(parsedRequest[1], parsedRequest[2]);
                        case (int)Notif.InvokeMethod:
                            int buttonIndex = Int32.Parse(parsedRequest[1]);
                            ControllerDevice currentDevice = ConnectedDeviceList[parsedRequest[2]];
                            DeviceInfo currentDeviceInfo = currentDevice.DeviceInfo;

                            if (currentDeviceInfo.ApiType == "LocalLib")
                            {

                                int count = currentDeviceInfo.FunctionArray.Count;
                                if (buttonIndex > count - 1) { }
                                else
                                {
                                    Task.Run(() =>
                                    {
                                        MethodInfo methodToBind = GetMethodInfo(currentDevice, currentDeviceInfo.FunctionArray[buttonIndex].Name);
                                        methodToBind.Invoke(currentDevice.DeviceObject, null);
                                    });
                                }
                            }
                            else if (currentDeviceInfo.ApiType == "Http")
                            {
                                Dictionary<string, string> body = new Dictionary<string, string>
                                {
                                    ["name"] = "smart lamp",
                                    ["method"] = "off"
                                };
                                RemoteDeviceMethod requestedDeviceMethod = currentDevice.DeviceInfo.Methods.Where(s => s.Method == "on").ToList()[0];
                                //HttpRequestMessage message = new HttpRequestMessage()
                                //{
                                //
                                //    Method = new HttpMethod(requestedDeviceMethod.HttpMethod),
                                //    RequestUri = new Uri(requestedDeviceMethod.Link),
                                //    Content = new StringContent(requestedDeviceMethod.Data)
                                //};
                                //message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                                HttpRequestMessage message = FormRequestMessage(
                                    requestedDeviceMethod.HttpMethod,
                                    requestedDeviceMethod.Link,
                                    requestedDeviceMethod.Data,
                                    "application/json");
                                client.SendAsync(message);

                            }
                            return "";
                        case (int)Notif.GetDeviceInfo:
                            RaiseDeviceInfoEvent?.Invoke(this, ConnectedDeviceList[parsedRequest[1]].DeviceInfo);
                            return "";
                        case (int)Notif.GetControllerDevice:
                            RaiseControllerDeviceEvent?.Invoke(this, ConnectedDeviceList[parsedRequest[1]]);
                            return "";
                        case (int)Notif.PostToServer:
                            byte[] buffer = new byte[100];
                            request.InputStream.Read(buffer, 0, 100);
                            string content = System.Text.Encoding.UTF8.GetString(TrimNull(buffer));
                            //Dictionary<string, string> body = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                            //ControllerDevice requestedDevice = ConnectedDeviceList[body["name"]];
                            //DeviceInfo requestedDeviceInfo = requestedDevice.DeviceInfo;
                            //RemoteDeviceMethod requestedDeviceMethod = requestedDevice.DeviceInfo.Methods.Where(s => s.Method == "on").ToList()[0];
                            //HttpRequestMessage message = new HttpRequestMessage()
                            //{
                            //
                            //    Method = new HttpMethod(requestedDeviceMethod.HttpMethod),
                            //    RequestUri = new Uri(requestedDeviceMethod.Link),
                            //    Content = new StringContent(requestedDeviceMethod.Data)
                            //};
                            //message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            //client.SendAsync(message);
                            return "";
                        case (int)Notif.DeviceDetected:
                            string deviceName = ObtainUSBDeviceInfo();
                            if (deviceName == "") RaiseUINotifEvent?.Invoke(this, "Device Not Found");
                            return "";
                        case (int)Notif.DeviceDisconnected:
                            CheckRemovedDevice();
                            return "";
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

            else
            {
                
                return "";
            }

        }

        private byte[] TrimNull(byte[] input)
        {
            int i = input.Length - 1;
            while (input[i] == 0)
                --i;
            byte[] temp = new byte[i + 1];
            Array.Copy(input, temp, i + 1);
            return temp;
        }


        public async void SendAsync(HttpRequestMessage message)
        {
            var response = await SendToRemoteServerAsync(message);
        }
        /// <summary>
        /// Post data to remote server
        /// </summary>
        /// <param name="host">Remote host address</param>
        /// <param name="message">Message to be post</param>
        /// <returns></returns>
        private async Task<string> SendToRemoteServerAsync(HttpRequestMessage message)
        {
            try
            {
                var response = await client.SendAsync(message);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }

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
        /// Instantiate new thread that check for removed usb devices
        /// </summary>
        public void CheckRemovedDevice()
        {
            Task.Run(() =>
            {
                ManagementClass USBClass = new ManagementClass("Win32_USBHUB");
                ManagementObjectCollection USBCollection = USBClass.GetInstances();

                foreach (ManagementObject usb in USBCollection)
                {
                    string deviceId = usb["deviceid"].ToString();

                    foreach (string registeredDevice in ConnectedDeviceList.Keys.ToArray())
                    {
                        if (deviceId != ConnectedDeviceList[registeredDevice].DeviceId)
                        {
                            ConnectedDeviceList.Remove(registeredDevice);
                            
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Check for the validity of USB devices connected to the machine
        /// </summary>
        /// <returns></returns>
        public string ObtainUSBDeviceInfo()
        {
            string result = "";
            Thread thread = new Thread(() =>
            {
                ManagementClass USBClass = new ManagementClass("Win32_USBHUB");
                ManagementObjectCollection USBCollection = USBClass.GetInstances();

                foreach (ManagementObject usb in USBCollection)
                {
                    try
                    {
                        string deviceId = usb["deviceid"].ToString();
                        if (deviceId == null)
                        {
                            throw new Exception("Device not found!");
                        }
                        else
                        {


                            int vidIndex = deviceId.IndexOf("VID_");
                            string startingAtVid = deviceId.Substring(vidIndex + 4); // + 4 to remove "VID_"                    
                            string vid = startingAtVid.Substring(0, 4); // vid is four characters long

                            int pidIndex = deviceId.IndexOf("PID_");
                            string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"                    
                            string pid = startingAtPid.Substring(0, 4); // pid is four characters long

                            if (vidIndex < 0 || pidIndex < 0) { }
                            else
                            {
                                DeviceInfo newDeviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(QueryDeviceInfo(vid, pid));
                                if (newDeviceInfo != null)
                                {
                                    newDeviceInfo.ToFile("newDevice");
                                    result = newDeviceInfo.Device;
                                    AddDevice(result, new ControllerDevice(newDeviceInfo, deviceId));
                                    BindDevice(result, newDeviceInfo.Assembly);
                                }
                            }
                        }
                    }
                    catch (Exception e) { Console.WriteLine(e); }
                }
            });
            thread.Start();
            thread.Join();
            return result;
        }

        public void ObtainRemoteDeviceInfo()
        {
            List<DeviceInfo> remoteDeviceList = QueryRemoteDeviceInfo();
            foreach (DeviceInfo deviceInfo in remoteDeviceList)
            {
                AddDevice(deviceInfo.Device, new ControllerDevice(deviceInfo, deviceInfo.Device));
            }

        }

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
