// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="IAuditTrailRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.AuditTrailModels;

namespace Core.Interfaces.AuditTrailInterface
{
    /// <summary>
    /// Interface IAuditTrailRepository
    /// </summary>
    public interface IAuditTrailRepository
    {
        /// <summary>
        /// Saves the audit log.
        /// </summary>
        /// <param name="newAuditLog">The new audit log.</param>
        /// <returns>System.Int32.</returns>
        int SaveAuditLog(AuditTrail newAuditLog);

        /// <summary>
        /// Gets the type of the audit trail by entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>AuditTrails.</returns>
        AuditTrails GetAuditTrailByEntityType(string entityType);

        /// <summary>
        /// Gets all audit trail.
        /// </summary>
        /// <returns>AuditTrails.</returns>
        AuditTrails GetAllAuditTrail();

        /// <summary>
        /// Gets the audit trail by date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>AuditTrails.</returns>
        AuditTrails GetAuditTrailByDate(string startDate, string endDate);
    }
}