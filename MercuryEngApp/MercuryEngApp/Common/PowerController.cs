using MicrochipController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.Common
{
    /// <summary>
    /// Internal Class
    /// </summary>
    internal class PowerController
    {
        /// <summary>
        /// STatic ICOntroller
        /// </summary>
        private static IController handleForController;
        /// <summary>
        /// The singleton creation lock
        /// </summary>
        private static Object singletonCreationLock = new Object();

        /// <summary>
        /// Static Object
        /// for Instance
        /// </summary>
        public static IController Instance
        {
            get
            {
                //sets Lock
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
