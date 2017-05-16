// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="ExamBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.ExamBAL;
using Core.Constants;
using Core.Interfaces.ExamInterface;
using Core.Models.ExamModels;
using Core.Models.RangeIndicatorModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class ExamBusinessTest.
    /// </summary>
    [TestClass]
    public class ExamBusinessTest
    {
        /// <summary>
        /// Checks the get all vessels by exam proc identifier.
        /// </summary>
        [TestMethod]
        public void CheckGetAllVesselsByExamProcId()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examProcId = 1;

            ExamProcedureSetting examProcedureSetting1 = new ExamProcedureSetting
            {
                ID = 1,
                Depth = 2,
                ExamProcedureID = 1,
                Vessel = "Vessel1",
            };

            ExamProcedureSetting examProcedureSetting2 = new ExamProcedureSetting
            {
                ID = 2,
                Depth = 5,
                ExamProcedureID = 1,
                Vessel = "Vessel2",
            };

            ExamProcedureSettings examProcedureSettings = new ExamProcedureSettings();
            examProcedureSettings.ExamProcedureSettingList.Add(examProcedureSetting1);
            examProcedureSettings.ExamProcedureSettingList.Add(examProcedureSetting2);

            mockExamRepository.Setup(x => x.GetAllVesselsByExamProcedureId(examProcId)).Returns(examProcedureSettings);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetAllVesselsByExamProcedureId(examProcId);

            //Assert
            Assert.AreEqual(examProcedureSettings, actualResult);
        }

        /// <summary>
        /// Checks the get all vessels by exam proc identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetAllVesselsByExamProcIdNotExist()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examProcId = 3;

            ExamProcedureSettings examProcedureSettings = null;

            mockExamRepository.Setup(x => x.GetAllVesselsByExamProcedureId(examProcId)).Returns(examProcedureSettings);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetAllVesselsByExamProcedureId(examProcId);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the get all snap shots of exam identifier.
        /// </summary>
        [TestMethod]
        public void CheckGetAllSnapShotsOfExamID()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examId = 1;
            ExamSnapShot examSnapShot = new ExamSnapShot
            {
                ChannelType = 1,
                Depth = 2,
                Indication = null,
                ExamSnapShotID = 1,
                CreatedDateTime = DateTime.Now,
                ExamInfoID = 1,
                Filter = 2,
                Gain = 10,
                IsSelected = false,
                NegMax = 20,
                PosMax = 100,
                NegMean = 20,
                NegMin = 10,
                PosMin = 100,
                Vessel = "Vessel1",
                PosPI = 2,
                NegPI = 5,
                BaseLinePos = 10,
                IsSnapShotImageCreated = false,
                PosMean = 1,
                Power = 10,
                SampleVolume = 10,
            };

            ExamSnapShots examSnapShots = new ExamSnapShots();
            examSnapShots.ExamSnapShotList.Add(examSnapShot);

            mockExamRepository.Setup(x => x.GetAllSnapShotsByExamID(examId)).Returns(examSnapShots);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetAllSnapShotsByExamID(examId);

            //Assert
            Assert.AreEqual(examSnapShots, actualResult);
        }

        /// <summary>
        /// Checks the get all snap shots of exam identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetAllSnapShotsOfExamIDNotExist()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examId = 7;
            ExamSnapShots examSnapShots = null;

            mockExamRepository.Setup(x => x.GetAllSnapShotsByExamID(examId)).Returns(examSnapShots);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetAllSnapShotsByExamID(examId);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the get exam vessel by age proc identifier.
        /// </summary>
        [TestMethod]
        public void CheckGetExamVesselByAgeProcId()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examProcId = 1;
            string vessel = "Vessel1";
            int age = 18;

            NormalRangeIndicator examVessel = new NormalRangeIndicator
            {
                //Age = 18,
                MeanCBFV = 10,
                PulsatilityIndex = 2,
            };

            mockExamRepository.Setup(x => x.GetExamVesselByAgeProcedureId(examProcId, age, vessel)).Returns(examVessel);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetExamVesselByAgeProcedureId(examProcId, age, vessel);

            //Assert
            Assert.AreEqual(examVessel, actualResult);
        }

        /// <summary>
        /// Checks the get exam vessel by age proc identifier age not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetExamVesselByAgeProcIdAgeNotExist()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examProcId = 1;
            string vessel = "Vessel1";
            int age = 18;

            NormalRangeIndicator examVessel = null;

            mockExamRepository.Setup(x => x.GetExamVesselByAgeProcedureId(examProcId, age, vessel)).Returns(examVessel);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetExamVesselByAgeProcedureId(examProcId, age, vessel);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the get exam vessel by age proc identifier vessel is null.
        /// </summary>
        [TestMethod]
        public void CheckGetExamVesselByAgeProcIdVesselIsNull()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examProcId = 1;
            string vessel = null;
            int age = 18;

            NormalRangeIndicator examVessel = null;

            mockExamRepository.Setup(x => x.GetExamVesselByAgeProcedureId(examProcId, age, vessel)).Returns(examVessel);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetExamVesselByAgeProcedureId(examProcId, age, vessel);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the get exam vessel by age proc identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetExamVesselByAgeProcIdNotExist()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examProcId = 1;
            string vessel = "Vessel1";
            int age = 18;

            NormalRangeIndicator examVessel = null;

            mockExamRepository.Setup(x => x.GetExamVesselByAgeProcedureId(examProcId, age, vessel)).Returns(examVessel);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetExamVesselByAgeProcedureId(examProcId, age, vessel);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the create snap shot.
        /// </summary>
        [TestMethod]
        public void CheckCreateSnapShot()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            ExamSnapShot examSnapShot = new ExamSnapShot
            {
                ChannelType = 1,
                Depth = 2,
                Indication = null,
                ExamSnapShotID = 1,
                CreatedDateTime = DateTime.Now,
                ExamInfoID = 1,
                Filter = 2,
                Gain = 10,
                IsSelected = false,
                NegMax = 20,
                PosMax = 100,
                NegMean = 20,
                NegMin = 10,
                PosMin = 100,
                Vessel = "Vessel1",
                PosPI = 2,
                NegPI = 5,
                BaseLinePos = 10,
                IsSnapShotImageCreated = false,
                PosMean = 1,
                Power = 10,
                SampleVolume = 10,
            };

            mockExamRepository.Setup(x => x.CreateSnapShot(examSnapShot)).Returns(true);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.CreateSnapShot(examSnapShot);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the create snap shot null.
        /// </summary>
        [TestMethod]
        public void CheckCreateSnapShotNull()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            ExamSnapShot examSnapShot = null;

            mockExamRepository.Setup(x => x.CreateSnapShot(examSnapShot)).Returns(false);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.CreateSnapShot(examSnapShot);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checkupdates the comments exam shapshot.
        /// </summary>
        [TestMethod]
        public void CheckupdateCommentsExamShapshot()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int selectedID = 2;
            string comments = "This is a sample comment";

            mockExamRepository.Setup(x => x.UpdateCommentsForExamShapshot(selectedID, comments)).Returns(true);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.UpdateCommentsForExamShapshot(selectedID, comments);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checkupdates the comments exam shapshot identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckupdateCommentsExamShapshotIdNotExist()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int selectedID = 2;
            string comments = "This is a sample comment";

            mockExamRepository.Setup(x => x.UpdateCommentsForExamShapshot(It.IsAny<int>(), comments)).Returns(false);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.UpdateCommentsForExamShapshot(selectedID, comments);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checkupdates the selected exam shapshot.
        /// </summary>
        [TestMethod]
        public void CheckupdateSelectedExamShapshot()
        {
            //Create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int selectedId = 2;
            int selectedImageTag = 1;

            mockExamRepository.Setup(x => x.UpdateDataForSelectedExamShapshot(selectedId, selectedImageTag)).Returns(true);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.UpdateDataForSelectedExamShapshot(selectedId, selectedImageTag);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update selected exam shapshot selected img tag not exist.
        /// </summary>
        [TestMethod]
        public void CheckUpdateSelectedExamShapshotSelectedImgTagNotExist()
        {
            //Create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int selectedId = 2;
            int selectedImageTag = 1;
            bool result = false;

            mockExamRepository.Setup<bool>(x => x.UpdateDataForSelectedExamShapshot(selectedId, selectedImageTag)).Returns(result);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.UpdateDataForSelectedExamShapshot(selectedId, selectedImageTag);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the save exam.
        /// </summary>
        [TestMethod]
        public void CheckSaveExam()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            Exam newExamInfo = new Exam
            {
                AccessionNumber = "1",
                CreatedDateTime = DateTime.Now,
                Conclusion = "assessment",
                Description = null,
                ExamInfoID = 0,
                ExamProcedureID = 2,

                Diagnosis = null,
                PatientID = 3,
                Physician = "David",
                ScheduledTime = DateTime.Now,
                Technologist = "James"
            };

            mockExamRepository.Setup(x => x.SaveExam(newExamInfo)).Returns(newExamInfo);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.SaveExam(newExamInfo);

            //Assert
            Assert.AreEqual(newExamInfo, actualResult);
        }

        /// <summary>
        /// Checks the save exam null.
        /// </summary>
        [TestMethod]
        public void CheckSaveExamNull()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            Exam newExamInfo = null;

            mockExamRepository.Setup(x => x.SaveExam(newExamInfo)).Returns(newExamInfo);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.SaveExam(newExamInfo);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the get filter delta for PRF below10000.
        /// </summary>
        [TestMethod]
        public void CheckGetFilterDeltaForPrfBelow10000()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            uint prfValue = 3000;
            uint expectedResult = 25;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetFilterDelta(prfValue);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks the get filter delta for PRF above10000.
        /// </summary>
        [TestMethod]
        public void CheckGetFilterDeltaForPrfAbove10000()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange

            uint prfValue = 10020;
            uint expectedResult = 100;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetFilterDelta(prfValue);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks the get filter maximum for PRF below10000.
        /// </summary>
        [TestMethod]
        public void CheckGetFilterMaxForPrfBelow10000()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            uint prfValue = 3000;
            uint expectedResult = Constants.maxFilter1;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetFilterMax(prfValue);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks the get filter maximum for PRF above10000.
        /// </summary>
        [TestMethod]
        public void CheckGetFilterMaxForPrfAbove10000()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            uint prfValue = 10020;
            uint expectedResult = Constants.maxFilter2;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetFilterMax(prfValue);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks the get filter minimum for PRF above10000.
        /// </summary>
        [TestMethod]
        public void CheckGetFilterMinForPrfAbove10000()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            uint prfValue = 10020;
            uint expectedResult = Constants.minFilter2;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetFilterMin(prfValue);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks the get filter minimum for PRF below10000.
        /// </summary>
        [TestMethod]
        public void CheckGetFilterMinForPrfBelow10000()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            uint prfValue = 3000;
            uint expectedResult = Constants.minFilter1;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.GetFilterMin(prfValue);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        /// <summary>
        /// Checks the get power delta return same.
        /// </summary>
        [TestMethod]
        public void CheckGetPowerDeltaReturnSame()
        {
            //create mock object
            var mockExamrepository = Mock.Create<IExamRepository>();

            //Arrange
            bool isIncrementFalse = false;
            uint currentPower = 4; uint expectedValue = currentPower;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamrepository.Object);
            var actualResult = examBusiness.GetPowerDelta(currentPower, isIncrementFalse);

            //Assert
            Assert.AreEqual(expectedValue, actualResult);
        }

        /// <summary>
        /// Checks the get power delta return maximum.
        /// </summary>
        [TestMethod]
        public void CheckGetPowerDeltaReturnMax()
        {
            //create mock object
            var mockExamrepository = Mock.Create<IExamRepository>();

            //Arrange
            bool isIncrementTrue = true;
            uint currentPower = 100; uint expectedValue = 100;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamrepository.Object);
            var actualResult = examBusiness.GetPowerDelta(currentPower, isIncrementTrue);

            //Assert
            Assert.AreEqual(expectedValue, actualResult);
        }

        /// <summary>
        /// Checks the get power delta return incremented value.
        /// </summary>
        [TestMethod]
        public void CheckGetPowerDeltaReturnIncrementedValue()
        {
            //create mock object
            var mockExamrepository = Mock.Create<IExamRepository>();

            //Arrange
            bool isIncrementTrue = true;
            uint currentPower = 20; uint expectedValue = 30;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamrepository.Object);
            var actualResult = examBusiness.GetPowerDelta(currentPower, isIncrementTrue);

            //Assert
            Assert.AreEqual(expectedValue, actualResult);
        }

        /// <summary>
        /// Checks the get power delta return minimum.
        /// </summary>
        [TestMethod]
        public void CheckGetPowerDeltaReturnMin()
        {
            //create mock object
            var mockExamrepository = Mock.Create<IExamRepository>();

            //Arrange
            bool isIncrementFalse = false;
            uint currentPower = 0; uint expectedValue = 0;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamrepository.Object);
            var actualResult = examBusiness.GetPowerDelta(currentPower, isIncrementFalse);

            //Assert
            Assert.AreEqual(expectedValue, actualResult);
        }

        /// <summary>
        /// Checks the get power delta return decremented value.
        /// </summary>
        [TestMethod]
        public void CheckGetPowerDeltaReturnDecrementedValue()
        {
            //create mock object
            var mockExamrepository = Mock.Create<IExamRepository>();

            //Arrange
            bool isIncrementFalse = false;
            uint currentPower = 30; uint expectedValue = 20;

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamrepository.Object);
            var actualResult = examBusiness.GetPowerDelta(currentPower, isIncrementFalse);

            //Assert
            Assert.AreEqual(expectedValue, actualResult);
        }

        /// <summary>
        /// Checks the update exam notes assessment.
        /// </summary>
        [TestMethod]
        public void CheckUpdateExamNotesAssessment()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            Exam exam = new Exam
            {
                AccessionNumber = "1",
                CreatedDateTime = DateTime.Now,
                Conclusion = "assessment",
                Description = null,
                ExamInfoID = 0,
                ExamProcedureID = 2,

                Diagnosis = null,
                PatientID = 3,
                Physician = "David",
                ScheduledTime = DateTime.Now,
                Technologist = "James"
            };

            mockExamRepository.Setup(x => x.UpdateExamNotesAssessment(exam)).Returns(true);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.UpdateExamNotesAssessment(exam);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update exam notes assessment null.
        /// </summary>
        [TestMethod]
        public void CheckUpdateExamNotesAssessmentNull()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            Exam exam = null;

            mockExamRepository.Setup(x => x.UpdateExamNotesAssessment(exam)).Returns(false);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.UpdateExamNotesAssessment(exam);

            //Assert
            Assert.IsFalse(actualResult);
        }

        /// <summary>
        /// Checks the hard delete exam.
        /// </summary>
        [TestMethod]
        public void CheckHardDeleteExam()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examInfoId = 3;

            mockExamRepository.Setup(x => x.HardDeleteExam(examInfoId)).Returns(true);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.HardDeleteExam(examInfoId);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the hard delete exam identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckHardDeleteExamIdNotExist()
        {
            //create mock object
            var mockExamRepository = Mock.Create<IExamRepository>();

            //Arrange
            int examInfoId = 3;

            mockExamRepository.Setup(x => x.HardDeleteExam(examInfoId)).Returns(false);

            //Act
            ExamBusiness examBusiness = new ExamBusiness(mockExamRepository.Object);
            var actualResult = examBusiness.HardDeleteExam(examInfoId);

            //Assert
            Assert.IsTrue(actualResult);
        }
    }
}