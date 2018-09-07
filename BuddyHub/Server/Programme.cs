using System.Net.Sockets;
using System.Net;
using System;
using UCUtility;
using System.IO;

namespace AppServer
{
    class Programme
    {
        static void Main(string[] args)
        {
            Server server;
            string localIP = NetworkManager.GenerateIPAddress(8080);
            string internalIP = NetworkManager.GenerateIPAddress(8192);
            NetworkManager.NetshRegister(localIP);
            server = new Server(new string[] { localIP }, new string[] { internalIP });
            server.Run();
            while (true)
            {

            }
        }
    }
}