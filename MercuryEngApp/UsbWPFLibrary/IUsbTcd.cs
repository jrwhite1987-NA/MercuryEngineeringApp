// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="IUsbTcd.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsbTcdLibrary.CommunicationProtocol;
using UsbTcdLibrary.PacketFormats;
using UsbTcdLibrary.StatusClasses;
using Core.Models.ReportModels;

namespace UsbTcdLibrary
{
    public interface IUsbTcd : IDisposable
    {
        bool isMockActive
        {
            get;
        }

        /// <summary>
        /// Gets the current active channel(s) from the TCD
        /// </summary>
        /// <value>The active channel.</value>
        ActiveChannels ActiveChannel
        {
            get;
        }

        bool InitializeTCD();

        #region PowerRelated

        /// <summary>
        /// Turns the power on for the TCD machine
        /// </summary>
        /// <returns>True if the TCD is attached to the system and communication is established
        /// False if it encounters any error</returns>
        Task<TCDResponse> TurnTCDPowerOnAsync();

        /// <summary>
        /// Turns the power off for the TCD machine
        /// </summary>
        /// <returns>True if successfully released the TCD False if attempt unsuccessful</returns>
        TCDResponse TurnTCDPowerOff();

        TCDResponse TurnSingleChannelOff(TCDRequest requestObject);

        TCDResponse TurnSingleChannelOn(TCDRequest requestObject);

        /// <summary>
        /// Gets true if communication with TCD fails
        /// </summary>
        /// <value><c>true</c> if this instance is handle null; otherwise, <c>false</c>.</value>
        bool IsTCDPowerOn { get; }

        #endregion PowerRelated

        #region RecordingRelated

        /// <summary>
        /// Starts saving the TCD data into the file for future reference
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        Task<TCDResponse> TurnRecordingOnAsync(TCDRequest requestObject);

        /// <summary>
        /// Stops saving the incoming TCD data into file
        /// </summary>
        /// <returns>Task.</returns>
        Task<TCDResponse> TurnRecordingOffAsync();

        bool IsRecordingOn { get; }

        #endregion RecordingRelated

        #region TCDInfoRelated

        /// <summary>
        /// Gets the module information of TCD.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>ModuleInfo.</returns>
        Task<TCDReadInfoResponse> GetModuleInfo(TCDRequest requestObject);

        /// <summary>
        /// Gets the device descriptors, configuration descriptors and endpoint descriptors of the TCD connected formatted in a string
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>System.String.</returns>
        TCDResponse GetTCDDescriptors(TCDRequest requestObject);

        Task<TCDReadInfoResponse> GetProbeInfo(TCDRequest requestObject);

        #endregion TCDInfoRelated

        #region DopplerCommands

        /// <summary>
        /// Sets the PRF of the channel provided
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="PRF">The PRF.</param>
        /// <param name="startDepth">The start depth.</param>
        /// <returns>System.UInt32.</returns>
        Task<TCDResponse> SetPRF(TCDRequest requestObj);

        Task<TCDResponse> SetEnvelopeRangeAsync(TCDRequest requestObj);

        Task<TCDResponse> SetDepthAsync(TCDRequest requestObj);

        Task<TCDResponse> SetPowerAsync(TCDRequest requestObj);

        Task<TCDResponse> SetFilterAsync(TCDRequest requestObj);

        Task<TCDResponse> SetLengthAsync(TCDRequest requestObj);

        /// <summary>
        /// Sets the mode asynchronous.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="modeToSet">The mode to set.</param>
        /// <returns></returns>
        Task<bool> SetModeAsync(TCDHandles channelId, TCDModes modeToSet);

        Task<byte> GetChannelNumber(TCDRequest requestObj);

        /// <summary>
        /// Clears the buffer of the specified TCD channel and endpoint
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.UInt32.</returns>
        Task<TCDResponse> ClearBufferAsync(TCDRequest requestObj);

        #endregion DopplerCommands

        #region PacketsRelated

        /// <summary>
        /// Gets the packet queue channel1.
        /// </summary>
        /// <value>The packet queue channel1.</value>
        List<DMIPmdDataPacket> PacketQueueChannel1
        {
            get;
        }

        /// <summary>
        /// Gets the packet queue channel2.
        /// </summary>
        /// <value>The packet queue channel2.</value>
        List<DMIPmdDataPacket> PacketQueueChannel2
        {
            get;
        }

        Dictionary<int, List<DMIPmdDataPacket>> PacketQueue
        {
            get;
        }

        /// <summary>
        /// Reads from file.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> ReadFromFileAsync(int examId, int channelId);

        Task<bool> ReadFromFileAsync(int examId, Dictionary<int, int> ReadPointerListCh1, Dictionary<int, int> ReadPointerListCh2);

        Task<bool> ReadFromFileWithRange(int examId, List<ReadPointerModel> ListReadPointerModelCh1);

        /// <summary>
        /// Read Binary file for exam with Range for 4/8/12 Sec
        /// </summary>
        /// <param name="examId">exam Id</param>
        /// <param name="ListReadPointerModelCh1">Exam screen shots parameter for ch1</param>
        /// <param name="ListReadPointerModelCh2">Exam screen shots parameter for ch2</param>
        /// <returns></returns>
        Task<bool> ReadFromFileWithRangeAsync(int examId, List<ReadPointerModel> ListReadPointerModelCh1, List<ReadPointerModel> ListReadPointerModelCh2);

        /// <summary>
        /// Reads a full doppler packet into the buffer
        /// Inputs:
        /// Returns:
        /// </summary>
        void StartTCDReading();

        #endregion PacketsRelated

        #region Events

        event TCDPacketFormed OnPacketFormed;

        event ProbePlugUnplug OnProbePlugged;

        event ProbePlugUnplug OnProbeUnplugged;

        void StartProbeScanning();

        Task CloseTCD();

        #endregion Events

        Task<bool> CreateBinaryFileOfExam(int examId);

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>The time.</value>
        long Time
        {
            get;
        }

        #region ProbesRelated

        /// <summary>
        /// Gets the active channel asynchronous.
        /// </summary>
        /// <returns>Task&lt;EnumList.ActiveChannels&gt;.</returns>
        Task<TCDResponse> GetProbesConnectedAsync();

        Task<TCDResponse> IsProbeConnectedAsync(TCDRequest requestObj);

        #endregion ProbesRelated

        #region Service Mode Commands and Requests

        Task<TCDResponse> WriteValueToFPGAAsync(TCDRequest requestObj);

        Task<TCDResponse> ResetFPGAAsync(TCDRequest requestObj);

        Task<TCDResponse> AbortServiceAsync(TCDRequest requestObj);

        Task<TCDResponse> AssignChannelAsync(TCDRequest requestObj);

        Task<TCDResponse> WriteBoardInfoAsync(TCDWriteInfoRequest requestObj);

        Task<TCDResponse> WriteProbeInfoAsync(TCDWriteInfoRequest requestObj);

        Task<TCDResponse> StartMeasurementOfBoardAsync(TCDRequest requestObj);

        Task<TCDResponse> ApplyMeasurementToBoardAsync(TCDRequest requestObj);

        Task<TCDResponse> CalibrateBoardAsync(TCDWriteInfoRequest requestObj);

        Task<TCDResponse> IsServiceActiveAsync(TCDRequest requestObj);

        Task<TCDResponse> ReadFPGAValueAsync(TCDRequest requestObj);

        Task<TCDReadInfoResponse> ReadCalibrationInfoAsync(TCDRequest requestObj);

        Task<TCDResponse> ReadOperatingMinutesAsync(TCDRequest requestObj);

        Task<TCDResponse> EnableTransmitTestControlAsync(TCDRequest requestObj);

        Task<TCDResponse> DisableTransmitTestControlAsync(TCDRequest requestObj);

        Task<TCDResponse> TransmitTestPowerAsync(TCDRequest requestObj);

        Task<TCDResponse> TransmitTestSampleLengthAsync(TCDRequest requestObj);

        Task<TCDResponse> TransmitTestPRFAsync(TCDRequest requestObj);

        Task<TCDReadInfoResponse> ReadServiceLogAsync(TCDRequest requestObj);

        #endregion Service Mode Commands and Requests

        #region Update mode requests

        Task<TCDResponse> StartUpdateProcessAsync(TCDRequest requestObj);

        Task<TCDResponse> EndUpdateProcessAsync(TCDRequest requestObj);

        Task<TCDReadInfoResponse> GetUpdateProgressAsync(TCDRequest requestObj);

        #endregion Update mode requests

        Task<TCDResponse> ReadFromFileCVRAsync(int examId, int channelId);

        List<short> CVRDataChannel1
        {
            get;
        }

        List<short> CVRDataChannel2
        {
            get;
        }

    
    }
}