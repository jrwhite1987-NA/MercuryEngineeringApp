using Core.Common;

// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="DMIProtcol.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Constants;
using System;

namespace UsbTcdLibrary
{
    /// <summary>
    /// Class DMIProtocol.
    /// </summary>
    public class DMIProtocol
    {
        #region Packet Size

        /// <summary>
        /// The packet size
        /// </summary>
        internal static int PACKET_SIZE = Packet_Size_128FFT;

        private const int Packet_Size_128FFT = 2756;
        private const int Packet_Size_256FFT = 3524;

        internal const int DMI_SERVICE_LOG_PACKET_SIZE = 40;
        internal const int DMI_SERVICELOG_MESSAGETEXT_SIZE = 20;

        #endregion Packet Size

        /// <summary>
        /// The doppler command
        /// </summary>
        internal const byte DOPPLER_COMMAND = 0x40;

        /// <summary>
        /// The doppler request
        /// </summary>
        internal const byte DOPPLER_REQUEST = 0xC0;

        #region Hardware IDs

        /// <summary>
        /// The TCD vendor identifier
        /// </summary>
        internal const uint TCD_VENDOR_ID = 0x0547;

        /// <summary>
        /// The TCD product identifier
        /// </summary>
        internal const uint TCD_PRODUCT_ID = 0x7CD1;

        #endregion Hardware IDs

        #region Parameter Defaults

        /// <summary>
        /// The dmi startup power
        /// </summary>
        internal const int DMI_STARTUP_POWER = 0;

        /// <summary>
        /// The dmi power minimum
        /// </summary>
        internal const int DMI_POWER_MIN = 0;

        /// <summary>
        /// The dmi power maximum
        /// </summary>
        internal const int DMI_POWER_MAX = 100;

        /// <summary>
        /// The dmi startup depth
        /// </summary>
        internal const int DMI_STARTUP_DEPTH = 50;

        /// <summary>
        /// The dmi depth minimum
        /// </summary>
        internal const int DMI_DEPTH_MIN = 23;

        /// <summary>
        /// The dmi depth maximum
        /// </summary>
        internal const int DMI_DEPTH_MAX = 86;

        /// <summary>
        /// The dmi startup filter
        /// </summary>
        internal const int DMI_STARTUP_FILTER = 200;

        /// <summary>
        /// The dmi filter minimum
        /// </summary>
        internal const int DMI_FILTER_MIN = 0;

        /// <summary>
        /// The dmi filter maximum
        /// </summary>
        internal const int DMI_FILTER_MAX = 600;

        /// <summary>
        /// The dmi startup sample
        /// </summary>
        internal const int DMI_STARTUP_SAMPLE = 6;

        /// <summary>
        /// The dmi sample minimum
        /// </summary>
        internal const int DMI_SAMPLE_MIN = 3;

        /// <summary>
        /// The dmi sample maximum
        /// </summary>
        internal const int DMI_SAMPLE_MAX = 9;

        /// <summary>
        /// The dmi startup PRF
        /// </summary>
        internal const int DMI_STARTUP_PRF = 8000;

        /// <summary>
        /// The dmi PRF minimum
        /// </summary>
        internal const int DMI_PRF_MIN = 5000;

        /// <summary>
        /// The dmi PRF maximum
        /// </summary>
        internal const int DMI_PRF_MAX = 12500;

        /// <summary>
        /// The dmi startup mmode start depth
        /// </summary>
        internal const int DMI_STARTUP_MMODE_START_DEPTH = 22;

        /// <summary>
        /// The dmi mmode start depth minimum
        /// </summary>
        internal const int DMI_MMODE_START_DEPTH_MIN = 22;

        /// <summary>
        /// The dmi mmode start depth maximum
        /// </summary>
        internal const int DMI_MMODE_START_DEPTH_MAX = 82;

        /// <summary>
        /// The dmi startup mmode gates
        /// </summary>
        internal const int DMI_STARTUP_MMODE_GATES = 33;

        /// <summary>
        /// The dmi mmode gates minimum
        /// </summary>
        internal const int DMI_MMODE_GATES_MIN = 33;

        /// <summary>
        /// The dmi mmode gates maximum
        /// </summary>
        internal const int DMI_MMODE_GATES_MAX = 34;

        /// <summary>
        /// The dmi startup mmode depth per gate
        /// </summary>
        internal const int DMI_MMODE_DEPTH_PER_GATE = 2;

        #endregion Parameter Defaults

        /// <summary>
        /// The dmi mode standby
        /// </summary>
        internal const int DMI_MODE_STANDBY = 0;

        /// <summary>
        /// The dmi mode operate
        /// </summary>
        internal const int DMI_MODE_OPERATE = 1;

        /// <summary>
        /// The dmi mode active
        /// </summary>
        internal const int DMI_MODE_ACTIVE = 2;

        /// <summary>
        /// The dmi mode service
        /// </summary>
        internal const int DMI_MODE_SERVICE = 3;

        /// <summary>
        /// The dmi mode update
        /// </summary>
        internal const int DMI_MODE_UPDATE = 4;

        /// <summary>
        /// The dmi mode playback
        /// </summary>
        internal const int DMI_MODE_PLAYBACK = 5;

        /// <summary>
        /// The dmi mode unknown
        /// </summary>
        internal const int DMI_MODE_UNKNOWN = 0xFF;

        /// <summary>
        /// The dmi channel minimum
        /// </summary>
        internal const int DMI_Channel_MIN = 1;

        /// <summary>
        /// The dmi channel maximum
        /// </summary>
        internal const int DMI_Channel_MAX = 20;

        /// <summary>
        /// The dmi packet synchronize
        /// </summary>
        internal const ulong DMI_PACKET_SYNC = 0xFEEDA7CDDA7AF00D;

        #region EventCodes

        internal const uint DMI_EVENTCODE_TCD_BASE = 0x01000000;
        internal const uint DMI_EVENTCODE_PROBE_BASE = DMI_EVENTCODE_TCD_BASE + 0x00020000;

        internal const uint DMI_EVENTCODE_PROBE_CONNECT = DMI_EVENTCODE_PROBE_BASE + 2;
        internal const uint DMI_EVENTCODE_PROBE_DISCONNECT = DMI_EVENTCODE_PROBE_BASE + 3;

        internal static string DMI_EVENTSTRING_PROBE_DISCONNECT = "Probe Disconnected";

        #endregion EventCodes

        private const int FFT128_SPECTRUM_POINTS = 128;
        public const int FFT256_POINTS = 256;
        private const int FFT256_SPECTRUM_POINTS = 512;

        /// <summary>
        /// The dmi PKT spect PTS
        /// </summary>
        public static int FFTSize = FFT128_SPECTRUM_POINTS;

        public static int SpectrumPointsCount = FFT128_SPECTRUM_POINTS;

        /// <summary>
        /// The dmi PKT mmode PTS
        /// </summary>
        internal const int DMI_PKT_MMODE_PTS = 64;

        /// <summary>
        /// The dmi audio array size
        /// </summary>
        internal const int DMI_AUDIO_ARRAY_SIZE = 128;

        /// <summary>
        /// The dmi archive mmode gates
        /// </summary>
        internal const int DMI_ARCHIVE_MMODE_GATES = 33;

        /// <summary>
        /// The dmi archive timeseries size
        /// </summary>
        internal const int DMI_ARCHIVE_TIMESERIES_SIZE = 128;

        /// <summary>
        /// The dmi tic factor
        /// </summary>
        internal const int DMI_TIC_FACTOR = 10;

        /// <summary>
        /// The dmi PKT type command
        /// </summary>
        internal const int DMI_PKT_TYPE_CMD = 4;

        #region DMI Commands Code

        /// <summary>
        /// The dmi command set mode
        /// </summary>
        internal const int DMI_CMD_SET_MODE = 1;

        /// <summary>
        /// The dmi command reset
        /// </summary>
        internal const int DMI_CMD_RESET = 6;

        /// <summary>
        /// The dmi command clear buffer
        /// </summary>
        internal const int DMI_CMD_CLEAR_BUFFER = 11;

        /// <summary>
        /// The dmi command set timestamp
        /// </summary>
        internal const int DMI_CMD_SET_TIMESTAMP = 15;

        /// <summary>
        /// FPGA register
        /// </summary>
        internal const int DMI_CMD_SERVICE_ACCESS_FPGA_REG = 210;

        /// <summary>
        /// Abort service
        /// </summary>
        internal const int DMI_CMD_SERVICE_ABORT = 201;

        /// <summary>
        /// The dmi command doppler
        /// </summary>
        internal const int DMI_CMD_DOPPLER = 41;

        /// <summary>
        /// The dmi command phase
        /// </summary>
        internal const int DMI_CMD_PHASE = 100;

        /// <summary>
        /// The dmi command service abort
        /// </summary>
        internal const int DMI_CMD_SERVICE_READ_LOG = 220;

        /// <summary>
        /// Command for service access information.
        /// </summary>
        internal const int DMI_CMD_SERVICE_ACCESS_INFO = 215;

        /// <summary>
        /// Commands used to perform calibration of the Doppler board.
        /// </summary>
        internal const int DMI_CMD_SERVICE_CALIBRATE = 225;

        /// <summary>
        /// Command to enable transmit testing.
        /// </summary>
        internal const int DMI_CMD_SERVICE_TX_CTRL = 230;

        #endregion DMI Commands Code

        #region DMI Requests Code

        /// <summary>
        /// The dmi req update action start
        /// </summary>
        internal const int DMI_REQ_UPDATE_ACTION_START = 1;

        /// <summary>
        /// The dmi req update action end
        /// </summary>
        internal const int DMI_REQ_UPDATE_ACTION_END = 2;

        /// <summary>
        /// The dmi req response ack
        /// </summary>
        internal const int DMI_REQ_RESPONSE_ACK = 0;

        /// <summary>
        /// The dmi req response na
        /// </summary>
        internal const int DMI_REQ_RESPONSE_NA = 1;

        /// <summary>
        /// The dmi req response busy
        /// </summary>
        internal const int DMI_REQ_RESPONSE_BUSY = 2;

        /// <summary>
        /// The dmi req service get state
        /// </summary>
        internal const int DMI_REQ_SERVICE_GET_STATE = 200;

        /// <summary>
        /// The dmi req get mode
        /// </summary>
        internal const int DMI_REQ_GET_MODE = 1;

        /// <summary>
        /// The dmi req channel
        /// </summary>
        internal const int DMI_REQ_Channel = 50;

        /// <summary>
        /// The dmi req module information
        /// </summary>
        internal const int DMI_REQ_MODULE_INFO = 70;

        /// <summary>
        /// The dmi req probe information
        /// </summary>
        internal const int DMI_REQ_PROBE_INFO = 71;

        /// <summary>
        /// The dmi req update action
        /// </summary>
        internal const int DMI_REQ_UPDATE_ACTION = 120;

        /// <summary>
        /// The dmi req update progress
        /// </summary>
        internal const int DMI_REQ_UPDATE_PROGRESS = 121;

        #endregion DMI Requests Code

        /// <summary>
        /// The dmi in maxpacket size
        /// </summary>
        internal const int DMI_IN_MAXPACKET_SIZE = 512;

        /// <summary>
        /// The checksum size
        /// </summary>
        internal const int CHECKSUM_SIZE = 4;

        /// <summary>
        /// The dmi channel one
        /// </summary>
        internal const int DMI_CHANNEL_ONE = 1;

        /// <summary>
        /// The dmi channel two
        /// </summary>
        internal const int DMI_CHANNEL_TWO = 2;

        /// <summary>
        /// The synchronize size
        /// </summary>
        internal const int SYNC_SIZE = 8;

        /// <summary>
        /// The floatiq size
        /// </summary>
        internal const int FLOATIQ_SIZE = 8;

        /// <summary>
        /// The syncid firstbyte
        /// </summary>
        internal const byte SYNCID_FIRSTBYTE = 13;

        /// <summary>
        /// The float size
        /// </summary>
        internal const int FLOAT_SIZE = 4;

        private const int FILE_SIZE_128FFT = 1132;
        private const int FILE_SIZE_256FFT = 1900;

        /// <summary>
        /// The short CVR PKT size
        /// </summary>
        public static int FilePacketSize = FILE_SIZE_128FFT;

        /// <summary>
        /// The module information request length
        /// </summary>
        internal const int MODULE_INFO_REQUEST_LENGTH = 140;

        internal const int PROBE_INFO_REQUEST_LENGTH = 84;
        internal const int BOARD_INFO_REQUEST_LENGTH = 84;

        private static bool is256 = false;

        static internal bool Is256FFTEnable
        {
            get
            {
                return is256;
            }
        }

        static private string currentSWRevisionString = null;

        static internal string CurrentFirmwareVersion
        {
            get
            {
                return CurrentFirmwareVersion;
            }
            set
            {
                currentSWRevisionString = value;
                Determine256FFT(currentSWRevisionString);
            }
        }

        private static void Determine256FFT(string SWString)
        {
            try
            {
                string[] versions = SWString.Split('.');
                int majorVersion = Convert.ToInt32(versions[0]);
                int minorVersion = Convert.ToInt32(versions[1]);
                int buildVersion = Convert.ToInt32(versions[2]);
                int revisionVersion = Convert.ToInt32(versions[3]);

                if (majorVersion == Constants.VALUE_1 && minorVersion == Constants.VALUE_0 && buildVersion == Constants.VALUE_3)
                {
                    if (revisionVersion == Constants.VALUE_1)
                    {
                        is256 = true;
                        PACKET_SIZE = Packet_Size_256FFT;
                        FFTSize = FFT256_POINTS;
                        SpectrumPointsCount = FFT256_SPECTRUM_POINTS;
                        FilePacketSize = FILE_SIZE_256FFT;
                    }
                    else if (revisionVersion >= Constants.VALUE_3)
                    {
                        is256 = true;
                        PACKET_SIZE = Packet_Size_256FFT;
                        FFTSize = FFT256_POINTS;
                        SpectrumPointsCount = FFT256_SPECTRUM_POINTS;
                        FilePacketSize = FILE_SIZE_256FFT;
                    }
                    else
                    {
                        is256 = false;
                        PACKET_SIZE = Packet_Size_128FFT;
                        FFTSize = FFT128_SPECTRUM_POINTS;
                        SpectrumPointsCount = FFT128_SPECTRUM_POINTS;
                        FilePacketSize = FILE_SIZE_128FFT;
                    }
                }
                else
                {
                    is256 = false;
                    PACKET_SIZE = Packet_Size_128FFT;
                    FFTSize = FFT128_SPECTRUM_POINTS;
                    SpectrumPointsCount = FFT128_SPECTRUM_POINTS;
                    FilePacketSize = FILE_SIZE_128FFT;
                }
                //Constants.FFTSize = FFTSize;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DMIProtocol>(ex, "Determine256FFT", Severity.Warning);
            }
        }
    }
}