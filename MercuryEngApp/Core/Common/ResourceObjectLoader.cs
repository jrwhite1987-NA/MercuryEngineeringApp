// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="ResourceObjectLoader.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using Windows.ApplicationModel.Resources;

namespace Core.Common
{
    /// <summary>
    /// Class ResourceObjectLoader.
    /// </summary>
    /// <seealso cref="Core.Interfaces.CommonInterface.IResourceObjectLoader" />
    public class ResourceObjectLoader 
    {
        #region Variable Declaration

        /// <summary>
        /// The loader
        /// </summary>
        private ResourceLoader _loader = null;

        #endregion Variable Declaration

        #region Connsructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceObjectLoader"/> class.
        /// </summary>
        public ResourceObjectLoader()
        {
            _loader = new ResourceLoader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceObjectLoader"/> class.
        /// </summary>
        /// <param name="resourceLoader">The resource loader.</param>
        private ResourceObjectLoader(ResourceLoader resourceLoader)
        {
            //This should be make public while doing Unit Test Cases.
            _loader = resourceLoader;
        }

        #endregion Connsructor

        #region Methods

        /// <summary>
        /// Get String.
        /// </summary>
        /// <param name="resourceConstants">The resource constants.</param>
        /// <returns>string</returns>
        public string GetString(string resourceConstants)
        {
            return _loader.GetString(resourceConstants);
        }

        #endregion Methods
    }
}