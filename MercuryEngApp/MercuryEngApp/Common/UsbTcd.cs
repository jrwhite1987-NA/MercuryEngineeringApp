using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbTcdLibrary;

namespace MercuryEngApp.Common
{
    internal class UsbTcd
    {
        private static IUsbTcd handleForTCD;
        /// <summary>
        /// The singleton creation lock
        /// </summary>
        private static Object singletonCreationLock = new Object();


        /// <summary>
        /// Provides a singleton instance
        /// </summary>
        /// <value>The current.</value>
        public static IUsbTcd TCDObj
        {
            get
            {
                lock (singletonCreationLock)
                {
                    if (handleForTCD == null)
                    {
                        handleForTCD = new UsbTcdDll();
                    }
                }
                return handleForTCD;
            }
        }
        
    }
}
