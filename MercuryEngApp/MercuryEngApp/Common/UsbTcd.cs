using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbTcdLibrary;

namespace MercuryEngApp.Common
{
    /// <summary>
    /// Internal Class
    /// </summary>
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
                //Lock
                lock (singletonCreationLock)
                {
                    if (handleForTCD == null)
                    {
                        //Create new object
                        handleForTCD = new UsbTcdDll();
                    }
                }
                return handleForTCD;
            }
        }
        
    }
}
