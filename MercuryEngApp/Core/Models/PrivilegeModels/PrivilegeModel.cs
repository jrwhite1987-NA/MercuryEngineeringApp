// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="PrivilegeModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;

namespace Core.Models
{
    /// <summary>
    /// Class Privilege.
    /// </summary>
    public class Privilege
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the privilege identifier.
        /// </summary>
        /// <value>The privilege identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int PrivilegeID { get; set; }

        /// <summary>
        /// Gets or sets the name of the privilege.
        /// </summary>
        /// <value>The name of the privilege.</value>
        public string PrivilegeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value><c>true</c> if this instance is deleted; otherwise, <c>false</c>.</value>
        public bool IsDeleted { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Privilege"/> class.
        /// </summary>
        public Privilege()
        {
            //empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Privilege"/> class.
        /// </summary>
        /// <param name="previligeId">The previlige identifier.</param>
        /// <param name="privilegeName">Name of the privilege.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        public Privilege(int previligeId, string privilegeName, bool isDeleted)
        {
            PrivilegeID = previligeId;
            PrivilegeName = privilegeName;
            IsDeleted = isDeleted;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Class Privileges.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.Privilege}" />
    public class Privileges : IEnumerable<Privilege>
    {
        /// <summary>
        /// Gets or sets the privilege list.
        /// </summary>
        /// <value>The privilege list.</value>
        public IList<Privilege> privilegeList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Privileges"/> class.
        /// </summary>
        public Privileges()
        {
            privilegeList = new List<Privilege>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Privilege> GetEnumerator()
        {
            return privilegeList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return privilegeList.GetEnumerator();
        }
    }
}