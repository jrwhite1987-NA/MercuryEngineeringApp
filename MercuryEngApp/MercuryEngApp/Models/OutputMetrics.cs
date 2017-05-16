using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    internal class OutputMetrics
    {
        public int PosMax { get; set; }
        public int PosMean { get; set; }
        public int PosMin { get; set; }
        public int PosPI { get; set; }

        public int NegMax { get; set; }
        public int NegMean { get; set; }
        public int NegMin { get; set; }
        public int NegPI { get; set; }
    }
}
