// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="ExamEmboliMeasurementModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class ExamEmboliMeasurement.
    /// </summary>
    public class ExamEmboliMeasurement
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the exam identifier.
        /// </summary>
        /// <value>The exam identifier.</value>
        public int ExamID { get; set; }

        /// <summary>
        /// Gets or sets the emboli count.
        /// </summary>
        /// <value>The emboli count.</value>
        public int EmboliCount { get; set; }

        /// <summary>
        /// Gets or sets the xvalue.
        /// </summary>
        /// <value>The xvalue.</value>
        public int Xvalue { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamEmboliMeasurement"/> class.
        /// </summary>
        public ExamEmboliMeasurement()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamEmboliMeasurement"/> class.
        /// </summary>
        /// <param name="examID">The exam identifier.</param>
        /// <param name="emboliCount">The emboli count.</param>
        /// <param name="xValue">The x value.</param>
        public ExamEmboliMeasurement(int examID, int emboliCount, int xValue)
        {
            ExamID = examID;
            EmboliCount = emboliCount;
            Xvalue = xValue;
        }

        #endregion Constructors

        /// <summary>
        /// Class ExamEmboliMeasurements.
        /// </summary>
        /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.ExamModels.ExamEmboliMeasurement}" />
        public class ExamEmboliMeasurements : IEnumerable<ExamEmboliMeasurement>
        {
            /// <summary>
            /// Gets or sets the exam emboli measurement list.
            /// </summary>
            /// <value>The exam emboli measurement list.</value>
            public IList<ExamEmboliMeasurement> ExamEmboliMeasurementList { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ExamEmboliMeasurements"/> class.
            /// </summary>
            public ExamEmboliMeasurements()
            {
                ExamEmboliMeasurementList = new List<ExamEmboliMeasurement>();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>An enumerator that can be used to iterate through the collection.</returns>
            public IEnumerator<ExamEmboliMeasurement> GetEnumerator()
            {
                return ExamEmboliMeasurementList.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return ExamEmboliMeasurementList.GetEnumerator();
            }
        }
    }
}