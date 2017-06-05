using Core.Constants;
using MercuryEngApp.Common;
using MercuryEngApp.ViewModels;
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
    /// Interaction logic for CalibrationTabUserControl.xaml
    /// </summary>
    public partial class CalibrationUserControl : UserControl
    {
        CalibrationViewModel calViewModel;
        int lastSafetyTrip;

        public CalibrationUserControl()
        {
            calViewModel = new CalibrationViewModel();
            InitializeComponent();
            this.Loaded += CalibrationUserControlLoaded;
            this.Unloaded += CalibrationUserControlUnloaded;
            this.DataContext = calViewModel;
        }

        async void CalibrationUserControlUnloaded(object sender, RoutedEventArgs e)
        {
            // Make sure FPGA is in reset
            await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel });

            // Make sure that if a safety calibration is in progress it is stopped
            SafetyStopClick(null, null);
        }

        void ClearSafetyStatus()
        {
            if (App.CurrentChannel == TCDHandles.Channel1 || App.CurrentChannel == TCDHandles.Channel2)
            {
                calViewModel.SafetyTripStatus = "OK";
            }
            else
            {
                calViewModel.SafetyTripStatus = "No Probe";
            }

        }

        async void CalibrationUserControlLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.CurrentChannel == TCDHandles.Channel1 || App.CurrentChannel == TCDHandles.Channel2)
                {
                    TCDReadInfoResponse readInfo = await UsbTcd.TCDObj.ReadCalibrationInfoAsync(new UsbTcdLibrary.CommunicationProtocol.TCDRequest() { ChannelID = App.CurrentChannel });
                    if (readInfo.Calibration != null)
                    {
                        calViewModel.TxOffset = readInfo.Calibration.ZeroIntensityDAC;
                        calViewModel.TxEnergy = readInfo.Calibration.MaxDACIntensity;
                        OverrideCalibration.IsChecked = true;
                    }
                    else
                    {
                        LogWrapper.Log(Constants.APPLog, "Failed to retrieve calibration data");
                    }
                }
                ClearSafetyStatus();
                lastSafetyTrip = 0;
            }
            catch(Exception ex)
            {

            }
        }

        private void OverrideCalClick(object sender, RoutedEventArgs e)
        {
            //Change the edits
            calViewModel.IsTxEnergyEnabled = (bool)OverrideCalibration.IsChecked;
            calViewModel.IsTxOffsetEnabled = (bool)OverrideCalibration.IsChecked;

            //Enable/Disable write
            calViewModel.IsWriteCalEnabled = (bool)OverrideCalibration.IsChecked;
        }

        private async void OverrideCalibrationClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TCDWriteInfoRequest writeInfo = new TCDWriteInfoRequest())
                {
                    writeInfo.ChannelID = App.CurrentChannel;
                    writeInfo.Calibration = new UsbTcdLibrary.StatusClasses.CalibrationInfo();
                    writeInfo.Calibration.MaxDACIntensity = calViewModel.TxEnergy;
                    writeInfo.Calibration.ZeroIntensityDAC = calViewModel.TxOffset;
                    await UsbTcd.TCDObj.CalibrateBoardAsync(writeInfo);
                    LogWrapper.Log(Constants.APPLog, "Calibration override successful.");
                }
            }
            catch(Exception ex)
            {
                LogWrapper.Log(Constants.APPLog, "Calibration override failed.");
            }
        }

        private void ResetMeasurements()
        {
            calViewModel.Measurement1 = 0;
            calViewModel.Measurement2 = 0;
            calViewModel.IsMeasurement2EditEnabled = false;
            calViewModel.IsMeasurement1EditEnabled = false;
            calViewModel.IsApplyMeasurementEnabled = false;
            calViewModel.IsMeasurement2ClickEnabled = false;
        }

        private async void StartMeasurement1Click(object sender, RoutedEventArgs e)
        {
            // Make sure that if a safety calibration is in progress it is stopped
            SafetyStopClick(null, null);

            if((await UsbTcd.TCDObj.StartMeasurementOfBoardAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value3 = 1 })).Result)
            {
                calViewModel.IsMeasurement1EditEnabled = true;
                calViewModel.Measurement1 = 0;
                calViewModel.Measurement2 = 0;
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, "Could not start calibration.");
            }
        }

        private async void StartMeasurement2Click(object sender, RoutedEventArgs e)
        {
            // Make sure that if a safety calibration is in progress it is stopped
            SafetyStopClick(null, null);

            if ((await UsbTcd.TCDObj.StartMeasurementOfBoardAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value3 = 2 })).Result)
            {
                calViewModel.IsMeasurement2EditEnabled = true;
                if(calViewModel.Measurement2>0)
                {
                    calViewModel.IsApplyMeasurementEnabled = true;
                }
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, "Could not start calibration.");
            }
        }

        private void ApplyMeasurementsClick(object sender, RoutedEventArgs e)
        {
            // Make sure that if a safety calibration is in progress it is stopped
            SafetyStopClick(null, null);
        }

        private void ConsistencyCheckStartClick(object sender, RoutedEventArgs e)
        {

        }

        private void ConsistencyCheckStopClick(object sender, RoutedEventArgs e)
        {

        }

        private void SafetyStartClick(object sender, RoutedEventArgs e)
        {

        }

        private void SafetyStopClick(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
