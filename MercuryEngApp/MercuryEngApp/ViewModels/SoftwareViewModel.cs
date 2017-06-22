using MicroMvvm;
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

        /// <summary>
        /// Gets of sets the Software Version
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Boot Version
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Hardware revision
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Perform Upadate Enabled
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Is Browse Enabled
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Is the Abort Enabled
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Progress Position
        /// </summary>
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

        /// <summary>
        /// Gets of sets the show warning labels
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Update Status
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Bytes Written
        /// </summary>
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

        /// <summary>
        /// Gets of sets the Bytes Receivbed
        /// </summary>
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
