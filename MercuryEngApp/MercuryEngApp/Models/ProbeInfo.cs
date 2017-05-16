using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    internal class ProbeInfo
    {
        public string PartNumber { get; set; }

        public string ModelName { get; set; }

        public string SerialNumber { get; set; }

        public string PhysicalID { get; set; }

        public string FormatID { get; set; }

        public string CenterFrequency { get; set; }

        public string Diameter { get; set; }

        public string TankFocalLength { get; set; }

        public string InsertionLoss { get; set; }

        public string TIC { get; set; }

        public string Fractional3dbBW { get; set; }

        public string Impedance { get; set; }

        public string PhaseAngle { get; set; }
    }
}
