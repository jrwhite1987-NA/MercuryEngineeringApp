// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="TCDDataModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models.TCDModels
{
    /// <summary>
    /// Class TCDData.
    /// </summary>
    public class TCDData
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the TCD data identifier.
        /// </summary>
        /// <value>The TCD data identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int TCDDataId { get; set; }

        /// <summary>
        /// Gets or sets the TCD information identifier.
        /// </summary>
        /// <value>The TCD information identifier.</value>
        public int TCDInfoId { get; set; }

        /// <summary>
        /// Gets or sets the index of the pulsatility.
        /// </summary>
        /// <value>The index of the pulsatility.</value>
        public string PulsatilityIndex { get; set; }

        /// <summary>
        /// Gets or sets the spectrogram.
        /// </summary>
        /// <value>The spectrogram.</value>
        public string Spectrogram { get; set; }

        /// <summary>
        /// Gets or sets the velocity envelope.
        /// </summary>
        /// <value>The velocity envelope.</value>
        public string VelocityEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the motion mode.
        /// </summary>
        /// <value>The motion mode.</value>
        public string MotionMode { get; set; }

        /// <summary>
        /// Gets or sets the modified motion mode.
        /// </summary>
        /// <value>The modified motion mode.</value>
        public string ModifiedMotionMode { get; set; }

        /// <summary>
        /// Gets or sets the CVR.
        /// </summary>
        /// <value>The CVR.</value>
        public string CVR { get; set; }

        /// <summary>
        /// Gets or sets the peak velocity.
        /// </summary>
        /// <value>The peak velocity.</value>
        public string PeakVelocity { get; set; }

        /// <summary>
        /// Gets or sets the mean velocity.
        /// </summary>
        /// <value>The mean velocity.</value>
        public string MeanVelocity { get; set; }

        /// <summary>
        /// Gets or sets the range for mean velocity.
        /// </summary>
        /// <value>The range for mean velocity.</value>
        public string RangeForMeanVelocity { get; set; }

        /// <summary>
        /// Gets or sets the diastolic.
        /// </summary>
        /// <value>The diastolic.</value>
        public string Diastolic { get; set; }

        /// <summary>
        /// Gets or sets the lindegaard ratio.
        /// </summary>
        /// <value>The lindegaard ratio.</value>
        public string LindegaardRatio { get; set; }

        /// <summary>
        /// Gets or sets the emboli count.
        /// </summary>
        /// <value>The emboli count.</value>
        public string EmboliCount { get; set; }

        /// <summary>
        /// Gets or sets the emboli size estimation.
        /// </summary>
        /// <value>The emboli size estimation.</value>
        public string EmboliSizeEstimation { get; set; }

        /// <summary>
        /// Gets or sets the emc.
        /// </summary>
        /// <value>The emc.</value>
        public string EMC { get; set; }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public string Volume { get; set; }

        /// <summary>
        /// Gets or sets the tic.
        /// </summary>
        /// <value>The tic.</value>
        public string TIC { get; set; }

        /// <summary>
        /// Gets or sets the PRF.
        /// </summary>
        /// <value>The PRF.</value>
        public string PRF { get; set; }

        /// <summary>
        /// Gets or sets the index of the thermal.
        /// </summary>
        /// <value>The index of the thermal.</value>
        public string ThermalIndex { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public string CreatedDate { get; set; }

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
    /// Class TCDDatas.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.TCDModels.TCDData}" />
    public class TCDDatas : IEnumerable<TCDData>
    {
        /// <summary>
        /// Gets or sets the TCD data list.
        /// </summary>
        /// <value>The TCD data list.</value>
        public IList<TCDData> TCDDataList { get; set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TCDData> GetEnumerator()
        {
            return TCDDataList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return TCDDataList.GetEnumerator();
        }
    }
}