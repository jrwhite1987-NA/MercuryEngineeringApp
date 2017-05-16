// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="UserRoleModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;

namespace Core.Models
{
    /// <summary>
    /// Class UserRole.
    /// </summary>
    public class UserRole
    {
        #region "Properties"

        //[SQLite.PrimaryKey, SQLite.AutoIncrement]
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole"/> class.
        /// </summary>
        public UserRole()
        {
            //empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRole"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        public UserRole(int userId, int roleId, bool isDeleted)
        {
            UserID = userId;
            RoleID = roleId;
            IsDeleted = isDeleted;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Class UserRoles.
    /// </summary>
    public class UserRoles
    {
        /// <summary>
        /// Gets or sets the user role list.
        /// </summary>
        /// <value>The user role list.</value>
        public IList<UserRole> UserRoleList { get; set; }
    }
}