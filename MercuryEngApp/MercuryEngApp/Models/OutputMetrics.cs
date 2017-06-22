using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    internal class OutputMetrics
    {
        /// <summary>
        /// Gets or sets the Max Positive value
        /// </summary>
        public int PosMax { get; set; }

        /// <summary>
        /// Gets or sets the Mean Positive Value
        /// </summary>
        public int PosMean { get; set; }

        /// <summary>
        /// Gets or sets the Minimum Positive Value
        /// </summary>
        public int PosMin { get; set; }

        /// <summary>
        /// Gets or sets the PI Positive Value
        /// </summary>
        public int PosPI { get; set; }

        /// <summary>
        /// Gets or sets the Max Negative Value
        /// </summary>
        public int NegMax { get; set; }

        /// <summary>
        /// Gets or sets the Mean Negative Value
        /// </summary>
        public int NegMean { get; set; }

        /// <summary>
        /// Gets or sets the Minimum Negative Value
        /// </summary>
        public int NegMin { get; set; }

        /// <summary>
        /// Gets or sets the PI Negative Value
        /// </summary>
        public int NegPI { get; set; }
    }
}
