// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="ExamProcedureModel.cs" company="">
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

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class ExamProcedure.
    /// </summary>
    public class ExamProcedure
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamProcedure"/> class.
        /// </summary>
        public ExamProcedure()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamProcedure"/> class.
        /// </summary>
        /// <param name="iD">The i d.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public ExamProcedure(int iD, string name, string description)
        {
            ID = iD;
            Name = name;
            Description = description;
        }

        #endregion Constructors

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
    /// Class ExamProcedures.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.ExamModels.ExamProcedure}" />
    public class ExamProcedures : IEnumerable<ExamProcedure>
    {
        /// <summary>
        /// Gets or sets the exam procedure list.
        /// </summary>
        /// <value>The exam procedure list.</value>
        public IList<ExamProcedure> ExamProcedureList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamProcedures"/> class.
        /// </summary>
        public ExamProcedures()
        {
            ExamProcedureList = new List<ExamProcedure>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ExamProcedure> GetEnumerator()
        {
            return ExamProcedureList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ExamProcedureList.GetEnumerator();
        }
    }

    /// <summary>
    /// Enum ExamProcedureModes
    /// </summary>
    public enum ExamProcedureModes : int
    {
        /// <summary>
        /// The complete
        /// </summary>
        Complete = 1,

        /// <summary>
        /// The limited
        /// </summary>
        Limited = 2,

        /// <summary>
        /// The detailed
        /// </summary>
        Detailed = 3,

        /// <summary>
        /// The open
        /// </summary>
        Open = 4,

        /// <summary>
        /// The monitoring
        /// </summary>
        Monitoring = 5,

        /// <summary>
        /// The CVR
        /// </summary>
        CVR = 6,

        /// <summary>
        /// The bubble
        /// </summary>
        Bubble = 7,

        /// <summary>
        /// The emboli
        /// </summary>
        Emboli = 8
    }
}