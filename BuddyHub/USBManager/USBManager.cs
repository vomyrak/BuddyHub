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
using System.Management;
using UCProtocol;
using Newtonsoft.Json;
using System.Net.Http;
using UCUtility;

namespace USBManager
{
    public class USBManager
    {
        ManagementEventWatcher Watcher { get; set; }
        string LocalIP { get; set; }

        /// <summary>
        /// Initialise internal server for communication with UI
        /// Initialise USB event watcher
        /// </summary>
        public USBManager()
        {
            LocalIP = NetworkManager.GenerateIPAddress(8192);
            NetworkManager.NetshRegister(LocalIP);
            ScanUSBDevices();
            Watcher = new ManagementEventWatcher();
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DEVICECHANGEEVENT WHERE EventType = 2 OR EventType = 3");
            Watcher.EventArrived += Watcher_EventArrived;
            Watcher.Query = query;
            Watcher.Start();
            Watcher.WaitForNextEvent();
        }

        
        public void ScanUSBDevices()
        {
            WqlObjectQuery query = new WqlObjectQuery("SELECT * FROM WIN32_USBHub");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection result = searcher.Get();
            HashSet<string> USBIdSet = new HashSet<string>();
            foreach (ManagementObject obj in result)
            {
                if (obj["DeviceID"] != null)
                {
                    Console.WriteLine("DeviceID:\t" + obj["DeviceID"].ToString());
                    USBIdSet.Add(obj["DeviceID"].ToString());
                }
            }
            if (USBIdSet.Count != 0)
            {
                var response = SendNotificationToServer(Notif.DeviceChanged, JsonConvert.SerializeObject(USBIdSet)).Result;
                Console.WriteLine(response.Content);
            }
        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ScanUSBDevices();
        }

        private async Task<HttpResponseMessage> SendNotificationToServer(Notif notif, string content)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage
            {
                Method = new HttpMethod("POST"),
                Content = new StringContent(content),
                RequestUri = new Uri(LocalIP + (int)notif)
            };
            var result = await client.SendAsync(message);
            return result;
        }
    }



}
