// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="ITCDInfoBusiness.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.TCDModels;

namespace Core.Interfaces.TCDInterface
{
    /// <summary>
    /// Interface ITCDInfoBusiness
    /// </summary>
    public interface ITCDInfoBusiness
    {
        /// <summary>
        /// Saves the TCD information.
        /// </summary>
        /// <param name="newTCDInfo">The new TCD information.</param>
        /// <returns>TCDInfo.</returns>
        TCDInfo SaveTCDInfo(TCDInfo newTCDInfo);
    }
}