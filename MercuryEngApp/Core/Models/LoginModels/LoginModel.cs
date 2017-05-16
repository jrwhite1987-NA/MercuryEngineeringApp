// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-22-2016
// ***********************************************************************
// <copyright file="LoginModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace Core.Models
{
    /// <summary>
    /// Class Login.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the salt key.
        /// </summary>
        /// <value>The salt key.</value>
        public string SaltKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hash password.
        /// </summary>
        /// <value>The hash password.</value>
        public string HashPassword
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last login.
        /// </summary>
        /// <value>The last login.</value>
        public DateTime LastLogin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is first login.
        /// </summary>
        /// <value><c>true</c> if this instance is first login; otherwise, <c>false</c>.</value>
        public bool IsFirstLogin
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Class Logins.
    /// </summary>
    public class Logins
    {
        /// <summary>
        /// Gets or sets the login list.
        /// </summary>
        /// <value>The login list.</value>
        public IList<Login> LoginList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logins"/> class.
        /// </summary>
        public Logins()
        {
            LoginList = new List<Login>();
        }
    }
}