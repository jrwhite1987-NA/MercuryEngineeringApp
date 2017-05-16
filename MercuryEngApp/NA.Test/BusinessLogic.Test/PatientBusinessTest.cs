// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="PatientBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.PatientBAL;
using Core.Interfaces.PatientInterface;
using Core.Models;
using Core.Models.ExamModels;
using Core.Models.PatientWithExamInfoModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class PatientBusinessTest.
    /// </summary>
    [TestClass]
    public class PatientBusinessTest
    {
        /// <summary>
        /// Checks the get all patients successful.
        /// </summary>
        [TestMethod]
        public void CheckGetAllPatientsSuccessful()
        {
            //Create Mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient1 = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",

                IsDeleted = 0
            };
            Patient patient2 = new Patient
            {
                PatientID = 2,
                FirstName = "Anna",
                LastName = "Taylor",
                Gender = "F",
                DOB = Convert.ToDateTime("10/03/1993"),
                IdentificationNumber = "121",

                IsDeleted = 0
            };

            Patients patients = new Patients();
            patients.PatientList.Add(patient1);
            patients.PatientList.Add(patient2);

            mockPatientRepository.Setup(x => x.GetAllPatients()).Returns(patients);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetAllPatients();

            //Assert
            Assert.AreEqual(patients, actualResult);
        }

        /// <summary>
        /// Checks the get all patients failed.
        /// </summary>
        [TestMethod]
        public void CheckGetAllPatientsFailed()
        {
            //Create Mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patients patients = new Patients();

            mockPatientRepository.Setup(x => x.GetAllPatients()).Returns(patients);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetAllPatients();

            //Assert
            Assert.AreEqual(patients, actualResult);
        }

        /// <summary>
        /// Checks the save patient successful.
        /// </summary>
        [TestMethod]
        public void CheckSavePatientSuccessful()
        {
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();
            const int DEFAULT_PATIENT_ID = 9;
            //Arrange
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",
                IsDeleted = 0
            };

            mockPatientRepository.Setup(x => x.MaxPatientId()).Returns(DEFAULT_PATIENT_ID);
            mockPatientRepository.Setup(x => x.SavePatient(patient)).Returns(patient);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.SavePatient(patient);

            //Assert
            Assert.AreEqual(patient, actualResult);
        }

        /// <summary>
        /// Checks the save patient failed.
        /// </summary>
        [TestMethod]
        public void CheckSavePatientFailed()
        {
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();
            const int DEFAULT_PATIENT_ID = 9;
            //Arrange
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",
                IsDeleted = 0
            };
            Patient savedPatient = null;

            mockPatientRepository.Setup(x => x.MaxPatientId()).Returns(DEFAULT_PATIENT_ID);
            mockPatientRepository.Setup(x => x.SavePatient(patient)).Returns(savedPatient);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.SavePatient(patient);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the save patient null.
        /// </summary>
        [TestMethod]
        public void CheckSavePatientNull()
        {
            const int DEFAULT_PATIENT_ID = 9;
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient = null;

            mockPatientRepository.Setup(x => x.MaxPatientId()).Returns(DEFAULT_PATIENT_ID);
            mockPatientRepository.Setup(x => x.SavePatient(patient)).Returns(patient);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.SavePatient(patient);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the get patient.
        /// </summary>
        [TestMethod]
        public void CheckGetPatient()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 1;
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",
                IsDeleted = 0
            };

            mockPatientRepository.Setup(x => x.GetPatient(patientId)).Returns(patient);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetPatient(patientId);

            //Assert
            Assert.AreEqual(patient, actualResult);
        }

        /// <summary>
        /// Checks the get patient identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetPatientIdNotExist()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 1;
            Patient patient = new Patient();

            mockPatientRepository.Setup(x => x.GetPatient(patientId)).Returns(patient);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetPatient(patientId);

            //Assert
            Assert.AreEqual(patient, actualResult);
        }

        /// <summary>
        /// Checks the update patient.
        /// </summary>
        [TestMethod]
        public void CheckUpdatePatient()
        {
            //Create Mock Object
            Mock<IPatientRepository> mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",

                IsDeleted = 0
            };

            mockPatientRepository.Setup(x => x.UpdatePatient(patient)).Returns(true);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.UpdatePatient(patient);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update patient null.
        /// </summary>
        [TestMethod]
        public void CheckUpdatePatientNull()
        {
            //Create Mock Object
            Mock<IPatientRepository> mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient = null;

            mockPatientRepository.Setup(x => x.UpdatePatient(It.IsAny<Patient>())).Returns(false);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.UpdatePatient(patient);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the delete patient.
        /// </summary>
        [TestMethod]
        public void CheckDeletePatient()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 1;

            mockPatientRepository.Setup(x => x.DeletePatient(patientId)).Returns(true);

            //Act
            PatientBusiness patienBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patienBusiness.DeletePatient(patientId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the delete patient identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckDeletePatientIdNotExist()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 1;

            mockPatientRepository.Setup(x => x.DeletePatient(It.IsAny<int>())).Returns(false);

            //Act
            PatientBusiness patienBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patienBusiness.DeletePatient(patientId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the search patients.
        /// </summary>
        [TestMethod]
        public void CheckSearchPatients()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",

                IsDeleted = 0
            };
            DateTime? DOB = null;
            int numberOfDays = 3;

            PatientWithExamInfo patientWithExamInfo = new PatientWithExamInfo
            {
                PatientID = 1,
                PatientName = "Mike",
                DOB = Convert.ToDateTime("12/03/1996"),
                MRN = "123"
            };

            PatientWithExamInfos patientWithExamInfos = new PatientWithExamInfos();
            patientWithExamInfos.PatientWithExamInfoList.Add(patientWithExamInfo);

            mockPatientRepository.Setup(x => x.SearchPatients(patient, DOB, numberOfDays)).Returns(patientWithExamInfos);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.SearchPatients(patient, DOB, numberOfDays);

            //Assert
            Assert.AreEqual(patientWithExamInfos, actualResult);
        }

        /// <summary>
        /// Checks the search patients no records.
        /// </summary>
        [TestMethod]
        public void CheckSearchPatientsNoRecords()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",

                IsDeleted = 0
            };
            DateTime? DOB = null;
            int numberOfDays = 3;
            PatientWithExamInfos patientWithExamInfos = new PatientWithExamInfos();

            mockPatientRepository.Setup(x => x.SearchPatients(patient, DOB, numberOfDays)).Returns(patientWithExamInfos);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.SearchPatients(patient, DOB, numberOfDays);

            //Assert
            Assert.AreEqual(patientWithExamInfos, actualResult);
        }

        /// <summary>
        /// MRN or Patient Id exists , but is of the same patient, So return should be false.
        /// Checked before updating the patient detail.
        /// </summary>
        [TestMethod]
        public void CheckIsMRNORPatientIdExistCase1()
        {
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 1;
            string mrnOrPatientId = patientId.ToString();
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",

                IsDeleted = 0
            };

            Patients patients = new Patients();
            patients.PatientList.Add(patient);

            mockPatientRepository.Setup(x => x.IsMRNORPatientIdExist(mrnOrPatientId)).Returns(patients);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            bool actualResult = patientBusiness.IsMRNORPatientIdExist(mrnOrPatientId, patientId);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Mrn Or PatientId exists for other patient, Return value should be true.
        /// Checked before adding new patient, so PatientId=0
        /// </summary>
        [TestMethod]
        public void CheckIsMRNORPatientIdExistCase2()
        {
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 0;
            string mrnOrPatientId = patientId.ToString();
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",

                IsDeleted = 0
            };

            Patients patients = new Patients();
            patients.PatientList.Add(patient);

            mockPatientRepository.Setup(x => x.IsMRNORPatientIdExist(mrnOrPatientId)).Returns(patients);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            bool actualResult = patientBusiness.IsMRNORPatientIdExist(mrnOrPatientId, patientId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// MRN or Patient Id exists for different patient, So return should be true.
        /// Checked before updating the patient detail, so PatientId!=0
        /// </summary>
        [TestMethod]
        public void CheckIsMRNORPatientIdExistCase3()
        {
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 2;
            string mrnOrPatientId = patientId.ToString();
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = Convert.ToDateTime("12/03/1996"),
                IdentificationNumber = "123",

                IsDeleted = 0
            };

            Patients patients = new Patients();
            patients.PatientList.Add(patient);

            mockPatientRepository.Setup(x => x.IsMRNORPatientIdExist(It.IsAny<string>())).Returns(patients);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            bool actualResult = patientBusiness.IsMRNORPatientIdExist(mrnOrPatientId, patientId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the is mrnor patient identifier exist MRN null.
        /// </summary>
        [TestMethod]
        public void CheckIsMRNORPatientIdExistMRNNull()
        {
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 1;
            string mrnOrPatientId = null;
            Patients patients = new Patients();

            mockPatientRepository.Setup(x => x.IsMRNORPatientIdExist(It.IsAny<string>())).Returns(patients);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.IsMRNORPatientIdExist(mrnOrPatientId, patientId);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the is mrnor patient identifier exist MRN not exists.
        /// </summary>
        [TestMethod]
        public void CheckIsMRNORPatientIdExistMRNNotExists()
        {
            //Create Mock Object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int patientId = 1;
            string mrnOrPatientId = "MRN1";
            Patients patients = new Patients();

            mockPatientRepository.Setup(x => x.IsMRNORPatientIdExist(It.IsAny<string>())).Returns(patients);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.IsMRNORPatientIdExist(mrnOrPatientId, patientId);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the get all patients with exam infos success.
        /// </summary>
        [TestMethod]
        public void CheckGetAllPatientsWithExamInfosSuccess()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            PatientWithExamInfo patientWithExamInfo1 = new PatientWithExamInfo
            {
                PatientID = 1,
                PatientName = "Mike",
                DOB = Convert.ToDateTime("12/03/1996"),
                MRN = "123"
            };

            PatientWithExamInfo patientWithExamInfo2 = new PatientWithExamInfo
            {
                PatientID = 2,
                PatientName = "Dan",
                DOB = Convert.ToDateTime("12/03/1997"),
                MRN = "121"
            };

            PatientWithExamInfos patientWithExamInfos = new PatientWithExamInfos();
            patientWithExamInfos.PatientWithExamInfoList.Add(patientWithExamInfo1);
            patientWithExamInfos.PatientWithExamInfoList.Add(patientWithExamInfo2);

            mockPatientRepository.Setup(x => x.GetAllPatientsWithExamInfos()).Returns(patientWithExamInfos);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualresult = patientBusiness.GetAllPatientsWithExamInfos();

            //Assert
            Assert.AreEqual(patientWithExamInfos, actualresult);
        }

        /// <summary>
        /// Checks the get all patients with exam infos failed.
        /// </summary>
        [TestMethod]
        public void CheckGetAllPatientsWithExamInfosFailed()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange

            PatientWithExamInfos patientWithExamInfos = new PatientWithExamInfos();

            mockPatientRepository.Setup(x => x.GetAllPatientsWithExamInfos()).Returns(patientWithExamInfos);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualresult = patientBusiness.GetAllPatientsWithExamInfos();

            //Assert
            Assert.AreEqual(patientWithExamInfos, actualresult);
        }

        /// <summary>
        /// Checks the get exams.
        /// </summary>
        [TestMethod]
        public void CheckGetExams()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = DateTime.Today,
                IdentificationNumber = "123",
                IsDeleted = 0
            };

            Exam exam1 = new Exam
            {
                PatientID = 1,
                AccessionNumber = "10",
                Conclusion = null,
                CreatedDateTime = DateTime.Now,
                Description = null,
                ExamInfoID = 1,
                ExamProcedureID = 1,

                Diagnosis = null,
                Physician = "Drake",
                ScheduledTime = DateTime.Now,
                Technologist = "David"
            };

            Exam exam2 = new Exam
            {
                PatientID = 1,
                AccessionNumber = "15",
                Conclusion = null,
                CreatedDateTime = DateTime.Now,
                Description = null,
                ExamInfoID = 3,
                ExamProcedureID = 4,

                Diagnosis = null,
                Physician = "Drake",
                ScheduledTime = DateTime.Now,
                Technologist = "David"
            };

            Exams exams = new Exams();
            exams.ExamList.Add(exam1);
            exams.ExamList.Add(exam2);

            mockPatientRepository.Setup(x => x.GetExams(patient)).Returns(exams);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetExams(patient);

            //Assert
            Assert.AreEqual(exams, actualResult);
        }

        /// <summary>
        /// Checks the get exams null.
        /// </summary>
        [TestMethod]
        public void CheckGetExamsNull()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            Patient patient = null;
            Exams exams = new Exams();

            mockPatientRepository.Setup(x => x.GetExams(null)).Returns(exams);

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetExams(patient);

            //Assert
            Assert.AreEqual(exams, actualResult);
        }

        /// <summary>
        /// Checks the get patient age successful.
        /// </summary>
        [TestMethod]
        public void CheckGetPatientAgeSuccessful()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            DateTime dob = DateTime.ParseExact("19930420", "yyyyMMdd", null);
            int expectedAge = DateTime.Today.Year - dob.Year;
            Patient patient = new Patient
            {
                PatientID = 1,
                FirstName = "Mike",
                LastName = "Taylor",
                Gender = "M",
                DOB = dob,
                IdentificationNumber = "123",
                IsDeleted = 0
            };

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetPatientAge(patient);

            //Assert
            Assert.AreEqual(expectedAge, actualResult);
        }

        /// <summary>
        /// Checks the get patient age failed.
        /// </summary>
        [TestMethod]
        public void CheckGetPatientAgeFailed()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            int expectedAge = 0;
            Patient patient = null;

            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetPatientAge(patient);

            //Assert
            Assert.AreEqual(expectedAge, actualResult);
        }

        /// <summary>
        /// Checks the get patient age range success.
        /// </summary>
        [TestMethod]
        public void CheckGetPatientAgeRangeSuccess()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            string expectedRange = "21-31";
            int patientAge = 23;
            mockPatientRepository.Setup(x => x.GetPatientAgeRange(patientAge)).Returns(expectedRange);
            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetPatientAgeRange(patientAge);

            //Assert
            Assert.AreEqual(expectedRange, actualResult);
        }

        /// <summary>
        /// Checks the get patient age range failed.
        /// </summary>
        [TestMethod]
        public void CheckGetPatientAgeRangeFailed()
        {
            //Create mock object
            var mockPatientRepository = Mock.Create<IPatientRepository>();

            //Arrange
            string expectedRange = string.Empty;
            int patientAge = 23;
            mockPatientRepository.Setup(x => x.GetPatientAgeRange(patientAge)).Returns(expectedRange);
            //Act
            PatientBusiness patientBusiness = new PatientBusiness(mockPatientRepository.Object);
            var actualResult = patientBusiness.GetPatientAgeRange(patientAge);

            //Assert
            Assert.AreEqual(expectedRange, actualResult);
        }
    }
}