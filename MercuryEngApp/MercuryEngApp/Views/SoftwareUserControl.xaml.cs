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
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                FileNameTextBox.Text = filename;

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(System.IO.File.ReadAllText(filename));
                FlowDocument document = new FlowDocument(paragraph);
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
        }

        /// <summary>
        /// Run the update process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PerformUpdate_Click(object sender, RoutedEventArgs e)
        {
            pbStatus.Value = 0;
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
        private async void AbortUpdate_Click(object sender, RoutedEventArgs e)
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
            string serial = string.Empty;
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
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Sending);

                    //Show warning labels - Pending
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.StartUpdateFailed);
                    return;
                }

                await LoadModuleUpdate(App.CurrentChannel, FileNameTextBox.Text);
            }
        }

        private async Task<bool> LoadModuleUpdate(TCDHandles tCDHandles, string fileName)
        {
            bool result = false;
            bool updateFinished = false;
            bool updateAborted = false;
            int start = 0;
            int blockSize = 0;
            const int UPDATE_LOAD_BLOCK_SIZE = 512*2;
            const int UPDATE_PROCESS_TIMEOUT = 30000; //30 seconds
            byte[] updateLoadBlock = new byte[UPDATE_LOAD_BLOCK_SIZE];
            UpdateProgress progress = new UpdateProgress();

            if (!File.Exists(fileName))
            {
                return false;
            }

            TCDResponse response = await UsbTcd.TCDObj.StartUpdateProcessAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
            if (response == null)
            {
                await UsbTcd.TCDObj.EndUpdateProcessAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
                return false;
            }

            start = Environment.TickCount;
            using (FileStream fs = File.OpenRead(fileName))
            {
                //Read till the file is completed or update is finished.
                while (((blockSize = fs.Read(updateLoadBlock, 0, UPDATE_LOAD_BLOCK_SIZE)) > 0) || updateFinished == true)
                {
                    //Write method to write data - pending


                    TCDReadInfoResponse progressResponse = await UsbTcd.TCDObj.GetUpdateProgressAsync(new TCDRequest() { ChannelID = App.CurrentChannel });
                    UpdateStatusCode currentStatus;
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

                    await Task.Delay(10);
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
            LogWrapper.Log(Constants.APPLog, string.Format(MercuryEngApp.Resources.BytesReceived, progress.BytesWritten.ToString()));
            LogWrapper.Log(Constants.APPLog, string.Format(MercuryEngApp.Resources.BytesErased, progress.BytesReceivedErased.ToString()));
            LogWrapper.Log(Constants.APPLog, string.Format(MercuryEngApp.Resources.Status, progress.StatusCode.ToString()));

            switch (progress.StatusCode)
            {
                case UpdateStatusCode.Ready:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Sending);
                    break;
                case UpdateStatusCode.Receiving:
                    pbStatus.Value = Math.Ceiling((Convert.ToDouble(progress.BytesReceivedErased / fileSize)) * 50);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Sending);
                    break;
                case UpdateStatusCode.Verifying:
                    pbStatus.Value = 55;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Verifying);
                    softwareViewModel.IsAbortEnabled = false;
                    break;
                case UpdateStatusCode.Writing:
                    pbStatus.Value = 60 + Math.Ceiling((Convert.ToDouble(progress.BytesReceivedErased / fileSize)) * 15) + Math.Ceiling((Convert.ToDouble(progress.BytesWritten / fileSize)) * 15);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Writing);
                    break;
                case UpdateStatusCode.Confirming:
                    pbStatus.Value = 95;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Confirming);
                    break;
                case UpdateStatusCode.Finished:
                    pbStatus.Value = 100;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.Finished);
                    break;
                case UpdateStatusCode.ReceivingFailure:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.UpdateError);
                    break;
                case UpdateStatusCode.ChecksumFailure:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.CheckSumFailure);
                    break;
                case UpdateStatusCode.IncompatibleVersion:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.IncompatVersion);
                    break;
                case UpdateStatusCode.TableInvalid:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.InvalidTable);
                    break;
                case UpdateStatusCode.AddressInvalid:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.InvalidAddress);
                    break;
                case UpdateStatusCode.EraseFailure:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.EraseFail);
                    break;
                case UpdateStatusCode.WriteFailure:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.WriteFail);
                    break;
                case UpdateStatusCode.ComparisionFailure:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.CompareFail);
                    break;
                case UpdateStatusCode.ReadFailure:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ReadFail);
                    break;
                case UpdateStatusCode.Timeout:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.TimeOut);
                    break;
                case UpdateStatusCode.Aborted:
                    pbStatus.Value = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.HostAbort);
                    break;
            }
        }

        private async Task<bool> InitModuleUpdate(TCDHandles currentChannel)
        {
            //Write method to get the current mode.
            TCDModes currentMode = TCDModes.Update;
            bool result;
            if (currentMode != TCDModes.Update)
            {
                result = await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Update);
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}