// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="ExamPatientHolder.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class ExamPatientHolder.
    /// </summary>
    public class ExamPatientHolder
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the exam procedure identifier.
        /// </summary>
        /// <value>The exam procedure identifier.</value>
        public int ExamProcedureId { get; set; }

        /// <summary>
        /// Gets or sets the exam information identifier.
        /// </summary>
        /// <value>The exam information identifier.</value>
        public int ExamInfoID { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>The patient identifier.</value>
        public int PatientID { get; set; }

        /// <summary>
        /// Gets or sets the exam image.
        /// </summary>
        /// <value>The exam image.</value>
        public string ExamImage { get; set; }

        #endregion "Properties"

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