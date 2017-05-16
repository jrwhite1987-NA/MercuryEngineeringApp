// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="ISettingRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace Core.Interfaces.SettingInterface
{
    /// <summary>
    /// Interface ISettingRepository
    /// </summary>
    public interface ISettingRepository
    {
        //load settings of a user
        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        Dictionary<string, string> GetSetting();

        //update setting of a user
        /// <summary>
        /// Updates the setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateSetting(Dictionary<string, string> setting);
    }
}