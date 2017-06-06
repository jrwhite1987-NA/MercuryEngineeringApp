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


namespace MercuryEngApp
{
    public delegate void TCDPower();
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static ILog logger = LogManager.GetLogger("EnggAppAppender");

        internal bool isPowerOn;
        public static event TCDPower TurnTCDON;
        public static event TCDPower TurnTCDOFF;
        public int previousIndex = 0;
        public int LogSelectedTabIndex = 0;
        public MainViewModel mainViewModel = new MainViewModel();
        Action workAction;
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

        public MainWindow()
        {
            logger.Debug("++");
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
            this.DataContext = mainViewModel;
            isPowerOn = false;
            PowerController.Instance.OnDeviceStateChanged += MicrocontrollerOnDeviceStateChanged;
            PowerController.Instance.StartWatcher();
            workAction = delegate
            {
                switch (App.ActiveChannels)
                {
                    case UsbTcdLibrary.ActiveChannels.Both:
                        BtnLeftProbe.IsHitTestVisible = true;
                        BtnRightProbe.IsHitTestVisible = true;
                        App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel2; //Temp for xoriant TCD since channel 1 doesn't work
                        break;
                    case UsbTcdLibrary.ActiveChannels.Channel1:
                        BtnLeftProbe.IsHitTestVisible = true;
                        break;
                    case UsbTcdLibrary.ActiveChannels.Channel2:
                        BtnRightProbe.IsHitTestVisible = true;
                        break;
                }
            };
           
            logger.Debug("--");
        }       

        async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            try
            {
                ExamTab.Content = new ExamUserControl();
                InfoTab.Content = new InfoUserControl();
                CalibrationTab.Content = new CalibrationUserControl();
                PacketTab.Content = new PacketControl();
                FPGATab.Content = new FPGAUserControl();
                SoftwareTab.Content = new SoftwareUserControl();
                LogTab.Content = new LogUserControl();
                BtnLeftProbe.IsHitTestVisible = false;
                BtnRightProbe.IsHitTestVisible = false;
                
                App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                await Dispatcher.BeginInvoke(workAction, System.Windows.Threading.DispatcherPriority.Normal, null);
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private async void MicrocontrollerOnDeviceStateChanged(bool flag)
        {
            logger.Debug("++");
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
                    await Dispatcher.BeginInvoke(workAction, System.Windows.Threading.DispatcherPriority.Normal, null);
                }
                else
                {
                    //microcontroller disconnected
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private void TCDPowerClick(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            try
            {
                if (!isPowerOn)
                {
                    //Turn TCD ON
                    if (TurnTCDON != null)
                    {
                        isPowerOn = true;
                        TurnTCDON();
                    }
                }
                else
                {
                    //Turn TCD OFF
                    if (TurnTCDOFF != null)
                    {
                        isPowerOn = false;
                        TurnTCDOFF();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private void LeftProbeClick(object sender, RoutedEventArgs e)
        {
            App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel1;
        }

        private void RightProbeClick(object sender, RoutedEventArgs e)
        {
            App.ActiveChannels = UsbTcdLibrary.ActiveChannels.Channel2;
        }

        private void ExpandClick(object sender, RoutedEventArgs e)
        {
            previousIndex = NavigationTabs.SelectedIndex;
            LogSelectedTabIndex = LogTabControl.SelectedIndex;
            NavigationTabs.SelectedIndex = 6;
            FooterTextBox.Visibility = Visibility.Collapsed;
        }

        private void ClearLogClick(object sender, RoutedEventArgs e)
        {
            mainViewModel.TCDLog = "";
            mainViewModel.ApplicationLog = "";
        }

        private void NavigationTabs_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (NavigationTabs.SelectedIndex != 6)
                {
                    FooterTextBox.Visibility = Visibility.Visible;
                }
                else
                {
                    FooterTextBox.Visibility = Visibility.Collapsed;
                }
            }
        }

        //private void spectrumBinCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    if(spectrumBinCombobox.SelectedIndex != -1)
        //    {
        //        Constants.SpectrumBin = Convert.ToInt32(spectrumBinCombobox.SelectedValue);
        //    }
        //}
    }
}
