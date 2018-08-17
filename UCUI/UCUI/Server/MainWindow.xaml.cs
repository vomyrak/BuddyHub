using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Net.Http;

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
        private const string SERVER_ADDRESS = "http://localhost:8080/";
        //private const string NETWORK_ADDRESS = "http://+:8080/";
        private const string INTERNAL_ADDRESS = "http://localhost:8192/";
        private Server server;
        private HwndSource windowHandle;

        public MainWindow()
        {
            InitializeComponent();
            var handle = new WindowInteropHelper(this).EnsureHandle();
            windowHandle = HwndSource.FromHwnd(handle);
            windowHandle.AddHook(new HwndSourceHook(WndProc));
            this.Hide();
            server = new Server(new string[] { SERVER_ADDRESS});
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
    }
}
