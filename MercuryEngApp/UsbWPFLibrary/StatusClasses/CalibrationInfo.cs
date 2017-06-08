// ***********************************************************************
// Assembly         : Mercury
// Author           : belapurkar_s
// Created          : 06-01-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="CalibrationInfo.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using Core.Common;
using Core.Constants;
using System;
using System.Collections.Generic;

namespace UsbTcdLibrary.StatusClasses
{
    /// <summary>
    /// Class CalibrationInfo.
    /// </summary>
    public class CalibrationInfo
    {
        /// <summary>
        /// Gets or sets the maximum dac intensity.
        /// </summary>
        /// <value>The maximum dac intensity.</value>
        public UInt16 MaxDACIntensity { get; set; }

        /// <summary>
        /// Gets or sets the zero intensity dac.
        /// </summary>
        /// <value>The zero intensity dac.</value>
        public UInt16 ZeroIntensityDAC { get; set; }

        /// <summary>
        /// Gets or sets the gain offset.
        /// </summary>
        /// <value>The gain offset.</value>
        public UInt16 GainOffset { get; set; }

        /// <summary>
        /// Gets or sets the bitmask.
        /// </summary>
        /// <value>The bitmask.</value>
        public UInt16 Bitmask { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalibrationInfo"/> class.
        /// </summary>
        public CalibrationInfo()
        {
            MaxDACIntensity = Constants.VALUE_0;
            ZeroIntensityDAC = Constants.VALUE_0;
            GainOffset = Constants.VALUE_0;
            Bitmask = Constants.VALUE_0;
        }

        /// <summary>
        /// Method to extract CalibrationInfo
        /// </summary>
        /// <param name="data">byte[]</param>
        /// <returns>CalibrationInfo</returns>
        internal static CalibrationInfo ConvertArrayToInfo(ref byte[] data)
        {
            const int MaxDACIntensityStartIndex = 0;
            const int ZeroIntensityDACStartIndex = 2;
            const int GainOffsetStartIndex = 4;
            const int BitmaskStartIndex = 6;
            try
            {
                CalibrationInfo calibrationDetails = new CalibrationInfo();
                calibrationDetails.MaxDACIntensity = BitConverter.ToUInt16(data, MaxDACIntensityStartIndex);
                calibrationDetails.ZeroIntensityDAC = BitConverter.ToUInt16(data, ZeroIntensityDACStartIndex);
                calibrationDetails.GainOffset = BitConverter.ToUInt16(data, GainOffsetStartIndex);
                calibrationDetails.Bitmask = BitConverter.ToUInt16(data, BitmaskStartIndex);
                return calibrationDetails;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: " + ex);
                return null;
            }
        }

        /// <summary>
        /// Converts the calibration information to array.
        /// </summary>
        /// <param name="calibration">The calibration.</param>
        /// <returns>System.Byte[].</returns>
        internal static byte[] ConvertCalibrationInfoToArray(CalibrationInfo calibration)
        {
            try
            {
                List<byte> bufferData = new List<byte>(8);

                bufferData.AddRange(BitConverter.GetBytes(calibration.MaxDACIntensity));
                bufferData.AddRange(BitConverter.GetBytes(calibration.ZeroIntensityDAC));
                bufferData.AddRange(BitConverter.GetBytes(calibration.GainOffset));
                bufferData.AddRange(BitConverter.GetBytes(calibration.Bitmask));

                return bufferData.ToArray();
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: " + ex);
                return null;
            }
        }
    }
}