using Core.Constants;
using Core.Models.ReportModels;
using MercuryEngApp.Common;
using Newtonsoft.Json;
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
using System.Xml;
using UsbTcdLibrary;
using UsbTcdLibrary.PacketFormats;



namespace MercuryEngApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
            PowerController.Instance.OnDeviceStateChanged += MicrocontrollerOnDeviceStateChanged;
            PowerController.Instance.StartWatcher();
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            ExamTab.Content = new ExamUserControl();
            InfoTab.Content = new InfoUserControl();
        }

        private async void MicrocontrollerOnDeviceStateChanged(bool flag)
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
            }
            else
            {
                //microcontroller disconnected
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            List<ReadPointerModel> listReadPointerModel = new List<ReadPointerModel>();
            ReadPointerModel readPointerModel = new ReadPointerModel();
            readPointerModel.ChannelId = 1;
            readPointerModel.ExamId = 13;
            readPointerModel.ExamSnapShotId = 82;
            readPointerModel.OffsetByte = 363372;
            readPointerModel.RangeOffsetByte = 566000;
            listReadPointerModel.Add(readPointerModel);
            UsbTcd.TCDObj.ReadFromFileWithRange(13, listReadPointerModel);
            List<DMIPmdDataPacket> ListDMIPmdDataPacket = UsbTcd.TCDObj.PacketQueue[82];
            string json = JsonConvert.SerializeObject(ListDMIPmdDataPacket);
            string jsonFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"LocalFolder\13-Channel1Json.txt");
            System.IO.File.WriteAllText(jsonFilePath, json);
            XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"Row\":" + json + "}", "root");
            string xmlFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"LocalFolder\13-Channel1Xml.xml");
            doc.Save(xmlFilePath);
        }       
    }
}
