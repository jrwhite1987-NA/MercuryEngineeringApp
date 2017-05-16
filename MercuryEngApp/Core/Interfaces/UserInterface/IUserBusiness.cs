// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-05-2016
// ***********************************************************************
// <copyright file="IUserBusiness.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models;
using Core.Models.UsersWithRolesModels;

namespace Core.Interfaces.UserInterface
{
    /// <summary>
    /// Interface IUserBusiness
    /// </summary>
    public interface IUserBusiness
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Users.</returns>
        Users GetAllUsers();

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>User.</returns>
        User GetUser(int userID);

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="newUser">The new user.</param>
        /// <returns>User.</returns>
        User SaveUser(User newUser);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateUser(User user);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteUser(int userID);

        /// <summary>
        /// Searches the users.
        /// </summary>
        /// <param name="userWithRole">The user with role.</param>
        /// <returns>UserWithRoles.</returns>
        UserWithRoles SearchUsers(UserWithRole userWithRole);

        /// <summary>
        /// Gets all users with roles.
        /// </summary>
        /// <returns>UserWithRoles.</returns>
        UserWithRoles GetAllUsersWithRoles();
    }
}