using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppServer;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Net.Http;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestUI();
            TestServer();
            
        }
        public static void TestUI()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = String.Format("http://{0}:8080/", endPoint.Address.ToString());
            }
            // Warning, this firewall policy is OS specific
            Process myProcess = new Process
            {
                StartInfo = {
                    FileName = "netsh.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("http add urlacl user=everyone url={0}"
                    ,
                    localIP),
                    Verb = "runas"
                    }
            };
            myProcess.Start();
            myProcess.WaitForExit();


            var server = new Server(new string[] { localIP });
            server.Run();
            while (true)
            {

            }
        }
        public static void TestServer()
        {
            string localIP;
            HttpClient client = new HttpClient();
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = String.Format("http://{0}:8080/", endPoint.Address.ToString());
            }

            
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        HttpRequestMessage message = new HttpRequestMessage()
                        {
                            Method = new HttpMethod("POST"),
                            RequestUri = new Uri(localIP + "AL5D/3"),
                            Content = new StringContent("data")
                        };
                        var result = client.SendAsync(message).Result;
                        Console.WriteLine(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
