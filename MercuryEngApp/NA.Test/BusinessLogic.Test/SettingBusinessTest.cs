// ***********************************************************************
// Assembly         : NA.Test
// Author           : belapurkar_s
// Created          : 08-11-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="SettingBusinessTest.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using BusinessLogic.BusinessLogicLayer.SettingBAL;
using Core.Constants;
using Core.Interfaces.SettingInterface;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Moq;
using System.Collections.Generic;

namespace NA.Test.BusinessLogic.Test
{
    /// <summary>
    /// Class SettingBusinessTest.
    /// </summary>
    [TestClass]
    public class SettingBusinessTest
    {
        /// <summary>
        /// Checks the get setting success.
        /// </summary>
        [TestMethod]
        public void CheckGetSettingSuccess()
        {
            //Mock
            var mockSettingRepository = Mock.Create<ISettingRepository>();

            //Arrange
            Dictionary<string, string> SettingDictionary = new Dictionary<string, string>();
            SettingDictionary.Add(SettingConstant.NumberOfExams, "10");
            SettingDictionary.Add(SettingConstant.NumberFormat, "29.999");
            SettingDictionary.Add(SettingConstant.DateFormat, "dd/MM/yyyy");

            mockSettingRepository.Setup(x => x.GetSetting()).Returns(SettingDictionary);

            //Act
            SettingBusiness settingBusiness = new SettingBusiness(mockSettingRepository.Object);
            var actualResult = settingBusiness.GetSetting();

            //Assert
            Assert.AreEqual(SettingDictionary, actualResult);
        }

        /// <summary>
        /// Checks the get setting failed.
        /// </summary>
        [TestMethod]
        public void CheckGetSettingFailed()
        {
            //Mock
            var mockSettingRepository = Mock.Create<ISettingRepository>();

            //Arrange
            Dictionary<string, string> SettingDictionary = new Dictionary<string, string>();

            mockSettingRepository.Setup(x => x.GetSetting()).Returns(SettingDictionary);

            //Act
            SettingBusiness settingBusiness = new SettingBusiness(mockSettingRepository.Object);
            var actualResult = settingBusiness.GetSetting();

            //Assert
            Assert.AreEqual(SettingDictionary, actualResult);
        }

        /// <summary>
        /// Checks the update setting success.
        /// </summary>
        [TestMethod]
        public void CheckUpdateSettingSuccess()
        {
            //Mock
            var mockSettingRepository = Mock.Create<ISettingRepository>();

            //Arrange
            Dictionary<string, string> SettingDictionary = new Dictionary<string, string>();
            SettingDictionary.Add(SettingConstant.NumberOfExams, "10");
            SettingDictionary.Add(SettingConstant.NumberFormat, "29.999");
            SettingDictionary.Add(SettingConstant.DateFormat, "dd/MM/yyyy");

            mockSettingRepository.Setup(x => x.UpdateSetting(SettingDictionary)).Returns(true);

            //Act
            SettingBusiness settingBusiness = new SettingBusiness(mockSettingRepository.Object);
            var actualResult = settingBusiness.UpdateSetting(SettingDictionary);

            //Assert
            Assert.IsTrue(actualResult);
        }

        /// <summary>
        /// Checks the update setting failed.
        /// </summary>
        [TestMethod]
        public void CheckUpdateSettingFailed()
        {
            //Mock
            var mockSettingRepository = Mock.Create<ISettingRepository>();

            //Arrange
            Dictionary<string, string> SettingDictionary = null;

            mockSettingRepository.Setup(x => x.UpdateSetting(SettingDictionary)).Returns(false);

            //Act
            SettingBusiness settingBusiness = new SettingBusiness(mockSettingRepository.Object);
            var actualResult = settingBusiness.UpdateSetting(SettingDictionary);

            //Assert
            Assert.IsTrue(actualResult);
        }
    }
}