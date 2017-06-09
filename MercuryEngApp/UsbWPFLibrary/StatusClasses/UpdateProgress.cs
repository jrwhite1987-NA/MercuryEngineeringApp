// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="UpdateProgress.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Constants;
using System;

namespace UsbTcdLibrary.StatusClasses
{
    /// <summary>
    /// Class UpdateProgress.
    /// </summary>
    public class UpdateProgress
    {
        /// <summary>
        /// The status code start index
        /// </summary>
        private const int STATUS_CODE_START_INDEX = 0;
        /// <summary>
        /// The bytes received erased start index
        /// </summary>
        private const int BYTES_RECEIVED_ERASED_START_INDEX = 4;
        /// <summary>
        /// The bytes written start index
        /// </summary>
        private const int BYTES_WRITTEN_START_INDEX = 8;

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public UpdateStatusCode StatusCode { get; set; }
        /// <summary>
        /// The status code
        /// </summary>
        private uint statusCode;
        /// <summary>
        /// Gets or sets the bytes received erased.
        /// </summary>
        /// <value>The bytes received erased.</value>
        public uint BytesReceivedErased { get; set; }
        /// <summary>
        /// Gets or sets the bytes written.
        /// </summary>
        /// <value>The bytes written.</value>
        public uint BytesWritten { get; set; }

        /// <summary>
        /// Converts the array to information.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>UpdateProgress.</returns>
        internal static UpdateProgress ConvertArrayToInfo(ref byte[] data)
        {
            UpdateProgress updateInfo = new UpdateProgress();
            updateInfo.statusCode = BitConverter.ToUInt32(data, STATUS_CODE_START_INDEX);
            updateInfo.StatusCode = (UpdateStatusCode)updateInfo.statusCode;
            updateInfo.BytesReceivedErased = BitConverter.ToUInt32(data, BYTES_RECEIVED_ERASED_START_INDEX);
            updateInfo.BytesWritten = BitConverter.ToUInt32(data, BYTES_WRITTEN_START_INDEX);
            return updateInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProgress"/> class.
        /// </summary>
        public UpdateProgress()
        {
            StatusCode = UpdateStatusCode.Ready;
            BytesReceivedErased = Constants.VALUE_0;
            BytesWritten = Constants.VALUE_0;
        }
    }
}