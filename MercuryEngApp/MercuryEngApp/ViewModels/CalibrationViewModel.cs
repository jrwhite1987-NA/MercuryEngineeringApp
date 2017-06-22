﻿using Core.Constants;
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
        private ushort txOffset;
        private ushort txEnergy;
        private int measurement1;
        private int measurement2;
        private uint selectedPRF;
        private uint selectedSVOL;
        private int power;
        private bool isTxOffsetEnabled;
        private bool isTxEnergyEnabled;
        private bool isWriteCalEnabled;
        private string safetyTripStatus;
        private bool isMeasurement1EditEnabled;
        private bool isMeasurement2EditEnabled;
        private bool isApplyMeasurementEnabled;
        private bool isMeasurement2ClickEnabled;

        /// <summary>
        /// Constructor
        /// </summary>
        public CalibrationViewModel()
        {
            IsTxOffsetEnabled = true;
            IsTxEnergyEnabled = true;
            IsWriteCalEnabled = true;
            IsMeasurement2EditEnabled = true;
            IsMeasurement1EditEnabled = true;
            IsMeasurement2ClickEnabled = true;
            IsApplyMeasurementEnabled = true;
            PRFList = new List<uint> { Constants.VALUE_5000, Constants.VALUE_6250, Constants.VALUE_8000 };
            SVOLList = new List<uint> { Constants.VALUE_3, Constants.VALUE_6, Constants.VALUE_9 };
            SelectedPRF = Constants.VALUE_8000;
            SelectedSVOL = Constants.VALUE_9;
            Power = Constants.SAFETY_CAL_START_POWER;
        }

        /// <summary>
        /// Gets or sets the if the Measurement 2 Button is Enabled
        /// </summary>
        public bool IsMeasurement2ClickEnabled
        {
            get
            {
                return isMeasurement2ClickEnabled;
            }
            set
            {
                isMeasurement2ClickEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Measuremnet Enabled
        /// </summary>
        public bool IsApplyMeasurementEnabled
        {
            get
            {
                return isApplyMeasurementEnabled;
            }
            set
            {
                isApplyMeasurementEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Measurement 2 Edit Enabled
        /// </summary>
        public bool IsMeasurement2EditEnabled
        {
            get
            {
                return isMeasurement2EditEnabled;
            }
            set
            {
                isMeasurement2EditEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Measurement 1 Edit Enabeld
        /// </summary>
        public bool IsMeasurement1EditEnabled
        {
            get
            {
                return isMeasurement1EditEnabled;
            }
            set
            {
                isMeasurement1EditEnabled = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Safety Strip Status
        /// </summary>
        public string SafetyTripStatus
        {
            get 
            {
                return safetyTripStatus; 
            }
            set
            {
                if (safetyTripStatus != value)
                {
                    safetyTripStatus = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Is Write Call Enabled
        /// </summary>
        public bool IsWriteCalEnabled
        {
            get
            {
                return isWriteCalEnabled;
            }
            set
            {
                if (isWriteCalEnabled != value)
                {
                    isWriteCalEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Is Tx Energy Enabled
        /// </summary>
        public bool IsTxEnergyEnabled
        {
            get
            {
                return isTxEnergyEnabled;
            }
            set
            {
                if (isTxEnergyEnabled != value)
                {
                    isTxEnergyEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Is Tx Offset Enabled
        /// </summary>
        public bool IsTxOffsetEnabled
        {
            get
            {
                return isTxOffsetEnabled;
            }
            set
            {
                if (isTxOffsetEnabled != value)
                {
                    isTxOffsetEnabled = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Power
        /// </summary>
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
                    if (value > Constants.SAFETY_CALIBRATION_MAX_POWER)
                    {
                        power = Constants.SAFETY_CAL_START_POWER;
                    }
                    else
                    {
                        power = value;
                    }
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Selected SVol
        /// </summary>
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

        /// <summary>
        /// Gets or sets the Selected PRF
        /// </summary>
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

        /// <summary>
        /// Gets or sets the PRF List
        /// </summary>
        public List<uint> PRFList { get; internal set; }

        /// <summary>
        /// Gets or sets the S Volume List
        /// </summary>
        public List<uint> SVOLList { get; internal set; }

        /// <summary>
        /// Gets or sets the TxOffset
        /// </summary>
        public ushort TxOffset
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

        /// <summary>
        /// Gets or sets the TxEnergy
        /// </summary>
        public ushort TxEnergy
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

        /// <summary>
        /// Gets or sets the Measurement1
        /// </summary>
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
                    IsMeasurement2EditEnabled = true;
                    IsMeasurement2ClickEnabled = true;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Measurement2
        /// </summary>
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
                    isApplyMeasurementEnabled = true;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
