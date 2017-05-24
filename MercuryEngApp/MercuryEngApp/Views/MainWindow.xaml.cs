﻿using Core.Constants;
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



namespace MercuryEngApp
{
    public delegate void TCDPower();
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isPowerOn;
        public static event TCDPower TurnTCDON;
        public static event TCDPower TurnTCDOFF;
        public MainWindow()
        {
            InitializeComponent();
           
            this.Loaded += MainWindowLoaded;
            isPowerOn = false;
            PowerController.Instance.OnDeviceStateChanged += MicrocontrollerOnDeviceStateChanged;
            PowerController.Instance.StartWatcher();
            //TestReview();
        }       

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {            
            ExamTab.Content = new ExamUserControl();
            InfoTab.Content = new InfoUserControl();
            CalibrationTab.Content = new CalibrationUserControl();
            PacketTab.Content = new PacketControl();           
            //Task.Delay(4500).Wait();
            //MainLayout.Visibility = Visibility.Visible;
            //temp.Visibility = Visibility.Collapsed;
        }

        public void TestReview()
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
                App.ActiveChannels = (await UsbTcd.TCDObj.GetProbesConnectedAsync()).ActiveChannel;
            }
            else
            {
                //microcontroller disconnected
            }
        }

        private void TCDPowerClick(object sender, RoutedEventArgs e)
        {
           if(!isPowerOn)
           {
               //Turn TCD ON
               if(TurnTCDON!=null)
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
    }
}
