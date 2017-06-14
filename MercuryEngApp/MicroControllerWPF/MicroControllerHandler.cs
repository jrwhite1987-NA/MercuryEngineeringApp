// ***********************************************************************
// Assembly         : MicrochipController
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="MicroControllerHandler.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Common;
using Core.Constants;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Usb;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace MicrochipController
{
    /// <summary>
    /// Class MicroControllerHandler.
    /// </summary>
    class MicroControllerHandler
    {
        #region Properties
        /// <summary>
        /// The controller handler
        /// </summary>
        internal UsbDevice controllerHandler;
        /// <summary>
        /// The interf
        /// </summary>
        private UsbInterface interf;
        /// <summary>
        /// The in pipe
        /// </summary>
        private UsbBulkInPipe inPipe;
        /// <summary>
        /// The out pipe
        /// </summary>
        private UsbBulkOutPipe outPipe;
        /// <summary>
        /// The out stream
        /// </summary>
        private IOutputStream outStream;
        /// <summary>
        /// The in stream
        /// </summary>
        private IInputStream inStream;
        /// <summary>
        /// The writer
        /// </summary>
        private DataWriter writer;
        /// <summary>
        /// The reader
        /// </summary>
        private DataReader reader;
        /// <summary>
        /// The buffer charge
        /// </summary>
        private IBuffer bufferCharge;
        /// <summary>
        /// The buffer current
        /// </summary>
        private IBuffer bufferCurrent;
        /// <summary>
        /// The buffer version
        /// </summary>
        private IBuffer bufferVersion;
        /// <summary>
        /// The battery charge data
        /// </summary>
        private byte[] batteryChargeData;
        /// <summary>
        /// The battery current data
        /// </summary>
        private byte[] batteryCurrentData;
        /// <summary>
        /// The version information
        /// </summary>
        private byte[] versionInfo;
        /// <summary>
        /// The data position
        /// </summary>
        private const int DATA_POS = 1;
        /// <summary>
        /// The packet check position
        /// </summary>
        private const int PACKET_CHECK_POS = 3;

        /// <summary>
        /// The on device added removed
        /// </summary>
        internal FlagEvent onDeviceAddedRemoved;
        /// <summary>
        /// Gets a value indicating whether this instance is channel1 enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is channel1 enabled; otherwise, <c>false</c>.</value>
        internal bool IsChannel1Enabled { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is channel2 enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is channel2 enabled; otherwise, <c>false</c>.</value>
        internal bool IsChannel2Enabled { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is mo enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is mo enabled; otherwise, <c>false</c>.</value>
        internal bool IsMOEnabled { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is channel reset.
        /// </summary>
        /// <value><c>true</c> if this instance is channel reset; otherwise, <c>false</c>.</value>
        internal bool IsChannelReset { get; private set; }
        /// <summary>
        /// Gets the state of the battery.
        /// </summary>
        /// <value>The state of the battery.</value>
        internal int BatteryState { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is controller working.
        /// </summary>
        /// <value><c>true</c> if this instance is controller working; otherwise, <c>false</c>.</value>
        internal bool IsControllerWorking { get; set; }

        /// <summary>
        /// The power microcontroller watcher
        /// </summary>
        internal DeviceWatcher powerMicrocontrollerWatcher;
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
        /// The handle for controller
        /// </summary>
        private static MicroControllerHandler handleForController;

        /// <summary>
        /// The singleton creation lock
        /// </summary>
        private static Object singletonCreationLock = new Object();

        /// <summary>
        /// The controller information
        /// </summary>
        private DeviceInformation controllerInfo;
        /// <summary>
        /// Gets the current.
        /// </summary>
        /// <value>The current.</value>
        public static MicroControllerHandler Current
        {
            get
            {
                lock (singletonCreationLock)
                {
                    if (handleForController == null)
                    {
                        handleForController = new MicroControllerHandler();
                    }
                }
                return handleForController;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroControllerHandler" /> class.
        /// </summary>
        public MicroControllerHandler()
        {
            controllerHandler = null;
            IsChannel1Enabled = false;
            IsChannel2Enabled = false;
            IsMOEnabled = false;
            IsControllerWorking = false;
            BatteryState = 0;
            interf = null;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the device watcher.
        /// </summary>
        internal void InitializeDeviceWatcher()
        {
            Helper.logger.Debug("++");
            var microControllerQueryString = UsbDevice.GetDeviceSelector(MicroControllerProtocol.MCVendorID, MicroControllerProtocol.MCProductID);
            powerMicrocontrollerWatcher = DeviceInformation.CreateWatcher(microControllerQueryString);

            powerMicrocontrollerWatcher.Added += new TypedEventHandler<DeviceWatcher, DeviceInformation>
                              (this.OnPowerControllerAdded);
            powerMicrocontrollerWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>
                              (this.OnPowerControllerRemoved);
            powerMicrocontrollerWatcher.Start();
            Helper.logger.Debug("--");
        }
        
        #region Event Handlers
        /// <summary>
        /// Called when [power controller removed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        void OnPowerControllerRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Helper.logger.Debug("++");
            IsControllerWorking = false;
            onDeviceAddedRemoved(false);
            ReleaseDevice();
            Helper.logger.Debug("--");
             Helper.logger.Debug("Remote control disconnected.");
        }

        /// <summary>
        /// Called when [power controller added].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        void OnPowerControllerAdded(DeviceWatcher sender, DeviceInformation args)
        {
            Helper.logger.Debug("++");
            controllerInfo = args;
            onDeviceAddedRemoved(true);
            Helper.logger.Debug("--");
             Helper.logger.Debug("Remote control connected.");
        }
        #endregion

        #region USB Methods
        /// <summary>
        /// get device handle as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> GetDeviceHandleAsync()
        {
            Helper.logger.Debug("++");
            try
            {
                 Helper.logger.Debug("async GetDeviceHandleAsync begins" );

                bool result = false;
                controllerHandler = await UsbDevice.FromIdAsync(controllerInfo.Id);
                if (controllerHandler == null)
                {
                    IsControllerWorking = false;
                    result = false;
                }
                else
                {
                    interf = controllerHandler.Configuration.UsbInterfaces[0];
                    inPipe = interf.BulkInPipes[0];
                    inPipe.ReadOptions |= UsbReadOptions.IgnoreShortPacket;
                    outPipe = interf.BulkOutPipes[0];
                    outStream = outPipe.OutputStream;
                    inStream = inPipe.InputStream;
                    writer = new DataWriter(outStream);
                    reader = new DataReader(inStream);

                    IsControllerWorking = true;
                    result = true;
                }
                Helper.logger.Debug("--");
                 Helper.logger.Debug("async GetDeviceHandleAsync ends");
                return result;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);                
                return false;
            }
        }

        /// <summary>
        /// Releases the device.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool ReleaseDevice()
        {
            Helper.logger.Debug("++");
            try
            {
                 Helper.logger.Debug("async ReleaseDevice begins ");
                if (IsControllerWorking)
                {
                    controllerHandler.Dispose();
                    controllerInfo = null;
                    controllerHandler = null;
                    IsControllerWorking = false;
                    interf = null;
                    inPipe = null;
                    outPipe = null;
                    outStream = null;
                    inStream = null;
                    reader = null;
                    writer = null;                    
                    Helper.logger.Debug("async ReleaseDevice ends ");
                    return true;
                }
                else
                {
                    Helper.logger.Debug("--");
                     
                    return false;
                }

            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);               
                return false;
            }
        }

        /// <summary>
        /// Gets the descriptors.
        /// Debug statements not required as the method assigns value to the variables
        /// </summary>
        private void GetDescriptors()
        {
            Helper.logger.Debug("++");
            deviceDescriptor = controllerHandler.DeviceDescriptor;
            deviceConfig = controllerHandler.Configuration;
            configDescriptor = controllerHandler.Configuration.ConfigurationDescriptor;
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Gets the descriptors as string.
        /// </summary>
        /// <returns>System.String.</returns>
        internal string GetDescriptorsAsString()
        {
            Helper.logger.Debug("++");
            try
            {
                 Helper.logger.Debug("async GetDescriptorsAsString begins ");
                GetDescriptors();
                var usbInterface = controllerHandler.DefaultInterface;
                var bulkInPipes = usbInterface.BulkInPipes;
                var bulkOutPipes = usbInterface.BulkOutPipes;
                var interruptInPipes = usbInterface.InterruptInPipes;
                var interruptOutPipes = usbInterface.InterruptOutPipes;
                StringBuilder descriptorString = new StringBuilder();
                descriptorString.Append("Device Descriptor\n");
                descriptorString.Append("\nUsb Spec Number : 0x");
                descriptorString.Append(deviceDescriptor.BcdUsb.ToString("X4", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nMax Packet Size (Endpoint 0) : ");
                descriptorString.Append(deviceDescriptor.MaxPacketSize0.ToString("D", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nVendor ID : 0x");
                descriptorString.Append(deviceDescriptor.VendorId.ToString("X4", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nProduct ID : 0x");
                descriptorString.Append(deviceDescriptor.ProductId.ToString("X4", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nDevice Revision : 0x");
                descriptorString.Append(deviceDescriptor.BcdDeviceRevision.ToString("X4", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nNumber of Configurations : ");
                descriptorString.Append(deviceDescriptor.NumberOfConfigurations.ToString("D", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nBulk In Pipe : ");
                descriptorString.Append(controllerHandler.DefaultInterface.Descriptors.First());
                descriptorString.Append("\n\nConfiguration Descriptor\n");
                descriptorString.Append("\nNumber of Interfaces : ");
                descriptorString.Append(deviceConfig.UsbInterfaces.Count.ToString("D", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nConfiguration Value : 0x");
                descriptorString.Append(configDescriptor.ConfigurationValue.ToString("X2", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\nSelf Powered : ");
                descriptorString.Append(configDescriptor.SelfPowered.ToString());
                descriptorString.Append("\nRemote Wakeup : ");
                descriptorString.Append(configDescriptor.RemoteWakeup.ToString());
                descriptorString.Append("\nMax Power (milliAmps) : ");
                descriptorString.Append(configDescriptor.MaxPowerMilliamps.ToString("D", NumberFormatInfo.InvariantInfo));
                descriptorString.Append("\n\nEndpoint Descriptors for open pipes");

                // Print Bulk In Endpoint descriptors
                foreach (UsbBulkInPipe bulkInPipe in bulkInPipes)
                {
                    var endpointDescriptor = bulkInPipe.EndpointDescriptor;

                    descriptorString.Append("\n\nBulk In Endpoint Descriptor");
                    descriptorString.Append("\nEndpoint Number : 0x");
                    descriptorString.Append(endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo));
                    descriptorString.Append("\nMax Packet Size : ");
                    descriptorString.Append(endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo));
                }

                // Print Bulk Out Endpoint descriptors
                foreach (UsbBulkOutPipe bulkOutPipe in bulkOutPipes)
                {
                    var endpointDescriptor = bulkOutPipe.EndpointDescriptor;

                    descriptorString.Append("\n\nBulk Out Endpoint Descriptor");
                    descriptorString.Append("\nEndpoint Number : 0x");
                    descriptorString.Append(endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo));
                    descriptorString.Append("\nMax Packet Size : ");
                    descriptorString.Append(endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo));
                }

                // Print Interrupt In Endpoint descriptors
                foreach (UsbInterruptInPipe interruptInPipe in interruptInPipes)
                {
                    var endpointDescriptor = interruptInPipe.EndpointDescriptor;

                    descriptorString.Append("\n\nInterrupt In Endpoint Descriptor");
                    descriptorString.Append("\nEndpoint Number : 0x");
                    descriptorString.Append(endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo));
                    descriptorString.Append("\nMax Packet Size : ");
                    descriptorString.Append(endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo));
                    descriptorString.Append("\nInterval : ");
                    descriptorString.Append(endpointDescriptor.Interval.Duration().ToString());
                }

                // Print Interrupt Out Endpoint descriptors
                foreach (UsbInterruptOutPipe interruptOutPipe in interruptOutPipes)
                {
                    var endpointDescriptor = interruptOutPipe.EndpointDescriptor;

                    descriptorString.Append("\n\nInterrupt Out Endpoint Descriptor");
                    descriptorString.Append("\nEndpoint Number : 0x");
                    descriptorString.Append(endpointDescriptor.EndpointNumber.ToString("X2", NumberFormatInfo.InvariantInfo));
                    descriptorString.Append("\nMax Packet Size : ");
                    descriptorString.Append(endpointDescriptor.MaxPacketSize.ToString("D", NumberFormatInfo.InvariantInfo));
                    descriptorString.Append("\nInterval : ");
                    descriptorString.Append(endpointDescriptor.Interval.Duration().ToString());
                }

                Helper.logger.Debug("--");                
                return descriptorString.ToString();
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);                
                return "";
            }

        }

        /// <summary>
        /// Sends a non-TCD request to the microcontroller .
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <param name="dataLength">Length of the data.</param>
        /// <returns>byte array on valid data</returns>
        internal async Task<byte[]> SendMicroControllerRequest(byte cmd, int dataLength)
        {
            Helper.logger.Debug("++");
            try
            {
                const int USB_PACKET_MICRO_CONTROLLER_REQUEST = 0;
                const int USB_PACKET_CMD = 1;
                const int USB_PACKET_EOM = 2;
                uint bytesRead = 0;
                byte[] usbPacket = new byte[MicroControllerProtocol.REQUEST_MSG_LENGTH];

                usbPacket[USB_PACKET_MICRO_CONTROLLER_REQUEST] = MicroControllerProtocol.MicroControllerRequest;
                usbPacket[USB_PACKET_CMD] = cmd;
                usbPacket[USB_PACKET_EOM] = MicroControllerProtocol.EOM;
                writer.WriteBytes(usbPacket);
                await writer.StoreAsync();

                await Task.Delay(Constants.TimeWaitForLoad);
                bytesRead = await reader.LoadAsync(MicroControllerProtocol.RESPONSE_MSG_LENGTH);
                IBuffer buf = reader.ReadBuffer(bytesRead);
                byte[] rawData = buf.ToArray();

                Helper.logger.Debug("--");
                if (VerifyInputStream(rawData[0], rawData[dataLength + 1], cmd))
                {
                    return rawData;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);               
                return null;
            }
        }

        /// <summary>
        /// Verifies that the input array from the microcontroller is valid.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>bool</returns>
        bool VerifyInputStream(byte start, byte end, byte cmd)
        {
            if (start != cmd)
            {
                return false;
            }

            //int x = start ^ end;            
            return ((start ^ end) == 0xFF);
        }

        #endregion

        /// <summary>
        /// Sends the power parameters.
        /// </summary>
        /// <param name="channel1Power">if set to <c>true</c> [channel1 power].</param>
        /// <param name="channel2Power">if set to <c>true</c> [channel2 power].</param>
        /// <param name="channel1Reset">if set to <c>true</c> [channel1 reset].</param>
        /// <param name="channel2Reset">if set to <c>true</c> [channel2 reset].</param>
        /// <param name="moenable">if set to <c>true</c> [moenable].</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> SendPowerParameters(bool channel1Power, bool channel2Power, bool channel1Reset, bool channel2Reset, bool moenable)
        {
            Helper.logger.Debug("++");
            try
            {
                 Helper.logger.Debug("async SendPowerParameters begins for channel1Power:" + channel1Power.ToString()
                  + "channel2Power:" + channel2Power.ToString() + "channel1Reset:" + channel1Reset.ToString() + "channel2Reset:" + channel2Reset.ToString()
                  + "moenable:" + moenable.ToString());

                const byte CHANNEL_1_POWER = 0x01;
                const byte CHANNEL_2_POWER = 0x04;
                const byte MOENABLE = 0x20;
                const byte CHANNEL_1_RESET = 0x02;
                const byte CHANNEL_2_RESET = 0x08;

                const int USB_PACKET_MICRO_CONTROLLER_COMMAND = 0;
                const int USB_PACKET_TCD_CONTROL_REQ = 1;
                const int USB_PACKET_MSG_BYTE = 2;
                const int USB_PACKET_EOM = 3;
                byte[] usbPacket = new byte[MicroControllerProtocol.COMMAND_MSG_LENGTH];

                byte msgByte = 0;
                if (channel1Power)
                {
                    msgByte |= CHANNEL_1_POWER;
                }
                if (channel2Power)
                {
                    msgByte |= CHANNEL_2_POWER;
                }
                if (moenable)
                {
                    msgByte |= MOENABLE;
                }
                if (channel1Reset)
                {
                    msgByte |= CHANNEL_1_RESET;
                }
                if (channel2Reset)
                {
                    msgByte |= CHANNEL_2_RESET;
                }

                usbPacket[USB_PACKET_MICRO_CONTROLLER_COMMAND] = MicroControllerProtocol.MicroControllerCommand;
                usbPacket[USB_PACKET_TCD_CONTROL_REQ] = MicroControllerProtocol.TCDControlReq;
                usbPacket[USB_PACKET_MSG_BYTE] = msgByte;
                usbPacket[USB_PACKET_EOM] = MicroControllerProtocol.EOM;

                writer.WriteBytes(usbPacket);
                await writer.StoreAsync();

                await Task.Delay(Constants.TimeWaitForLoad);
                uint bytesRead = await reader.LoadAsync(MicroControllerProtocol.RESPONSE_MSG_LENGTH);
                IBuffer buf = reader.ReadBuffer(bytesRead);
                byte[] rawData = buf.ToArray();

                Helper.logger.Debug("--");
               
                if (VerifyInputStream(rawData[0], rawData[1], MicroControllerProtocol.TCDControlReq))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Debug("Exception: ", ex);               
                return false;
            }
        }

        /// <summary>
        /// Gets the version information.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        internal async Task<string> GetVersionInfo()
        {
            Helper.logger.Debug("++");
            String result = MessageConstants.NotAvailable;
            try
            {
                 Helper.logger.Debug("async GetVersionInfo begins ");
                byte[] data = await SendMicroControllerRequest(MicroControllerProtocol.VersionNumberRequest, 3);
                if (data != null)
                {
                    int majorVersion = data[1]; //get higher 4 bits
                    int minorVersion = data[2]; //get lower 4 bits
                    String buildNumber = data[3].ToString("D3");
                    result = String.Format("{0}.{1}.{2}", majorVersion, minorVersion, buildNumber);
                }
                Helper.logger.Debug("--");
                return result;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);                
                return MessageConstants.NotAvailable;
            }
        }

        /// <summary>
        /// Gets the voltage level of the battery.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        internal async Task<int> GetVoltageLevel()
        {
            Helper.logger.Debug("++");
            int voltage = 0;
            try
            {
                 Helper.logger.Debug("async GetVoltageLevel begins ");
                byte[] data = await SendMicroControllerRequest(MicroControllerProtocol.BatteryVoltageValueRequest, sizeof(Int16));

                if (data != null)
                {
                    voltage = BitConverter.ToInt16(data, DATA_POS);
                }
                else
                {
                    voltage = short.MaxValue;
                }
                Helper.logger.Debug("--");
                return voltage;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);               
                return short.MaxValue;
            }
        }

        /// <summary>
        /// Gets the current of the battery.
        /// </summary>
        /// <returns>Task&lt;ControllerEnumList.BatteryChargingState&gt;.</returns>
        internal async Task<int> GetBatteryCurrent()
        {
            Helper.logger.Debug("++");
            int current = 0;
            try
            {
                 Helper.logger.Debug("async GetBatteryCurrent begins ");
                byte[] data = await SendMicroControllerRequest(MicroControllerProtocol.BatteryCurrentRequest, sizeof(Int16));

                if (data != null)
                {
                    current = BitConverter.ToInt16(data, DATA_POS);
                }
                else
                {
                    current = short.MaxValue;
                }

                Helper.logger.Debug("--");
                return current;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);               
                return short.MaxValue;
            }
        }

        /// <summary>
        /// Gets the battery charge.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        internal async Task<int> GetBatteryCharge()
        {
            Helper.logger.Debug("++");
            int result = 0;
            try
            {   
                byte[] data = await SendMicroControllerRequest(MicroControllerProtocol.BatteryStateRequest, sizeof(byte));

                if (data != null)
                {
                    result = data[DATA_POS];
                }

                Helper.logger.Debug("--");
                return result;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);               
                return 0;
            }
        }

        /// <summary>
        /// Determines the number of mAh the battery has left.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        internal async Task<int> GetRemainingCharge()
        {
            Helper.logger.Debug("++");
            int remainingCharge = 0;
            try
            {
                byte[] data = await SendMicroControllerRequest(MicroControllerProtocol.RemainingChargeRequest, sizeof(Int16));

                if (data != null)
                {
                    remainingCharge = BitConverter.ToInt16(data, DATA_POS);
                }

                Helper.logger.Debug("--");
                return remainingCharge;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);                
                return 0;
            }
        }

        /// <summary>
        /// Determines the capacity of the battery in mAh.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        internal async Task<int> GetFullCharge()
        {
            Helper.logger.Debug("++");
            int fullCharge = 0;
            try
            {
                byte[] data = await SendMicroControllerRequest(MicroControllerProtocol.FullChargeRequest, sizeof(Int16));

                if (data != null)
                {
                    fullCharge = BitConverter.ToInt16(data, DATA_POS);
                }
                Helper.logger.Debug("--");
                return fullCharge;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);                
                return 0;
            }
        }

        /// <summary>
        /// Determines if external voltage is applied.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        internal async Task<int> GetVoltageState()
        {
            Helper.logger.Debug("++");
            int voltageMask = 0;
            try
            {
                byte[] data = await SendMicroControllerRequest(MicroControllerProtocol.BatteryVoltageRequest, sizeof(byte));
                if (data != null)
                {
                    voltageMask = BitConverter.ToInt16(data, DATA_POS);
                }
                else
                {
                    voltageMask = short.MaxValue;
                }

                Helper.logger.Debug("--");
                return voltageMask;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception", ex);               
                return short.MaxValue;
            }
        }
        #endregion
    }
}
