// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="BoardInfo.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Common;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsbTcdLibrary.StatusClasses
{
    /// <summary>
    /// Class BoardInfo.
    /// </summary>
    public class BoardInfo
    {
        /// <summary>
        /// The model string
        /// </summary>
        public string modelString;

        /// <summary>
        /// The part number string
        /// </summary>
        public string partNumberString;

        /// <summary>
        /// The serial number string
        /// </summary>
        public string serialNumberString;

        /// <summary>
        /// The hardware revision string
        /// </summary>
        public uint hardwareRevision;

        /// <summary>
        /// The model string index
        /// </summary>
        private const int MODEL_STRING_INDEX = 0;
        /// <summary>
        /// The part number string index
        /// </summary>
        private const int PART_NUMBER_STRING_INDEX = 40;
        /// <summary>
        /// The serial number string index
        /// </summary>
        private const int SERIAL_NUMBER_STRING_INDEX = 60;
        /// <summary>
        /// The hardware version index
        /// </summary>
        private const int HARDWARE_VERSION_INDEX = 80;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardInfo"/> class.
        /// </summary>
        public BoardInfo()
        {
            modelString = MessageConstants.NotAvailable;
            partNumberString = MessageConstants.NotAvailable;
            serialNumberString = MessageConstants.NotAvailable;
            hardwareRevision = Constants.VALUE_0;
        }

        /// <summary>
        /// Converts the board information to array.
        /// </summary>
        /// <param name="boardInfo">The board information.</param>
        /// <returns>System.Byte[].</returns>
        internal static byte[] ConvertBoardInfoToArray(BoardInfo boardInfo)
        {
            try
            {
                List<byte> bufferData = new List<byte>(DMIProtocol.BOARD_INFO_REQUEST_LENGTH);
                char[] bufferCharArr = new char[HARDWARE_VERSION_INDEX];

                boardInfo.modelString.ToCharArray().CopyTo
                    (bufferCharArr, MODEL_STRING_INDEX);
                boardInfo.partNumberString.ToCharArray().CopyTo
                    (bufferCharArr, PART_NUMBER_STRING_INDEX);
                boardInfo.serialNumberString.ToCharArray().CopyTo
                    (bufferCharArr, SERIAL_NUMBER_STRING_INDEX);

                bufferData.AddRange(Encoding.UTF8.GetBytes(bufferCharArr));
                bufferData.AddRange(BitConverter.GetBytes(boardInfo.hardwareRevision));

                return bufferData.ToArray();
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<BoardInfo>(ex, "ConvertBoardInfoToArray", Severity.Warning);
                return null;
            }
        }
    }
}