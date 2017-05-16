// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="TCDInfoBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.TCDBAL;
using Core.Interfaces.TCDInterface;
using Core.Models.TCDModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class TCDInfoBusinessTest.
    /// </summary>
    [TestClass]
    public class TCDInfoBusinessTest
    {
        /// <summary>
        /// Checks the save TCD information.
        /// </summary>
        [TestMethod]
        public void CheckSaveTCDInfo()
        {
            //create mock object
            var mockTCDInterface = Mock.Create<ITCDInfoRepository>();

            //Arrange
            TCDInfo tcdInfo = new TCDInfo
            {
                ChannelType = 1,
                CreatedDateTime = Convert.ToDateTime("12/03/2016"),
                Depth = 1,
                ExamInfoID = 1,
                Filter = 20,
                Gain = 30,
                TCDInfoID = 1,
                UltrasonicPower = 2,
                Vessel = "vessel"
            };

            mockTCDInterface.Setup(x => x.SaveTCDInfo(tcdInfo)).Returns(tcdInfo);

            //Act
            TCDInfoBusiness tcdInfoBusiness = new TCDInfoBusiness(mockTCDInterface.Object);
            var actualResult = tcdInfoBusiness.SaveTCDInfo(tcdInfo);

            //Assert
            Assert.AreEqual(tcdInfo, actualResult);
        }
    }
}