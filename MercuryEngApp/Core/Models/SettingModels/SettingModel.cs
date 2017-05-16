// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 02-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="SettingModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections;
using System.Collections.Generic;

namespace Core.Models.SettingModels
{
    /// <summary>
    /// Class Setting.
    /// </summary>
    public class Setting
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [SQLite.PrimaryKey]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        #endregion "Properties"

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Setting"/> class.
        /// </summary>
        public Setting()
        {
            //Empty constructor
        }

        #endregion Constructors
    }

    /// <summary>
    /// Class Settings.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.SettingModels.Setting}" />
    public class Settings : IEnumerable<Setting>
    {
        /// <summary>
        /// Gets or sets the setting list.
        /// </summary>
        /// <value>The setting list.</value>
        public IList<Setting> SettingList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings()
        {
            SettingList = new List<Setting>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<Setting> GetEnumerator()
        {
            return SettingList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return SettingList.GetEnumerator();
        }
    }
}