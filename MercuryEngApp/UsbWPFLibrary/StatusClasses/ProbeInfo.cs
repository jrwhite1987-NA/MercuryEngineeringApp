using Core.Common;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsbTcdLibrary.StatusClasses
{
    /// <summary>
    /// Class ProbeStatus.
    /// </summary>
    public class ProbeInfo
    {
        /// <summary>
        /// The description string
        /// </summary>
        public string descriptionString;

        /// <summary>
        /// The part number
        /// </summary>
        public string partNumber;

        /// <summary>
        /// The serial number string
        /// </summary>
        public string serialNumberString;

        /// <summary>
        /// The physical identifier
        /// </summary>
        public byte physicalId;

        /// <summary>
        /// The format identifier
        /// </summary>
        public byte formatId;

        /// <summary>
        /// The center frequency
        /// </summary>
        public ushort centerFrequency;

        /// <summary>
        /// The diameter
        /// </summary>
        public ushort diameter;

        /// <summary>
        /// The tank focal length
        /// </summary>
        public ushort tankFocalLength;

        /// <summary>
        /// The tic
        /// </summary>
        public ushort TIC;

        /// <summary>
        /// The fractional bw
        /// </summary>
        public ushort fractionalBW;

        /// <summary>
        /// The impedance
        /// </summary>
        public ushort impedance;

        /// <summary>
        /// The phase angle
        /// </summary>
        public ushort phaseAngle;

        /// <summary>
        /// The insertion loss
        /// </summary>
        public ushort insertionLoss;

        /// <summary>
        /// The adjusted m1
        /// </summary>
        public ushort adjustedM1;

        /// <summary>
        /// The adjusted m2
        /// </summary>
        public ushort adjustedM2;

        private const int DESCRIPTION_STRING_INDEX = 0;
        private const int DESCRIPTION_STRING_COUNT = 30;

        private const int PART_NUMBER_INDEX = 30;
        private const int PART_NUMBER_COUNT = 16;

        private const int SERIAL_NUMBER_STRING_INDEX = 46;
        private const int SERIAL_NUMBER_STRING_COUNT = 12;

        private const int PHYSICAL_ID_BUFFER_SIZE = 58;
        private const int FORMAT_ID_BUFFER_SIZE = 59;

        private const int CENTER_FREQUENCY_START_INDEX = 60;
        private const int DIAMETER_START_INDEX = 62;
        private const int TANK_FOCAL_LENGTH_START_INDEX = 64;
        private const int TIC_START_INDEX = 66;
        private const int FRACTIONAL_BW_START_INDEX = 68;
        private const int IMPEDANCE_START_INDEX = 70;
        private const int PHASE_ANGLE_START_INDEX = 72;
        private const int INSERTION_LOSS_START_INDEX = 74;
        private const int ADJUSTED_M1_START_INDEX = 76;
        private const int ADJUSTED_M2_START_INDEX = 78;

        public ProbeInfo()
        {
            descriptionString = MessageConstants.NotAvailable;
            partNumber = MessageConstants.NotAvailable;
            serialNumberString = MessageConstants.NotAvailable;
            physicalId = 0;
            formatId = 0;
            centerFrequency = 0;
            diameter = 0;
            tankFocalLength = 0;
            TIC = 0;
            fractionalBW = 0;
            impedance = 0;
            phaseAngle = 0;
            insertionLoss = 0;
            adjustedM1 = 0;
            adjustedM2 = 0;
        }

        internal static ProbeInfo ConvertArrayToInfo(byte[] data)
        {
            ProbeInfo probeInfoObj = null;
            try
            {
                probeInfoObj = new ProbeInfo();
                probeInfoObj.descriptionString = Helper.ConvertBytesToString(data, DESCRIPTION_STRING_INDEX, DESCRIPTION_STRING_COUNT);
                probeInfoObj.descriptionString = probeInfoObj.descriptionString.Substring(0, probeInfoObj.descriptionString.IndexOf('\0'));

                probeInfoObj.partNumber = Helper.ConvertBytesToString(data, PART_NUMBER_INDEX, PART_NUMBER_COUNT);
                probeInfoObj.partNumber = probeInfoObj.partNumber.Substring(0, probeInfoObj.partNumber.IndexOf('\0'));

                probeInfoObj.serialNumberString = Helper.ConvertBytesToString(data, SERIAL_NUMBER_STRING_INDEX, SERIAL_NUMBER_STRING_COUNT);
                probeInfoObj.serialNumberString = probeInfoObj.serialNumberString.Substring(0, probeInfoObj.serialNumberString.IndexOf('\0'));

                probeInfoObj.physicalId = data[PHYSICAL_ID_BUFFER_SIZE];
                probeInfoObj.formatId = data[FORMAT_ID_BUFFER_SIZE];

                probeInfoObj.centerFrequency = BitConverter.ToUInt16(data, CENTER_FREQUENCY_START_INDEX);
                probeInfoObj.diameter = BitConverter.ToUInt16(data, DIAMETER_START_INDEX);
                probeInfoObj.tankFocalLength = BitConverter.ToUInt16(data, TANK_FOCAL_LENGTH_START_INDEX);
                probeInfoObj.TIC = BitConverter.ToUInt16(data, TIC_START_INDEX);
                probeInfoObj.fractionalBW = BitConverter.ToUInt16(data, FRACTIONAL_BW_START_INDEX);
                probeInfoObj.impedance = BitConverter.ToUInt16(data, IMPEDANCE_START_INDEX);
                probeInfoObj.phaseAngle = BitConverter.ToUInt16(data, PHASE_ANGLE_START_INDEX);
                probeInfoObj.insertionLoss = BitConverter.ToUInt16(data, INSERTION_LOSS_START_INDEX);
                probeInfoObj.adjustedM1 = BitConverter.ToUInt16(data, ADJUSTED_M1_START_INDEX);
                probeInfoObj.adjustedM2 = BitConverter.ToUInt16(data, ADJUSTED_M2_START_INDEX);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<ProbeInfo>(ex, "ConvertArrayToInfo", Severity.Debug);
            }
            return probeInfoObj;
        }

        /// <summary>
        /// Convert ProbeInfo object into byte array.
        /// </summary>
        /// <param name="probeInfoObj"></param>
        /// <returns></returns>
        internal static byte[] ConvertProbeInfoToArray(ProbeInfo probeInfoObj)
        {
            List<byte> byteArray = new List<byte>(96);
            char[] bufferCharArr = new char[58];

            probeInfoObj.descriptionString.ToCharArray().CopyTo(bufferCharArr, DESCRIPTION_STRING_INDEX);
            probeInfoObj.partNumber.ToCharArray().CopyTo(bufferCharArr, PART_NUMBER_INDEX);
            probeInfoObj.serialNumberString.ToCharArray().CopyTo(bufferCharArr, SERIAL_NUMBER_STRING_INDEX);

            byteArray.AddRange(Encoding.UTF8.GetBytes(bufferCharArr));

            byteArray.Add(probeInfoObj.physicalId);
            byteArray.Add(probeInfoObj.formatId);
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.centerFrequency));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.diameter));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.tankFocalLength));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.TIC));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.fractionalBW));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.impedance));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.phaseAngle));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.insertionLoss));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.adjustedM1));
            byteArray.AddRange(BitConverter.GetBytes(probeInfoObj.adjustedM2));

            return byteArray.ToArray();
        }
    }
}