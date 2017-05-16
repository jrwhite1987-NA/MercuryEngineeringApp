// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="CVRModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.CVRModels
{
    /// <summary>
    /// Class CVR.
    /// </summary>
    public class CVR
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the CVR data file.
        /// </summary>
        /// <value>The CVR data file.</value>
        public string CVRDataFile { get; set; }

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
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is automatic mode.
        /// </summary>
        /// <value><c>true</c> if this instance is automatic mode; otherwise, <c>false</c>.</value>
        public bool IsAutoMode { get; set; }

        /// <summary>
        /// Gets or sets the probe identifier.
        /// </summary>
        /// <value>The probe identifier.</value>
        public int ProbeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is CVR seen.
        /// </summary>
        /// <value><c>true</c> if this instance is CVR seen; otherwise, <c>false</c>.</value>
        public bool IsCVRSeen { get; set; }

        #endregion "Properties"
    }

    /// <summary>
    /// Class CVRs.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.CVRModels.CVR}" />
    public class CVRs : IEnumerable<CVR>
    {
        /// <summary>
        /// Gets or sets the CVR list.
        /// </summary>
        /// <value>The CVR list.</value>
        public IList<CVR> CVRList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CVRs"/> class.
        /// </summary>
        public CVRs()
        {
            CVRList = new List<CVR>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<CVR> GetEnumerator()
        {
            return CVRList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return CVRList.GetEnumerator();
        }
    }

    /// <summary>
    /// Class CalculatedCVRValues.
    /// </summary>
    public class CalculatedCVRValues
    {
        #region "Properties"

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

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedCVRValues"/> class.
        /// </summary>
        public CalculatedCVRValues()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatedCVRValues"/> class.
        /// </summary>
        /// <param name="cvr">The CVR.</param>
        /// <param name="bhi">The bhi.</param>
        public CalculatedCVRValues(double cvr, double bhi)
        {
            this.CVR = cvr;
            this.BHI = bhi;
        }

        #endregion Constructors
    }
}