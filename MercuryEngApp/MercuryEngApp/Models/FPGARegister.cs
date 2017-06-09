using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    public class FPGARegister
    {
        public string Name { get; set; }

        public int NoOfBytes { get; set; }

        public string MemoryLocation { get; set; }

        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public int Value { get; set; }
    }
}
