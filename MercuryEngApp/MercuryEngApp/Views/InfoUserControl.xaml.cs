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
using System.Collections.ObjectModel;
using Core.Constants;

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
            infoViewModelObj.ChannelNumberList = xmlDoc.Root.Elements("ChannelNumbers").Elements("Channel").
                Select(element => Convert.ToByte(element.Value)).ToList();
            infoViewModelObj.ProbeModelNameList = xmlDoc.Root.Elements("ProbeModelNames").Elements("ModelName").Select(element => element.Value).ToList();
            infoViewModelObj.ProbePhysicalIDList = xmlDoc.Root.Elements("ProbePhysicalIDs").Elements("PhysicalID").
                Select(element => Convert.ToByte(element.Value)).ToList();
            infoViewModelObj.ProbeFormatIDList = xmlDoc.Root.Elements("ProbeFormatIDs").Elements("FormatID").
                Select(element => Convert.ToByte(element.Value)).ToList();

        }

        async void InfoUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                if (PowerController.Instance.IsControllerOn)
                {
                    this.IsEnabled = true;
                    App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                    await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Service);
                    using (TCDRequest request = new TCDRequest())
                    {
                        request.ChannelID = App.CurrentChannel;
                        request.Value = Constants.VALUE_10;
                        await ReadServiceLog(request);
                    }
                }
                else
                {
                    this.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private static async Task ReadServiceLog(TCDRequest request)
        {
            request.Value = Constants.VALUE_10;
            request.ChannelID = App.CurrentChannel;

            TCDReadInfoResponse response = await UsbTcd.TCDObj.ReadServiceLogAsync(request);

            foreach (var item in response.ServicePacketList)
            {
                LogWrapper.Log(Constants.TCDLog, item.Message);
            }
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
                    if (!infoViewModelObj.BoardPartNumberList.Contains(response.Module.partNumberString))
                    {
                        infoViewModelObj.BoardPartNumberList.Add(response.Module.partNumberString);
                    }
                    infoViewModelObj.SelectedBoardPartNumber = response.Module.partNumberString;
                    if (!infoViewModelObj.BoardModelNameList.Contains(response.Module.modelString))
                    {
                        infoViewModelObj.BoardModelNameList.Add(response.Module.modelString);
                    }
                    infoViewModelObj.SelectedBoardModelName = response.Module.modelString;
                    if (!infoViewModelObj.BoardHardwareRevisionList.Contains(response.Module.hardwareRevisionString))
                    {
                        infoViewModelObj.BoardHardwareRevisionList.Add(response.Module.hardwareRevisionString);
                    }
                    infoViewModelObj.SelectedHardwareRevision = response.Module.hardwareRevisionString;
                    infoViewModelObj.BoardSerialNumber = response.Module.serialNumberString;

                    await ReadServiceLog(request);                    
                }
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.BoardInfoReadSuccessful);
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
            bool isValid = false;
            string errorMessage = string.Empty;
            try
            {
                //Validation for Board Info
                isValid = ValidateBoardInfo(out errorMessage);
                if (isValid)
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
                            arr[Constants.VALUE_3 - i] = Convert.ToByte(temp[i]);
                        }
                        request.Board.hardwareRevision = BitConverter.ToUInt32(arr, Constants.VALUE_0);
                        request.Board.serialNumberString = infoViewModelObj.BoardSerialNumber;
                        await UsbTcd.TCDObj.WriteBoardInfoAsync(request);
                    }

                    using (TCDRequest request = new TCDRequest())
                    {
                        await ReadServiceLog(request);
                    }
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.BoardInfoWriteSuccessful);
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, errorMessage);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private bool ValidateBoardInfo(out string validationMessage)
        {
            bool isValid = true;

            List<DependencyObject> objects = new List<DependencyObject>();
            objects.Add(BoardPartNumberCombo);
            objects.Add(BoardModelNameCombo);
            objects.Add(BoardHardwareRevCombo);
            objects.Add(BoardSerialNumberTextBox);

            isValid = ValidateObjects(objects, out validationMessage);

            return isValid;
            
        }

        private bool ValidateObjects(List<DependencyObject> objects, out string validationMessage)
        {
            bool isValid = true;
            string errorMessage = string.Empty;
            StringBuilder message = new StringBuilder();
            int errorCount = 0;

            foreach (DependencyObject obj in objects)
            {
                isValid = Validators.ValidationRules.ValidateControl(obj, out errorMessage);
                if (!isValid)
                {
                    errorCount++;
                    message.Append(errorMessage);
                }
            }

            validationMessage = message.ToString();
            return errorCount > 0 ? false : true;
        }

        private async void ReadChannelClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDRequest request = new TCDRequest())
                {
                    request.ChannelID = App.CurrentChannel;
                    byte channel = await UsbTcd.TCDObj.GetChannelNumber(request);
                    if(!infoViewModelObj.ChannelNumberList.Contains(channel))
                    {
                        infoViewModelObj.ChannelNumberList.Add(channel);
                    }
                    infoViewModelObj.SelectedChannelNumber = channel;

                    await ReadServiceLog(request);
                }
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ChannelReadSuccessful);
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
            string errorMessage = string.Empty;
            try
            {
                if (ValidateChannelAssignment(out errorMessage))
                {
                    using (TCDRequest request = new TCDRequest())
                    {
                        request.ChannelID = App.CurrentChannel;
                        request.Value2 = infoViewModelObj.SelectedChannelNumber;
                        await UsbTcd.TCDObj.AssignChannelAsync(request);

                        await ReadServiceLog(request);
                    }
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ChannelWriteSuccessful);
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, errorMessage);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private bool IsValid(DependencyObject dependencyObject)
        {
            DependencyObject obj = ChannelPartNumber;
            Validation.GetHasError(obj);
            Validation.GetErrors(obj);
            return !Validation.GetHasError(dependencyObject) && LogicalTreeHelper.GetChildren(dependencyObject).OfType<DependencyObject>().All(IsValid);
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

                    if (!infoViewModelObj.ProbePartNumberList.Contains(response.Probe.partNumber))
                    {
                        infoViewModelObj.ProbePartNumberList.Add(response.Probe.partNumber);
                    }
                    infoViewModelObj.SelectedProbePartNumber = response.Probe.partNumber;

                    if (!infoViewModelObj.ProbeModelNameList.Contains(response.Probe.descriptionString))
                    {
                        infoViewModelObj.ProbeModelNameList.Add(response.Probe.descriptionString);
                    }
                    infoViewModelObj.ProbeModelName = response.Probe.descriptionString;

                    if (!infoViewModelObj.ProbePhysicalIDList.Contains(response.Probe.physicalId))
                    {
                        infoViewModelObj.ProbePhysicalIDList.Add(response.Probe.physicalId);
                    }
                    infoViewModelObj.PhysicalID = response.Probe.physicalId;

                    infoViewModelObj.ProbeSerialNumber = response.Probe.serialNumberString;

                    if (!infoViewModelObj.ProbeFormatIDList.Contains(response.Probe.formatId))
                    {
                        infoViewModelObj.ProbeFormatIDList.Add(response.Probe.formatId);
                    }
                    infoViewModelObj.FormatID = response.Probe.formatId;
                    infoViewModelObj.CenterFrequency = response.Probe.centerFrequency;
                    infoViewModelObj.Diameter = response.Probe.diameter;
                    infoViewModelObj.TankFocalLength = response.Probe.tankFocalLength;
                    infoViewModelObj.InsertionLoss = response.Probe.insertionLoss;
                    infoViewModelObj.TIC = response.Probe.TIC;
                    infoViewModelObj.Fractional3dbBW = response.Probe.fractionalBW;
                    infoViewModelObj.Impedance = response.Probe.impedance;
                    infoViewModelObj.PhaseAngle = response.Probe.phaseAngle;

                    await ReadServiceLog(request);
                }              

                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ProbeInfoReadSuccessful);

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
            bool isValid = false;
            string validationMessage=string.Empty;
            try
            {
                isValid = ValidateProbeInfo(out validationMessage);
                if (isValid)
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
                    using (TCDRequest request = new TCDRequest())
                    {
                        await ReadServiceLog(request);
                    }
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ProbeInfoWriteSuccessful);
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, validationMessage);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        private bool ValidateProbeInfo(out string validationMessage)
        {
            bool isValid = true;

            List<DependencyObject> objects = new List<DependencyObject>();
            objects.Add(ProbePartNumberCombo);
            objects.Add(ProbeModelName);
            objects.Add(ProbeSerialNumberTextBox);
            objects.Add(PhysicalID);
            objects.Add(FormatID);
            objects.Add(CenterFrequencyTextBox);
            objects.Add(DiameterTextBox);
            objects.Add(TankFocalTextBox);
            objects.Add(InsertionLossTextBox);
            objects.Add(TICTextBox);
            objects.Add(FractionalTextBox);
            objects.Add(ImpedenceTextBox);
            objects.Add(PhaseAngleTextBox);

            isValid = ValidateObjects(objects, out validationMessage);
            return isValid;
        }

        private bool ValidateChannelAssignment(out string errorMessage)
        {
            bool isValid = true;
            List<DependencyObject> objects = new List<DependencyObject>();
            objects.Add(ChannelPartNumber);

            isValid = ValidateObjects(objects, out errorMessage);
            return isValid;
        }

        private async void ReadOperatingMinutesClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                using (TCDRequest requestObj = new TCDRequest())
                {
                    requestObj.ChannelID = App.CurrentChannel;
                    infoViewModelObj.OperatingMinutes = (uint)(await UsbTcd.TCDObj.ReadOperatingMinutesAsync(requestObj)).Value;

                    await ReadServiceLog(requestObj);
                }
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.OperatingMinutesReadSuccessful);
            }
            catch(Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }
    }
}
