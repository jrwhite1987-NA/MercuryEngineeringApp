using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;
using MercuryEngApp.Models;

namespace MercuryEngApp
{
    internal class ExamViewModel : ObservableObject
    {
        private ushort tIC;

        public OutputMetrics OutputMetrics { get; set; }
        public TCDParameters InputParams { get; set; }
        public TCDParameters PacketData { get; set; } 

        public ExamViewModel()
        {
            OutputMetrics = new OutputMetrics();
            InputParams = new TCDParameters(999);
            PacketData = new TCDParameters();
        }

        public ushort TIC
        {
            get
            {
                return tIC;
            }
            set
            {
                if (tIC != value)
                {
                    tIC = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint Power
        {
            get
            {
                return InputParams.Power;
            }
            set
            {
                if (InputParams.Power != value)
                {
                    InputParams.Power = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint Depth
        {
            get
            {
                return InputParams.Depth;
            }
            set
            {
                if (InputParams.Depth != value)
                {
                    InputParams.Depth = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint Filter
        {
            get
            {
                return InputParams.Filter;
            }
            set
            {
                if (InputParams.Filter != value)
                {
                    InputParams.Filter = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint SVol
        {
            get
            {
                return InputParams.SVol;
            }
            set
            {
                if (InputParams.SVol != value)
                {
                    InputParams.SVol = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort StartDepth
        {
            get
            {
                return InputParams.StartDepth;
            }
            set
            {
                if (InputParams.StartDepth != value)
                {
                    InputParams.StartDepth = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<uint> PRFList { get; internal set; }
        public uint SelectedPRF
        {
            get
            {
                return InputParams.PRF;
            }
            set
            {
                if(InputParams.PRF!=value)
                {
                    InputParams.PRF = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint PacketPower
        {
            get
            {
                return PacketData.Power;
            }
            set
            {
                if (PacketData.Power != value)
                {
                    PacketData.Power = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint PacketDepth
        {
            get
            {
                return PacketData.Depth;
            }
            set
            {
                if (PacketData.Depth != value)
                {
                    PacketData.Depth = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint PacketFilter
        {
            get
            {
                return PacketData.Filter;
            }
            set
            {
                if (PacketData.Filter != value)
                {
                    PacketData.Filter = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint PacketSVol
        {
            get
            {
                return PacketData.SVol;
            }
            set
            {
                if (PacketData.SVol != value)
                {
                    PacketData.SVol = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ushort PacketStartDepth
        {
            get
            {
                return PacketData.StartDepth;
            }
            set
            {
                if (PacketData.StartDepth != value)
                {
                    PacketData.StartDepth = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint PacketPRF
        {
            get
            {
                return PacketData.PRF;
            }
            set
            {
                if (PacketData.PRF != value)
                {
                    PacketData.PRF = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int PosMax
        {
            get { return OutputMetrics.PosMax;}
            set
            {
                if (OutputMetrics.PosMax != value)
                {
                    OutputMetrics.PosMax = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int PosMean
        {
            get { return OutputMetrics.PosMean; }
            set
            {
                if(OutputMetrics.PosMean != value)
                {
                    OutputMetrics.PosMean = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int PosMin
        {
            get { return OutputMetrics.PosMin; }
            set
            {
                if (OutputMetrics.PosMin != value)
                {
                    OutputMetrics.PosMin = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int PosPI
        {
            get { return OutputMetrics.PosPI; }
            set
            {
                if(OutputMetrics.PosPI != value)
                {
                    OutputMetrics.PosPI = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int NegMax
        {
            get { return OutputMetrics.NegMax; }
            set
            {
                if (OutputMetrics.NegMax != value)
                {
                    OutputMetrics.NegMax = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int NegMean
        {
            get { return OutputMetrics.NegMean; }
            set
            {
                if (OutputMetrics.NegMean != value)
                {
                    OutputMetrics.NegMean = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int NegMin
        {
            get { return OutputMetrics.NegMin; }
            set
            {
                if (OutputMetrics.NegMin != value)
                {
                    OutputMetrics.NegMin = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int NegPI
        {
            get { return OutputMetrics.NegPI; }
            set
            {
                if (OutputMetrics.NegPI != value)
                {
                    OutputMetrics.NegPI = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
