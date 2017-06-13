using MercuryEngApp.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MercuryEngApp.Test.MercuryEngApp.Common
{
    [TestClass]
    public class ScaleTest
    {
        Scale scale;
        public ScaleTest()
        {
            scale = new Scale();
        }

        [TestMethod]
        public void CreateScaleForSpectrogramTest()
        {
            ScaleParameters param = new ScaleParameters();
            param.ScaleType = ScaleTypeEnum.Spectrogram;
            param.ParentControl = new System.Windows.Controls.Grid();
            param.VelocityRange = 308;
            param.BitmapHeight = 100;
            param.ScreenCoords = new System.Windows.Point(10, 120);
            scale.CreateScale(param);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CreateMmodeScaleTest()
        {
            double minimum = 10;
            double maximum = 200;
            var parentControl = new System.Windows.Controls.Grid { Height = 150};

            scale.CreateMmodeScale(parentControl, minimum, maximum);
            Assert.IsTrue(true);
        }
    }
}
