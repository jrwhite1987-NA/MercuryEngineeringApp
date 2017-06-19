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
        const int VALUE_308 = 308;
        const int VALUE_100 = 100;
        const int VALUE_10 = 10;
        const int VALUE_120 = 120;
        const int VALUE_200 = 200;
        const int VALUE_150 = 150;
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
            param.VelocityRange = VALUE_308;
            param.BitmapHeight = VALUE_100;
            param.ScreenCoords = new System.Windows.Point(VALUE_10, VALUE_120);
            scale.CreateScale(param);
            bool result = true;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateMmodeScaleTest()
        {
            double minimum = VALUE_10;
            double maximum = VALUE_200;
            var parentControl = new System.Windows.Controls.Grid { Height = VALUE_150};

            scale.CreateMmodeScale(parentControl, minimum, maximum);
            bool result = true;
            Assert.IsTrue(result);
        }
    }
}
