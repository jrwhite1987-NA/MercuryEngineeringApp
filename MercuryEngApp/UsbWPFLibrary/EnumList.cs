// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="EnumList.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace UsbTcdLibrary
{
    /// <summary>
    /// Enum DopplerParameters
    /// </summary>
    public enum DopplerParameters : uint { Power = 1, SampleLength, Depth, ClutterFilter, PRF, MModeFilter, EnvelopeRange = 9, SpectrumBins = 10 };

    /// <summary>
    /// Enum TCDModes
    /// </summary>
    public enum TCDModes : uint { Standby = 0, Operate, Active, Service, Update, Playback };

    /// <summary>
    /// Enum EndpointNumber
    /// </summary>
    public enum EndpointNumber : uint { None = 0, DopplerData = 2, Bulk = 6, EventMessage = 8 };

    /// <summary>
    /// Enum TCDHandles
    /// </summary>
    public enum TCDHandles : int { None = 0, Channel1 = 1, Channel2 = 2 };

    /// <summary>
    /// Enum ActiveChannels
    /// </summary>
    public enum ActiveChannels : int { None = 0, Channel1, Channel2, Both, NoTCD };

    public enum UpdateStatusCode : uint
    {
        Ready = 0,
        Finished = 1,
        UnableToLaunch = 9,
        Receiving = 10,
        ReceivingFailure = 11,
        KeyInvalid = 12,
        TypeInvalid = 13,
        TableInvalid = 14,
        Verifying = 20,
        ChecksumFailure = 21,
        IncompatibleVersion = 22,
        AddressInvalid = 23,
        Writing = 30,
        EraseFailure = 31,
        WriteFailure = 32,
        Confirming = 40,
        ComparisionFailure = 41,
        ReadFailure = 42,
        Aborted = 99,
        Timeout = 100
    }
}