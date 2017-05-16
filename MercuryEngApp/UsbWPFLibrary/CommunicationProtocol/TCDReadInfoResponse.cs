// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="TCDReadInfoResponse.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using UsbTcdLibrary.StatusClasses;

namespace UsbTcdLibrary.CommunicationProtocol
{
    /// <summary>
    /// Class TCDReadInfoResponse.
    /// </summary>
    public class TCDReadInfoResponse
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public Exception Error { get; set; }
        /// <summary>
        /// Gets or sets the module.
        /// </summary>
        /// <value>The module.</value>
        public ModuleInfo Module { get; set; }
        /// <summary>
        /// Gets or sets the probe.
        /// </summary>
        /// <value>The probe.</value>
        public ProbeInfo Probe { get; set; }
        /// <summary>
        /// Gets or sets the calibration.
        /// </summary>
        /// <value>The calibration.</value>
        public CalibrationInfo Calibration { get; set; }
        /// <summary>
        /// Gets or sets the update progress.
        /// </summary>
        /// <value>The update progress.</value>
        public UpdateProgress UpdateProgress { get; set; }
        /// <summary>
        /// Gets or sets the service packet list.
        /// </summary>
        /// <value>The service packet list.</value>
        public List<PacketFormats.ServiceLogPacket> ServicePacketList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TCDReadInfoResponse"/> class.
        /// </summary>
        public TCDReadInfoResponse()
        {
            Error = null;
            Module = null;
            Probe = null;
            Calibration = null;
            UpdateProgress = null;
            ServicePacketList = null;
        }
    }
}