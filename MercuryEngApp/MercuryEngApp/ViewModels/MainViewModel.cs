using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp
{
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

        internal MainWindow GetLoginByUserID(int NA_TECH_ID)
        {
            throw new NotImplementedException();
        }

        internal MainWindow GetMainWindowByUserID(int NA_TECH_ID)
        {
            throw new NotImplementedException();
        }
    }
}
