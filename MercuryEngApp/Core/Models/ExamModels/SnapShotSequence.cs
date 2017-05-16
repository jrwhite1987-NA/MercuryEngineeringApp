// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="SnapShotSequence.cs" company="">
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
    /// Class SnapShotSequence.
    /// </summary>
    public class SnapShotSequence
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the snapshot sequence identifier.
        /// </summary>
        /// <value>The snapshot sequence identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int SnapshotSequenceID { get; set; }

        /// <summary>
        /// Gets or sets the report identifier.
        /// </summary>
        /// <value>The report identifier.</value>
        public int ReportID { get; set; }

        /// <summary>
        /// Gets or sets the exam snapshot identifier.
        /// </summary>
        /// <value>The exam snapshot identifier.</value>
        public int ExamSnapshotID { get; set; }

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public string Sequence { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapShotSequence"/> class.
        /// </summary>
        public SnapShotSequence()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapShotSequence"/> class.
        /// </summary>
        /// <param name="snapshotSequenceID">The snapshot sequence identifier.</param>
        /// <param name="reportID">The report identifier.</param>
        /// <param name="examSnapshotID">The exam snapshot identifier.</param>
        /// <param name="sequence">The sequence.</param>
        public SnapShotSequence(int snapshotSequenceID, int reportID, int examSnapshotID, string sequence)
        {
            SnapshotSequenceID = snapshotSequenceID;
            ReportID = reportID;
            ExamSnapshotID = examSnapshotID;
            Sequence = sequence;
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
    /// Class SnapShotSequences.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.ExamModels.SnapShotSequence}" />
    public class SnapShotSequences : IEnumerable<SnapShotSequence>
    {
        /// <summary>
        /// Gets or sets the snap shot sequence list.
        /// </summary>
        /// <value>The snap shot sequence list.</value>
        public IList<SnapShotSequence> SnapShotSequenceList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapShotSequences"/> class.
        /// </summary>
        public SnapShotSequences()
        {
            SnapShotSequenceList = new List<SnapShotSequence>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<SnapShotSequence> GetEnumerator()
        {
            return SnapShotSequenceList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return SnapShotSequenceList.GetEnumerator();
        }
    }
}