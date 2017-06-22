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
    //Test Class for NA Graph
    public class NAGraphTest
    {
        /// <summary>
        /// Gets or sets the NA Graph object
        /// </summary>
        public NaGraph NaGraph { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public NAGraphTest()
        {
            //Create new Bitmap
            WriteableBitmap dummyBitmap = BitmapFactory.New(Constants.VALUE_10, Constants.VALUE_10);

            //Instantiate the object
            NaGraph = new NaGraph(new BitmapList {
                LeftMmodeBitmap = dummyBitmap, 
                LeftSpectrumBitmap = dummyBitmap,
                RightMmodeBitmap = dummyBitmap,
                RightSpectrumBitmap = dummyBitmap
            });
        }

        /// <summary>
        /// Test Metod for Test four second
        /// </summary>
        [TestMethod]
        public void InitializeTestFourSecond()
        {           
            //Arrange
            SpectrumXScale scale = SpectrumXScale.FourSecond;
            var gain = Constants.VALUE_10;
            //Act
            NaGraph.Initialize(scale,gain);
            //Assert
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test Metod for Test Eight second
        /// </summary>
        [TestMethod]
        public void InitializeTestEightSecond()
        {
            //Arrange
            SpectrumXScale scale = SpectrumXScale.EightSecond;
            var gain = Constants.VALUE_10;
            //Act
            NaGraph.Initialize(scale, gain);
            //Assert
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test Metod for Test Twelve second
        /// </summary>
        [TestMethod]
        public void InitializeTestTwelveSecond()
        {
            //Arrange
            SpectrumXScale scale = SpectrumXScale.TwelveSecond;
            var gain = Constants.VALUE_10;
            //Act
            NaGraph.Initialize(scale, gain);
            //Assert
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test Metod for Test set gain for positive
        /// </summary>
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


        /// <summary>
        /// Test Metod for Test set gain for Negative
        /// </summary>
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
