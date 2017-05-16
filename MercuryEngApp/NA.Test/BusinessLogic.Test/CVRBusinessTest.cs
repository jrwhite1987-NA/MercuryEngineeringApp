// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="CVRBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.CVRBAL;
using Core.Interfaces.CVRInterface;
using Core.Models.CVRModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class CVRBusinessTest.
    /// </summary>
    [TestClass]
    public class CVRBusinessTest
    {
        /// <summary>
        /// Checks the get CVR.
        /// </summary>
        [TestMethod]
        public void CheckGetCVR()
        {
            //create mock object
            var mockCVRRepository = Mock.Create<ICVRDualChannelRepository>();

            //Arrange
            int examId = 1;
            int ChannelId = 1;
            bool flag = true;
            CVR cvr = new CVR
            {
                Id = 1,
                CreatedDate = Convert.ToDateTime("12/03/2016"),
                CVRDataFile = "cvrFile",
                ExamID = 1,
                High = 10.0,
                IsAutoMode = true,
                Low = 5.5,
                Mean = 2.3
            };

            mockCVRRepository.Setup(x => x.GetCVRData(examId, ChannelId, flag)).Returns(cvr);

            //Act
            CVRDualChannelBusiness cvrBusiness = new CVRDualChannelBusiness(mockCVRRepository.Object);
            var actualResult = cvrBusiness.GetCVRData(examId, ChannelId, flag);

            //Assert
            Assert.AreEqual(cvr, actualResult);
        }

        /// <summary>
        /// Checks the get CVR exam identifier not exist.
        /// </summary>
        [TestMethod]
        public void CheckGetCVRExamIdNotExist()
        {
            //create mock object
            var mockCVRRepository = Mock.Create<ICVRDualChannelRepository>();

            //Arrange
            int examId = 0;
            int channelId = 2;
            CVR cvr = null;
            bool flag = true;

            mockCVRRepository.Setup(x => x.GetCVRData(examId, channelId, flag)).Returns(cvr);

            //Act
            CVRDualChannelBusiness cvrBusiness = new CVRDualChannelBusiness(mockCVRRepository.Object);
            var actualResult = cvrBusiness.GetCVRData(examId, channelId, flag);

            //Assert
            Assert.IsNull(actualResult);
        }

        /// <summary>
        /// Checks the update CVR.
        /// </summary>
        [TestMethod]
        public void CheckUpdateCVR()
        {
            //create mock object
            var mockCVRRepository = Mock.Create<ICVRDualChannelRepository>();

            //Arrange
            CVR cvr = new CVR
            {
                Id = 1,
                CreatedDate = Convert.ToDateTime("12/03/2016"),
                CVRDataFile = "cvrFile",
                ExamID = 1,
                High = 10.0,
                IsAutoMode = true,
                Low = 5.5,
                Mean = 2.3
            };

            mockCVRRepository.Setup(x => x.UpdateCVR(cvr)).Returns(true);

            //Act
            CVRDualChannelBusiness cvrBusiness = new CVRDualChannelBusiness(mockCVRRepository.Object);
            var actualResult = cvrBusiness.UpdateCVR(cvr);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update CVR null.
        /// </summary>
        [TestMethod]
        public void CheckUpdateCVRNull()
        {
            //create mock object
            var mockCVRRepository = Mock.Create<ICVRDualChannelRepository>();
            CVR cvr = null;

            mockCVRRepository.Setup(x => x.UpdateCVR(cvr)).Returns(false);

            //Act
            CVRDualChannelBusiness cvrBusiness = new CVRDualChannelBusiness(mockCVRRepository.Object);
            var actualResult = cvrBusiness.UpdateCVR(cvr);

            //Assert
            Assert.IsTrue(actualResult);
        }
    }
}