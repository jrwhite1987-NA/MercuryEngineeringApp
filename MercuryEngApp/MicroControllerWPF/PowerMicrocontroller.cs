// ***********************************************************************
// Assembly         : MicrochipController
// Author           : belapurkar_s
// Created          : 06-30-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="PowerMicrocontroller.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Common;
using Core.Constants;
using System;
using System.Threading.Tasks;


namespace MicrochipController
{
    /// <summary>
    /// Class PowerMicrocontroller.
    /// </summary>
    /// <seealso cref="MicrochipController.IController" />
    public class PowerMicrocontroller : IController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerMicrocontroller"/> class.
        /// </summary>
        public PowerMicrocontroller()
        {
            MicroControllerHandler.Current.onDeviceAddedRemoved += OnDeviceAddedRemoved;
        }

        /// <summary>
        /// Called when [device added removed].
        /// </summary>
        /// <param name="flag">if set to <c>true</c> [flag].</param>
        private void OnDeviceAddedRemoved(bool flag)
        {
            if (OnDeviceStateChanged != null)
            {
                OnDeviceStateChanged(flag);
            }
        }

        /// <summary>
        /// Returns the remaining charge % of battery
        /// </summary>
        /// <returns>System.Int32.</returns>
        /// Debug statements not required
        public async Task<int> GetBatteryPercentage()
        {
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                return await MicroControllerHandler.Current.GetBatteryCharge();
            }
            else
            {
                return Constants.VALUE_0;
            }
        }

        /// <summary>
        /// Gets the handle of power micro contoller
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        /// Debug statements not required
        public async Task<bool> TurnControllerOn()
        {
            try
            {
                if (!MicroControllerHandler.Current.IsControllerWorking)
                {
                    return await MicroControllerHandler.Current.GetDeviceHandleAsync();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<PowerMicrocontroller>(ex, "TurnControllerOn", Severity.Critical);
                return false;
            }
        }

        /// <summary>
        /// Gets the usb descriptors of the mircro controller
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetDescriptors()
        {
            try
            {
                //Logs.Instance.ErrorLog<MicroControllerHandler>("GetDescriptors begins ", "GetDescriptors", Severity.Debug);
                if (MicroControllerHandler.Current.IsControllerWorking)
                {
                    //Logs.Instance.ErrorLog<MicroControllerHandler>("GetDescriptors ends ", "GetDescriptors", Severity.Debug);
                    return MicroControllerHandler.Current.GetDescriptorsAsString();
                }
                else
                {
                    //Logs.Instance.ErrorLog<MicroControllerHandler>("GetDescriptors ends ", "GetDescriptors", Severity.Debug);
                    return "";
                }

            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<PowerMicrocontroller>(ex, "GetDescriptors", Severity.Warning);
                return "";
            }
        }

        /// <summary>
        /// Returns if the power to channel1 is on and handle is acquired
        /// </summary>
        /// <value><c>true</c> if this instance is channel1 enabled; otherwise, <c>false</c>.</value>
        public bool IsChannel1Enabled
        {
            get { return MicroControllerHandler.Current.IsChannel1Enabled; }
        }

        /// <summary>
        /// Returns if the power to channel2 is on and handle is acquired
        /// </summary>
        /// <value><c>true</c> if this instance is channel2 enabled; otherwise, <c>false</c>.</value>
        public bool IsChannel2Enabled
        {
            get { return MicroControllerHandler.Current.IsChannel2Enabled; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is channel reset.
        /// </summary>
        /// <value><c>true</c> if this instance is channel reset; otherwise, <c>false</c>.</value>
        public bool IsChannelReset
        {
            get { return MicroControllerHandler.Current.IsChannelReset; }
        }

        /// <summary>
        /// Returns if the Oscillator is turned on
        /// </summary>
        /// <value><c>true</c> if this instance is mo enabled; otherwise, <c>false</c>.</value>
        public bool IsMOEnabled
        {
            get { return MicroControllerHandler.Current.IsMOEnabled; }
        }

        /// <summary>
        /// Returns if the new power parameters are set on the controller
        /// </summary>
        /// <param name="channel1Power">if set to <c>true</c> [channel1 power].</param>
        /// <param name="channel2Power">if set to <c>true</c> [channel2 power].</param>
        /// <param name="channel1Reset">if set to <c>true</c> [channel1 reset].</param>
        /// <param name="channel2Reset">if set to <c>true</c> [channel2 reset].</param>
        /// <param name="MOEnable">if set to <c>true</c> [mo enable].</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> UpdatePowerParameters
            (bool channel1Power, bool channel2Power, bool channel1Reset, bool channel2Reset, bool MOEnable)
        {
            try
            {
                //Logs.Instance.ErrorLog<MicroControllerHandler>("async UpdatePowerParameters begins for channel1Power:" + channel1Power.ToString()
                   // + "channel2Power:" + channel2Power.ToString() + "channel1Reset:" + channel1Reset.ToString() + "channel2Reset:" + channel2Reset.ToString()
                   // + "moenable:" + MOEnable.ToString(), "UpdatePowerParameters", Severity.Debug);
                if (MicroControllerHandler.Current.IsControllerWorking)
                {
                    //Logs.Instance.ErrorLog<MicroControllerHandler>("async UpdatePowerParameters ends ", "UpdatePowerParameters", Severity.Debug);
                    return await MicroControllerHandler.Current.SendPowerParameters
                        (channel1Power, channel2Power, channel1Reset, channel2Reset, MOEnable);
                }
                else
                {
                    //Logs.Instance.ErrorLog<MicroControllerHandler>
                      //  (MessageConstants.MicroControllerConnectionFailed, "SendPowerParameters", Severity.Warning);
                    //Logs.Instance.ErrorLog<MicroControllerHandler>("async UpdatePowerParameters ends ", "UpdatePowerParameters", Severity.Debug);
                    return false;
                }

            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<PowerMicrocontroller>(ex, "UpdatePowerParameters", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Releases the handle for power micro controller
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// Debug statements not required
        public bool TurnControllerOff()
        {
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                return MicroControllerHandler.Current.ReleaseDevice();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Starts the watcher.
        /// </summary>
        public void StartWatcher()
        {
            try
            {
                MicroControllerHandler.Current.InitializeDeviceWatcher();
            }
            catch(Exception ex)
            {
                //Logs.Instance.ErrorLog<PowerMicrocontroller>(ex, "StartWatcher", Severity.Warning);
            }
        }

        /// <summary>
        /// Determines whether [is battery charging].
        /// </summary>
        /// <returns>ControllerEnumList.BatteryChargingState.</returns>
        public async Task<ControllerEnumList.BatteryChargingState> IsBatteryCharging()
        {
            try
            {
                if (MicroControllerHandler.Current.IsControllerWorking)
                {
                    int current = await MicroControllerHandler.Current.GetBatteryCurrent();

                    if (Constants.VALUE_0 < current)
                    {
                        return ControllerEnumList.BatteryChargingState.Charging;
                    }
                    else if (Constants.VALUE_0 > current)
                    {
                        return ControllerEnumList.BatteryChargingState.Discharging;
                    }
                    else
                    {
                        return ControllerEnumList.BatteryChargingState.NA;
                    }
                }
                return ControllerEnumList.BatteryChargingState.NA;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<PowerMicrocontroller>(ex, "IsBatteryCharging", Severity.Warning);
                return ControllerEnumList.BatteryChargingState.NA;
            }
        }

        /// <summary>
        /// Determines whether [is external voltage applied].
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> IsExternalVoltageApplied()
        {
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                int externalVoltage = await MicroControllerHandler.Current.GetVoltageState();
                //if there was an error, return the max value
                if (externalVoltage == short.MaxValue)
                {
                    return int.MaxValue;
                }
                //otherwise return 1 if it's connected and 0 if it is not
                bool status = (externalVoltage & 0x10) == 0x10;  //external voltage mask

                if (status)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            return int.MaxValue;
        }


        /// <summary>
        /// Gets the version number.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// Debug statements not required
        public async Task<string> GetVersionNumber()
        {
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                return await MicroControllerHandler.Current.GetVersionInfo();
            }
            else
            {
                return MessageConstants.NotAvailable;
            }
        }

        /// <summary>
        /// Gets the battery current.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        /// Debug statements not required
        public async Task<int> GetBatteryCurrent()
        {
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                return await MicroControllerHandler.Current.GetBatteryCurrent();
            }
            else
            {
                return Constants.VALUE_0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is controller on.
        /// </summary>
        /// <value><c>true</c> if this instance is controller on; otherwise, <c>false</c>.</value>
        public bool IsControllerOn
        {
            get
            {
                if (MicroControllerHandler.Current.controllerHandler != null)
                {
                    return MicroControllerHandler.Current.IsControllerWorking;
                }
                else
                {
                    MicroControllerHandler.Current.IsControllerWorking = false;
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the voltage level.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> GetVoltageLevel()
        {
            int voltage = 0;
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                voltage = await MicroControllerHandler.Current.GetVoltageLevel();
            }
            return voltage;
        }

        /// <summary>
        /// Gets the battery voltage.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> GetBatteryVoltage()
        { 
            int externalVoltage=0;
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                externalVoltage = await MicroControllerHandler.Current.GetVoltageState();
            }
            return externalVoltage;
        }

        /// <summary>
        /// Gets the battery remaining capacity.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> GetBatteryRemainingCapacity()
        {
            int remainingCapacity = 0;
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                remainingCapacity = await MicroControllerHandler.Current.GetRemainingCharge();
            }
            return remainingCapacity;
        }

        /// <summary>
        /// Gets the battery full capacity.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public async Task<int> GetBatteryFullCapacity()
        {
            int fullCapacity = 0;
            if (MicroControllerHandler.Current.IsControllerWorking)
            {
                fullCapacity = await MicroControllerHandler.Current.GetFullCharge();
            }
            return fullCapacity;
        }

        /// <summary>
        /// Occurs when [on device state changed].
        /// </summary>
        public event FlagEvent OnDeviceStateChanged;
    }
}
