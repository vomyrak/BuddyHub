using System;
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
using CSharpServer;
using System.Threading;


namespace UCUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[] ButtonArray;
        private Server server;
        private DeviceInterface deviceInterface;

        public MainWindow()
        {
            InitializeComponent();
            ControlOptions.ItemsSource = ControlSource.Options;
            Panel.SetZIndex(MainView, 0);
            Panel.SetZIndex(HelpView, 3);
            Panel.SetZIndex(SettingsView, 3);
            Panel.SetZIndex(Overlay, 1);
            Panel.SetZIndex(Outside, 2);
            DataContext = new UCSettings();

            // Server Script
            var serverThread = new Thread(ServerRoutine);
            serverThread.Start();

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
            CheckCenterMouse(null, null);
        }

        private void Outside_Click(object sender, RoutedEventArgs e)
        {
            HelpView.Visibility = System.Windows.Visibility.Collapsed;
            SettingsView.Visibility = System.Windows.Visibility.Collapsed;
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
                for (int i = 0; i < 9; i++)
                {
                    ButtonArray[i] = new Button();
                }

                ControlOption myOption = (ControlOption)ControlOptions.SelectedItem;

                for (int i = 0; i < 9; i++)
                {
                    if (myOption.buttonVisible[i])
                    {
                        ButtonArray[i].Content = i.ToString();
                        string disp = i.ToString();
                        ButtonArray[i].Name = "Button" + i.ToString();
                        ButtonArray[i].Margin = new Thickness(10, 10, 10, 10);
                        ButtonArray[i].Click += CheckCenterMouse;
                        Grid.SetColumn(ButtonArray[i], i % 3 + 1);
                        Grid.SetRow(ButtonArray[i], i / 3 + 1);
                        ButtonGrid.Children.Add(ButtonArray[i]);
                        ButtonArray[i].Style = (Style)Application.Current.Resources["Pusher"];

                        ButtonArray[i].PreviewMouseDown += delegate (object a, MouseButtonEventArgs b)
                        {
                            if(((UCSettings)DataContext).IsSound) UCMethods.PlayMySound();
                        };
                        ButtonArray[i].Click += delegate (object a, RoutedEventArgs b)
                        {
                            //deviceInterface.TestRoboticArm();
                            var newThread = new Thread(deviceInterface.TestRoboticArm);
                            newThread.Start();
                        };

                        ButtonArray[i].MouseEnter += delegate (object a, MouseEventArgs b)
                        {
                            if (((UCSettings)DataContext).IsHover)
                            {
                                CheckSound(null, null);
                                CheckCenterMouse(null, null);
                            }
                           
                        };
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
                }
            }
            CheckCenterMouse(null, null);

        }

        private void CheckCenterMouse(object sender, RoutedEventArgs e)
        {
            if (((UCSettings)DataContext).IsCenter) 
                UCMethods.SetPosition(this);

        }

        private void CheckSound(object sender, RoutedEventArgs e)
        {
            if (((UCSettings)DataContext).IsSound)
                UCMethods.PlayMySound();
        }



        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            UCMethods.PlayMySound();
            switch(e.Key)
            {
                case (Key.NumPad7):
                    ((UCSettings)DataContext).ButtonKey = "Button0";
                    break;
                case (Key.NumPad8):
                    ((UCSettings)DataContext).ButtonKey = "Button1";
                    break;
                case (Key.NumPad9):
                    ((UCSettings)DataContext).ButtonKey = "Button2";
                    break;
                case (Key.NumPad4):
                    ((UCSettings)DataContext).ButtonKey = "Button3";
                    break;
                case (Key.NumPad5):
                    ((UCSettings)DataContext).ButtonKey = "Button4";
                    break;
                case (Key.NumPad6):
                    ((UCSettings)DataContext).ButtonKey = "Button5";
                    break;
                case (Key.NumPad1):
                    ((UCSettings)DataContext).ButtonKey = "Button6";
                    break;
                case (Key.NumPad2):
                    ((UCSettings)DataContext).ButtonKey = "Button7";
                    break;
                case (Key.NumPad3):
                    ((UCSettings)DataContext).ButtonKey = "Button8";
                    break;

            }
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            ((UCSettings)DataContext).ButtonKey = "Button12";
        }

        private void ServerRoutine()
        {
            server = new Server();
            deviceInterface = new DeviceInterface();
            server.Run();
            //deviceInterface.TestRoboticArm();

        }

    }

}
