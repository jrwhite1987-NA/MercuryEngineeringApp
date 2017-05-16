// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="AuditTrailModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.AuditTrailModels
{
    /// <summary>
    /// Class AuditTrail.
    /// </summary>
    public class AuditTrail
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the audit trail identifier.
        /// </summary>
        /// <value>The audit trail identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int AuditTrailId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the type of the action.
        /// </summary>
        /// <value>The type of the action.</value>
        public string ActionType { get; set; }

        /// <summary>
        /// Gets or sets the old value.
        /// </summary>
        /// <value>The old value.</value>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the new value.
        /// </summary>
        /// <value>The new value.</value>
        public string NewValue { get; set; }

        /// <summary>
        /// Gets or sets the event date time.
        /// </summary>
        /// <value>The event date time.</value>
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public string EntityType { get; set; }

        #endregion "Properties"
    }

    /// <summary>
    /// Class AuditTrails.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.AuditTrailModels.AuditTrail}" />
    public class AuditTrails : IEnumerable<AuditTrail>
    {
        /// <summary>
        /// Gets or sets the audit trails list.
        /// </summary>
        /// <value>The audit trails list.</value>
        public IList<AuditTrail> AuditTrailsList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditTrails"/> class.
        /// </summary>
        public AuditTrails()
        {
            AuditTrailsList = new List<AuditTrail>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<AuditTrail> GetEnumerator()
        {
            return AuditTrailsList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return AuditTrailsList.GetEnumerator();
        }
    }

    /// <summary>
    /// Class AuditTrailExport.
    /// </summary>
    public class AuditTrailExport
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the audit trail identifier.
        /// </summary>
        /// <value>The audit trail identifier.</value>
        public int AuditTrailId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the exam identifier.
        /// </summary>
        /// <value>The exam identifier.</value>
        public int ExamID { get; set; }

        /// <summary>
        /// Gets or sets the type of the exam.
        /// </summary>
        /// <value>The type of the exam.</value>
        public string ExamType { get; set; }

        /// <summary>
        /// Gets or sets the exam date time.
        /// </summary>
        /// <value>The exam date time.</value>
        public DateTime ExamDateTime { get; set; }

        /// <summary>
        /// Gets or sets the patient identifier.
        /// </summary>
        /// <value>The patient identifier.</value>
        public int PatientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        /// <value>The name of the patient.</value>
        public string PatientName { get; set; }

        /// <summary>
        /// Gets or sets the patient dob.
        /// </summary>
        /// <value>The patient dob.</value>
        public DateTime PatientDOB { get; set; }

        /// <summary>
        /// Gets or sets the patient identification number.
        /// </summary>
        /// <value>The patient identification number.</value>
        public string PatientIdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        public string EventType { get; set; }

        /// <summary>
        /// Gets or sets the event date time.
        /// </summary>
        /// <value>The event date time.</value>
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        public string EntityType { get; set; }

        #endregion "Properties"
    }

    /// <summary>
    /// Class AuditTrailExports.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.AuditTrailModels.AuditTrailExport}" />
    public class AuditTrailExports : IEnumerable<AuditTrailExport>
    {
        /// <summary>
        /// Gets or sets the audit trail export list.
        /// </summary>
        /// <value>The audit trail export list.</value>
        public IList<AuditTrailExport> AuditTrailExportList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditTrailExports"/> class.
        /// </summary>
        public AuditTrailExports()
        {
            AuditTrailExportList = new List<AuditTrailExport>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<AuditTrailExport> GetEnumerator()
        {
            return AuditTrailExportList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return AuditTrailExportList.GetEnumerator();
        }
    }
}