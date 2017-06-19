using Core.Common;
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
        bool isSafetyCalibrationInProgress;

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
            await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_1});

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
                if (PowerController.Instance.IsControllerOn)
                {
                    this.IsEnabled = true;
                    if (App.CurrentChannel == TCDHandles.Channel1 || App.CurrentChannel == TCDHandles.Channel2)
                    {
                        TCDReadInfoResponse readInfo = await UsbTcd.TCDObj.ReadCalibrationInfoAsync(
                            new UsbTcdLibrary.CommunicationProtocol.TCDRequest() { ChannelID = App.CurrentChannel });
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
                else
                {
                    this.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
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
            catch (Exception ex)
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

            if ((await UsbTcd.TCDObj.StartMeasurementOfBoardAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value3 =  Constants.VALUE_1 })).Result)
            {
                calViewModel.IsMeasurement1EditEnabled = true;
                calViewModel.Measurement1 = Constants.VALUE_0;
                calViewModel.Measurement2 = Constants.VALUE_0;
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

            if ((await UsbTcd.TCDObj.StartMeasurementOfBoardAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value3 = Constants.VALUE_2 })).Result)
            {
                calViewModel.IsMeasurement2EditEnabled = true;
                if (calViewModel.Measurement2 > Constants.VALUE_0)
                {
                    calViewModel.IsApplyMeasurementEnabled = true;
                }
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, "Could not start calibration.");
            }
        }

        private async void ApplyMeasurementsClick(object sender, RoutedEventArgs e)
        {
            bool m1Valid, m2Valid;
            double m1AdjustmentFactor, m2AdjustmentFactor;
            TCDRequest requestObj = new TCDRequest();
            requestObj.ChannelID = App.CurrentChannel;
            // Make sure that if a safety calibration is in progress it is stopped
            SafetyStopClick(null, null);

            m1AdjustmentFactor = Constants.M1AdjustmentFactor;
            m2AdjustmentFactor = Constants.M2AdjustmentFactor;

            m1Valid = false;
            if (calViewModel.Measurement1 > Constants.VALUE_0)
            {
                requestObj.Value = (int)(m1AdjustmentFactor * calViewModel.Measurement1);
                m1Valid = true;
            }
            if (!m1Valid)
            {
                LogWrapper.Log(Constants.APPLog, "Invalid value for measurement #1.");
            }

            m2Valid = false;
            if (calViewModel.Measurement2 > Constants.VALUE_0)
            {
                requestObj.Value = (int)(m2AdjustmentFactor * calViewModel.Measurement2);
                m2Valid = true;
            }
            if (!m2Valid)
            {
                LogWrapper.Log(Constants.APPLog, "Invalid value for measurement #2.");
            }

            if (m1Valid & m2Valid)
            {
                if ((await UsbTcd.TCDObj.ApplyMeasurementToBoardAsync(requestObj)).Result)
                {
                    calViewModel.IsMeasurement1EditEnabled = false;
                    calViewModel.IsMeasurement2EditEnabled = false;
                    calViewModel.IsApplyMeasurementEnabled = false;
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, "Could not apply calibration.");
                }
            }
        }

        private async void ConsistencyCheckStartClick(object sender, RoutedEventArgs e)
        {
            const int CONSISTENCY_CHECK_DAC_DA1085 = 1705;
            const int CONSISTENCY_CHECK_PRF = 8000;
            const int CONSISTENCY_CHECK_SAMPLE_LENGTH = 9;


            if(!await StartFixedTransmit(CONSISTENCY_CHECK_DAC_DA1085, CONSISTENCY_CHECK_PRF, CONSISTENCY_CHECK_SAMPLE_LENGTH))
            {
                LogWrapper.Log(Constants.APPLog, "Unable to start transmit.");
            }
        }

        private async Task<bool> StartFixedTransmit(int DAC, int PRF, int sampleLength)
        {
            const int TX_CYCLES_PER_SECOND = 2000000; // 2MHz transmit carrier
            await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Service);
            // Start by putting the FPGA into reset to clear any previous state.
            await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 1 });

            // Pull the FPGA out of reset.  Note that this calls AcqHW_Sleep in the
            // Doppler module software, which also initializes the sample clock and
            // transmit clock for a 2MHz carrier.
            if (!(await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 0 })).Result)
            {
                return false;
            }

            // Set as self-master because we don't want to depend on another
            // module or have another module depend on us.
            if (!(await UsbTcd.TCDObj.WriteValueToFPGAAsync
                (new TCDRequest() { ChannelID = App.CurrentChannel, Value3 = Constants.INTERMODULE_ADDRESS, Value = Constants.PRIORITY_SELF_MASTER })).Result)
            {
                await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 1 });
                return false;
            }

            // *** Calculate the transmit control register *** //
            // Convert PRF from pulse per second to tx cycles per pulse
            int txCyclesPerPRF = TX_CYCLES_PER_SECOND / PRF;
            // See module software for explanation of the conversion from mm to cycles
            var txBurstCycles = ((Constants.VALUE_24 * sampleLength) / Constants.VALUE_9) + Constants.VALUE_1;
            // See module software for explanation of the transmit register format
            if (txCyclesPerPRF == (Constants.VALUE_1 << Constants.VALUE_12) && txBurstCycles == (Constants.VALUE_1 << Constants.VALUE_8))
            {
                int txRegister = (txBurstCycles << Constants.VALUE_24) + (txCyclesPerPRF << Constants.VALUE_12) + DAC;

                // Set the transmit control register
                if (!(await UsbTcd.TCDObj.WriteValueToFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value3 = Constants.TX_PULSE_ADDRESS, Value = txRegister })).Result)
                {
                    await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_1 });
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void ConsistencyCheckStopClick(object sender, RoutedEventArgs e)
        {
            await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_1 });
        }

        private async void SafetyStartClick(object sender, RoutedEventArgs e)
        {
            // Make sure we're not in the middle of acoustic calibration
            ResetMeasurements();

            // Check for probe
            ClearSafetyStatus();
            if((await UsbTcd.TCDObj.GetProbeInfo(new TCDRequest() { ChannelID = App.CurrentChannel })).Probe==null)
            {
                return;
            }
            BtnSafetyStart.IsEnabled = false;
            isSafetyCalibrationInProgress = true;
            await UsbTcd.TCDObj.EnableTransmitTestControlAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
            await Task.Delay(Constants.VALUE_100);
            await UsbTcd.TCDObj.TransmitTestPowerAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = calViewModel.Power });
            await UsbTcd.TCDObj.TransmitTestSampleLengthAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = (int)calViewModel.SelectedSVOL });
            await UsbTcd.TCDObj.TransmitTestPRFAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = (int)calViewModel.SelectedPRF });

            // Warn the user if they leave tab without performing safety verification,
            // but only if the module is actually capable of performing verification.
        }

        private async void SafetyStopClick(object sender, RoutedEventArgs e)
        {
            if(isSafetyCalibrationInProgress)
            {
                isSafetyCalibrationInProgress = false;
                BtnSafetyStart.IsEnabled = true;
                await UsbTcd.TCDObj.EnableTransmitTestControlAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
            }
            ClearSafetyStatus();
        }
        
    }
}
