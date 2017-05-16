// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="IUserRoleBusiness.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models;

namespace Core.Interfaces.UserRoleInterface
{
    /// <summary>
    /// Interface IUserRoleBusiness
    /// </summary>
    public interface IUserRoleBusiness
    {
        //Get the Role of an user by user Id
        /// <summary>
        /// Gets the roles by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Roles.</returns>
        Roles GetRolesByUserId(int userId);

        //Delete User-Role of a User when a User is deleted
        /// <summary>
        /// Deletes the roles of user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteRolesOfUser(int userId);

        //Hard delete roles of user
        /// <summary>
        /// Hards the delete roles of user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool HardDeleteRolesOfUser(int userId);

        //Assign Roles to the User
        /// <summary>
        /// Assigns the roles to user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roles">The roles.</param>
        void AssignRolesToUser(int userId, Roles roles);
    }
}