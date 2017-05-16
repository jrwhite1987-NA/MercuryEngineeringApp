// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="IReportBusiness.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.ReportModels;

namespace Core.Interfaces.ReportInterface
{
    /// <summary>
    /// Interface IReportBusiness
    /// </summary>
    public interface IReportBusiness
    {
        /// <summary>
        /// Gets the max value of Report Id from the Report table.
        /// </summary>
        /// <returns>Max value of Id present in the table.</returns>
        int GetMaxReportId();

        /// <summary>
        /// Is Report Exist
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns><c>true</c> if [is report exist] [the specified exam identifier]; otherwise, <c>false</c>.</returns>
        bool IsReportExist(int examId);

        /// <summary>
        /// Get Report By ExamId
        /// </summary>
        /// <param name="examID">The exam identifier.</param>
        /// <returns>Report</returns>
        Report GetReportByExamId(int examID);

        /// <summary>
        /// Save Report
        /// </summary>
        /// <param name="newReport">The new report.</param>
        /// <returns>bool</returns>
        bool SaveReport(Report newReport);

        /// <summary>
        /// Update Report with the data
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns>update success or fail as bool</returns>
        bool UpdateExamNotesAssessment(Report report);
    }
}