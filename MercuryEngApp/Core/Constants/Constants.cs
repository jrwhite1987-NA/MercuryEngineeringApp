// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="Constants.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.Constants
{
    /// <summary>
    /// Class Constants.
    /// </summary>
    public class Constants
    {
        #region Validation Constant

        /// <summary>
        /// The regular expression pattern
        /// </summary>
        public const string RegularExpressionPattern = @"^([a-zA-Z0-9!@#$%^&*]{3,20})$";

        /// <summary>
        /// The regular expression email
        /// </summary>
        public const string RegularExpressionEmail = @"^[^@]+@[^@]+\.[^@]+$";

        #endregion Validation Constant

        #region Image Assets Constants

        /// <summary>
        /// The image folder path
        /// </summary>
        public const string ImageFolderPath = "Assets\\Images";

        /// <summary>
        /// The PDF report logo file
        /// </summary>
        public const string PDFReportLogoFile = "PDF_Report_Logo.png";

        /// <summary>
        /// The Video Play button
        /// </summary>
        public const string PDFPlayImg = "Play.png";

        #endregion Image Assets Constants

        #region TCD Control Constants

        /// <summary>
        /// The minimum gain
        /// </summary>
        public const int minGain = -39;

        /// <summary>
        /// The maximum gain
        /// </summary>
        public const int maxGain = 39;

        /// <summary>
        /// The minimum power
        /// </summary>
        public const uint minPower = 0;

        /// <summary>
        /// The maximum power
        /// </summary>
        public const uint maxPower = 100;

        /// <summary>
        /// The minimum length
        /// </summary>
        public const uint minLength = 2;

        /// <summary>
        /// The maximum length
        /// </summary>
        public const uint maxLength = 12;

        /// <summary>
        /// The minimum depth
        /// </summary>
        public const uint minDepth = 23;

        /// <summary>
        /// The maximum depth
        /// </summary>
        public const uint maxDepth = 146;

        /// <summary>
        /// The minimum audio
        /// </summary>
        public const uint minAudio = 0;

        /// <summary>
        /// The maximum audio
        /// </summary>
        public const uint maxAudio = 10;

        /// <summary>
        /// The maximum filter1
        /// </summary>
        public const uint maxFilter1 = 600;

        /// <summary>
        /// The maximum filter2
        /// </summary>
        public const uint maxFilter2 = 2400;

        /// <summary>
        /// The minimum filter1
        /// </summary>
        public const uint minFilter1 = 25;

        /// <summary>
        /// The minimum filter2
        /// </summary>
        public const uint minFilter2 = 100;

        /// <summary>
        /// The delta filter low
        /// </summary>
        public const int DeltaFilterLow = 25;
        /// <summary>
        /// The delta filter high
        /// </summary>
        public const int DeltaFilterHigh = 100;

        /// <summary>
        /// The default step frequency
        /// </summary>
        public const int defaultStepFrequency = 10;

        /// <summary>
        /// The default gain
        /// </summary>
        public const int defaultGain = 0;

        /// <summary>
        /// The default power
        /// </summary>
        public const uint defaultPower = 10; //value changed for testing, default was 10

        /// <summary>
        /// The default length
        /// </summary>
        public const uint defaultLength = 10;

        /// <summary>
        /// The default depth
        /// </summary>
        public const uint defaultDepth = 50;

        /// <summary>
        /// The default audio
        /// </summary>
        public const uint defaultAudio = 30;

        /// <summary>
        /// The default filter
        /// </summary>
        public const uint defaultFilter = 300;

        /// <summary>
        /// The default PRF
        /// </summary>
        public const uint defaultPRF = 8000;

        public const byte defaultStartDepth = 23;
        /// <summary>
        /// Can be used 19.3;
        /// </summary>
        public const double defaultDividingFactor = 27.3;

        /// <summary>
        /// The default lower limit
        /// </summary>
        public const double defaultLowerLimit = 1000;

        /// <summary>
        /// The power values
        /// </summary>
        public static List<uint> powerValues = new List<uint> { 0, 1, 2, 5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        /// <summary>
        /// The default tic
        /// </summary>
        public const float defaultTIC = 1.0f;

        /// <summary>
        /// Enum CurrentMode
        /// </summary>
        public enum CurrentMode : int { LeftChannel = 1, BothChannels, RightChannel, None };

        #endregion TCD Control Constants

        #region Channel Constants

        /// <summary>
        /// The channel 1
        /// </summary>
        public const int CHANNEL_1 = 1;
        /// <summary>
        /// The channel 2
        /// </summary>
        public const int CHANNEL_2 = 2;

        #endregion Channel Constants

        #region Vessel Constants

        /// <summary>
        /// The position vessels
        /// </summary>
        public static List<string> posVessels = new List<string>() { "LMCA", "RMCA", "LTICA", "RTICA", "LPCA", "RPCA", "LOPTH", "ROPTH", "LSIPHN", "RSIPHN" };

        /// <summary>
        /// The neg vessels
        /// </summary>
        public static List<string> negVessels = new List<string>() { "LACA", "RACA", "LICA", "RICA", "LVERT", "RVERT", "BASILAR" };

        /// <summary>
        /// The div vessels
        /// </summary>
        public static List<string> divVessels = new List<string>() { "LBIFUR", "RBIFUR" };

        /// <summary>
        /// The low powered vessels
        /// </summary>
        public static List<string> lowPoweredVessels = new List<string>() { "RICA", "LICA", "ROPTH", "RSIPHN", "LOPTH", "LSIPHN" };

        #endregion Vessel Constants

        #region Page Level Constants

        /// <summary>
        /// The admin
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// The female
        /// </summary>
        public const string Female = "Female";

        /// <summary>
        /// The male
        /// </summary>
        public const string Male = "Male";

        /// <summary>
        /// The zero value
        /// </summary>
        public const int VALUE_0 = 0;

        /// <summary>
        /// The one value
        /// </summary>
        public const int VALUE_1 = 1;

        /// <summary>
        /// The two value
        /// </summary>
        public const int VALUE_2 = 2;

        /// <summary>
        /// The two value
        /// </summary>
        public const int VALUE_3 = 3;

        /// <summary>
        /// The value 4
        /// </summary>
        public const int VALUE_4 = 4;

        /// <summary>
        /// The value 5
        /// </summary>
        public const int VALUE_5 = 5;
        /// <summary>
        /// The value 6
        /// </summary>
        public const int VALUE_6 = 6;
        /// <summary>
        /// The value 7
        /// </summary>
        public const int VALUE_7 = 7;
        /// <summary>
        /// The value 8
        /// </summary>
        public const int VALUE_8 = 8;
        /// <summary>
        /// The value 10
        /// </summary>
        public const int VALUE_10 = 10;
        /// <summary>
        /// The value 11
        /// </summary>
        public const int VALUE_11 = 11;
        /// <summary>
        /// The value 12
        /// </summary>
        public const int VALUE_12 = 12;
        /// <summary>
        /// The value 13
        /// </summary>
        public const int VALUE_13 = 13;
        /// <summary>
        /// The value 15
        /// </summary>
        public const int VALUE_15 = 15;
        /// <summary>
        /// The value 18
        /// </summary>
        public const int VALUE_18 = 18;
        /// <summary>
        /// The value 20
        /// </summary>
        public const int VALUE_20 = 20;
        /// <summary>
        /// The value 23
        /// </summary>
        public const int VALUE_23 = 23;
        /// <summary>
        /// The value 25
        /// </summary>
        public const int VALUE_25 = 25;
        /// <summary>
        /// The value 28
        /// </summary>
        public const int VALUE_28 = 28;
        /// <summary>
        /// The value 29
        /// </summary>
        public const int VALUE_29 = 29;
        /// <summary>
        /// The value 30
        /// </summary>
        public const int VALUE_30 = 30;
        /// <summary>
        /// The value 31
        /// </summary>
        public const int VALUE_31 = 31;
        /// <summary>
        /// The value 33
        /// </summary>
        public const int VALUE_33 = 33;
        /// <summary>
        /// The value 34
        /// </summary>
        public const int VALUE_34 = 34;
        /// <summary>
        /// The value 35
        /// </summary>
        public const int VALUE_35 = 35;
        /// <summary>
        /// The value 38
        /// </summary>
        public const int VALUE_38 = 38;
        /// <summary>
        /// The value 39
        /// </summary>
        public const int VALUE_39 = 39;
        /// <summary>
        /// The value 40
        /// </summary>
        public const int VALUE_40 = 40;
        /// <summary>
        /// The value 42
        /// </summary>
        public const int VALUE_42 = 42;
        /// <summary>
        /// The value 50
        /// </summary>
        public const int VALUE_50 = 50;
        /// <summary>
        /// The value 52
        /// </summary>
        public const int VALUE_52 = 52;
        /// <summary>
        /// The value 56
        /// </summary>
        public const int VALUE_56 = 56;
        /// <summary>
        /// The value 60
        /// </summary>
        public const int VALUE_60 = 60;
        /// <summary>
        /// The value 64
        /// </summary>
        public const int VALUE_64 = 64;
        /// <summary>
        /// The value 68
        /// </summary>
        public const int VALUE_68 = 68;
        /// <summary>
        /// The value 72
        /// </summary>
        public const int VALUE_72 = 72;
        /// <summary>
        /// The value 73
        /// </summary>
        public const int VALUE_73 = 73;
        /// <summary>
        /// The value 80
        /// </summary>
        public const int VALUE_80 = 80;
        /// <summary>
        /// The value 81
        /// </summary>
        public const int VALUE_81 = 81;
        /// <summary>
        /// The value 87
        /// </summary>
        public const int VALUE_87 = 87;
        /// <summary>
        /// The value 88
        /// </summary>
        public const int VALUE_88 = 88;
        /// <summary>
        /// The value 90
        /// </summary>
        public const int VALUE_90 = 90;
        /// <summary>
        /// The value 100
        /// </summary>
        public const int VALUE_100 = 100;
        /// <summary>
        /// The value 104
        /// </summary>
        public const int VALUE_104 = 104;
        /// <summary>
        /// The value 105
        /// </summary>
        public const int VALUE_105 = 105;
        /// <summary>
        /// The value 115
        /// </summary>
        public const int VALUE_115 = 115;
        /// <summary>
        /// The value 120
        /// </summary>
        public const int VALUE_120 = 120;
        /// <summary>
        /// The value 128
        /// </summary>
        public const int VALUE_128 = 128;
        /// <summary>
        /// The value 130
        /// </summary>
        public const int VALUE_130 = 130;
        /// <summary>
        /// The value 132
        /// </summary>
        public const int VALUE_132 = 132;
        /// <summary>
        /// The value 134
        /// </summary>
        public const int VALUE_134 = 134;
        /// <summary>
        /// The value 140
        /// </summary>
        public const int VALUE_140 = 140;
        /// <summary>
        /// The value 146
        /// </summary>
        public const int VALUE_146 = 146;
        /// <summary>
        /// The value 149
        /// </summary>
        public const int VALUE_149 = 149;
        /// <summary>
        /// The value 150
        /// </summary>
        public const int VALUE_150 = 150;
        /// <summary>
        /// The value 180
        /// </summary>
        public const int VALUE_180 = 180;
        /// <summary>
        /// The value 192
        /// </summary>
        public const int VALUE_192 = 192;
        /// <summary>
        /// The value 240
        /// </summary>
        public const int VALUE_240 = 240;
        /// <summary>
        /// The value 250
        /// </summary>
        public const int VALUE_250 = 250;
        /// <summary>
        /// The value 256
        /// </summary>
        public const int VALUE_256 = 256;
        /// <summary>
        /// The value 277
        /// </summary>
        public const int VALUE_277 = 277;
        /// <summary>
        /// The value 295
        /// </summary>
        public const int VALUE_295 = 295;
        /// <summary>
        /// The value 300
        /// </summary>
        public const int VALUE_300 = 300;
        /// <summary>
        /// The value 308
        /// </summary>
        public const int VALUE_308 = 308;
        /// <summary>
        /// The value 321
        /// </summary>
        public const int VALUE_321 = 321;
        /// <summary>
        /// The value 385
        /// </summary>
        public const int VALUE_385 = 385;
        /// <summary>
        /// The value 431
        /// </summary>
        public const int VALUE_431 = 431;
        /// <summary>
        /// The value 432
        /// </summary>
        public const int VALUE_432 = 432;
        /// <summary>
        /// The value 444
        /// </summary>
        public const int VALUE_444 = 444;
        /// <summary>
        /// The value 480
        /// </summary>
        public const int VALUE_480 = 480;
        /// <summary>
        /// The value 500
        /// </summary>
        public const int VALUE_500 = 500;
        /// <summary>
        /// The value 750
        /// </summary>
        public const int VALUE_750 = 750;
        /// <summary>
        /// The value 1000
        /// </summary>
        public const int VALUE_1000 = 1000;
        /// <summary>
        /// The value 1500
        /// </summary>
        public const int VALUE_1500 = 1500;
        /// <summary>
        /// The value 1540
        /// </summary>
        public const int VALUE_1540 = 1540;
        /// <summary>
        /// The value 5000
        /// </summary>
        public const int VALUE_5000 = 5000;
        /// <summary>
        /// The value 6250
        /// </summary>
        public const int VALUE_6250 = 6250;
        /// <summary>
        /// The value 8000
        /// </summary>
        public const int VALUE_8000 = 8000;
        /// <summary>
        /// The value 10000
        /// </summary>
        public const int VALUE_10000 = 10000;
        /// <summary>
        /// The value 12500
        /// </summary>
        public const int VALUE_12500 = 12500;

        /// <summary>
        /// The value double 10
        /// </summary>
        public const double VALUE_DOUBLE_10 = 10.0;

        /// <summary>
        /// The x axis value of thickness
        /// </summary>
        public const int X_VALUE = -410;

        #endregion Page Level Constants

        #region CVR Constants

        /// <summary>
        /// The default minimum
        /// </summary>
        public const double defaultMin = 0.0;

        /// <summary>
        /// The default maximum
        /// </summary>
        public const double defaultMax = 0.0;

        /// <summary>
        /// The default mean
        /// </summary>
        public const double defaultMean = 0.0;

        /// <summary>
        /// The CVR bitmap height
        /// </summary>
        public const int CVRBitmapHeight = 400;
        /// <summary>
        /// The CVR bitmap width
        /// </summary>
        public const int CVRBitmapWidth = 990;

        /// <summary>
        /// The CVR 900
        /// </summary>
        public const int CVR_900 = 900;
        /// <summary>
        /// The CVR value 10
        /// </summary>
        public const int CVR_VALUE_10 = 10;

        #endregion CVR Constants

        #region Video Constants

        /// <summary>
        /// The frame spee d4 sec
        /// </summary>
        public const int FRAME_SPEED4SEC = 400000;

        /// <summary>
        /// The frame spee d8 sec
        /// </summary>
        public const int FRAME_SPEED8SEC = 400000;

        /// <summary>
        /// The frame spee D12 sec
        /// </summary>
        public const int FRAME_SPEED12SEC = 400000;

        /// <summary>
        /// Video folder name
        /// </summary>
        public const string VIDEO_FOLDER_NAME = "VideoAudioFiles";

        #endregion Video Constants

        #region NA Tech Constants

        /// <summary>
        /// The na tech user name
        /// </summary>
        public const string NaTechUserName = "na_service_tech";

        /// <summary>
        /// The na tech password filename
        /// </summary>
        public const string NaTechPasswordFilename = "NATechnitianLoginDetails.txt";

        #endregion NA Tech Constants

        #region Log

        /// <summary>
        /// The log folder name
        /// </summary>
        public const string LogFolderName = "MetroLogs";

        #endregion Log

        #region Password

        /// <summary>
        /// The password constant
        /// </summary>
        public const string PasswordConst = "******";

        #endregion Password

        #region Exam Parameter Constants

        /// <summary>
        /// The positive
        /// </summary>
        public const string Positive = "Pos";

        /// <summary>
        /// The negative
        /// </summary>
        public const string Negative = "Neg";

        /// <summary>
        /// The position and neg both
        /// </summary>
        public const string PosAndNegBoth = "Both";

        #endregion Exam Parameter Constants

        #region Exam Procedure Constants

        /// <summary>
        /// The emboli exam name
        /// </summary>
        public const string EXAM_NAME_EMBOLI = "Emboli";

        /// <summary>
        /// The emboli exam identifier
        /// </summary>
        public const int EXAM_ID_EMBOLI = 8;

        /// <summary>
        /// The bubble exam name
        /// </summary>
        public const string EXAM_NAME_BUBBLE = "Bubble";

        /// <summary>
        /// The bubble exam identifier
        /// </summary>
        public const int EXAM_ID_BUBBLE = 7;

        /// <summary>
        /// The CVR exam name
        /// </summary>
        public const string EXAM_NAME_CVR = "CVR";

        /// <summary>
        /// The CVR exam identifier
        /// </summary>
        public const int EXAM_ID_CVR = 6;

        /// <summary>
        /// The monitoring exam name
        /// </summary>
        public const string EXAM_NAME_MONITORING = "Monitoring";

        /// <summary>
        /// The monitoring exam identifier
        /// </summary>
        public const int EXAM_ID_MONITORING = 5;

        /// <summary>
        /// The open exam name
        /// </summary>
        public const string EXAM_NAME_OPEN = "Open";

        /// <summary>
        /// The open exam identifier
        /// </summary>
        public const int EXAM_ID_OPEN = 4;

        /// <summary>
        /// The detail exam name
        /// </summary>
        public const string EXAM_NAME_DETAIL = "Detail";

        /// <summary>
        /// The detail exam identifier
        /// </summary>
        public const int EXAM_ID_DETAIL = 3;

        /// <summary>
        /// The limited exam name
        /// </summary>
        public const string EXAM_NAME_LIMITED = "Limited";

        /// <summary>
        /// The limited exam identifier
        /// </summary>
        public const int EXAM_ID_LIMITED = 2;

        /// <summary>
        /// The complete exam name
        /// </summary>
        public const string EXAM_NAME_COMPLETE = "Complete";

        /// <summary>
        /// The complete exam identifier
        /// </summary>
        public const int EXAM_ID_COMPLETE = 1;

        #endregion Exam Procedure Constants

        #region Exam Size Constant

        //Space according to 256FFT and 5MB per video so 50MB video buffer
        /// <summary>
        /// The required free space for exam
        /// </summary>
        public const ulong REQUIRED_FREE_SPACE_FOR_EXAM = 1819883648;

        //10 MB - 10*1024*1024 = 10485760 bytes
        /// <summary>
        /// The internal hard disk buffer space
        /// </summary>
        public const ulong INTERNAL_HARD_DISK_BUFFER_SPACE = 31457280;

        /// <summary>
        /// The disk space warning safe
        /// </summary>
        public const int DISK_SPACE_WARNING_SAFE = 0;

        /// <summary>
        /// The disk space warning below threshold
        /// </summary>
        public const int DISK_SPACE_WARNING_BELOW_THRESHOLD = 1;

        /// <summary>
        /// The disk space warning critical
        /// </summary>
        public const int DISK_SPACE_WARNING_CRITICAL = 2;

        /// <summary>
        /// The disk space warning undetermined
        /// </summary>
        public const int DISK_SPACE_WARNING_UNDETERMINED = -1;

        #endregion Exam Size Constant

        #region Export Constants

        /// <summary>
        /// The export today
        /// </summary>
        public const int EXPORT_TODAY = 0;

        /// <summary>
        /// The export past week
        /// </summary>
        public const int EXPORT_PAST_WEEK = 7;

        /// <summary>
        /// The export past month
        /// </summary>
        public const int EXPORT_PAST_MONTH = 30;

        /// <summary>
        /// The export everything
        /// </summary>
        public const int EXPORT_EVERYTHING = -1;

        /// <summary>
        /// The export no duration choosen
        /// </summary>
        public const int EXPORT_NO_DURATION_CHOOSEN = -2;

        #endregion Export Constants

        #region Audit Error Constants

        /// <summary>
        /// The audit no trails
        /// </summary>
        public const int AUDIT_NO_TRAILS = 0;

        /// <summary>
        /// The audit successful
        /// </summary>
        public const int AUDIT_SUCCESSFUL = 1;

        /// <summary>
        /// The audit ex
        /// </summary>
        public const int AUDIT_EX = -1;

        /// <summary>
        /// The audit file name
        /// </summary>
        public const string AUDIT_FILE_NAME = "AuditLog.csv";

        #endregion Audit Error Constants

        #region Graph Constants

        /// <summary>
        /// The FFT size
        /// </summary>
       // private const int fftSize;

        /// <summary>
        /// Gets or sets the size of the FFT.
        /// </summary>
        /// <value>The size of the FFT.</value>
        public const int FFTSize  = 128;
        //{
        //    get
        //    {
        //        return fftSize;
        //    }
        //    set
        //    {
        //        fftSize = value;
        //        //fftSize = value;
        //        //DefaultBaseline = (value + Constants.VALUE_1) / Constants.VALUE_2;
        //        //BaselineValue = value / Constants.VALUE_2;
        //        //SpectrumBitmapHeight = value + Constants.VALUE_1;
        //    }
        //}


        private static int spectrumBin = FFTSize;
        public static int SpectrumBin
        {
            get {return spectrumBin;}
            set { spectrumBin = value; }
        }

        public static int SpectrumBitmapHeight = 257;

        /// <summary>
        /// The value 34
        /// </summary>
        public const int MMODE_BITMAP_HEIGHT = 34;

        /// <summary>
        /// The value 708
        /// </summary>
        public const int TREND_BITMAP_WIDTH = 750;

        /// <summary>
        /// The value 708
        /// </summary>
        public const int TREND_BITMAP_HEIGHT = 308;

        /// <summary>
        /// The default baseline
        /// </summary>
        /// <value>The default baseline.</value>
        public static int DefaultBaseline { get; set; }

        /// <summary>
        /// Gets or sets the baseline value.
        /// </summary>
        /// <value>The baseline value.</value>
        public static int BaselineValue { get; set; }

        /// <summary>
        /// The decrement factor
        /// </summary>
        public const int DecrementFactor = 11;

        /// <summary>
        /// The bytes for color
        /// </summary>
        public const int BytesForColor = 4;

        #endregion Graph Constants

        #region Delete Exam Return Value

        /// <summary>
        /// The delete successful
        /// </summary>
        public const int DELETE_SUCCESSFUL = 1;

        /// <summary>
        /// The delete unsuccessful
        /// </summary>
        public const int DELETE_UNSUCCESSFUL = 0;

        /// <summary>
        /// The delete file failed
        /// </summary>
        public const int DELETE_FILE_FAILED = 2;

        #endregion Delete Exam Return Value

        #region ScrollBar

        /// <summary>
        /// The scrollbar start index
        /// </summary>
        public const int SCROLLBAR_START_INDEX = 2;

        #endregion ScrollBar

        #region Common

        /// <summary>
        /// The blankspace
        /// </summary>
        public const string BLANKSPACE = " ";

        /// <summary>
        /// The newline
        /// </summary>
        public const string NEWLINE = "\n";

        #endregion Common

        #region Battery Constants

        /// <summary>
        /// The battery full
        /// </summary>
        public const string Battery_Full = @"ms-appx:///Assets/Images/NA_btn_bottom_battery_1_80x58.png";
        /// <summary>
        /// The battery high
        /// </summary>
        public const string Battery_High = @"ms-appx:///Assets/Images/NA_btn_bottom_battery_2_80x58.png";
        /// <summary>
        /// The battery medium
        /// </summary>
        public const string Battery_Medium = @"ms-appx:///Assets/Images/NA_btn_bottom_battery_Medium_80x58.png";
        /// <summary>
        /// The battery low
        /// </summary>
        public const string Battery_Low = @"ms-appx:///Assets/Images/NA_btn_bottom_battery_Low_80x58.png";
        /// <summary>
        /// The battery very low
        /// </summary>
        public const string Battery_Very_Low = @"ms-appx:///Assets/Images/NA_btn_bottom_battery_lowsignal_80x58.png";
        /// <summary>
        /// The battery empty
        /// </summary>
        public const string Battery_Empty = @"ms-appx:///Assets/Images/NA_btn_bottom_battery_empty_80x58.png";
        /// <summary>
        /// The battery charging
        /// </summary>
        public const string Battery_Charging = @"ms-appx:///Assets/Images/NA_btn_bottom_battery_Charging_80x58.png";

        /// <summary>
        /// The charge level1
        /// </summary>
        public const int ChargeLevel1 = 75;
        /// <summary>
        /// The charge level2
        /// </summary>
        public const int ChargeLevel2 = 50;
        /// <summary>
        /// The charge level3
        /// </summary>
        public const int ChargeLevel3 = 25;
        public const int ChargeLevel4 = 20;
        public const int ChargeLevel5 = 15;

        /// <summary>
        /// The ac plug current threshold
        /// </summary>
        public const int ACPlugCurrentThreshold = -10;

        /// <summary>
        /// The small interval
        /// </summary>
        public static TimeSpan SmallInterval = TimeSpan.FromSeconds(10);
        /// <summary>
        /// The large interval
        /// </summary>
        public static TimeSpan LargeInterval = TimeSpan.FromSeconds(30);
        /// <summary>
        /// The battery interval
        /// </summary>
        public static TimeSpan BatteryInterval = TimeSpan.FromSeconds(3);
        /// <summary>
        /// The initial interval
        /// </summary>
        public static TimeSpan InitialInterval = TimeSpan.FromMilliseconds(500);

        #endregion Battery Constants

        #region Exam Channel Width

        /// <summary>
        /// The channel width 55
        /// </summary>
        public const int CHANNEL_WIDTH_55 = 55;
        /// <summary>
        /// The channel width 68
        /// </summary>
        public const int CHANNEL_WIDTH_68 = 68;
        /// <summary>
        /// The channel width 46
        /// </summary>
        public const int CHANNEL_WIDTH_46 = 46;
        /// <summary>
        /// The channel width 131
        /// </summary>
        public const int CHANNEL_WIDTH_131 = 131;

        #endregion Exam Channel Width

        #region Exam - Spectrum TimeFrame

        /// <summary>
        /// The spectrum x scale 500
        /// </summary>
        public const int SpectrumXScale_500 = 500;
        /// <summary>
        /// The spectrum x scale 1000
        /// </summary>
        public const int SpectrumXScale_1000 = 1000;
        /// <summary>
        /// The spectrum x scale 1500
        /// </summary>
        public const int SpectrumXScale_1500 = 1500;

        #endregion Exam - Spectrum TimeFrame

        #region Trending Constant

        /// <summary>
        /// The trend 500
        /// </summary>
        public const int TREND_500 = 500;
        /// <summary>
        /// The trend mins 15
        /// </summary>
        public const int TREND_MINS_15 = 15;
        /// <summary>
        /// The trend blocksize 75
        /// </summary>
        public const int TREND_BLOCKSIZE_75 = 75;
        /// <summary>
        /// The trend blocksize 45
        /// </summary>
        public const int TREND_BLOCKSIZE_45 = 45;
        /// <summary>
        /// The trend plot division 5
        /// </summary>
        public const int TREND_PLOT_DIVISION_5 = 5;
        /// <summary>
        /// The trend maximum value
        /// </summary>
        public const int TREND_MAX_VALUE = 4500;

        #endregion Trending Constant

        #region Acoustic Power Constants

        /// <summary>
        /// The display power 0
        /// </summary>
        public const int DISPLAY_POWER_0 = 0;
        /// <summary>
        /// The actual power 0
        /// </summary>
        public const int ACTUAL_POWER_0 = 0;
        /// <summary>
        /// The display power 1
        /// </summary>
        public const int DISPLAY_POWER_1 = 1;
        /// <summary>
        /// The actual power 1
        /// </summary>
        public const int ACTUAL_POWER_1 = 1;
        /// <summary>
        /// The display power 2
        /// </summary>
        public const int DISPLAY_POWER_2 = 2;
        /// <summary>
        /// The actual power 2
        /// </summary>
        public const int ACTUAL_POWER_2 = 2;
        /// <summary>
        /// The display power 5
        /// </summary>
        public const int DISPLAY_POWER_5 = 5;
        /// <summary>
        /// The actual power 5
        /// </summary>
        public const int ACTUAL_POWER_5 = 5;
        /// <summary>
        /// The display power 10
        /// </summary>
        public const int DISPLAY_POWER_10 = 10;
        /// <summary>
        /// The actual power 9
        /// </summary>
        public const int ACTUAL_POWER_9 = 9;
        /// <summary>
        /// The display power 20
        /// </summary>
        public const int DISPLAY_POWER_20 = 20;
        /// <summary>
        /// The actual power 18
        /// </summary>
        public const int ACTUAL_POWER_18 = 18;
        /// <summary>
        /// The display power 30
        /// </summary>
        public const int DISPLAY_POWER_30 = 30;
        /// <summary>
        /// The actual power 27
        /// </summary>
        public const int ACTUAL_POWER_27 = 27;
        /// <summary>
        /// The display power 40
        /// </summary>
        public const int DISPLAY_POWER_40 = 40;
        /// <summary>
        /// The actual power 36
        /// </summary>
        public const int ACTUAL_POWER_36 = 36;
        /// <summary>
        /// The display power 50
        /// </summary>
        public const int DISPLAY_POWER_50 = 50;
        /// <summary>
        /// The actual power 45
        /// </summary>
        public const int ACTUAL_POWER_45 = 45;
        /// <summary>
        /// The display power 60
        /// </summary>
        public const int DISPLAY_POWER_60 = 60;
        /// <summary>
        /// The actual power 54
        /// </summary>
        public const int ACTUAL_POWER_54 = 54;
        /// <summary>
        /// The display power 70
        /// </summary>
        public const int DISPLAY_POWER_70 = 70;
        /// <summary>
        /// The actual power 63
        /// </summary>
        public const int ACTUAL_POWER_63 = 63;
        /// <summary>
        /// The display power 80
        /// </summary>
        public const int DISPLAY_POWER_80 = 80;
        /// <summary>
        /// The actual power 72
        /// </summary>
        public const int ACTUAL_POWER_72 = 72;
        /// <summary>
        /// The display power 90
        /// </summary>
        public const int DISPLAY_POWER_90 = 90;
        /// <summary>
        /// The actual power 81
        /// </summary>
        public const int ACTUAL_POWER_81 = 81;
        /// <summary>
        /// The display power 100
        /// </summary>
        public const int DISPLAY_POWER_100 = 100;
        /// <summary>
        /// The actual power 90
        /// </summary>
        public const int ACTUAL_POWER_90 = 90;

        #endregion Acoustic Power Constants

        #region TestReview Constants

        /// <summary>
        /// The plot area margin left
        /// </summary>
        public const int PLOT_AREA_MARGIN_LEFT = 0;
        /// <summary>
        /// The plot area margin top
        /// </summary>
        public const int PLOT_AREA_MARGIN_TOP = 95;
        /// <summary>
        /// The plot area margin right
        /// </summary>
        public const int PLOT_AREA_MARGIN_RIGHT = 135;
        /// <summary>
        /// The plot area margin bottom
        /// </summary>
        public const int PLOT_AREA_MARGIN_BOTTOM = 0;

        #endregion TestReview Constants

        #region TICLableMargin Constants

        /// <summary>
        /// The tic label left 392
        /// </summary>
        public const int TIC_LABEL_LEFT_392 = 392;
        /// <summary>
        /// The tic label top 27
        /// </summary>
        public const int TIC_LABEL_TOP_27 = 27;
        /// <summary>
        /// The tic label right 770
        /// </summary>
        public const int TIC_LABEL_RIGHT_770 = 770;
        /// <summary>
        /// The tic label bottom 712
        /// </summary>
        public const int TIC_LABEL_BOTTOM_712 = 712;

        /// <summary>
        /// The tic label left 890
        /// </summary>
        public const int TIC_LABEL_LEFT_890 = 890;
        /// <summary>
        /// The tic label left 938
        /// </summary>
        public const int TIC_LABEL_LEFT_938 = 938;
        /// <summary>
        /// The tic label right 272
        /// </summary>
        public const int TIC_LABEL_RIGHT_272 = 272;
        /// <summary>
        /// The tic label right 242
        /// </summary>
        public const int TIC_LABEL_RIGHT_242 = 242;

        /// <summary>
        /// The tic label left 436
        /// </summary>
        public const int TIC_LABEL_LEFT_436 = 436;
        /// <summary>
        /// The tic label right 737
        /// </summary>
        public const int TIC_LABEL_RIGHT_737 = 737;

        #endregion TICLableMargin Constants

        #region Volume Control

        /// <summary>
        /// The v ol control visibility time interval
        /// </summary>
        public const int VOl_CTRL_VISIBILITY_TIME_INTERVAL = 2000;

        #endregion Volume Control

        #region About/Acknowledgment

        /// <summary>
        /// The copyright file path
        /// </summary>
        public const string CopyrightFilePath = "Assets\\Copyright\\Copyright.rtf";

        #endregion About/Acknowledgment

        #region pdf Constants

        /// <summary>
        /// Value 190
        /// </summary>
        public const float VALUE_190 = 190;

        #endregion pdf Constants

        #region CalibrationConstants
        public const int SAFETY_CALIBRATION_MAX_POWER = 150;
        public const int SAFETY_CAL_START_POWER = 135;
        #endregion

        #region LogConstants
        public const string APPLog = "APP";
        public const string TCDLog = "TCD";
        #endregion

        public static List<int> SpectrumBinList
        {
            get
            {
                return new List<int> { 
                    128,
                    256,
                    512 };
            }
        }
        /// <summary>
        /// The packets per sec
        /// </summary>
        public const int PACKETS_PER_SEC = 125;

        /// <summary>
        /// Enum LoggingLevel
        /// </summary>
        public enum LoggingLevel : int { Level0 = 0, Level1, Level2, Level3, Level4 };

        /// <summary>
        /// The time for tc dto load
        /// </summary>
        public static TimeSpan TimeForTCDtoLoad = TimeSpan.FromSeconds(4);

        /// <summary>
        /// The time wait for load
        /// </summary>
        public static TimeSpan TimeWaitForLoad = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Initializes static members of the <see cref="Constants"/> class.
        /// </summary>
        //static Constants()
        //{
        //    FFTSize = Constants.VALUE_128;
        //}
    }

    
    /// <summary>
    /// Class MonthDictionary.
    /// </summary>
    public static class MonthDictionary
    {
        /// <summary>
        /// Enum MonthNumber
        /// </summary>
        private enum MonthNumber
        {
            /// <summary>
            /// The january
            /// </summary>
            January = 1,

            /// <summary>
            /// The february
            /// </summary>
            February = 2,

            /// <summary>
            /// The march
            /// </summary>
            March = 3,

            /// <summary>
            /// The april
            /// </summary>
            April = 4,

            /// <summary>
            /// The may
            /// </summary>
            May = 5,

            /// <summary>
            /// The june
            /// </summary>
            June = 6,

            /// <summary>
            /// The july
            /// </summary>
            July = 7,

            /// <summary>
            /// The august
            /// </summary>
            August = 8,

            /// <summary>
            /// The september
            /// </summary>
            September = 9,

            /// <summary>
            /// The october
            /// </summary>
            October = 10,

            /// <summary>
            /// The november
            /// </summary>
            November = 11,

            /// <summary>
            /// The december
            /// </summary>
            December = 12,
        }

        /// <summary>
        /// The year
        /// </summary>
        public const string YEAR = "Year";

        /// <summary>
        /// The month
        /// </summary>
        public const string MONTH = "Month";

        /// <summary>
        /// The day
        /// </summary>
        public const string DAY = "Day";

        /// <summary>
        /// The month dictionary
        /// </summary>
        private static Dictionary<string, int> monthDictionary = null;

        /// <summary>
        /// The days in months
        /// </summary>
        private static int[] daysInMonths = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        /// <summary>
        /// Initializes static members of the <see cref="MonthDictionary" /> class.
        /// </summary>
        static MonthDictionary()
        {
            monthDictionary = new Dictionary<string, int>();
            monthDictionary.Add("January", (int)MonthNumber.January);
            monthDictionary.Add("February", (int)MonthNumber.February);
            monthDictionary.Add("March", (int)MonthNumber.March);
            monthDictionary.Add("April", (int)MonthNumber.April);
            monthDictionary.Add("May", (int)MonthNumber.May);
            monthDictionary.Add("June", (int)MonthNumber.June);
            monthDictionary.Add("July", (int)MonthNumber.July);
            monthDictionary.Add("August", (int)MonthNumber.August);
            monthDictionary.Add("September", (int)MonthNumber.September);
            monthDictionary.Add("October", (int)MonthNumber.October);
            monthDictionary.Add("November", (int)MonthNumber.November);
            monthDictionary.Add("December", (int)MonthNumber.December);
        }

        /// <summary>
        /// Monthes this instance.
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.Int32&gt;.</returns>
        public static Dictionary<string, int> Month()
        {
            return monthDictionary;
        }

        /// <summary>
        /// Dayses the in month.
        /// </summary>
        /// <returns>System.Int32[].</returns>
        public static int[] DaysInMonth()
        {
            return daysInMonths;
        }
    }

    /// <summary>
    /// Class ColorStroke.
    /// </summary>
    public static class ColorStrokeDisable
    {
        /// <summary>
        /// The alpha
        /// </summary>
        public const byte ALPHA = 0xFF;

        /// <summary>
        /// The red
        /// </summary>
        public const byte RED = 0x4E;

        /// <summary>
        /// The green
        /// </summary>
        public const byte GREEN = 0x4F;

        /// <summary>
        /// The blue
        /// </summary>
        public const byte BLUE = 0x4F;
    }

    /// <summary>
    /// Class ColorStrokeEnable.
    /// </summary>
    public static class ColorStrokeEnable
    {
        /// <summary>
        /// The alpha
        /// </summary>
        public const byte ALPHA = 0xFF;

        /// <summary>
        /// The red
        /// </summary>
        public const byte RED = 0x01;

        /// <summary>
        /// The green
        /// </summary>
        public const byte GREEN = 0xA9;

        /// <summary>
        /// The blue
        /// </summary>
        public const byte BLUE = 0xF6;
    }

    /// <summary>
    /// Struct EnvelopState
    /// </summary>
    public struct EnvelopState
    {
        /// <summary>
        /// The above
        /// </summary>
        public const string Above = "+ Flow";
        /// <summary>
        /// The below
        /// </summary>
        public const string Below = "- Flow";
        /// <summary>
        /// The both
        /// </summary>
        public const string Both = "+/-";
        /// <summary>
        /// The none
        /// </summary>
        public const string None = "None";
        /// <summary>
        /// The automatic
        /// </summary>
        public const string Auto = "Auto";
    }
}