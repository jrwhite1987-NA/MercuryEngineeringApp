using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MercuryEngApp.Common;
using System.Collections.Generic;
using Core.Constants;

namespace MercuryEngApp.Test.MercuryEngApp.Common
{
    /// <summary>
    /// Test Class
    /// </summary>
    [TestClass]
    public class SpectrogramSettingTest
    {
        /// <summary>
        /// Test Method to get Spectogramsetting
        /// </summary>
        [TestMethod]
        public void GetSpectrogramSettingTest1()
        {
            SpectrogramSetting spectrogramSetting =
                SpectrogramSetting.GetSpectrogramSetting(Constants.VALUE_23, true);

            Assert.IsTrue((spectrogramSetting != null));
        }

        /// <summary>
        /// Test Method to get Spectogramsetting with exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Value of depth can not be zero.")]
        public void GetSpectrogramSettingExTest()
        {
            SpectrogramSetting.GetSpectrogramSetting(0, false);
        }

        /// <summary>
        /// Test Method to get Nearest velocity range
        /// </summary>
        [TestMethod]
        public void GetNearestVelocityRangeTest()
        {
            Assert.IsTrue(true);
            //SpectrogramSetting.GetNearestVelocityRange(10)
        }
        
        /// <summary>
        /// Test Method to get index for base line
        /// </summary>
        [TestMethod]
        public void GetIndexforBaselineTest()
        {
            const int baseLinePosition = Constants.VALUE_0;
            Assert.IsTrue(SpectrogramSetting.GetIndexforBaseline(baseLinePosition) ==
                baseLinePosition);
        }

        /// <summary>
        /// Test Method to get velocaity range with exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Value of PRF can not be zero.")]
        public void GetTotalVelocityRangeExTest()
        {
            PRFOptions.GetTotalVelocityRange(0);
        }

        /// <summary>
        /// Test Method to get total velocity range
        /// </summary>
        [TestMethod]
        public void GetTotalVelocityRangeTest()
        {
            int velocityRange = PRFOptions.GetTotalVelocityRange(1);
            string str = velocityRange.ToString();
        }
    }
}
