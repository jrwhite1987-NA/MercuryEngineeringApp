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
            ItemsMenu PacketRoot = new ItemsMenu() { Title = "Packet" };
            PacketRoot.IsExpanded = true;
            // 1th Element 
            ItemsMenu ChildP1 = GetMenuItem("Header", ServiceHeader.sync, "header");
            ChildP1.Items.Add(GetMenuItem("Sync", ServiceHeader.sync, "long"));
            ChildP1.Items.Add(GetMenuItem("SystemID", ServiceHeader.systemId, "byte"));
            ChildP1.Items.Add(GetMenuItem("DataSource", ServiceHeader.dataSource, "byte"));
            ChildP1.Items.Add(GetMenuItem("MessageType", ServiceHeader.messageType, "byte"));
            ChildP1.Items.Add(GetMenuItem("MessageSubType", ServiceHeader.messageSubType, "byte"));
            ChildP1.Items.Add(GetMenuItem("DataLength", ServiceHeader.dataLength, "ushort"));
            ChildP1.Items.Add(GetMenuItem("Sequence", ServiceHeader.sequence, "ushort"));
            PacketRoot.Items.Add(ChildP1);
            // 2nd Element
            ItemsMenu ChildP2 = new ItemsMenu() { Title = "Archive" };
            //Add to root
            PacketRoot.Items.Add(ChildP2);
            // 3rd Element 
            ItemsMenu ChildP3 = GetMenuItem("Audio", ServiceHeader.sync, "audio"); 
            ChildP3.Items.Add(new ItemsMenu() { Title = "Depth" });
            ChildP3.Items.Add(new ItemsMenu() { Title = "RFU" });
            ChildP3.Items.Add(new ItemsMenu() { Title = "SampleRate" });
            ChildP3.Items.Add(new ItemsMenu() { Title = "MaxAmplitude" });
            ChildP3.Items.Add(new ItemsMenu() { Title = "Toward" });
            ChildP3.Items.Add(new ItemsMenu() { Title = "Away" });
            PacketRoot.Items.Add(ChildP3);

            // 4th Element
            PacketRoot.Items.Add(new ItemsMenu() { Title = "CheckSum" });

            //5th Element
            PacketRoot.Items.Add(new ItemsMenu() { Title = "DataFormatRev" });

            // 6th Element 
            PacketRoot.Items.Add(new ItemsMenu() { Title = "EmbCount" });

            // 7th Element 
            ItemsMenu ChildP7 = new ItemsMenu() { Title = "Envelope" };
            ChildP7.Items.Add(new ItemsMenu() { Title = "Depth" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "VelocityUnits" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "ColIndexPos" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "PosVelocity" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "PosPEAK" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "PosMEAN" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "PosDIAS" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "PosPI" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "PosRI" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "ColIndexNeg" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "NegVelocity" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "NegPEAK" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "NegMEAN" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "NegDIAS" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "NegPI" });
            ChildP7.Items.Add(new ItemsMenu() { Title = "NegRI" });
            PacketRoot.Items.Add(ChildP7);         

            // 8th Element 
            ItemsMenu ChildP8 = new ItemsMenu() { Title = "Mmode" };
            ChildP8.Items.Add(new ItemsMenu() { Title = "AutoGainOffset" });
            ChildP8.Items.Add(new ItemsMenu() { Title = "StartDepth" });
            ChildP8.Items.Add(new ItemsMenu() { Title = "EndDepth" });
            ChildP8.Items.Add(new ItemsMenu() { Title = "PointsPerColumn" });
            ChildP8.Items.Add(new ItemsMenu() { Title = "Power" });
            ChildP8.Items.Add(new ItemsMenu() { Title = "Velocity" });
            PacketRoot.Items.Add(ChildP8);

            // 9th Element 
            ItemsMenu ChildP9 = new ItemsMenu() { Title = "Parameter" };
            ChildP9.Items.Add(new ItemsMenu() { Title = "TimestampL" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "TimestampH" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "EventFlags" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "OperatingState" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "AcousticPower" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "SampleLength" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "UserDepth" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "PRF" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "TIC" });
            ChildP9.Items.Add(new ItemsMenu() { Title = "RFU" });
            PacketRoot.Items.Add(ChildP9);

            // 10th Element
            PacketRoot.Items.Add(new ItemsMenu() { Title = "Reserved" });

            // 11th Element 
            ItemsMenu ChildP11 = new ItemsMenu() { Title = "Spectrum" };
            ChildP11.Items.Add(new ItemsMenu() { Title = "Depth" });
            ChildP11.Items.Add(new ItemsMenu() { Title = "ClutterFilter" });
            ChildP11.Items.Add(new ItemsMenu() { Title = "AutoGainOffset" });
            ChildP11.Items.Add(new ItemsMenu() { Title = "StartVelocity" });
            ChildP11.Items.Add(new ItemsMenu() { Title = "EndVelocity" });
            ChildP11.Items.Add(new ItemsMenu() { Title = "PointsPerColumn" });
            ChildP11.Items.Add(new ItemsMenu() { Title = "Points" });
            PacketRoot.Items.Add(ChildP11);
            trvMenu.Items.Add(PacketRoot);

            trvMenu.SelectedItemChanged += treeItem_Selected;

        }

        void treeItem_Selected(object sender, RoutedEventArgs e)
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
                    byteSize = ServiceHeader.sequence + 1;
                    break;
                case "points":
                    byteSize = (DMIProtocol.SpectrumPointsCount * 2) - 1;
                    break;
                default:
                    break;
            }

            return byteSize;
        }

        public void LoadBinaryData()
        {
            string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"LocalFolder\13-Channel1.txt");
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int hexIn;

            StringBuilder sb = new StringBuilder();
            BigInteger InitailBInt = BigInteger.Parse("00000000", NumberStyles.HexNumber);
            BigInteger ConstantBInt = BigInteger.Parse("00000010", NumberStyles.HexNumber);
            HexRecord hexRecord = null;
            byte[] cutFs = null;
            int count = 1;

            ObservableCollection<HexRecord> listHexRecord = new ObservableCollection<HexRecord>();

            using (BinaryReader reader = new BinaryReader(fs))
            {
                //reader.BaseStream.Seek(363372, SeekOrigin.Begin);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                cutFs = reader.ReadBytes(1132);

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

        private void SelectCellsByIndexes(KeyValuePair<int,int> fromIndex, KeyValuePair<int,int> toIndex)
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

            
            //foreach (KeyValuePair<int, int> cellIndex in cellIndexes)
            //{
            //    int rowIndex = cellIndex.Key;
            //    int columnIndex = cellIndex.Value;

            //    if (rowIndex < 0 || rowIndex > (grdMailbag.Items.Count - 1))
            //        throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            //    if (columnIndex < 0 || columnIndex > (grdMailbag.Columns.Count - 1))
            //        throw new ArgumentException(string.Format("{0} is an invalid column index.", columnIndex));

            //    object item = grdMailbag.Items[rowIndex]; //= Product X
            //    DataGridRow row = grdMailbag.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            //    if (row == null)
            //    {
            //        grdMailbag.ScrollIntoView(item);
            //        row = grdMailbag.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            //    }
            //    if (row != null)
            //    {
            //        DataGridCell cell = GetCell(grdMailbag, row, columnIndex);
            //        if (cell != null)
            //        {
            //            DataGridCellInfo dataGridCellInfo = new DataGridCellInfo(cell);
            //            grdMailbag.SelectedCells.Add(dataGridCellInfo);
            //            cell.Focus();
            //        }
            //    }
            //}
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
