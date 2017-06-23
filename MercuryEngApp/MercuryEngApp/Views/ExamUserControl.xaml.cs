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
using Core.Common;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for ExamTabUserControl.xaml
    /// </summary>
    public partial class ExamUserControl : UserControl
    {
        #region private fields

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

        /// <summary>
        /// Constructor
        /// </summary>
        public ExamUserControl()
        {
            Helper.logger.Debug("++");           
            InitializeComponent();
            examViewModelObj = new ExamViewModel();
            this.Loaded += ExamUserControlLoaded;
            this.Unloaded += ExamUserControlUnloaded;
            this.DataContext = examViewModelObj;
            xmlDoc = XDocument.Load("LocalFolder/InfoConfig.xml");
            examViewModelObj.PRFList = xmlDoc.Root.Elements("PRFList").
                Elements("PRF").Select(element => Convert.ToUInt32(element.Value)).ToList();
            TCDAudio = new AudioWrapper();
            TCDAudio.PRF = Constants.VALUE_8000;
            TCDAudio.SetVolume(Constants.VALUE_30);
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// TCD Probe Unplugged
        /// </summary>
        /// <param name="probe"></param>
        void TCDObjOnProbeUnplugged(TCDHandles probe)
        {
            if (probe == App.CurrentChannel)
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ProbeDisconnected);
                MainWindowTurnTCDOFF();
            }
        }

        /// <summary>
        /// Page UnLoaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExamUserControlUnloaded(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                MainWindow.TurnTCDON -= MainWindowTurnTCDON;
                MainWindow.TurnTCDOFF -= MainWindowTurnTCDOFF;
                UsbTcd.TCDObj.OnProbeUnplugged -= TCDObjOnProbeUnplugged;
                if ((bool)App.mainWindow.IsPowerChecked)
                {
                    MainWindowTurnTCDOFF();
                    App.mainWindow.IsPowerChecked = false;
                }
                //Clear graph data
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Page loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExamUserControlLoaded(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                MainWindow.TurnTCDON += MainWindowTurnTCDON;
                MainWindow.TurnTCDOFF += MainWindowTurnTCDOFF;
                UsbTcd.TCDObj.OnProbeUnplugged += TCDObjOnProbeUnplugged;
                btnEnvelop.IsChecked = true;
                toggleLimits.IsChecked = true;
                if(!(bool)App.mainWindow.IsPowerChecked)
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

        /// <summary>
        /// The counter packet check left
        /// </summary>
        private int _counterPacketCh1;

        /// <summary>
        /// Gets or sets the counter packet check left.
        /// </summary>
        /// <value>The counter packet check left.</value>
        private int counterPacketCh1
        {
            get
            {
                return _counterPacketCh1;
            }
            set
            {
                _counterPacketCh1 = value;
                if (value == Constants.PACKETS_PER_SEC)
                {
                    _counterPacketCh1 = 0;
                    DisableProbe(TCDHandles.Channel1);
                }
            }
        }

        /// <summary>
        /// The counter packet check left
        /// </summary>
        private int _counterPacketCh2;

        /// <summary>
        /// Gets or sets the counter packet check left.
        /// </summary>
        /// <value>The counter packet check left.</value>
        private int counterPacketCh2
        {
            get
            {
                return _counterPacketCh2;
            }
            set
            {
                _counterPacketCh2 = value;
                if (value == Constants.PACKETS_PER_SEC)
                {
                    _counterPacketCh2 = 0;
                    DisableProbe(TCDHandles.Channel2);
                }
            }
        }

        /// <summary>
        /// Disable the probe
        /// </summary>
        /// <param name="channel"></param>
        private async void DisableProbe(TCDHandles channel)
        {
            TCDRequest requestObject = null;
            try
            {
                requestObject = new TCDRequest();
                requestObject.ChannelID = channel;
                bool result = (await UsbTcd.TCDObj.IsProbeConnectedAsync(requestObject)).Result;
                if (!result)
                {
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ProbeDisconnected);
                    MainWindowTurnTCDOFF();
                    App.mainWindow.IsPowerChecked = false;
                    App.mainWindow.TCDObjOnProbeUnplugged(channel);
                    await PowerController.Instance.UpdatePowerParameters(true, true, false, false, true);
                    await Task.Delay(Constants.TimeForTCDtoLoad);
                    App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
        }

        /// <summary>
        /// TCD Off from Main Window
        /// </summary>
        async void MainWindowTurnTCDOFF()
        {
            Helper.logger.Debug("++");
            try
            {
                TCDAudio.AudioCollection.CollectionChanged -= TCDAudio.AudioCollectionCollectionChanged;
                CompositionTarget.Rendering -= CompositionTargetRendering;
                UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.TCDTurnedOff);
                UsbTcd.TCDObj.TurnTCDPowerOff();
                this.IsEnabled = false;
                using (TCDRequest request = new TCDRequest())
                {
                    request.Value = Constants.VALUE_10;
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// TCD On from Main Window
        /// </summary>
        async void MainWindowTurnTCDON()
        {
            Helper.logger.Debug("++");
            try
            {
                this.IsEnabled = true;
                await PlotGraph();
                CreateVerticalScales();               
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Create the vertical Scales
        /// </summary>
        private void CreateVerticalScales()
        {
            Helper.logger.Debug("++");            
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Plot the Graphs
        /// </summary>
        /// <returns></returns>
        async Task PlotGraph()
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// TCD on Packet Formed
        /// </summary>
        /// <param name="packets"></param>
        void TCDObjOnPacketFormed(DMIPmdDataPacket[] packets)
        {
            Helper.logger.Debug("++");
            try
            {
                PacketCollection.Enqueue(packets);
                AddPacketsToAudioCollection(packets);
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }

            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Add the Packets to Audio Collectuion
        /// </summary>
        /// <param name="packets"></param>
        private void AddPacketsToAudioCollection(DMIPmdDataPacket[] packets)
        {
            Helper.logger.Debug("++");

            var currentPacket = packets[Constants.VALUE_0];
            if (currentPacket == null)
            {
                currentPacket = packets[Constants.VALUE_1];
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
                    Helper.logger.Warn("Exception: ", ex);
                }
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// CompositionTargetRendering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CompositionTargetRendering(object sender, EventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                if (PacketCollection.Count > 0)
                {
                    while (PacketCollection.Count > Constants.VALUE_0)
                    {
                        DMIPmdDataPacket[] packet = PacketCollection.Dequeue();

                        if (App.CurrentChannel == TCDHandles.Channel1 && packet[0] != null)
                        {
                            counterPacketCh1 = 0;
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
                            if (packet[1] != null && App.CurrentChannel == TCDHandles.Channel2)
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
                else
                {
                    if (App.CurrentChannel == TCDHandles.Channel1)
                    {
                        counterPacketCh1++;
                    }
                    else if(App.CurrentChannel == TCDHandles.Channel2)
                    {
                        counterPacketCh2++;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw;
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Initialize the Bit Map
        /// </summary>
        private void InitializeBitmap()
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Send the Power to TCD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SendPower(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Send the Depth to TCD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SendDepth(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
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
                        await UsbTcd.TCDObj.SetDepthAsync(requestObject);
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Send the Filter to TCD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SendFilter(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Send the Length to TCD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SendLength(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Send the PRF to TCD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SendPRF(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }
        
        /// <summary>
        /// Custom Slider Mouse Lost Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CusomSliderLostMouseCapture(object sender, MouseEventArgs e)
        {
            Helper.logger.Debug("++");

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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Gets the Thumb
        /// </summary>
        public Thumb Thumb
        {
            get
            {
                return GetThumb(this) as Thumb;
            }
        }

        /// <summary>
        /// Get the Thumb
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private DependencyObject GetThumb(DependencyObject root)
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");

            return thumb;
        }

        /// <summary>
        /// Toggle Limits clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleLimitsClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
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
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
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

        /// <summary>
        /// Spectrum Bin Combo Box selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpectrumBinComboboxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CusomSliderLostMouseCapture(null, null);
            InitializeBitmap();
        }

        /// <summary>
        /// Envelope Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnvelopeClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                if (btnEnvelop.Content.ToString() == "Envelope Off")
                {
                    btnEnvelop.Content = "Envelope On";
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.NegativeFlowVisible = true;
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.PositiveFlowVisible = true;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.NegativeFlowVisible = true;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.PositiveFlowVisible = true;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.EnvelopeTurnedOn);
                }
                else
                {
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.NegativeFlowVisible = false;
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.PositiveFlowVisible = false;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.NegativeFlowVisible = false;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.PositiveFlowVisible = false;
                    btnEnvelop.Content = "Envelope Off";
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.EnvelopeTurnedOff);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Screen Size changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExamControlSizeChanged(object sender, SizeChangedEventArgs e)
        {
            examViewModelObj.Column1Width = ParentGrid.ColumnDefinitions[0].ActualWidth;
            examViewModelObj.Column2Width = ParentGrid.ColumnDefinitions[1].ActualWidth;
        }
       
    }

}
