// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="PatientModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models
{
    /// <summary>
    /// Class Patient.
    /// </summary>
    public class Patient : INotifyPropertyChanged
    {
        #region Constants

        /// <summary>
        /// The not deleted
        /// </summary>
        private const int NOT_DELETED = 0;

        #endregion Constants

        #region "Properties"

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>The patient identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int PatientID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [SQLite.NotNull]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        /// <value>The name of the middle.</value>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the identification number.
        /// </summary>
        /// <value>The identification number.</value>
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the visit identification number.
        /// </summary>
        /// <value>The visit identification number.</value>
        public string VisitIdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>The dob.</value>
        public DateTime DOB { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the is deleted.
        /// </summary>
        /// <value>The is deleted.</value>
        public int IsDeleted { get; set; }

        #endregion "Properties"

        #region "Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        public Patient()
        {
            IsDeleted = NOT_DELETED;
        }

        #endregion "Constructors

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
    /// Class Patients.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.Patient}" />
    public class Patients : IEnumerable<Patient>
    {
        /// <summary>
        /// Gets or sets the patient list.
        /// </summary>
        /// <value>The patient list.</value>
        public IList<Patient> PatientList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Patients"/> class.
        /// </summary>
        public Patients()
        {
            PatientList = new List<Patient>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Patient> GetEnumerator()
        {
            return PatientList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return PatientList.GetEnumerator();
        }
    }
}