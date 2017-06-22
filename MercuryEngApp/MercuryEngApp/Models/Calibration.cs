using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.Models
{
    internal class CalibrationData
    {
        /// <summary>
        /// Gets or sets the DAC Value
        /// </summary>
        public int DACValue { get; set; }

        /// <summary>
        /// Acoustic Energy
        /// </summary>
        public int AcousticEnergy { get; set; }
    }

    internal class TxCalibration
    {
        /// <summary>
        /// Gets or sets the Acoustic Measurement1
        /// </summary>
        public int AcousticMeasurement1 { get; set; }

        /// <summary>
        /// Gets or sets the Acoustic Measurement2
        /// </summary>
        public int AcousticMeasurement2 { get; set; }

        /// <summary>
        /// Gets or sets the Is Using Transducer
        /// </summary>
        public bool IsUsingRefTransducer { get; set; }
    }

    internal class SafeCalibration
    {
        /// <summary>
        /// Gets or sets the Safety Trip Status
        /// </summary>
        public string SafetyTripStatus { get; set; }

        /// <summary>
        /// Gets or sets the Sample
        /// </summary>
        public int Sample { get; set; }

        /// <summary>
        /// Gets or sets the PRF
        /// </summary>
        public int PRF { get; set; }

        /// <summary>
        /// Gets or sets the Power
        /// </summary>
        public int Power { get; set; }
    }
}
