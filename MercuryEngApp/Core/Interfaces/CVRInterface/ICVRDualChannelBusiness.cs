// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-13-2016
// ***********************************************************************
// <copyright file="ICVRDualChannelBusiness.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.CVRModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CVRInterface
{
    /// <summary>
    /// Interface ICVRDualChannelBusiness
    /// </summary>
    public interface ICVRDualChannelBusiness
    {
        /// <summary>
        /// Updating Manual CVR data.
        /// </summary>
        /// <param name="cvr">The CVR.</param>
        /// <returns>bool</returns>
        bool UpdateCVR(CVR cvr);

        /// <summary>
        /// CVR and BHI computation
        /// </summary>
        /// <param name="high">The high.</param>
        /// <param name="low">The low.</param>
        /// <param name="mean">The mean.</param>
        /// <returns>CalculatedCVRValues.</returns>
        CalculatedCVRValues ComputeCVRAndBHI(double high, double low, double mean);

        /// <summary>
        /// Save CVR At Exam exit
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>CVR</returns>
        Task<bool> SaveCVR(int examId, int channelId);

        /// <summary>
        /// Gets the CVR graph.
        /// </summary>
        /// <param name="examInfoId">The exam information identifier.</param>
        /// <param name="ChannelId">The channel identifier.</param>
        /// <returns>Task&lt;List&lt;System.Int16&gt;&gt;.</returns>
        Task<CVRGraphData> GetCVRGraph(int examInfoId, int ChannelId);

        /// <summary>
        /// Gets the CVR data.
        /// </summary>
        /// <param name="examInfoId">The exam information identifier.</param>
        /// <param name="ChannelId">The channel identifier.</param>
        /// <param name="flag">if set to <c>true</c> [flag].</param>
        /// <returns>CVR.</returns>
        CVR GetCVRData(int examInfoId, int ChannelId, bool flag);

        /// <summary>
        /// Check if the CVR is seen
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>bool</returns>
        bool CheckIsCVRSeen(int examId);

        /// <summary>
        /// Set CVR seen
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>bool</returns>
        bool SetCVRSeen(int examId);

        /// <summary>
        /// Save CVR of current channel
        /// </summary>
        /// <param name="calculatedCVR">The calculated CVR.</param>
        /// <returns>bool</returns>
        bool SaveCurrentChannelCVR(CalculatedCVR calculatedCVR);

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
        /// Check if CVR exist for the exam;
        /// </summary>
        /// <param name="examInfoId">The exam information identifier.</param>
        /// <returns><c>true</c> if [is CVR exist] [the specified exam information identifier]; otherwise, <c>false</c>.</returns>
        bool IsCVRExist(int examInfoId);
    }
}