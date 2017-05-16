using System;

namespace Core.Models.ExamModels
{
    public class EditExamSnapShot
    {
        /// <summary>
        /// Gets or sets the exam snap shot identifier.
        /// </summary>
        /// <value>The exam snap shot identifier.</value>
        public int ExamSnapShotID { get; set; }

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
        /// Gets or sets the sample volume.
        /// </summary>
        /// <value>The sample volume.</value>
        public double SampleVolume { get; set; }

        /// <summary>
        /// Gets or sets the base line position.
        /// </summary>
        /// <value>The base line position.</value>
        public double BaseLinePos { get; set; }

        /// Gets or sets the exam information identifier.
        /// </summary>
        /// <value>The exam information identifier.</value>
        public int ExamInfoID { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool PosEnvelopeState { get; set; }

        public bool NegEnvelopeState { get; set; }
    }
}