// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 06-17-2016
// ***********************************************************************
// <copyright file="ILoginRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models;

namespace Core.Interfaces.LoginInterface
{
    /// <summary>
    /// Interface ILoginRepository
    /// </summary>
    public interface ILoginRepository
    {
        /// <summary>
        /// Validates the login credentials.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Login.</returns>
        Login ValidateLoginCredentials(string userName);

        /// <summary>
        /// Saves the login.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="saltKey">The salt key.</param>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="IsFirstLogin">if set to <c>true</c> [is first login].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SaveLogin(string loginName, string saltKey, string hashedPassword, int userId, bool IsFirstLogin);

        /// <summary>
        /// Deletes the login by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteLoginByUserId(int userId);

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdatePassword(Login login);

        // Get a login by UserID.
        /// <summary>
        /// Gets the login by user identifier.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>Login.</returns>
        Login GetLoginByUserID(int userID);

        /// <summary>
        /// Gets the name of the login by login.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <returns>Login.</returns>
        Login GetLoginByLoginName(string loginName);

        /// <summary>
        /// Validates the name of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Login.</returns>
        Login ValidateUserName(string userName);

        /// <summary>
        /// Updates the na tech login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateNaTechLogin(Login login);
    }
}