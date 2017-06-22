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
        private uint operatingMins;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfoViewModel()
        {
            BoardInfo = new BoardInfo();
            Probeinfo = new ProbeInfo();
            ProbePartNumberList = new List<string>();
        }

        /// <summary>
        /// OperatingMinutes Property
        /// </summary>
        public uint OperatingMinutes
        {
            get
            {
                return operatingMins;
            }
            set
            {
                if (operatingMins != value)
                {
                    operatingMins = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// ChannelNumberList Property
        /// </summary>
        public List<byte> ChannelNumberList { get; internal set; }

        /// <summary>
        /// SelectedChannelNumber Property
        /// </summary>
        public byte SelectedChannelNumber
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

        /// <summary>
        /// BoardPartNumberList Property
        /// </summary>
        public List<String> BoardPartNumberList { get; internal set; }

        /// <summary>
        /// SelectedBoardPartNumber Property
        /// </summary>
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

        /// <summary>
        /// BoardModelNameList Property
        /// </summary>
        public List<String> BoardModelNameList { get; internal set; }

        /// <summary>
        /// SelectedBoardModelName Property
        /// </summary>
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

        /// <summary>
        /// BoardHardwareRevisionList Property
        /// </summary>
        public List<String> BoardHardwareRevisionList { get; internal set; }

        /// <summary>
        /// SelectedHardwareRevision Property
        /// </summary>
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

        /// <summary>
        /// BoardSerialNumber Property
        /// </summary>
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

        /// <summary>
        /// ProbePartNumberList Property
        /// </summary>
        public List<string> ProbePartNumberList { get; internal set; }

        /// <summary>
        /// SelectedProbePartNumber Property
        /// </summary>
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

        /// <summary>
        /// ProbeModelNameList Property
        /// </summary>
        public List<string> ProbeModelNameList { get; internal set; }
        
        /// <summary>
        /// ProbeModelName Property
        /// </summary>
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

        /// <summary>
        /// ProbeSerialNumber Property
        /// </summary>
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

        /// <summary>
        /// ProbePhysicalIDList Property
        /// </summary>
        public List<byte> ProbePhysicalIDList { get; internal set; }

        /// <summary>
        /// PhysicalID Property
        /// </summary>
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

        /// <summary>
        /// ProbeFormatIDList Property
        /// </summary>
        public List<byte> ProbeFormatIDList { get; internal set; }
        
        /// <summary>
        /// FormatID Property
        /// </summary>
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

        /// <summary>
        /// CenterFrequency Property
        /// </summary>
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

        /// <summary>
        /// Diameter Property
        /// </summary>
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

        /// <summary>
        /// TankFocalLength Property
        /// </summary>
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

        /// <summary>
        /// InsertionLoss Property
        /// </summary>
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

        /// <summary>
        /// TIC Property
        /// </summary>
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

        /// <summary>
        /// Fractional3dbBW Property
        /// </summary>
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

        /// <summary>
        /// Impedance Property
        /// </summary>
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

        /// <summary>
        /// PhaseAngle Property
        /// </summary>
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
