using System.Collections.Generic;

namespace Core.Constants
{
    /// <summary>
    /// Mapping of power value and its display value
    /// </summary>
   public static class AccousticPowerMapping
   {
       /// <summary>
       /// Gets or sets the lookup table.
       /// </summary>
       /// <value>
       /// The lookup table.
       /// </value>
       public static System.Collections.Generic.Dictionary<uint,uint> LookupTable { get; set; }
       /// <summary>
       /// Initializes the <see cref="AccousticPowerMapping" /> class.
       /// </summary>
       static AccousticPowerMapping()
       {
           LookupTable = new Dictionary<uint, uint>();
           LookupTable.Add(Constants.DISPLAY_POWER_0, Constants.ACTUAL_POWER_0);
           LookupTable.Add(Constants.DISPLAY_POWER_1, Constants.ACTUAL_POWER_1);
           LookupTable.Add(Constants.DISPLAY_POWER_2, Constants.ACTUAL_POWER_2);
           LookupTable.Add(Constants.DISPLAY_POWER_5, Constants.ACTUAL_POWER_5);
           LookupTable.Add(Constants.DISPLAY_POWER_10, Constants.ACTUAL_POWER_9);
           LookupTable.Add(Constants.DISPLAY_POWER_20, Constants.ACTUAL_POWER_18);
           LookupTable.Add(Constants.DISPLAY_POWER_30, Constants.ACTUAL_POWER_27);
           LookupTable.Add(Constants.DISPLAY_POWER_40, Constants.ACTUAL_POWER_36);
           LookupTable.Add(Constants.DISPLAY_POWER_50, Constants.ACTUAL_POWER_45);
           LookupTable.Add(Constants.DISPLAY_POWER_60, Constants.ACTUAL_POWER_54);
           LookupTable.Add(Constants.DISPLAY_POWER_70, Constants.ACTUAL_POWER_63);
           LookupTable.Add(Constants.DISPLAY_POWER_80, Constants.ACTUAL_POWER_72);
           LookupTable.Add(Constants.DISPLAY_POWER_90, Constants.ACTUAL_POWER_81);
           LookupTable.Add(Constants.DISPLAY_POWER_100, Constants.ACTUAL_POWER_90);
       }
    }
}