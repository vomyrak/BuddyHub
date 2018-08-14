using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using UCUI.Models;
using UCUI.UserControls;
using System.Windows.Interop;
using System.Windows.Controls.Primitives;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using AppServer;

namespace UCUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[] ButtonArray;
        //private Server server;
        private HttpClient client;
        private WebServer internalServer;

        private const string SERVER_ADDRESS = "http://localhost:8080/";
        private const string INTERNAL_ADDRESS = "http://localhost:8192/";
        // USB Message Constants
        private const int WM_DEVICECHANGE = 0x219;
        private const int WM_DEVICEARRIVAL = 0x8000;
        private const int WM_DEVICEREMOVECOMPLETE = 0X8004;

        public MainWindow()
        {

            InitializeComponent();
            DataContext = new UCSettings();
            try
            {
                ControlOptions.ItemsSource = ControlSource.Options;
            }
            catch (TypeInitializationException)
            {
                TitleBlock.Text = "Make sure the Control Options folder is set up correctly!";
                OptionsHeader.Text = "Couldn't list options";
            }
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
            
            client = new HttpClient()
            {
                BaseAddress = new Uri(SERVER_ADDRESS)
            };


            internalServer = new WebServer(ProcessUINotif, INTERNAL_ADDRESS);
            Task.Run(() =>
            {
                internalServer.Run();
            });
        }

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
            Outside.Visibility = Visibility.Visible;
            MainView.Effect = new BlurEffect();
            CheckCenterMouse();
        }

        private void Outside_Click(object sender, RoutedEventArgs e)
        {
            HelpView.Visibility = Visibility.Collapsed;
            SettingsView.Visibility = Visibility.Collapsed;
            Outside.Visibility = Visibility.Collapsed;
            Overlay.Visibility = Visibility.Collapsed;
            MainView.Effect = null;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonGrid.Children.Clear();

            if (ControlOptions.SelectedItem != null)
            {
                ButtonArray = new Button[9];
                TextBox myTextbox = null;
                ControlOption myOption = (ControlOption)ControlOptions.SelectedItem;
                int visibleButtonCounter=0;
                for (int i = 0; i < 9; i++)
                {
                    ButtonArray[i] = new Button();

                    if (myOption.buttonVisible[i])
                    {


                        ButtonArray[i].Content = i.ToString();
                        string disp = i.ToString();
                        ButtonArray[i].Name = "Button" + i.ToString();
                        ButtonArray[i].Content = myOption.buttonLabels[visibleButtonCounter];
                        ButtonArray[i].Margin = new Thickness(10, 10, 10, 10);
                        Grid.SetColumn(ButtonArray[i], i % 3 + 1);
                        Grid.SetRow(ButtonArray[i], i / 3 + 1);
                        ButtonGrid.Children.Add(ButtonArray[i]);
                        ButtonArray[i].Style = (Style)Application.Current.Resources["Pusher"];

                        ButtonArray[i].PreviewMouseDown += delegate (object a, MouseButtonEventArgs b)
                        {
                            if (((UCSettings)DataContext).IsSound) UCMethods.PlayMySound();
                        };

                        ButtonArray[i].Click += delegate (object a, RoutedEventArgs b)
                        {
                            // Get DeviceInfo Object
                            Button sourceButton = (Button)a;
                            string buttonName = sourceButton.Name;
                            int buttonIndex = Int32.Parse(buttonName.Substring(6));


                            NotifyServer(SERVER_ADDRESS + "Alexa" + "/" + "Text To Speech",
                                JsonConvert.SerializeObject(new Dictionary<string, string>() { ["input"] = myTextbox.Text }), 
                                "POST");
                            
                            
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
                    myTextbox = new TextBox();
                    myTextbox.TextWrapping = TextWrapping.Wrap;
                    myTextbox.Name = "TextInput";
                    myTextbox.FontSize = 36;
                    Grid.SetColumn(myTextbox, 1);
                    Grid.SetRow(myTextbox, 1);
                    Grid.SetColumnSpan(myTextbox, 3);
                    ButtonGrid.Children.Add(myTextbox);

                }
                HeaderPic.Source = new BitmapImage(myOption.actualUri);
                TitleBlock.Text = myOption.name;
            }
            CheckCenterMouse();


        }

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



        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (SettingsView.Visibility != Visibility.Visible)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (UCSettings.GetKey(i) == e.Key.ToString()&& ButtonArray[i].Content != null)
                    {
                        ((UCSettings)DataContext).ButtonKey = "Button" + i.ToString();
                        ButtonArray[i].RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                        CheckSound();
                        break;
                    }
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            ((UCSettings)DataContext).ButtonKey = "ButtonNull";
        }

        private void NotifyServer(string url, string content, string method)
        {
            Task.Run(() =>
            {
                HttpRequestMessage message = new HttpRequestMessage()
                {
                    Method = new HttpMethod(method),
                    Content = new StringContent(content),
                    RequestUri = new Uri(url)
                };
                message.Content.Headers.Clear();
                message.Content.Headers.Add("Content-Type", "application/json");
                client.SendAsync(message);
            });
        }
        
        private string ProcessUINotif(HttpListenerRequest request)
        {
            string rawUrl = request.RawUrl.Replace("%20", " ");
            string[] parsedRequest = rawUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (request.HttpMethod == "POST")
            {
                MessageBox.Show(parsedRequest[0]);
            }
            return "";
        }

    }
    

}
