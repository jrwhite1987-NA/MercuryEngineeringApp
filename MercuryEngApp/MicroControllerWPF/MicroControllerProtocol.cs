// ***********************************************************************
// Assembly         : MicrochipController
// Author           : belapurkar_s
// Created          : 07-22-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="MicroControllerProtocol.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace MicrochipController
{
    /// <summary>
    /// Class MicroControllerProtocol.
    /// </summary>
   internal class MicroControllerProtocol
    {
        /// <summary>
        /// The mc vendor identifier
        /// </summary>
       internal const int MCVendorID = 0x04D8;
       /// <summary>
       /// The mc product identifier
       /// </summary>
       internal const int MCProductID = 0x0053;

       /// <summary>
       /// The micro controller command
       /// </summary>
       internal const int MicroControllerCommand = 0x80;
       /// <summary>
       /// The micro controller request
       /// </summary>
       internal const byte MicroControllerRequest = 0x81;

       /// <summary>
       /// The TCD control req
       /// </summary>
       internal const int TCDControlReq = 0x01;
       /// <summary>
       /// Power Microcontroller Battery commands
       /// </summary>
       internal const byte BatteryVoltageValueRequest = 0x09;
       internal const byte BatteryCurrentRequest = 0x0a;
       internal const byte BatteryStateRequest = 0x0d;
       internal const byte RemainingChargeRequest = 0x0f;
       internal const byte FullChargeRequest = 0x10;
       internal const byte BatteryVoltageRequest = 0x50;
       /// <summary>
       /// The version number request
       /// </summary>
       internal const byte VersionNumberRequest = 0x42;

       /// <summary>
       /// The eom
       /// </summary>
       internal const byte EOM = 0x00;
       /// <summary>
       /// The response MSG length
       /// </summary>
       internal const uint RESPONSE_MSG_LENGTH = 64;
       /// <summary>
       /// The request MSG length
       /// </summary>
       internal const int REQUEST_MSG_LENGTH = 3;
       /// <summary>
       /// The command MSG length
       /// </summary>
       internal const int COMMAND_MSG_LENGTH = 4;
    }
 
}
