// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="ExamProcedureSetting.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class ExamProcedureSetting.
    /// </summary>
    public class ExamProcedureSetting
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the exam procedure identifier.
        /// </summary>
        /// <value>The exam procedure identifier.</value>
        public int ExamProcedureID { get; set; }

        /// <summary>
        /// Gets or sets the vessel.
        /// </summary>
        /// <value>The vessel.</value>
        public string Vessel { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExamProcedureSetting"/> is chkboxon2.
        /// </summary>
        /// <value><c>true</c> if chkboxon2; otherwise, <c>false</c>.</value>
        public bool Chkboxon2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExamProcedureSetting"/> is chkboxon.
        /// </summary>
        /// <value><c>true</c> if chkboxon; otherwise, <c>false</c>.</value>
        public bool Chkboxon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ExamProcedureSetting"/> is chkboxoff.
        /// </summary>
        /// <value><c>true</c> if chkboxoff; otherwise, <c>false</c>.</value>
        public bool Chkboxoff { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamProcedureSetting"/> class.
        /// </summary>
        public ExamProcedureSetting()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamProcedureSetting"/> class.
        /// </summary>
        /// <param name="iD">The i d.</param>
        /// <param name="examProcedureID">The exam procedure identifier.</param>
        /// <param name="vessel">The vessel.</param>
        /// <param name="ultrasonicPower">The ultrasonic power.</param>
        /// <param name="depth">The depth.</param>
        public ExamProcedureSetting(int iD, int examProcedureID, string vessel, int depth)
        {
            ID = iD;
            ExamProcedureID = examProcedureID;
            Vessel = vessel;
            Depth = depth;
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
    /// Class ExamProcedureSettings.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.ExamModels.ExamProcedureSetting}" />
    public class ExamProcedureSettings : IEnumerable<ExamProcedureSetting>
    {
        /// <summary>
        /// Gets or sets the exam procedure setting list.
        /// </summary>
        /// <value>The exam procedure setting list.</value>
        public IList<ExamProcedureSetting> ExamProcedureSettingList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamProcedureSettings"/> class.
        /// </summary>
        public ExamProcedureSettings()
        {
            ExamProcedureSettingList = new List<ExamProcedureSetting>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ExamProcedureSetting> GetEnumerator()
        {
            return ExamProcedureSettingList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ExamProcedureSettingList.GetEnumerator();
        }
    }
}