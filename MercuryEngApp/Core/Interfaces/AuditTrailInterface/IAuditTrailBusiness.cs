// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="IAuditTrailBusiness.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.AuditTrailModels;

namespace Core.Interfaces.AuditTrailInterface
{
    /// <summary>
    /// Interface IAuditTrailBusiness
    /// </summary>
    public interface IAuditTrailBusiness
    {
        /// <summary>
        /// Saves the audit log.
        /// </summary>
        /// <param name="oldValueModel">The old value model.</param>
        /// <param name="newValueModel">The new value model.</param>
        /// <returns>System.Int32.</returns>
        int SaveAuditLog(AuditTrailExport oldValueModel, AuditTrailExport newValueModel);

        /// <summary>
        /// Gets all audit trail.
        /// </summary>
        /// <returns>AuditTrailExports.</returns>
        AuditTrailExports GetAllAuditTrail();

        /// <summary>
        /// Gets the audit trail by date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>AuditTrailExports.</returns>
        AuditTrailExports GetAuditTrailByDate(string startDate, string endDate);
    }
}