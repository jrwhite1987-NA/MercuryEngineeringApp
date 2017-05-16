// ***********************************************************************
// Assembly         : MicrochipController
// Author           : belapurkar_s
// Created          : 06-30-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="IController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Threading.Tasks;

namespace MicrochipController
{
    public delegate void FlagEvent(bool flag);
    /// <summary>
    /// Interface IController
    /// </summary>
    public interface IController
    {
        event FlagEvent OnDeviceStateChanged;
        bool IsControllerOn
        {
            get;
        }
        /// <summary>
        /// Gets a value indicating whether this instance is channel1 enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is channel1 enabled; otherwise, <c>false</c>.</value>
        bool IsChannel1Enabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is channel2 enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is channel2 enabled; otherwise, <c>false</c>.</value>
        bool IsChannel2Enabled
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is channel reset.
        /// </summary>
        /// <value><c>true</c> if this instance is channel reset; otherwise, <c>false</c>.</value>
        bool IsChannelReset
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is mo enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is mo enabled; otherwise, <c>false</c>.</value>
        bool IsMOEnabled
        {
            get;
        }

        /// <summary>
        /// Turns the controller on.
        /// </summary>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> TurnControllerOn();

        /// <summary>
        /// Gets the descriptors.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetDescriptors();

        /// <summary>
        /// Updates the power parameters.
        /// </summary>
        /// <param name="channel1Power">if set to <c>true</c> [channel1 power].</param>
        /// <param name="channel2Power">if set to <c>true</c> [channel2 power].</param>
        /// <param name="channel1Reset">if set to <c>true</c> [channel1 reset].</param>
        /// <param name="channel2Reset">if set to <c>true</c> [channel2 reset].</param>
        /// <param name="MOEnable">if set to <c>true</c> [mo enable].</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> UpdatePowerParameters(bool channel1Power, bool channel2Power, bool channel1Reset, bool channel2Reset, bool MOEnable);

        void StartWatcher();

        /// <summary>
        /// Turns the controller off.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TurnControllerOff();

        /// <summary>
        /// Gets the version number.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        Task<string> GetVersionNumber();

        /// <summary>
        /// Determines whether [is battery charging].
        /// </summary>
        /// <returns>ControllerEnumList.BatteryChargingState.</returns>
        Task<ControllerEnumList.BatteryChargingState> IsBatteryCharging();

        Task<int> GetBatteryCurrent();
        /// <summary>
        /// Gets the battery charge.
        /// </summary>
        /// <returns>System.Int32.</returns>
        Task<int> GetBatteryPercentage();

        //returns the voltage level
        Task<int> GetVoltageLevel();
        //returns external voltage state
        Task<int> GetBatteryVoltage();

        Task<int> GetBatteryRemainingCapacity();

        Task<int> GetBatteryFullCapacity();

        Task<int> IsExternalVoltageApplied();

    }
}
