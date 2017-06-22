using Core.Constants;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbTcdLibrary.PacketFormats;

namespace MercuryEngApp.Common
{
    public static class LogWrapper
    {       
        /// <summary>
        /// Static Object for ILog for App Log
        /// </summary>
        static ILog AppLogger = LogManager.GetLogger("AppLogAppender");

        /// <summary>
        /// Static Object for ILog for TCD Log
        /// </summary>
        static ILog TCDLogger = LogManager.GetLogger("TCDLogAppender");

        /// <summary>
        /// Log the message in file based on type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void Log(string type, string message)
        {
            if (type == Constants.APPLog)
            {
                //Set log for App Log
                App.ApplicationLog = message + Environment.NewLine + App.ApplicationLog;
                AppLogger.Info(message);
            }
            else
            {
                //Set log for TCD Log
                App.TCDLog = message + Environment.NewLine + App.TCDLog;
                TCDLogger.Info(message);
            }
        }

        /// <summary>
        /// Log the ServiceLogMessage in file based on type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public static void Log(string type, ServiceLogMessage message)
        {
            if (type == Constants.APPLog)
            {
                //Set log for App Log
                App.ApplicationLog = message + Environment.NewLine + App.ApplicationLog;
                AppLogger.Info(message);
            }
            else
            {
                //Set log for TCD Log
                App.TCDLog = message.MessageCode + " " + message.MessageText + Environment.NewLine + App.TCDLog;
                TCDLogger.Info(message);
            }
        }

    }
}
