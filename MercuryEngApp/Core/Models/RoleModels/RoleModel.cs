// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="RoleModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Core.Models
{
    /// <summary>
    /// Class Role.
    /// </summary>
    public class Role
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
        {
            //empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        public Role(int roleId, string roleName, bool isDeleted)
        {
            RoleID = roleId;
            RoleName = roleName;
            IsDeleted = isDeleted;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Class Roles.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.Role}" />
    public class Roles : IEnumerable<Role>
    {
        /// <summary>
        /// Gets or sets the role list.
        /// </summary>
        /// <value>The role list.</value>
        public IList<Role> RoleList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Roles"/> class.
        /// </summary>
        public Roles()
        {
            RoleList = new List<Role>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Role> GetEnumerator()
        {
            return RoleList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return RoleList.GetEnumerator();
        }
    }
}