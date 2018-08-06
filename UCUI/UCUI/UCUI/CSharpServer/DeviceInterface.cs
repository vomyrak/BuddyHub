﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;

namespace CSharpServer
{

    public enum Action
    {
        Test,
        CheckName,
        CheckIds,
        CallFunction
    }

    public class DeviceInterface
    {
        public Dictionary<string, ControllerDevice> ConnectedDeviceList { get; set; }
        private static readonly HttpClient client = new HttpClient();
        private string SelectedDevice { get; set; }
        private string ServerAddress { get; set; }



        public DeviceInterface() {
            ConnectedDeviceList = new Dictionary<string, ControllerDevice>();
            SelectedDevice = "";
            ServerAddress = "http://localhost:8080/";
        }

        public DeviceInterface(string serverAddress)
        {
            ConnectedDeviceList = new Dictionary<string, ControllerDevice>();
            SelectedDevice = "";
            ServerAddress = serverAddress;
        }

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
            DeviceInfo resultDeviceInfo = QueryDeviceInfo(deviceName);
            if (resultDeviceInfo != null)
            {
                ConnectedDeviceList[deviceName] = new ControllerDevice();
                ConnectedDeviceList[deviceName].DeviceInfo = resultDeviceInfo;
                BindDevice(deviceName);
            }
            else throw new InvalidDeviceException("Device Not Recognised by System", 0);
        }

        public DeviceInfo QueryDeviceInfo(string deviceName)
        {
            string QueryUrl = QueryUrlBuilder(ServerAddress, Action.CheckName, deviceName);
            string result = "";
            try
            {
                result = getResponse(QueryUrl).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return JsonConvert.DeserializeObject<DeviceInfo>(result);
        }

        public DeviceInfo QueryDeviceInfo(string vid, string pid)
        {
            string QueryUrl = QueryUrlBuilder(ServerAddress, Action.CheckIds, vid + "/" + pid);
            string result = "";
            try
            {
                result = getResponse(QueryUrl).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return JsonConvert.DeserializeObject<DeviceInfo>(result);
        }
        #region
        // For testing only
        public void TestFunction(string command)
        {
            string QueryUrl = QueryUrlBuilder(ServerAddress, Action.CallFunction, command);
            string result = "";
            try
            {
                result = getResponse(QueryUrl).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            string[] parsedResult = result.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            CallFunction(parsedResult[0], parsedResult[1], new object[] { parsedResult[2] });
        }

        public void CallFunction(string deviceName, string funcName, dynamic[] param)
        {
            dynamic testObject = ConnectedDeviceList[deviceName].DeviceObject;
            testObject.setGripper_PW(short.Parse(param[0]));
            testObject.updateServos();
        }
        #endregion

        public MethodInfo BindFunction(ControllerDevice controllerDevice, string funcName){
            Assembly dll = controllerDevice.Library;
            dynamic deviceInstance = controllerDevice.DeviceObject;
            //Type deviceType = dll.GetType(controllerDevice.DeviceInfo.Device);
            Type deviceType = dll.GetType(dll.GetName().Name + ".AL5C");
            return deviceType.GetMethod(funcName);
            
        }





        public string QueryUrlBuilder(string serverAddress, Action action, string deviceName)
        {
            return ServerAddress + (int)action + "/" + deviceName;
        }

        private static async Task<string> getResponse(string path)
        {
            string responseString = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                responseString = await client.GetStringAsync(path);
            }
            return responseString;
        }

        public void BindDevice(string deviceName)
        {
            var dll = Assembly.LoadFile(Directory.GetCurrentDirectory() + "\\" + "Lynxmotion" + ".dll");
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

        public void TestRoboticArm()
        {
            //CheckValidDevice("robotic_arm");
            //string input = Console.ReadLine();
            //while (input != "")
            //{
            //    int number = 0;
            //    int.TryParse(input, out number);
            //    if (number != 0)
            //    {
            //        TestFunction("robotic_arm/random/" + number);
            //    }
            //    input = Console.ReadLine();
            //}
            int param = 500;
            while (param <= 2500)
            {
                TestFunction("robotic_arm/random/" + param);
                param += 20;
                System.Threading.Thread.Sleep(200);
                
            }
        }

        public class ControllerDevice
        {
            public DeviceInfo DeviceInfo { get; set; }
            public dynamic Library { get; set; }
            public dynamic DeviceObject { get; set; }
            public string DeviceId { get; set; }

            public ControllerDevice() { }
            public ControllerDevice(DeviceInfo deviceInfo, string deviceId)
            {
                DeviceInfo = deviceInfo;
                DeviceId = deviceId;
            }
        }
    }


}