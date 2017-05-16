// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-22-2016
// ***********************************************************************
// <copyright file="ExamSnapShotTestReview.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace Core.Models.ExamModels
{
    /// <summary>
    /// Class ExamSnapShotTestReview.
    /// </summary>
    public class ExamSnapShotTestReview
    {
        /// <summary>
        /// Gets or sets the exam snap shot identifier.
        /// </summary>
        /// <value>The exam snap shot identifier.</value>
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
        /// Gets or sets the exam information identifier.
        /// </summary>
        /// <value>The exam information identifier.</value>
        public int ExamInfoID { get; set; }

        /// <summary>
        /// Gets or sets the string created date time.
        /// </summary>
        /// <value>The string created date time.</value>
        public string StrCreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the image file path.
        /// </summary>
        /// <value>The image file path.</value>
        public BitmapImage ImageFilePath { get; set; }

        /// <summary>
        /// Gets or sets the image file path.
        /// </summary>
        /// <value>The image file path.</value>
        public BitmapImage PlayImageFilePath { get; set; }

        /// <summary>
        /// Gets or sets the video play button image file path.
        /// </summary>
        /// <value>The image file path.</value>
        public BitmapImage VideoPlayButtonPath { get; set; }

        /// <summary>
        /// Gets or sets the vessel desc list.
        /// </summary>
        /// <value>The vessel desc list.</value>
        public List<string> VesselDescList { get; set; }

        /// <summary>
        /// Gets or sets the vessel desc.
        /// </summary>
        /// <value>The vessel desc.</value>
        public string VesselDesc { get; set; }

        /// <summary>
        /// Gets or sets the position mean.
        /// </summary>
        /// <value>The position mean.</value>
        public string PosMean { get; set; }

        /// <summary>
        /// Gets or sets the neg mean.
        /// </summary>
        /// <value>The neg mean.</value>
        public string NegMean { get; set; }

        /// <summary>
        /// Gets or sets the position pi.
        /// </summary>
        /// <value>The position pi.</value>
        public string PosPI { get; set; }

        /// <summary>
        /// Gets or sets the neg pi.
        /// </summary>
        /// <value>The neg pi.</value>
        public string NegPI { get; set; }

        /// <summary>
        /// Gets or sets the position maximum.
        /// </summary>
        /// <value>The position maximum.</value>
        public string PosMax { get; set; }

        /// <summary>
        /// Gets or sets the neg maximum.
        /// </summary>
        /// <value>The neg maximum.</value>
        public string NegMax { get; set; }

        /// <summary>
        /// Gets or sets the position minimum.
        /// </summary>
        /// <value>The position minimum.</value>
        public string PosMin { get; set; }

        /// <summary>
        /// Gets or sets the neg minimum.
        /// </summary>
        /// <value>The neg minimum.</value>
        public string NegMin { get; set; }

        /// <summary>
        /// Gets or sets the snap serial number.
        /// </summary>
        /// <value>The snap serial number.</value>
        public int SnapSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the index of the vessel desc list selected.
        /// </summary>
        /// <value>The index of the vessel desc list selected.</value>
        public int VesselDescListSelectedIndex { get; set; }

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
        /// Gets or sets the IsSnapShotImageCreated.
        /// </summary>
        /// <value>The IsSnapShotImageCreated.</value>
        public bool IsSnapShotImageCreated { get; set; }

        /// <summary>
        /// Indicates if the positive envelope is enabled or disabled
        /// </summary>
        public bool PosEnvelopeState { get; set; }

        /// <summary>
        /// Indicates if the negative envelope is enabled or disabled
        /// </summary>
        public bool NegEnvelopeState { get; set; }

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
    }
}