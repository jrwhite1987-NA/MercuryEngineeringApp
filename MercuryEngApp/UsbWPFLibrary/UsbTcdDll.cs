// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="UsbTcdDll.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Common;
using Core.Constants;
using Core.Models.ReportModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsbTcdLibrary.CommunicationProtocol;
using UsbTcdLibrary.PacketFormats;
using UsbTcdLibrary.StatusClasses;

namespace UsbTcdLibrary
{
    /// <summary>
    /// Class UsbTcdDll.
    /// </summary>
    /// <seealso cref="UsbTcdLibrary.IUsbTcd" />
    public class UsbTcdDll : IUsbTcd
    {
        #region Properties

        /// <summary>
        /// The doppler module
        /// </summary>
        private DopplerModule dopplerModule = new DopplerModule();   //ask for naming convention of the object

        /// <summary>
        /// The use mock TCD
        /// </summary>
        internal bool useMockTCD = false;

        /// <summary>
        /// The is recording on
        /// </summary>
        private bool isRecordingOn = false;

        /// <summary>
        /// The mock object
        /// </summary>
        private MockTCD mockObject = new MockTCD();

        /// <summary>
        /// The time for one packet
        /// </summary>
        private const int TimeForOnePacket = 8;

        /// <summary>
        /// Occurs when [on packet formed].
        /// </summary>
        public event TCDPacketFormed OnPacketFormed;

        /// <summary>
        /// Constructor to use the DLL
        /// </summary>
        /// <param name="interfaceObject">Static interface variable</param>
        public UsbTcdDll(IUsbTcd interfaceObject)
        {
        }

        /// <summary>
        /// Gets the current active channel(s) from the TCD
        /// </summary>
        /// <value>The active channel.</value>
        public ActiveChannels ActiveChannel
        {
            get
            {
                DopplerModule.t1.Stop();
                if (TCDHandler.Current.Channel1.IsChannelEnabled & TCDHandler.Current.Channel2.IsChannelEnabled)
                {
                    return ActiveChannels.Both;
                }
                else if (TCDHandler.Current.Channel1.IsChannelEnabled | TCDHandler.Current.Channel2.IsChannelEnabled)
                {
                    if (TCDHandler.Current.Channel1.IsChannelEnabled)
                    {
                        return ActiveChannels.Channel1;
                    }
                    else
                    {
                        return ActiveChannels.Channel2;
                    }
                }
                else
                {
                    return ActiveChannels.None;
                }
            }
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>The time.</value>
        public long Time
        {
            get
            {
                return DopplerModule.t1.ElapsedMilliseconds / TimeForOnePacket;
            }
        }

        /// <summary>
        /// Gets the packet queue channel1.
        /// </summary>
        /// <value>The packet queue channel1.</value>
        public List<DMIPmdDataPacket> PacketQueueChannel1
        {
            get { return DopplerModule.packetQueueChannel1; }
        }

        /// <summary>
        /// Gets the packet queue channel2.
        /// </summary>
        /// <value>The packet queue channel2.</value>
        public List<DMIPmdDataPacket> PacketQueueChannel2
        {
            get { return DopplerModule.packetQueueChannel2; }
        }

        /// <summary>
        /// Gets the packet queue channel1.
        /// </summary>
        /// <value>The packet queue channel1.</value>
        public List<short> CVRDataChannel1
        {
            get { return DopplerModule.cvrDataChannel1; }
        }

        /// <summary>
        /// Gets the packet queue channel2.
        /// </summary>
        /// <value>The packet queue channel2.</value>
        public List<short> CVRDataChannel2
        {
            get { return DopplerModule.cvrDataChannel2; }
        }


        /// <summary>
        /// Gets the packet queue.
        /// </summary>
        /// <value>The packet queue.</value>
        public Dictionary<int, List<DMIPmdDataPacket>> PacketQueue
        {
            get { return dopplerModule.packetQueue; }
        }

        /// <summary>
        /// Gets true if communication with TCD fails
        /// </summary>
        /// <value><c>true</c> if this instance is handle null; otherwise, <c>false</c>.</value>
        public bool IsTCDPowerOn
        {
            get { return !TCDHandler.Current.isTCDWorking; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public UsbTcdDll()
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Turns on the TCD and establishes connection with each of the active channels
        /// </summary>
        /// <returns>True if the TCD is attached to the system and communication is established
        /// False if it encounters any error</returns>
        public async Task<TCDResponse> TurnTCDPowerOnAsync()
        {
            TCDResponse responseObject = new TCDResponse();
            try
            {
                bool result;
                if (TCDHandler.Current.isTCDWorking)
                {
                    TCDHandler.Current.CloseDevice();
                    TCDHandler.Current.isTCDWorking = false;
                }
                result = await TCDHandler.Current.GetDeviceHandleAsync();
                SealProbes();
                if (result | useMockTCD)
                {
                    if (TCDHandler.Current.Channel1.IsChannelEnabled | TCDHandler.Current.Channel2.IsChannelEnabled | useMockTCD)
                    {
                        if (useMockTCD)
                        {
                            mockObject.OnPacketFormationDual += DopplerModuleOnPacketFormation;
                        }
                        else
                        {
                            dopplerModule.OnPacketFormationDual += DopplerModuleOnPacketFormation;
                            TCDHandler.Current.InitializeStreams();
                        }
                    }
                    responseObject.ActiveChannel = DetermineActiveChannels();
                }
                else
                {
                    responseObject.ActiveChannel = ActiveChannels.NoTCD;
                }
                responseObject.Result = result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "TurnTCDPowerOn", Severity.Critical);
                responseObject.Result = false;
                responseObject.Error = ex;
            }
            return responseObject;
        }

        /// <summary>
        /// Raise event on the UI sideGetModuleInfoOfTCD
        /// </summary>
        /// <param name="packets">The packets.</param>
        private void DopplerModuleOnPacketFormation(DMIPmdDataPacket[] packets)
        {
            OnPacketFormed(packets);
        }

        /// <summary>
        /// Turns off the TCD power
        /// </summary>
        /// <returns>True if successfully released the TCD False if attempt unsuccessful</returns>
        public TCDResponse TurnTCDPowerOff()
        {
            TCDResponse responseObj = null;
            try
            {
                mockObject.isMockActive = false;
                responseObj = new TCDResponse();
                //Logs.Instance.ErrorLog<UsbTcdDll>("TurnTCDPowerOff begins for examId", "TurnTCDPowerOff", Severity.Debug);
                if (TCDHandler.Current.isTCDWorking)
                {
                    if (TCDHandler.Current.Channel1.IsChannelEnabled ||
                        TCDHandler.Current.Channel2.IsChannelEnabled)
                    {
                        dopplerModule.OnPacketFormationDual -= DopplerModuleOnPacketFormation;
                    }
                    responseObj.Result = dopplerModule.ReleaseTCDHandle();
                    //Logs.Instance.ErrorLog<UsbTcdDll>("TurnTCDPowerOff ends ", "TurnTCDPowerOff", Severity.Debug);
                }
                else
                {
                    //Logs.Instance.ErrorLog<UsbTcdDll>("TurnTCDPowerOff ends ", "TurnTCDPowerOff", Severity.Debug);
                    responseObj.Result = true;
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "TurnTCDPowerOff", Severity.Critical);
                responseObj.Result = false;
                responseObj.Error = ex;
            }
            return responseObj;
        }

        /// <summary>
        /// Starts saving the TCD data into the file for future reference
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> TurnRecordingOnAsync(TCDRequest requestObject)
        {
            TCDResponse responseObj = null;
            try
            {
                responseObj = new TCDResponse();
                //Logs.Instance.ErrorLog<UsbTcdDll>("TurnRecordingOn begins for examId:" + requestObject.Value.ToString(), "TurnRecordingOn", Severity.Debug);
                if (TCDHandler.Current.isTCDWorking)
                {
                    if (await dopplerModule.CreateBinaryFileOfExam(requestObject.Value))
                    {
                        await dopplerModule.InitializeStreams();
                        isRecordingOn = true;
                        dopplerModule.OnRecordingEnabled += dopplerModule.WriteToFile;
                    }
                }
                //Logs.Instance.ErrorLog<UsbTcdDll>("TurnRecordingOn ends ", "TurnRecordingOn", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "TurnRecordingOn", Severity.Warning);
                responseObj.Error = ex;
            }
            return responseObj;
        }

        /// <summary>
        /// Stops saving the incoming TCD data into file
        /// </summary>
        /// <returns>Task.</returns>
        public async Task<TCDResponse> TurnRecordingOffAsync()
        {
            TCDResponse responseObj = null;
            try
            {
                responseObj = new TCDResponse();
                dopplerModule.OnRecordingEnabled -= dopplerModule.WriteToFile;
                isRecordingOn = false;
                await dopplerModule.ReleaseFileWritingResources();
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "TurnRecordingOff", Severity.Warning);
                responseObj.Error = ex;
            }
            return responseObj;
        }

        /// <summary>
        /// Sets the PRF of the channel provided
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>System.UInt32.</returns>
        public async Task<TCDResponse> SetPRF(TCDRequest requestObj)
        {
            TCDResponse responseObj = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    await dopplerModule.SetPRF(requestObj.ChannelID, requestObj.Value3, requestObj.Value2);
                    responseObj.Result = true;
                }
                else
                { responseObj.Result = false; }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "SetPRF", Severity.Warning);
                responseObj.Error = ex;
            }
            return responseObj;
        }

        /// <summary>
        /// Clears the buffer of the specified TCD channel and endpoint
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>System.UInt32.</returns>
        public async Task<TCDResponse> ClearBufferAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            if (TCDHandler.Current.isTCDWorking)
            {
                await dopplerModule.SendClearBufferCommand(requestObj.ChannelID, requestObj.Endpoint);
                responseObject.Result = true;
            }
            else
            {
                responseObject.Result = false;
            }
            return responseObject;
        }

        /// <summary>
        /// Reads a full doppler packet into the buffer
        /// Inputs:
        /// Returns:
        /// </summary>
        public void StartTCDReading()
        {
            if (TCDHandler.Current.isTCDWorking)
            {
                dopplerModule.PacketsFromTCD();
            }
        }

        /// <summary>
        /// Gets the device descriptors, configuration descriptors and endpoint descriptors of the TCD connected formatted in a string
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns>System.String.</returns>
        public TCDResponse GetTCDDescriptors(TCDRequest requestObject)
        {
            TCDResponse responseObj = new TCDResponse();
            if (TCDHandler.Current.isTCDWorking)
            {
                responseObj.TCDDescriptor = TCDHandler.Current.GetDescriptorsAsString(requestObject.ChannelID);
            }
            else
            {
                responseObj.TCDDescriptor = "The TCD is not powered on properly. Please try again";
            }
            return responseObj;
        }

        /// <summary>
        /// Initialize the TCD by clearing all buffers and setting it to operate mode
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool InitializeTCD()
        {
            if (TCDHandler.Current.isTCDWorking | useMockTCD)
            {
                if (useMockTCD)
                {
                    mockObject.isMockActive = true;
                    mockObject.SendPacketsFromFile();
                    return true;
                }
                else
                {
                    return dopplerModule.StartDopplerData();
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reads from file.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> ReadFromFileAsync(int examId, int channelId)
        {
            try
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>("ReadFromFile begins for examId:" + examId.ToString() + " channelId:" + channelId.ToString(), "ReadFromFile", Severity.Debug);
                bool result = false;
                result = await dopplerModule.ReadFile(examId, channelId);
                //Logs.Instance.ErrorLog<UsbTcdDll>("ReadFromFile ends", "ReadFromFile", Severity.Debug);
                return result;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(e, "ReadFromFile", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Reads from file.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<TCDResponse> ReadFromFileCVRAsync(int examId, int channelId)
        {
            TCDResponse responseObj = new TCDResponse();
            try
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>("ReadFromFile begins for examId:" + examId.ToString() + " channelId:" + channelId.ToString(), "ReadFromFile", Severity.Debug);
                responseObj.Value = await dopplerModule.ReadFileCVR(examId, channelId);
                if(responseObj.Value>0)
                {
                    responseObj.Result = true;
                }
                //Logs.Instance.ErrorLog<UsbTcdDll>("ReadFromFile ends", "ReadFromFile", Severity.Debug);
            }
            catch (Exception e)
            {
                responseObj.Error = e;
                //Logs.Instance.ErrorLog<UsbTcdDll>(e, "ReadFromFile", Severity.Warning);
            }
            return responseObj;
        }

        /// <summary>
        /// read from file as an asynchronous operation.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="ReadPointerListCh1">The read pointer list CH1.</param>
        /// <param name="ReadPointerListCh2">The read pointer list CH2.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> ReadFromFileAsync(int examId, Dictionary<int, int> ReadPointerListCh1, Dictionary<int, int> ReadPointerListCh2)
        {
            try
            {
                bool result = false;
                result = await dopplerModule.ReadFile(examId, ReadPointerListCh1, ReadPointerListCh2);
                //Logs.Instance.ErrorLog<UsbTcdDll>("ReadFromFile ends", "ReadFromFile", Severity.Debug);
                return result;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(e, "ReadFromFile", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Read Binary file for exam with Range for 4/8/12 Sec
        /// </summary>
        /// <param name="examId">exam Id</param>
        /// <param name="ListReadPointerModelCh1">Exam screen shots parameter for ch1</param>
        /// <param name="ListReadPointerModelCh2">Exam screen shots parameter for ch2</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> ReadFromFileWithRangeAsync(int examId, List<ReadPointerModel> ListReadPointerModelCh1, List<ReadPointerModel> ListReadPointerModelCh2)
        {
            try
            {
                bool result = false;
                result = await dopplerModule.ReadFileWithRange(examId, ListReadPointerModelCh1, ListReadPointerModelCh2);
                //Logs.Instance.ErrorLog<UsbTcdDll>("ReadFromFile ends", "ReadFromFile", Severity.Debug);
                return result;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(e, "ReadFromFile", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Read Binary file for exam with Range for 4/8/12 Sec
        /// </summary>
        /// <param name="examId">exam Id</param>
        /// <param name="ListReadPointerModelCh1">Exam screen shots parameter for ch1</param>
        /// <param name="ListReadPointerModelCh2">Exam screen shots parameter for ch2</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> ReadFromFileWithRange(int examId, List<ReadPointerModel> ListReadPointerModelCh1)
        {
            try
            {
                bool result = false;
                result = await dopplerModule.ReadFileWithRange(examId, ListReadPointerModelCh1);
                //Logs.Instance.ErrorLog<UsbTcdDll>("ReadFromFile ends", "ReadFromFile", Severity.Debug);
                return result;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(e, "ReadFromFile", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Create binary file for both channels
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> CreateBinaryFileOfExam(int examId)
        {
            try
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>("CreateBinaryFileOfExam begins for examId:" + examId.ToString(), "CreateBinaryFileOfExam", Severity.Debug);
                bool result = await dopplerModule.CreateBinaryFileOfExam(examId);
                //Logs.Instance.ErrorLog<UsbTcdDll>("CreateBinaryFileOfExam ends ", "CreateBinaryFileOfExam", Severity.Debug);
                return result;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(e, "CreateBinaryFileOfExam", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// get active channel as an asynchronous operation.
        /// </summary>
        /// <returns>Task&lt;EnumList.ActiveChannels&gt;.</returns>
        public async Task<TCDResponse> GetProbesConnectedAsync()
        {
            TCDResponse responseObject = null;
            try
            {
                responseObject = new TCDResponse();
                //Logs.Instance.ErrorLog<UsbTcdDll>("GetProbesConnectedAsync begins ", "GetProbesConnectedAsync", Severity.Debug);
                ActiveChannels activeChannel = ActiveChannels.NoTCD;
                if (await TCDHandler.Current.GetDeviceHandleAsync())
                {
                    activeChannel = DetermineActiveChannels();
                    TCDHandler.Current.InitializeStreams();
                }

                if (useMockTCD)
                {
                    responseObject.ActiveChannel = ActiveChannels.Both;
                }
                else
                {
                    dopplerModule.ActiveChannel = activeChannel;
                    responseObject.ActiveChannel = activeChannel;
                }
                dopplerModule.OnProbePlugged += DopplerModuleOnProbePlugged;
                dopplerModule.OnProbeUnplugged += DopplerModuleOnProbeUnplugged;
                await dopplerModule.InitializeProbeEvents();
                //Logs.Instance.ErrorLog<UsbTcdDll>("GetProbesConnectedAsync ends ", "GetProbesConnectedAsync", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "GetActiveChannelAsync", Severity.Warning);
                responseObject.ActiveChannel = ActiveChannels.None;
                responseObject.Error = ex;
            }
            return responseObject;
        }

        /// <summary>
        /// Determines the active channels.
        /// </summary>
        /// <returns>ActiveChannels.</returns>
        private ActiveChannels DetermineActiveChannels()
        {
            ActiveChannels activeChannel;
            if (TCDHandler.Current.Channel1.IsChannelEnabled & TCDHandler.Current.Channel2.IsChannelEnabled)
            {
                activeChannel = ActiveChannels.Both;
            }
            else if (TCDHandler.Current.Channel1.IsChannelEnabled)
            {
                activeChannel = ActiveChannels.Channel1;
            }
            else if (TCDHandler.Current.Channel2.IsChannelEnabled)
            {
                activeChannel = ActiveChannels.Channel2;
            }
            else
            {
                activeChannel = ActiveChannels.None;
            }
            return activeChannel;
        }

        /// <summary>
        /// Dopplers the module on probe unplugged.
        /// </summary>
        /// <param name="probe">The probe.</param>
        private void DopplerModuleOnProbeUnplugged(TCDHandles probe)
        {
            if (OnProbeUnplugged != null)
            {
                OnProbeUnplugged(probe);
            }
        }

        /// <summary>
        /// Dopplers the module on probe plugged.
        /// </summary>
        /// <param name="probe">The probe.</param>
        private void DopplerModuleOnProbePlugged(TCDHandles probe)
        {
            if (OnProbePlugged != null)
            {
                OnProbePlugged(probe);
            }
        }

        /// <summary>
        /// Gets the module information of TCD.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns>ModuleInfo</returns>
        public async Task<TCDReadInfoResponse> GetModuleInfo(TCDRequest requestObject)
        {
            TCDReadInfoResponse readInfoResponse = new TCDReadInfoResponse();
            try
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>("GetModuleInfoOfTCD begins for channel:" + requestObject.ChannelID.ToString(), "IsProbeDisconnected", Severity.Debug);
                if (TCDHandler.Current.isTCDWorking)
                {
                    //Logs.Instance.ErrorLog<UsbTcdDll>("GetModuleInfoOfTCD ends for channel:" + requestObject.ChannelID.ToString(), "GetModuleInfoOfTCD", Severity.Debug);
                    if (requestObject.ChannelID == TCDHandles.Channel1)
                    {
                        await dopplerModule.SetMode(TCDHandles.Channel1, TCDModes.Active);
                        readInfoResponse.Module = await TCDHandler.Current.GetModuleInfo(TCDHandles.Channel1);
                    }
                    else
                    {
                        await dopplerModule.SetMode(TCDHandles.Channel2, TCDModes.Active);
                        readInfoResponse.Module = await TCDHandler.Current.GetModuleInfo(TCDHandles.Channel2);
                    }
                }
                else
                {
                    //Logs.Instance.ErrorLog<UsbTcdDll>("GetModuleInfoOfTCD ends for channel:" + requestObject.ChannelID.ToString(), "GetModuleInfoOfTCD", Severity.Debug);
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "GetModuleInfoOfTCD", Severity.Warning);
                readInfoResponse.Error = ex;
            }
            return readInfoResponse;
        }

        /// <summary>
        /// set envelope range as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> SetEnvelopeRangeAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            if (DMIProtocol.FFTSize == DMIProtocol.FFT256_POINTS)
            {
                try
                {
                    await TCDHandler.Current.SetEnvelopeRangeAsync(requestObj.ChannelID, (short)requestObj.Value3, (short)requestObj.Value);
                    responseObject.Result = true;
                }
                catch (Exception ex)
                {
                    //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "SetEnvelopeRangeAsync", Severity.Warning);
                    responseObject.Result = false;
                    responseObject.Error = ex;
                }
            }
            return responseObject;
        }

        /// <summary>
        /// is probe connected as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> IsProbeConnectedAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            try
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>("IsProbeConnected begins for channel:" + requestObj.ChannelID.ToString(), "IsProbeConnected", Severity.Debug);

                ServiceLogPacket ep8Packet = dopplerModule.GetLogFromArray(await TCDHandler.Current.ReadServiceLog(requestObj.ChannelID, 1), 0);

                await TCDHandler.Current.SendControlCommandAsync(requestObj.ChannelID, DMIProtocol.DMI_CMD_SET_MODE, (uint)TCDModes.Active, 0);

                if (ep8Packet.Message.MessageCode == DMIProtocol.DMI_EVENTCODE_PROBE_DISCONNECT)
                {
                    TCDHandler.Current.DisableChannel(requestObj.ChannelID);
                    responseObject.Result = false;
                }
                else
                {
                    responseObject.Result = true;
                }
                //Logs.Instance.ErrorLog<UsbTcdDll>("IsProbeConnected ends for channel:" + requestObj.ChannelID.ToString(), "IsProbeConnected", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "IsProbeConnected", Severity.Warning);
                responseObject.Result = true;
                responseObject.Error = ex;
            }
            return responseObject;
        }

        /// <summary>
        /// Turns the single channel off.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns>TCDResponse.</returns>
        public TCDResponse TurnSingleChannelOff(TCDRequest requestObject)
        {
            TCDResponse responseObj = null;
            try
            {
                responseObj = new TCDResponse();
                //Logs.Instance.ErrorLog<UsbTcdDll>("TurnSingleChannelOff begins for channel:" + requestObject.ChannelID.ToString(), "TurnSingleChannelOff", Severity.Debug);
                if (TCDHandler.Current.isTCDWorking)
                {
                    if (requestObject.ChannelID == TCDHandles.Channel1)
                    {
                        TCDHandler.Current.Channel1.ResetForSensing();
                    }
                    else if (requestObject.ChannelID == TCDHandles.Channel2)
                    {
                        TCDHandler.Current.Channel2.ResetForSensing();
                    }
                    else
                    {
                        //Logs.Instance.ErrorLog<UsbTcdDll>(ExecutionConstants.UsbTcdDll + ExecutionConstants.MethodName,
                      //  "TurnSingleChannelOff:Recieved garbage value for Channel|" + requestObject.ChannelID.ToString() + "|" + ExecutionConstants.MethodExecutionFail, Severity.Warning);
                    }
                }
                //Logs.Instance.ErrorLog<UsbTcdDll>("TurnSingleChannelOff ends for channel:" + requestObject.ChannelID.ToString(), "TurnSingleChannelOff", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "TurnSingleChannelOff", Severity.Warning);
                responseObj.Error = ex;
            }
            return responseObj;
        }

        #endregion Methods

        /// <summary>
        /// Turns the single channel on.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns>TCDResponse.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public TCDResponse TurnSingleChannelOn(TCDRequest requestObject)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether this instance is recording on.
        /// </summary>
        /// <value><c>true</c> if this instance is recording on; otherwise, <c>false</c>.</value>
        public bool IsRecordingOn
        {
            get { return isRecordingOn; }
        }

        /// <summary>
        /// Gets the probe information.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <returns>TCDReadInfoResponse.</returns>
        public async Task <TCDReadInfoResponse> GetProbeInfo(TCDRequest requestObject)
        {
            TCDReadInfoResponse response = new TCDReadInfoResponse();

            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    if (requestObject.ChannelID == TCDHandles.Channel1)
                    {
                        await dopplerModule.SetMode(TCDHandles.Channel1, TCDModes.Active);
                        response.Probe = await TCDHandler.Current.GetProbeInfoAsync(TCDHandler.Current.Channel1);
                    }
                    else 
                    {
                        if (requestObject.ChannelID == TCDHandles.Channel2)
                        {
                            await dopplerModule.SetMode(TCDHandles.Channel2, TCDModes.Active);
                            response.Probe = await TCDHandler.Current.GetProbeInfoAsync(TCDHandler.Current.Channel2);
                        }
                    }
                }
                else
                {
                    response.Probe = new ProbeInfo();
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "GetProbeInfo", Severity.Warning);
                response.Error = ex;
            }

            return response;
        }

        /// <summary>
        /// set depth as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> SetDepthAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            if (TCDHandler.Current.isTCDWorking)
            {
                await dopplerModule.SendDopplerCommand(requestObj.ChannelID, DopplerParameters.Depth, requestObj.Value3);
                responseObject.Result = true;
            }
            else
            {
                responseObject.Result = false;
            }
            return responseObject;
        }

        /// <summary>
        /// set power as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> SetPowerAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            if (TCDHandler.Current.isTCDWorking)
            {
                await dopplerModule.SendDopplerCommand(requestObj.ChannelID, DopplerParameters.Power, requestObj.Value3);
                responseObject.Result = true;
            }
            else
            {
                responseObject.Result = false;
            }
            return responseObject;
        }

        /// <summary>
        /// set filter as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> SetFilterAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            if (TCDHandler.Current.isTCDWorking)
            {
                await dopplerModule.SendDopplerCommand(requestObj.ChannelID, DopplerParameters.ClutterFilter, requestObj.Value3);
                responseObject.Result = true;
            }
            else
            {
                responseObject.Result = false;
            }
            return responseObject;
        }

        /// <summary>
        /// set length as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> SetLengthAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            if (TCDHandler.Current.isTCDWorking)
            {
                await dopplerModule.SendDopplerCommand(requestObj.ChannelID, DopplerParameters.SampleLength, requestObj.Value3);
                responseObject.Result = true;
            }
            else
            {
                responseObject.Result = false;
            }
            return responseObject;
        }

        /// <summary>
        /// Occurs when [on probe plugged].
        /// </summary>
        public event ProbePlugUnplug OnProbePlugged;

        /// <summary>
        /// Occurs when [on probe unplugged].
        /// </summary>
        public event ProbePlugUnplug OnProbeUnplugged;

        /// <summary>
        /// write value to fpga as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> WriteValueToFPGAAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    responseObject.Result = await dopplerModule.WriteFPGAValue(requestObj.ChannelID, requestObj.Value, requestObj.Value3);
                }
            }
            catch (Exception ex)
            {
                responseObject.Error = ex;
            }
            return responseObject;
        }

        /// <summary>
        /// reset fpga as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> ResetFPGAAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    responseObject.Result = await dopplerModule.ResetFPGAAsync(requestObj.ChannelID, requestObj.Value);
                }
            }
            catch (Exception ex)
            {
                responseObject.Error = ex;
            }
            return responseObject;
        }

        /// <summary>
        /// Cancel the currently active service activity.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> AbortServiceAsync(TCDRequest requestObj)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.AbortServiceAsync(requestObj.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Assigns channel number to the module specified.Assigns channel number to the module specified.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> AssignChannelAsync(TCDRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.AssignChannelAsync(request.ChannelID, request.Value2);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Writes the board information to the module specified.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> WriteBoardInfoAsync(TCDWriteInfoRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.WriteBoardInfoAsync(request.ChannelID, request.Board);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Writes the probe information to the module specified.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> WriteProbeInfoAsync(TCDWriteInfoRequest request)
        {
            TCDResponse response = new TCDResponse();

            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.WriteProbeInfoAsync(request.ChannelID, request.Probe);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }

            return response;
        }

        /// <summary>
        /// Measurement start for calibration of Doppler board.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> StartMeasurementOfBoardAsync(TCDRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.StartMeasurementOfBoard(request.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Measurement start for calibration of Doppler board.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> ApplyMeasurementToBoardAsync(TCDRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.ApplyMeasurementToBoard(request.ChannelID, (uint)request.Value, request.Value3);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Writes the calibration information to the board directly.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> CalibrateBoardAsync(TCDWriteInfoRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.CalibrateBoard(request.ChannelID, request.Calibration);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// read fpga value as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> ReadFPGAValueAsync(TCDRequest requestObj)
        {
            TCDResponse responseObject = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    responseObject.Value = await dopplerModule.ReadFPGAValueAsync(requestObj.ChannelID, requestObj.Value3);
                    responseObject.Result = true;
                }
            }
            catch (Exception ex)
            {
                responseObject.Error = ex;
            }
            return responseObject;
        }

        /// <summary>
        /// Reads the calibration information of the module specified
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDReadInfoResponse&gt;.</returns>
        public async Task<TCDReadInfoResponse> ReadCalibrationInfoAsync(TCDRequest request)
        {
            TCDReadInfoResponse response = new TCDReadInfoResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Calibration = await dopplerModule.ReadCalibrationInfo(request.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Provides the count of minutes the specified module has been on
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> ReadOperatingMinutesAsync(TCDRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Value = await dopplerModule.ReadOperatingMinutes(request.ChannelID);
                    response.Result = true;
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Enables the transmit testing.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> EnableTransmitTestControlAsync(TCDRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.EnableTransmitTestControl(request.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Disables the transmit testing
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> DisableTransmitTestControlAsync(TCDRequest request)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.DisableTransmitTestControl(request.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// transmit test power as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> TransmitTestPowerAsync(TCDRequest requestObj)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.TransmitTestPower(requestObj.ChannelID, (uint)requestObj.Value);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// transmit test sample length as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> TransmitTestSampleLengthAsync(TCDRequest requestObj)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.TransmitSampleLength(requestObj.ChannelID, (uint)requestObj.Value);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is mock active.
        /// </summary>
        /// <value><c>true</c> if this instance is mock active; otherwise, <c>false</c>.</value>
        public bool isMockActive
        {
            get { return useMockTCD; }
        }

        /// <summary>
        /// transmit test PRF as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> TransmitTestPRFAsync(TCDRequest requestObj)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.TransmitPRF(requestObj.ChannelID, (uint)requestObj.Value);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// start update process as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> StartUpdateProcessAsync(TCDRequest requestObj)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Value = await dopplerModule.StartUpdateAsync(requestObj.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// end update process as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> EndUpdateProcessAsync(TCDRequest requestObj)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Value = await dopplerModule.EndUpdateAsync(requestObj.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// get update progress as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDReadInfoResponse&gt;.</returns>
        public async Task<TCDReadInfoResponse> GetUpdateProgressAsync(TCDRequest requestObj)
        {
            TCDReadInfoResponse response = new TCDReadInfoResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.UpdateProgress = await dopplerModule.GetUpdateProgressAsync(requestObj.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// is service active as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDResponse&gt;.</returns>
        public async Task<TCDResponse> IsServiceActiveAsync(TCDRequest requestObj)
        {
            TCDResponse response = new TCDResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.Result = await dopplerModule.IsServiceActive(requestObj.ChannelID);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// read service log as an asynchronous operation.
        /// </summary>
        /// <param name="requestObj">The request object.</param>
        /// <returns>Task&lt;TCDReadInfoResponse&gt;.</returns>
        public async Task<TCDReadInfoResponse> ReadServiceLogAsync(TCDRequest requestObj)
        {
            TCDReadInfoResponse response = new TCDReadInfoResponse();
            try
            {
                if (TCDHandler.Current.isTCDWorking)
                {
                    response.ServicePacketList = await dopplerModule.GetServiceLogs(requestObj.ChannelID, requestObj.Value);
                }
            }
            catch (Exception ex)
            {
                response.Error = ex;
            }
            return response;
        }

        /// <summary>
        /// Seals the probes.
        /// </summary>
        private void SealProbes()
        {
            try
            {
                if (dopplerModule.CheckProbeTimer != null)
                {
                    dopplerModule.CheckProbeTimer.Cancel();
                }
                dopplerModule.OnProbePlugged -= DopplerModuleOnProbePlugged;
                dopplerModule.OnProbeUnplugged -= DopplerModuleOnProbeUnplugged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Starts the probe scanning.
        /// </summary>
        public async void StartProbeScanning()
        {
            dopplerModule.OnProbePlugged += DopplerModuleOnProbePlugged;
            dopplerModule.OnProbeUnplugged += DopplerModuleOnProbeUnplugged;
            await dopplerModule.InitializeProbeEvents();
        }

        /// <summary>
        /// Closes the TCD.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task CloseTCD()
        {
            try
            {
                await Task.Delay(Constants.VALUE_100);
                SealProbes();
                TCDHandler.Current.Channel1.Dispose();
                TCDHandler.Current.Channel2.Dispose();
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "CloseTCD", Severity.Warning);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {           
            PacketQueueChannel1.Clear();
            PacketQueueChannel2.Clear();
            PacketQueue.Clear();
            OnProbePlugged = null;
            OnProbeUnplugged = null;
            OnPacketFormed = null;         
        }


        public async Task<bool> SetModeAsync(TCDHandles channelId, TCDModes modeToSet)
        {
            if (TCDHandler.Current.isTCDWorking)
            {
                if (await dopplerModule.SetMode(channelId, modeToSet) == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}