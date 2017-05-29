namespace UsbTcdLibrary
{
    #region Property positions

    /// <summary>
    ///
    /// </summary>
    public static class ServiceHeader
    {
        /// <summary>
        /// The synchronize
        /// </summary>
        public const int sync = 0;

        /// <summary>
        /// The system identifier
        /// </summary>
        public const int systemId = 8;

        /// <summary>
        /// The data source
        /// </summary>
        public const int dataSource = 9;

        /// <summary>
        /// The message type
        /// </summary>
        public const int messageType = 10;

        /// <summary>
        /// The message sub type
        /// </summary>
        public const int messageSubType = 11;

        /// <summary>
        /// The data length
        /// </summary>
        public const int dataLength = 12;

        /// <summary>
        /// The sequence
        /// </summary>
        public const int sequence = 14;
    }

    /// <summary>
    ///
    /// </summary>
    public static class ServiceMessage
    {
        /// <summary>
        /// The message code
        /// </summary>
        public const int messageCode = 16;

        /// <summary>
        /// The message text
        /// </summary>
        public const int messageText = 20;
    }

    /// <summary>
    /// Class Header.
    /// </summary>
    public static class Header
    {
        /// <summary>
        /// The synchronize
        /// </summary>
        public const int Sync = 0;

        /// <summary>
        /// The system identifier
        /// </summary>
        public const int SystemId = 8;

        /// <summary>
        /// The data source
        /// </summary>
        public const int DataSource = 9;

        /// <summary>
        /// The message type
        /// </summary>
        public const int MessageType = 10;

        /// <summary>
        /// The message sub type
        /// </summary>
        public const int MessageSubType = 11;

        /// <summary>
        /// The data length
        /// </summary>
        public const int DataLength = 12;

        /// <summary>
        /// The sequence
        /// </summary>
        public const int Sequence = 14;

        /// <summary>
        /// The reserved
        /// </summary>
        public const int Reserved = 16;

        /// <summary>
        /// The data format rev
        /// </summary>
        public const int DataFormatREV = 18;
    }

    /// <summary>
    /// Class Parameter.
    /// </summary>
    public static class Parameter
    {
        /// <summary>
        /// The left time stamp
        /// </summary>
        public const int LeftTimeStamp = 20;

        /// <summary>
        /// The right time stamp
        /// </summary>
        public const int RightTimeStamp = 24;

        /// <summary>
        /// The event flags
        /// </summary>
        public const int EventFlags = 28;

        /// <summary>
        /// The operating state
        /// </summary>
        public const int OperatingState = 30;

        /// <summary>
        /// The acousing power
        /// </summary>
        public const int AcousingPower = 31;

        /// <summary>
        /// The sample length
        /// </summary>
        public const int SampleLength = 32;

        /// <summary>
        /// The user depth
        /// </summary>
        public const int UserDepth = 33;

        /// <summary>
        /// The PRF
        /// </summary>
        public const int PRF = 34;

        /// <summary>
        /// The tic
        /// </summary>
        public const int TIC = 36;

        /// <summary>
        /// The rfu
        /// </summary>
        public const int RFU = 38;

        /// <summary>
        /// The emboli count
        /// </summary>
        public const int EmboliCount = 40;
    }

    /// <summary>
    /// Class Envelop.
    /// </summary>
    public static class Envelop
    {
        /// <summary>
        /// The depth
        /// </summary>
        public const int Depth = 44;

        /// <summary>
        /// The velocity unit
        /// </summary>
        public const int VelocityUnit = 46;

        /// <summary>
        /// The col index position
        /// </summary>
        public const int ColIndexPos = 48;

        /// <summary>
        /// The position velocity
        /// </summary>
        public const int PosVelocity = 50;

        /// <summary>
        /// The position peak
        /// </summary>
        public const int PosPeak = 52;

        /// <summary>
        /// The position mean
        /// </summary>
        public const int PosMean = 54;

        /// <summary>
        /// The position dias
        /// </summary>
        public const int PosDias = 56;

        /// <summary>
        /// The position pi
        /// </summary>
        public const int PosPI = 58;

        /// <summary>
        /// The position ri
        /// </summary>
        public const int PosRI = 60;

        /// <summary>
        /// The col index neg
        /// </summary>
        public const int ColIndexNeg = 62;

        /// <summary>
        /// The neg velocity
        /// </summary>
        public const int NegVelocity = 64;

        /// <summary>
        /// The neg peak
        /// </summary>
        public const int NegPeak = 66;

        /// <summary>
        /// The neg mean
        /// </summary>
        public const int NegMean = 68;

        /// <summary>
        /// The neg dias
        /// </summary>
        public const int NegDias = 70;

        /// <summary>
        /// The neg pi
        /// </summary>
        public const int NegPI = 72;

        /// <summary>
        /// The neg ri
        /// </summary>
        public const int NegRI = 74;
    }

    /// <summary>
    /// Class Spectrum.
    /// </summary>
    public static class Spectrum
    {
        /// <summary>
        /// The depth
        /// </summary>
        public const int Depth = 76;

        /// <summary>
        /// The clutter filter
        /// </summary>
        public const int ClutterFilter = 78;

        /// <summary>
        /// The automatic gain offset
        /// </summary>
        public const int AutoGainOffset = 80;

        /// <summary>
        /// The start velocity
        /// </summary>
        public const int StartVelocity = 82;

        /// <summary>
        /// The end velocity
        /// </summary>
        public const int EndVelocity = 84;

        /// <summary>
        /// The points per column
        /// </summary>
        public const int PointsPerColumn = 86;

        /// <summary>
        /// The points
        /// </summary>
        public const int Points = 88;
    }

    /// <summary>
    /// Class MMode.
    /// </summary>
    public static class MMode
    {
        /// <summary>
        /// The automatic gain offset
        /// </summary>
        public static int AutoGainOffset;

        /// <summary>
        /// The start depth
        /// </summary>
        public static int StartDepth;

        /// <summary>
        /// The end depth
        /// </summary>
        public static int EndDepth;

        /// <summary>
        /// The points per column
        /// </summary>
        public static int PointsPerColumn;

        /// <summary>
        /// The power
        /// </summary>
        public static int Power;

        /// <summary>
        /// The velocity
        /// </summary>
        public static int Velocity;

        static MMode()
        {
            if(DMIProtocol.Is256FFTEnable)
                AutoGainOffset = DMIProtocol.SpectrumPointsCount * 2 + Spectrum.Points;
            else
                AutoGainOffset = DMIProtocol.FFT128_SPECTRUM_POINTS * 2 + Spectrum.Points;

            StartDepth = AutoGainOffset + 2;
            EndDepth = StartDepth + 2;
            PointsPerColumn = EndDepth + 2;
            Power = PointsPerColumn + 2;
            Velocity = DMIProtocol.DMI_PKT_MMODE_PTS * 2 + Power;
        }
    }

    /// <summary>
    /// Class Audio.
    /// </summary>
    public static class Audio
    {
        /// <summary>
        /// The depth
        /// </summary>
        public static int Depth = DMIProtocol.DMI_PKT_MMODE_PTS * 2 + MMode.Velocity;

        /// <summary>
        /// The rfu
        /// </summary>
        public static int RFU = Depth + 2;

        /// <summary>
        /// The sample rate
        /// </summary>
        public static int SampleRate = RFU + 2;

        /// <summary>
        /// The maximum amplitude
        /// </summary>
        public static int MaxAmplitude = SampleRate + 2;

        /// <summary>
        /// The toward
        /// </summary>
        public static int Toward = MaxAmplitude + 2;

        /// <summary>
        /// The away
        /// </summary>
        public static int Away = DMIProtocol.DMI_AUDIO_ARRAY_SIZE * 2 + Toward;
    }

    /// <summary>
    /// Class EDetect.
    /// </summary>
    public static class EDetect
    {
        /// <summary>
        /// The phase a mq
        /// </summary>
        public static int PhaseA_MQ = DMIProtocol.DMI_AUDIO_ARRAY_SIZE * 2 + Audio.Away;

        /// <summary>
        /// The phase a clut count
        /// </summary>
        public static int PhaseA_ClutCount = PhaseA_MQ + 4;

        /// <summary>
        /// The phase a me position
        /// </summary>
        public static int PhaseA_MEPosition = PhaseA_ClutCount + 4;

        /// <summary>
        /// The phase a m sum
        /// </summary>
        public static int PhaseA_MSum = PhaseA_MEPosition + 4;

        /// <summary>
        /// The phase a mp local
        /// </summary>
        public static int PhaseA_MPLocal = PhaseA_MSum + 4;

        /// <summary>
        /// The phase a ae flag
        /// </summary>
        public static int PhaseA_AEFlag = PhaseA_MPLocal + 4;

        /// <summary>
        /// The phase a ae down count
        /// </summary>
        public static int PhaseA_AEDownCount = PhaseA_AEFlag + 4;

        /// <summary>
        /// The phase a ae detect
        /// </summary>
        public static int PhaseA_AEDetect = PhaseA_AEDownCount + 4;

        /// <summary>
        /// The phase b mq
        /// </summary>
        public static int PhaseB_MQ = PhaseA_AEDetect + 4;

        /// <summary>
        /// The phase b clut count
        /// </summary>
        public static int PhaseB_ClutCount = PhaseB_MQ + 4;

        /// <summary>
        /// The phase b me position
        /// </summary>
        public static int PhaseB_MEPosition = PhaseB_ClutCount + 4;

        /// <summary>
        /// The phase b m sum
        /// </summary>
        public static int PhaseB_MSum = PhaseB_MEPosition + 4;

        /// <summary>
        /// The phase b mp local
        /// </summary>
        public static int PhaseB_MPLocal = PhaseB_MSum + 4;

        /// <summary>
        /// The phase b ae flag
        /// </summary>
        public static int PhaseB_AEFlag = PhaseB_MPLocal + 4;

        /// <summary>
        /// The phase b ae down count
        /// </summary>
        public static int PhaseB_AEDownCount = PhaseB_AEFlag + 4;

        /// <summary>
        /// The phase b ae detect
        /// </summary>
        public static int PhaseB_AEDetect = PhaseB_AEDownCount + 4;

        /// <summary>
        /// The e detect value
        /// </summary>
        public static int EDetectValue = PhaseB_AEDetect + 4;
    }

    /// <summary>
    /// Class Archive.
    /// </summary>
    public static class Archive
    {
        /// <summary>
        /// The time series depth
        /// </summary>
        public static int TimeSeriesDepth = EDetect.EDetectValue + 4;

        /// <summary>
        /// The rfu
        /// </summary>
        public static int RFU = TimeSeriesDepth + 2;

        /// <summary>
        /// The timeseries i
        /// </summary>
        public static int TimeseriesI = RFU + 2;

        /// <summary>
        /// The timeseries q
        /// </summary>
        public static int TimeseriesQ = TimeseriesI + 4;

        /// <summary>
        /// The mmode phase a
        /// </summary>
        public static int MmodePhaseA = DMIProtocol.DMI_ARCHIVE_TIMESERIES_SIZE * 8 + TimeseriesI;

        /// <summary>
        /// The mmode phase b
        /// </summary>
        public static int mmodePhaseB = DMIProtocol.DMI_ARCHIVE_MMODE_GATES * 4 + MmodePhaseA;

        /// <summary>
        /// The mmode power a
        /// </summary>
        public static int mmodePowerA = DMIProtocol.DMI_ARCHIVE_MMODE_GATES * 4 + mmodePhaseB;

        /// <summary>
        /// The mmode power b
        /// </summary>
        public static int mmodePowerB = DMIProtocol.DMI_ARCHIVE_MMODE_GATES * 4 + mmodePowerA;
    }

    /// <summary>
    /// Class Checksum.
    /// </summary>
    public static class Checksum
    {
        /// <summary>
        /// The checksum position
        /// </summary>
        public static int ChecksumPos = DMIProtocol.DMI_ARCHIVE_MMODE_GATES * 4 + Archive.mmodePowerB;
    }

    #endregion Property positions
}