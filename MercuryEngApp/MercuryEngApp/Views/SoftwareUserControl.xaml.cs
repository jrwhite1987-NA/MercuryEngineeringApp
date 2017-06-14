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
using MercuryEngApp.Common;
using UsbTcdLibrary.CommunicationProtocol;
using MercuryEngApp.ViewModels;
using Core.Constants;
using UsbTcdLibrary;
using System.IO;
using UsbTcdLibrary.StatusClasses;

namespace MercuryEngApp.Views
{
    /// <summary>
    /// Interaction logic for SoftwareUserControl.xaml
    /// </summary>
    public partial class SoftwareUserControl : UserControl
    {
        SoftwareViewModel softwareViewModel;
        public SoftwareUserControl()
        {
            softwareViewModel = new SoftwareViewModel();
            InitializeComponent();
            this.Loaded += SoftwareUserControlLoaded;
            this.DataContext = softwareViewModel;
        }

        void SoftwareUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Opens up a browse dialog and stores the result in the edit field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".upd";
            dlg.Filter = "UPD File (.upd)|*.upd";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result.Value)
            {
                // Open document
                string filename = dlg.FileName;
                FileNameTextBox.Text = filename;
            }
        }

        /// <summary>
        /// Updates the display
        /// </summary>
        private async void Refresh()
        {
            TCDReadInfoResponse response = await UsbTcd.TCDObj.GetModuleInfo(new TCDRequest() { ChannelID = App.CurrentChannel });
            softwareViewModel.SoftwareVersion = response.Module != null ? response.Module.dopplerSWRevisionString : string.Empty;
            softwareViewModel.BootVersion = response.Module != null ? response.Module.bootSWRevisionString : string.Empty;
            softwareViewModel.HardwareRevision = response.Module != null ? response.Module.hardwareRevisionString : string.Empty;

            // Enable / disable
            softwareViewModel.IsPerformUpdateEnabled = response.Module != null;
            softwareViewModel.IsBrowseEnabled = response.Module != null;
            softwareViewModel.IsAbortEnabled = false;
            softwareViewModel.ShowWarningLabels = false;

            if (response.Module == null)
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.TCDNotConnected);
            }
        }

        /// <summary>
        /// Run the update process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PerformUpdateClick(object sender, RoutedEventArgs e)
        {
            pbStatus.Value = Constants.VALUE_0;
            // Check that the file exists
            if (File.Exists(FileNameTextBox.Text))
            {
                ExecuteUpdate();
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FileNotFound);
            }
        }

        /// <summary>
        /// Abort the update process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AbortUpdateClick(object sender, RoutedEventArgs e)
        {
            if (await AbortUpdate())
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Aborted);
            }
            else
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.AbortFailed);
            }
        }

        private async Task<bool> AbortUpdate()
        {
            TCDResponse response = await UsbTcd.TCDObj.EndUpdateProcessAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
            if (response != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void ExecuteUpdate()
        {
            TCDReadInfoResponse response = await UsbTcd.TCDObj.GetModuleInfo(new TCDRequest() { ChannelID = App.CurrentChannel });
            if (response.Module == null)
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.StartUpdateFailed);
                return;
            }
            else
            {
                if (await InitModuleUpdate(App.CurrentChannel))
                {
                    softwareViewModel.IsPerformUpdateEnabled = false;
                    softwareViewModel.IsAbortEnabled = true;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.Sending;

                    //Show warning labels
                    pbStatus.Visibility = Visibility.Visible;
                    StatusGridRow.Visibility=Visibility.Visible;
                    softwareViewModel.ShowWarningLabels = true;
                }
                else
                {
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.StartUpdateFailed;
                    return;
                }

                await LoadModuleUpdate(App.CurrentChannel, FileNameTextBox.Text);
                softwareViewModel.ShowWarningLabels = false;
            }
        }

        private async Task<bool> LoadModuleUpdate(TCDHandles tCDHandles, string fileName)
        {
            bool result = false;
            bool updateFinished = false;
            bool updateAborted = false;
            int start = 0;
            int blockSize = 0;
            
            const int UPDATE_PROCESS_TIMEOUT = 30000; //30 seconds
            byte[] updateLoadBlock = new byte[DMIProtocol.UPDATE_LOAD_BLOCK_SIZE];
            UpdateProgress progress = new UpdateProgress();

            if (!File.Exists(fileName))
            {
                return false;
            }

            TCDResponse response = await UsbTcd.TCDObj.StartUpdateProcessAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
            if (response.Value == Constants.VALUE_0)
            {
                await UsbTcd.TCDObj.EndUpdateProcessAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
                return false;
            }

            start = Environment.TickCount;
            using (FileStream fs = File.OpenRead(fileName))
            {
                //Read till the file is completed or update is finished.
                blockSize = fs.Read(updateLoadBlock, Constants.VALUE_0, DMIProtocol.UPDATE_LOAD_BLOCK_SIZE);
                while (blockSize > Constants.VALUE_0 || updateFinished)
                {
                    //Write method to write data
                    TCDResponse writeResponse = await UsbTcd.TCDObj.WriteData(new TCDWriteInfoRequest() { ChannelID = App.CurrentChannel, UpdateData = updateLoadBlock });

                    if (!writeResponse.Result)
                    {
                        pbStatus.Value = Constants.VALUE_0;
                        softwareViewModel.UpdateStatus = MercuryEngApp.Resources.UpdateError;
                        return false;
                    }

                    TCDReadInfoResponse progressResponse = await UsbTcd.TCDObj.GetUpdateProgressAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
                    UpdateStatusCode currentStatus;

                    if (progressResponse.UpdateProgress == null)
                    {
                        pbStatus.Value = Constants.VALUE_0;
                        softwareViewModel.UpdateStatus = MercuryEngApp.Resources.UpdateError;
                        return false;
                    }

                    progress = progressResponse.UpdateProgress;
                    currentStatus = progress.StatusCode;

                    UpdateProgressBar(progress, fs.Length);

                    if (currentStatus != UpdateStatusCode.Ready || currentStatus != UpdateStatusCode.Receiving ||
                        currentStatus != UpdateStatusCode.Verifying || currentStatus != UpdateStatusCode.Writing || currentStatus != UpdateStatusCode.Confirming)
                    {
                        updateFinished = true;
                    }

                    if ((Environment.TickCount - start) > UPDATE_PROCESS_TIMEOUT)
                    {
                        updateFinished = true;
                    }

                    if (updateAborted)
                    {
                        updateFinished = true;
                    }

                    await Task.Delay(Constants.VALUE_10);
                }
            }

            if (progress.StatusCode != UpdateStatusCode.Timeout)
            {
                //Progres Call Back.
            }

            await UsbTcd.TCDObj.EndUpdateProcessAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
            if (progress.StatusCode == UpdateStatusCode.Finished)
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// Updates the Progress bar and display the status in App log
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="fileSize"></param>
        private void UpdateProgressBar(UpdateProgress progress, long fileSize)
        {
            softwareViewModel.BytesReceivedErased = progress.BytesReceivedErased;
            softwareViewModel.BytesWritten = progress.BytesWritten;

            switch (progress.StatusCode)
            {
                case UpdateStatusCode.Ready:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.Sending;
                    break;
                case UpdateStatusCode.Receiving:
                    pbStatus.Value = Math.Ceiling((Convert.ToDouble(progress.BytesReceivedErased / fileSize)) * Constants.VALUE_50);
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.Sending;
                    break;
                case UpdateStatusCode.Verifying:
                    pbStatus.Value = Constants.VALUE_55;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.Verifying;
                    softwareViewModel.IsAbortEnabled = false;
                    break;
                case UpdateStatusCode.Writing:
                    pbStatus.Value = Constants.VALUE_60 + Math.Ceiling((Convert.ToDouble(progress.BytesReceivedErased / fileSize)) * Constants.VALUE_15) + Math.Ceiling((Convert.ToDouble(progress.BytesWritten / fileSize)) * Constants.VALUE_15);
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.Writing;
                    break;
                case UpdateStatusCode.Confirming:
                    pbStatus.Value = Constants.VALUE_95;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.Confirming;
                    break;
                case UpdateStatusCode.Finished:
                    pbStatus.Value = Constants.VALUE_100;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.Finished;
                    break;
                case UpdateStatusCode.ReceivingFailure:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.UpdateError;
                    break;
                case UpdateStatusCode.ChecksumFailure:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.CheckSumFailure;
                    break;
                case UpdateStatusCode.IncompatibleVersion:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.IncompatVersion;
                    break;
                case UpdateStatusCode.TableInvalid:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.InvalidTable;
                    break;
                case UpdateStatusCode.AddressInvalid:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.InvalidAddress;
                    break;
                case UpdateStatusCode.EraseFailure:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.EraseFail;
                    break;
                case UpdateStatusCode.WriteFailure:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.WriteFail;
                    break;
                case UpdateStatusCode.ComparisionFailure:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.CompareFail;
                    break;
                case UpdateStatusCode.ReadFailure:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.ReadFail;
                    break;
                case UpdateStatusCode.Timeout:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.TimeOut;
                    break;
                case UpdateStatusCode.Aborted:
                    pbStatus.Value = Constants.VALUE_0;
                    softwareViewModel.UpdateStatus = MercuryEngApp.Resources.HostAbort;
                    break;
            }
        }

        private async Task<bool> InitModuleUpdate(TCDHandles currentChannel)
        {
            //Command the module to update mode and wait for it to get there
            TCDModes currentMode = UsbTcd.TCDObj.GetMode(currentChannel);
            bool result;
            if (currentMode != TCDModes.Update)
            {
                // Switch to update mode
                result = await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Update);
            }
            else
            {
                result = false;
            }
            return result;
        }
    }

    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool ShowWarningLabels = (bool)value;
            return ShowWarningLabels ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}