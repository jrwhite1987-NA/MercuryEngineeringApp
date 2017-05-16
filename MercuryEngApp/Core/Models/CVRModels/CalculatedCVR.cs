// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="CalculatedCVR.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
namespace Core.Models.CVRModels
{
    /// <summary>
    /// Class CalculatedCVR.
    /// </summary>
    public class CalculatedCVR
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the probe identifier.
        /// </summary>
        /// <value>The probe identifier.</value>
        public int ProbeId { get; set; }

        /// <summary>
        /// Gets or sets the exam identifier.
        /// </summary>
        /// <value>The exam identifier.</value>
        public int ExamID { get; set; }

        /// <summary>
        /// Gets or sets the high.
        /// </summary>
        /// <value>The high.</value>
        public double High { get; set; }

        /// <summary>
        /// Gets or sets the low.
        /// </summary>
        /// <value>The low.</value>
        public double Low { get; set; }

        /// <summary>
        /// Gets or sets the mean.
        /// </summary>
        /// <value>The mean.</value>
        public double Mean { get; set; }

        /// <summary>
        /// Gets or sets the CVR.
        /// </summary>
        /// <value>The CVR.</value>
        public double CVR { get; set; }

        /// <summary>
        /// Gets or sets the bhi.
        /// </summary>
        /// <value>The bhi.</value>
        public double BHI { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic mode.
        /// </summary>
        /// <value><c>true</c> if this instance is automatic mode; otherwise, <c>false</c>.</value>
        public bool IsAutoMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is CVR seen.
        /// </summary>
        /// <value><c>true</c> if this instance is CVR seen; otherwise, <c>false</c>.</value>
        public bool IsCVRSeen { get; set; }

        /// <summary>
        /// Gets or sets the CVR data file.
        /// </summary>
        /// <value>The CVR data file.</value>
        public string CVRDataFile { get; set; }

        #endregion "Properties"
    }

    /// <summary>
    /// Class CVRForChannel.
    /// </summary>
    public class CVRForChannel
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the automatic CVR.
        /// </summary>
        /// <value>The automatic CVR.</value>
        public CVR AutoCVR { get; set; }

        /// <summary>
        /// Gets or sets the manual CVR.
        /// </summary>
        /// <value>The manual CVR.</value>
        public CVR ManualCVR { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is manual CVR changed.
        /// </summary>
        /// <value><c>true</c> if this instance is manual CVR changed; otherwise, <c>false</c>.</value>
        public bool IsManualCVRChanged { get; set; }

        #endregion "Properties"

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CVRForChannel"/> class.
        /// </summary>
        public CVRForChannel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CVRForChannel"/> class.
        /// </summary>
        /// <param name="auto">The automatic.</param>
        /// <param name="manual">The manual.</param>
        /// <param name="manualCVRChanged">if set to <c>true</c> [manual CVR changed].</param>
        public CVRForChannel(CVR auto, CVR manual, bool manualCVRChanged)
        {
            AutoCVR = auto;
            ManualCVR = manual;
            IsManualCVRChanged = manualCVRChanged;
        }

        #endregion Constructor
    }

    /// <summary>
    /// Class CVRForExam.
    /// </summary>
    public class CVRForExam
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the CVR channel1.
        /// </summary>
        /// <value>The CVR channel1.</value>
        public CVRForChannel CVRChannel1 { get; set; }

        /// <summary>
        /// Gets or sets the CVR channel2.
        /// </summary>
        /// <value>The CVR channel2.</value>
        public CVRForChannel CVRChannel2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is manual CVR changed.
        /// </summary>
        /// <value><c>true</c> if this instance is manual CVR changed; otherwise, <c>false</c>.</value>
        public bool IsManualCVRChanged { get; set; }

        #endregion "Properties"

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CVRForExam"/> class.
        /// </summary>
        public CVRForExam()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CVRForExam"/> class.
        /// </summary>
        /// <param name="channel1">The channel1.</param>
        /// <param name="channel2">The channel2.</param>
        public CVRForExam(CVRForChannel channel1, CVRForChannel channel2)
        {
            CVRChannel1 = channel1;
            CVRChannel2 = channel2;
        }

        #endregion Constructor
    }

    public class CVRGraphData
    {
        public IEnumerable<short> GraphData;
        public ulong PacketCount;
    }
}
