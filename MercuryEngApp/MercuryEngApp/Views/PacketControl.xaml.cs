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


namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for PacketControl.xaml
    /// </summary>
    public partial class PacketControl : UserControl
    {
        public bool IsManualClickSelect = true;
        public PacketControl()
        {
            InitializeComponent();
            LoadTreeView();
            LoadBinaryData();
        }

        private void LoadTreeView()
        {
            //Root Item
            ItemsMenu PacketRoot = GetMenuItem("Packet", 0, "packet");
            PacketRoot.IsExpanded = true;
            // 1th Element 
            ItemsMenu ChildHeader = GetMenuItem("Header", ServiceHeader.sync, "header");
            ChildHeader.Items.Add(GetMenuItem("Sync", ServiceHeader.sync, "long"));
            ChildHeader.Items.Add(GetMenuItem("SystemID", ServiceHeader.systemId, "byte"));
            ChildHeader.Items.Add(GetMenuItem("DataSource", ServiceHeader.dataSource, "byte"));
            ChildHeader.Items.Add(GetMenuItem("MessageType", ServiceHeader.messageType, "byte"));
            ChildHeader.Items.Add(GetMenuItem("MessageSubType", ServiceHeader.messageSubType, "byte"));
            ChildHeader.Items.Add(GetMenuItem("DataLength", ServiceHeader.dataLength, "ushort"));
            ChildHeader.Items.Add(GetMenuItem("Sequence", ServiceHeader.sequence, "ushort"));
            PacketRoot.Items.Add(ChildHeader);

            // 2nd Element
            PacketRoot.Items.Add(GetMenuItem("Reserved", Header.Reserved, "ushort"));

            //3rd Element
            PacketRoot.Items.Add(GetMenuItem("DataFormatREV", Header.DataFormatREV, "ushort"));

            // 4th Element 
            ItemsMenu ChildParameter = GetMenuItem("Parameter", Parameter.LeftTimeStamp, "parameter");
            ChildParameter.Items.Add(GetMenuItem("TimestampL", Parameter.LeftTimeStamp, "uint"));
            ChildParameter.Items.Add(GetMenuItem("TimestampH", Parameter.RightTimeStamp, "uint"));
            ChildParameter.Items.Add(GetMenuItem("EventFlags", Parameter.EventFlags, "ushort"));
            ChildParameter.Items.Add(GetMenuItem("OperatingState", Parameter.OperatingState, "byte"));
            ChildParameter.Items.Add(GetMenuItem("AcousticPower", Parameter.AcousingPower, "byte"));
            ChildParameter.Items.Add(GetMenuItem("SampleLength", Parameter.SampleLength, "byte"));
            ChildParameter.Items.Add(GetMenuItem("UserDepth", Parameter.UserDepth, "byte"));
            ChildParameter.Items.Add(GetMenuItem("PRF", Parameter.PRF, "ushort"));
            ChildParameter.Items.Add(GetMenuItem("TIC", Parameter.TIC, "ushort"));
            ChildParameter.Items.Add(GetMenuItem("RFU", Parameter.RFU, "ushort"));
            PacketRoot.Items.Add(ChildParameter);

            // 5th Element 
            PacketRoot.Items.Add(GetMenuItem("EmbCount", Parameter.EmboliCount, "uint"));

            // 6th Element 
            ItemsMenu ChildEnvelop = GetMenuItem("Envelop", Envelop.Depth, "envelop");
            ChildEnvelop.Items.Add(GetMenuItem("Depth", Envelop.Depth, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("VelocityUnits", Envelop.VelocityUnit, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("ColIndexPos", Envelop.ColIndexPos, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("PosVelocity", Envelop.PosVelocity, "short"));
            ChildEnvelop.Items.Add(GetMenuItem("PosPEAK", Envelop.PosPeak, "short"));
            ChildEnvelop.Items.Add(GetMenuItem("PosMEAN", Envelop.PosMean, "short"));
            ChildEnvelop.Items.Add(GetMenuItem("PosDIAS", Envelop.PosDias, "short"));
            ChildEnvelop.Items.Add(GetMenuItem("PosPI", Envelop.PosPI, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("PosRI", Envelop.PosRI, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("ColIndexNeg", Envelop.ColIndexNeg, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("NegVelocity", Envelop.NegVelocity, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("NegPEAK", Envelop.NegPeak, "short"));
            ChildEnvelop.Items.Add(GetMenuItem("NegMEAN", Envelop.NegMean, "short"));
            ChildEnvelop.Items.Add(GetMenuItem("NegDIAS", Envelop.NegDias, "short"));
            ChildEnvelop.Items.Add(GetMenuItem("NegPI", Envelop.NegPI, "ushort"));
            ChildEnvelop.Items.Add(GetMenuItem("NegRI", Envelop.NegRI, "ushort"));
            PacketRoot.Items.Add(ChildEnvelop);

            // 7th Element 
            ItemsMenu ChildSpectrum = GetMenuItem("Spectrum", Spectrum.Depth, "spectrum");
            ChildSpectrum.Items.Add(GetMenuItem("Depth", Spectrum.Depth, "ushort"));
            ChildSpectrum.Items.Add(GetMenuItem("ClutterFilter", Spectrum.ClutterFilter, "ushort"));
            ChildSpectrum.Items.Add(GetMenuItem("AutoGainOffset", Spectrum.AutoGainOffset, "short"));
            ChildSpectrum.Items.Add(GetMenuItem("StartVelocity", Spectrum.StartVelocity, "short"));
            ChildSpectrum.Items.Add(GetMenuItem("EndVelocity", Spectrum.EndVelocity, "short"));
            ChildSpectrum.Items.Add(GetMenuItem("PointsPerColumn", Spectrum.PointsPerColumn, "ushort"));
            ChildSpectrum.Items.Add(GetMenuItem("Points", Spectrum.Points, "points"));
            PacketRoot.Items.Add(ChildSpectrum);
            trvMenu.Items.Add(PacketRoot);

            // 8th Element 
            ItemsMenu ChildMmode = GetMenuItem("Mmode", MMode.AutoGainOffset, "Mmode");
            ChildMmode.Items.Add(GetMenuItem("AutoGainOffset", MMode.AutoGainOffset, "short"));
            ChildMmode.Items.Add(GetMenuItem("StartDepth", MMode.StartDepth, "ushort"));
            ChildMmode.Items.Add(GetMenuItem("EndDepth", MMode.EndDepth, "ushort"));
            ChildMmode.Items.Add(GetMenuItem("PointsPerColumn", MMode.PointsPerColumn, "ushort"));
            ChildMmode.Items.Add(GetMenuItem("Power", MMode.Power, "power"));
            ChildMmode.Items.Add(GetMenuItem("Velocity", MMode.Velocity, "velocity"));
            PacketRoot.Items.Add(ChildMmode);

            // 3rd Element 
            ItemsMenu ChildP3 = GetMenuItem("Audio", Audio.Depth, "audio");
            ChildP3.Items.Add(GetMenuItem("Depth", Audio.Depth, "ushort"));
            ChildP3.Items.Add(GetMenuItem("RFU", Audio.RFU, "ushort"));
            ChildP3.Items.Add(GetMenuItem("SampleRate", Audio.SampleRate, "ushort"));
            ChildP3.Items.Add(GetMenuItem("MaxAmplitude", Audio.MaxAmplitude, "ushort"));
            ChildP3.Items.Add(GetMenuItem("Toward", Audio.Toward, "toward"));
            ChildP3.Items.Add(GetMenuItem("Away", Audio.Away, "away"));
            PacketRoot.Items.Add(ChildP3);

            // 2nd Element
            ItemsMenu ChildP2 = new ItemsMenu() { Title = "Archive" };
            //Add to root
            PacketRoot.Items.Add(ChildP2);

            // 4th Element
            PacketRoot.Items.Add(GetMenuItem("CheckSum", 1128, "int"));
            trvMenu.SelectedItemChanged += TreeItem_Selected;

        }

        void TreeItem_Selected(object sender, RoutedEventArgs e)
        {
            ItemsMenu item = (ItemsMenu)trvMenu.SelectedItem;
            KeyValuePair<int, int> fromIndex;
            KeyValuePair<int, int> toIndex;
            fromIndex = new KeyValuePair<int, int>(item.StartRow, item.StartColumn);
            toIndex = new KeyValuePair<int, int>(item.EndRow, item.EndColumn);
            SelectCellsByIndexes(fromIndex, toIndex);
        }

        public ItemsMenu GetMenuItem(string title, int postion, string type)
        {
            ItemsMenu item = new ItemsMenu();

            item.Title = title;
            item.StartRow = postion / 16;
            item.StartColumn = (postion % 16) + 1;

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
                    byteSize = Envelop.NegRI + 1;
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
                    byteSize = Parameter.RFU + 1;
                    break;
                case "spectrum":
                    byteSize = (Spectrum.Points - 1) + (DMIProtocol.SpectrumPointsCount * 2) - Spectrum.Depth;
                    break;
                default:
                    break;
            }

            return byteSize;
        }

        public void LoadBinaryData()
        {
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"LocalFolder\13-Channel1.txt");            
            int hexIn;
            byte[] cutFs = UsbTcd.TCDObj.GetPacketDetails(filePath, 0); // Offset byte 
            List<DMIPmdDataPacket> ListDMIPmdDataPacket = UsbTcd.TCDObj.PacketQueue[0];
            int count = 1;
            ObservableCollection<HexRecord> listHexRecord = new ObservableCollection<HexRecord>();
            StringBuilder sb = new StringBuilder();
            BigInteger InitailBInt = BigInteger.Parse("00000000", NumberStyles.HexNumber);
            BigInteger ConstantBInt = BigInteger.Parse("00000010", NumberStyles.HexNumber);
            HexRecord hexRecord = null;

            for (int i = 0; i < cutFs.Length; i++)
            {
                hexIn = cutFs[i];

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

            if (cutFs.Length % 16 != 0)
            {
                hexRecord.Offset = string.Format("{0:X8}", InitailBInt);
                listHexRecord.Add(hexRecord);
            }

            //link business data to CollectionViewSource
            CollectionViewSource itemCollectionViewSource;
            itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSource"));
            itemCollectionViewSource.Source = listHexRecord;
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
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int EndRow { get; set; }
        public int EndColumn { get; set; }

        public bool IsExpanded { get; set; }

        public ObservableCollection<ItemsMenu> Items { get; set; }

    }
}
