using Core.Common;
using Core.Constants;
using System;

namespace UsbTcdLibrary.StatusClasses
{
    /// <summary>
    /// Class ModuleInfo.
    /// </summary>
    public class ModuleInfo
    {
        /// <summary>
        /// The model string
        /// </summary>
        public string modelString;

        /// <summary>
        /// The part number string
        /// </summary>
        public string partNumberString;

        /// <summary>
        /// The serial number string
        /// </summary>
        public string serialNumberString;

        /// <summary>
        /// The hardware revision string
        /// </summary>
        public string hardwareRevisionString;

        /// <summary>
        /// The boot sw revision string
        /// </summary>
        public string bootSWRevisionString;

        /// <summary>
        /// The doppler sw revision string
        /// </summary>
        public string dopplerSWRevisionString;

        public ModuleInfo()
        {
            modelString = MessageConstants.NotAvailable;
            partNumberString = MessageConstants.NotAvailable;
            serialNumberString = MessageConstants.NotAvailable;
            hardwareRevisionString = MessageConstants.NotAvailable;
            bootSWRevisionString = MessageConstants.NotAvailable;
            dopplerSWRevisionString = MessageConstants.NotAvailable;
        }

        /// <summary>
        /// Converts the arrayto information.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>ModuleInfo.</returns>
        internal static ModuleInfo ConvertArraytoInfo(byte[] data)
        {
            const int INDEX_0 = 0;
            const int COUNT_20 = 20;
            const int COUNT_8 = 8;
            const int COUNT_40 = 40;
            const int INDEX_40 = 40;
            const int INDEX_60 = 60;
            const int INDEX_80 = 80;
            const int INDEX_100 = 100;
            const int INDEX_120 = 120;
            try
            {
                Helper.logger.Debug("ConvertArraytoInfo begins for data:" + (data != null ? data.Length.ToString() : "Zero"));
                if (data.Length == DMIProtocol.MODULE_INFO_REQUEST_LENGTH)
                {
                    ModuleInfo moduleInfoObj = new ModuleInfo();
                    moduleInfoObj.modelString = System.Text.Encoding.UTF8.GetString(data, INDEX_0, COUNT_40);

                    moduleInfoObj.modelString = moduleInfoObj.modelString.Substring(0, moduleInfoObj.modelString.IndexOf('\0'));

                    moduleInfoObj.partNumberString = System.Text.Encoding.UTF8.GetString(data, INDEX_40, COUNT_20);
                    moduleInfoObj.partNumberString = moduleInfoObj.partNumberString.Substring(0, moduleInfoObj.partNumberString.IndexOf('\0'));

                    moduleInfoObj.serialNumberString = System.Text.Encoding.UTF8.GetString(data, INDEX_60, COUNT_20);
                    moduleInfoObj.serialNumberString = moduleInfoObj.serialNumberString.Substring(0, moduleInfoObj.serialNumberString.IndexOf('\0'));

                    moduleInfoObj.hardwareRevisionString = System.Text.Encoding.UTF8.GetString(data, INDEX_80, COUNT_20);
                    moduleInfoObj.hardwareRevisionString = moduleInfoObj.hardwareRevisionString.Substring(0, moduleInfoObj.hardwareRevisionString.IndexOf('\0'));

                    moduleInfoObj.bootSWRevisionString = System.Text.Encoding.UTF8.GetString(data, INDEX_100, COUNT_20);
                    moduleInfoObj.bootSWRevisionString = moduleInfoObj.bootSWRevisionString.Substring(0, moduleInfoObj.bootSWRevisionString.IndexOf('\0'));

                    moduleInfoObj.dopplerSWRevisionString = Helper.ConvertBytesToString(data, INDEX_120, COUNT_8);

                    DMIProtocol.CurrentFirmwareVersion = moduleInfoObj.dopplerSWRevisionString;
                    return moduleInfoObj;
                }
                else
                {
                    Helper.logger.Debug("ConvertArraytoInfo ends");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: " + ex);
                return null;
            }
        }
    }
}