// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 07-22-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="ReportBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.ReportBAL;
using Core.Interfaces.ReportInterface;
using Core.Models.ReportModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class ReportBusinessTest.
    /// </summary>
    [TestClass]
    public class ReportBusinessTest
    {
        /// <summary>
        /// Checks the get maximum report identifier.
        /// </summary>
        [TestMethod]
        public void CheckGetMaxReportId()
        {
            //create mock object
            var mockReportRepository = Mock.Create<IReportRepository>();

            //Arrange
            int maxReportId = 1;

            mockReportRepository.Setup(x => x.GetMaxReportId()).Returns(maxReportId).Callback(() => maxReportId++);

            //Act
            ReportBusiness reportBusiness = new ReportBusiness(mockReportRepository.Object);
            var actualResult = reportBusiness.GetMaxReportId();

            //Assert
            Assert.IsTrue(actualResult > 0);
        }

        /// <summary>
        /// Checks the is report exist.
        /// </summary>
        [TestMethod]
        public void CheckIsReportExist()
        {
            //create mock object
            var mockReportRepository = Mock.Create<IReportRepository>();

            //Arrange
            int examId = 8;

            mockReportRepository.Setup(x => x.IsReportExist(examId)).Returns(true);

            //Act
            ReportBusiness reportBusiness = new ReportBusiness(mockReportRepository.Object);
            var actualResult = reportBusiness.IsReportExist(examId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the get report by exam identifier.
        /// </summary>
        [TestMethod]
        public void CheckGetReportByExamId()
        {
            //create mock object
            var mockReportRepository = Mock.Create<IReportRepository>();

            //Arrange
            int examId = 8;

            Report report = new Report
            {
                Assessment = "Assessment",
                Conclusion = "Conclusion",
                CreatedDateTime = Convert.ToDateTime("12-03-2916"),
                ExamInfoID = 8,
                FilePath = "c:\\",
                ID = 1,
                Name = "ReportName",
                Type = 2
            };

            mockReportRepository.Setup(x => x.GetReportByExamId(examId)).Returns(report);

            //Act
            ReportBusiness reportBusiness = new ReportBusiness(mockReportRepository.Object);
            var actualResult = reportBusiness.GetReportByExamId(examId);

            //Assert
            Assert.AreEqual(report, actualResult);
        }

        /// <summary>
        /// Checks the save report.
        /// </summary>
        [TestMethod]
        public void CheckSaveReport()
        {
            //create mock object
            var mockReportRepository = Mock.Create<IReportRepository>();

            //Arrange

            Report report = new Report
            {
                Assessment = "Assessment",
                Conclusion = "Conclusion",
                CreatedDateTime = Convert.ToDateTime("12-03-2916"),
                ExamInfoID = 8,
                FilePath = "c:\\",
                ID = 1,
                Name = "ReportName",
                Type = 2
            };

            mockReportRepository.Setup(x => x.SaveReport(report)).Returns(true);

            //Act
            ReportBusiness reportBusiness = new ReportBusiness(mockReportRepository.Object);
            var actualResult = reportBusiness.SaveReport(report);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update exam notes assessment.
        /// </summary>
        [TestMethod]
        public void CheckUpdateExamNotesAssessment()
        {
            //create mock object
            var mockReportRepository = Mock.Create<IReportRepository>();

            //Arrange

            Report report = new Report
            {
                Assessment = "Assessment",
                Conclusion = "Conclusion",
                CreatedDateTime = Convert.ToDateTime("12-03-2916"),
                ExamInfoID = 8,
                FilePath = "c:\\",
                ID = 1,
                Name = "ReportName",
                Type = 2
            };

            mockReportRepository.Setup(x => x.UpdateExamNotesAssessment(report)).Returns(true);

            //Act
            ReportBusiness reportBusiness = new ReportBusiness(mockReportRepository.Object);
            var actualResult = reportBusiness.UpdateExamNotesAssessment(report);

            //Assert
            Assert.IsTrue(actualResult);
        }
    }
}