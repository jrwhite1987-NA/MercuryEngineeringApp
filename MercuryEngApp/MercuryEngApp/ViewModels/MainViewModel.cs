using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
    /// <summary>
    /// Class Main View Model
    /// </summary>
    public  class MainViewModel : ObservableObject
    {
        private string appLog;
        public string ApplicationLog
        {
            get
            {
                return appLog;
            }
            set
            {
                appLog = value;
                RaisePropertyChanged();
            }
        }

        private string tcdLog;
        public string TCDLog
        {
            get
            {
                return tcdLog;
            }
            set
            {
                tcdLog = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// GetLoginByUserID
        /// </summary>
        /// <param name="NA_TECH_ID"></param>
        /// <returns></returns>
        internal MainWindow GetLoginByUserID(int NA_TECH_ID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// GetMainWindowByUserID
        /// </summary>
        /// <param name="NA_TECH_ID"></param>
        /// <returns></returns>
        internal MainWindow GetMainWindowByUserID(int NA_TECH_ID)
        {
            throw new NotImplementedException();
        }
    }
}
