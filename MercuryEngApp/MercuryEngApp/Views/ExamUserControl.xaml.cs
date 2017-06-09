using Core.Constants;
using MercuryEngApp.Common;
using PlottingLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UsbTcdLibrary;
using UsbTcdLibrary.CommunicationProtocol;
using UsbTcdLibrary.PacketFormats;
using log4net;
using System.Xml.Linq;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for ExamTabUserControl.xaml
    /// </summary>
    public partial class ExamUserControl : UserControl
    {
        #region private fields

        static ILog logger = LogManager.GetLogger("EnggAppAppender");
        static ILog AppLogger = LogManager.GetLogger("AppLogAppender");
        static ILog TCDLogger = LogManager.GetLogger("TCDLogAppender");

        private ExamViewModel examViewModelObj;

        private Queue<DMIPmdDataPacket[]> PacketCollection = new Queue<DMIPmdDataPacket[]>();
        /// <summary>
        /// The left spectrum bitmap
        /// </summary>
        private WriteableBitmap leftSpectrumBitmap;

        /// <summary>
        /// The left m mode bitmap
        /// </summary>
        private WriteableBitmap leftMModeBitmap;

        private Scale scaleObj = new Scale();

        private NaGraph NaGraph { get; set; }

        public AudioWrapper TCDAudio { get; set; }       

        XDocument xmlDoc;

        #endregion

        public ExamUserControl()
        {
            logger.Debug("++");           
            InitializeComponent();
            examViewModelObj = new ExamViewModel();
            this.Loaded += ExamUserControlLoaded;
            this.Unloaded += ExamUserControlUnloaded;
            PowerController.Instance.OnDeviceStateChanged += MicrocontrollerOnDeviceStateChanged;
            this.DataContext = examViewModelObj;
            xmlDoc = XDocument.Load("LocalFolder/InfoConfig.xml");
            examViewModelObj.PRFList = xmlDoc.Root.Elements("PRFList").
                Elements("PRF").Select(element => Convert.ToUInt32(element.Value)).ToList();
            TCDAudio = new AudioWrapper();
            TCDAudio.PRF = Constants.VALUE_8000;
            TCDAudio.SetVolume(Constants.VALUE_30);
            logger.Debug("--");
        }

        private void MicrocontrollerOnDeviceStateChanged(bool flag)
        {
            logger.Debug("++");
            if (!flag)
            {
                try
                {
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.TCDTurnedOff);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.MicrocontrollerDisconnected);
                    CompositionTarget.Rendering -= CompositionTargetRendering;
                    UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
                    //microcontroller disconnected
                    UsbTcd.TCDObj.TurnTCDPowerOff();
                }
                catch (Exception ex)
                {
                    logger.Warn("Exception: ", ex);
                }
            }
            logger.Debug("--");
        }

        void ExamUserControlUnloaded(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            try
            {
                MainWindow.TurnTCDON -= MainWindowTurnTCDON;
                MainWindow.TurnTCDOFF -= MainWindowTurnTCDOFF;
                if ((bool)App.mainWindow.IsPowerChecked)
                {
                    MainWindowTurnTCDOFF();
                    App.mainWindow.IsPowerChecked = false;
                }
                //Clear graph data
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        void ExamUserControlLoaded(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            try
            {
                MainWindow.TurnTCDON += MainWindowTurnTCDON;
                MainWindow.TurnTCDOFF += MainWindowTurnTCDOFF; 
                btnEnvelop.IsChecked = true;
                toggleLimits.IsChecked = true;
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        async void MainWindowTurnTCDOFF()
        {
            logger.Debug("++");
            try
            {
                TCDAudio.AudioCollection.CollectionChanged -= TCDAudio.AudioCollectionCollectionChanged;
                CompositionTarget.Rendering -= CompositionTargetRendering;
                UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.TCDTurnedOff);
                UsbTcd.TCDObj.TurnTCDPowerOff();

                using (TCDRequest request = new TCDRequest())
                {
                    request.Value = 10;
                    request.ChannelID = App.CurrentChannel;
                    TCDReadInfoResponse response = await UsbTcd.TCDObj.ReadServiceLogAsync(request);

                    foreach (var item in response.ServicePacketList)
                    {
                        LogWrapper.Log(Constants.TCDLog, item.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        async void MainWindowTurnTCDON()
        {
            logger.Debug("++");
            try
            {
                await PlotGraph();
                CreateVerticalScales();               
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private void CreateVerticalScales()
        {
            logger.Debug("++");            
            try
            {
                scaleObj.CreateScale(new ScaleParameters
                {
                    ParentControl = scaleGrid,
                    ScreenCoords = examViewModelObj.ScreenCoords,
                    VelocityRange = examViewModelObj.VelocityRange,
                    ScaleType = ScaleTypeEnum.Spectrogram,
                    BitmapHeight = imageSpectrogram.Height
                });

                MmodeSetting mModeSetting = MmodeSetting.GetDepthRange((int)examViewModelObj.Depth);
                customDepthSlider.Maximum = mModeSetting.MaxDepthDisplay;
                customDepthSlider.Minimum = mModeSetting.MinDepthDisplay;
                scaleObj.CreateMmodeScale(scaleDepthGrid, mModeSetting.MinDepthDisplay, mModeSetting.MaxDepthDisplay);
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        async Task PlotGraph()
        {
            logger.Debug("++");
            try
            {
                InitializeBitmap();
                await UsbTcd.TCDObj.TurnTCDPowerOnAsync();
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.TCDTurnedOn);

                if (UsbTcd.TCDObj.InitializeTCD())
                {
                    CompositionTarget.Rendering += CompositionTargetRendering;
                    TCDRequest request = new TCDRequest();
                    request.ChannelID = App.CurrentChannel;
                    request.Value3 = Constants.defaultLength;
                    await UsbTcd.TCDObj.SetLengthAsync(request);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.LengthSent);

                    request.Value3 = Constants.defaultDepth;
                    await UsbTcd.TCDObj.SetDepthAsync(request);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.DepthSent);

                    request.Value3 = Constants.defaultPower;
                    await UsbTcd.TCDObj.SetPowerAsync(request);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.PowerSent);

                    request.Value3 = Constants.defaultFilter;
                    await UsbTcd.TCDObj.SetFilterAsync(request);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FilterSent);

                    request.Value3 = Constants.defaultPRF;
                    request.Value2 = Constants.defaultStartDepth;
                    await UsbTcd.TCDObj.SetPRF(request);
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.PRFSet);

                    UsbTcd.TCDObj.OnPacketFormed += TCDObjOnPacketFormed;
                    UsbTcd.TCDObj.StartTCDReading();
                    TCDAudio.AudioCollection.CollectionChanged += TCDAudio.AudioCollectionCollectionChanged;
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }


        void TCDObjOnPacketFormed(DMIPmdDataPacket[] packets)
        {
            logger.Debug("++");
            try
            {
                PacketCollection.Enqueue(packets);
                AddPacketsToAudioCollection(packets);
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }

            logger.Debug("--");
        }

        private void AddPacketsToAudioCollection(DMIPmdDataPacket[] packets)
        {
            logger.Debug("++");

            var currentPacket = packets[0];
            if (currentPacket == null)
            {
                currentPacket = packets[1];
            }

            if (currentPacket != null)
            {
                try
                {
                    if (TCDAudio.PRF != currentPacket.parameter.PRF)
                    {
                        TCDAudio.PRF = currentPacket.parameter.PRF;
                        TCDAudio.soundBuffer = new byte[sizeof(short) * TCDAudio.PRF / Constants.VALUE_5];
                    }

                    TCDAudio.AudioCollection.Add(currentPacket);
                }
                catch (Exception ex)
                {
                    logger.Warn("Exception: ", ex);
                }
            }
            logger.Debug("--");
        }

        void CompositionTargetRendering(object sender, EventArgs e)
        {
            logger.Debug("++");
            try
            {
                while (PacketCollection.Count > 0)
                {
                    DMIPmdDataPacket[] packet = PacketCollection.Dequeue();
                    if (App.ActiveChannels==ActiveChannels.Channel1 && packet[0]!=null)
                    {
                        examViewModelObj.PosMean = packet[0].envelope.posMEAN / Constants.VALUE_10;
                        examViewModelObj.PosMin = packet[0].envelope.posDIAS / Constants.VALUE_10;
                        examViewModelObj.PosMax = packet[0].envelope.posPEAK / Constants.VALUE_10;
                        examViewModelObj.PosPI = packet[0].envelope.posPI / Constants.VALUE_10;
                        examViewModelObj.NegMean = packet[0].envelope.negMEAN / Constants.VALUE_10;
                        examViewModelObj.NegMin = packet[0].envelope.negDIAS / Constants.VALUE_10;
                        examViewModelObj.NegMax = packet[0].envelope.negPEAK / Constants.VALUE_10;
                        examViewModelObj.NegPI = packet[0].envelope.negPI / Constants.VALUE_10;

                        examViewModelObj.PacketDepth = packet[0].spectrum.depth;
                        examViewModelObj.PacketFilter = packet[0].spectrum.clutterFilter;
                        examViewModelObj.PacketPower = packet[0].parameter.acousticPower;
                        examViewModelObj.PacketPRF = packet[0].parameter.PRF;
                        examViewModelObj.PacketStartDepth = packet[0].mmode.startDepth;
                        examViewModelObj.PacketSVol = packet[0].parameter.sampleLength;
                        examViewModelObj.TIC = packet[0].parameter.TIC;
                        NaGraph.ProcessPacket(packet, true, Constants.VALUE_1);
                    }
                    else 
                    {
                        if (packet[1] != null && App.ActiveChannels == ActiveChannels.Channel2)
                        {
                            examViewModelObj.PosMean = packet[1].envelope.posMEAN / Constants.VALUE_10;
                            examViewModelObj.PosMin = packet[1].envelope.posDIAS / Constants.VALUE_10;
                            examViewModelObj.PosMax = packet[1].envelope.posPEAK / Constants.VALUE_10;
                            examViewModelObj.PosPI = packet[1].envelope.posPI / Constants.VALUE_10;
                            examViewModelObj.NegMean = packet[1].envelope.negMEAN / Constants.VALUE_10;
                            examViewModelObj.NegMin = packet[1].envelope.negDIAS / Constants.VALUE_10;
                            examViewModelObj.NegMax = packet[1].envelope.negPEAK / Constants.VALUE_10;
                            examViewModelObj.NegPI = packet[1].envelope.negPI / Constants.VALUE_10;

                            examViewModelObj.PacketDepth = packet[1].spectrum.depth;
                            examViewModelObj.PacketFilter = packet[1].spectrum.clutterFilter;
                            examViewModelObj.PacketPower = packet[1].parameter.acousticPower;
                            examViewModelObj.PacketPRF = packet[1].parameter.PRF;
                            examViewModelObj.PacketStartDepth = packet[1].mmode.startDepth;
                            examViewModelObj.PacketSVol = packet[1].parameter.sampleLength;
                            examViewModelObj.TIC = packet[1].parameter.TIC;
                            NaGraph.ProcessPacket(packet, true, Constants.VALUE_2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
                throw;
            }
            logger.Debug("--");
        }

        private void InitializeBitmap()
        {
            logger.Debug("++");
            int width = Constants.VALUE_500;

            try
            {
                leftSpectrumBitmap = BitmapFactory.New(width, Constants.SpectrumBin+1);
                imageSpectrogram.Source = leftSpectrumBitmap;


                leftMModeBitmap = BitmapFactory.New(width, Constants.VALUE_30);
                imageMmode.Source = leftMModeBitmap;

                NaGraph = new NaGraph(new BitmapList
                {
                    LeftMmodeBitmap = leftMModeBitmap,
                    LeftSpectrumBitmap = leftSpectrumBitmap,
                    RightMmodeBitmap = leftMModeBitmap,
                    RightSpectrumBitmap = leftSpectrumBitmap
                });
                NaGraph.SizeOfQueue = Constants.VALUE_500;
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private async void SendPower(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            bool isValid = false;
            string errorMessage = string.Empty;
            try
            {
                isValid = Validators.ValidationRules.ValidateControl(Power, out errorMessage);
                if (isValid)
                {
                    using (TCDRequest requestObject = new TCDRequest())
                    {
                        requestObject.ChannelID = App.CurrentChannel;
                        requestObject.Value3 = examViewModelObj.Power;
                        await UsbTcd.TCDObj.SetPowerAsync(requestObject);
                        await Task.Delay(Constants.VALUE_100);
                        if (examViewModelObj.Power == examViewModelObj.PacketPower)
                        {
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.PowerSent);
                        }
                        else
                        {
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.PowerNotAccepted);
                        }
                    }       
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private async void SendDepth(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            bool isValid = false;
            string errorMessage = string.Empty;
            try
            {
                isValid = Validators.ValidationRules.ValidateControl(DepthTextBox, out errorMessage);
                if (isValid)
                {
                    using (TCDRequest requestObject = new TCDRequest())
                    {
                        requestObject.ChannelID = App.CurrentChannel;
                        requestObject.Value3 = examViewModelObj.Depth;
                        TCDResponse response = await UsbTcd.TCDObj.SetDepthAsync(requestObject);
                        await Task.Delay(Constants.VALUE_100);
                        if (examViewModelObj.Depth == examViewModelObj.PacketDepth)
                        {
                            customDepthSlider.Value = examViewModelObj.Depth;
                            MmodeSetting mModeSetting = MmodeSetting.GetDepthRange((int)examViewModelObj.Depth);
                            customDepthSlider.Maximum = mModeSetting.MaxDepthDisplay;
                            customDepthSlider.Minimum = mModeSetting.MinDepthDisplay;
                            scaleObj.CreateMmodeScale(scaleDepthGrid, mModeSetting.MinDepthDisplay, mModeSetting.MaxDepthDisplay);
                            customDepthSlider.Resources["textValue"] = Convert.ToInt32(customDepthSlider.Value).ToString();
                            customDepthSlider.InvalidateArrange();                          
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.DepthSent);
                        }
                        else
                        {
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.DepthNotAccepted);
                        }

                    }
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private async void SendFilter(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            string errorMessage = string.Empty;
            try
            {
                if (Validators.ValidationRules.ValidateControl(FilterTextBox, out errorMessage))
                {
                    using (TCDRequest requestObject = new TCDRequest())
                    {
                        requestObject.ChannelID = App.CurrentChannel;
                        requestObject.Value3 = examViewModelObj.Filter;
                        await UsbTcd.TCDObj.SetFilterAsync(requestObject);
                        await Task.Delay(Constants.VALUE_100);
                        if (examViewModelObj.Filter == examViewModelObj.PacketFilter)
                        {   
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FilterSent);
                        }
                        else
                        {
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.FilterNotAccepted);
                        }
                    }
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private async void SendLength(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            string errorMessage = string.Empty;
            try
            {
                if (Validators.ValidationRules.ValidateControl(SVolumeTextBox, out errorMessage))
                {
                    using (TCDRequest requestObject = new TCDRequest())
                    {
                        requestObject.ChannelID = App.CurrentChannel;
                        requestObject.Value3 = examViewModelObj.SVol;
                        await UsbTcd.TCDObj.SetLengthAsync(requestObject);
                        await Task.Delay(Constants.VALUE_100);
                        if (examViewModelObj.SVol == examViewModelObj.PacketSVol)
                        {   
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.LengthSent);
                        }
                        else
                        {
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.LengthNotAccepted);
                        }
                    }
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, errorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        private async void SendPRF(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            bool isDepthValid = false;
            bool isPRFValid = false;
            string depthErrorMessage = string.Empty;
            string prfErrorMessage = string.Empty;
            
            try
            {
                isDepthValid = Validators.ValidationRules.ValidateControl(StartDepthTextBox, out depthErrorMessage);
                isPRFValid = Validators.ValidationRules.ValidateControl(PRFCombo, out prfErrorMessage);
                if (isDepthValid && isPRFValid)
                {
                    using (TCDRequest requestObject = new TCDRequest())
                    {
                        requestObject.ChannelID = App.CurrentChannel;
                        requestObject.Value3 = examViewModelObj.SelectedPRF;
                        requestObject.Value2 = (byte)examViewModelObj.StartDepth;
                        await UsbTcd.TCDObj.SetPRF(requestObject);

                        await Task.Delay(Constants.VALUE_100);
                        if (examViewModelObj.SelectedPRF == examViewModelObj.PacketPRF)
                        {
                            examViewModelObj.VelocityRange = PRFOptions.GetMappedVelocityRange((int)examViewModelObj.SelectedPRF);
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.PRFSet);
                            scaleObj.CreateScale(new ScaleParameters
                            {
                                ParentControl = scaleGrid,
                                ScreenCoords = examViewModelObj.ScreenCoords,
                                VelocityRange = examViewModelObj.VelocityRange,
                                ScaleType = ScaleTypeEnum.Spectrogram,
                                BitmapHeight = imageSpectrogram.Height
                            });
                        }
                        else
                        {
                            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.PRFNotAccepted);
                        }
                    }
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, depthErrorMessage + " " + prfErrorMessage);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }
        
        private void CusomSliderLostMouseCapture(object sender, MouseEventArgs e)
        {
            logger.Debug("++");

            try
            {
                NaGraph.LeftSpectrogram.BaseLinePosition = examViewModelObj.BaselinePosition;
                NaGraph.RightSpectrogram.BaseLinePosition = examViewModelObj.BaselinePosition;
                examViewModelObj.ScreenCoords = Thumb.TransformToVisual(scaleGrid).Transform(new Point(Constants.VALUE_0, Constants.VALUE_25));   
                scaleObj.CreateScale(new ScaleParameters

                {
                    ParentControl = scaleGrid,
                    ScreenCoords = examViewModelObj.ScreenCoords,
                    VelocityRange = examViewModelObj.VelocityRange,
                    ScaleType = ScaleTypeEnum.Spectrogram,
                    BitmapHeight = imageSpectrogram.Height
                });
                if((bool)toggleLimits.IsChecked)
                {
                    SetEnvelopeRange(App.CurrentChannel);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        public Thumb Thumb
        {
            get
            {
                return GetThumb(this) as Thumb;
            }
        }

        private DependencyObject GetThumb(DependencyObject root)
        {
            logger.Debug("++");
            if (root is Thumb)
            {
                return root;
            }

            DependencyObject thumb = null;

            try
            {

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
                {
                    thumb = GetThumb(VisualTreeHelper.GetChild(root, i));

                    if (thumb is Thumb)
                    {
                        return thumb;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");

            return thumb;
        }

        private void ToggleLimitsClick(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");
            try
            {
                if (toggleLimits.Content.ToString() == "Limits Off")
                {
                    toggleLimits.Content = "Limits On";
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ToggleLimitsOn);
                }
                else
                {
                    toggleLimits.Content = "Limits Off";
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ToggleLimitsOff);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        /// <summary>
        /// Set envelope range
        /// </summary>
        /// <param name="channel">The channel.</param>
        private void SetEnvelopeRange(TCDHandles channel)
        {
            TCDRequest requestObject = new TCDRequest();
            requestObject.ChannelID = channel;
            double velocityPerUnitBaseline = (examViewModelObj.VelocityRange * 10) / (double)Constants.FFTSize;
            short posVelocity = 0, negVelocity = 0;


            if (examViewModelObj.BaselinePosition > 0)
            {
                negVelocity = (short)((examViewModelObj.SliderValue + 1) * velocityPerUnitBaseline * -1);
                posVelocity = (short)((Constants.FFTSize - examViewModelObj.SliderValue - 1) * velocityPerUnitBaseline);
            }
            else
            {
                posVelocity = (short)((Constants.FFTSize - examViewModelObj.SliderValue) * velocityPerUnitBaseline);
                negVelocity = (short)(examViewModelObj.SliderValue * velocityPerUnitBaseline * -1);
            }


            requestObject.Value = negVelocity;
            requestObject.Value3 = (ushort)posVelocity;
            UsbTcd.TCDObj.SetEnvelopeRangeAsync(requestObject);
            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.EnvelopeRangeSet);
        }

        private void SpectrumBinComboboxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CusomSliderLostMouseCapture(null, null);
            InitializeBitmap();
        }

        private void BtnEnvelopChecked(object sender, RoutedEventArgs e)
        {
            NaGraph.LeftSpectrogram.SpectrumEnvolope.NegativeFlowVisible = true;
            NaGraph.LeftSpectrogram.SpectrumEnvolope.PositiveFlowVisible = true;
            NaGraph.RightSpectrogram.SpectrumEnvolope.NegativeFlowVisible = true;
            NaGraph.RightSpectrogram.SpectrumEnvolope.PositiveFlowVisible = true;
            btnEnvelop.Content = "Envelope On";
            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.EnvelopeTurnedOn);
        }

        private void BtnEnvelopUnchecked(object sender, RoutedEventArgs e)
        {
            NaGraph.LeftSpectrogram.SpectrumEnvolope.NegativeFlowVisible = false;
            NaGraph.LeftSpectrogram.SpectrumEnvolope.PositiveFlowVisible = false;
            NaGraph.RightSpectrogram.SpectrumEnvolope.NegativeFlowVisible = false;
            NaGraph.RightSpectrogram.SpectrumEnvolope.PositiveFlowVisible = false;
            btnEnvelop.Content = "Envelope Off";
            LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.EnvelopeTurnedOff);
        }
       
    }

}
