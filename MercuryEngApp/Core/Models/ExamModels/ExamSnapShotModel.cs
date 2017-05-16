// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="ExamSnapShotModel.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class ExamSnapShot.
    /// </summary>
    public class ExamSnapShot
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the exam snap shot identifier.
        /// </summary>
        /// <value>The exam snap shot identifier.</value>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int ExamSnapShotID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsVideoSelected { get; set; }

        /// <summary>
        /// Gets or sets the indication.
        /// </summary>
        /// <value>The indication.</value>
        public string Indication { get; set; }

        /// <summary>
        /// Gets or sets the type of the channel.
        /// </summary>
        /// <value>The type of the channel.</value>
        public int ChannelType { get; set; }

        /// <summary>
        /// Gets or sets the power.
        /// </summary>
        /// <value>The power.</value>
        public float Power { get; set; }

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

        public int CurrentVelocityRange { get; set; }

        /// <summary>
        /// Gets or sets the position mean.
        /// </summary>
        /// <value>The position mean.</value>
        public float PosMean { get; set; }

        /// <summary>
        /// Gets or sets the neg mean.
        /// </summary>
        /// <value>The neg mean.</value>
        public float NegMean { get; set; }

        /// <summary>
        /// Gets or sets the position pi.
        /// </summary>
        /// <value>The position pi.</value>
        public float PosPI { get; set; }

        /// <summary>
        /// Gets or sets the neg pi.
        /// </summary>
        /// <value>The neg pi.</value>
        public float NegPI { get; set; }

        /// <summary>
        /// Gets or sets the position maximum.
        /// </summary>
        /// <value>The position maximum.</value>
        public float PosMax { get; set; }

        /// <summary>
        /// Gets or sets the neg maximum.
        /// </summary>
        /// <value>The neg maximum.</value>
        public float NegMax { get; set; }

        /// <summary>
        /// Gets or sets the position minimum.
        /// </summary>
        /// <value>The position minimum.</value>
        public float PosMin { get; set; }

        /// <summary>
        /// Gets or sets the neg minimum.
        /// </summary>
        /// <value>The neg minimum.</value>
        public float NegMin { get; set; }

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

        /// <summary>
        /// Gets or sets the base line position.
        /// </summary>
        /// <value>The base line position.</value>
        public double BaseLinePos { get; set; }

        /// <summary>
        /// Gets or sets the sample volume.
        /// </summary>
        /// <value>The sample volume.</value>
        public double SampleVolume { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is snap shot image created.
        /// </summary>
        /// <value><c>true</c> if this instance is snap shot image created; otherwise, <c>false</c>.</value>
        public bool IsSnapShotImageCreated { get; set; }

        /// <summary>
        /// Gets or sets the offset bytes
        /// </summary>
        public int OffsetBytes { get; set; }

        /// <summary>
        /// Indicates if the positive envelope is enabled or disabled
        /// </summary>
        public bool PosEnvelopeState { get; set; }

        /// <summary>
        /// Indicates if the negative envelope is enabled or disabled
        /// </summary>
        public bool NegEnvelopeState { get; set; }

        /// <summary>
        /// Frame rate
        /// </summary>
        public int FrameSpeed { get; set; }

        /// <summary>
        /// Scale x Axis
        /// </summary>
        public int ScaleXPoint { get; set; }

        /// <summary>
        /// Scale Y Axis
        /// </summary>
        public int ScaleYPoint { get; set; }

        /// <summary>
        /// The Custom slider value on Thum move
        /// </summary>
        public double CustomSliderValue { get; set; }

        /// <summary>
        /// Flag to check snapshot Type
        /// </summary>
        public bool IsManualTypeSnap { get; set; }

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
    /// Class ExamSnapShots.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{Core.Models.ExamModels.ExamSnapShot}" />
    public class ExamSnapShots : IEnumerable<ExamSnapShot>
    {
        /// <summary>
        /// Gets or sets the exam snap shot list.
        /// </summary>
        /// <value>The exam snap shot list.</value>
        public IList<ExamSnapShot> ExamSnapShotList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamSnapShots"/> class.
        /// </summary>
        public ExamSnapShots()
        {
            ExamSnapShotList = new List<ExamSnapShot>();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<ExamSnapShot> GetEnumerator()
        {
            return ExamSnapShotList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ExamSnapShotList.GetEnumerator();
        }
    }
}