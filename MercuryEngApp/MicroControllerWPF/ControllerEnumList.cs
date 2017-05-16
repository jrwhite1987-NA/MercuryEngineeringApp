// ***********************************************************************
// Assembly         : MicrochipController
// Author           : belapurkar_s
// Created          : 08-04-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-15-2016
// ***********************************************************************
// <copyright file="ControllerEnumList.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace MicrochipController
{
    /// <summary>
    /// Class ControllerEnumList.
    /// </summary>
    public class ControllerEnumList
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="ControllerEnumList"/> class from being created.
        /// </summary>
        private ControllerEnumList()
        {
        }

        /// <summary>
        /// Enum BatteryChargingState
        /// </summary>
        public enum BatteryChargingState : int { Charging = 1, Discharging, NA };

    }
}
