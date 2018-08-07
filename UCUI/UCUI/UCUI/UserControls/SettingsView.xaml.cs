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
            ImageArray = new Image[9];
            TextBoxArray = new TextBox[9];
            for (int i = 0; i < 9; i++)
            {
                ImageArray[i] = new Image();
                TextBoxArray[i] = new TextBox();
                Grid.SetColumn(ImageArray[i], i % 3 );
                Grid.SetRow(ImageArray[i], i / 3 );
                Grid.SetColumn(TextBoxArray[i], i % 3 );
                Grid.SetRow(TextBoxArray[i], i / 3 );
                BindGrid.Children.Add(ImageArray[i]);
                BindGrid.Children.Add(TextBoxArray[i]);

                TextBoxArray[i].PreviewTextInput += Bind_PreviewTextInput;
                TextBoxArray[i].PreviewKeyDown += Bind_PreviewKeyDown;
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
                    ((UCSettings)App.Current.MainWindow.DataContext).IsCenter = words[9] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsHover = words[10] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsShake = words[11] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsSound = words[12] == "True";
                    ((UCSettings)App.Current.MainWindow.DataContext).IsFull = words[13] == "True";

                    for (int j = 0; j < 9; j++)
                    {
                        if (words[j] != "null") TextBoxArray[j].Text = words[j];
                    }

                    ThemeBox.SelectedIndex = Int32.Parse(words[14]);
                }
                catch (Exception)
                {
                    ((UCSettings)App.Current.MainWindow.DataContext).Message = "Could not load settings from UCConfig.txt";
                }
            }
        }

        //For TextBoxArray.Text sometimes Key.Tostring sometimes Text[0] is used, So things like LeftShift will show up, but not Oem-s.
        private void Bind_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox myTextBox = (TextBox)sender;
            myTextBox.Text = null;

            if (e.Key == Key.Tab) return;
            if(e.Key==Key.Back)
            {
                UCSettings.SetKey("null", Array.IndexOf(TextBoxArray, myTextBox));
                return;
            }
            
            if(e.Key.ToString().Length>2)
            {
                if (e.Key.ToString()[2] != 'm') myTextBox.Text = e.Key.ToString();
            }
            else if(e.Key.ToString().Length==1) myTextBox.Text = e.Key.ToString();
            UCSettings.SetKey(e.Key.ToString(), Array.IndexOf(TextBoxArray, myTextBox));
        }

        private void Bind_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!Char.IsNumber(e.Text[0])&& !Char.IsPunctuation(e.Text[0]) && !Char.IsSymbol(e.Text[0]))
            e.Handled = true;
           
        }

        //Saves information of current state of settings so it can be loaded during startup next time.
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0; i<9; i++)
            {
                if(UCSettings.GetKey(i).Length!=0)sb.Append(UCSettings.GetKey(i)).Append(" ");
                else sb.Append("null ");
            }
            sb.Append(CenterMouse.IsChecked.ToString()).Append(" ");
            sb.Append(HoverButton.IsChecked.ToString()).Append(" ");
            sb.Append(ShakeButton.IsChecked.ToString()).Append(" ");
            sb.Append(AudioButton.IsChecked.ToString()).Append(" ");
            sb.Append(FullScreenButton.IsChecked.ToString()).Append(" ");
            sb.Append(ThemeBox.SelectedIndex.ToString());
            if (File.Exists("UCConfig.txt"))
            System.IO.File.WriteAllText("UCConfig.txt", sb.ToString());
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0; i<9; i++)
            {
                TextBoxArray[i].Text = null;
                UCSettings.SetKey("null", i);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(ThemeBox.SelectedIndex)
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
        }
    }
}
