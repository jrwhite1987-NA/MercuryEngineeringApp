using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicrochipController;
using System.Threading.Tasks;

namespace MercuryEngApp.Test.MicroControllerWPF
{
    [TestClass]
    public class PowerMicrocontrollerTest
    {
        IController powerMicroController;

        public PowerMicrocontrollerTest()
        {
            powerMicroController = new PowerMicrocontroller();
        }

        [TestMethod]
        public void GetBatteryPercentageTest()
        {
            System.Threading.Tasks.Task<int> percent =
                powerMicroController.GetBatteryPercentage();

            Assert.IsTrue(percent != null);
        }

        [TestMethod]
        public void TurnControllerOnTest()
        {
            Task<bool> bOn = powerMicroController.TurnControllerOn();
            Assert.IsTrue(bOn != null);
        }

        [TestMethod]
        public void TurnControllerOffTest()
        {
            bool bOff = powerMicroController.TurnControllerOff();
            Assert.IsTrue(bOff == true);
        }

        [TestMethod]
        public void GetDescriptorsTest()
        {
            string descriptors = powerMicroController.GetDescriptors();
            Assert.IsTrue(!string.IsNullOrEmpty(descriptors));
        }

        [TestMethod]
        public void IsBatteryChargingTest()
        {
            Task<ControllerEnumList.BatteryChargingState> status =
                powerMicroController.IsBatteryCharging();

            Assert.IsTrue(status.Result == ControllerEnumList.BatteryChargingState.Charging ||
                status.Result == ControllerEnumList.BatteryChargingState.Discharging ||
                status.Result == ControllerEnumList.BatteryChargingState.NA);
        }

        [TestMethod]
        public void IsExternalVoltageAppliedTest()
        {
            Task<int> isVoltageApplied = powerMicroController.IsExternalVoltageApplied();
            Assert.IsTrue(isVoltageApplied.Exception == null);
        }
    }
}
