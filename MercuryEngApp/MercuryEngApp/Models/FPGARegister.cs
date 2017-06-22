using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    public class FPGARegister
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the No of Bytes
        /// </summary>
        public int NoOfBytes { get; set; }

        /// <summary>
        /// Gets or sets the Memory location
        /// </summary>
        public string MemoryLocation { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Default Value
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public int Value { get; set; }
    }
}
