using MercuryEngApp.Models;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MercuryEngApp
{
    class PacketViewModel : ObservableObject
    {
        private int silderValue;
        private int packetNumber;

        public int SilderValue
        {
            get
            {
                return silderValue;
            }
            set
            {
                if (silderValue != value)
                {
                    silderValue = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int PacketNumber
        {
            get
            {
                return packetNumber;
            }
            set
            {
                if (packetNumber != value)
                {
                    packetNumber = value;
                    RaisePropertyChanged();
                }
            }
        }

    }
}
