using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using NetFwTypeLib;

namespace AppServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // USB Event Constants
        private const int WM_DEVICECHANGE = 0x219;
        private const int WM_DEVICEARRIVAL = 0x8000;
        private const int WM_DEVICEREMOVECOMPLETE = 0X8004;

        // Server Addresses
        //private const string SERVER_ADDRESS = "http://192.168.0.105:8080/";
        //private const string NETWORK_ADDRESS = "http://+:8080/";
        private const string INTERNAL_ADDRESS = "http://localhost:8192/";
        private Server server;
        private HwndSource windowHandle;
        public MainWindow()
        {
            InitializeComponent();
            FireWallManagement(Process.GetCurrentProcess().ProcessName, AppDomain.CurrentDomain.FriendlyName);
            #region Obtain local IP address
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
                    Arguments = String.Format("http add urlacl user = everyone url={0}"
                    ,
                    localIP),
                    Verb = "runas"
                    }
            };
            myProcess.Start();
            myProcess.WaitForExit();
            //Process.Start(new ProcessStartInfo()
            //{
            //    FileName = "netsh.exe",
            //    WindowStyle = ProcessWindowStyle.Hidden,
            //    Arguments = String.Format("http add urlacl user=everyone url=", localIP),
            //    Verb = "runas"
            //});
            #endregion


            var handle = new WindowInteropHelper(this).EnsureHandle();
            windowHandle = HwndSource.FromHwnd(handle);
            windowHandle.AddHook(new HwndSourceHook(WndProc));
            this.Hide();
            server = new Server(new string[] { localIP });
            server.Run();
            server.ObtainUSBDeviceInfo();
            server.ObtainRemoteDeviceInfo();

        }

        #region USB Device Events
        /// <summary>
        /// Callback function for message processing
        /// </summary>
        /// <param name="hwnd">Window Handle</param>
        /// <param name="msg">Message</param>
        /// <param name="wParam">Main Message</param>
        /// <param name="lParam">Secondary Message</param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_DEVICECHANGE:
                    switch ((uint)wParam.ToInt32())
                    {
                        case WM_DEVICEARRIVAL:
                            string deviceName = server.ObtainUSBDeviceInfo();
                            if (deviceName == "")
                            {
                                HttpRequestMessage message = server.FormRequestMessage(
                                    "POST",
                                    INTERNAL_ADDRESS + "Device Not Found",
                                    ""
                                    );
                                server.SendToRemoteServerAsync(message);
                            }
                            break;
                        case WM_DEVICEREMOVECOMPLETE:
                            HttpRequestMessage message2 = server.FormRequestMessage("POST", INTERNAL_ADDRESS + "Removed", "");
                            server.SendToRemoteServerAsync(message2);
                            server.CheckRemovedDevice();
                            break;
        
                    }
                    break;
            }
            return IntPtr.Zero;
        }



        #endregion

        private void FireWallManagement(string ruleName, string progName)
        {
            Type NetFwMgrType = Type.GetTypeFromProgID("HNetCfg.FwMgr", false);
            INetFwMgr mgr = (INetFwMgr)Activator.CreateInstance(NetFwMgrType);
            bool fireWallEnabled = mgr.LocalPolicy.CurrentProfile.FirewallEnabled;
            if (fireWallEnabled)
            {
                Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);
                foreach (INetFwRule2 rule in fwPolicy2.Rules)
                {
                    if (rule.Name == ruleName)
                    {
                        if (!rule.Enabled)
                        {
                            rule.Enabled = true;
                        }
                        return;
                    }
                }
                INetFwRule2 inboundRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                inboundRule.Enabled = true;
                inboundRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                inboundRule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
                inboundRule.LocalPorts = "8080";
                inboundRule.Name = ruleName;
                inboundRule.ApplicationName = progName;
                fwPolicy2.Rules.Add(inboundRule);
            }
        }
    }
}
