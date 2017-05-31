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
using System.Xml.Linq;
using System.Xml.Serialization;
using UsbTcdLibrary;
using UsbTcdLibrary.CommunicationProtocol;
using log4net;
using Core.Common;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for InfoTabUserControl.xaml
    /// </summary>
    public partial class InfoUserControl : UserControl
    {
        private InfoViewModel infoViewModelObj = new InfoViewModel();
        public System.Xml.XmlReader stream { get; set; }
        XDocument xmlDoc;

        public InfoUserControl()
        {
            InitializeComponent();
            this.Loaded += InfoUserControlLoaded;
            this.DataContext = infoViewModelObj;
            xmlDoc = XDocument.Load("LocalFolder/InfoConfig.xml");
            infoViewModelObj.ProbePartNumberList = xmlDoc.Root.Elements("ProbePartNumbers").Elements("PartNumber").Select(element => element.Value).ToList();
            infoViewModelObj.BoardPartNumberList = xmlDoc.Root.Elements("BoardPartNumbers").Elements("PartNumber").Select(element => element.Value).ToList();
            infoViewModelObj.BoardModelNameList = xmlDoc.Root.Elements("BoardModelNames").Elements("ModelName").Select(element => element.Value).ToList();
            infoViewModelObj.BoardHardwareRevisionList = xmlDoc.Root.Elements("HardwareRevisions").Elements("Revision").Select(element => element.Value).ToList();
        }

        async void InfoUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Service);
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private async void ReadBoardInfoClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDRequest request = new TCDRequest())
                {
                    request.ChannelID = App.CurrentChannel;
                    TCDReadInfoResponse response = await UsbTcd.TCDObj.GetModuleInfo(request);
                    infoViewModelObj.SelectedBoardPartNumber = response.Module.partNumberString;
                    infoViewModelObj.SelectedBoardModelName = response.Module.modelString;
                    infoViewModelObj.SelectedHardwareRevision = response.Module.hardwareRevisionString;
                    infoViewModelObj.BoardSerialNumber = response.Module.serialNumberString;
                }
                App.ApplicationLog += "Read Board Info";
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private async void WriteBoardInfoClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDWriteInfoRequest request = new TCDWriteInfoRequest())
                {
                    request.ChannelID = App.CurrentChannel;
                    request.Board = new UsbTcdLibrary.StatusClasses.BoardInfo();
                    request.Board.partNumberString = infoViewModelObj.SelectedBoardPartNumber;
                    request.Board.modelString = infoViewModelObj.SelectedBoardModelName;
                    string[] temp = infoViewModelObj.SelectedHardwareRevision.Split('.');
                    int strLen = temp.Length;
                    byte[] arr = new byte[4];
                    for (int i = 0; i < strLen; i++)
                    {
                        arr[3 - i] = Convert.ToByte(temp[i]);
                    }
                    request.Board.hardwareRevision = BitConverter.ToUInt32(arr, 0);
                    request.Board.serialNumberString = infoViewModelObj.BoardSerialNumber;
                    await UsbTcd.TCDObj.WriteBoardInfoAsync(request);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private async void ReadChannelClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDRequest request = new TCDRequest())
                {
                    request.ChannelID = App.CurrentChannel;
                    infoViewModelObj.ChannelNumber = await UsbTcd.TCDObj.GetChannelNumber(request);
                }
                App.ApplicationLog += "Read Channel Info";
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private async void WriteChannelClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDRequest request = new TCDRequest())
                {
                    request.ChannelID = App.CurrentChannel;
                    request.Value2 = infoViewModelObj.ChannelNumber;
                    await UsbTcd.TCDObj.AssignChannelAsync(request);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private async void ReadProbeInfoClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDRequest request = new TCDRequest())
                {
                    request.ChannelID = App.CurrentChannel;
                    TCDReadInfoResponse response = await UsbTcd.TCDObj.GetProbeInfo(request);
                    infoViewModelObj.SelectedProbePartNumber = response.Probe.partNumber;
                    infoViewModelObj.SelectedProbePartNumber = "2";
                    infoViewModelObj.ProbeModelName = response.Probe.descriptionString;
                    infoViewModelObj.PhysicalID = response.Probe.physicalId;
                    infoViewModelObj.ProbeSerialNumber = response.Probe.serialNumberString;
                    infoViewModelObj.FormatID = response.Probe.formatId;
                    infoViewModelObj.CenterFrequency = response.Probe.centerFrequency;
                    infoViewModelObj.Diameter = response.Probe.diameter;
                    infoViewModelObj.TankFocalLength = response.Probe.tankFocalLength;
                    infoViewModelObj.InsertionLoss = response.Probe.insertionLoss;
                    infoViewModelObj.TIC = response.Probe.TIC;
                    infoViewModelObj.Fractional3dbBW = response.Probe.fractionalBW;
                    infoViewModelObj.Impedance = response.Probe.impedance;
                    infoViewModelObj.PhaseAngle = response.Probe.phaseAngle;
                }
                App.ApplicationLog += "Read Probe Info";
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private async void WriteProbeInfoClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDWriteInfoRequest request = new TCDWriteInfoRequest())
                {
                    request.ChannelID = App.CurrentChannel;
                    request.Probe = new UsbTcdLibrary.StatusClasses.ProbeInfo();
                    request.Probe.partNumber = infoViewModelObj.SelectedProbePartNumber;
                    request.Probe.descriptionString = infoViewModelObj.ProbeModelName;
                    request.Probe.physicalId = infoViewModelObj.PhysicalID;
                    request.Probe.serialNumberString = infoViewModelObj.ProbeSerialNumber;
                    request.Probe.formatId = infoViewModelObj.FormatID;
                    request.Probe.centerFrequency = infoViewModelObj.CenterFrequency;
                    request.Probe.diameter = infoViewModelObj.Diameter;
                    request.Probe.tankFocalLength = infoViewModelObj.TankFocalLength;
                    request.Probe.insertionLoss = infoViewModelObj.InsertionLoss;
                    request.Probe.TIC = infoViewModelObj.TIC;
                    request.Probe.fractionalBW = infoViewModelObj.Fractional3dbBW;
                    request.Probe.impedance = infoViewModelObj.Impedance;
                    request.Probe.phaseAngle = infoViewModelObj.PhaseAngle;
                    await UsbTcd.TCDObj.WriteProbeInfoAsync(request);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }
    }
}
