// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="ServiceLogPacket.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace UsbTcdLibrary.PacketFormats
{
    /// <summary>
    /// Class ServiceLogPacketHeader.
    /// </summary>
    public class ServiceLogPacketHeader
    {
        /// <summary>
        /// Gets or sets the synchronize identifier.
        /// </summary>
        /// <value>The synchronize identifier.</value>
        public long SyncId { get; set; }
        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <value>The system identifier.</value>
        public byte SystemId { get; set; }
        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public byte DataSource { get; set; }
        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public byte MessageType { get; set; }
        /// <summary>
        /// Gets or sets the type of the message sub.
        /// </summary>
        /// <value>The type of the message sub.</value>
        public byte MessageSubType { get; set; }
        /// <summary>
        /// Gets or sets the length of the data.
        /// </summary>
        /// <value>The length of the data.</value>
        public ushort DataLength { get; set; }
        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public ushort Sequence { get; set; }
    }

    /// <summary>
    /// Class ServiceLogMessage.
    /// </summary>
    public class ServiceLogMessage
    {
        /// <summary>
        /// Gets or sets the message code.
        /// </summary>
        /// <value>The message code.</value>
        public uint MessageCode { get; set; }
        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        /// <value>The message text.</value>
        public byte[] MessageText { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogMessage"/> class.
        /// </summary>
        public ServiceLogMessage()
        {
            MessageText = new byte[DMIProtocol.DMI_SERVICELOG_MESSAGETEXT_SIZE];
        }
    }

    /// <summary>
    /// Class ServiceLogPacket.
    /// </summary>
    public class ServiceLogPacket
    {
        /// <summary>
        /// The packet header
        /// </summary>
        public ServiceLogPacketHeader PacketHeader;
        /// <summary>
        /// The message
        /// </summary>
        public ServiceLogMessage Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogPacket"/> class.
        /// </summary>
        public ServiceLogPacket()
        {
            PacketHeader = new ServiceLogPacketHeader();
            Message = new ServiceLogMessage();
        }
    }
}