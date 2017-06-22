using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    internal class ProbeInfo
    {
        /// <summary>
        /// Gets or sets the Part Number
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Gets or sets the Model Name
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the Serial Number
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets the Physical ID
        /// </summary>
        public byte PhysicalID { get; set; }

        /// <summary>
        /// Gets or sets the Format ID
        /// </summary>
        public byte FormatID { get; set; }

        /// <summary>
        /// Gets or sets the Center Frequency
        /// </summary>
        public ushort CenterFrequency { get; set; }

        /// <summary>
        /// Gets or sets the Diameter
        /// </summary>
        public ushort Diameter { get; set; }

        /// <summary>
        /// Gets or sets the Tank Focal Length
        /// </summary>
        public ushort TankFocalLength { get; set; }

        /// <summary>
        /// Gets or sets the Insertion Loss
        /// </summary>
        public ushort InsertionLoss { get; set; }

        /// <summary>
        /// Gets or sets the TIC
        /// </summary>
        public ushort TIC { get; set; }

        /// <summary>
        /// Gets or sets the Fractional 3 db BW
        /// </summary>
        public ushort Fractional3dbBW { get; set; }

        /// <summary>
        /// Gets or sets the Impedance
        /// </summary>
        public ushort Impedance { get; set; }

        /// <summary>
        /// Gets or sets the Phase Angle
        /// </summary>
        public ushort PhaseAngle { get; set; }
    }
}
