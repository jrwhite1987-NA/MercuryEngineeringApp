// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 06-17-2016
// ***********************************************************************
// <copyright file="UserModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models
{
    /// <summary>
    /// Class User.
    /// </summary>
    public class User
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the is deleted.
        /// </summary>
        /// <value>The is deleted.</value>
        public int IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the is system admin.
        /// </summary>
        /// <value>The is system admin.</value>
        public int IsSysAdmin { get; set; }

        /// <summary>
        /// Gets or sets the is na technician.
        /// </summary>
        /// <value>The is na technician.</value>
        public int IsNaTechnician { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            IsDeleted = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="createdDate">The created date.</param>
        public User(int userId, string firstName, string lastName, DateTime createdDate)
        {
            UserID = userId;
            FirstName = firstName;
            LastName = lastName;
            CreatedDate = createdDate;
            // false on creation of new user.
            IsDeleted = 0;
        }

        #endregion Constructors

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed.
        /// </summary>
        /// <param name="property">The property.</param>
        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    /// <summary>
    /// Class Users.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.User}" />
    public class Users : IEnumerable<User>
    {
        /// <summary>
        /// Gets or sets the user list.
        /// </summary>
        /// <value>The user list.</value>
        public IList<User> UserList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Users"/> class.
        /// </summary>
        public Users()
        {
            UserList = new List<User>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<User> GetEnumerator()
        {
            return UserList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return UserList.GetEnumerator();
        }
    }
}