using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlottingLib;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace MercuryEngApp.Test.PlottingLib
{
    [TestClass]
    public class NAGraphTest
    {
        public NaGraph NaGraph { get; set; }

        public NAGraphTest()
        {
            WriteableBitmap dummyBitmap = BitmapFactory.New(10, 10);
            NaGraph = new NaGraph(new BitmapList {
                LeftMmodeBitmap = dummyBitmap, 
                LeftSpectrumBitmap = dummyBitmap,
                RightMmodeBitmap = dummyBitmap,
                RightSpectrumBitmap = dummyBitmap
            });
        }

        [TestMethod]
        public void InitializeTest_FourSecond()
        {
            SpectrumXScale scale = SpectrumXScale.FourSecond;
            var gain = 10;
            NaGraph.Initialize(scale,gain);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void InitializeTest_EightSecond()
        {
            SpectrumXScale scale = SpectrumXScale.EightSecond;
            var gain = 10;
            NaGraph.Initialize(scale, gain);
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void InitializeTest_TwelveSecond()
        {
            SpectrumXScale scale = SpectrumXScale.TwelveSecond;
            var gain = 10;
            NaGraph.Initialize(scale, gain);
            Assert.IsTrue(true);
        }

    }
}
