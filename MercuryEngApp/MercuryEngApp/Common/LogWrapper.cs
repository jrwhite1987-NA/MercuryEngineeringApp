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
        static ILog AppLogger = LogManager.GetLogger("AppLogAppender");
        static ILog TCDLogger = LogManager.GetLogger("TCDLogAppender");

        public static void Log(string type, string message)
        {
            if (type == Constants.APPLog)
            {
                App.ApplicationLog = message + Environment.NewLine + App.ApplicationLog;
                AppLogger.Info(message);
            }
            else
            {
                App.TCDLog = message + Environment.NewLine + App.TCDLog;
                TCDLogger.Info(message);
            }
        }

        public static void Log(string type, ServiceLogMessage message)
        {
            if (type == Constants.APPLog)
            {
                App.ApplicationLog = message + Environment.NewLine + App.ApplicationLog;
                AppLogger.Info(message);
            }
            else
            {
                App.TCDLog = message.MessageCode + " " + message.MessageText + Environment.NewLine + App.TCDLog;
                TCDLogger.Info(message);
            }
        }

    }
}
