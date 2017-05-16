// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="TCDResponse.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace UsbTcdLibrary.CommunicationProtocol
{
    /// <summary>
    /// Class TCDResponse.
    /// </summary>
    public class TCDResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="TCDResponse"/> is result.
        /// </summary>
        /// <value><c>true</c> if result; otherwise, <c>false</c>.</value>
        public bool Result { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public ulong Value { get; set; }
        /// <summary>
        /// Gets or sets the active channel.
        /// </summary>
        /// <value>The active channel.</value>
        public ActiveChannels ActiveChannel { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public Exception Error { get; set; }
        /// <summary>
        /// Gets or sets the TCD descriptor.
        /// </summary>
        /// <value>The TCD descriptor.</value>
        public string TCDDescriptor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TCDResponse"/> class.
        /// </summary>
        public TCDResponse()
        {
            Result = false;
            Value = 0;
            ActiveChannel = ActiveChannels.None;
            Error = null;
            TCDDescriptor = null;
        }
    }
}