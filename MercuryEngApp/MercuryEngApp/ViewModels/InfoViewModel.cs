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
    public class InfoViewModel : ObservableObject
    {
        internal BoardInfo BoardInfo { get; set; }

        internal ProbeInfo Probeinfo { get; set; }

        private byte channelNumber;

        public InfoViewModel()
        {
            BoardInfo = new BoardInfo();
            Probeinfo = new ProbeInfo();
            ProbePartNumberList = new ObservableCollection<string>();
        }

        public byte ChannelNumber
        {
            get
            {
                return channelNumber;
            }
            set
            {
                if(channelNumber!=value)
                {
                    channelNumber = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedBoardPartNumber
        {
            get
            {
                return BoardInfo.PartNumber;
            }
            set
            {
                if(BoardInfo.PartNumber!=value)
                {
                    BoardInfo.PartNumber = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedBoardModelName
        {
            get
            {
                return BoardInfo.ModelName;
            }
            set
            {
                if(BoardInfo.ModelName!=value)
                {
                    BoardInfo.ModelName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SelectedHardwareRevision
        {
            get
            {
                return BoardInfo.HardwareRevision;
            }
            set
            {
                if(BoardInfo.HardwareRevision!=value)
                {
                    BoardInfo.HardwareRevision = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string BoardSerialNumber
        {
            get
            {
                return BoardInfo.SerialNumber;
            }
            set
            {
                if(BoardInfo.SerialNumber!=value)
                {
                    BoardInfo.SerialNumber = value;
                    RaisePropertyChanged();
                }
            }
        }

        [XmlElement("PartNumber")]
        public ObservableCollection<string> ProbePartNumberList { get; internal set; }

        public string SelectedProbePartNumber
        {
            get
            {
                return Probeinfo.PartNumber;
            }
            set
            {
                if(Probeinfo.PartNumber!=value)
                {
                    Probeinfo.PartNumber = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ProbeModelName
        {
            get
            {
                return Probeinfo.ModelName;
            }
            set
            {
                if (Probeinfo.ModelName != value)
                {
                    Probeinfo.ModelName = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ProbeSerialNumber
        {
            get
            {
                return Probeinfo.SerialNumber;
            }
            set
            {
                if (Probeinfo.SerialNumber != value)
                {
                    Probeinfo.SerialNumber = value;
                    RaisePropertyChanged();
                }
            }
        }

        public byte PhysicalID
        {
            get
            {
                return Probeinfo.PhysicalID;
            }
            set
            {
                if(Probeinfo.PhysicalID!=value)
                {
                    Probeinfo.PhysicalID = value;
                    RaisePropertyChanged();
                }
            }
        }

        public byte FormatID
        {
            get
            {
                return Probeinfo.FormatID;
            }
            set
            {
                if(Probeinfo.FormatID!=value)
                {
                    Probeinfo.FormatID = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort CenterFrequency
        {
            get
            {
                return Probeinfo.CenterFrequency;
            }
            set 
            {
                if(Probeinfo.CenterFrequency!=value)
                {
                    Probeinfo.CenterFrequency = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort Diameter
        {
            get
            {
                return Probeinfo.Diameter;
            }
            set
            {
                if (Probeinfo.Diameter != value)
                {
                    Probeinfo.Diameter = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort TankFocalLength
        {
            get
            {
                return Probeinfo.TankFocalLength;
            }
            set
            {
                if(Probeinfo.TankFocalLength!=value)
                {
                    Probeinfo.TankFocalLength = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort InsertionLoss
        {
            get
            {
                return Probeinfo.InsertionLoss;
            }
            set
            {
                if(Probeinfo.InsertionLoss!=value)
                {
                    Probeinfo.InsertionLoss = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort TIC
        {
            get
            {
                return Probeinfo.TIC;
            }
            set
            {
                if(Probeinfo.TIC!=value)
                {
                    Probeinfo.TIC = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort Fractional3dbBW
        {
            get
            {
                return Probeinfo.Fractional3dbBW;
            }
            set
            {
                if(Probeinfo.Fractional3dbBW!=value)
                {
                    Probeinfo.Fractional3dbBW = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort Impedance
        {
            get
            {
                return Probeinfo.Impedance;
            }
            set
            {
                if(Probeinfo.Impedance!=value)
                {
                    Probeinfo.Impedance = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort PhaseAngle
        {
            get
            {
                return Probeinfo.PhaseAngle;
            }
            set
            {
                if(Probeinfo.PhaseAngle!=value)
                {
                    Probeinfo.PhaseAngle = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
