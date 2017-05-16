// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="IResourceObjectLoader.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Core.Interfaces.CommonInterface
{
    /// <summary>
    /// Interface IResourceObjectLoader
    /// </summary>
    public interface IResourceObjectLoader
    {
        /// <summary>
        /// Get String
        /// </summary>
        /// <param name="resourceConstants">The resource constants.</param>
        /// <returns>string</returns>
        string GetString(string resourceConstants);
    }
}