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
using Core.Common;

namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for PacketControl.xaml
    /// </summary>
    public partial class PacketControl : UserControl
    {
        private System.Windows.Forms.Timer GrabTimer;
        private PacketViewModel packetViewModelObj = new PacketViewModel();
        private List<string> TList = new List<string>();
        List<DMIPmdDataPacket> ListDMIPmdDataPacket;
        int count = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public PacketControl()
        {
            
            InitializeComponent();
            this.Loaded += PacketControlLoaded;
            this.Unloaded += PacketControlUnloaded;
            this.DataContext = packetViewModelObj;
        }

        /// <summary>
        /// TCDObjOnProbeUnplugged event
        /// </summary>
        /// <param name="probe"></param>
        public PacketControl(bool IsTest)
        {

        }

        void TCDObjOnProbeUnplugged(TCDHandles probe)
        {
            if (probe == App.CurrentChannel)
            {
                LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ProbeDisconnected);
                MainWindowTurnTCDOFF();
            }
        }

        /// <summary>
        /// Screen Unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PacketControlUnloaded(object sender, RoutedEventArgs e)
        {
            ClearRecentTimer();
            IntervalSilder.Value = 0;
            MainWindow.TurnTCDON -= MainWindowTurnTCDON;
            MainWindow.TurnTCDOFF -= MainWindowTurnTCDOFF;
            UsbTcd.TCDObj.OnProbeUnplugged -= TCDObjOnProbeUnplugged;
            if ((bool)App.mainWindow.IsPowerChecked)
            {
                MainWindowTurnTCDOFF();
                App.mainWindow.IsPowerChecked = false;
            }

            App.mainWindow.IsProbe1HitTestVisible = true;
            App.mainWindow.IsProbe2HitTestVisible = true;
        }

        /// <summary>
        /// Screen Loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PacketControlLoaded(object sender, RoutedEventArgs e)
        {
            MainWindow.TurnTCDON += MainWindowTurnTCDON;
            MainWindow.TurnTCDOFF += MainWindowTurnTCDOFF;

            UsbTcd.TCDObj.OnProbeUnplugged += TCDObjOnProbeUnplugged;
            if (!(bool)App.mainWindow.IsPowerChecked)
            {
                this.IsEnabled = false;
            }
        }

        /// <summary>
        /// TCD sets to off from MainWindow
        /// </summary>
        void MainWindowTurnTCDOFF()
        {
            UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;
            UsbTcd.TCDObj.TurnTCDPowerOff();
            this.IsEnabled = false;
        }

        /// <summary>
        /// TCD On from Main window
        /// </summary>
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

                
                UsbTcd.TCDObj.StartTCDReading(Constants.PacketPage);
                this.IsEnabled = true;
            }
        }

        /// <summary>
        /// Event for TCD Apcket Formed
        /// </summary>
        /// <param name="packets"></param>
        void TCDObjOnPacketFormed(DMIPmdDataPacket[] packets)
        {
            Helper.logger.Debug("++");

            try
            {
                if (packetViewModelObj.PacketNumber > count)
                {  
                    if (App.CurrentChannel == TCDHandles.Channel1)
                    {
                        if (packets[0] != null)
                        {
                            ListDMIPmdDataPacket.Add(packets[0]);
                        }
                    }
                    else 
                    {
                        if (App.CurrentChannel == TCDHandles.Channel2 && packets[1] != null)
                        {
                            ListDMIPmdDataPacket.Add(packets[1]);
                        }
                    }

                    count++;
                }

                if (count == packetViewModelObj.PacketNumber)
                {
                    UsbTcd.TCDObj.OnPacketFormed -= TCDObjOnPacketFormed;

                    string jsonFileName = "PacketJson" + Convert.ToString(App.CurrentChannel) + ".txt";
                    string xmlFileName = "Packetxml" + Convert.ToString(App.CurrentChannel) + ".xml";


                    string json = JsonConvert.SerializeObject(ListDMIPmdDataPacket);

                    string folderFile = System.IO.Path.Combine(Environment.CurrentDirectory, @"AppData\ExportFiles");

                    if(!Directory.Exists(folderFile))
                    {
                        Directory.CreateDirectory(folderFile);
                    }

                    string jsonFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"AppData\ExportFiles\" + jsonFileName);
                    System.IO.File.WriteAllText(jsonFilePath, json);
                    XmlDocument xmlDoc = JsonConvert.DeserializeXmlNode("{\"Packet\":" + json + "}", "root");
                    string xmlFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"AppData\ExportFiles\" + xmlFileName);
                    xmlDoc.Save(xmlFilePath);
                    count = 0;
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.PacketExport);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception:" + ex);
            }

            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Load the Tree view from byte array
        /// </summary>
        /// <param name="byteArray"></param>
        private void LoadTreeView(byte[] byteArray)
        {
            Helper.logger.Debug("++");

            try
            {
                trvMenu.Items.Clear();

                ItemsMenu PacketRoot = GetTreeStructure(byteArray);
                trvMenu.Items.Add(PacketRoot);

                Helper.logger.Debug("TreeView Created");
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception:" + ex);
            }

            Helper.logger.Debug("--");
        }

        public ItemsMenu GetTreeStructure(byte[] byteArray)
        {
            ItemsMenu PacketRoot = new ItemsMenu();
            if (byteArray != null)
            {
                DMIPmdDataPacket dMIPmdDataPacket = UsbTcd.TCDObj.GetPacketDetails(byteArray);

                //Root Item
                PacketRoot = GetMenuItem("Packet", 0, "packet", "");
                PacketRoot.IsExpanded = true;

                DMIPktHeader dmiPktHeader = dMIPmdDataPacket.header;

                ItemsMenu ChildHeader = GetMenuItem("Header", ServiceHeader.sync, "header", "");
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
                ChildParameter.Items.Add(GetMenuItem("TimestampL", Parameter.LeftTimeStamp, "uint", Convert.ToString(dmiParameter.timestampL)));
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
                ItemsMenu ChildSpectrum = GetMenuItem("Spectrum", Spectrum.Depth, "spectrum", "");
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
                ItemsMenu ChildMmode = GetMenuItem("Mmode", MMode.AutoGainOffset, "Mmode", "");
                ChildMmode.Items.Add(GetMenuItem("AutoGainOffset", MMode.AutoGainOffset, "short", Convert.ToString(dMIMMode.autoGainOffset)));
                ChildMmode.Items.Add(GetMenuItem("StartDepth", MMode.StartDepth, "ushort", Convert.ToString(dMIMMode.startDepth)));
                ChildMmode.Items.Add(GetMenuItem("EndDepth", MMode.EndDepth, "ushort", Convert.ToString(dMIMMode.endDepth)));
                ChildMmode.Items.Add(GetMenuItem("PointsPerColumn", MMode.PointsPerColumn, "ushort", Convert.ToString(dMIMMode.pointsPerColumn)));
                ChildMmode.Items.Add(GetMenuItem("Power", MMode.Power, "power", GetStringFromArray(dMIMMode.power)));
                ChildMmode.Items.Add(GetMenuItem("Velocity", MMode.Velocity, "velocity", GetStringFromArray(dMIMMode.velocity)));
                PacketRoot.Items.Add(ChildMmode);

                DMIAudio dMIAudio = dMIPmdDataPacket.audio;
                // 9rd Element 
                ItemsMenu ChildAudio = GetMenuItem("Audio", Audio.Depth, "audio", "");
                ChildAudio.Items.Add(GetMenuItem("Depth", Audio.Depth, "ushort", Convert.ToString(dMIAudio.depth)));
                ChildAudio.Items.Add(GetMenuItem("RFU", Audio.RFU, "ushort", Convert.ToString(dMIAudio.rfu)));
                ChildAudio.Items.Add(GetMenuItem("SampleRate", Audio.SampleRate, "ushort", Convert.ToString(dMIAudio.sampleRate)));
                ChildAudio.Items.Add(GetMenuItem("MaxAmplitude", Audio.MaxAmplitude, "ushort", Convert.ToString(dMIAudio.maxAmplitude)));
                ChildAudio.Items.Add(GetMenuItem("Toward", Audio.Toward, "toward", GetStringFromArray(dMIAudio.toward)));
                ChildAudio.Items.Add(GetMenuItem("Away", Audio.Away, "away", GetStringFromArray(dMIAudio.away)));
                PacketRoot.Items.Add(ChildAudio);

                DMIEDetectResults dMIEDetectResults = dMIPmdDataPacket.edetectResults;
                // 10th Element 
                ItemsMenu ChildEdetect = GetMenuItem("EdetectResult", EDetect.PhaseA_MQ, "EdetectResult", "");
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
                ChildArchive.Items.Add(GetMenuItem("TimeseriesDepth", Archive.TimeSeriesDepth, "ushort", dMIArchive != null ? Convert.ToString(dMIArchive.timeseriesDepth) : ""));
                ChildArchive.Items.Add(GetMenuItem("Rfu", Archive.RFU, "ushort", dMIArchive != null ? Convert.ToString(dMIArchive.rfu) : ""));
                ChildArchive.Items.Add(GetMenuItem("Timeseries", Archive.TimeseriesI, "ushort", dMIArchive != null ? GetStringFromArray(dMIArchive.timeseries) : ""));
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
            }
            return PacketRoot;
        }

        /// <summary>
        /// Get the strring from array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public string GetStringFromArray<T>(IList<T> array)
        {
            string arrayValues = "[";

            for (int i = Constants.VALUE_0; i < Constants.VALUE_3; i++)
            {
                arrayValues = arrayValues + array[i] + ", ";
            }

            return arrayValues + array[Constants.VALUE_3] + "..]";
        }

        /// <summary>
        /// Calls when the Tree item  is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeItemSelected(object sender, RoutedEventArgs e)
        {
            if (trvMenu.SelectedItem != null)
            {
                ItemsMenu item = (ItemsMenu)trvMenu.SelectedItem;
                item.IsExpanded = true;
                KeyValuePair<int, int> fromIndex;
                KeyValuePair<int, int> toIndex;
                fromIndex = new KeyValuePair<int, int>(item.StartRow, item.StartColumn);
                toIndex = new KeyValuePair<int, int>(item.EndRow, item.EndColumn);
                SelectCellsByIndexes(fromIndex, toIndex);
            }
        }
        
        /// <summary>
        /// Called when Tree Item is expanded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItemExpanded(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Is Item Expanded
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public bool IsItemExpanded(string header)
        {
            return TList.Contains(header);
        }

        /// <summary>
        /// Gets the Items Menu with details
        /// </summary>
        /// <param name="title"></param>
        /// <param name="postion"></param>
        /// <param name="type"></param>
        /// <param name="packetValue"></param>
        /// <returns></returns>
        private ItemsMenu GetMenuItem(string title, int postion, string type, string packetValue = "0")
        {
            ItemsMenu item = new ItemsMenu();

            item.Title = title;
            item.PakcetValue = packetValue;
            item.StartRow = postion / Constants.NumberOfCloumn;
            item.StartColumn = (postion % Constants.NumberOfCloumn) + Constants.VALUE_1;
            item.IsExpanded = IsItemExpanded(title);

            int byteSize = GetByteSize(type);

            item.EndRow = (postion + byteSize) / Constants.NumberOfCloumn;
            item.EndColumn = ((postion + byteSize) % Constants.NumberOfCloumn) + Constants.VALUE_1;

            return item;
        }

        /// <summary>
        /// Get the Byte Size of type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int GetByteSize(string type)
        {
            int byteSize = Constants.VALUE_0;

            switch (type)
            {
                case "packet":
                    byteSize = DMIProtocol.PACKET_SIZE - 1;
                    break;
                case "int":
                    byteSize = Constants.VALUE_3;
                    break;
                case "uint":
                    byteSize = Constants.VALUE_3;
                    break;
                case "long":
                    byteSize = Constants.VALUE_7;
                    break;
                case "float":
                    byteSize = Constants.VALUE_3;
                    break;
                case "short":
                    byteSize = Constants.VALUE_1;
                    break;
                case "ushort":
                    byteSize = Constants.VALUE_1;
                    break;
                case "byte":
                    byteSize = Constants.VALUE_0;
                    break;
                case "header":
                    byteSize = ServiceHeader.sequence + Constants.VALUE_1; //last element postion + byte size
                    break;
                case "points":
                    byteSize = (DMIProtocol.SpectrumPointsCount * Constants.VALUE_2) - Constants.VALUE_1;
                    break;
                case "audio":
                    byteSize = (Audio.Away - Constants.VALUE_1) + (DMIProtocol.DMI_AUDIO_ARRAY_SIZE * Constants.VALUE_2) - Audio.Depth;
                    break;
                case "toward":
                    byteSize = (DMIProtocol.DMI_AUDIO_ARRAY_SIZE * Constants.VALUE_2) - Constants.VALUE_1;
                    break;
                case "away":
                    byteSize = (DMIProtocol.DMI_AUDIO_ARRAY_SIZE * Constants.VALUE_2) - Constants.VALUE_1;
                    break;
                case "envelop":
                    byteSize = Envelop.NegRI + Constants.VALUE_1 - Envelop.Depth;
                    break;
                case "Mmode":
                    byteSize = (MMode.Velocity - Constants.VALUE_1) + (DMIProtocol.DMI_PKT_MMODE_PTS * Constants.VALUE_2) - MMode.AutoGainOffset;
                    break;
                case "power":
                    byteSize = (DMIProtocol.DMI_PKT_MMODE_PTS * Constants.VALUE_2) - Constants.VALUE_1;
                    break;
                case "velocity":
                    byteSize = (DMIProtocol.DMI_PKT_MMODE_PTS * Constants.VALUE_2) - Constants.VALUE_1;
                    break;
                case "parameter":
                    byteSize = Parameter.RFU + Constants.VALUE_1 - Parameter.LeftTimeStamp;
                    break;
                case "spectrum":
                    byteSize = (Spectrum.Points - Constants.VALUE_1) + (DMIProtocol.SpectrumPointsCount * Constants.VALUE_2) - Spectrum.Depth;
                    break;
                case "EdetectResult":
                    byteSize = EDetect.EDetectValue + Constants.VALUE_3 - EDetect.PhaseA_MQ;
                    break;
                case "PhaseA":
                    byteSize = EDetect.PhaseA_AEDetect + Constants.VALUE_3 - EDetect.PhaseA_MQ;
                    break;
                case "PhaseB":
                    byteSize = EDetect.PhaseB_AEDetect + Constants.VALUE_3 - EDetect.PhaseB_MQ;
                    break;
                case "Archive":
                    byteSize = (Archive.mmodePowerB - Constants.VALUE_1) + (DMIProtocol.DMI_ARCHIVE_MMODE_GATES * Constants.VALUE_4) - Archive.TimeSeriesDepth;
                    break;
                case "ArchiveMmode":
                    byteSize = (Archive.mmodePowerB - Constants.VALUE_1) + (DMIProtocol.DMI_ARCHIVE_MMODE_GATES * Constants.VALUE_4) - Archive.MmodePhaseA;
                    break;
                case "FloatMmode":
                    byteSize = (DMIProtocol.DMI_ARCHIVE_MMODE_GATES * Constants.VALUE_4) - Constants.VALUE_1;
                    break;     
                default:
                    break;
            }

            return byteSize;
        }

        /// <summary>
        /// Load the Binary data from byte array
        /// </summary>
        /// <param name="byteArray"></param>
        private void LoadBinaryData(byte[] byteArray)
        {

            Helper.logger.Debug("++");

            try
            {

                ObservableCollection<HexRecord> listHexRecord = GetBinaryData(byteArray);    

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
                Helper.logger.Warn("Exception:" + ex);
            }

            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Selects Cell by Indexes
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="toIndex"></param>
        public ObservableCollection<HexRecord> GetBinaryData(byte[] byteArray)
        {
            int hexIn;
            int count = Constants.VALUE_1;
            ObservableCollection<HexRecord> listHexRecord = new ObservableCollection<HexRecord>();           
            HexRecord hexRecord = null;
            BigInteger InitailBInt = BigInteger.Parse("00000000", NumberStyles.HexNumber);
            BigInteger ConstantBInt = BigInteger.Parse("00000010", NumberStyles.HexNumber);

            for (int i = Constants.VALUE_0; i < byteArray.Length; i++)
            {
                hexIn = byteArray[i];

                if (count == Constants.VALUE_1)
                {
                    hexRecord = new HexRecord();
                }

                switch (count)
                {
                    case Constants.VALUE_1:
                        hexRecord.Hx_00 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_2:
                        hexRecord.Hx_01 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_3:
                        hexRecord.Hx_02 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_4:
                        hexRecord.Hx_03 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_5:
                        hexRecord.Hx_04 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_6:
                        hexRecord.Hx_05 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_7:
                        hexRecord.Hx_06 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_8:
                        hexRecord.Hx_07 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_9:
                        hexRecord.Hx_08 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_10:
                        hexRecord.Hx_09 = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_11:
                        hexRecord.Hx_0a = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_12:
                        hexRecord.Hx_0b = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_13:
                        hexRecord.Hx_0c = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_14:
                        hexRecord.Hx_0d = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_15:
                        hexRecord.Hx_0e = string.Format("{0:X2}", hexIn);
                        count++;
                        break;
                    case Constants.VALUE_16:
                        hexRecord.Hx_0f = string.Format("{0:X2}", hexIn);
                        hexRecord.Offset = string.Format("{0:X8}", InitailBInt);
                        InitailBInt = BigInteger.Add(InitailBInt, ConstantBInt);
                        listHexRecord.Add(hexRecord);
                        count = Constants.VALUE_1;
                        break;
                    default:
                        break;
                }
            }

            if (byteArray.Length % Constants.NumberOfCloumn != Constants.VALUE_0)
            {
                hexRecord.Offset = string.Format("{0:X8}", InitailBInt);
                listHexRecord.Add(hexRecord);
            }
            return listHexRecord;
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

        /// <summary>
        /// Selects Cell by Index
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        private void SelectCellByIndex(int rowIndex, int columnIndex)
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
                    }
                }
            }
        }

        /// <summary>
        /// Get the cell from the datagrid
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="rowContainer"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
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

        /// <summary>
        /// Find Visual Child of grid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = Constants.VALUE_0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);

                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }       

        /// <summary>
        /// Called when refresh clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            ClearRecentTimer();
            IntervalSilder.Value = 0;
            List<byte[]> byteArray = UsbTcd.TCDObj.GrabPacket();
            ReloadHexViewer(byteArray);
        }

        /// <summary>
        /// Reaload the Hex Viewer
        /// </summary>
        /// <param name="byteArray"></param>
        public async void ReloadHexViewer(List<byte[]> byteArray)
        {
            Helper.logger.Debug("++");

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
                else
                {
                    LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.ProbeDisconnected);
                    ClearRecentTimer();
                    MainWindowTurnTCDOFF();
                    App.mainWindow.IsPowerChecked = false;
                    App.mainWindow.TCDObjOnProbeUnplugged(App.CurrentChannel);
                    await PowerController.Instance.UpdatePowerParameters(true, true, false, false, true);
                    await Task.Delay(Constants.TimeForTCDtoLoad);
                    App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }

            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Called when Slider value is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="er"></param>
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> er)
        {
            double sliderValue = (double)IntervalSilder.Value;

            if (sliderValue != Constants.VALUE_0)
            {
                ClearRecentTimer();
                GrabTimer = new System.Windows.Forms.Timer();
                GrabTimer.Tick += new EventHandler(GrabPacket);
                GrabTimer.Interval = Convert.ToInt16(sliderValue * Constants.VALUE_1000); // in miliseconds
                GrabTimer.Start();
            }
            else
            {
                ClearRecentTimer();
                IntervalSilder.Value = Constants.VALUE_0;                
            }
        }

        /// <summary>
        /// Clear the timer
        /// </summary>
        private void ClearRecentTimer()
        {
            if (GrabTimer != null)
            {
                GrabTimer.Stop();
                GrabTimer.Dispose();
            }
        }

        /// <summary>
        /// Grab the Packet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrabPacket(object sender, EventArgs e)
        {
            Helper.logger.Debug("++");
            try
            {
                List<byte[]> byteArray = UsbTcd.TCDObj.GrabPacket();
                ReloadHexViewer(byteArray);
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
            }

            Helper.logger.Debug("--");
        }

        //Called when Export isa clicked
        private void ExportClick(object sender, RoutedEventArgs e)
        {
            Helper.logger.Debug("++");
            string errorMessage = string.Empty;
            try
            {
                if (ValidatePacketExport(out errorMessage))
                {
                    if (packetViewModelObj.PacketNumber != 0)
                    {
                        ListDMIPmdDataPacket = new List<DMIPmdDataPacket>();
                        UsbTcd.TCDObj.OnPacketFormed += TCDObjOnPacketFormed;
                    }
                    else
                    {
                        LogWrapper.Log(Constants.APPLog, MercuryEngApp.Resources.NoOfOPacketsNonZero);
                    }
                }
                else
                {
                    LogWrapper.Log(Constants.APPLog, errorMessage);
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: " + ex);
            }

            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Validate the Export of Packet
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool ValidatePacketExport(out string errorMessage)
        {
            bool isValid = true;
            List<DependencyObject> objects = new List<DependencyObject>();
            objects.Add(NoOfPacket);

            isValid = ValidateObjects(objects, out errorMessage);
            return isValid;
        }

        /// <summary>
        /// Validate Objects
        /// </summary>
        /// <param name="objects"></param>
        /// <param name="validationMessage"></param>
        /// <returns></returns>
        private bool ValidateObjects(List<DependencyObject> objects, out string validationMessage)
        {
            bool isValid = true;
            string errorMessage = string.Empty;
            StringBuilder message = new StringBuilder();
            int errorCount = 0;

            foreach (DependencyObject obj in objects)
            {
                isValid = Validators.ValidationRules.ValidateControl(obj, out errorMessage);
                if (!isValid)
                {
                    errorCount++;
                    message.Append(errorMessage);
                }
            }

            validationMessage = message.ToString();
            return errorCount > 0 ? false : true;
        }
    }
}
