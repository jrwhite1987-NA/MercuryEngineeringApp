﻿using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.ViewModels
{
    internal class SoftwareViewModel : ObservableObject
    {
        private string softwareVersion;
        private string bootVersion;
        private string hardwareRevision;
        private bool isPerformUpdateEnabled;
        private bool isBrowseEnabled;
        private bool isAbortEnabled;
        private uint progressPosition;
        private bool showWarningLabels;
        private string updateStatus;
        private uint bytesWritten;
        private uint bytesReceivedErased;

        public string SoftwareVersion
        {
            get
            {
                return softwareVersion;
            }
            set
            {
                if (softwareVersion != value)
                {
                    softwareVersion = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string BootVersion
        {
            get
            {
                return bootVersion;
            }
            set
            {
                if (bootVersion != value)
                {
                    bootVersion = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string HardwareRevision
        {
            get
            {
                return hardwareRevision;
            }
            set
            {
                if (hardwareRevision != value)
                {
                    hardwareRevision = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool IsPerformUpdateEnabled
        {
            get
            {
                return isPerformUpdateEnabled;
            }
            set
            {
                isPerformUpdateEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBrowseEnabled
        {
            get
            {
                return isBrowseEnabled;
            }
            set
            {
                isBrowseEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAbortEnabled
        {
            get
            {
                return isAbortEnabled;
            }
            set
            {
                isAbortEnabled = value;
                RaisePropertyChanged();
            }
        }

        public uint ProgressPosition
        {
            get
            {
                return progressPosition;
            }
            set
            {
                progressPosition = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowWarningLabels
        {
            get
            {
                return showWarningLabels;
            }
            set
            {
                showWarningLabels = value;
                RaisePropertyChanged();
            }
        }

        public string UpdateStatus
        {
            get
            {
                return updateStatus;
            }
            set
            {
                updateStatus = value;
                RaisePropertyChanged();
            }
        }

        public uint BytesWritten
        {
            get
            {
                return bytesWritten;
            }
            set
            {
                bytesWritten = value;
                RaisePropertyChanged();
            }
        }

        public uint BytesReceivedErased
        {
            get
            {
                return bytesReceivedErased;
            }
            set
            {
                bytesReceivedErased = value;
                RaisePropertyChanged();
            }
        }
    }
}
