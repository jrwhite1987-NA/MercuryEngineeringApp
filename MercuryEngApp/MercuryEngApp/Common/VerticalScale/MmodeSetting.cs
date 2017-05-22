using Core.Constants;
// ***********************************************************************
// Assembly         : Mercury
// Author           : belapurkar_s
// Created          : 08-11-2016
//
// Last Modified By : belapurkar_s
// Last Modified On : 08-11-2016
// ***********************************************************************
// <copyright file="MmodeSetting.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace MercuryEngApp.Common
{
    /// <summary>
    /// Class MmodeSetting.
    /// </summary>
    public class MmodeSetting
    {
        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <value>The range.</value>
        public double PRF { get; set; }

        /// <summary>
        /// Gets or sets the minimum depth display.
        /// </summary>
        /// <value>The minimum depth display.</value>
        public byte MinDepthDisplay { get; set; }

        /// <summary>
        /// Gets or sets the maximum depth display.
        /// </summary>
        /// <value>The maximum depth display.</value>
        public byte MaxDepthDisplay { get; set; }

        /// <summary>
        /// Gets or sets the minimum depth current.
        /// </summary>
        /// <value>The minimum depth current.</value>
        public byte MinDepthCurrent { get; set; }

        /// <summary>
        /// Gets or sets the maximum depth current.
        /// </summary>
        /// <value>The maximum depth current.</value>
        public byte MaxDepthCurrent { get; set; }

        private static List<MmodeSetting> MmodeRangeCollection
        {
            get
            {
                return new List<MmodeSetting>
                {
                    new MmodeSetting{ PRF = Constants.VALUE_5000, MinDepthDisplay = Constants.VALUE_23 , MaxDepthDisplay = 
                                      Constants.VALUE_87, MinDepthCurrent = Constants.VALUE_23, MaxDepthCurrent = Constants.VALUE_72},
                    new MmodeSetting{ PRF = Constants.VALUE_5000, MinDepthDisplay = Constants.VALUE_56 , MaxDepthDisplay = 
                                      Constants.VALUE_120, MinDepthCurrent = Constants.VALUE_73, MaxDepthCurrent = Constants.VALUE_104},
                    new MmodeSetting{ PRF = Constants.VALUE_5000, MinDepthDisplay = Constants.VALUE_81 , MaxDepthDisplay = 
                                      Constants.VALUE_146, MinDepthCurrent = Constants.VALUE_105, MaxDepthCurrent = Constants.VALUE_146},
                    new MmodeSetting{ PRF = Constants.VALUE_6250, MinDepthDisplay = Constants.VALUE_23 , MaxDepthDisplay = 
                                      Constants.VALUE_87, MinDepthCurrent = Constants.VALUE_23, MaxDepthCurrent = Constants.VALUE_72},
                    new MmodeSetting{ PRF = Constants.VALUE_6250, MinDepthDisplay = Constants.VALUE_50 , MaxDepthDisplay = 
                                      Constants.VALUE_115, MinDepthCurrent = Constants.VALUE_73, MaxDepthCurrent = Constants.VALUE_115},
                    new MmodeSetting{ PRF = Constants.VALUE_8000, MinDepthDisplay = Constants.VALUE_23 , MaxDepthDisplay = 
                                      Constants.VALUE_80, MinDepthCurrent = Constants.VALUE_23, MaxDepthCurrent = Constants.VALUE_34},
                    new MmodeSetting{ PRF = Constants.VALUE_8000, MinDepthDisplay = Constants.VALUE_30 , MaxDepthDisplay = 
                                      Constants.VALUE_88, MinDepthCurrent = Constants.VALUE_35, MaxDepthCurrent = Constants.VALUE_87},
                    new MmodeSetting{ PRF = Constants.VALUE_10000, MinDepthDisplay = Constants.VALUE_23 , MaxDepthDisplay = 
                                      Constants.VALUE_68, MinDepthCurrent = Constants.VALUE_23, MaxDepthCurrent = Constants.VALUE_68},
                    new MmodeSetting{ PRF = Constants.VALUE_12500, MinDepthDisplay = Constants.VALUE_23 , MaxDepthDisplay = 
                                      Constants.VALUE_52, MinDepthCurrent = Constants.VALUE_23, MaxDepthCurrent = Constants.VALUE_52}
                };
            }
        }

        /// <summary>
        /// Gets the depth range.
        /// </summary>
        /// <param name="depth">The depth.</param>
        /// <returns>MmodeSetting.</returns>
        /// <exception cref="Exception">
        /// Value of depth can not be zero.
        /// or
        /// Provided depth is not found in default range.
        /// </exception>
        public static MmodeSetting GetDepthRange(int depth, double prf, bool isEmboliExam)
        {
            try
            {
                if (depth == Constants.VALUE_0)
                {
                    throw new Exception("Value of depth can not be zero.");
                }

                if(isEmboliExam)
                {
                    return new MmodeSetting { PRF = Constants.VALUE_8000, MinDepthDisplay = Constants.VALUE_23, MaxDepthDisplay = Constants.VALUE_88, MinDepthCurrent = Constants.VALUE_23, MaxDepthCurrent = Constants.VALUE_88 };
                }

                MmodeSetting mModeSetting = MmodeSetting.MmodeRangeCollection
                    .Where(x => depth >= x.MinDepthCurrent && depth <= x.MaxDepthCurrent && prf == x.PRF)
                    .OrderByDescending(k => k.MinDepthDisplay)
                    .FirstOrDefault();

                while (mModeSetting == null)
                {
                    int index = PRFOptions.PRFOptionsList.FindIndex(x => x.PRF == prf);
                    if (index != Constants.VALUE_0)
                    {
                        PRFOptions prfOption = PRFOptions.PRFOptionsList.ElementAt(index - Constants.VALUE_1);

                        mModeSetting = MmodeSetting.MmodeRangeCollection
                            .Where(x => depth >= x.MinDepthCurrent && depth <= x.MaxDepthCurrent && prfOption.PRF == x.PRF)
                            .OrderByDescending(k => k.MinDepthDisplay)
                            .FirstOrDefault();
                    }
                }

                return mModeSetting;
            }
            catch (Exception)
            {
                throw new Exception("Provided depth is not found in default range.");
            }
        }
    }
}