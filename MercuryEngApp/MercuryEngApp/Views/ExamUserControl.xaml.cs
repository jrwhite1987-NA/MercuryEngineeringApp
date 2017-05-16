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

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for ExamTabUserControl.xaml
    /// </summary>
    public partial class ExamUserControl : UserControl
    {
        #region private fields

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

        #endregion
       
        public ExamUserControl()
        {
            InitializeComponent();
            this.Loaded += ExamUserControlLoaded;
            this.Unloaded += ExamUserControlUnloaded;
            PowerController.Instance.OnDeviceStateChanged += MicrocontrollerOnDeviceStateChanged;
            this.DataContext = examViewModelObj;
        }

        private async void MicrocontrollerOnDeviceStateChanged(bool flag)
        {
            if (!flag)
            {
                CompositionTarget.Rendering -= CompositionTargetRendering;
                UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
                //microcontroller disconnected
                await UsbTcd.TCDObj.TurnRecordingOffAsync();
                UsbTcd.TCDObj.TurnTCDPowerOff();
            }
        }


        async void ExamUserControlUnloaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= CompositionTargetRendering;
            UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;

            //Turn recording off
            await UsbTcd.TCDObj.TurnRecordingOffAsync();

            UsbTcd.TCDObj.TurnTCDPowerOff();
            //Clear graph data
        }

        async void ExamUserControlLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                await UsbTcd.TCDObj.SetModeAsync(App.CurrentChannel, TCDModes.Active);
                await PlotGraph();
            }
            catch(Exception ex)
            {

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
                    request.ChannelID = TCDHandles.Channel1;
                    request.Value3 = Constants.defaultLength;
                    await UsbTcd.TCDObj.SetLengthAsync(request);

                    request.Value3 = Constants.defaultDepth;
                    await UsbTcd.TCDObj.SetDepthAsync(request);

                    request.Value3 = Constants.defaultPower;
                    await UsbTcd.TCDObj.SetPowerAsync(request);

                    request.Value3 = Constants.defaultFilter;
                    await UsbTcd.TCDObj.SetFilterAsync(request);

                    //Turn recording on
                    await UsbTcd.TCDObj.TurnRecordingOnAsync(new TCDRequest() { Value = 1 });

                    UsbTcd.TCDObj.OnPacketFormed += TCDObjOnPacketFormed;
                    UsbTcd.TCDObj.StartTCDReading();
                }
            }
            catch (Exception ex)
            {

            }
        }


        void TCDObjOnPacketFormed(DMIPmdDataPacket[] packets)
        {
            PacketCollection.Enqueue(packets);
            AddPacketsToAudioCollection(packets);
        }

        private void AddPacketsToAudioCollection(DMIPmdDataPacket[] packets)
        {
            var currentPacket = packets[0];
            if (currentPacket != null)
            {
                //if (TCDAudio.PRF != currentPacket.parameter.PRF)
                //{
                //    TCDAudio.PRF = currentPacket.parameter.PRF;
                //    TCDAudio.soundBuffer = new byte[sizeof(short) * TCDAudio.PRF / 5];
                //}
                //TCDAudio.AudioCollection.Add(currentPacket);
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

                    }
                    NaGraph.ProcessPacket(packet, true, 1);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void InitializeBitmap()
        {
            int width = 500;
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

        private async void SendPower(object sender, RoutedEventArgs e)
        {
            using (TCDRequest requestObject = new TCDRequest())
            {
                requestObject.ChannelID = App.CurrentChannel;
                requestObject.Value3 = examViewModelObj.Power;
                await UsbTcd.TCDObj.SetPowerAsync(requestObject);
            }
        }

        private async void SendDepth(object sender, RoutedEventArgs e)
        {
            using (TCDRequest requestObject = new TCDRequest())
            {
                requestObject.ChannelID = App.CurrentChannel;
                requestObject.Value3 = examViewModelObj.Depth;
                await UsbTcd.TCDObj.SetDepthAsync(requestObject);
            }
        }

        private async void SendFilter(object sender, RoutedEventArgs e)
        {
            using (TCDRequest requestObject = new TCDRequest())
            {
                requestObject.ChannelID = App.CurrentChannel;
                requestObject.Value3 = examViewModelObj.Filter;
                await UsbTcd.TCDObj.SetFilterAsync(requestObject);
            }
        }

        private async void SendLength(object sender, RoutedEventArgs e)
        {
            using (TCDRequest requestObject = new TCDRequest())
            {
                requestObject.ChannelID = App.CurrentChannel;
                requestObject.Value3 = examViewModelObj.SVol;
                await UsbTcd.TCDObj.SetLengthAsync(requestObject);
            }
        }

        private async void SendPRF(object sender, RoutedEventArgs e)
        {
            using (TCDRequest requestObject = new TCDRequest())
            {
                requestObject.ChannelID = App.CurrentChannel;
                requestObject.Value3 = examViewModelObj.PRF;
                requestObject.Value2 = (byte)examViewModelObj.StartDepth;
                await UsbTcd.TCDObj.SetPRF(requestObject);
            }

        }
    }
}
