// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="TCDRequest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace UsbTcdLibrary.CommunicationProtocol
{
    /// <summary>
    /// Class TCDRequest.
    /// </summary>
    public class TCDRequest : IDisposable
    {
        /// <summary>
        /// Gets or sets the channel identifier.
        /// </summary>
        /// <value>The channel identifier.</value>
        public TCDHandles ChannelID { get; set; }
        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>The endpoint.</value>
        public EndpointNumber Endpoint { get; set; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public String Data { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { get; set; }
        /// <summary>
        /// Gets or sets the value2.
        /// </summary>
        /// <value>The value2.</value>
        public byte Value2 { get; set; }
        /// <summary>
        /// Gets or sets the value3.
        /// </summary>
        /// <value>The value3.</value>
        public uint Value3 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TCDRequest"/> class.
        /// </summary>
        public TCDRequest()
        {
            ChannelID = TCDHandles.None;
            Endpoint = EndpointNumber.None;
            Data = null;
            Value = 0;
            Value2 = 0;
            Value3 = 0;
        }

        /// <summary>
        /// Clears the values.
        /// </summary>
        public void ClearValues()
        {
            ChannelID = TCDHandles.None;
            Endpoint = EndpointNumber.None;
            Data = null;
            Value = 0;
            Value2 = 0;
            Value3 = 0;
        }

        public void Dispose()
        {
            ClearValues();
        }
    }
}