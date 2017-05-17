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
        public string TextOutput { get; set; }

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
