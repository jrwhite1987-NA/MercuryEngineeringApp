// ***********************************************************************
// Assembly         : Core
// Author           : belapurkar_s
// Created          : 05-24-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 07-28-2016
// ***********************************************************************
// <copyright file="RegionalSettings.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Core.Common
{
    /// <summary>
    /// Class RegionalSettings.
    /// </summary>
    public class RegionalSettings
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static RegionalSettings _instance = null;

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>The date format.</value>
        public static string DateFormat { get; set; }

        /// <summary>
        /// Gets of sets the page size format
        /// </summary>
        public static string PDFPageFormat { get; set; }

        /// <summary>
        /// Gets or sets the time format.
        /// </summary>
        /// <value>The time format.</value>
        public static string TimeFormat { get; set; }

        /// <summary>
        /// Gets or sets the date time format.
        /// </summary>
        /// <value>The date time format.</value>
        public static string DateTimeFormat { get; set; }

        /// <summary>
        /// Gets or sets the number format.
        /// </summary>
        /// <value>The number format.</value>
        public static CultureInfo NumberFormat { get; set; }

        public static TimeSpan DateTimeOffset { get; set; }

        //Set the values of below variables as per culture / Settings
        /// <summary>
        /// The value of group seperator
        /// </summary>
        public string valueOfGroupSeperator;

        /// <summary>
        /// The value of decimal seperator
        /// </summary>
        public string valueOfDecimalSeperator;

        //Set value of this for number formatting

        /// <summary>
        /// The group sizes
        /// </summary>
        public int[] GroupSizes = new int[] { 3, 3, 3 };

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static RegionalSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RegionalSettings();
                }
                return _instance;
            }
        }
    }

    //Need this class to move in a separate file where date format should come from Settings.
    /// <summary>
    /// Class DateFormatConverter.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Data.IValueConverter" />
    public class DateFormatConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the date time format.
        /// </summary>
        /// <value>The date time format.</value>
        public static string DateTimeFormat { get; set; }

        /// <summary>
        /// Gets or sets the date format.
        /// </summary>
        /// <value>The date format.</value>
        public static string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets the time format.
        /// </summary>
        /// <value>The time format.</value>
        public static string TimeFormat { get; set; }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property.
        /// This uses a different type depending on whether you're programming with Microsoft .NET or Visual C++ component
        /// extensions (C++/CX). See Remarks.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Helper.logger.Debug("Method execution begins to convert datetime to string.");
            string returnString = string.Empty;
            if (value != null)
            {
                Helper.logger.Debug("Parse date from string.");
                DateTime date = DateTime.Parse(value.ToString());
                if (date == new DateTime())
                {
                    Helper.logger.Debug("Invalid date string.Return empty string.");
                    return returnString;
                }
                Helper.logger.Debug("Method executed.Return date as string");
                return date.ToString(parameter != null && parameter.ToString().Contains("Time") ?
                    RegionalSettings.DateTimeFormat : RegionalSettings.DateFormat, CultureInfo.InvariantCulture);
            }
            Helper.logger.Debug("Null object.Return empty string.");
            return returnString;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in TwoWay bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property,
        /// specified by a helper structure that wraps the type name.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}