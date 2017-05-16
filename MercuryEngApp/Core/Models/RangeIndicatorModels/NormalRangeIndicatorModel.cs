// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-12-2016
// ***********************************************************************
// <copyright file="NormalRangeIndicatorModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models.RangeIndicatorModels
{
    /// <summary>
    /// Class NormalRangeIndicator.
    /// </summary>
    public class NormalRangeIndicator
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the minimum age.
        /// </summary>
        /// <value>The minimum age.</value>
        public int MinAge { get; set; }

        /// <summary>
        /// Gets or sets the maximum age.
        /// </summary>
        /// <value>The maximum age.</value>
        public int MaxAge { get; set; }

        /// <summary>
        /// Gets or sets the vessel.
        /// </summary>
        /// <value>The vessel.</value>
        public string Vessel { get; set; }

        /// <summary>
        /// Gets or sets the mean CBFV.
        /// </summary>
        /// <value>The mean CBFV.</value>
        public float MeanCBFV { get; set; }

        /// <summary>
        /// Gets or sets the mean CBFVSD.
        /// </summary>
        /// <value>The mean CBFVSD.</value>
        public float MeanCBFVSD { get; set; }

        /// <summary>
        /// Gets or sets the index of the pulsatility.
        /// </summary>
        /// <value>The index of the pulsatility.</value>
        public float PulsatilityIndex { get; set; }

        /// <summary>
        /// Gets or sets the pulsatility index sd.
        /// </summary>
        /// <value>The pulsatility index sd.</value>
        public float PulsatilityIndexSD { get; set; }

        public int NormalFlow { get; set; }

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
    /// Class NormalRangeIndicators.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.RangeIndicatorModels.NormalRangeIndicator}" />
    public class NormalRangeIndicators : IEnumerable<NormalRangeIndicator>
    {
        /// <summary>
        /// Gets or sets the normal range indicator list.
        /// </summary>
        /// <value>The normal range indicator list.</value>
        public IList<NormalRangeIndicator> NormalRangeIndicatorList { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<NormalRangeIndicator> GetEnumerator()
        {
            return NormalRangeIndicatorList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return NormalRangeIndicatorList.GetEnumerator();
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns><c>true</c> if this instance is empty; otherwise, <c>false</c>.</returns>
        public bool IsEmpty()
        {
            if (this.NormalRangeIndicatorList.Count > 0 || this.NormalRangeIndicatorList != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public NormalRangeIndicators()
        {
            NormalRangeIndicatorList = new List<NormalRangeIndicator>();
        }
    }
}