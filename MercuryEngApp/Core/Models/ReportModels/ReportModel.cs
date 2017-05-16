// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="ReportModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Core.Models.ReportModels
{
    /// <summary>
    /// Class Report.
    /// </summary>
    public class Report
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the exam information identifier.
        /// </summary>
        /// <value>The exam information identifier.</value>
        public int ExamInfoID { get; set; }

        /// <summary>
        /// Gets or sets the assessment.
        /// </summary>
        /// <value>The assessment.</value>
        public string Assessment { get; set; }

        /// <summary>
        /// Gets or sets the conclusion.
        /// </summary>
        /// <value>The conclusion.</value>
        public string Conclusion { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        #endregion "Properties"
    }
}