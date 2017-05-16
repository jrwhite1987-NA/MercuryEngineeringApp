// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="SnapShotReportModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Models.ReportModels
{
    /// <summary>
    /// Class SnapShotReport.
    /// </summary>
    public class SnapShotReport
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the report identifier.
        /// </summary>
        /// <value>The report identifier.</value>
        public int ReportID { get; set; }

        /// <summary>
        /// Gets or sets the snap shot identifier.
        /// </summary>
        /// <value>The snap shot identifier.</value>
        public int SnapShotID { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapShotReport"/> class.
        /// </summary>
        /// <param name="reportSnapShotID">The report snap shot identifier.</param>
        /// <param name="reportID">The report identifier.</param>
        /// <param name="snapShotID">The snap shot identifier.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="createdDateTime">The created date time.</param>
        public SnapShotReport(int reportSnapShotID, int reportID, int snapShotID, string notes, DateTime createdDateTime)
        {
            ID = reportSnapShotID;
            ReportID = reportID;
            SnapShotID = snapShotID;
            Notes = notes;
            CreatedDateTime = createdDateTime;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Class SnapShotReports.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.ReportModels.SnapShotReport}" />
    public class SnapShotReports : IEnumerable<SnapShotReport>
    {
        /// <summary>
        /// Gets or sets the snap shot report list.
        /// </summary>
        /// <value>The snap shot report list.</value>
        public IList<SnapShotReport> SnapShotReportList { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<SnapShotReport> GetEnumerator()
        {
            return SnapShotReportList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return SnapShotReportList.GetEnumerator();
        }
    }
}