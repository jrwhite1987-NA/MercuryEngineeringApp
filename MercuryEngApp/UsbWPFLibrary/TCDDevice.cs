using Core.Common;
using System;
using UsbTcdLibrary.StatusClasses;
using Windows.Devices.Usb;
using Windows.Storage.Streams;

namespace UsbTcdLibrary
{
    internal class TCDDevice : IDisposable
    {
        /// <summary>
        /// Gets or sets the TCD handle channel.
        /// </summary>
        /// <value>The TCD handle channel.</value>
        internal UsbDevice TCDHandleChannel { get; set; }

        internal ModuleInfo ModuleInformation { get; set; }

        internal ProbeInfo ProbeInformation { get; set; }

        internal TCDModes CurrentMode { get; set; }

        internal bool IsSensingEnabled { get; set; }

        internal bool IsReadingEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is channel enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is channel enabled; otherwise, <c>false</c>.</value>
        internal bool IsChannelEnabled { get; set; }

        /// <summary>
        /// Gets or sets the temporary buffer ch.
        /// </summary>
        /// <value>The temporary buffer ch.</value>
        internal byte[] TempBufferCh { get; set; }

        /// <summary>
        /// Gets or sets the service buffer ch.
        /// </summary>
        /// <value>The service buffer ch.</value>
        internal byte[] ServiceBufferCh { get; set; }

        /// <summary>
        /// Gets or sets the reader channel.
        /// </summary>
        /// <value>The reader channel.</value>
        internal DataReader ChannelReader { get; set; }

        /// <summary>
        /// Gets or sets the service log channel.
        /// </summary>
        /// <value>The service log channel.</value>
        internal DataReader ServiceLogReader { get; set; }

        /// <summary>
        /// Gets or sets the stream channel.
        /// </summary>
        /// <value>The stream channel.</value>
        internal IInputStream ChannelStream { get; set; }

        /// <summary>
        /// Gets or sets the read buffer channel.
        /// </summary>
        /// <value>The read buffer channel.</value>
        internal IBuffer ReadBuffer { get; set; }

        /// <summary>
        /// Gets or sets the service buffer.
        /// </summary>
        /// <value>The service buffer.</value>
        internal IBuffer ServiceBuffer { get; set; }

        /// <summary>
        /// Gets or sets the channel probe.
        /// </summary>
        /// <value>The channel probe.</value>
        internal IBuffer ProbeBuffer { get; set; }

        /// <summary>
        /// Gets or sets the emb count.
        /// </summary>
        /// <value>The emb count.</value>
        internal uint EmbCount { get; set; }

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        internal ushort Sequence { get; set; }

        internal bool canReadLog { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (TCDHandleChannel != null)
                {
                    TCDHandleChannel.Dispose();
                    TCDHandleChannel = null;
                }
                ProbeBuffer = null;
                if (ServiceLogReader != null)
                {
                    ServiceLogReader.Dispose();
                    ServiceLogReader = null;
                }
                if (ChannelReader != null)
                {
                    ChannelReader.Dispose();
                    ChannelReader = null;
                }
                TempBufferCh = null;
                ChannelStream = null;
                ReadBuffer = null;
                ServiceBuffer = null;
                ProbeBuffer = null;
                IsChannelEnabled = false;
                IsSensingEnabled = false;
                IsReadingEnabled = false;
                canReadLog = false;
                CurrentMode = TCDModes.Active;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "Dispose", Severity.Warning);
            }
        }

        public void ResetForSensing()
        {
            try
            {
                ProbeBuffer = null;
                if (ChannelReader != null)
                {
                    ChannelReader.DetachBuffer();
                    ChannelReader.DetachStream();
                    ChannelReader.Dispose();
                    ChannelReader = null;
                }
                TempBufferCh = null;
                ChannelStream = null;
                ReadBuffer = null;
                ProbeBuffer = null;
                IsChannelEnabled = false;
                IsReadingEnabled = false;
                CurrentMode = TCDModes.Active;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<TCDHandler>(ex, "Dispose", Severity.Warning);
            }
        }
    }
}