// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-12-2016
// ***********************************************************************
// <copyright file="IExamRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models.ExamModels;
using Core.Models.RangeIndicatorModels;

namespace Core.Interfaces.ExamInterface
{
    /// <summary>
    /// Interface IExamRepository
    /// </summary>
    public interface IExamRepository
    {
        /// <summary>
        /// Get All Vessels By Exam Procedure Id.
        /// </summary>
        /// <param name="examProcId">The exam proc identifier.</param>
        /// <returns>ExamProcedureSettings</returns>
        ExamProcedureSettings GetAllVesselsByExamProcedureId(int examProcId);

        /// <summary>
        /// Get Exam Vessel By Age Procedure Id.
        /// </summary>
        /// <param name="examProcId">The exam proc identifier.</param>
        /// <param name="age">The age.</param>
        /// <param name="vessel">The vessel.</param>
        /// <returns>ExamVessel</returns>
        NormalRangeIndicator GetExamVesselByAgeProcedureId(int examProcId, int age, string vessel);

        /// <summary>
        /// Save Exam.
        /// </summary>
        /// <param name="newExamInfo">The new exam information.</param>
        /// <returns>Exam.</returns>
        Exam SaveExam(Exam newExamInfo);

        /// <summary>
        /// Create an entry of the snapshot in the database.
        /// </summary>
        /// <param name="examSnapShot">The exam snap shot.</param>
        /// <returns>Identifier for whether the snapshot was created or not.</returns>
        bool CreateSnapShot(ExamSnapShot examSnapShot);

        /// <summary>
        /// Get All SnapShots By Exam ID.
        /// </summary>
        /// <param name="examInfoID">The exam information identifier.</param>
        /// <returns>ExamSnapShots</returns>
        ExamSnapShots GetAllSnapShotsByExamID(int examInfoID);

        /// <summary>
        /// Gets the snap shot by identifier.
        /// </summary>
        /// <param name="snapShotID">The snap shot identifier.</param>
        /// <param name="examInfoID">The exam information identifier.</param>
        /// <returns></returns>
        ExamSnapShot GetSnapShotByID(int snapShotID, int examInfoId);

        /// <summary>
        /// Save data into edit snapshot table.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool SaveEditedSnapshot(EditExamSnapShot model);

        /// <summary>
        /// Get edited exam snapshot
        /// </summary>
        /// <param name="examInfoId"></param>
        /// <param name="examSnapShotID"></param>
        /// <returns></returns>
        EditExamSnapShot GetEditedSnapShot(int examInfoId, int examSnapShotID);

        /// <summary>
        /// Get all edited snapshot
        /// </summary>
        /// <param name="examInfoId"></param>
        /// <returns></returns>
        System.Collections.Generic.List<EditExamSnapShot> GetAllEditedSnapShot(int examInfoId);

        /// <summary>
        /// Update Comment column of ExamSnapShot table.
        /// </summary>
        /// <param name="selectedRow">The selected row.</param>
        /// <param name="comments">The comments.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateCommentsForExamShapshot(int selectedRow, string comments);

        /// <summary>
        /// Update IsSelected column of ExamSnapShot table.
        /// </summary>
        /// <param name="selectedRow">The selected row.</param>
        /// <param name="selectedImageTag">The selected image tag.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateDataForSelectedExamShapshot(int selectedRow, int selectedImageTag);

        /// <summary>
        /// Update IsSelected column of ExamSnapShot table.
        /// </summary>
        /// <param name="selectedRow">The selected row.</param>
        /// <param name="selectedImageTag">The selected image tag.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateDataForSelectedExamShapshotVideo(int selectedRow, int selectedImageTag);

        /// <summary>
        /// Update examinfo with the Notes and assessment.
        /// </summary>
        /// <param name="exam">The exam.</param>
        /// <returns>update success or fail as bool</returns>
        bool UpdateExamNotesAssessment(Exam exam);

        /// <summary>
        /// Hard Delete Exam.
        /// </summary>
        /// <param name="examInfoId">The exam information identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool HardDeleteExam(int examInfoId);

        /// <summary>
        /// Update IsSnapShot Taken.
        /// </summary>
        /// <param name="examSnapShot">The exam snap shot.</param>
        /// <returns>bool</returns>
        bool UpdateIsSnapShotTaken(ExamSnapShot examSnapShot);

        /// <summary>
        /// Update Vessel For Exam Shapshot.
        /// </summary>
        /// <param name="selectedID">The selected identifier.</param>
        /// <param name="vesselName">Name of the vessel.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateVesselForExamShapshot(int selectedID, string vesselName);

        /// <summary>
        /// Save Emboli Meaurement Data.
        /// </summary>
        /// <param name="examEmboliMeasurementInfo">The exam emboli measurement information.</param>
        /// <returns>bool</returns>
        bool SaveEmboliMeaurementData(ExamEmboliMeasurement examEmboliMeasurementInfo);

        /// <summary>
        /// Gets the exam procedure id of the exam.
        /// </summary>
        /// <param name="examInfoID">The exam information identifier.</param>
        /// <returns>System.Int32.</returns>
        int GetExamProcId(int examInfoID);

        /// <summary>
        /// Gets the emboli meaurement data.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>System.Int32.</returns>
        int GetEmboliMeaurementData(int examId);

        /// <summary>
        /// Updates the exam.
        /// </summary>
        /// <param name="currentExam">The current exam.</param>
        void UpdateExam(Exam currentExam);

        /// <summary>
        /// Delete/Update examsnapshotdata on successful creation of snapshot image
        /// </summary>
        /// <param name="examSnapshotId">The exam snapshot identifier.</param>
        /// <returns>update success or fail as bool</returns>
        bool UpdateSnapshotParameterData(ExamSnapShotTestReview examSnapshot);

        /// <summary>
        /// Save the Lindegaard Ratio for exam
        /// </summary>
        /// <param name="lRLeft"></param>
        /// <param name="lRRight"></param>
        /// <param name="examInfoId"></param>
        /// <returns>update success or fail as bool</returns>
        bool SaveLR(float lRLeft, float lRRight, int examInfoId);

        /// <summary>
        /// Save the embolicount for the exam
        /// </summary>
        /// <param name="emboliLeft"></param>
        /// <param name="emboliRight"></param>
        /// <param name="examInfoId"></param>
        /// <returns>update success or fail as bool</returns>
        bool SaveEmboliCount(int emboliLeft, int emboliRight, int examInfoId);
    }
}