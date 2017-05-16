using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.Models
{
    internal class CalibrationData
    {
        public int DACValue { get; set; }

        public int AcousticEnergy { get; set; }
    }

    internal class TxCalibration
    {
        public int AcousticMeasurement1 { get; set; }

        public int AcousticMeasurement2 { get; set; }

        public bool IsUsingRefTransducer { get; set; }
    }

    internal class SafeCalibration
    {
        public string SafetyTripStatus { get; set; }

        public int Sample { get; set; }

        public int PRF { get; set; }

        public int Power { get; set; }
    }
}
