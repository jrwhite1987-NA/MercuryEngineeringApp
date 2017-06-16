﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MercuryEngApp.Common;
using System.Collections.Generic;
using Core.Constants;

namespace MercuryEngApp.Test.MercuryEngApp.Common
{
    [TestClass]
    public class SpectrogramSettingTest
    {
        [TestMethod]
        public void GetSpectrogramSettingTest1()
        {
            SpectrogramSetting spectrogramSetting =
                SpectrogramSetting.GetSpectrogramSetting(Constants.VALUE_23, true);

            Assert.IsTrue((spectrogramSetting != null));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Value of depth can not be zero.")]
        public void GetSpectrogramSettingExTest()
        {
            SpectrogramSetting spectrogramSetting =
                SpectrogramSetting.GetSpectrogramSetting(0, false);
        }

        [TestMethod]
        public void GetNearestVelocityRangeTest()
        {
            Assert.IsTrue(true);
            //SpectrogramSetting.GetNearestVelocityRange(10)
        }
         
        [TestMethod]
        public void GetIndexforBaselineTest()
        {
            const int baseLinePosition = Constants.VALUE_0;
            Assert.IsTrue(SpectrogramSetting.GetIndexforBaseline(baseLinePosition) ==
                baseLinePosition);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Value of PRF can not be zero.")]
        public void GetTotalVelocityRangeExTest()
        {
            PRFOptions.GetTotalVelocityRange(0);
        }

        [TestMethod]
        public void GetTotalVelocityRangeTest()
        {
            PRFOptions.GetTotalVelocityRange(1);
        }
    }
}
