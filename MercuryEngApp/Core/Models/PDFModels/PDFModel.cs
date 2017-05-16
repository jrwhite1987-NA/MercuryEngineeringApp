// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-22-2016
// ***********************************************************************
// <copyright file="PDFModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.ExamModels;
using Core.Models.RangeIndicatorModels;
using System.ComponentModel;

namespace Core.Models
{
    /// <summary>
    /// PDF Model class.
    /// </summary>
    public class PDF
    {
        #region Properties

        /// <summary>
        /// Gets or sets the patient.
        /// </summary>
        /// <value>The patient.</value>
        public Patient Patient { get; set; }

        /// <summary>
        /// Gets or sets the exam.
        /// </summary>
        /// <value>The exam.</value>
        public Exam Exam { get; set; }

        /// <summary>
        /// Gets or sets the exam snap shots.
        /// </summary>
        /// <value>The exam snap shots.</value>
        public ExamSnapShots ExamSnapShots { get; set; }

        /// <summary>
        /// Gets or sets the exam vessels.
        /// </summary>
        /// <value>The exam vessels.</value>
        public NormalRangeIndicators ExamVessels { get; set; }

        /// <summary>
        /// Gets or sets the age range.
        /// </summary>
        /// <value>The age range.</value>
        public string AgeRange { get; set; }

        /// <summary>
        /// Gets or sets the emboli count.
        /// </summary>
        /// <value>The emboli count.</value>
        public int EmboliCount { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PDF"/> class.
        /// </summary>
        public PDF()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PDF"/> class.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="exam">The exam.</param>
        /// <param name="examSnapShots">The exam snap shots.</param>
        /// <param name="examVessels">The exam vessels.</param>
        /// <param name="ageRange">The age range.</param>
        /// <param name="emboliCount">The emboli count.</param>
        public PDF(Patient patient, Exam exam, ExamSnapShots examSnapShots, NormalRangeIndicators examVessels, string ageRange, int emboliCount)
        {
            Patient = patient;
            Exam = exam;
            ExamSnapShots = examSnapShots;
            ExamVessels = examVessels;
            AgeRange = ageRange;
            EmboliCount = emboliCount;
        }

        #endregion Constructors

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
    }
}