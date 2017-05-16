// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="ExamModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class Exam.
    /// </summary>
    public class Exam
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the exam information identifier.
        /// </summary>
        /// <value>The exam information identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ExamInfoID { get; set; }

        /// <summary>
        /// Gets or sets the physician.
        /// </summary>
        /// <value>The physician.</value>
        public string Physician { get; set; }

        /// <summary>
        /// Gets or sets the technologist.
        /// </summary>
        /// <value>The technologist.</value>
        public string Technologist { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the diagnosis.
        /// </summary>
        /// <value>The diagnosis.</value>
        public string Diagnosis { get; set; }

        /// <summary>
        /// Gets or sets the conclusion.
        /// </summary>
        /// <value>The conclusion.</value>
        public string Conclusion { get; set; }

        /// <summary>
        /// Gets or sets the accession number.
        /// </summary>
        /// <value>The accession number.</value>
        public string AccessionNumber { get; set; }

        /// <summary>
        /// Gets or sets the scheduled time.
        /// </summary>
        /// <value>The scheduled time.</value>
        public DateTime ScheduledTime { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>The patient identifier.</value>
        public int PatientID { get; set; }

        /// <summary>
        /// Gets or sets the exam procedure identifier.
        /// </summary>
        /// <value>The exam procedure identifier.</value>
        public int ExamProcedureID { get; set; }

        /// <summary>
        /// Gets or sets the string created date time.
        /// </summary>
        /// <value>The string created date time.</value>
        public string StrCreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the PDF image file path.
        /// </summary>
        /// <value>The PDF image file path.</value>
        public BitmapImage PdfImageFilePath { get; set; }

        /// <summary>
        /// Gets or sets the type of the exam.
        /// </summary>
        /// <value>The type of the exam.</value>
        public string ExamType { get; set; }

        /// <summary>
        /// Gets or sets the left lindegaard ratio of the exam.
        /// </summary>
        /// <value>The left LR.</value>
        public float LRLeft { get; set; }

        /// <summary>
        /// Gets or sets the right lindegaard ratio of the exam.
        /// </summary>
        /// <value>The right LR.</value>
        public float LRRight { get; set; }

        /// <summary>
        /// Gets or sets the left emboli count of the exam.
        /// </summary>
        /// <value>The left emboli count.</value>
        public int EmboliLeft { get; set; }

        /// <summary>
        /// Gets or sets the right emboli count of the exam.
        /// </summary>
        /// <value>The right emboli count.</value>
        public int EmboliRight { get; set; }

        #endregion "Properties"

        #region "PropertyEvents"

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion "PropertyEvents"
    }

    /// <summary>
    /// Class Exams.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.ExamModels.Exam}" />
    public class Exams : IEnumerable<Exam>
    {
        /// <summary>
        /// Gets or sets the exam list.
        /// </summary>
        /// <value>The exam list.</value>
        public IList<Exam> ExamList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Exams"/> class.
        /// </summary>
        public Exams()
        {
            ExamList = new List<Exam>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Exam> GetEnumerator()
        {
            return ExamList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ExamList.GetEnumerator();
        }
    }
}