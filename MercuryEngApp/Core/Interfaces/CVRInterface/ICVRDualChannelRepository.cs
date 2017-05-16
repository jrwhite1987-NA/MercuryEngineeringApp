// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-13-2016
// ***********************************************************************
// <copyright file="ICVRDualChannelRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.CVRModels;
using System.Collections.Generic;

namespace Core.Interfaces.CVRInterface
{
    /// <summary>
    /// Interface ICVRDualChannelRepository
    /// </summary>
    public interface ICVRDualChannelRepository
    {
        /// <summary>
        /// Updating Manual CVR data.
        /// </summary>
        /// <param name="cvr">The CVR.</param>
        /// <returns>bool</returns>
        bool UpdateCVR(CVR cvr);

        /// <summary>
        /// Saves the CVR.
        /// </summary>
        /// <param name="newCVR">The new CVR.</param>
        /// <returns>CVR.</returns>
        CVR SaveCVR(CVR newCVR);

        //Get max cvrId
        /// <summary>
        /// Gets the maximum CVR identifier.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int GetMaxCVRId();

        /// <summary>
        /// Gets the CVR data.
        /// </summary>
        /// <param name="examInfoId">The exam information identifier.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="flag">if set to <c>true</c> [flag].</param>
        /// <returns>CVR.</returns>
        CVR GetCVRData(int examInfoId, int channelId, bool flag);

        /// <summary>
        /// Set CVR seen
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>bool</returns>
        bool SetCVRSeen(int examId);

        /// <summary>
        /// GetCVRForExam - get cvr rows of exam for both manual/auto cvr for both channels
        /// </summary>
        /// <param name="examInfoId">The exam information identifier.</param>
        /// <returns>CVRForExam</returns>
        CVRForExam GetCVRForExam(int examInfoId);

        /// <summary>
        /// SaveCVRValuesForExam - Save/Update manual CVR values for both channels for exam
        /// </summary>
        /// <param name="cvrOfExam">The CVR of exam.</param>
        /// <returns>CVRForExam</returns>
        bool SaveCVRValuesForExam(CVRForExam cvrOfExam);

        /// <summary>
        /// Fetch CVR if exists for an exam
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>CVR</returns>
        List<CVR> GetCVRForExamId(int examId);
    }
}