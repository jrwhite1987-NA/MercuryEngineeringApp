using Core.Constants;
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

namespace MercuryEngApp.Views
{
    /// <summary>
    /// Interaction logic for FPGAUserControl.xaml
    /// </summary>
    public partial class FPGAUserControl : UserControl
    {
        //Constants
        const int ADC_FREQ_ADDRESS = 0x020;
        const int INTERMODULE_ADDRESS = 0x080;
        const int RX_GAIN_ADDRESS = 0x010;
        const int TX_FREQ_ADDRESS = 0x008;
        const int TX_PULSE_ADDRESS = 0x004;
        const int ADC_FREQ_DEFAULT = 1; // 48MHz ADC clock
        const int PRIORITY_SELF_MASTER = 0;
        const int INTERMODULE_DEFAULT = PRIORITY_SELF_MASTER;
        const int RX_GAIN_DEFAULT = 0x199;
        const int TX_FREQ_DEFAULT = 24;  // 48 div 24 = 2MHz
        const int TX_PULSE_DEFAULT = 0x100FA400;  // 16 cycle sample, 250 cycle PRF (8KHz)      
        FPGAViewModel fpgaViewModel = new FPGAViewModel();

        /// <summary>
        /// Constructor
        /// </summary>
        public FPGAUserControl()
        {
            InitializeComponent();
            this.Loaded += FPGAUserControlLoaded;
            this.DataContext = fpgaViewModel;
        }

        /// <summary>
        /// Screen Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FPGAUserControlLoaded(object sender, RoutedEventArgs e)
        {
            txtUserGuide.Text = fpgaViewModel.GetFPGAUserGuideContent();
            if(PowerController.Instance.IsControllerOn)
            {
                this.IsEnabled = true;
            }
            else
            {
                this.IsEnabled = false;
            }
        }

        /// <summary>
        /// Start Fixed transmit 
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="DAC"></param>
        /// <param name="PRF"></param>
        /// <param name="sampleLen"></param>
        /// <returns></returns>
        public async Task<bool> StartFixedTransmit(int channel, int DAC, int PRF, int sampleLen)
        {

            const int TX_CYCLES_PER_SECOND = 2000000; // 2MHz transmit carrier
            int txCyclesPerPRF;
            int txBurstCycles;
            int txRegister;

            await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Service);

            // Start by putting the FPGA into reset to clear any previous state.
            await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_1 });
            // Pull the FPGA out of reset.  Note that this calls AcqHW_Sleep in the
            // Doppler module software, which also initializes the sample clock and
            // transmit clock for a 2MHz carrier.
            TCDResponse response = await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_0 });

            if (!response.Result)
            {
                return false;
            }


            // Set as self-master because we don't want to depend on another
            // module or have another module depend on us.
            if (!(await UsbTcd.TCDObj.WriteValueToFPGAAsync
                (new TCDRequest() { ChannelID = App.CurrentChannel, Value3 = Constants.INTERMODULE_ADDRESS, Value = Constants.PRIORITY_SELF_MASTER })).Result)
            {
                await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_1 });
                return false;
            }


            // *** Calculate the transmit control register *** //
            // Convert PRF from pulse per second to tx cycles per pulse
            txCyclesPerPRF = TX_CYCLES_PER_SECOND / PRF;
            // See module software for explanation of the conversion from mm to cycles
            txBurstCycles = ((Constants.VALUE_24 * sampleLen) / Constants.VALUE_9) + Constants.VALUE_1;

            // See module software for explanation of the transmit register format
            if (txCyclesPerPRF == (Constants.VALUE_1 << Constants.VALUE_12) && txBurstCycles == (Constants.VALUE_1 << Constants.VALUE_8) && DAC == (Constants.VALUE_1 << Constants.VALUE_12))
            {
                txRegister = (txBurstCycles << Constants.VALUE_24) + (txCyclesPerPRF << Constants.VALUE_12) + DAC;

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

        /// <summary>
        /// Stpo the fixed transmit
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task<bool> StopFixedTransmit(int channel)
        {
            return (await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_1 })).Result;
        }

        /// <summary>
        /// Read the register on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ReadRegisterClick(object sender, RoutedEventArgs e)
        {
            uint address = Convert.ToUInt32(fpgaViewModel.SelectedRegister.MemoryLocation, Constants.VALUE_16);
            TCDResponse response = await UsbTcd.TCDObj.ReadFPGAValueAsync(new TCDRequest { ChannelID = App.CurrentChannel, Value3 = address });
            if(response.Result)
            {
                fpgaViewModel.SelectedRegister.Value = (int)response.Value;
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGAReadSuccess);               
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGAReadFailure);  
            }
        }

        /// <summary>
        /// write the register on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WriteRegisterClick(object sender, RoutedEventArgs e)
        {
            TCDResponse response = await UsbTcd.TCDObj.WriteValueToFPGAAsync(new TCDRequest { ChannelID = App.CurrentChannel, Value = fpgaViewModel.SelectedRegister.Value, Value3 = Convert.ToUInt32(fpgaViewModel.SelectedRegister.MemoryLocation) });
            if (response.Result)
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGAWriteSuccess);               
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGAWriteFailure);               
            }
        }

        /// <summary>
        /// Reset the register
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ResetRegisterClick(object sender, RoutedEventArgs e)
        {
            TCDResponse response = await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = Constants.VALUE_1 });

            if (response.Result)
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGAResetSuccess);
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGAResetFailure);
            }
        }

        /// <summary>
        /// set all defaults
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SetAllDefaultsClick(object sender, RoutedEventArgs e)
        {
            TCDResponse response;
            var isSuccess = true;
            foreach (var register in fpgaViewModel.FPGARegisterList)
            {
                response = await UsbTcd.TCDObj.WriteValueToFPGAAsync(new TCDRequest { ChannelID = App.CurrentChannel, Value = Convert.ToInt32(register.DefaultValue), Value3 = Convert.ToUInt32(register.MemoryLocation) });
                if (!response.Result)
                {
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGASetDefaultFailure + " " + register.Name );
                    isSuccess = false;
                }
            }

            if (isSuccess)
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FPGASetAllDefaultSuccess);
            }
        }
    }
}
