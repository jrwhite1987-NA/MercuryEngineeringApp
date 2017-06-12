using System;
// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="TCDWriteInfoRequest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using UsbTcdLibrary.StatusClasses;

namespace UsbTcdLibrary.CommunicationProtocol
{
    /// <summary>
    /// Class TCDWriteInfoRequest.
    /// </summary>
    public class TCDWriteInfoRequest: IDisposable
    {
        /// <summary>
        /// Gets or sets the channel identifier.
        /// </summary>
        /// <value>The channel identifier.</value>
        public TCDHandles ChannelID { get; set; }
        /// <summary>
        /// Gets or sets the probe.
        /// </summary>
        /// <value>The probe.</value>
        public ProbeInfo Probe { get; set; }
        /// <summary>
        /// Gets or sets the board.
        /// </summary>
        /// <value>The board.</value>
        public BoardInfo Board { get; set; }
        /// <summary>
        /// Gets or sets the calibration.
        /// </summary>
        /// <value>The calibration.</value>
        public CalibrationInfo Calibration { get; set; }

        public byte[] UpdateData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TCDWriteInfoRequest"/> class.
        /// </summary>
        public TCDWriteInfoRequest()
        {
            UpdateData = new byte[DMIProtocol.UPDATE_LOAD_BLOCK_SIZE];
            ChannelID = TCDHandles.None;
            Probe = null;
            Board = null;
            Calibration = null;
        }

        public void Dispose()
        {
            ChannelID = TCDHandles.None; 
            Probe = null;
            Board = null;
            Calibration = null;
        }
    }
}