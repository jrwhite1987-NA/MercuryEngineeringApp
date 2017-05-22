// ***********************************************************************
// Assembly         : Mercury
// Author           : belapurkar_s
// Created          : 06-30-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="SpectrogramSetting.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using UsbTcdLibrary;

namespace MercuryEngApp.Common
{
    /// <summary>
    /// Class SpectrogramSetting.
    /// </summary>
    public class SpectrogramSetting
    {
        #region Constant Class

        /// <summary>
        /// The value 192
        /// </summary>
        private const int VALUE_192 = 192;

        /// <summary>
        /// The value 240
        /// </summary>
        private const int VALUE_240 = 240;

        /// <summary>
        /// The value 308
        /// </summary>
        private const int VALUE_308 = 308;

        /// <summary>
        /// The value 385
        /// </summary>
        private const int VALUE_385 = 385;

        /// <summary>
        /// The value 480
        /// </summary>
        private const int VALUE_480 = 480;

        /// <summary>
        /// Class FirstOption.
        /// </summary>
        private static class FirstOption
        {
            /// <summary>
            /// The range
            /// </summary>
            public const int Range = 1;

            /// <summary>
            /// The minimum depth
            /// </summary>
            public const int MinDepth = 23;

            /// <summary>
            /// The maximum depth
            /// </summary>
            public const int MaxDepth = 52;

            /// <summary>
            /// The defaut PRF
            /// </summary>
            public const int DefautPRF = 8000;
        }

        /// <summary>
        /// Class SecondOption.
        /// </summary>
        private static class SecondOption
        {
            /// <summary>
            /// The range
            /// </summary>
            public const int Range = 2;

            /// <summary>
            /// The minimum depth
            /// </summary>
            public const int MinDepth = 53;

            /// <summary>
            /// The maximum depth
            /// </summary>
            public const int MaxDepth = 68;

            /// <summary>
            /// The defaut PRF
            /// </summary>
            public const int DefautPRF = 8000;
        }

        /// <summary>
        /// Class ThirdOption.
        /// </summary>
        private static class ThirdOption
        {
            /// <summary>
            /// The range
            /// </summary>
            public const int Range = 3;

            /// <summary>
            /// The minimum depth
            /// </summary>
            public const int MinDepth = 69;

            /// <summary>
            /// The maximum depth
            /// </summary>
            public const int MaxDepth = 87;

            /// <summary>
            /// The defaut PRF
            /// </summary>
            public const int DefautPRF = 8000;
        }

        /// <summary>
        /// Class ForthOption.
        /// </summary>
        private static class ForthOption
        {
            /// <summary>
            /// The range
            /// </summary>
            public const int Range = 4;

            /// <summary>
            /// The minimum depth
            /// </summary>
            public const int MinDepth = 88;

            /// <summary>
            /// The maximum depth
            /// </summary>
            public const int MaxDepth = 115;

            /// <summary>
            /// The defaut PRF
            /// </summary>
            public const int DefautPRF = 6250;
        }

        /// <summary>
        /// Class FifthOption.
        /// </summary>
        private static class FifthOption
        {
            /// <summary>
            /// The range
            /// </summary>
            public const int Range = 5;

            /// <summary>
            /// The minimum depth
            /// </summary>
            public const int MinDepth = 116;

            /// <summary>
            /// The maximum depth
            /// </summary>
            public const int MaxDepth = 146;

            /// <summary>
            /// The defaut PRF
            /// </summary>
            public const int DefautPRF = 5000;
        }

        #endregion Constant Class

        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>The range.</value>
        public int Range { get; set; }

        /// <summary>
        /// Gets or sets the minimum depth.
        /// </summary>
        /// <value>The minimum depth.</value>
        public int MinDepth { get; set; }

        /// <summary>
        /// Gets or sets the maximum depth.
        /// </summary>
        /// <value>The maximum depth.</value>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets the defaut PRF.
        /// </summary>
        /// <value>The defaut PRF.</value>
        public double DefautPRF { get; set; }

        /// <summary>
        /// Gets or sets the total velocity range list.
        /// </summary>
        /// <value>The total velocity range list.</value>
        public int[] TotalVelocityRangeList { get; set; }

        /// <summary>
        /// Gets or sets the current velocity range.
        /// </summary>
        /// <value>The current velocity range.</value>
        public int CurrentVelocityRange { get; set; }

        /// <summary>
        /// Gets the default value list.
        /// </summary>
        /// <value>The default value list.</value>
        private static List<SpectrogramSetting> DefaultValueList
        {
            get
            {
                return new List<SpectrogramSetting>
                    {
                        new SpectrogramSetting { Range = FirstOption.Range, MinDepth = FirstOption.MinDepth,
                            MaxDepth = FirstOption.MaxDepth, DefautPRF = FirstOption.DefautPRF,
                            TotalVelocityRangeList =  new int[] { VALUE_192, VALUE_240, VALUE_308, VALUE_385, VALUE_480 }},

                        new SpectrogramSetting { Range = SecondOption.Range, MinDepth = SecondOption.MinDepth,
                            MaxDepth = SecondOption.MaxDepth, DefautPRF = SecondOption.DefautPRF,
                            TotalVelocityRangeList = new int[] { VALUE_192, VALUE_240, VALUE_308, VALUE_385 }},

                        new SpectrogramSetting { Range = ThirdOption.Range, MinDepth = ThirdOption.MinDepth,
                            MaxDepth = ThirdOption.MaxDepth, DefautPRF = ThirdOption.DefautPRF,
                            TotalVelocityRangeList =  new int[] { VALUE_192, VALUE_240, VALUE_308 }},

                        new SpectrogramSetting { Range = ForthOption.Range, MinDepth = ForthOption.MinDepth,
                            MaxDepth = ForthOption.MaxDepth, DefautPRF = ForthOption.DefautPRF,
                            TotalVelocityRangeList =  new int[] { VALUE_192, VALUE_240 }},

                        new SpectrogramSetting { Range = FifthOption.Range, MinDepth = FifthOption.MinDepth,
                            MaxDepth = FifthOption.MaxDepth, DefautPRF = FifthOption.DefautPRF,
                            TotalVelocityRangeList =  new int[] { VALUE_192 }}
                    };
            }
        }

        /// <summary>
        /// Gets the spectrogram setting.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <returns>SpectrogramSetting.</returns>
        /// <exception cref="Exception">
        /// Value of depth can not be zero.
        /// or
        /// Provided depth is not found in default range.
        /// </exception>
        public static SpectrogramSetting GetSpectrogramSetting(int depth, bool isEmboli)
        {
            try
            {
                if (depth == 0)
                {
                    throw new Exception("Value of depth can not be zero.");
                }

                if(isEmboli)
                {
                    return new SpectrogramSetting
                    {
                        Range = ThirdOption.Range,
                        MinDepth = Constants.VALUE_23,
                        MaxDepth = Constants.VALUE_88,
                        DefautPRF = ThirdOption.DefautPRF,
                        TotalVelocityRangeList = new int[] { VALUE_308 }
                    }; 
                        
                    
                }
                return DefaultValueList
                      .Where(x => x.MinDepth <= depth && x.MaxDepth >= depth)
                      .First();
            }
            catch (Exception)
            {
                throw new Exception("Provided depth is not found in default range.");
            }
        }

        /// <summary>
        /// Gets the nearest velocity range.
        /// </summary>
        /// <param name="currentVelocityRange">The current velocity range.</param>
        /// <param name="setting">The setting.</param>
        /// <returns>System.Int32.</returns>
        public static int GetNearestVelocityRange(int currentVelocityRange, SpectrogramSetting setting)
        {
            if (setting.TotalVelocityRangeList.Contains(currentVelocityRange))
            {
                return currentVelocityRange;
            }
            else
            {
                int length = setting.TotalVelocityRangeList.Length;
                return setting.TotalVelocityRangeList[length - 1];
            }
        }

        /// <summary>
        /// Gets the indexfor baseline.
        /// </summary>
        /// <param name="baseLinePosition">The base line position.</param>
        /// <returns>System.Int32.</returns>
        public static int GetIndexforBaseline(int baseLinePosition)
        {
            const int DEFAULT_BASE_VALUE = 0;

            if (baseLinePosition < Constants.DefaultBaseline &&
                baseLinePosition >= -Constants.DefaultBaseline && baseLinePosition != DEFAULT_BASE_VALUE)
            {
                if (baseLinePosition < Constants.VALUE_0)
                {
                    return DMIProtocol.FFTSize + baseLinePosition;
                }
                return baseLinePosition;
            }
            return DEFAULT_BASE_VALUE;
        }
    }

    /// <summary>
    /// Class PRFOptions.
    /// </summary>
    public class PRFOptions
    {
        #region Constant Class

        /// <summary>
        /// Class FirstOption.
        /// </summary>
        public static class FirstOption
        {
            /// <summary>
            /// The PRF
            /// </summary>
            public const int PRF = 5000;

            /// <summary>
            /// The default velocity range
            /// </summary>
            public const int DefaultVelocityRange = 192;
        }

        /// <summary>
        /// Class SecondOption.
        /// </summary>
        public static class SecondOption
        {
            /// <summary>
            /// The PRF
            /// </summary>
            public const int PRF = 6250;

            /// <summary>
            /// The default velocity range
            /// </summary>
            public const int DefaultVelocityRange = 240;
        }

        /// <summary>
        /// Class ThirdOption.
        /// </summary>
        public static class ThirdOption
        {
            /// <summary>
            /// The PRF
            /// </summary>
            public const int PRF = 8000;

            /// <summary>
            /// The default velocity range
            /// </summary>
            public const int DefaultVelocityRange = 308;
        }

        /// <summary>
        /// Class ForthOption.
        /// </summary>
        public static class ForthOption
        {
            /// <summary>
            /// The PRF
            /// </summary>
            public const int PRF = 10000;

            /// <summary>
            /// The default velocity range
            /// </summary>
            public const int DefaultVelocityRange = 385;
        }

        /// <summary>
        /// Class FifthOption.
        /// </summary>
        public static class FifthOption
        {
            /// <summary>
            /// The PRF
            /// </summary>
            public const int PRF = 12500;

            /// <summary>
            /// The default velocity range
            /// </summary>
            public const int DefaultVelocityRange = 480;
        }

        #endregion Constant Class

        /// <summary>
        /// Gets or sets the PRF.
        /// </summary>
        /// <value>The PRF.</value>
        public uint PRF { get; set; }

        /// <summary>
        /// Gets or sets the default velocity range.
        /// </summary>
        /// <value>The default velocity range.</value>
        public int DefaultVelocityRange { get; set; }

        /// <summary>
        /// Gets the PRF options list.
        /// </summary>
        /// <value>The PRF options list.</value>
        public static List<PRFOptions> PRFOptionsList
        {
            get
            {
                return new List<PRFOptions>
                    {
                        new PRFOptions { PRF = FirstOption.PRF , DefaultVelocityRange = FirstOption.DefaultVelocityRange },
                        new PRFOptions { PRF = SecondOption.PRF , DefaultVelocityRange = SecondOption.DefaultVelocityRange },
                        new PRFOptions { PRF = ThirdOption.PRF , DefaultVelocityRange = ThirdOption.DefaultVelocityRange },
                        new PRFOptions { PRF = ForthOption.PRF , DefaultVelocityRange = ForthOption.DefaultVelocityRange },
                        new PRFOptions { PRF = FifthOption.PRF , DefaultVelocityRange = FifthOption.DefaultVelocityRange },
                    };
            }
        }

        /// <summary>
        /// Gets the total velocity range.
        /// </summary>
        /// <param name="PRF">The PRF.</param>
        /// <returns>System.Int32.</returns>
        /// <exception cref="Exception">Value of PRF can not be zero.</exception>
        public static int GetTotalVelocityRange(double PRF)
        {
            if ((int)PRF == 0)
            {
                throw new Exception("Value of PRF can not be zero.");
            }

            var totalVelocityRange = PRFOptionsList.Where(x => x.PRF == PRF).Select(x => x.DefaultVelocityRange).First();
            return totalVelocityRange;
        }

        /// <summary>
        /// Gets the PRF.
        /// </summary>
        /// <param name="velocityRange">The velocity range.</param>
        /// <returns>System.UInt32.</returns>
        /// <exception cref="Exception">Value of velocityRange can not be zero.</exception>
        public static uint GetPRF(int velocityRange)
        {
            if (velocityRange == 0)
            {
                throw new Exception("Value of velocityRange can not be zero.");
            }

            var prf = PRFOptionsList.Where(x => x.DefaultVelocityRange == velocityRange)
                .Select(x => x.PRF)
                .First();
            return prf;
        }
    }

    /// <summary>
    /// Class VelocityRange.
    /// </summary>
    public class VelocityRange
    {
        /// <summary>
        /// Gets or sets the minimum velocity display.
        /// </summary>
        /// <value>The minimum velocity display.</value>
        public double MinVelocityDisplay { get; set; }

        /// <summary>
        /// Gets or sets the maximum velocity display.
        /// </summary>
        /// <value>The maximum velocity display.</value>
        public double MaxVelocityDisplay { get; set; }

        /// <summary>
        /// Gets the velocity range.
        /// </summary>
        /// <param name="totalvelocityRange">The totalvelocity range.</param>
        /// <returns>VelocityRange.</returns>
        /// <exception cref="Exception">Value of totalvelocityRange can not be zero.</exception>
        public static VelocityRange GetVelocityRange(int totalvelocityRange)
        {
            const int DIVIDE_VELOCITY_RANGE = 2;
            const int DEFAULT_VELOCITY_RANGE = 0;
            if (totalvelocityRange == DEFAULT_VELOCITY_RANGE)
            {
                throw new Exception("Value of totalvelocityRange can not be zero.");
            }

            int number = totalvelocityRange / DIVIDE_VELOCITY_RANGE;

            return new VelocityRange { MaxVelocityDisplay = number, MinVelocityDisplay = -number };
        }
    }
}