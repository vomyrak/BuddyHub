using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace UCUtility
{
    public class NetworkManager
    {
        public static int NetshRegister(string localIP)
        {
            Process myProcess = new Process
            {
                StartInfo =
                {
                    FileName = "netsh.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("http add urlacl user=everyone url={0}", localIP),
                    Verb = "runas"
                }
            };
            myProcess.Start();
            myProcess.WaitForExit();
            return myProcess.ExitCode;
        }

        public static string GenerateIPAddress(int port)
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = String.Format("http://{0}:{1}/", endPoint.Address.ToString(), port);
            }
            return localIP;
        }
    }
}
