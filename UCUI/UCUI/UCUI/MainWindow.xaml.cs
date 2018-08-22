
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UCUI.Models;
using UCUI.UserControls;
using System.Media;
<<<<<<< HEAD
=======
using CSharpServer;
using System.Threading;
using System.Windows.Interop;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
>>>>>>> rachel
using System.Windows.Controls.Primitives;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace UCUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[] ButtonArray;
<<<<<<< HEAD

        public MainWindow()
        {
=======
        private Server server;
        private HttpClient client;
        private HwndSource windowHandle;

        private const string SERVER_ADDRESS = "http://localhost:8080/";

        // USB Message Constants
        private const int WM_DEVICECHANGE = 0x219;
        private const int WM_DEVICEARRIVAL = 0x8000;
        private const int WM_DEVICEREMOVECOMPLETE = 0X8004;


        // Threading Management
        private Thread newThread;
        private delegate void CallingDelegate();
        private Random random = new Random();


        public MainWindow()
        {

            InitializeComponent();
>>>>>>> rachel
            DataContext = new UCSettings();
            InitializeComponent();
            ButtonArray = new Button[9];
            SettingsView.ExecuteMethod += new EventHandler(UserControlHandler); //Handling when a button from SettingsView is pressed
            HelpView.ExecuteMethod += new EventHandler(UserControlHandler);
            try
            {
                ControlOptions.ItemsSource = ControlSource.Options;
                
                
            }
            catch (TypeInitializationException)
            {
                TitleBlock.Text = "Make sure the Control Options folder is set up correctly!";
                OptionsHeader.Text = "Couldn't list options";
            }
<<<<<<< HEAD
            
        }

        /*---------------------------
         
               UIElement events

         ---------------------------*/
        #region Navigating usercontrols
=======
            if (File.Exists("UCConfig.txt"))
            {
                try
                {
                    string lines = System.IO.File.ReadAllText("UCConfig.txt");

                    string[] words = lines.Split(' ');

                    for (int i = 0; i < 9; i++)
                    {
                        UCSettings.SetKey(words[i], i);
                    }
                    ((UCSettings)DataContext).IsCenter = words[9] == "True";
                    ((UCSettings)DataContext).IsHover = words[10] == "True";
                    ((UCSettings)DataContext).IsShake = words[11] == "True";
                    ((UCSettings)DataContext).IsSound = words[12] == "True";
                }
                catch(Exception)
                {
                    TitleBlock.Text = "Could not load settings from UCConfig.txt";
                }

            }
            // Server script
            var serverThread = new Thread(ServerRoutine);
            serverThread.Start();
            client = new HttpClient()
            {
                BaseAddress = new Uri(SERVER_ADDRESS)
            };

            // Obtain window handle and attach system message hook
            var handle = new WindowInteropHelper(this).EnsureHandle();
            windowHandle = HwndSource.FromHwnd(handle);
            windowHandle.AddHook(new HwndSourceHook(WndProc));

            Task.Run(() =>
            {
                serverThread.Join();
                server.ObtainUSBDeviceInfo();
                server.ObtainRemoteDeviceInfo();
            });
            server.RaiseUINotifEvent += Server_RaiseUINotifEvent;
        }

        private void Server_RaiseUINotifEvent(object sender, string e)
        {
            MessageBox.Show(e);
        }




        #region
        // Region of Code for USB device events
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
                    switch ((uint)wParam.ToInt32()) {
                        case WM_DEVICEARRIVAL:
                            NotifyServer(SERVER_ADDRESS + Notif.DeviceDetected, "");
                            break;
                        case WM_DEVICEREMOVECOMPLETE:
                            NotifyServer(SERVER_ADDRESS + Notif.DeviceDisconnected, "");
                            break;

                    }
                    break;
            }
            return IntPtr.Zero;
        }

        
        
        #endregion
>>>>>>> rachel
        private void PageOpen(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;
            switch (myButton.Content)
            {
                case "Settings":
                    SettingsView.Visibility = System.Windows.Visibility.Visible;
                    break;
                case "Help":
                    HelpView.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
            Overlay.Visibility = Visibility.Visible;
            MainView.Effect = new BlurEffect();
            CheckCenterMouse();
            OptionsExpander.Focusable = false;
            ControlOptions.IsTabStop = false;
            ((UCSettings)DataContext).IsOpen = true;
            
        }

        public void Outside_Click(object sender, RoutedEventArgs e)
        {
            HelpView.Visibility = System.Windows.Visibility.Collapsed;
            SettingsView.Visibility = System.Windows.Visibility.Collapsed;
            Overlay.Visibility = Visibility.Collapsed;
            MainView.Effect = null;
            ControlOptions.Focusable = true;
            OptionsExpander.IsTabStop = true;
            ((UCSettings)DataContext).IsOpen = false;
            SettingsView.SaveButton.Content = "Save Settings";
        }
        #endregion

        //Every time the selection changes in the CotrolOptions listbox the 3x3 
        //array of buttons in ButtonGrid is repolpulated. 
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonGrid.Children.Clear();

            if (ControlOptions.SelectedItem != null)
            {
                ControlOption myOption = (ControlOption)ControlOptions.SelectedItem;
                int visibleButtonCounter=0;                                                             //Used to iterate through label array from ControlOption

                for (int i = 0; i < 9; i++)
                {
                    ButtonArray[i] = new Button();

                    if (myOption.buttonVisible[i])
                    {
<<<<<<< HEAD
                        ButtonArray[i].Style = (Style)Application.Current.Resources["Pusher"];
=======


                        ButtonArray[i].Content = i.ToString();
                        string disp = i.ToString();
>>>>>>> rachel
                        ButtonArray[i].Name = "Button" + i.ToString();

                        ButtonArray[i].Margin = new Thickness(10, 10, 10, 10);

                        StackPanel ButtonContent = new StackPanel();
                        ButtonContent.HorizontalAlignment = HorizontalAlignment.Center;
                        ButtonContent.Orientation = Orientation.Vertical;
                        if (myOption.buttonUris[visibleButtonCounter] != null)
                        {
                            Image ContentImage = new Image();
                            ContentImage.Source = new BitmapImage(myOption.buttonUris[visibleButtonCounter]);
                            ContentImage.MaxWidth = 50;
                            ButtonContent.Children.Add(ContentImage);
                        }
                        TextBlock ContentText = new TextBlock();
                        ContentText.Text= myOption.buttonLabels[visibleButtonCounter];
                        ButtonContent.Children.Add(ContentText);

                        ButtonArray[i].Content = ButtonContent;
                        Grid.SetColumn(ButtonArray[i], i % 3 + 1);
                        Grid.SetRow(ButtonArray[i], i / 3 + 1);
                        ButtonGrid.Children.Add(ButtonArray[i]);
                        

                        ButtonArray[i].PreviewMouseDown += delegate (object a, MouseButtonEventArgs b)
                        {
                            if (((UCSettings)DataContext).IsSound) UCMethods.PlayMySound();
                        };

                        ButtonArray[i].Click += delegate (object a, RoutedEventArgs b)
                        {
<<<<<<< HEAD
=======
                            // Get DeviceInfo Object
                            Button sourceButton = (Button)a;
                            string buttonName = sourceButton.Name;
                            int buttonIndex = Int32.Parse(buttonName.Substring(6));

                            // To replace "AL5D" with reference from the selected menu or button
                            ControllerDevice currentDevice = server.ConnectedDeviceList["AL5D"];
                            DeviceInfo currentDeviceInfo = currentDevice.DeviceInfo;

                            if (currentDeviceInfo.ApiType == "LocalLib")
                            {
                                
                                int count = currentDeviceInfo.FunctionArray.Count;
                                if (buttonIndex > count - 1) { }
                                else
                                {
                                    Task.Run(() =>
                                    {
                                        MethodInfo methodToBind = server.GetMethodInfo(currentDevice, currentDeviceInfo.FunctionArray[buttonIndex].Name);
                                        methodToBind.Invoke(currentDevice.DeviceObject, null);
                                    });
                                }
                            }
                            else if (currentDeviceInfo.ApiType == "Http")
                            {
                                Dictionary<string, string> messageDict = new Dictionary<string, string>
                                {
                                    ["name"] = "smart lamp",
                                    ["method"] = "off"
                                };

                                NotifyServer(SERVER_ADDRESS + (int)Notif.PostToServer, JsonConvert.SerializeObject(messageDict));
                            }
>>>>>>> rachel
                            CheckCenterMouse();
                        };

                        ButtonArray[i].MouseEnter += delegate (object a, MouseEventArgs b)
                        {
                            if (((UCSettings)DataContext).IsHover)
                            {
                                CheckSound();
                                ((Button)a).RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                            }

                        };
                        visibleButtonCounter++;
                    }
                }

                if (myOption.textBoxVisible)
                {
                    TextBox myTextbox = new TextBox();
                    myTextbox.TextWrapping = TextWrapping.Wrap;
                    myTextbox.Name = "TextInput";
                    myTextbox.FontSize = 36;
                    Grid.SetColumn(myTextbox, 1);
                    Grid.SetRow(myTextbox, 1);
                    Grid.SetColumnSpan(myTextbox, 3);
                    ButtonGrid.Children.Add(myTextbox);
                    foreach (Control curControl in ButtonGrid.Children)
                    {
                        if (curControl.GetType() == HelpButton.GetType())
                            if (((TextBlock)((StackPanel)((Button)curControl).Content).Children[1]).Text.Equals("Clear"))
                            {
                                ((Button)curControl).Click += delegate (object a, RoutedEventArgs i)
                                {
                                    myTextbox.Text = null;
                                };

                            }
                    }
                }
                HeaderPic.Source = new BitmapImage(myOption.actualUri);
                ((UCSettings)DataContext).Message = myOption.name;
            }
            CheckCenterMouse();


        }

        #region Detecting keystrokes for keybinds and animating it
        //In Pusher style the multibinding animationcondition fires by comparing 
        //the name of indiviudual buttons to a stored "Button that's supposed to 
        //be pressed" string. This function changes that string, and fires the 
        //click event. Alternatively a new dependencyproperty could have been 
        //delared but this was simpler. 
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (!((UCSettings)DataContext).IsOpen)
            {
                if (e.Key == Key.Return)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if ((ButtonArray?[i]?.Content) != null && ButtonArray[i].IsFocused)
                        {
                            ((UCSettings)DataContext).ButtonKey = "Button" + i.ToString();
                            CheckSound();
                            return;
                        }
                    }
                }

                for (int i = 0; i < 9; i++)
                {
                    if (UCSettings.GetKey(i).Equals(e.Key.ToString()) && (ButtonArray?[i]?.Content) != null) //if content is null the button is not visible, so checksound shouldn't be played
                    {
                        ((UCSettings)DataContext).ButtonKey = "Button" + i.ToString();
                        ButtonArray[i].RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        CheckSound();
                        e.Handled = true;
                        return;
                    }
                }
                if (UCSettings.GetKey(9).Equals(e.Key.ToString()))  //key 9 is the key bound to the sidebar
                {
                    OptionsExpander.IsExpanded = !OptionsExpander.IsExpanded;
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.Escape) Outside_Click(null, null);
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            ((UCSettings)DataContext).ButtonKey = "ButtonNull";
        }
        #endregion

        #region Checks for settings
        private void CheckCenterMouse()
        {
            if (((UCSettings)DataContext).IsCenter)
                UCMethods.SetPosition(this);

        }
        
        private void CheckSound()
        {
            if (((UCSettings)DataContext).IsSound)
                UCMethods.PlayMySound();
        }
        #endregion

        #region Switch Control functions
       private void OptionsExpander_LostFocus(object sender, RoutedEventArgs e)
        {
<<<<<<< HEAD
            Expander myExpander = (Expander)sender;
            myExpander.SetResourceReference(Expander.BackgroundProperty, "ThemeBrush");
            OptionsHeader.SetResourceReference(TextBlock.BackgroundProperty, "ThemeBrush");
        }
=======
            server = new Server(SERVER_ADDRESS);
            server.Run();
            //deviceInterface.TestRoboticArm();
>>>>>>> rachel

        private void OptionsExpander_GotFocus(object sender, RoutedEventArgs e)
        {
            Expander myExpander = (Expander)sender;

            if (!myExpander.IsMouseOver)
            {
                myExpander.Background = (Brush)Application.Current.Resources["GoldBrush"];
                OptionsHeader.Background = (Brush)Application.Current.Resources["GoldBrush"];
            }
        }
        
        private void ControlOptions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (ControlOptions.SelectedIndex >= ControlSource.Options.Count - 1) ControlOptions.SelectedIndex = 0;
                else ControlOptions.SelectedIndex++;
            }
        }

        #endregion

<<<<<<< HEAD
        protected void UserControlHandler(object sender, EventArgs e)
        {
            Outside_Click(null, null); //Mainwindow has access to this method
        }

       
}
=======
        private void NotifyServer(string url, string content)
        {
            Task.Run(() =>
            {
                HttpRequestMessage message = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    Content = new StringContent(content),
                    RequestUri = new Uri(url)
                };
                message.Content.Headers.Clear();
                message.Content.Headers.Add("Content-Type", "application/json");
                client.SendAsync(message);
            });
        }
        

    }
    
>>>>>>> rachel

}