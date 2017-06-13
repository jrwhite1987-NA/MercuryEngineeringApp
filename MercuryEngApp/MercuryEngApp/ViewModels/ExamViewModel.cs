using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;
using MercuryEngApp.Models;
using MercuryEngApp.Common;
using Core.Constants;

namespace MercuryEngApp
{
    internal class ExamViewModel : ObservableObject
    {
        private ushort tIC;
        private int sliderValue;
        private int sliderMax;
        private int currentFFT;
        public List<uint> PRFList { get; internal set; }
        public OutputMetrics OutputMetrics { get; set; }
        public TCDParameters InputParams { get; set; }
        public TCDParameters PacketData { get; set; }
        public int VelocityRange { get; set; }

        private double col1Width;

        public double Column1Width
        {
            get { return col1Width; }
            set { col1Width = value;
            RaisePropertyChanged();
            }
        }

        private double col2Width;

        public double Column2Width
        {
            get { return col2Width; }
            set { col2Width = value;
            RaisePropertyChanged();
            }
        }
        
        
        
        public List<int> FFTList { get; internal set; }

        /// <summary>
        /// The leftcurrent base line postion
        /// </summary>
        private int currentBaseLinePosition = 0;

        /// <summary>
        /// Gets or sets the left baseline postion.
        /// </summary>
        /// <value>The left baseline postion.</value>
        public int BaselinePosition
        {
            get { return currentBaseLinePosition; }
            set
            {
                currentBaseLinePosition = value - Constants.BaselineValue;
            }
        }

        public ExamViewModel()
        {
            OutputMetrics = new OutputMetrics();
            InputParams = new TCDParameters();
            PacketData = new TCDParameters();
            VelocityRange = PRFOptions.ThirdOption.DefaultVelocityRange;
            FFTList = Constants.SpectrumBinList;
            SelectedFFT = FFTList[0];
            SliderValue = Constants.BaselineValue;
            ScreenCoords = new System.Windows.Point(-Constants.VALUE_17, Constants.VALUE_107);
        }

        public int SelectedFFT
        {
            get
            {
                return currentFFT;
            }
            set
            {
                if (currentFFT != value)
                {
                    currentFFT = value;
                    RaisePropertyChanged();
                    SliderMax = value - Constants.VALUE_1;
                    Constants.BaselineValue = value / Constants.VALUE_2;
                    Constants.SpectrumBin = value;
                    SliderValue = Constants.BaselineValue;
                }
            }
        }

        public int SliderMax
        {
            get
            {
                return sliderMax;
            }
            set
            {
                if(sliderMax!=value)
                {
                    sliderMax = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int SliderValue
        {
            get
            {
                return sliderValue;
            }
            set
            {
                if(sliderValue!=value)
                {
                    sliderValue = value;
                    RaisePropertyChanged();
                    BaselinePosition = value;
                }
            }
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

        


        public System.Windows.Point ScreenCoords { get; set; }

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
