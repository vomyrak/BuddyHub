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
            
        }

        /*---------------------------
         
               UIElement events

         ---------------------------*/
        #region Navigating usercontrols
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
                        ButtonArray[i].Style = (Style)Application.Current.Resources["Pusher"];
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
            Expander myExpander = (Expander)sender;
            myExpander.SetResourceReference(Expander.BackgroundProperty, "ThemeBrush");
            OptionsHeader.SetResourceReference(TextBlock.BackgroundProperty, "ThemeBrush");
        }

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

        protected void UserControlHandler(object sender, EventArgs e)
        {
            Outside_Click(null, null); //Mainwindow has access to this method
        }

       
}

}
