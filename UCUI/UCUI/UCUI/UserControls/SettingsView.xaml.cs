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
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UCUI.Models;
using System.IO;
using System.Threading;

namespace UCUI.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);

        private Image[] ImageArray;
        private TextBox[] TextBoxArray;

        public SettingsView()
        {
            InitializeComponent();

            //The code below saves space in the XAML for populating the grid with visuals
            #region poulating BindGrid
            ImageArray = new Image[10];
            TextBoxArray = new TextBox[10];
            for (int i = 0; i < 9; i++)
            {
                ImageArray[i] = new Image();
                TextBoxArray[i] = new TextBox();
                Grid.SetColumn(ImageArray[i], i % 3);
                Grid.SetRow(ImageArray[i], i / 3);
                Grid.SetColumn(TextBoxArray[i], i % 3);
                Grid.SetRow(TextBoxArray[i], i / 3);
                BindGrid.Children.Add(ImageArray[i]);
                BindGrid.Children.Add(TextBoxArray[i]);

                TextBoxArray[i].PreviewTextInput += Bind_PreviewTextInput;
                TextBoxArray[i].PreviewKeyDown += Bind_PreviewKeyDown;
            }
            ImageArray[9] = new Image();
            TextBoxArray[9] = new TextBox();
            SidebarGrid.Children.Add(ImageArray[9]);
            SidebarGrid.Children.Add(TextBoxArray[9]);

            TextBoxArray[9].PreviewTextInput += Bind_PreviewTextInput;
            TextBoxArray[9].PreviewKeyDown += Bind_PreviewKeyDown;
            #endregion

            //Loading settings:
            if (File.Exists("UCConfig.txt"))
            {
                try
                {
                    string lines = System.IO.File.ReadAllText("UCConfig.txt");

                    string[] words = lines.Split(' ');

                    for (int i = 0; i < 10; i++)
                    {
                        UCSettings.SetKey(words[i], i);
                    }
                    ((UCSettings)App.Current.MainWindow.DataContext).IsCenter = words[10] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsHover = words[11] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsShake = words[12] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsSound = words[13] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsFull = words[14] == "True";

                    for (int j = 0; j < 10; j++)
                    {
                        if (words[j] != "null") TextBoxArray[j].Text = words[j];
                    }

                    ThemeBox.SelectedIndex = Int32.Parse(words[15]);
                }
                catch (Exception)
                {
                    ((UCSettings)App.Current.MainWindow.DataContext).Message = "Could not load settings from UCConfig.txt";
                }
            }
        }

        //For TextBoxArray.Text sometimes Key.Tostring sometimes Text[0] is used, So things like LeftShift will show up, but not Oem-s. Hence the two events.
        #region Handling key events for binding
        private void Bind_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab || e.Key == Key.Escape) return;
            TextBox myTextBox = (TextBox)sender;
            myTextBox.Text = null;

            if (e.Key == Key.Back)
            {
                UCSettings.SetKey("null", Array.IndexOf(TextBoxArray, myTextBox));
                return;
            }

            if (e.Key.ToString().Length > 2)
            {
                if (e.Key.ToString()[2] != 'm') myTextBox.Text = e.Key.ToString();
            }
            else myTextBox.Text = e.Key.ToString();
            UCSettings.SetKey(e.Key.ToString(), Array.IndexOf(TextBoxArray, myTextBox));
        }

        private void Bind_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsNumber(e.Text[0]) && !Char.IsPunctuation(e.Text[0]) && !Char.IsSymbol(e.Text[0]))
                e.Handled = true;

        }
        #endregion

        public void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Content)
            {
                case "Save Settings":  //Saves information of current state of settings so it can be loaded during startup next time.
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < 10; i++)
                    {
                        if (UCSettings.GetKey(i).Length != 0) sb.Append(UCSettings.GetKey(i)).Append(" ");
                        else sb.Append("null ");
                    }
                    sb.Append(CenterMouse.IsChecked.ToString()).Append(" ");
                    sb.Append(HoverButton.IsChecked.ToString()).Append(" ");
                    sb.Append(ShakeButton.IsChecked.ToString()).Append(" ");
                    sb.Append(AudioButton.IsChecked.ToString()).Append(" ");
                    sb.Append(FullScreenButton.IsChecked.ToString()).Append(" ");
                    sb.Append(ThemeBox.SelectedIndex.ToString());
                    if (File.Exists("UCConfig.txt"))
                    {
                        System.IO.File.WriteAllText("UCConfig.txt", sb.ToString());
                        SaveButton.Content = "Saved!";
                    }
                    break;

                case "Clear":
                    for (int i = 0; i < 10; i++)
                    {
                        TextBoxArray[i].Text = null;
                        UCSettings.SetKey("null", i);
                    }
                    break;

                case "Return":
                    OnExecuteMethod();
                    break;
            }
        }


        #region Methods for changing UI theme, and doing so with switches and controlling Autotabbing
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBox)sender).Name)
            {
                case "ThemeBox":
                    switch (ThemeBox.SelectedIndex)
                    {
                        case 0:
                            Application.Current.Resources["ThemeBrush"] = new SolidColorBrush((Color)Application.Current.Resources["myGray"]);
                            Application.Current.Resources["ButtonBrush"] = new SolidColorBrush(Colors.Gray);
                            break;
                        case 1:
                            Application.Current.Resources["ThemeBrush"] = new SolidColorBrush(Colors.DarkSlateBlue);
                            Application.Current.Resources["ButtonBrush"] = new SolidColorBrush(Colors.DarkTurquoise);
                            break;
                    }
                    break;

                case "TimerBox":
                    Thread thread = new Thread(UCSettings.AutoTab);
                    thread.IsBackground = true;
                    switch (TimerBox.SelectedIndex)
                    {
                        case 0:
                            UCSettings.IsAuto = false;
                            break;
                        case 1:
                            UCSettings.IsAuto = true;
                            UCSettings.AutoTabTime = 1000;
                            if (!thread.IsAlive) thread.Start();
                            break;
                        case 2:
                            UCSettings.IsAuto = true;
                            UCSettings.AutoTabTime = 1500;
                            if (!thread.IsAlive)
                            {
                                thread.Start();
                                ((UCSettings)App.Current.MainWindow.DataContext).Message = "Oh shit";
                            }
                            break;
                        case 3:
                            UCSettings.IsAuto = true;
                            UCSettings.AutoTabTime = 2000;
                            if (!thread.IsAlive) thread.Start();
                            break;
                    }
                    break;
            }
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e) //For some reason tab didn't highlight items (only focused) so this was added for switch control
        {
            if (e.Key == Key.Return)
            {
                ComboBox myComboBox = (ComboBox)sender;
                for (int i = 0; i < myComboBox.Items.Count; i++)
                {
                    if (((ComboBoxItem)myComboBox.Items[i]).IsFocused)
                    {
                        myComboBox.SelectedIndex = i;
                    }
                }
            }
        }
        #endregion

        #region Creating return button event the mainwindow can subscribe to
        public event EventHandler ExecuteMethod; //I couldn't set the visibility of the overlay to collapsed, since I couldn't access it from this namespace. So I'm using the Mianwindow's Outside_Click method with an event

        protected virtual void OnExecuteMethod()
        {
            if (ExecuteMethod != null) ExecuteMethod(this, EventArgs.Empty);
        }


        #endregion


    }
}
