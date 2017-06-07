// ***********************************************************************
// Assembly         : Mercury
// Author           : belapurkar_s
// Created          : 07-28-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="ScaleGenerator.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************

using Core.Common;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MercuryEngApp.Common
{

    /// <summary>
    /// Class ScaleParameters.
    /// </summary>
    public class ScaleParameters
    {
        /// <summary>
        /// Gets or sets the parent control.
        /// </summary>
        /// <value>The parent control.</value>
        public Grid ParentControl { get; set; }

        /// <summary>
        /// Gets or sets the custom slider value.
        /// </summary>
        /// <value>The custom slider value.</value>
        public double CustomSliderValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the scale.
        /// </summary>
        /// <value>The type of the scale.</value>
        public ScaleTypeEnum ScaleType { get; set; }
        
        public double BitmapHeight { get; set; }

        /// <summary>
        /// Gets or sets the screen coords.
        /// </summary>
        /// <value>The screen coords.</value>
        public System.Windows.Point ScreenCoords { get; set; }

        public int VelocityRange { get; set; }
        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>The maximum.</value>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public double Minimum { get; set; }

       
    }
}