// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="UsersWithRolesModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Media.Imaging;

namespace Core.Models.UsersWithRolesModels
{
    /// <summary>
    /// Class UserWithRole.
    /// </summary>
    public class UserWithRole : INotifyPropertyChanged
    {
        #region "Properties"

        private string _firstName;
        private string _lastName;
        private string _password;

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                if (value != this._firstName)
                {
                    this._firstName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                if (value != this._lastName)
                {
                    this._lastName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        /// <value>The full name of the user.</value>
        public string UserFullName { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets the Login name.
        /// </summary>
        /// <value>The name of the role.</value>
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        /// <value>The Password.</value>
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (value != this._password)
                {
                    this._password = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The list user with role
        /// </summary>
        public List<UserDataList> ListUserWithRole { get; set; }

        /// <summary>
        /// Gets or sets the Selected Roles.
        /// </summary>
        /// <value>The role identifier.</value>
        public Roles SelectedRoles { get; set; }

        #endregion "Properties"

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWithRole"/> class.
        /// </summary>
        public UserWithRole()
        {
            //empty constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWithRole"/> class.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="roleName">Name of the role.</param>
        public UserWithRole(int userID, string firstName, string lastName, string roleName)
        {
            UserID = userID;
            FirstName = firstName;
            LastName = lastName;
            RoleName = roleName;
        }

        #endregion Constructors
    }

    /// <summary>
    /// Class UserWithRoles.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.UsersWithRolesModels.UserWithRole}" />
    public class UserWithRoles : IEnumerable<UserWithRole>
    {
        /// <summary>
        /// Gets or sets the users roles data list.
        /// </summary>
        /// <value>The users roles data list.</value>
        public IList<UserWithRole> UsersRolesDataList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserWithRoles"/> class.
        /// </summary>
        public UserWithRoles()
        {
            UsersRolesDataList = new List<UserWithRole>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<UserWithRole> GetEnumerator()
        {
            return UsersRolesDataList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return UsersRolesDataList.GetEnumerator();
        }
    }

    /// <summary>
    /// Class UserDataList.
    /// </summary>
    public class UserDataList
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
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
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        /// <value>The full name of the user.</value>
        public string UserFullName { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        public int RoleID { get; set; }

        /// <summary>
        /// Gets or sets the image file path.
        /// </summary>
        /// <value>The image file path.</value>
        public BitmapImage ImageFilePath { get; set; }
    }

    /// <summary>
    /// Class AdminWithUsername.
    /// </summary>
    public class AdminWithUsername
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
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
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }
    }
}