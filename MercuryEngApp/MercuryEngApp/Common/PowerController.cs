using MicrochipController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.Common
{
    internal class PowerController
    {
        private static IController handleForController;
        /// <summary>
        /// The singleton creation lock
        /// </summary>
        private static Object singletonCreationLock = new Object();

        public static IController Instance
        {
            get
            {
                lock (singletonCreationLock)
                {
                    if (handleForController == null)
                    {
                        handleForController = new PowerMicrocontroller();
                    }
                }
                return handleForController;
            }
        }
    }
}
