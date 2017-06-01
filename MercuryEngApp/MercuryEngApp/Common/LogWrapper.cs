using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.Common
{
    public static class LogWrapper
    {       
        static ILog AppLogger = LogManager.GetLogger("AppLogAppender");
        static ILog TCDLogger = LogManager.GetLogger("TCDLogAppender");

        public static void Log(string type, string message)
        {
            if (type == "APP")
            {
                App.ApplicationLog += message;
                AppLogger.Info(message);
            }
            else
            {
                App.TCDLog += message;
                TCDLogger.Info(message);
            }
        }

    }
}
