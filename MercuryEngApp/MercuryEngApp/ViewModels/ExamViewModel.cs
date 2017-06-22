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

        /// <summary>
        /// Column1Width Property
        /// </summary>
        public double Column1Width
        {
            get { return col1Width; }
            set { col1Width = value;
            RaisePropertyChanged();
            }
        }

        private double col2Width;

        /// <summary>
        /// Column2Width Property
        /// </summary>
        public double Column2Width
        {
            get { return col2Width; }
            set { col2Width = value;
            RaisePropertyChanged();
            }
        }
        
        /// <summary>
        /// FFTList Property
        /// </summary>
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

        /// <summary>
        /// Constructor
        /// </summary>
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

        /// <summary>
        /// Selected FFT Property
        /// </summary>
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

        /// <summary>
        /// Slider Max Property
        /// </summary>
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

        /// <summary>
        /// Slider Value Property
        /// </summary>
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

        /// <summary>
        /// TIC Property
        /// </summary>
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

        /// <summary>
        /// Gets or Sets the Power
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Depth
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Filter
        /// </summary>
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

        /// <summary>
        /// Gets or sets the S Volume
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Start Depth
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Selected PRF
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Packet Power
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Packet Depth
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Packet Filter
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Packet S Volume
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Packet Start Depth
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Screen Co-ordinates
        /// </summary>
        public System.Windows.Point ScreenCoords { get; set; }

        /// <summary>
        /// Gets or sets the Packet PRF
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Max Positive value
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Mean Positive value
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Minimum Positive value
        /// </summary>
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

        /// <summary>
        /// Gets or sets the PI Positive value
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Max Negative value
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Mean Negative Value
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Minimum Negative Value
        /// </summary>
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

        /// <summary>
        /// Gets or sets the PI Negative Value
        /// </summary>
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
