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
using System.Windows.Controls.Primitives;
using System.IO;

namespace UCUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[] ButtonArray;

        public MainWindow()
        {
            DataContext = new UCSettings();
            InitializeComponent();
            try
            {
                ControlOptions.ItemsSource = ControlSource.Options;
            }
            catch (TypeInitializationException)
            {
                TitleBlock.Text = "Make sure the Control Options folder is set up correctly!";
                OptionsHeader.Text = "Couldn't list options";
            }
            
        }

        /*---------------------------
         
               UIElement events

         ---------------------------*/

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
            
        }

        private void Outside_Click(object sender, RoutedEventArgs e)
        {
            HelpView.Visibility = System.Windows.Visibility.Collapsed;
            SettingsView.Visibility = System.Windows.Visibility.Collapsed;
            Overlay.Visibility = Visibility.Collapsed;
            MainView.Effect = null;
        }


        //Every time the selection changes in the CotrolOptions listbox the 3x3 
        //array of buttons in ButtonGrid is repolpulated. 
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonGrid.Children.Clear();

            if (ControlOptions.SelectedItem != null)
            {
                ButtonArray = new Button[9];
                ControlOption myOption = (ControlOption)ControlOptions.SelectedItem;
                int visibleButtonCounter=0;                                                             //Used to iterate through label array from ControlOption

                for (int i = 0; i < 9; i++)
                {
                    ButtonArray[i] = new Button();

                    if (myOption.buttonVisible[i])
                    {
                        ButtonArray[i].Style = (Style)Application.Current.Resources["Pusher"];
                        ButtonArray[i].Name = "Button" + i.ToString();
                        ButtonArray[i].Content = myOption.buttonLabels[visibleButtonCounter];
                        ButtonArray[i].Margin = new Thickness(10, 10, 10, 10);
                        Grid.SetColumn(ButtonArray[i], i % 3 + 1);
                        Grid.SetRow(ButtonArray[i], i / 3 + 1);
                        ButtonGrid.Children.Add(ButtonArray[i]);
                        

                        ButtonArray[i].PreviewMouseDown += delegate (object a, MouseButtonEventArgs b)
                        {
                            if (((UCSettings)DataContext).IsSound) UCMethods.PlayMySound();
                        };

                        ButtonArray[i].Click += delegate (object a, RoutedEventArgs b)
                        {
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
                }
                HeaderPic.Source = new BitmapImage(myOption.actualUri);
                TitleBlock.Text = myOption.name;
            }
            CheckCenterMouse();


        }

        //In Pusher style the multibinding animationcondition fires by comparing 
        //the name of indiviudual buttons to a stored "Button that's supposed to 
        //be pressed" string. This function changes that string, and fires the 
        //click event. Alternatively a new dependencyproperty could have been 
        //delared but this was simpler. 
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (SettingsView.Visibility != Visibility.Visible)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (UCSettings.GetKey(i).Equals(e.Key.ToString())&& ButtonArray[i].Content != null)
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

    }

}
