// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="RolePrivilegeModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Core.Models
{
    /// <summary>
    /// Class RolePrivilege.
    /// </summary>
    public class RolePrivilege
    {
        #region "Properties"

        //[SQLite.PrimaryKey, SQLite.AutoIncrement]
        /// <summary>
        /// Gets or sets the privilege identifier.
        /// </summary>
        /// <value>The privilege identifier.</value>
        public int PrivilegeID { get; set; }

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
        /// Initializes a new instance of the <see cref="RolePrivilege"/> class.
        /// </summary>
        public RolePrivilege()
        {
            //empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RolePrivilege"/> class.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="privilegeId">The privilege identifier.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        public RolePrivilege(int roleId, int privilegeId, bool isDeleted)
        {
            RoleID = roleId;
            PrivilegeID = privilegeId;
            IsDeleted = isDeleted;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Class RolePrivileges.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.RolePrivilege}" />
    public class RolePrivileges : IEnumerable<RolePrivilege>
    {
        /// <summary>
        /// Gets or sets the role privileges list.
        /// </summary>
        /// <value>The role privileges list.</value>
        public IList<RolePrivilege> rolePrivilegesList { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<RolePrivilege> GetEnumerator()
        {
            return rolePrivilegesList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return rolePrivilegesList.GetEnumerator();
        }
    }
}