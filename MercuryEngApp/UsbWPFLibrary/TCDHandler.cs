// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="TCDHandler.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Common;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UsbTcdLibrary.PacketFormats;
using UsbTcdLibrary.StatusClasses;
using Windows.Devices.Enumeration;
using Windows.Devices.Usb;
using Windows.Storage.Streams;

namespace UsbTcdLibrary
{
    /// <summary>
    /// Delegate ReadTCDDelegateDual
    /// </summary>
    /// <returns>Task.</returns>
    public delegate Task ReadTCDDelegateDual();

    /// <summary>
    /// Delegate TCDPacketFormed
    /// </summary>
    /// <param name="packets">The packets.</param>
    public delegate void TCDPacketFormed(DMIPmdDataPacket[] packets);

    /// <summary>
    /// Delegate RecordPacketDelegate
    /// </summary>
    /// <param name="leftChannelData">The left channel data.</param>
    /// <param name="rightChannelData">The right channel data.</param>
    internal delegate void RecordPacketDelegate(byte[] leftChannelData, byte[] rightChannelData);

    /// <summary>
    /// Delegate ProbePlugUnplug
    /// </summary>
    /// <param name="probe">The probe.</param>
    public delegate void ProbePlugUnplug(TCDHandles probe);

    /// <summary>
    /// Class TCDHandler.
    /// </summary>
    internal class TCDHandler
    {
        #region Variables

        /// <summary>
        /// The current channel
        /// </summary>
        internal int _currentChannel;

        /// <summary>
        /// The channel one
        /// </summary>
        private const int CHANNEL_ONE = 1;

        /// <summary>
        /// The channel two
        /// </summary>
        private const int CHANNEL_TWO = 2;

        /// <summary>
        /// Gets or sets the temporary handle.
        /// </summary>
        /// <value>The temporary handle.</value>
        internal UsbDevice tempHandle { get; set; }

        /// <summary>
        /// Gets or sets the general TCD handle.
        /// </summary>
        /// <value>The general TCD handle.</value>
        internal UsbDevice GeneralTCDHandle
        {
            get
            {
                if (_currentChannel == CHANNEL_ONE)
                {
                    return Channel1.TCDHandleChannel;
                }
                else if (_currentChannel == CHANNEL_TWO)
                {
                    return Channel2.TCDHandleChannel;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_currentChannel == CHANNEL_ONE)
                {
                    Channel1.TCDHandleChannel = value;
                }
                else 
                {
                    if (_currentChannel == CHANNEL_TWO)
                    {
                        Channel2.TCDHandleChannel = value;
                    }
                }
            }
        }

        /// <summary>
        /// The is TCD working
        /// </summary>
        private bool _isTCDWorking;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is TCD working.
        /// </summary>
        /// <value><c>true</c> if this instance is TCD working; otherwise, <c>false</c>.</value>
        internal bool isTCDWorking
        {
            get
            {
                return _isTCDWorking;
            }
            set
            {
                _isTCDWorking = value;
                if (!_isTCDWorking)
                {
                    Channel1.IsChannelEnabled = false;
                    Channel2.IsChannelEnabled = false;
                }
            }
        }

        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        /// <value>The channel identifier.</value>
        internal int ChannelId { get; private set; }

        /// <summary>
        /// Gets the device descriptor.
        /// </summary>
        /// <value>The device descriptor.</value>
        internal UsbDeviceDescriptor deviceDescriptor { get; private set; }

        /// <summary>
        /// Gets the configuration descriptor.
        /// </summary>
        /// <value>The configuration descriptor.</value>
        internal UsbConfigurationDescriptor configDescriptor { get; private set; }

        /// <summary>
        /// Gets the device configuration.
        /// </summary>
        /// <value>The device configuration.</value>
        internal UsbConfiguration deviceConfig { get; private set; }

        /// <summary>
        /// The handle for TCD
        /// </summary>
        private static TCDHandler handleForTCD;

        /// <summary>
        /// The singleton creation lock
        /// </summary>
        private static Object singletonCreationLock = new Object();

        /// <summary>
        /// Occurs when [on TCD read dual].
        /// </summary>
        public static event ReadTCDDelegateDual OnTCDReadDual;

        /// <summary>
        /// Delegate ReadMessage
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="channel">The channel.</param>
        public delegate void ReadMessage(byte[] data, int channel);

        /// <summary>
        /// Occurs when [channel message].
        /// </summary>
        public static event ReadMessage ChannelMessage;

        #endregion Variables

        #region ChannelVariables

        /// <summary>
        /// Gets or sets the channel1 parameter.
        /// </summary>
        /// <value>The channel1 parameter.</value>
        internal TCDDevice Channel1 { get; set; }

        /// <summary>
        /// Gets or sets the channel2 parameter.
        /// </summary>
        /// <value>The channel2 parameter.</value>
        internal TCDDevice Channel2 { get; set; }

        #endregion ChannelVariables

        #region Constructor

        /// <summary>
        /// Prevents a default instance of the <see cref="TCDHandler" /> class from being created.
        /// </summary>
        private TCDHandler()
        {
            const int BUFFER_SIZE = 84;

            Channel1 = new TCDDevice();
            Channel1.ProbeBuffer = new Windows.Storage.Streams.Buffer(BUFFER_SIZE);

            Channel2 = new TCDDevice();
            Channel2.ProbeBuffer = new Windows.Storage.Streams.Buffer(BUFFER_SIZE);

            DopplerModule.packetQueueChannel1 = new List<DMIPmdDataPacket>();
            DopplerModule.packetQueueChannel2 = new List<DMIPmdDataPacket>();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Provides a singleton instance
        /// </summary>
        /// <value>The current.</value>
        public static TCDHandler Current
        {
            get
            {
                lock (singletonCreationLock)
                {
                    if (handleForTCD == null)
                    {
                        handleForTCD = new TCDHandler();
                    }
                }
                return handleForTCD;
            }
        }

        /// <summary>
        /// Gets the current device.
        /// </summary>
        /// <param name="currentHandle">The current handle.</param>
        /// <returns>TCDDevice.</returns>
        internal TCDDevice GetCurrentDevice(TCDHandles currentHandle)
        {
            if (currentHandle == TCDHandles.Channel1)
            {
                return Channel1;
            }
            else if (currentHandle == TCDHandles.Channel2)
            {
                return Channel2;
            }
            else
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ExecutionConstants.TCDHandler + ExecutionConstants.MethodName,
                   // "GetCurrentDevice:Received garbage value for Channel|" + currentHandle.ToString() + "|" + ExecutionConstants.MethodExecutionFail, Severity.Warning);
                return null;
            }
        }

        /// <summary>
        /// Attempts to connect with the TCD hardware connected asynchronously
        /// </summary>
        /// <returns>True if connection is successful
        /// False if unable to connect to TCD</returns>
        internal async Task<bool> GetDeviceHandleAsync()
        {
            int count = 0;
            const int DEVICE_COUNT_TWO = 2;
            const int DEVICE_FIRST_INDEX = 0;
            const int DEVICE_SECOND_INDEX = 1;
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async GetDeviceHandleAsync begins", "GetDeviceHandleAsync", Severity.Debug);
                string deviceQueryString = UsbDevice.GetDeviceSelector(DMIProtocol.TCD_VENDOR_ID, DMIProtocol.TCD_PRODUCT_ID);
                DeviceInformationCollection myDevices = await DeviceInformation.FindAllAsync(deviceQueryString, null);
                count = myDevices.Count();

                if (myDevices != null && count >= Constants.VALUE_1)
                {
                    tempHandle = await UsbDevice.FromIdAsync(myDevices[DEVICE_FIRST_INDEX].Id);
                    if (tempHandle == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Device null");
                    }
                    await AssignDeviceInfo(tempHandle);
                    tempHandle = null;

                    if (count >= DEVICE_COUNT_TWO)
                    {
                        if (tempHandle == null)
                        {
                            System.Diagnostics.Debug.WriteLine("Device null");
                        }
                        tempHandle = await UsbDevice.FromIdAsync(myDevices[DEVICE_SECOND_INDEX].Id);
                        await AssignDeviceInfo(tempHandle);
                        tempHandle = null;
                    }

                    return GetTCDStatus();
                }
                isTCDWorking = false;
                //Logs.Instance.ErrorLog<TCDHandler>("async GetDeviceHandleAsync ends ", "GetDeviceHandleAsync", Severity.Debug);
                return isTCDWorking;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetDeviceHandleAsync", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Gets the TCD status.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool GetTCDStatus()
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("GetTCDStatus begins", "GetTCDStatus", Severity.Debug);
                if (Channel1.TCDHandleChannel == null && Channel2.TCDHandleChannel == null)
                {
                    isTCDWorking = false;
                    return isTCDWorking;
                }
                else
                {
                    isTCDWorking = true;
                    //Logs.Instance.ErrorLog<TCDHandler>("GetTCDStatus ends ", "GetTCDStatus", Severity.Debug);
                    return isTCDWorking;
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetTCDStatus", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Seals the probes.
        /// </summary>
        internal void SealProbes()
        {
            if (!Channel1.IsChannelEnabled)
            {
                Channel1.TCDHandleChannel = null;
                Channel1.IsSensingEnabled = false;
            }

            if (!Channel2.IsChannelEnabled)
            {
                Channel2.TCDHandleChannel = null;
                Channel2.IsSensingEnabled = false;
            }
        }

        /// <summary>
        /// Gets the active channel.
        /// </summary>
        /// <param name="tempUSBDevice">The temporary usb device.</param>
        /// <returns>Task.</returns>
        internal async Task AssignDeviceInfo(UsbDevice tempUSBDevice)
        {
            byte channelId = 0;
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async GetActiveChannel begins ", "GetActiveChannel", Severity.Debug);
                if (tempUSBDevice != null)
                {
                    channelId = await GetChannelIdAsync(tempUSBDevice);
                }
                if (channelId == CHANNEL_ONE)
                {
                    Channel1.TCDHandleChannel = tempUSBDevice;
                    Channel1.ProbeInformation = await GetProbeInfoAsync(Channel1);
                    Channel1.ModuleInformation = await GetModuleInfo(TCDHandles.Channel1);
                    //Logs.Instance.ErrorLog<TCDHandler>(MessageConstants.Probe1Connected, "GetActiveChannel", Severity.Information);
                    tempUSBDevice = null;
                }
                else if (channelId == CHANNEL_TWO)
                {
                    Channel2.TCDHandleChannel = tempUSBDevice;
                    Channel2.ProbeInformation = await GetProbeInfoAsync(Channel2);
                    Channel2.ModuleInformation = await GetModuleInfo(TCDHandles.Channel2);
                    //Logs.Instance.ErrorLog<TCDHandler>(MessageConstants.Probe2Connected, "GetActiveChannel", Severity.Information);
                    tempUSBDevice = null;
                }
                else
                {
                    //Logs.Instance.ErrorLog<TCDHandler>("No Probe found", "GetActiveChannel", Severity.Warning);
                    tempUSBDevice = null;
                }
                //Logs.Instance.ErrorLog<TCDHandler>("async GetActiveChannel ends ", "GetActiveChannel", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetActiveChannel", Severity.Warning);
            }
        }

        /// <summary>
        /// Initializes the streams.
        /// </summary>
        internal void InitializeStreams()
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("InitializeStreams begins ", "GetActiveChannel", Severity.Debug);

                if (Channel1.TCDHandleChannel != null)
                {
                    if (!Channel1.IsSensingEnabled)
                    {
                        Channel1.ServiceLogReader = new DataReader(Channel1.TCDHandleChannel.DefaultInterface.BulkInPipes[1].InputStream);
                        Channel1.canReadLog = true;
                        Channel1.IsSensingEnabled = true;
                    }
                    if (!Channel1.IsReadingEnabled)
                    {
                        if (Channel1.IsChannelEnabled)
                        {
                            Channel1.ChannelStream = Channel1.TCDHandleChannel.DefaultInterface.BulkInPipes[0].InputStream;
                            Channel1.ChannelReader = new DataReader(Channel1.ChannelStream);
                            Channel1.IsReadingEnabled = true;
                        }
                    }
                }

                if (Channel2.TCDHandleChannel != null)
                {
                    if (!Channel2.IsSensingEnabled)
                    {
                        Channel2.ServiceLogReader = new DataReader(Channel2.TCDHandleChannel.DefaultInterface.BulkInPipes[1].InputStream);
                        Channel2.canReadLog = true;
                        Channel2.IsSensingEnabled = true;
                    }
                    if (!Channel2.IsReadingEnabled)
                    {
                        if (Channel2.IsChannelEnabled)
                        {
                            Channel2.ChannelStream = Channel2.TCDHandleChannel.DefaultInterface.BulkInPipes[0].InputStream;
                            Channel2.ChannelReader = new DataReader(Channel2.ChannelStream);
                            Channel2.IsReadingEnabled = true;
                        }
                    }
                }
                //Logs.Instance.ErrorLog<TCDHandler>("InitializeStreams ends ", "GetActiveChannel", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "InitializeStreams", Severity.Warning);
            }
        }

        /// <summary>
        /// Gets the device and config descriptor of the selected TCD channel
        /// </summary>
        /// <param name="channel">The channel.</param>
        private void GetDescriptors(TCDHandles channel)
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("GetDescriptors begins for channel: " + channel.ToString(), "GetDescriptors", Severity.Debug);

                _currentChannel = (int)channel;
                deviceDescriptor = GeneralTCDHandle.DeviceDescriptor;
                deviceConfig = GeneralTCDHandle.Configuration;
                configDescriptor = GeneralTCDHandle.Configuration.ConfigurationDescriptor;

                //Logs.Instance.ErrorLog<TCDHandler>("GetDescriptors ends ", "GetDescriptors", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetDescriptors", Severity.Warning);
            }
        }

        /// <summary>
        /// Gets the module information.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>Task&lt;ModuleInfo&gt;.</returns>
        internal async Task<ModuleInfo> GetModuleInfo(TCDHandles channel)
        {
            _currentChannel = (int)channel;
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("GetModuleInfo begins for channel: " + channel.ToString(), "GetModuleInfo", Severity.Debug);

                if (GeneralTCDHandle != null)
                {
                    IBuffer moduleInfo = new Windows.Storage.Streams.Buffer(140);
                    UsbSetupPacket setupPacket = SetupPacket(DMIProtocol.DMI_REQ_MODULE_INFO, 0, 0, DMIProtocol.DOPPLER_REQUEST, UsbTransferDirection.Out, 140);
                    await GeneralTCDHandle.SendControlInTransferAsync(setupPacket, moduleInfo);
                    return ModuleInfo.ConvertArraytoInfo(moduleInfo.ToArray());
                }
                else
                {
                    //Logs.Instance.ErrorLog<TCDHandler>("GetModuleInfo ends ", "GetModuleInfo", Severity.Debug);
                    return new ModuleInfo();
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetModuleInfo", Severity.Warning);
                return new ModuleInfo();
            }
        }

        /// <summary>
        /// Frees the resources and disconnects the device
        /// the state of isTCDWorking is also set to false
        /// </summary>
        internal void CloseDevice()
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("CloseDevice begins", "CloseDevice", Severity.Debug);
                OnTCDReadDual = null;
                ChannelMessage = null;

                if (Channel1.IsChannelEnabled)
                {
                    Channel1.Dispose();
                }
                if (Channel2.IsChannelEnabled)
                {
                    Channel2.Dispose();
                }
                //Logs.Instance.ErrorLog<TCDHandler>("CloseDevice ends", "CloseDevice", Severity.Debug);
                isTCDWorking = false;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "CloseDevice", Severity.Warning);
            }
        }

        /// <summary>
        /// GetDescriptorAsString
        /// Gets the device descriptors, configuration descriptors and endpoint descriptors of the TCD connected formatted in a string
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The string containing the descriptors</returns>
        internal string GetDescriptorsAsString(TCDHandles channel)
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("GetDescriptorsAsString begins for channel:" + channel.ToString(), "GetDescriptorsAsString", Severity.Debug);

                _currentChannel = (int)channel;
                GetDescriptors(channel);
                var usbInterface = GeneralTCDHandle.DefaultInterface;
                var bulkInPipes = usbInterface.BulkInPipes;
                var bulkOutPipes = usbInterface.BulkOutPipes;
                var interruptInPipes = usbInterface.InterruptInPipes;
                var interruptOutPipes = usbInterface.InterruptOutPipes;

                string descriptorString = "Device Descriptor\n"
                         + "\nUsb Spec Number : 0x" + deviceDescriptor.BcdUsb.ToString("X4", NumberFormatInfo.InvariantInfo)
                         + "\nMax Packet Size (Endpoint 0) : " + deviceDescriptor.MaxPacketSize0.ToString("D", NumberFormatInfo.InvariantInfo)
                         + "\nVendor ID : 0x" + deviceDescriptor.VendorId.ToString("X4", NumberFormatInfo.InvariantInfo)
                         + "\nProduct ID : 0x" + deviceDescriptor.ProductId.ToString("X4", NumberFormatInfo.InvariantInfo)
                         + "\nDevice Revision : 0x" + deviceDescriptor.BcdDeviceRevision.ToString("X4", NumberFormatInfo.InvariantInfo)
                         + "\nNumber of Configurations : " + deviceDescriptor.NumberOfConfigurations.ToString("D", NumberFormatInfo.InvariantInfo)
                         + "\nBulk In Pipe : " + Channel1.TCDHandleChannel.DefaultInterface.Descriptors.First()
                         + "\n\nConfiguration Descriptor\n"
                         + "\nNumber of Interfaces : " + deviceConfig.UsbInterfaces.Count.ToString("D", NumberFormatInfo.InvariantInfo)
                         + "\nConfiguration Value : 0x" + configDescriptor.ConfigurationValue.ToString("X2", NumberFormatInfo.InvariantInfo)
                         + "\nSelf Powered : " + configDescriptor.SelfPowered.ToString()
                         + "\nRemote Wakeup : " + configDescriptor.RemoteWakeup.ToString()
                         + "\nMax Power (milliAmps) : " + configDescriptor.MaxPowerMilliamps.ToString("D", NumberFormatInfo.InvariantInfo)
                         + "\n\nEndpoint Descriptors for open pipes";

                // Print Bulk In Endpoint descriptors
                foreach (UsbBulkInPipe bulkInPipe in bulkInPipes)
                {
                    var endpointDescriptor = bulkInPipe.EndpointDescriptor;

                    descriptorString += "\n\nBulk In Endpoint Descriptor"
                            + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                            + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo);
                }

                // Print Bulk Out Endpoint descriptors
                foreach (UsbBulkOutPipe bulkOutPipe in bulkOutPipes)
                {
                    var endpointDescriptor = bulkOutPipe.EndpointDescriptor;

                    descriptorString += "\n\nBulk Out Endpoint Descriptor"
                            + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                            + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo);
                }

                // Print Interrupt In Endpoint descriptors
                foreach (UsbInterruptInPipe interruptInPipe in interruptInPipes)
                {
                    var endpointDescriptor = interruptInPipe.EndpointDescriptor;

                    descriptorString += "\n\nInterrupt In Endpoint Descriptor"
                            + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                            + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo)
                    + "\nInterval : " + endpointDescriptor.Interval.Duration().ToString();
                }

                // Print Interrupt Out Endpoint descriptors
                foreach (UsbInterruptOutPipe interruptOutPipe in interruptOutPipes)
                {
                    var endpointDescriptor = interruptOutPipe.EndpointDescriptor;

                    descriptorString += "\n\nInterrupt Out Endpoint Descriptor"
                            + "\nEndpoint Number : 0x" + endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo)
                            + "\nMax Packet Size : " + endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo)
                    + "\nInterval : " + endpointDescriptor.Interval.Duration().ToString();
                }
                //Logs.Instance.ErrorLog<TCDHandler>("GetDescriptorsAsString ends ", "GetDescriptorsAsString", Severity.Debug);
                return descriptorString;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetDescriptorsAsString", Severity.Warning);
                return "Could not connect to the TCD";
            }
        }

        /// <summary>
        /// Send the commands to TCD with no Data buffer and Length = 0
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="request">bRequest</param>
        /// <param name="index">wIndex</param>
        /// <param name="value">wValue</param>
        /// <returns>The number of bytes transferred to the TCD</returns>
        public async Task<uint> SendControlCommandAsync(TCDHandles channel, int request, uint index, uint value)
        {
            _currentChannel = (int)channel;
            uint bytesTransferred = 0;
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("SendControlCommandAsync begins for channel:" + channel.ToString() + " request:" + request.ToString() + " index:"
              //  + index.ToString() + " value:" + value.ToString(),
              //  "SendControlCommandAsync", Severity.Debug);
                if (GeneralTCDHandle != null)
                {
                    const int PACKET_LENGTH = 0;
                    UsbSetupPacket setupPacket = SetupPacket(request, index, value, DMIProtocol.DOPPLER_COMMAND, UsbTransferDirection.Out, PACKET_LENGTH);
                    bytesTransferred = await GeneralTCDHandle.SendControlOutTransferAsync(setupPacket);
                }
                if (request == DMIProtocol.DMI_CMD_SET_MODE)
                {
                    TCDHandler.Current.GetCurrentDevice(channel).CurrentMode = (TCDModes)index;
                }
                //Logs.Instance.ErrorLog<TCDHandler>("SendControlCommandAsync ends ", "SendControlCommandAsync", Severity.Debug);
                return bytesTransferred;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "SendControlCommandAsync", Severity.Warning);
                return 0;
            }
        }

        /// <summary>
        /// send control request as an asynchronous operation.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="request">The request.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        public async Task<byte[]> SendControlRequestAsync(TCDHandles channel, int request, uint index, uint value, uint bufferLength)
        {
            _currentChannel = (int)channel;
            byte[] resultArray = null;
            try
            {
                IBuffer tempBuffer = new Windows.Storage.Streams.Buffer(bufferLength);
                //Logs.Instance.ErrorLog<TCDHandler>("SendControlRequestAsync begins for channel:" + channel.ToString() + " request:" + request.ToString() + " index:"
               // + index.ToString() + " value:" + value.ToString(),
               // "SendControlRequestAsync", Severity.Debug);
                if (GeneralTCDHandle != null)
                {
                    const int PACKET_LENGTH = 0;
                    UsbSetupPacket setupPacket = SetupPacket(request, index, value, DMIProtocol.DOPPLER_REQUEST, UsbTransferDirection.In, PACKET_LENGTH);
                    await GeneralTCDHandle.SendControlInTransferAsync(setupPacket, tempBuffer);
                    resultArray = tempBuffer.ToArray();
                }
                //Logs.Instance.ErrorLog<TCDHandler>("SendControlRequestAsync ends ", "SendControlRequestAsync", Severity.Debug);
                return resultArray;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "SendControlRequestAsync", Severity.Warning);
                return null;
            }
        }

        /// <summary>
        /// Setups the packet.
        /// </summary>
        /// <param name="requestCode">The request code.</param>
        /// <param name="requestIndex">Index of the request.</param>
        /// <param name="requestValue">The request value.</param>
        /// <param name="asByte">As byte.</param>
        /// <param name="requestTypeDirection">The request type direction.</param>
        /// <param name="requestLength">Length of the request.</param>
        /// <returns>UsbSetupPacket.</returns>
        private static UsbSetupPacket SetupPacket(int requestCode, uint requestIndex,
            uint requestValue, byte asByte, UsbTransferDirection requestTypeDirection, uint requestLength)
        {
            UsbSetupPacket setupPacket = new UsbSetupPacket
            {
                RequestType = new UsbControlRequestType
                {
                    Direction = requestTypeDirection,
                    Recipient = UsbControlRecipient.Device,
                    ControlTransferType = UsbControlTransferType.Vendor,
                    AsByte = asByte,
                },
                Request = (byte)requestCode,
                Length = requestLength,
                Value = requestValue,
                Index = requestIndex
            };
            return setupPacket;
        }

        /// <summary>
        /// Get the channel Id of the current TCD handle
        /// </summary>
        /// <param name="tempHandle">The temporary handle.</param>
        /// <returns>Task&lt;System.Byte&gt;.</returns>
        internal async Task<byte> GetChannelIdAsync(UsbDevice tempHandle)
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async GetChannelIdAsync begins for channel:" + tempHandle.ToString(), "GetChannelIdAsync", Severity.Debug);
                const int REQUEST_INDEX = 0;
                const int REQUEST_VALUE = 0;
                const int REQUEST_LENGTH = 1;
                const int BUFFER_SIZE = 1;
                IBuffer channelId = new Windows.Storage.Streams.Buffer(BUFFER_SIZE);

                UsbSetupPacket setupPacket = SetupPacket(DMIProtocol.DMI_REQ_Channel,
                    REQUEST_INDEX, REQUEST_VALUE, DMIProtocol.DOPPLER_REQUEST, UsbTransferDirection.In, REQUEST_LENGTH);

                await tempHandle.SendControlInTransferAsync(setupPacket, channelId);
                Byte[] result = channelId.ToArray();
                //Logs.Instance.ErrorLog<TCDHandler>("async GetChannelIdAsync ends", "GetChannelIdAsync", Severity.Debug);
                return result[0];
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetChannelIdAsync", Severity.Warning);
                return 0;
            }
        }

        /// <summary>
        /// Determine if the probe is connected to the given channel
        /// </summary>
        /// <param name="tempDevice">The temporary device.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<ProbeInfo> GetProbeInfoAsync(TCDDevice tempDevice)
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async GetProbeInfoAsync begins for channel:" + tempDevice.ToString(), "GetProbeInfoAsync", Severity.Debug);
                const int REQUEST_INDEX = 0;
                const int REQUEST_VALUE = 0;
                
                IBuffer probeInfo = new Windows.Storage.Streams.Buffer(DMIProtocol.PROBE_INFO_REQUEST_LENGTH);

                UsbSetupPacket setupPacket = SetupPacket(DMIProtocol.DMI_REQ_PROBE_INFO,
                   REQUEST_INDEX, REQUEST_VALUE, DMIProtocol.DOPPLER_REQUEST, UsbTransferDirection.In, DMIProtocol.PROBE_INFO_REQUEST_LENGTH);

                await tempDevice.TCDHandleChannel.SendControlInTransferAsync(setupPacket, probeInfo);
                tempDevice.IsChannelEnabled = true;
                //Logs.Instance.ErrorLog<TCDHandler>("async GetProbeInfoAsync ends", "GetProbeInfoAsync", Severity.Debug);

                return ProbeInfo.ConvertArrayToInfo(probeInfo.ToArray());
            }
            catch (Exception ex)
            {
                if (ex.Message != "A device attached to the system is not functioning. (Exception from HRESULT: 0x8007001F)")
                {
                    //Logs.Instance.ErrorLog<TCDHandler>(ex, "GetProbeInfoAsync", Severity.Warning);
                }
                tempDevice.IsChannelEnabled = false;
                return null;
            }
        }

        /// <summary>
        /// Sets the Timestamp on the TCD machine
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="currentTime">The current time.</param>
        /// <returns>Task&lt;System.UInt32&gt;.</returns>
        internal async Task<uint> ResetTimeStampAsync(TCDHandles channel, DateTime currentTime)
        {
            _currentChannel = (int)channel;
            uint bytesTransferred = 0;
            if (GeneralTCDHandle != null)
            {
                const int DEFAULT_VALUE = 0;
                byte[] tempBuffer = { DEFAULT_VALUE, DEFAULT_VALUE, DEFAULT_VALUE, DEFAULT_VALUE }; //TODO: Need to fix this after updating the firmware of TCD
                try
                {
                    //Logs.Instance.ErrorLog<TCDHandler>("async ResetTimeStampAsync begins for channel:" + channel.ToString() + "currentTime: " + currentTime.ToString(),
                     //   "ResetTimeStampAsync", Severity.Debug);
                    if (GeneralTCDHandle != null)
                    {
                        const int REQUEST_INDEX = 0;
                        const int REQUEST_VALUE = 0;
                        const int REQUEST_LENGTH = 4;

                        UsbSetupPacket setupPacket = SetupPacket(DMIProtocol.DMI_CMD_SET_TIMESTAMP,
                        REQUEST_INDEX, REQUEST_VALUE, DMIProtocol.DOPPLER_COMMAND, UsbTransferDirection.Out, REQUEST_LENGTH);
                        bytesTransferred = await GeneralTCDHandle.SendControlOutTransferAsync(setupPacket, tempBuffer.AsBuffer());
                    }
                    //Logs.Instance.ErrorLog<TCDHandler>("async ResetTimeStampAsync ends", "ResetTimeStampAsync", Severity.Debug);
                    return DEFAULT_VALUE;
                }
                catch (Exception ex)
                {
                    //Logs.Instance.ErrorLog<TCDHandler>(ex, "ResetTimeStampAsync", Severity.Warning);
                    return DEFAULT_VALUE;
                }
            }
            return bytesTransferred;
        }

        /// <summary>
        /// send control command as an asynchronous operation.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="request">The request.</param>
        /// <param name="index">The index.</param>
        /// <param name="value">The value.</param>
        /// <param name="bufferLength">Length of the buffer.</param>
        /// <param name="bufferData">The buffer data.</param>
        /// <returns>Task&lt;System.UInt32&gt;.</returns>
        internal async Task<uint> SendControlCommandAsync(TCDHandles channel, int request, uint index, uint value, uint bufferLength, byte[] bufferData)
        {
            _currentChannel = (int)channel;
            uint bytesTransferred = 0;
            if (GeneralTCDHandle != null)
            {
                try
                {
                    if (bufferData != null)
                    {
                        UsbSetupPacket setupPacket = SetupPacket(request, index, value, DMIProtocol.DOPPLER_COMMAND, UsbTransferDirection.Out, bufferLength);
                        bytesTransferred = await GeneralTCDHandle.SendControlOutTransferAsync(setupPacket, bufferData.AsBuffer());
                    }
                }
                catch (Exception ex)
                {
                    //Logs.Instance.ErrorLog<TCDHandler>(ex, "ResetTimeStampAsync", Severity.Warning);
                    throw;
                }
            }
            return bytesTransferred;
        }

        /// <summary>
        /// Set the PRF value to the selected channel
        /// </summary>
        /// <param name="channel">Channel of the TCD</param>
        /// <param name="PRF">The PRF value</param>
        /// <param name="startDepth">The startDepth of mmode</param>
        /// <returns>Task&lt;System.UInt32&gt;.</returns>
        internal async Task<uint> SetPRFAsync(TCDHandles channel, uint PRF, byte startDepth)
        {
            _currentChannel = (int)channel;
            uint bytesTransferred = 0;
            if (GeneralTCDHandle != null)
            {
                const int DEFAULT_VALUE = 0;
                byte[] prfBuffer = { startDepth, DEFAULT_VALUE, DEFAULT_VALUE, DEFAULT_VALUE };
                //Logs.Instance.ErrorLog<TCDHandler>("async SetPRFAsync begins for channel:" + channel.ToString() + "PRF: " + PRF.ToString() + "startDepth:" + startDepth.ToString(),
                 //   "SetPRFAsync", Severity.Debug);
                try
                {
                    const int REQUEST_INDEX = 5;
                    const int REQUEST_LENGTH = 4;
                    UsbSetupPacket setupPacket = SetupPacket(DMIProtocol.DMI_CMD_DOPPLER,
                    REQUEST_INDEX, PRF, DMIProtocol.DOPPLER_COMMAND, UsbTransferDirection.Out, REQUEST_LENGTH);
                    bytesTransferred = await GeneralTCDHandle.SendControlOutTransferAsync(setupPacket, prfBuffer.AsBuffer());
                    //Logs.Instance.ErrorLog<TCDHandler>("async SetPRFAsync ends", "SetPRFAsync", Severity.Debug);
                }
                catch (Exception ex)
                {
                    //Logs.Instance.ErrorLog<TCDHandler>(ex, "SetPRFAsync", Severity.Warning);
                    return DEFAULT_VALUE;
                }
            }
            return bytesTransferred;
        }

        /// <summary>
        /// set envelope range as an asynchronous operation.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="posMaxVelocity">The position maximum velocity.</param>
        /// <param name="negMaxVelocity">The neg maximum velocity.</param>
        /// <returns>Task&lt;System.UInt32&gt;.</returns>
        internal async Task<uint> SetEnvelopeRangeAsync(TCDHandles channel, short posMaxVelocity, short negMaxVelocity)
        {
            _currentChannel = (int)channel;
            uint bytesTransferred = 0;
            if (GeneralTCDHandle != null)
            {
                byte[] EnvelopeBuffer = new byte[4];
                //copy the pos velocity to buffer
                byte[] tempBuffer = BitConverter.GetBytes(posMaxVelocity);
                System.Buffer.BlockCopy(tempBuffer, Constants.VALUE_0, EnvelopeBuffer, Constants.VALUE_0, Constants.VALUE_2);
                //copy the neg velocity to buffer
                tempBuffer = BitConverter.GetBytes(negMaxVelocity);
                System.Buffer.BlockCopy(tempBuffer, Constants.VALUE_0, EnvelopeBuffer, Constants.VALUE_2, Constants.VALUE_2);

                try
                {
                    //Logs.Instance.ErrorLog<TCDHandler>("async SetEnvelopeRangeAsync begins for channel:" + channel.ToString() + "posMaxVelocity: " + posMaxVelocity.ToString()
                       // + "negMaxVelocity:" + negMaxVelocity.ToString(), "SetPRFAsync", Severity.Debug);
                    const int REQUEST_INDEX = 9;
                    const int REQUEST_VALUE = 0;
                    const int REQUEST_LENGTH = 4;

                    UsbSetupPacket setupPacket = SetupPacket(DMIProtocol.DMI_CMD_DOPPLER,
                    REQUEST_INDEX, REQUEST_VALUE, DMIProtocol.DOPPLER_COMMAND, UsbTransferDirection.Out, REQUEST_LENGTH);
                    bytesTransferred = await GeneralTCDHandle.SendControlOutTransferAsync(setupPacket, EnvelopeBuffer.AsBuffer());
                    //Logs.Instance.ErrorLog<TCDHandler>("async SetEnvelopeRangeAsync ends", "SetEnvelopeRangeAsync", Severity.Debug);
                }
                catch (Exception ex)
                {
                    //Logs.Instance.ErrorLog<TCDHandler>(ex, "SetEnvelopeRangeAsync", Severity.Warning);
                    return 0;
                }
            }
            return bytesTransferred;
        }

        /// <summary>
        /// Reads the data from the active channels of the TCD
        /// </summary>
        /// <returns>Task.</returns>
        internal async Task ReadTCDData()
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadTCDData begins ", "ReadTCDData", Severity.Debug);
                await ReadChannel1TCDData();
                await ReadChannel2TCDData();

                if (Channel1.IsChannelEnabled | Channel2.IsChannelEnabled)
                {
                    await OnTCDReadDual();
                }
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadTCDData ends ", "ReadTCDData", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "ReadTCDData", Severity.Warning);
            }
        }

        /// <summary>
        /// Reads the message data.
        /// </summary>
        /// <returns>Task.</returns>
        internal async Task ReadMessageData()
        {
            byte[] x = await ReadServiceLog(TCDHandles.Channel1, Constants.VALUE_1);        //TODO: make sure packet count should be 1

            if (x != null)
            {
                ChannelMessage(x, Constants.VALUE_1);
            }

            x = await ReadServiceLog(TCDHandles.Channel2, Constants.VALUE_1);

            if (x != null)
            {
                ChannelMessage(x, Constants.VALUE_2);
            }
        }

        /// <summary>
        /// Reads the channel2 TCD data.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task ReadChannel2TCDData()
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadChannel2TCDData begins ", "ReadChannel2TCDData", Severity.Debug);
                if (Channel2.TCDHandleChannel != null)
                {
                    uint bytesReadCh2 = 0;
                    bytesReadCh2 = await Channel2.ChannelReader.LoadAsync((uint)DMIProtocol.PACKET_SIZE);
                    Channel2.ReadBuffer = Channel2.ChannelReader.ReadBuffer(bytesReadCh2);

                    if (Channel2.ReadBuffer != null)
                    {
                        Channel2.TempBufferCh = Channel2.ReadBuffer.ToArray();

                        for (int i = 0; i < Channel2.TempBufferCh.Length; i++)
                        {
                            CircularQueueChannel2.Channel2Queue.Enqueue(Channel2.TempBufferCh[i]);
                        }
                    }
                }
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadChannel2TCDData ends ", "ReadChannel2TCDData", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "ReaderChannel2 - ReadTCDData", Severity.Warning);
            }
        }

        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="Exception">The exception.</param>
        /// <param name="ProbInfo">The prob information.</param>
        /// <param name="ChannelName">Name of the channel.</param>
        private void WriteLog(Exception Exception, string ProbInfo, string ChannelName)
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async WriteLog begins ", "WriteLog", Severity.Debug);
                if (Exception.Message == "A device attached to the system is not functioning. (Exception from HRESULT: 0x8007001F)")
                {
                    //Logs.Instance.ErrorLog<TCDHandler>(ProbInfo, "ReaderChannel", Severity.Error);
                }
                if (Exception.Message == "The system cannot find the file specified. (Exception from HRESULT: 0x80070002)")
                {
                    isTCDWorking = false;
                    //Logs.Instance.ErrorLog<TCDHandler>("TCD Turned off", "ReaderChannel", Severity.Error);
                }
                //Logs.Instance.ErrorLog<TCDHandler>("async WriteLog ends ", "WriteLog", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "WriteLog", Severity.Warning);
            }
        }

        /// <summary>
        /// Reads the channel1 TCD data.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task ReadChannel1TCDData()
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadChannel1TCDData begins ", "ReadChannel1TCDData", Severity.Debug);
                if (Channel1.TCDHandleChannel != null)
                {
                    uint bytesReadCh1 = 0;

                    bytesReadCh1 = await Channel1.ChannelReader.LoadAsync((uint)DMIProtocol.PACKET_SIZE);
                    Channel1.ReadBuffer = Channel1.ChannelReader.ReadBuffer(bytesReadCh1);

                    if (Channel1.ReadBuffer != null)
                    {
                        Channel1.TempBufferCh = Channel1.ReadBuffer.ToArray();
                        for (int i = 0; i < Channel1.TempBufferCh.Length; i++)
                        {
                            CircularQueueChannel1.Channel1Queue.Enqueue(Channel1.TempBufferCh[i]);
                        }
                    }
                }
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadChannel1TCDData ends ", "ReadChannel1TCDData", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "ReaderChannel1 - ReadTCDData", Severity.Warning);
            }
        }

        /// <summary>
        /// Disables the channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        internal void DisableChannel(TCDHandles channel)
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async DisableChannel begins for channel:" + channel.ToString(), "DisableChannel", Severity.Debug);
                if (channel == TCDHandles.Channel1)
                {
                    Channel1.ResetForSensing();
                }
                else if (channel == TCDHandles.Channel2)
                {
                    Channel2.ResetForSensing();
                }
                else
                {
                    //Logs.Instance.ErrorLog<TCDHandler>(ExecutionConstants.TCDHandler + ExecutionConstants.MethodName,
                  //  "DisableChannel:Received garbage value for Channel|" + channel.ToString() + "|" + ExecutionConstants.MethodExecutionFail, Severity.Warning);
                }
                //Logs.Instance.ErrorLog<TCDHandler>("async DisableChannel ends ", "DisableChannel", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "DisableChannel", Severity.Warning);
            }
        }

        /// <summary>
        /// Reads the service log.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="packetCount">The packet count.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        internal async Task<byte[]> ReadServiceLog(TCDHandles channel, int packetCount)
        {
            try
            {
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadServiceLog begins for channel:" + channel.ToString(), "ReadServiceLog", Severity.Debug);
                TCDDevice tempParameter = GetCurrentDevice(channel);
                if (tempParameter.CurrentMode != TCDModes.Service)
                {
                    await SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_SET_MODE, (uint)TCDModes.Service, 0);
                }
                await SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_SERVICE_READ_LOG, 0, (uint)packetCount);

                if (tempParameter.TCDHandleChannel != null && tempParameter.canReadLog)
                {
                    tempParameter.canReadLog = false;
                    uint bytesRead = 0;
                    uint numBytes = (uint)(DMIProtocol.DMI_SERVICE_LOG_PACKET_SIZE * packetCount);
                    bytesRead = await tempParameter.ServiceLogReader.LoadAsync(numBytes);
                    if (bytesRead == 0)
                    {
                        tempParameter.canReadLog = true;
                        return null;
                    }
                    tempParameter.ServiceBuffer = tempParameter.ServiceLogReader.ReadBuffer(bytesRead);

                    if (tempParameter.ServiceBuffer != null)
                    {
                        tempParameter.ServiceBufferCh = tempParameter.ServiceBuffer.ToArray();
                    }
                    tempParameter.canReadLog = true;
                }
                //Logs.Instance.ErrorLog<TCDHandler>("async ReadServiceLog ends ", "ReadServiceLog", Severity.Debug);
                return tempParameter.ServiceBufferCh;
            }
            catch (Exception ex)
            {
                WriteLog(ex, MessageConstants.Probe1Disconnected, "ReadServiceLog");
                return null;
            }
        }

        #endregion Methods
    }
}