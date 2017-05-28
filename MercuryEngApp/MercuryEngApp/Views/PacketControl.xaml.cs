using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Numerics;
using System.IO;
using System.Globalization;
using System.Windows.Controls.Primitives;
using UsbTcdLibrary;
using MercuryEngApp.Common;
using UsbTcdLibrary.PacketFormats;
using log4net;
using UsbTcdLibrary.CommunicationProtocol;
using Core.Constants;
using Newtonsoft.Json;
using System.Xml;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for PacketControl.xaml
    /// </summary>
    public partial class PacketControl : UserControl
    {
        static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Windows.Forms.Timer GrabTimer;
        private List<string> TList = new List<string>();
        List<DMIPmdDataPacket> ListDMIPmdDataPacket;

        public PacketControl()
        {
            InitializeComponent();
            this.Loaded += PacketControlLoaded;
            this.Unloaded += PacketControlUnloaded;          
        }

        void PacketControlUnloaded(object sender, RoutedEventArgs e)
        {
            MainWindow.TurnTCDON -= MainWindowTurnTCDON;
            MainWindow.TurnTCDOFF -= MainWindowTurnTCDOFF;
            if ((bool)App.mainWindow.IsPowerChecked)
            {
                MainWindowTurnTCDOFF();
                App.mainWindow.IsPowerChecked = false;
            }
        }

        void PacketControlLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow.TurnTCDON += MainWindowTurnTCDON;
            MainWindow.TurnTCDOFF += MainWindowTurnTCDOFF;
        }

        void MainWindowTurnTCDOFF()
        {
            UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
            UsbTcd.TCDObj.TurnTCDPowerOff();
        }

        async void MainWindowTurnTCDON()
        {
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

                
                UsbTcd.TCDObj.StartTCDReading();
            }
        }

        void TCDObjOnPacketFormed(DMIPmdDataPacket[] packets)
        {
            ListDMIPmdDataPacket = new List<DMIPmdDataPacket>();
            //if (count<)
            {
                if (App.CurrentChannel == TCDHandles.Channel1)
                {
                    if (packets[0] != null)
                    {
                        ListDMIPmdDataPacket.Add(packets[0]);
                    }
                }
                else if (App.CurrentChannel == TCDHandles.Channel2)
                {
                    if (packets[1] != null)
                    {
                        ListDMIPmdDataPacket.Add(packets[1]);
                    }
                }
            }
            UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
        }

        private void LoadTreeView(byte[] byteArray)
        {
            logger.Debug("++");

            try
            {
                UsbTcd.TCDObj.GetPacketDetails(byteArray);
                //ListDMIPmdDataPacket = UsbTcd.TCDObj.PacketQueue[0];
                DMIPmdDataPacket dMIPmdDataPacket = UsbTcd.TCDObj.PacketQueueChannel1[0];
                ItemsMenu PacketRoot = new ItemsMenu();
                trvMenu.Items.Clear();
                //Root Item
                PacketRoot = GetMenuItem("Packet", 0, "packet","");
                PacketRoot.IsExpanded = true;

                DMIPktHeader dmiPktHeader = dMIPmdDataPacket.header;

                ItemsMenu ChildHeader = GetMenuItem("Header", ServiceHeader.sync, "header","");
                ChildHeader.Items.Add(GetMenuItem("Sync", ServiceHeader.sync, "long", Convert.ToString(dmiPktHeader.sync)));
                ChildHeader.Items.Add(GetMenuItem("SystemID", ServiceHeader.systemId, "byte", Convert.ToString(dmiPktHeader.systemID)));
                ChildHeader.Items.Add(GetMenuItem("DataSource", ServiceHeader.dataSource, "byte", Convert.ToString(dmiPktHeader.dataSource)));
                ChildHeader.Items.Add(GetMenuItem("MessageType", ServiceHeader.messageType, "byte", Convert.ToString(dmiPktHeader.messageType)));
                ChildHeader.Items.Add(GetMenuItem("MessageSubType", ServiceHeader.messageSubType, "byte", Convert.ToString(dmiPktHeader.messageSubType)));
                ChildHeader.Items.Add(GetMenuItem("DataLength", ServiceHeader.dataLength, "ushort", Convert.ToString(dmiPktHeader.dataLength)));
                ChildHeader.Items.Add(GetMenuItem("Sequence", ServiceHeader.sequence, "ushort", Convert.ToString(dmiPktHeader.sequence)));
                PacketRoot.Items.Add(ChildHeader);

                // 2nd Element
                PacketRoot.Items.Add(GetMenuItem("Reserved", Header.Reserved, "ushort", Convert.ToString(dMIPmdDataPacket.reserved)));

                //3rd Element
                PacketRoot.Items.Add(GetMenuItem("DataFormatREV", Header.DataFormatREV, "ushort", Convert.ToString(dMIPmdDataPacket.dataFormatRev)));

                DMIParameter dmiParameter = dMIPmdDataPacket.parameter;
                // 4th Element 
                ItemsMenu ChildParameter = GetMenuItem("Parameter", Parameter.LeftTimeStamp, "parameter", "");
                ChildParameter.Items.Add(GetMenuItem("TimestampL",Parameter.LeftTimeStamp, "uint",Convert.ToString(dmiParameter.timestampL)));
                ChildParameter.Items.Add(GetMenuItem("TimestampH", Parameter.RightTimeStamp, "uint", Convert.ToString(dmiParameter.timestampH)));
                ChildParameter.Items.Add(GetMenuItem("EventFlags", Parameter.EventFlags, "ushort", Convert.ToString(dmiParameter.eventFlags)));
                ChildParameter.Items.Add(GetMenuItem("OperatingState", Parameter.OperatingState, "byte", Convert.ToString(dmiParameter.operatingState)));
                ChildParameter.Items.Add(GetMenuItem("AcousticPower", Parameter.AcousingPower, "byte", Convert.ToString(dmiParameter.acousticPower)));
                ChildParameter.Items.Add(GetMenuItem("SampleLength", Parameter.SampleLength, "byte", Convert.ToString(dmiParameter.sampleLength)));
                ChildParameter.Items.Add(GetMenuItem("UserDepth", Parameter.UserDepth, "byte", Convert.ToString(dmiParameter.userDepth)));
                ChildParameter.Items.Add(GetMenuItem("PRF", Parameter.PRF, "ushort", Convert.ToString(dmiParameter.PRF)));
                ChildParameter.Items.Add(GetMenuItem("TIC", Parameter.TIC, "ushort", Convert.ToString(dmiParameter.TIC)));
                ChildParameter.Items.Add(GetMenuItem("RFU", Parameter.RFU, "ushort", Convert.ToString(dmiParameter.rfu)));
                PacketRoot.Items.Add(ChildParameter);

                PacketRoot.Items.Add(GetMenuItem("EmbCount", Parameter.EmboliCount, "uint", Convert.ToString(dMIPmdDataPacket.embCount)));

                DMIEnvelope dMIEnvelope = dMIPmdDataPacket.envelope;
                // 6th Element 
                ItemsMenu ChildEnvelop = GetMenuItem("Envelop", Envelop.Depth, "envelop", "");
                ChildEnvelop.Items.Add(GetMenuItem("Depth", Envelop.Depth, "ushort", Convert.ToString(dMIEnvelope.depth)));
                ChildEnvelop.Items.Add(GetMenuItem("VelocityUnits", Envelop.VelocityUnit, "ushort", Convert.ToString(dMIEnvelope.velocityUnits)));
                ChildEnvelop.Items.Add(GetMenuItem("ColIndexPos", Envelop.ColIndexPos, "ushort", Convert.ToString(dMIEnvelope.colIndexPos)));
                ChildEnvelop.Items.Add(GetMenuItem("PosVelocity", Envelop.PosVelocity, "short", Convert.ToString(dMIEnvelope.posVelocity)));
                ChildEnvelop.Items.Add(GetMenuItem("PosPEAK", Envelop.PosPeak, "short", Convert.ToString(dMIEnvelope.posPEAK)));
                ChildEnvelop.Items.Add(GetMenuItem("PosMEAN", Envelop.PosMean, "short", Convert.ToString(dMIEnvelope.posMEAN)));
                ChildEnvelop.Items.Add(GetMenuItem("PosDIAS", Envelop.PosDias, "short", Convert.ToString(dMIEnvelope.posDIAS)));
                ChildEnvelop.Items.Add(GetMenuItem("PosPI", Envelop.PosPI, "ushort", Convert.ToString(dMIEnvelope.posPI)));
                ChildEnvelop.Items.Add(GetMenuItem("PosRI", Envelop.PosRI, "ushort", Convert.ToString(dMIEnvelope.posRI)));
                ChildEnvelop.Items.Add(GetMenuItem("ColIndexNeg", Envelop.ColIndexNeg, "ushort", Convert.ToString(dMIEnvelope.colIndexNeg)));
                ChildEnvelop.Items.Add(GetMenuItem("NegVelocity", Envelop.NegVelocity, "ushort", Convert.ToString(dMIEnvelope.negVelocity)));
                ChildEnvelop.Items.Add(GetMenuItem("NegPEAK", Envelop.NegPeak, "short", Convert.ToString(dMIEnvelope.negPEAK)));
                ChildEnvelop.Items.Add(GetMenuItem("NegMEAN", Envelop.NegMean, "short", Convert.ToString(dMIEnvelope.negMEAN)));
                ChildEnvelop.Items.Add(GetMenuItem("NegDIAS", Envelop.NegDias, "short", Convert.ToString(dMIEnvelope.negDIAS)));
                ChildEnvelop.Items.Add(GetMenuItem("NegPI", Envelop.NegPI, "ushort", Convert.ToString(dMIEnvelope.negPI)));
                ChildEnvelop.Items.Add(GetMenuItem("NegRI", Envelop.NegRI, "ushort", Convert.ToString(dMIEnvelope.negRI)));
                PacketRoot.Items.Add(ChildEnvelop);

                DMISpectrum dMISpectrum = dMIPmdDataPacket.spectrum;
                // 7th Element 
                ItemsMenu ChildSpectrum = GetMenuItem("Spectrum", Spectrum.Depth, "spectrum","");
                ChildSpectrum.Items.Add(GetMenuItem("Depth", Spectrum.Depth, "ushort", Convert.ToString(dMISpectrum.depth)));
                ChildSpectrum.Items.Add(GetMenuItem("ClutterFilter", Spectrum.ClutterFilter, "ushort", Convert.ToString(dMISpectrum.clutterFilter)));
                ChildSpectrum.Items.Add(GetMenuItem("AutoGainOffset", Spectrum.AutoGainOffset, "short", Convert.ToString(dMISpectrum.autoGainOffset)));
                ChildSpectrum.Items.Add(GetMenuItem("StartVelocity", Spectrum.StartVelocity, "short", Convert.ToString(dMISpectrum.startVelocity)));
                ChildSpectrum.Items.Add(GetMenuItem("EndVelocity", Spectrum.EndVelocity, "short", Convert.ToString(dMISpectrum.endVelocity)));
                ChildSpectrum.Items.Add(GetMenuItem("PointsPerColumn", Spectrum.PointsPerColumn, "ushort", Convert.ToString(dMISpectrum.pointsPerColumn)));
                ChildSpectrum.Items.Add(GetMenuItem("Points", Spectrum.Points, "points", GetStringFromArray(dMISpectrum.points)));
                PacketRoot.Items.Add(ChildSpectrum);
                

                DMIMMode dMIMMode = dMIPmdDataPacket.mmode;
                // 8th Element 
                ItemsMenu ChildMmode = GetMenuItem("Mmode", MMode.AutoGainOffset, "Mmode","");
                ChildMmode.Items.Add(GetMenuItem("AutoGainOffset", MMode.AutoGainOffset, "short", Convert.ToString(dMIMMode.autoGainOffset)));
                ChildMmode.Items.Add(GetMenuItem("StartDepth", MMode.StartDepth, "ushort", Convert.ToString(dMIMMode.startDepth)));
                ChildMmode.Items.Add(GetMenuItem("EndDepth", MMode.EndDepth, "ushort", Convert.ToString(dMIMMode.endDepth)));
                ChildMmode.Items.Add(GetMenuItem("PointsPerColumn", MMode.PointsPerColumn, "ushort", Convert.ToString(dMIMMode.pointsPerColumn)));
                ChildMmode.Items.Add(GetMenuItem("Power", MMode.Power, "power", GetStringFromArray(dMIMMode.power)));
                ChildMmode.Items.Add(GetMenuItem("Velocity", MMode.Velocity, "velocity", GetStringFromArray(dMIMMode.velocity)));
                PacketRoot.Items.Add(ChildMmode);

                DMIAudio dMIAudio = dMIPmdDataPacket.audio;
                // 9rd Element 
                ItemsMenu ChildAudio = GetMenuItem("Audio", Audio.Depth, "audio","");
                ChildAudio.Items.Add(GetMenuItem("Depth", Audio.Depth, "ushort", Convert.ToString(dMIAudio.depth)));
                ChildAudio.Items.Add(GetMenuItem("RFU", Audio.RFU, "ushort", Convert.ToString(dMIAudio.rfu)));
                ChildAudio.Items.Add(GetMenuItem("SampleRate", Audio.SampleRate, "ushort", Convert.ToString(dMIAudio.sampleRate)));
                ChildAudio.Items.Add(GetMenuItem("MaxAmplitude", Audio.MaxAmplitude, "ushort", Convert.ToString(dMIAudio.maxAmplitude)));
                ChildAudio.Items.Add(GetMenuItem("Toward", Audio.Toward, "toward", GetStringFromArray(dMIAudio.toward)));
                ChildAudio.Items.Add(GetMenuItem("Away", Audio.Away, "away", GetStringFromArray(dMIAudio.away)));
                PacketRoot.Items.Add(ChildAudio);

                DMIEDetectResults dMIEDetectResults = dMIPmdDataPacket.edetectResults;
                // 10th Element 
                ItemsMenu ChildEdetect = GetMenuItem("EdetectResult", EDetect.PhaseA_MQ, "EdetectResult","");
                ItemsMenu phaseA = GetMenuItem("PhaseA", EDetect.PhaseA_MQ, "PhaseA", "");
                phaseA.Items.Add(GetMenuItem("MQ", EDetect.PhaseA_MQ, "uint", Convert.ToString(dMIEDetectResults.phaseA.MQ)));
                phaseA.Items.Add(GetMenuItem("ClutCount", EDetect.PhaseA_ClutCount, "uint", Convert.ToString(dMIEDetectResults.phaseA.ClutCount)));
                phaseA.Items.Add(GetMenuItem("MEPosition", EDetect.PhaseA_MEPosition, "uint", Convert.ToString(dMIEDetectResults.phaseA.MEPosition)));
                phaseA.Items.Add(GetMenuItem("MSum", EDetect.PhaseA_MSum, "uint", Convert.ToString(dMIEDetectResults.phaseA.MSum)));
                phaseA.Items.Add(GetMenuItem("MPLocal", EDetect.PhaseA_MPLocal, "uint", Convert.ToString(dMIEDetectResults.phaseA.MPLocal)));
                phaseA.Items.Add(GetMenuItem("AEFlag", EDetect.PhaseA_AEFlag, "uint", Convert.ToString(dMIEDetectResults.phaseA.AEFlag)));
                phaseA.Items.Add(GetMenuItem("AEDownCount", EDetect.PhaseA_AEDownCount, "uint", Convert.ToString(dMIEDetectResults.phaseA.AEDownCount)));
                phaseA.Items.Add(GetMenuItem("AEDetect", EDetect.PhaseA_AEDetect, "uint", Convert.ToString(dMIEDetectResults.phaseA.AEDetect)));
                ChildEdetect.Items.Add(phaseA);
                ItemsMenu phaseB = GetMenuItem("PhaseB", EDetect.PhaseB_MQ, "PhaseB", "");
                phaseB.Items.Add(GetMenuItem("MQ", EDetect.PhaseB_MQ, "uint", Convert.ToString(dMIEDetectResults.phaseB.MQ)));
                phaseB.Items.Add(GetMenuItem("ClutCount", EDetect.PhaseB_ClutCount, "uint", Convert.ToString(dMIEDetectResults.phaseB.ClutCount)));
                phaseB.Items.Add(GetMenuItem("MEPosition", EDetect.PhaseB_MEPosition, "uint", Convert.ToString(dMIEDetectResults.phaseB.MEPosition)));
                phaseB.Items.Add(GetMenuItem("MSum", EDetect.PhaseB_MSum, "uint", Convert.ToString(dMIEDetectResults.phaseB.MSum)));
                phaseB.Items.Add(GetMenuItem("MPLocal", EDetect.PhaseB_MPLocal, "uint", Convert.ToString(dMIEDetectResults.phaseB.MPLocal)));
                phaseB.Items.Add(GetMenuItem("AEFlag", EDetect.PhaseB_AEFlag, "uint", Convert.ToString(dMIEDetectResults.phaseB.AEFlag)));
                phaseB.Items.Add(GetMenuItem("AEDownCount", EDetect.PhaseB_AEDownCount, "uint", Convert.ToString(dMIEDetectResults.phaseB.AEDownCount)));
                phaseB.Items.Add(GetMenuItem("AEDetect", EDetect.PhaseB_AEDetect, "uint", Convert.ToString(dMIEDetectResults.phaseB.AEDetect)));
                ChildEdetect.Items.Add(phaseB);
                ChildEdetect.Items.Add(GetMenuItem("Edetect", EDetect.EDetectValue, "int", Convert.ToString(dMIEDetectResults.edetect)));
                PacketRoot.Items.Add(ChildEdetect);

                DMIArchive dMIArchive = dMIPmdDataPacket.archive;
                // 2nd Element
                ItemsMenu ChildArchive = GetMenuItem("Archive", Archive.TimeSeriesDepth, "Archive", ""); 
                ChildArchive.Items.Add(GetMenuItem("TimeseriesDepth", Archive.TimeSeriesDepth, "ushort", dMIArchive!=null ?Convert.ToString(dMIArchive.timeseriesDepth): ""));
                ChildArchive.Items.Add(GetMenuItem("Rfu", Archive.RFU, "ushort", dMIArchive!=null ?Convert.ToString(dMIArchive.rfu): ""));                
                ChildArchive.Items.Add(GetMenuItem("Timeseries", Archive.TimeseriesI, "ushort", dMIArchive!=null ?GetStringFromArray(dMIArchive.timeseries):""));
                ItemsMenu mmodeData = GetMenuItem("MmodeData", Archive.MmodePhaseA, "ArchiveMmode", "");
                mmodeData.Items.Add(GetMenuItem("MmodePhaseA", Archive.MmodePhaseA, "FloatMmode", dMIArchive != null ? GetStringFromArray(dMIArchive.mmodeData.mmodePhaseA) : ""));
                mmodeData.Items.Add(GetMenuItem("MmodePhaseB", Archive.mmodePhaseB, "FloatMmode", dMIArchive != null ? GetStringFromArray(dMIArchive.mmodeData.mmodePhaseB) : ""));
                mmodeData.Items.Add(GetMenuItem("MmodePowerA", Archive.mmodePowerA, "FloatMmode", dMIArchive != null ? GetStringFromArray(dMIArchive.mmodeData.mmodePowerA) : ""));
                mmodeData.Items.Add(GetMenuItem("MmodePowerB", Archive.mmodePowerB, "FloatMmode", dMIArchive != null ? GetStringFromArray(dMIArchive.mmodeData.mmodePowerB) : ""));
                ChildArchive.Items.Add(mmodeData);
                //Add to root
                PacketRoot.Items.Add(ChildArchive);
               
                // 4th Element
                PacketRoot.Items.Add(GetMenuItem("CheckSum", Checksum.ChecksumPos, "int", Convert.ToString(dMIPmdDataPacket.checksum)));
                trvMenu.Items.Add(PacketRoot);

                trvMenu.SelectedItemChanged += TreeItem_Selected;    
                

                logger.Debug("TreeView Created");
            }
            catch (Exception ex)
            {
                logger.Warn("Exception:" + ex);
            }

            logger.Debug("--");
        }

        public string GetStringFromArray<T>(IList<T> array)
        {
            string arrayValues = "[";

            for (int i = 0; i < 3; i++)
            {
                arrayValues = arrayValues + array[i] + ", ";
            }

            return arrayValues + array[3] + "..]";
        }

        void TreeItem_Selected(object sender, RoutedEventArgs e)
        {
            ItemsMenu item = (ItemsMenu)trvMenu.SelectedItem;
            item.IsExpanded = true;
            KeyValuePair<int, int> fromIndex;
            KeyValuePair<int, int> toIndex;
            fromIndex = new KeyValuePair<int, int>(item.StartRow, item.StartColumn);
            toIndex = new KeyValuePair<int, int>(item.EndRow, item.EndColumn);
            SelectCellsByIndexes(fromIndex, toIndex);
        }
        
        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvi = e.OriginalSource as TreeViewItem;
            ItemsMenu itemM =(ItemsMenu)tvi.Header;

            if (itemM != null)
            {
                if (!TList.Contains(itemM.Title))
                {
                    TList.Add(itemM.Title);
                }
                else
                {
                    TList.Remove(itemM.Title);
                }
            }
        }

        public bool IsItemExpanded(string header)
        {
            return TList.Contains(header);
        }

        public ItemsMenu GetMenuItem(string title, int postion, string type, string packetValue = "0")
        {
            ItemsMenu item = new ItemsMenu();

            item.Title = title;
            item.PakcetValue = packetValue;
            item.StartRow = postion / 16;
            item.StartColumn = (postion % 16) + 1;
            item.IsExpanded = IsItemExpanded(title);

            int byteSize = GetByteSize(type);

            item.EndRow = (postion + byteSize) / 16;
            item.EndColumn = ((postion + byteSize) % 16) + 1;

            return item;
        }

        public int GetByteSize(string type)
        {
            int byteSize = 0;

            switch (type)
            {
                case "packet":
                    byteSize = 1131;
                    break;
                case "int":
                    byteSize = 3;
                    break;
                case "uint":
                    byteSize = 3;
                    break;
                case "long":
                    byteSize = 7;
                    break;
                case "float":
                    byteSize = 3;
                    break;
                case "short":
                    byteSize = 1;
                    break;
                case "ushort":
                    byteSize = 1;
                    break;
                case "byte":
                    byteSize = 0;
                    break;
                case "header":
                    byteSize = ServiceHeader.sequence + 1; //last element postion + byte size
                    break;
                case "points":
                    byteSize = (DMIProtocol.SpectrumPointsCount * 2) - 1;
                    break;
                case "audio":
                    byteSize = (Audio.Away - 1) + (DMIProtocol.DMI_AUDIO_ARRAY_SIZE * 2) - Audio.Depth;
                    break;
                case "toward":
                    byteSize = (DMIProtocol.DMI_AUDIO_ARRAY_SIZE * 2) - 1;
                    break;
                case "away":
                    byteSize = (DMIProtocol.DMI_AUDIO_ARRAY_SIZE * 2) - 1;
                    break;
                case "envelop":
                    byteSize = Envelop.NegRI + 1 - Envelop.Depth;
                    break;
                case "Mmode":
                    byteSize = (MMode.Velocity - 1) + (DMIProtocol.DMI_PKT_MMODE_PTS * 2) - MMode.AutoGainOffset;
                    break;
                case "power":
                    byteSize = (DMIProtocol.DMI_PKT_MMODE_PTS * 2) - 1;
                    break;
                case "velocity":
                    byteSize = (DMIProtocol.DMI_PKT_MMODE_PTS * 2) - 1;
                    break;
                case "parameter":
                    byteSize = Parameter.RFU + 1 - Parameter.LeftTimeStamp;
                    break;
                case "spectrum":
                    byteSize = (Spectrum.Points - 1) + (DMIProtocol.SpectrumPointsCount * 2) - Spectrum.Depth;
                    break;
                case "EdetectResult":
                    byteSize = EDetect.EDetectValue + 3 - EDetect.PhaseA_MQ;
                    break;
                case "PhaseA":
                    byteSize = EDetect.PhaseA_AEDetect + 3 - EDetect.PhaseA_MQ;
                    break;
                case "PhaseB":
                    byteSize = EDetect.PhaseB_AEDetect + 3 - EDetect.PhaseB_MQ;
                    break;
                case "Archive":
                    byteSize = (Archive.mmodePowerB - 1) + (DMIProtocol.DMI_ARCHIVE_MMODE_GATES * 4) - Archive.TimeSeriesDepth;
                    break;
                case "ArchiveMmode":
                    byteSize = (Archive.mmodePowerB - 1) + (DMIProtocol.DMI_ARCHIVE_MMODE_GATES * 4) - Archive.MmodePhaseA;
                    break;
                case "FloatMmode":
                    byteSize = (DMIProtocol.DMI_ARCHIVE_MMODE_GATES * 4) - 1;
                    break;     
                default:
                    break;
            }

            return byteSize;
        }

        public void LoadBinaryData(byte[] byteArray)
        {

            logger.Debug("++");

            int hexIn;           
            int count = 1;
            ObservableCollection<HexRecord> listHexRecord = new ObservableCollection<HexRecord>();
            StringBuilder sb = new StringBuilder();
            BigInteger InitailBInt = BigInteger.Parse("00000000", NumberStyles.HexNumber);
            BigInteger ConstantBInt = BigInteger.Parse("00000010", NumberStyles.HexNumber);
            HexRecord hexRecord = null;

            try
            {

                for (int i = 0; i < byteArray.Length; i++)
                {
                    hexIn = byteArray[i];

                    if (count == 1)
                    {
                        hexRecord = new HexRecord();
                    }

                    switch (count)
                    {
                        case 1:
                            hexRecord.Hx_00 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 2:
                            hexRecord.Hx_01 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 3:
                            hexRecord.Hx_02 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 4:
                            hexRecord.Hx_03 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 5:
                            hexRecord.Hx_04 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 6:
                            hexRecord.Hx_05 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 7:
                            hexRecord.Hx_06 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 8:
                            hexRecord.Hx_07 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 9:
                            hexRecord.Hx_08 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 10:
                            hexRecord.Hx_09 = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 11:
                            hexRecord.Hx_0a = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 12:
                            hexRecord.Hx_0b = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 13:
                            hexRecord.Hx_0c = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 14:
                            hexRecord.Hx_0d = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 15:
                            hexRecord.Hx_0e = string.Format("{0:X2}", hexIn);
                            count++;
                            break;
                        case 16:
                            hexRecord.Hx_0f = string.Format("{0:X2}", hexIn);
                            hexRecord.Offset = string.Format("{0:X8}", InitailBInt);
                            InitailBInt = BigInteger.Add(InitailBInt, ConstantBInt);
                            listHexRecord.Add(hexRecord);
                            count = 1;
                            break;

                    }
                }

                if (byteArray.Length % 16 != 0)
                {
                    hexRecord.Offset = string.Format("{0:X8}", InitailBInt);
                    listHexRecord.Add(hexRecord);
                }

                this.Dispatcher.Invoke(() =>
                {
                    //link business data to CollectionViewSource
                    CollectionViewSource itemCollectionViewSource = new CollectionViewSource();
                    itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
                    itemCollectionViewSource.Source = listHexRecord;
                });
            }
            catch (Exception ex)
            {
                logger.Warn("Exception:" + ex);
            }

            logger.Debug("--");
        }

        private void SelectCellsByIndexes(KeyValuePair<int, int> fromIndex, KeyValuePair<int, int> toIndex)
        {
            grdMailbag.SelectedItems.Clear();
            grdMailbag.SelectedCells.Clear();
            int fromRowIndex = fromIndex.Key;
            int fromColumnIndex = fromIndex.Value;
            int toRowIndex = toIndex.Key;
            int toColumnIndex = toIndex.Value;

            for (int i = fromRowIndex; i <= toRowIndex; i++)
            {
                int rowIndex = i;
                int columnIndex = 1;
                if (rowIndex == fromRowIndex)
                {
                    columnIndex = fromColumnIndex;
                }
                else
                {
                    columnIndex = 1;
                }

                while (columnIndex < grdMailbag.Columns.Count)
                {
                    if (rowIndex == toRowIndex && columnIndex > toColumnIndex)
                    {
                        break;
                    }
                    SelectCellByIndex(rowIndex, columnIndex);
                    columnIndex++;
                }
            }
        }

        public void SelectCellByIndex(int rowIndex, int columnIndex)
        {
            object item = grdMailbag.Items[rowIndex]; //=Product X
            DataGridRow row = grdMailbag.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                grdMailbag.ScrollIntoView(item);
                row = grdMailbag.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            if (row != null)
            {
                DataGridCell cell = GetCell(grdMailbag, row, columnIndex);
                if (cell != null)
                {
                    DataGridCellInfo dataGridCellInfo = new DataGridCellInfo(cell);
                    if (!grdMailbag.SelectedCells.Contains(dataGridCellInfo))
                    {
                        grdMailbag.SelectedCells.Add(dataGridCellInfo);
                        //cell.Focus();
                    }
                }
            }
        }

        public DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    /* if the row has been virtualized away, call its ApplyTemplate() method
                     * to build its visual tree in order for the DataGridCellsPresenter
                     * and the DataGridCells to be created */
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        /* bring the column into view
                         * in case it has been virtualized away */
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }

        public T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }       

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ClearRecentTimer();
            IntervalSilder.Value = 0;
            List<byte[]> byteArray = UsbTcd.TCDObj.GrabPacket();
            ReloadHexViewer(byteArray);
        }

        public void ReloadHexViewer(List<byte[]> byteArray)
        {
            logger.Debug("++");

            try
            {                
                if (byteArray != null)
                {
                    if (App.CurrentChannel == TCDHandles.Channel1)
                    {
                        LoadTreeView(byteArray[0]);
                        LoadBinaryData(byteArray[0]);
                    }

                    if (App.CurrentChannel == TCDHandles.Channel2)
                    {
                        LoadTreeView(byteArray[1]);
                        LoadBinaryData(byteArray[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }

            logger.Debug("--");
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> er)
        {
            int sliderValue = (int)IntervalSilder.Value;

            if (sliderValue != 0)
            {
                ClearRecentTimer();
                GrabTimer = new System.Windows.Forms.Timer();
                GrabTimer.Tick += new EventHandler(GrabPacket);
                GrabTimer.Interval = sliderValue * 1000; // in miliseconds
                GrabTimer.Start();
            }
            else
            {
                ClearRecentTimer();
                IntervalSilder.Value = 0;                
            }
        }

        private void ClearRecentTimer()
        {
            if (GrabTimer != null)
            {
                GrabTimer.Stop();
                GrabTimer.Dispose();
            }
        }

        private void GrabPacket(object sender, EventArgs e)
        {
            logger.Debug("++");

            try
            {
                List<byte[]> byteArray = UsbTcd.TCDObj.GrabPacket();
                ReloadHexViewer(byteArray);
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }

            logger.Debug("--");
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            logger.Debug("++");

            try
            {
                UsbTcd.TCDObj.OnPacketFormed += TCDObjOnPacketFormed;

                string jsonFileName = "PacketJson" + Convert.ToString(App.CurrentChannel) + ".txt";
                string xmlFileName = "Packetxml" + Convert.ToString(App.CurrentChannel) + ".xml";
                

                string json = JsonConvert.SerializeObject(ListDMIPmdDataPacket);
                string jsonFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"LocalFolder\" + jsonFileName);
                System.IO.File.WriteAllText(jsonFilePath, json);
                XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode("{\"Row\":" + json + "}", "root");
                string xmlFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"LocalFolder\" + xmlFileName);
                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: " + ex);
            }

            logger.Debug("--");
        }
    }

    public class HexRecord
    {
        public string Offset { get; set; }
        public string Hx_00 { get; set; }
        public string Hx_01 { get; set; }
        public string Hx_02 { get; set; }
        public string Hx_03 { get; set; }
        public string Hx_04 { get; set; }
        public string Hx_05 { get; set; }
        public string Hx_06 { get; set; }
        public string Hx_07 { get; set; }

        public string Hx_08 { get; set; }
        public string Hx_09 { get; set; }
        public string Hx_0a { get; set; }
        public string Hx_0b { get; set; }
        public string Hx_0c { get; set; }
        public string Hx_0d { get; set; }
        public string Hx_0e { get; set; }
        public string Hx_0f { get; set; }

    }

    public class ItemsMenu
    {
        public ItemsMenu()
        {
            this.Items = new ObservableCollection<ItemsMenu>();
        }

        public string Title { get; set; }
        public string PakcetValue { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int EndRow { get; set; }
        public int EndColumn { get; set; }

        public bool IsExpanded { get; set; }
        public bool Collapsed { get; set; }

        public ObservableCollection<ItemsMenu> Items { get; set; }

    }
}
