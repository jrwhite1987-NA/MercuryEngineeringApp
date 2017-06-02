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

namespace MercuryEngApp.Views
{
    /// <summary>
    /// Interaction logic for LogUserControl.xaml
    /// </summary>
    public partial class LogUserControl : UserControl
    {
        public LogUserControl()
        {
            InitializeComponent();
            this.DataContext = App.mainWindow.mainViewModel;
            Loaded += OnLogLoaded;
            
        }

        private void OnLogLoaded(object sender, RoutedEventArgs e)
        {
            this.LogTabControl.SelectedIndex = App.mainWindow.LogSelectedTabIndex;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            App.mainWindow.NavigationTabs.SelectedIndex = App.mainWindow.previousIndex;
            App.mainWindow.FooterTextBox.Visibility = Visibility.Visible;
        }

        private void ClearLogClick(object sender, RoutedEventArgs e)
        {
            App.mainWindow.mainViewModel.TCDLog = "";
            App.mainWindow.mainViewModel.ApplicationLog = "";
        }
    }
}
