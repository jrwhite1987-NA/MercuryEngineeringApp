// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="PatientWithExamInfoModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models.PatientWithExamInfoModels
{
    /// <summary>
    /// Class PatientWithExamInfo.
    /// </summary>
    public class PatientWithExamInfo
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>The patient identifier.</value>
        public int PatientID { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>The name of the patient.</value>
        public string PatientName { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>The dob.</value>
        public Nullable<DateTime> DOB { get; set; }

        /// <summary>
        /// Gets or sets the string dob.
        /// </summary>
        /// <value>The string dob.</value>
        public string StrDOB { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>The MRN.</value>
        public string MRN { get; set; }

        /// <summary>
        /// Gets or sets the exam date.
        /// </summary>
        /// <value>The exam date.</value>
        public string ExamDate { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the dt exam date.
        /// </summary>
        /// <value>The dt exam date.</value>
        public Nullable<DateTime> DtExamDate { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientWithExamInfo"/> class.
        /// </summary>
        public PatientWithExamInfo()
        {
            //empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientWithExamInfo"/> class.
        /// </summary>
        /// <param name="patientID">The patient identifier.</param>
        /// <param name="patientName">Name of the patient.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="mrn">The MRN.</param>
        /// <param name="examDate">The exam date.</param>
        /// <param name="gender">The gender.</param>
        public PatientWithExamInfo(int patientID, string patientName, DateTime dob, string mrn, string examDate, string gender)
        {
            PatientID = patientID;
            PatientName = patientName;
            DOB = dob;
            MRN = mrn;
            ExamDate = examDate;
            Gender = gender;
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

    /// <summary>
    /// Class PatientWithExamInfos.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.PatientWithExamInfoModels.PatientWithExamInfo}" />
    public class PatientWithExamInfos : IEnumerable<PatientWithExamInfo>
    {
        /// <summary>
        /// Gets or sets the patient with exam information list.
        /// </summary>
        /// <value>The patient with exam information list.</value>
        public IList<PatientWithExamInfo> PatientWithExamInfoList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientWithExamInfos"/> class.
        /// </summary>
        public PatientWithExamInfos()
        {
            PatientWithExamInfoList = new List<PatientWithExamInfo>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<PatientWithExamInfo> GetEnumerator()
        {
            return PatientWithExamInfoList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return PatientWithExamInfoList.GetEnumerator();
        }
    }
}