using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Management;

namespace USB_Detector
{
    public partial class Form1 : Form
    {
        private const int WM_DEVICECHANGE = 0x219;
        private const int WM_DEVICEARRIVAL = 0x8000;
        private const int WM_DEVICEREMOVECOMPLETE = 0X8004;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch ((int)m.WParam)
                    {
                        case WM_DEVICEARRIVAL:
                            //listBox1.Items.Clear();
                            //listBox1.Items.Add("New Device Connected");

                            //Going to need a separate thread for the call;
                            react();
                            break;
                        case WM_DEVICEREMOVECOMPLETE:
                            listBox1.Items.Clear();

                            listBox1.Items.Add("Device Removed");
                            break;
                    }
                    break;
            }
        }


        public Form1()
        {
            InitializeComponent();
            

        }

        public void react()
        {
            Thread thread = new Thread(() =>
            {
                ManagementClass USBClass = new ManagementClass("Win32_USBHUB");
                System.Management.ManagementObjectCollection USBCollection = USBClass.GetInstances();

                foreach (System.Management.ManagementObject usb in USBCollection)
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

                            if (vid == "0403")
                            {
                                
                                this.Invoke(new MethodInvoker(delegate () { listBox1.Items.Add("VID: " + vid); }));

                                int pidIndex = deviceId.IndexOf("PID_");
                                string startingAtPid = deviceId.Substring(pidIndex + 4); // + 4 to remove "PID_"                    
                                string pid = startingAtPid.Substring(0, 4); // pid is four characters long

                                if (pid == "6001")
                                {
                                    this.Invoke(new MethodInvoker(() => { listBox1.Items.Add("PID: " + pid); }));
                                }
                                    
                            }
                            else
                            {
                                this.Invoke(new MethodInvoker(() => { listBox1.Items.Add("Device not registered"); }));
                            }
                        }
                    }
                    catch (Exception e) { }
                }
            });
            thread.Start();
            
        }
    }
}
