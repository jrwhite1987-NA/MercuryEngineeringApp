// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 05-24-2016
// ***********************************************************************
// <copyright file="ILocalApplicationDataStorage.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Core.Interfaces.CommonInterface
{
    /// <summary>
    /// Interface ILocalApplicationDataStorage
    /// </summary>
    public interface ILocalApplicationDataStorage
    {
        /// <summary>
        /// Saves the asynchronous local application data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classObject">The class object.</param>
        void SaveAsyncLocalApplicationData<T>(T classObject);

        /// <summary>
        /// Reterieves the asynchronous local application data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classObject">The class object.</param>
        /// <returns>Task&lt;ObservableCollection&lt;Login&gt;&gt;.</returns>
        Task<ObservableCollection<Login>> ReterieveAsyncLocalApplicationData<T>(T classObject);
    }
}