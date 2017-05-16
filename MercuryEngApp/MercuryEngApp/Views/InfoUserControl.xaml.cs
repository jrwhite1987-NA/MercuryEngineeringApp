using MercuryEngApp.Common;
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
using UsbTcdLibrary;
using UsbTcdLibrary.CommunicationProtocol;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for InfoTabUserControl.xaml
    /// </summary>
    public partial class InfoUserControl : UserControl
    {
        private InfoViewModel infoViewModelObj = new InfoViewModel();

        public InfoUserControl()
        {
            InitializeComponent();
            this.Loaded += InfoUserControlLoaded;
            this.DataContext = infoViewModelObj;
        }

        async void InfoUserControlLoaded(object sender, RoutedEventArgs e)
        {
            App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
            await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Service);
        }

        private void ReadBoardInfoClick(object sender, RoutedEventArgs e)
        {
        }

        private void WriteBoardInfoClick(object sender, RoutedEventArgs e)
        {

        }

        private void ReadChannelClick(object sender, RoutedEventArgs e)
        {

        }

        private void WriteChannelClick(object sender, RoutedEventArgs e)
        {

        }

        private void ReadProbeInfoClick(object sender, RoutedEventArgs e)
        {

        }

        private void WriteProbeInfoClick(object sender, RoutedEventArgs e)
        {

        }

    }
}
