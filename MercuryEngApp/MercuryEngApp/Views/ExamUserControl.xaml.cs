using Core.Constants;
using MercuryEngApp.Common;
using PlottingLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UsbTcdLibrary;
using UsbTcdLibrary.CommunicationProtocol;
using UsbTcdLibrary.PacketFormats;
using log4net;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for ExamTabUserControl.xaml
    /// </summary>
    public partial class ExamUserControl : UserControl
    {
        #region private fields

        static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ExamViewModel examViewModelObj = new ExamViewModel();

        private Queue<DMIPmdDataPacket[]> PacketCollection = new Queue<DMIPmdDataPacket[]>();
        /// <summary>
        /// The left spectrum bitmap
        /// </summary>
        private WriteableBitmap leftSpectrumBitmap;

        /// <summary>
        /// The left m mode bitmap
        /// </summary>
        private WriteableBitmap leftMModeBitmap;

        private NaGraph NaGraph { get; set; }

        public AudioWrapper TCDAudio { get; set; }

        public int VelocityRange { get; set; }

        public int Depth { get; set; }

        public int PRF { get; set; }

        private bool IsPriorityToChannel1;
        #endregion
       
        public ExamUserControl()
        {
            InitializeComponent();
            this.Loaded += ExamUserControlLoaded;
            this.Unloaded += ExamUserControlUnloaded;
            PowerController.Instance.OnDeviceStateChanged += MicrocontrollerOnDeviceStateChanged;
            this.DataContext = examViewModelObj;

            TCDAudio = new AudioWrapper();
            TCDAudio.PRF = 8000;
            TCDAudio.SetVolume(30);    
        }

        private void MicrocontrollerOnDeviceStateChanged(bool flag)
        {
            if (!flag)
            {
                try
                {
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
        }

        void ExamUserControlUnloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.TurnTCDON -= MainWindowTurnTCDON;
                MainWindow.TurnTCDOFF -= MainWindowTurnTCDOFF;
                //Clear graph data
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }

        }

        void ExamUserControlLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.TurnTCDON += MainWindowTurnTCDON;
                MainWindow.TurnTCDOFF += MainWindowTurnTCDOFF;
            }
            catch(Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        void MainWindowTurnTCDOFF()
        {
            TCDAudio.AudioCollection.CollectionChanged -= TCDAudio.AudioCollectionCollectionChanged;
            CompositionTarget.Rendering -= CompositionTargetRendering;
            UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
            UsbTcd.TCDObj.TurnTCDPowerOff();
        }

        async void MainWindowTurnTCDON()
        {
            await PlotGraph();
            CreateVerticalScales();
            btnEnvelopToggle_Click(null, null);
        }

        private void CreateVerticalScales()
        {
            VelocityRange = 308;
            Depth = 48;
            PRF = 8000;
            try
            {
                Scale.CreateScale(new ScaleParameters
                {
                    ParentControl = scaleGrid,
                    ScreenCoords = new Point(-17, 107),
                    VelocityRange = VelocityRange,
                    ScaleType = ScaleTypeEnum.Spectrogram,
                    BitmapHeight = imageSpectrogram.Height
                });

                MmodeSetting mModeSetting = MmodeSetting.GetDepthRange(Depth, PRF, false);
                customDepthSlider.Maximum = mModeSetting.MaxDepthDisplay;
                customDepthSlider.Minimum = mModeSetting.MinDepthDisplay;
                Scale.CreateMmodeScale(scaleDepthGrid, mModeSetting.MinDepthDisplay, mModeSetting.MaxDepthDisplay);
            }
            catch(Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }  

        async Task PlotGraph()
        {
            try
            {
                InitializeBitmap();
                CompositionTarget.Rendering += CompositionTargetRendering;
                await UsbTcd.TCDObj.TurnTCDPowerOnAsync();
                if (UsbTcd.TCDObj.InitializeTCD())
                {
                    TCDRequest request = new TCDRequest();
                    request.ChannelID = App.CurrentChannel;
                    request.Value3 = Constants.defaultLength;
                    await UsbTcd.TCDObj.SetLengthAsync(request);

                    request.Value3 = Constants.defaultDepth;
                    await UsbTcd.TCDObj.SetDepthAsync(request);

                    request.Value3 = Constants.defaultPower;
                    await UsbTcd.TCDObj.SetPowerAsync(request);

                    request.Value3 = Constants.defaultFilter;
                    await UsbTcd.TCDObj.SetFilterAsync(request);

                    request.Value3 = Constants.defaultPRF;
                    request.Value2 = Constants.defaultStartDepth;
                    await UsbTcd.TCDObj.SetPRF(request);

                    UsbTcd.TCDObj.OnPacketFormed += TCDObjOnPacketFormed;
                    UsbTcd.TCDObj.StartTCDReading();
                    TCDAudio.AudioCollection.CollectionChanged += TCDAudio.AudioCollectionCollectionChanged;
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }


        void TCDObjOnPacketFormed(DMIPmdDataPacket[] packets)
        {
            try
            {
                PacketCollection.Enqueue(packets);
                AddPacketsToAudioCollection(packets);
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private void AddPacketsToAudioCollection(DMIPmdDataPacket[] packets)
        {
            var currentPacket = packets[0];
            if (currentPacket == null)
                currentPacket = packets[1];

            if (currentPacket != null)
            {
                try
                {
                    if (TCDAudio.PRF != currentPacket.parameter.PRF)
                    {
                        TCDAudio.PRF = currentPacket.parameter.PRF;
                        TCDAudio.soundBuffer = new byte[sizeof(short) * TCDAudio.PRF / 5];
                    }

                    TCDAudio.AudioCollection.Add(currentPacket);
                }
                catch (Exception ex)
                {
                    logger.Warn("Exception: ", ex);
                }
            }
        }

        void CompositionTargetRendering(object sender, EventArgs e)
        {
            while (PacketCollection.Count > 0)
            {
                try
                {
                    DMIPmdDataPacket[] packet = PacketCollection.Dequeue();
                    if (packet[0] != null)
                    {
                        IsPriorityToChannel1 = true;
                        examViewModelObj.PosMean = packet[0].envelope.posMEAN/10;
                        examViewModelObj.PosMin = packet[0].envelope.posDIAS/10;
                        examViewModelObj.PosMax = packet[0].envelope.posPEAK/10;
                        examViewModelObj.PosPI = packet[0].envelope.posPI/10;
                        examViewModelObj.NegMean = packet[0].envelope.negMEAN/10;
                        examViewModelObj.NegMin = packet[0].envelope.negDIAS/10;
                        examViewModelObj.NegMax = packet[0].envelope.negPEAK/10;
                        examViewModelObj.NegPI = packet[0].envelope.negPI/10;

                        examViewModelObj.PacketDepth = packet[0].spectrum.depth;
                        examViewModelObj.PacketFilter = packet[0].spectrum.clutterFilter;
                        examViewModelObj.PacketPower = packet[0].parameter.acousticPower;
                        examViewModelObj.PacketPRF = packet[0].parameter.PRF;
                        examViewModelObj.PacketStartDepth = packet[0].mmode.startDepth;
                        examViewModelObj.PacketSVol = packet[0].parameter.sampleLength;
                        examViewModelObj.TIC = packet[0].parameter.TIC;
                        NaGraph.ProcessPacket(packet, true, 1);
                    }
                    else if(packet[1]!=null && !IsPriorityToChannel1 )
                    {
                        examViewModelObj.PosMean = packet[1].envelope.posMEAN/10;
                        examViewModelObj.PosMin = packet[1].envelope.posDIAS/10;
                        examViewModelObj.PosMax = packet[1].envelope.posPEAK/10;
                        examViewModelObj.PosPI = packet[1].envelope.posPI/10;
                        examViewModelObj.NegMean = packet[1].envelope.negMEAN/10;
                        examViewModelObj.NegMin = packet[1].envelope.negDIAS/10;
                        examViewModelObj.NegMax = packet[1].envelope.negPEAK/10;
                        examViewModelObj.NegPI = packet[1].envelope.negPI/10;

                        examViewModelObj.PacketDepth = packet[1].spectrum.depth;
                        examViewModelObj.PacketFilter = packet[1].spectrum.clutterFilter;
                        examViewModelObj.PacketPower = packet[1].parameter.acousticPower;
                        examViewModelObj.PacketPRF = packet[1].parameter.PRF;
                        examViewModelObj.PacketStartDepth = packet[1].mmode.startDepth;
                        examViewModelObj.PacketSVol = packet[1].parameter.sampleLength;
                        examViewModelObj.TIC = packet[1].parameter.TIC;
                        NaGraph.ProcessPacket(packet, true, 2);
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn("Exception: ", ex);
                    throw;
                }
            }
        }

        private void InitializeBitmap()
        {
            int width = 500;

            try
            {
                leftSpectrumBitmap = BitmapFactory.New(width, 129);
                imageSpectrogram.Source = leftSpectrumBitmap;


                leftMModeBitmap = BitmapFactory.New(width, 30);
                imageMmode.Source = leftMModeBitmap;

                NaGraph = new NaGraph(new BitmapList
                {
                    LeftMmodeBitmap = leftMModeBitmap,
                    LeftSpectrumBitmap = leftSpectrumBitmap,
                    RightMmodeBitmap = leftMModeBitmap,
                    RightSpectrumBitmap = leftSpectrumBitmap
                });
                NaGraph.SizeOfQueue = 500;
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private async void SendPower(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TCDRequest requestObject = new TCDRequest())
                {
                    requestObject.ChannelID = App.CurrentChannel;
                    requestObject.Value3 = examViewModelObj.Power;
                    await UsbTcd.TCDObj.SetPowerAsync(requestObject);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private async void SendDepth(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TCDRequest requestObject = new TCDRequest())
                {
                    requestObject.ChannelID = App.CurrentChannel;
                    requestObject.Value3 = examViewModelObj.Depth;
                    TCDResponse response = await UsbTcd.TCDObj.SetDepthAsync(requestObject);
                    if (response.Result)
                    {
                        customDepthSlider.Value = examViewModelObj.Depth;
                        customDepthSlider.Resources["textValue"] = Convert.ToInt32(customDepthSlider.Value).ToString();
                        customDepthSlider.InvalidateArrange();
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private async void SendFilter(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TCDRequest requestObject = new TCDRequest())
                {
                    requestObject.ChannelID = App.CurrentChannel;
                    requestObject.Value3 = examViewModelObj.Filter;
                    await UsbTcd.TCDObj.SetFilterAsync(requestObject);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private async void SendLength(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TCDRequest requestObject = new TCDRequest())
                {
                    requestObject.ChannelID = App.CurrentChannel;
                    requestObject.Value3 = examViewModelObj.SVol;
                    await UsbTcd.TCDObj.SetLengthAsync(requestObject);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private async void SendPRF(object sender, RoutedEventArgs e)
        {
            try
            {
                using (TCDRequest requestObject = new TCDRequest())
                {
                    requestObject.ChannelID = App.CurrentChannel;
                    requestObject.Value3 = examViewModelObj.PRF;
                    requestObject.Value2 = (byte)examViewModelObj.StartDepth;
                    await UsbTcd.TCDObj.SetPRF(requestObject);
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }
        /// <summary>
        /// The leftcurrent base line postion
        /// </summary>
        private int leftcurrentBaseLinePosition = 0;

        /// <summary>
        /// Gets or sets the left baseline postion.
        /// </summary>
        /// <value>The left baseline postion.</value>
        public int LeftBaselinePosition
        {
            get { return leftcurrentBaseLinePosition; }
            set
            {
                leftcurrentBaseLinePosition = value - Constants.BaselineValue;
            }
        }

       
        private void CusomSlider_LostMouseCapture(object sender, MouseEventArgs e)
        {
            Constants.BaselineValue = 64;

            try
            {
                LeftBaselinePosition = (int)CustomSlider.Value;

                NaGraph.LeftSpectrogram.BaseLinePosition = LeftBaselinePosition;
                NaGraph.RightSpectrogram.BaseLinePosition = LeftBaselinePosition;
                Point screenCoords = Thumb.TransformToVisual(scaleGrid).Transform(new Point(0, 0));
                screenCoords.Y = screenCoords.Y + 25;
                Scale.CreateScale(new ScaleParameters
                {
                    ParentControl = scaleGrid,
                    ScreenCoords = screenCoords,
                    VelocityRange = VelocityRange,
                    ScaleType = ScaleTypeEnum.Spectrogram,
                    BitmapHeight = imageSpectrogram.Height
                });
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
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

            return thumb;
        }

        private void customDepthSlider_LostMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                customDepthSlider.Resources["textValue"] = Convert.ToInt32(customDepthSlider.Value).ToString();
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private void btnEnvelopToggle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnEnvelop.Content.ToString() == "Envelope On")
                {
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.NegativeFlowVisible = true;
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.PositiveFlowVisible = true;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.NegativeFlowVisible = true;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.PositiveFlowVisible = true;
                    btnEnvelop.Content = "Envelope Off";
                }
                else
                {
                    btnEnvelop.Content = "Envelope On";
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.NegativeFlowVisible = false;
                    NaGraph.LeftSpectrogram.SpectrumEnvolope.PositiveFlowVisible = false;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.NegativeFlowVisible = false;
                    NaGraph.RightSpectrogram.SpectrumEnvolope.PositiveFlowVisible = false;
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }

        private void toggleLimitsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (toggleLimits.Content.ToString() == "Limits Off")
                {
                    toggleLimits.Content = "Limits On";
                }
                else
                {
                    toggleLimits.Content = "Limits Off";
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
        }
    }
  
}
