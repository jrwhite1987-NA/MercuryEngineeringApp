using Core.Constants;
using Core.Models.ReportModels;
using MercuryEngApp.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using UsbTcdLibrary.PacketFormats;
using log4net;
using MercuryEngApp.Views;
using System.Windows.Controls;
using Core.Common;


namespace MercuryEngApp
{
    public delegate void TCDPower();
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Event for TCD On
        /// </summary>
        public static event TCDPower TurnTCDON;
        /// <summary>
        /// Event for TCD Off
        /// </summary>
        public static event TCDPower TurnTCDOFF;
        public int previousIndex = 0;
        public int LogSelectedTabIndex = 0;
        public MainViewModel mainViewModel = new MainViewModel();

        /// <summary>
        /// Gets or sets the Is Power CHecked
        /// </summary>
        internal bool? IsPowerChecked
        {
            get
            {
                return BtnPower.IsChecked;
            }
            set
            {
                BtnPower.IsChecked = value;
            }
        }

        /// <summary>
        /// Gets or sets the Probe 1 Hit test visible
        /// </summary>
        internal bool IsProbe1HitTestVisible
        {
            get
            {
                return BtnLeftProbe.IsHitTestVisible;
            }
            set
            {
                BtnLeftProbe.IsHitTestVisible = value;
            }
        }
        /// <summary>
        /// Gets or sets the Probe 2 Hit test visible
        /// </summary>
        internal bool IsProbe2HitTestVisible
        {
            get
            {
                return BtnRightProbe.IsHitTestVisible;
            }
            set
            {
                BtnRightProbe.IsHitTestVisible = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            Helper.logger.Debug("++");
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
            this.DataContext = mainViewModel;
            UsbTcd.TCDObj.OnProbeUnplugged += TCDObjOnProbeUnplugged;
            UsbTcd.TCDObj.OnProbePlugged += TCDObjOnProbePlugged;
            PowerController.Instance.OnDeviceStateChanged += MicrocontrollerOnDeviceStateChanged;
            PowerController.Instance.StartWatcher();
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Event for TCD Probe plugged
        /// </summary>
        /// <param name="probe"></param>
        void TCDObjOnProbePlugged(UsbTcdLibrary.TCDHandles probe)
        {
            if(probe==UsbTcdLibrary.TCDHandles.Channel1)
            {
                this.Dispatcher.Invoke(() =>
                    {
                        BtnLeftProbe.IsEnabled = true;
                    });
                if(App.ActiveChannels==UsbTcdLibrary.ActiveChannels.Channel2)
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Both;
                }
                else
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel1;
                }
            }
            else if(probe == UsbTcdLibrary.TCDHandles.Channel2)
            {
                this.Dispatcher.Invoke(() =>
                {
                    BtnRightProbe.IsEnabled = true;
                });
                if(App.ActiveChannels==UsbTcdLibrary.ActiveChannels.Channel1)
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Both;
                }
                else
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel2;
                }
            }
        }

        /// <summary>
        /// Event for TCD probe unplugged
        /// </summary>
        /// <param name="probe"></param>
        internal void TCDObjOnProbeUnplugged(UsbTcdLibrary.TCDHandles probe)
        {
            if (probe == UsbTcdLibrary.TCDHandles.Channel1)
            {
                this.Dispatcher.Invoke(() =>
                {
                    BtnLeftProbe.IsEnabled = false;
                });
                if (App.ActiveChannels == UsbTcdLibrary.ActiveChannels.Both)
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel2;
                }
                else
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.None;
                }
            }
            else if (probe == UsbTcdLibrary.TCDHandles.Channel2)
            {
                this.Dispatcher.Invoke(() =>
                {
                    BtnRightProbe.IsEnabled = false;
                });
                if (App.ActiveChannels == UsbTcdLibrary.ActiveChannels.Both)
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel1;
                }
                else
                {
                    App.ActiveChannels = UsbTcdLibrary.ActiveChannels.None;
                }
            }
        }       

        /// <summary>
        /// Main Window load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                ExamTab.Content = new ExamUserControl();
                InfoTab.Content = new InfoUserControl();
                CalibrationTab.Content = new CalibrationUserControl();
                PacketTab.Content = new PacketControl();
                FPGATab.Content = new FPGAUserControl();
                SoftwareTab.Content = new SoftwareUserControl();
                LogTab.Content = new LogUserControl();
                BtnLeftProbe.IsEnabled = false;
                BtnRightProbe.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// State change for Micro Controller
        /// </summary>
        /// <param name="flag"></param>
        private async void MicrocontrollerOnDeviceStateChanged(bool flag)
        {
            Helper.logger.Debug("++");
            try
            {
                if (flag)
                {
                    //Microcontroller is connected
                    if (!PowerController.Instance.IsControllerOn)
                    {
                        await PowerController.Instance.TurnControllerOn();
                    }
                    await PowerController.Instance.UpdatePowerParameters(true, true, false, false, true);
                    await Task.Delay(Constants.TimeForTCDtoLoad);
                    App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.MicrocontrollerConnected);
                    this.Dispatcher.Invoke(() => 
                    {
                        switch (App.ActiveChannels)
                        {
                            case UsbTcdLibrary.ActiveChannels.Both:
                                BtnLeftProbe.IsEnabled = true;
                                BtnRightProbe.IsEnabled = true;
                                App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel2; //Temp for xoriant TCD since channel 1 doesn't work
                                break;
                            case UsbTcdLibrary.ActiveChannels.Channel1:
                                BtnLeftProbe.IsEnabled = true;
                                break;
                            case UsbTcdLibrary.ActiveChannels.Channel2:
                                BtnRightProbe.IsEnabled = true;
                                break;
                        }
                        BtnPower.IsEnabled = true;
                        (this.InfoTab.Content as InfoUserControl).IsEnabled = true;
                        (this.FPGATab.Content as FPGAUserControl).IsEnabled = true;
                        (this.CalibrationTab.Content as CalibrationUserControl).IsEnabled = true;
                        (this.SoftwareTab.Content as SoftwareUserControl).IsEnabled = true;
                    });
                }
                else
                {
                    //microcontroller disconnected
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.MicrocontrollerDisconnected);
                    TurnTCDOFF();
                    this.Dispatcher.Invoke(() =>
                    {
                        BtnPower.IsEnabled = false;
                    });
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// TCD Power button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TCDPowerClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                if (App.CurrentChannel == UsbTcdLibrary.TCDHandles.Channel1 || App.CurrentChannel == UsbTcdLibrary.TCDHandles.Channel2)
                {
                    if ((bool)IsPowerChecked)
                    {
                        //Turn TCD ON
                        if (TurnTCDON != null)
                        {
                            BtnLeftProbe.IsHitTestVisible = false;
                            BtnRightProbe.IsHitTestVisible = false;
                            TurnTCDON();
                        }
                    }
                    else
                    {
                        //Turn TCD OFF
                        if (TurnTCDOFF != null)
                        {
                            TurnTCDOFF();
                            BtnLeftProbe.IsHitTestVisible = true;
                            BtnRightProbe.IsHitTestVisible = true;
                            App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Left Probe icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftProbeClick(object sender, RoutedEventArgs e)
        {
            App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel1;
        }

        /// <summary>
        /// right probe icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightProbeClick(object sender, RoutedEventArgs e)
        {
            App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel2;
        }

        /// <summary>
        /// expand click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpandClick(object sender, RoutedEventArgs e)
        {
            previousIndex = NavigationTabs.SelectedIndex;
            LogSelectedTabIndex = LogTabControl.SelectedIndex;
            NavigationTabs.SelectedIndex = Constants.VALUE_6;
            FooterTextBox.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// clear the logs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearLogClick(object sender, RoutedEventArgs e)
        {
            mainViewModel.TCDLog = "";
            mainViewModel.ApplicationLog = "";
        }

        /// <summary>
        /// Navigation on different tabs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationTabsSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (NavigationTabs.SelectedIndex != Constants.VALUE_6)
                {
                    FooterTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    FooterTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }       
    }
}
