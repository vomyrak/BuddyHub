using System;
using System.IO;


namespace CSharpServer
{
    class Programme
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            DeviceInterface deviceInterface = new DeviceInterface();
            server.Run();
            deviceInterface.TestRoboticArm();
        }
    }
}
