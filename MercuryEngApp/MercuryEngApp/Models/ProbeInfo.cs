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

        public byte PhysicalID { get; set; }

        public byte FormatID { get; set; }

        public ushort CenterFrequency { get; set; }

        public ushort Diameter { get; set; }

        public ushort TankFocalLength { get; set; }

        public ushort InsertionLoss { get; set; }

        public ushort TIC { get; set; }

        public ushort Fractional3dbBW { get; set; }

        public ushort Impedance { get; set; }

        public ushort PhaseAngle { get; set; }
    }
}
