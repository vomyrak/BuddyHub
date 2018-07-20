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
using UCUI.Hook;



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
            InitializeComponent();
            ControlOptions.ItemsSource=ControlSource.Options;
            Panel.SetZIndex(MainView, 0);
            Panel.SetZIndex(HelpView, 3);
            Panel.SetZIndex(SettingsView, 3);
            Panel.SetZIndex(Overlay, 1);
            Panel.SetZIndex(Outside, 2);
            DataContext = new UCSettings();
            HelpButton.Style= (Style)Application.Current.Resources["Shaker"];




        }

        private void PageOpen(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;
            switch(myButton.Content)
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
                for(int i=0; i<9; i++)
                {
                    ButtonArray[i] = new Button();
                }
                
                ControlOption myOption = (ControlOption)ControlOptions.SelectedItem;
                
                for (int i = 0; i < 9; i++)
                {
                    if (myOption.buttonVisible[i])
                    {
                        ButtonArray[i].Content = i.ToString();
                        ButtonArray[i].Name = "Button" + i.ToString();
                        ButtonArray[i].Margin = new Thickness(10, 10, 10, 10);
                        ButtonArray[i].Click += CheckCenterMouse;
                        Grid.SetColumn(ButtonArray[i], i % 3 + 1);
                        Grid.SetRow(ButtonArray[i], i/3 + 1);
                        ButtonGrid.Children.Add(ButtonArray[i]);
                        ButtonArray[i].Style = (Style)Application.Current.Resources["Shaker"];
                    }
                }

                if(myOption.textBoxVisible)
                {
                    TextBox myTextbox = new TextBox();
                    myTextbox.TextWrapping = TextWrapping.Wrap;
                    myTextbox.Name = "TextInput";
                    myTextbox.FontSize = 36;
                    Grid.SetColumn(myTextbox, 1 );
                    Grid.SetRow(myTextbox, 1);
                    Grid.SetColumnSpan(myTextbox, 3);
                    ButtonGrid.Children.Add(myTextbox);
                }
            }
            CheckCenterMouse(null, null);
            
        }

        private void CheckCenterMouse(object sender, RoutedEventArgs e)
        {
            if ((bool)SettingsView.CenterMouse.IsChecked)
            {
               System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2);

            }
        }


    }

}
