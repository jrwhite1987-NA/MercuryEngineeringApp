using Core.Constants;
using System;

namespace UsbTcdLibrary.PacketFormats
{
    /// <summary>
    /// Class DMIPktHeader.
    /// </summary>
    public class DMIPktHeader
    {
        /// <summary>
        /// The synchronize
        /// </summary>
        public long sync;

        /// <summary>
        /// The system identifier
        /// </summary>
        public byte systemID;

        /// <summary>
        /// The data source
        /// </summary>
        public byte dataSource;

        /// <summary>
        /// The message type
        /// </summary>
        public byte messageType;

        /// <summary>
        /// The message sub type
        /// </summary>
        public byte messageSubType;

        /// <summary>
        /// The data length
        /// </summary>
        public ushort dataLength;

        /// <summary>
        /// The sequence
        /// </summary>
        public ushort sequence;
    }

    /// <summary>
    /// Class DMIParameter.
    /// </summary>
    public class DMIParameter
    {
        /// <summary>
        /// The timestamp l
        /// </summary>
        public uint timestampL;

        /// <summary>
        /// The timestamp h
        /// </summary>
        public uint timestampH;

        /// <summary>
        /// The event flags
        /// </summary>
        public ushort eventFlags;

        /// <summary>
        /// The operating state
        /// </summary>
        public byte operatingState;

        /// <summary>
        /// The acoustic power
        /// </summary>
        public byte acousticPower;

        /// <summary>
        /// The sample length
        /// </summary>
        public byte sampleLength;

        /// <summary>
        /// The user depth
        /// </summary>
        public byte userDepth;

        /// <summary>
        /// The PRF
        /// </summary>
        public ushort PRF;

        /// <summary>
        /// The tic
        /// </summary>
        public ushort TIC;

        /// <summary>
        /// The rfu
        /// </summary>
        public ushort rfu;
    }

    /// <summary>
    /// Class DMIEnvelope.
    /// </summary>
    public class DMIEnvelope
    {
        /// <summary>
        /// The depth
        /// </summary>
        public ushort depth;

        /// <summary>
        /// The velocity units
        /// </summary>
        public ushort velocityUnits;

        /// <summary>
        /// The col index position
        /// </summary>
        public ushort colIndexPos;

        /// <summary>
        /// The position velocity
        /// </summary>
        public short posVelocity;

        /// <summary>
        /// The position peak
        /// </summary>
        public short posPEAK;

        /// <summary>
        /// The position mean
        /// </summary>
        public short posMEAN;

        /// <summary>
        /// The position dias
        /// </summary>
        public short posDIAS;

        /// <summary>
        /// The position pi
        /// </summary>
        public ushort posPI;

        /// <summary>
        /// The position ri
        /// </summary>
        public ushort posRI;

        /// <summary>
        /// The col index neg
        /// </summary>
        public ushort colIndexNeg;

        /// <summary>
        /// The neg velocity
        /// </summary>
        public short negVelocity;

        /// <summary>
        /// The neg peak
        /// </summary>
        public short negPEAK;

        /// <summary>
        /// The neg mean
        /// </summary>
        public short negMEAN;

        /// <summary>
        /// The neg dias
        /// </summary>
        public short negDIAS;

        /// <summary>
        /// The neg pi
        /// </summary>
        public ushort negPI;

        /// <summary>
        /// The neg ri
        /// </summary>
        public ushort negRI;
    }

    /// <summary>
    /// Class DMISpectrum.
    /// </summary>
    public class DMISpectrum : IDisposable
    {
        /// <summary>
        /// The depth
        /// </summary>
        public ushort depth;

        /// <summary>
        /// The clutter filter
        /// </summary>
        public ushort clutterFilter;

        /// <summary>
        /// The automatic gain offset
        /// </summary>
        public short autoGainOffset;

        /// <summary>
        /// The start velocity
        /// </summary>
        public short startVelocity;

        /// <summary>
        /// The end velocity
        /// </summary>
        public short endVelocity;

        /// <summary>
        /// The points per column
        /// </summary>
        public ushort pointsPerColumn;

        /// <summary>
        /// The points
        /// </summary>
        public short[] points = new short[DMIProtocol.SpectrumPointsCount];

        public void Dispose()
        {
            points = null;
        }
    }

    /// <summary>
    /// Class DMIMMode.
    /// </summary>
    public class DMIMMode : IDisposable
    {
        /// <summary>
        /// The automatic gain offset
        /// </summary>
        public short autoGainOffset;

        /// <summary>
        /// The start depth
        /// </summary>
        public ushort startDepth;

        /// <summary>
        /// The end depth
        /// </summary>
        public ushort endDepth;

        /// <summary>
        /// The points per column
        /// </summary>
        public ushort pointsPerColumn;

        /// <summary>
        /// The power
        /// </summary>
        public short[] power = new short[DMIProtocol.DMI_PKT_MMODE_PTS];

        /// <summary>
        /// The velocity
        /// </summary>
        public short[] velocity = new short[DMIProtocol.DMI_PKT_MMODE_PTS];

        public void Dispose()
        {
            power = null;
            velocity = null;
        }
    }

    /// <summary>
    /// Class DMIAudio.
    /// </summary>
    public class DMIAudio : IDisposable
    {
        /// <summary>
        /// The depth
        /// </summary>
        public ushort depth;

        /// <summary>
        /// The rfu
        /// </summary>
        public ushort rfu;

        /// <summary>
        /// The sample rate
        /// </summary>
        public ushort sampleRate;

        /// <summary>
        /// The maximum amplitude
        /// </summary>
        public short maxAmplitude;

        /// <summary>
        /// The toward
        /// </summary>
        public short[] toward = new short[DMIProtocol.DMI_AUDIO_ARRAY_SIZE];

        /// <summary>
        /// The away
        /// </summary>
        public short[] away = new short[DMIProtocol.DMI_AUDIO_ARRAY_SIZE];

        public void Dispose()
        {
            toward = null;
            away = null;
        }
    }

    /// <summary>
    /// Class DMIEdetectPhaseResults.
    /// </summary>
    public class DMIEdetectPhaseResults
    {
        /// <summary>
        /// The mq
        /// </summary>
        public uint MQ;

        /// <summary>
        /// The clut count
        /// </summary>
        public uint ClutCount;

        /// <summary>
        /// Me position
        /// </summary>
        public uint MEPosition;

        /// <summary>
        /// The m sum
        /// </summary>
        public uint MSum;

        /// <summary>
        /// The mp local
        /// </summary>
        public uint MPLocal;

        /// <summary>
        /// The ae flag
        /// </summary>
        public uint AEFlag;

        /// <summary>
        /// The ae down count
        /// </summary>
        public uint AEDownCount;

        /// <summary>
        /// The ae detect
        /// </summary>
        public uint AEDetect;
    }

    /// <summary>
    /// Class DMIEDetectResults.
    /// </summary>
    public class DMIEDetectResults : IDisposable
    {
        /// <summary>
        /// The phase a
        /// </summary>
        public DMIEdetectPhaseResults phaseA;

        /// <summary>
        /// The phase b
        /// </summary>
        public DMIEdetectPhaseResults phaseB;

        /// <summary>
        /// The edetect
        /// </summary>
        public int edetect;

        /// <summary>
        /// Initializes a new instance of the <see cref="DMIEDetectResults"/> class.
        /// </summary>
        public DMIEDetectResults()
        {
            phaseA = new DMIEdetectPhaseResults();
            phaseB = new DMIEdetectPhaseResults();
        }

        public void Dispose()
        {
            phaseA = null;
            phaseB = null;
        }
    }

    /// <summary>
    /// Class FloatIQ.
    /// </summary>
    public class FloatIQ
    {
        /// <summary>
        /// The i
        /// </summary>
        public float I;

        /// <summary>
        /// The q
        /// </summary>
        public float Q;
    }

    /// <summary>
    /// Class DMIArchiveMmodeData.
    /// </summary>
    public class DMIArchiveMmodeData : IDisposable
    {
        /// <summary>
        /// The mmode phase a
        /// </summary>
        public float[] mmodePhaseA = new float[DMIProtocol.DMI_ARCHIVE_MMODE_GATES];

        /// <summary>
        /// The mmode phase b
        /// </summary>
        public float[] mmodePhaseB = new float[DMIProtocol.DMI_ARCHIVE_MMODE_GATES];

        /// <summary>
        /// The mmode power a
        /// </summary>
        public float[] mmodePowerA = new float[DMIProtocol.DMI_ARCHIVE_MMODE_GATES];

        /// <summary>
        /// The mmode power b
        /// </summary>
        public float[] mmodePowerB = new float[DMIProtocol.DMI_ARCHIVE_MMODE_GATES];

        public void Dispose()
        {
            mmodePhaseA = null;
            mmodePhaseB = null;
            mmodePowerA = null;
            mmodePowerB = null;
        }
    }

    /// <summary>
    /// Class DMIArchive.
    /// </summary>
    public class DMIArchive : IDisposable
    {
        /// <summary>
        /// The timeseries depth
        /// </summary>
        public ushort timeseriesDepth;

        /// <summary>
        /// The rfu
        /// </summary>
        public ushort rfu;

        /// <summary>
        /// The timeseries
        /// </summary>
        public FloatIQ[] timeseries;

        /// <summary>
        /// The mmode data
        /// </summary>
        public DMIArchiveMmodeData mmodeData;

        /// <summary>
        /// Initializes a new instance of the <see cref="DMIArchive"/> class.
        /// </summary>
        public DMIArchive()
        {
            mmodeData = new DMIArchiveMmodeData();
            timeseries = new FloatIQ[DMIProtocol.DMI_ARCHIVE_TIMESERIES_SIZE];
            for (int i = Constants.VALUE_0; i < DMIProtocol.DMI_ARCHIVE_TIMESERIES_SIZE; i++)
            {
                timeseries[i] = new FloatIQ();
            }
        }

        public void Dispose()
        {
            timeseries = null;
            mmodeData.Dispose();
        }
    }

    /// <summary>
    /// Class DMIPmdDataPacket.
    /// </summary>
    public class DMIPmdDataPacket
    {
        /// <summary>
        /// The header
        /// </summary>
        public DMIPktHeader header;

        /// <summary>
        /// The reserved
        /// </summary>
        public ushort reserved;

        /// <summary>
        /// The data format rev
        /// </summary>
        public ushort dataFormatRev;

        /// <summary>
        /// The parameter
        /// </summary>
        public DMIParameter parameter;

        /// <summary>
        /// The emb count
        /// </summary>
        public uint embCount;

        /// <summary>
        /// The envelope
        /// </summary>
        public DMIEnvelope envelope;

        /// <summary>
        /// The spectrum
        /// </summary>
        public DMISpectrum spectrum;

        /// <summary>
        /// The mmode
        /// </summary>
        public DMIMMode mmode;

        /// <summary>
        /// The audio
        /// </summary>
        public DMIAudio audio;

        /// <summary>
        /// The edetect results
        /// </summary>
        public DMIEDetectResults edetectResults;

        /// <summary>
        /// The archive
        /// </summary>
        public DMIArchive archive;

        /// <summary>
        /// The checksum
        /// </summary>
        public int checksum;

        /// <summary>
        /// Initializes a new instance of the <see cref="DMIPmdDataPacket"/> class.
        /// </summary>
        public DMIPmdDataPacket()
        {
            header = new DMIPktHeader();
            parameter = new DMIParameter();
            envelope = new DMIEnvelope();
            spectrum = new DMISpectrum();
            mmode = new DMIMMode();
            audio = new DMIAudio();
            edetectResults = new DMIEDetectResults();
        }
    }
}