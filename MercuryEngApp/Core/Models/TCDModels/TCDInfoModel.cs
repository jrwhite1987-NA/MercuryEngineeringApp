// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="TCDInfoModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models.TCDModels
{
    /// <summary>
    /// Class TCDInfo.
    /// </summary>
    public class TCDInfo
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the TCD information identifier.
        /// </summary>
        /// <value>The TCD information identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int TCDInfoID { get; set; }

        /// <summary>
        /// Gets or sets the type of the channel.
        /// </summary>
        /// <value>The type of the channel.</value>
        public int ChannelType { get; set; }

        /// <summary>
        /// Gets or sets the ultrasonic power.
        /// </summary>
        /// <value>The ultrasonic power.</value>
        public float UltrasonicPower { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public float Filter { get; set; }

        /// <summary>
        /// Gets or sets the gain.
        /// </summary>
        /// <value>The gain.</value>
        public float Gain { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets the vessel.
        /// </summary>
        /// <value>The vessel.</value>
        public string Vessel { get; set; }

        /// <summary>
        /// Gets or sets the exam information identifier.
        /// </summary>
        /// <value>The exam information identifier.</value>
        public int ExamInfoID { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        #endregion "Properties"

        #region "PropertyEvents"

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

        #endregion "PropertyEvents"
    }

    /// <summary>
    /// Class TCDInfos.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.TCDModels.TCDInfo}" />
    public class TCDInfos : IEnumerable<TCDInfo>
    {
        /// <summary>
        /// Gets or sets the TCD information list.
        /// </summary>
        /// <value>The TCD information list.</value>
        public IList<TCDInfo> TCDInfoList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TCDInfos"/> class.
        /// </summary>
        public TCDInfos()
        {
            TCDInfoList = new List<TCDInfo>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TCDInfo> GetEnumerator()
        {
            return TCDInfoList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return TCDInfoList.GetEnumerator();
        }
    }
}