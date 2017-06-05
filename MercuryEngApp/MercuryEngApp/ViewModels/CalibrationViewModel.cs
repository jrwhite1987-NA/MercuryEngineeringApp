using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.ViewModels
{
    internal class CalibrationViewModel : ObservableObject
    {
        private int txOffset;
        private int txEnergy;
        private int measurement1;
        private int measurement2;
        private uint selectedPRF;
        private uint selectedSVOL;
        private int power;

        public CalibrationViewModel()
        {
            PRFList = new List<uint> { 5000, 6250, 8000 };
            SVOLList = new List<uint> { 3, 6, 9 };
            SelectedPRF = 8000;
            SelectedSVOL = 9;
        }

        public int Power
        {
            get 
            { 
                return power; 
            }
            set
            {
                if (power != value)
                {
                    power = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint SelectedSVOL
        {
            get
            {
                return selectedSVOL;
            }
            set
            {
                if (selectedSVOL != value)
                {
                    selectedSVOL = value;
                    RaisePropertyChanged();
                }
            }
        }

        public uint SelectedPRF
        {
            get
            {
                return selectedPRF;
            }
            set
            {
                if (selectedPRF != value)
                {
                    selectedPRF = value;
                    RaisePropertyChanged();
                }
            }
        }

        public List<uint> PRFList { get; internal set; }
        public List<uint> SVOLList { get; internal set; }

        public int TxOffset
        {
            get 
            { 
                return txOffset; 
            }
            set
            {
                if (txOffset != value)
                {
                    txOffset = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int TxEnergy
        {
            get 
            { 
                return txEnergy; 
            }
            set
            {
                if (txEnergy != value)
                {
                    txEnergy = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Measurement1
        {
            get 
            { 
                return measurement1; 
            }
            set
            {
                if (measurement1 != value)
                {
                    measurement1 = value;
                    RaisePropertyChanged();
                }
            }
        }

        public int Measurement2
        {
            get 
            { 
                return measurement2; 
            }
            set
            {
                if (measurement2 != value)
                {
                    measurement2 = value;
                    RaisePropertyChanged();
                }
            }
        }
        
        
    }
}
