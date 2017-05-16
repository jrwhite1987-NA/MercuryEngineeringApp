// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="IPatientRepository.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Models;
using Core.Models.ExamModels;
using Core.Models.PatientWithExamInfoModels;
using System;

namespace Core.Interfaces.PatientInterface
{
    /// <summary>
    /// Interface IPatientRepository
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns>Patients.</returns>
        Patients GetAllPatients();

        /// <summary>
        /// Gets all patients with exam infos.
        /// </summary>
        /// <returns>PatientWithExamInfos.</returns>
        PatientWithExamInfos GetAllPatientsWithExamInfos();

        /// <summary>
        /// Gets the patient.
        /// </summary>
        /// <param name="patientID">The patient identifier.</param>
        /// <returns>Patient.</returns>
        Patient GetPatient(int patientID);

        /// <summary>
        /// Saves the patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>Patient.</returns>
        Patient SavePatient(Patient patient);

        /// <summary>
        /// Updates the patient.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdatePatient(Patient patient);

        /// <summary>
        /// Deletes the patient.
        /// </summary>
        /// <param name="patientID">The patient identifier.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DeletePatient(int patientID);

        /// <summary>
        /// Searches the patients.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <param name="dob">The dob.</param>
        /// <param name="numberOfdays">The number ofdays.</param>
        /// <returns>PatientWithExamInfos.</returns>
        PatientWithExamInfos SearchPatients(Patient patient, Nullable<DateTime> dob, Nullable<int> numberOfdays);

        /// <summary>
        /// Maximums the patient identifier.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int MaxPatientId();

        /// <summary>
        /// Determines whether [is mrnor patient identifier exist] [the specified MRN or patient identifier].
        /// </summary>
        /// <param name="mrnOrPatientId">The MRN or patient identifier.</param>
        /// <returns>Patients.</returns>
        Patients IsMRNORPatientIdExist(string mrnOrPatientId);

        /// <summary>
        /// Gets the exams.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns>Exams.</returns>
        Exams GetExams(Patient patient);

        /// <summary>
        /// Gets the patient age range.
        /// </summary>
        /// <param name="patientage">The patientage.</param>
        /// <returns>System.String.</returns>
        string GetPatientAgeRange(int patientage);
    }
}