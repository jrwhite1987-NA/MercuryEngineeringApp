// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 06-17-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 06-17-2016
// ***********************************************************************
// <copyright file="TestReviewInfoHolder.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class TestReviewInfoHolder.
    /// </summary>
    public class TestReviewInfoHolder
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestReviewInfoHolder"/> class.
        /// </summary>
        public TestReviewInfoHolder()
        {
            //empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestReviewInfoHolder"/> class.
        /// </summary>
        /// <param name="notes">The notes.</param>
        public TestReviewInfoHolder(string notes)
        {
            Notes = notes;
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