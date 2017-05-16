// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 06-17-2016
// ***********************************************************************
// <copyright file="ILoginBusiness.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models;

namespace Core.Interfaces.LoginInterface
{
    /// <summary>
    /// Interface ILoginBusiness
    /// </summary>
    public interface ILoginBusiness
    {
        /// <summary>
        /// Validates the login credentials.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ValidateLoginCredentials(string userName, string password);

        /// <summary>
        /// Generates the salt key.
        /// </summary>
        /// <returns>System.String.</returns>
        string GenerateSaltKey();

        /// <summary>
        /// Generates the final string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="saltKey">The salt key.</param>
        /// <returns>System.String.</returns>
        string GenerateFinalString(string password, string saltKey);

        /// <summary>
        /// Generates the hashed password.
        /// </summary>
        /// <param name="saltedFinaleString">The salted finale string.</param>
        /// <param name="hashAlgorithmName">Name of the hash algorithm.</param>
        /// <returns>System.String.</returns>
        string GenerateHashedPassword(string saltedFinaleString, string hashAlgorithmName);

        /// <summary>
        /// Validates the hashed password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="login">The login.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ValidateHashedPassword(string password, Login login);

        /// <summary>
        /// Saves the login.
        /// </summary>
        /// <param name="loginName">Name of the login.</param>
        /// <param name="password">The password.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="IsFirstLogin">if set to <c>true</c> [is first login].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SaveLogin(string loginName, string password, int userId, bool IsFirstLogin);

        /// <summary>
        /// Deletes the login by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeleteLoginByUserId(int userId);

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="IsFirstLogin">if set to <c>true</c> [is first login].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdatePassword(string newPassword, int userId, bool IsFirstLogin);

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
        /// Validates the old password.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool ValidateOldPassword(int userId, string oldPassword);

        /// <summary>
        /// Validates the name of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Login.</returns>
        Login ValidateUserName(string userName);

        /// <summary>
        /// Updates the na tech login.
        /// </summary>
        /// <param name="newPassword">The new password.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="isFirstLogin">if set to <c>true</c> [is first login].</param>
        /// <param name="loginName">Name of the login.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateNaTechLogin(string newPassword, int userId, bool isFirstLogin, string loginName);
    }
}