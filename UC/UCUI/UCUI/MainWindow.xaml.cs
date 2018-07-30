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
        public MainWindow()
        {
            InitializeComponent();
            ControlOptions.ItemsSource=ControlSource.Options;
            Panel.SetZIndex(MainView, 0);
            Panel.SetZIndex(HelpView, 3);
            Panel.SetZIndex(SettingsView, 3);
            Panel.SetZIndex(Overlay, 1);
            Panel.SetZIndex(Outside, 2);


        }

        private void HelpOpen(object sender, RoutedEventArgs e)
        {
            HelpView.Visibility = System.Windows.Visibility.Visible;
            Overlay.Visibility = Visibility.Visible;
            Outside.Visibility = Visibility.Visible;
            MainView.Effect = new BlurEffect();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2);
        }

       
        private void SettingsOpen(object sender, RoutedEventArgs e)
        {
            SettingsView.Visibility = System.Windows.Visibility.Visible;
            Outside.Visibility = Visibility.Visible;
            Overlay.Visibility = Visibility.Visible;
            MainView.Effect = new BlurEffect();
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height / 2);
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
            if (ControlOptions.SelectedItem != null)
                foreach (Button curButton in ButtonGrid.Children)
                {
                    curButton.Content = ((ControlOption)ControlOptions.SelectedItem).name;
                }
        }


    }

}
