// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="IRoleRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models;

namespace Core.Interfaces.RoleInterface
{
    /// <summary>
    /// Interface IRoleRepository
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>Roles.</returns>
        Roles GetAllRoles();
    }
}