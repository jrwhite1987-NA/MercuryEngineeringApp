using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlottingLib;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Moq;
using Core.Constants;
namespace MercuryEngApp.Test.PlottingLib
{
    [TestClass]
    public class NAGraphTest
    {
        public NaGraph NaGraph { get; set; }

        public NAGraphTest()
        {
            WriteableBitmap dummyBitmap = BitmapFactory.New(Constants.VALUE_10, Constants.VALUE_10);
            NaGraph = new NaGraph(new BitmapList {
                LeftMmodeBitmap = dummyBitmap, 
                LeftSpectrumBitmap = dummyBitmap,
                RightMmodeBitmap = dummyBitmap,
                RightSpectrumBitmap = dummyBitmap
            });
        }

        [TestMethod]
        public void InitializeTestFourSecond()
        {            
            SpectrumXScale scale = SpectrumXScale.FourSecond;
            var gain = Constants.VALUE_10;
            NaGraph.Initialize(scale,gain);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InitializeTestEightSecond()
        {
            SpectrumXScale scale = SpectrumXScale.EightSecond;
            var gain = Constants.VALUE_10;
            NaGraph.Initialize(scale, gain);
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void InitializeTestTwelveSecond()
        {
            SpectrumXScale scale = SpectrumXScale.TwelveSecond;
            var gain = Constants.VALUE_10;
            NaGraph.Initialize(scale, gain);
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void SetGainTestForPositive()
        {
            var gain = Constants.VALUE_10;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);

            gain = Constants.VALUE_20;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);

            gain = Constants.VALUE_30;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);

            gain = Constants.VALUE_39;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void SetGainTestForNegative()
        {
            var gain = -Constants.VALUE_10;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);

            gain = -Constants.VALUE_20;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);

            gain = -Constants.VALUE_30;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);

            gain = -Constants.VALUE_39;
            NaGraph.SetGain(gain);
            Assert.IsTrue(true);
        }
    }
}
