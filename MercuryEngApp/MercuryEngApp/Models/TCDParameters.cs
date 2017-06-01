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
        public uint Power { get; set; }
        public uint Depth { get; set; }
        public uint Filter { get; set; }
        public uint SVol { get; set; }
        public ushort StartDepth { get; set; }
        public uint PRF { get; set; }

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
