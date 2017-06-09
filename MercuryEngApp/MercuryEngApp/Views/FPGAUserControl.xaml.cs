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

        public FPGAUserControl()
        {
            InitializeComponent();
            this.DataContext = fpgaViewModel;
        }


        public void Activate()
        {

        }

        public void Deactivate()
        {

        }

        public void Refresh()
        {

        }

        public async Task<bool> StartFixedTransmit(int channel, int DAC, int PRF, int sampleLen)
        {

            const int TX_CYCLES_PER_SECOND = 2000000; // 2MHz transmit carrier
            int txCyclesPerPRF;
            int txBurstCycles;
            int txRegister;

            await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Service);

            // Start by putting the FPGA into reset to clear any previous state.
            await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 1 });
            // Pull the FPGA out of reset.  Note that this calls AcqHW_Sleep in the
            // Doppler module software, which also initializes the sample clock and
            // transmit clock for a 2MHz carrier.
            TCDResponse response = await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 0 });

            if (!response.Result) return false;


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
            txCyclesPerPRF = TX_CYCLES_PER_SECOND / PRF;
            // See module software for explanation of the conversion from mm to cycles
            txBurstCycles = ((24 * sampleLen) / 9) + 1;

            // See module software for explanation of the transmit register format
            if (txCyclesPerPRF == (1 << 12) && txBurstCycles == (1 << 8) && DAC == (1<<12))
            {
                 txRegister = (txBurstCycles << 24) + (txCyclesPerPRF << 12) + DAC;

                // Set the transmit control register
                if (!(await UsbTcd.TCDObj.WriteValueToFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value3 = Constants.TX_PULSE_ADDRESS, Value = txRegister })).Result)
                {
                    await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 1 });
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }              
        }

        public async Task<bool> StopFixedTransmit(int channel)
        {
            return (await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 1 })).Result;
        }
        int i = 2;
        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            uint address =  Convert.ToUInt32(fpgaViewModel.SelectedRegister.MemoryLocation,16);
            TCDResponse response = await UsbTcd.TCDObj.ReadFPGAValueAsync(new TCDRequest { ChannelID = App.CurrentChannel, Value3 = address });
            if(response.Result)
            {
                fpgaViewModel.SelectedRegister.Value = (int)response.Value;
            }
            else
            {
                App.ApplicationLog = "Error at read FPGA";
            }
        }

        private async void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            TCDResponse response = await UsbTcd.TCDObj.WriteValueToFPGAAsync(new TCDRequest { ChannelID = App.CurrentChannel, Value = fpgaViewModel.SelectedRegister.Value, Value3 = Convert.ToUInt32(fpgaViewModel.SelectedRegister.MemoryLocation) });
            if (!response.Result)
            {
                App.ApplicationLog = "Error at write FPGA";
            }
        }

        private async void btnReset_Click(object sender, RoutedEventArgs e)
        {
            TCDResponse response =  await UsbTcd.TCDObj.ResetFPGAAsync(new TCDRequest() { ChannelID = App.CurrentChannel, Value = 1 });
            
            if(!response.Result)
            {
                App.ApplicationLog = "Error at reset FPGA";
            }
        }

        private async void btnSetAllDefault_Click(object sender, RoutedEventArgs e)
        {
            TCDResponse response;
            foreach (var register in fpgaViewModel.FPGARegisterList)
            {
                response = await UsbTcd.TCDObj.WriteValueToFPGAAsync(new TCDRequest { ChannelID = App.CurrentChannel, Value = Convert.ToInt32(register.DefaultValue), Value3 = Convert.ToUInt32(register.MemoryLocation) });
                if (!response.Result)
                {
                    App.ApplicationLog = "Error at write FPGA for register "+ register.Name;
                }
            }
        }
    }
}
