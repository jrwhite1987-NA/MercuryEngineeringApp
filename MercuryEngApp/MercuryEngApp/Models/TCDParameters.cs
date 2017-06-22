using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.Models
{
    internal class TCDParameters
    {
        /// <summary>
        /// Gets or sets the Power
        /// </summary>
        public uint Power { get; set; }

        /// <summary>
        /// Gets or sets the Depth
        /// </summary>
        public uint Depth { get; set; }

        /// <summary>
        /// Gets or sets the Filter
        /// </summary>
        public uint Filter { get; set; }

        /// <summary>
        /// Gets or sets the S Volume
        /// </summary>
        public uint SVol { get; set; }

        /// <summary>
        /// Gets or sets the Start Depth
        /// </summary>
        public ushort StartDepth { get; set; }

        /// <summary>
        /// Gets or sets the PRF
        /// </summary>
        public uint PRF { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public TCDParameters()
        {
            Power = Constants.defaultPower;
            Depth = Constants.defaultDepth;
            Filter = Constants.defaultFilter;
            SVol = Constants.defaultLength;
            PRF = Constants.defaultPRF;
        }
    }
}
