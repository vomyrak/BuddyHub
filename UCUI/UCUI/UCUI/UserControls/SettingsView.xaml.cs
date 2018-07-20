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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UCUI.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }


        private void ShakeButton_Click(object sender, RoutedEventArgs e)
        {
            if((bool)ShakeButton.IsChecked)
            {
                Feedback.Style = (Style)Application.Current.Resources["Shaker"];
                ScanTemplates.Style = (Style)Application.Current.Resources["Shaker"];
            }
            else
            {
                Feedback.Style = (Style)Application.Current.Resources["DefaultSmall"];
                ScanTemplates.Style = (Style)Application.Current.Resources["DefaultSmall"];
            }
        }
    }
}
