﻿using MercuryEngApp.Models;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    public class InfoViewModel : ObservableObject
    {
        internal BoardInfo BoardInfo { get; set; }

        internal ProbeInfo Probeinfo { get; set; }

        private string channelNumber;

        public InfoViewModel()
        {
            BoardInfo = new BoardInfo();
            Probeinfo = new ProbeInfo();
        }

        public string ChannelNumber
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

        public string BoardPartNumber
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

        public string BoardModelName
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

        public string HardwareRevision
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

        public string ProbePartNumber
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

        public string PhysicalID
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

        public string FormatID
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

        public string CenterFrequency
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

        public string Diameter
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

        public string TankFocalLength
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

        public string InsertionLoss
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

        public string TIC
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

        public string Fractional3dbBW
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

        public string Impedance
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

        public string PhaseAngle
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