using Core.Common;
using Core.Constants;
using System;
using Windows.UI;

namespace PlottingLib
{
    /// <summary>
    /// Class DefaultValue.
    /// </summary>
    public static class ColorCode
    {
        /// <summary>
        /// The value 100
        /// </summary>
        public const int VALUE_100 = 100;

        /// <summary>
        /// The value 240
        /// </summary>
        public const int VALUE_240 = 240;

        /// <summary>
        /// The value 340
        /// </summary>
        public const int VALUE_340 = 340;

        /// <summary>
        /// The value 50
        /// </summary>
        public const int VALUE_50 = 50;

        /// <summary>
        /// The value 290
        /// </summary>
        public const int VALUE_290 = 290;

        /// <summary>
        /// The value 0
        /// </summary>
        public const int VALUE_0 = 0;

        /// <summary>
        /// The value 1
        /// </summary>
        public const int VALUE_1 = 1;

        /// <summary>
        /// The value 255
        /// </summary>
        public const byte VALUE_255 = 255;
    }

    public static class Defaults
    {
        /// <summary>
        /// The value 240
        /// </summary>
        public const int VALUE_240 = 240;

        /// <summary>
        /// The value 255 d
        /// </summary>
        public const double VALUE_255D = 255d;

        /// <summary>
        /// The value 60
        /// </summary>
        public const int VALUE_60 = 60;

        /// <summary>
        /// The value 6 d
        /// </summary>
        public const double VALUE_6D = 6.0d;

        /// <summary>
        /// The value 0 5 d
        /// </summary>
        public const double VALUE_0_5D = 0.5d;

        /// <summary>
        /// The value 2 d
        /// </summary>
        public const double VALUE_2D = 2.0d;

        /// <summary>
        /// The value 1 d
        /// </summary>
        public const double VALUE_1D = 1.0d;

        /// <summary>
        /// The value 3 d
        /// </summary>
        public const double VALUE_3D = 3.0d;

        /// <summary>
        /// The value 255
        /// </summary>
        public const byte VALUE_255 = 255;

        /// <summary>
        /// The value 34 d
        /// </summary>
        public const double VALUE_34D = 34.0d;

        /// <summary>
        /// The value 10 d
        /// </summary>
        public const double VALUE_10D = 10.0d;

        /// <summary>
        /// The value 10
        /// </summary>
        public const int VALUE_10 = 10;

        /// <summary>
        /// The value 1100
        /// </summary>
        public const int VALUE_1100 = 1100;

        /// <summary>
        /// The value 34
        /// </summary>
        public const int VALUE_34 = 34;

        public const int VALUE_340 = 340;

        /// <summary>
        /// The value 1000
        /// </summary>
        public const int VALUE_1000 = 1000;

        /// <summary>
        /// The value 1340
        /// </summary>
        public const int VALUE_1340 = 1340;

        public const int MMODE_BLUE_RANGE_START = -1400;
        public const int MMODE_BLUE_RANGE_END = 32167;
        public const int MMODE_RED_RANGE_START = -33368;
        public const int MMODE_RED_RANGE_END = 1400;
        public const int SPECTRUM_RANGE_START = -34168;
        public const int SPECTRUM_RANGE_END = 32167;
        public const int MMODE_RANGE = 700;
    }

    /// <summary>
    /// Class ColorPalatte.
    /// </summary>
    internal class ColorPalatte
    {
        /// <summary>
        /// The color palatte1
        /// </summary>
        public System.Collections.Generic.Dictionary<Int32, Color> colorPalatte1 = new System.Collections.Generic.Dictionary<Int32, Color>();

        /// <summary>
        /// The color palatte2
        /// </summary>
        public System.Collections.Generic.Dictionary<Int32, Color> colorPalatte2 = new System.Collections.Generic.Dictionary<Int32, Color>();

        /// <summary>
        /// The color palatte m mmode
        /// </summary>
        public System.Collections.Generic.Dictionary<Int32, Color> colorPalatteMMmodeRed = new System.Collections.Generic.Dictionary<Int32, Color>();

        public System.Collections.Generic.Dictionary<Int32, Color> colorPalatteMMmodeBlue = new System.Collections.Generic.Dictionary<Int32, Color>();

        internal static int MmodeRange;

        /// <summary>
        /// The palatte
        /// </summary>
        private static ColorPalatte palatte;

        /// <summary>
        /// The singleton lock
        /// </summary>
        private static Object singletonLock = new Object();

        /// <summary>
        /// Gets the dictionaries.
        /// </summary>
        /// <value>The dictionaries.</value>
        public static ColorPalatte Dictionaries
        {
            get
            {
                lock (singletonLock)
                {
                    if (palatte == null)
                    {
                        palatte = new ColorPalatte();
                    }
                }
                return palatte;
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ColorPalatte"/> class from being created.
        /// </summary>
        private ColorPalatte()
        {
            FillSpectrumColorMap();

            FillMmodeColorMap();
        }

        private void FillMmodeColorMap()
        {
            FillMModeBluePalatte();
            FillMModeRedPalatte();
        }

        private void FillMModeRedPalatte()
        {
            try
            {                
                if (colorPalatteMMmodeRed.Count == ColorCode.VALUE_0)
                {
                    for (int i = Defaults.MMODE_RED_RANGE_START; i <= Defaults.MMODE_RED_RANGE_END; i++)
                    {
                        if (i <= ColorCode.VALUE_0 && i >= -MmodeRange)
                        {
                            colorPalatteMMmodeRed.Add(i, ToRGB(ColorCode.VALUE_0, ColorCode.VALUE_100, (-i * ColorCode.VALUE_100*1.0 / MmodeRange)));
                        }
                        else if (i < -MmodeRange)
                        {
                            colorPalatteMMmodeRed.Add(i, Colors.White);
                        }
                        else
                        {
                            colorPalatteMMmodeRed.Add(i, Colors.Black);
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillMModeBluePalatte()
        {
            try
            {               
                if (colorPalatteMMmodeBlue.Count == ColorCode.VALUE_0)
                {
                    for (int i = Defaults.MMODE_BLUE_RANGE_START; i <= Defaults.MMODE_BLUE_RANGE_END; i++)
                    {
                        if (i >= ColorCode.VALUE_0 && i <= MmodeRange)
                        {
                            colorPalatteMMmodeBlue.Add(i, ToRGB(ColorCode.VALUE_240, ColorCode.VALUE_100, (i * ColorCode.VALUE_100*1.0 / MmodeRange)));
                        }
                        else if (i > MmodeRange)
                        {
                            colorPalatteMMmodeBlue.Add(i, Colors.White);
                        }
                        else
                        {
                            colorPalatteMMmodeBlue.Add(i, Colors.Black);
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillSpectrumColorMap()
        {
            try
            {                
                if (colorPalatte1.Count == ColorCode.VALUE_0)
                {
                    for (int i = Defaults.SPECTRUM_RANGE_START; i <= Defaults.SPECTRUM_RANGE_END; i++)
                    {
                        if (i >= ColorCode.VALUE_0 && i < Defaults.VALUE_340)
                        {
                            if (i <= ColorCode.VALUE_50)
                            {
                                colorPalatte1.Add(i, ToRGB(ColorCode.VALUE_240, ColorCode.VALUE_100, i));
                            }
                            else
                            {
                                if (i <= ColorCode.VALUE_290)
                                {
                                    colorPalatte1.Add(i, ToRGB(ColorCode.VALUE_290 - i, ColorCode.VALUE_100, ColorCode.VALUE_50));
                                }
                                else
                                {
                                    colorPalatte1.Add(i, ToRGB(ColorCode.VALUE_0, ColorCode.VALUE_100, i - ColorCode.VALUE_240));
                                }
                            }
                        }
                        else if (i < ColorCode.VALUE_0)
                        {
                            colorPalatte1.Add(i, Colors.Black);
                        }
                        else if (i >= Defaults.VALUE_340)
                        {
                            colorPalatte1.Add(i, Colors.White);
                        }
                       
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To the RGB.
        /// </summary>
        /// <param name="Hue">The hue.</param>
        /// <param name="Saturation">The saturation.</param>
        /// <param name="Luminosity">The luminosity.</param>
        /// <returns>Color.</returns>
        private Color ToRGB(double Hue, double Saturation, double Luminosity)
        {
            byte r;
            byte g;
            byte b;

            try
            {                
                double tempHue = Hue / Defaults.VALUE_60;
                double temSaturation = Saturation / ColorCode.VALUE_100;
                double temLuminosity = Luminosity / ColorCode.VALUE_100;

                if ((int)temSaturation == ColorCode.VALUE_0)
                {
                    r = (byte)Math.Round(temLuminosity * Defaults.VALUE_255D);
                    g = (byte)Math.Round(temLuminosity * Defaults.VALUE_255D);
                    b = (byte)Math.Round(temLuminosity * Defaults.VALUE_255D);
                }
                else
                {
                    double t1, t2;
                    double th = tempHue / Defaults.VALUE_6D;

                    if (temLuminosity < Defaults.VALUE_0_5D)
                    {
                        t2 = temLuminosity * (Defaults.VALUE_1D + temSaturation);
                    }
                    else
                    {
                        t2 = (temLuminosity + temSaturation) - (temLuminosity * temSaturation);
                    }
                    t1 = Defaults.VALUE_2D * temLuminosity - t2;

                    double tr, tg, tb;
                    tr = th + (Defaults.VALUE_1D / Defaults.VALUE_3D);
                    tg = th;
                    tb = th - (Defaults.VALUE_1D / Defaults.VALUE_3D);

                    tr = ColorCalc(tr, t1, t2);
                    tg = ColorCalc(tg, t1, t2);
                    tb = ColorCalc(tb, t1, t2);
                    r = (byte)Math.Round(tr * Defaults.VALUE_255D);
                    g = (byte)Math.Round(tg * Defaults.VALUE_255D);
                    b = (byte)Math.Round(tb * Defaults.VALUE_255D);
                }
               
                return Color.FromArgb(ColorCode.VALUE_255, r, g, b);
            }
            catch (Exception ex)
            {               
                return Colors.Black;
            }
        }

        /// <summary>
        /// Colors the calculate.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="t1">The t1.</param>
        /// <param name="t2">The t2.</param>
        /// <returns>System.Double.</returns>
        private double ColorCalc(double c, double t1, double t2)
        {
            try
            {               
                double tempc = c;
                if (tempc < ColorCode.VALUE_0) { tempc += Defaults.VALUE_1D; }
                if (tempc > ColorCode.VALUE_1) { tempc -= Defaults.VALUE_1D; }
                if (Defaults.VALUE_6D * tempc < Defaults.VALUE_1D) { return t1 + (t2 - t1) * Defaults.VALUE_6D * tempc; }
                if (Defaults.VALUE_2D * tempc < Defaults.VALUE_1D) { return t2; }
                if (Defaults.VALUE_3D * tempc < Defaults.VALUE_2D)
                {
                    return t1 + (t2 - t1) *
                    (Defaults.VALUE_2D / Defaults.VALUE_3D - tempc) * Defaults.VALUE_6D;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return t1;
        }
    }

    /// <summary>
    /// Class ExamColorMap.
    /// </summary>
    public class ExamColorMap
    {
        /// <summary>
        /// The value left spectrum
        /// </summary>
        private int valueLeftSpectrum = 0;

        /// <summary>
        /// The value left mmode
        /// </summary>
        private int valueLeftMmode = 0;

        /// <summary>
        /// The value right spectrum
        /// </summary>
        private int valueRightSpectrum = 0;

        /// <summary>
        /// The value right mmode
        /// </summary>
        private int valueRightMmode = 0;

        /// <summary>
        /// The adjust factor spectrogram
        /// </summary>
        private double AdjustFactorSpectrogram;

        /// <summary>
        /// The adjust factor mmode
        /// </summary>
        private double AdjustFactorMmode;

        /// <summary>
        /// The range spectrogram
        /// </summary>
        private double _rangeSpectrogram;

        /// <summary>
        /// The range mmode
        /// </summary>
        private double _rangeMmode;

        /// <summary>
        /// Gets or sets the range spectrogram.
        /// </summary>
        /// <value>The range spectrogram.</value>
        public double RangeSpectrogram
        {
            get { return _rangeSpectrogram; }
            set
            {
                _rangeSpectrogram = value;
                AdjustFactorSpectrogram = Defaults.VALUE_34D / value;
            }
        }

        /// <summary>
        /// Gets or sets the range mmode.
        /// </summary>
        /// <value>The range mmode.</value>
        public double RangeMmode
        {
            get { return _rangeMmode; }
            set
            {
                _rangeMmode = value;
                AdjustFactorMmode = Defaults.VALUE_10D / value;
            }
        }

        /// <summary>
        /// The current lower limit channel1
        /// </summary>
        internal double _currentLowerLimitChannel1;

        /// <summary>
        /// The current lower limit channel2
        /// </summary>
        internal double _currentLowerLimitChannel2;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamColorMap"/> class.
        /// </summary>
        public ExamColorMap(int mmodeRange = Defaults.MMODE_RANGE)
        {
            ColorPalatte.MmodeRange = mmodeRange;
            RangeMmode = Defaults.VALUE_10;
            RangeSpectrogram = Defaults.VALUE_34;
            _currentLowerLimitChannel1 = Defaults.VALUE_1000;
            _currentLowerLimitChannel2 = Defaults.VALUE_1000;
        }

        #region Methods

        /// <summary>
        /// Gets the spectrogram color channel1.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>Color.</returns>
        public Color GetSpectrogramColorChannel1(short p)
        {
            try
            {
                valueLeftSpectrum = (int)Math.Round((p - _currentLowerLimitChannel1) * AdjustFactorSpectrogram);
                return ColorPalatte.Dictionaries.colorPalatte1[valueLeftSpectrum];
            }
            catch
            {
                return Colors.Black;
            }
        }

        /// <summary>
        /// Gets the spectrogram color channel2.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>Color.</returns>
        public Color GetSpectrogramColorChannel2(short p)
        {
            try
            {
                valueRightSpectrum = (int)Math.Round((p - _currentLowerLimitChannel2) * AdjustFactorSpectrogram);
                return ColorPalatte.Dictionaries.colorPalatte1[valueRightSpectrum];
            }
            catch
            {
                return Colors.Black;
            }
        }

        /// <summary>
        /// Gets the mmode color channel1.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>Color.</returns>
        public Color GetMmodeColorChannel1(short p)
        {
            try
            {
                if (p > ColorCode.VALUE_0)
                {
                    valueLeftMmode = (int)Math.Round((p - _currentLowerLimitChannel1) * AdjustFactorMmode);
                    return ColorPalatte.Dictionaries.colorPalatteMMmodeBlue[valueLeftMmode];
                }
                else
                {
                    valueLeftMmode = (int)Math.Round((p + _currentLowerLimitChannel1) * AdjustFactorMmode);
                    return ColorPalatte.Dictionaries.colorPalatteMMmodeRed[valueLeftMmode];
                }
            }
            catch
            {
                return Colors.Black;
            }
        }

        /// <summary>
        /// Gets the mmode color channel2.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>Color.</returns>
        public Color GetMmodeColorChannel2(short p)
        {
            try
            {
                if (p > ColorCode.VALUE_0)
                {
                    valueRightMmode = (int)Math.Round((p - _currentLowerLimitChannel2) * AdjustFactorMmode);
                    return ColorPalatte.Dictionaries.colorPalatteMMmodeBlue[valueRightMmode];
                }
                else
                {
                    valueRightMmode = (int)Math.Round((p + _currentLowerLimitChannel2) * AdjustFactorMmode);
                    return ColorPalatte.Dictionaries.colorPalatteMMmodeRed[valueRightMmode];
                }
            }
            catch
            {
                return Colors.Black;
            }
        }

        /// <summary>
        /// Set the lower limit value from gain
        /// </summary>
        /// <param name="gain"></param>
        public void SetLowerLimitFromGain(float gain)
        {
            //Set only for channel 1 as only channel 1 is used for plotting the snapshot data
            _currentLowerLimitChannel1 = Defaults.VALUE_1000 - (Defaults.VALUE_10 * gain);
        }

        #endregion Methods
    }

    /// <summary>
    /// Class SnapshotColorMap.
    /// </summary>
    internal class SnapshotColorMap
    {
        /// <summary>
        /// The snapshot lower limit
        /// </summary>
        private double _snapshotLowerLimit;

        /// <summary>
        /// The value spectrum
        /// </summary>
        private int valueSpectrum = 0;

        /// <summary>
        /// The value mmode
        /// </summary>
        private int valueMmode = 0;

        /// <summary>
        /// The adjust factor spectrogram
        /// </summary>
        private double AdjustFactorSpectrogram;

        /// <summary>
        /// The adjust factor mmode
        /// </summary>
        private double AdjustFactorMmode;

        /// <summary>
        /// The range spectrogram
        /// </summary>
        private double _rangeSpectrogram;

        /// <summary>
        /// The range mmode
        /// </summary>
        private double _rangeMmode;

        /// <summary>
        /// Gets or sets the range spectrogram.
        /// </summary>
        /// <value>The range spectrogram.</value>
        public double RangeSpectrogram
        {
            get { return _rangeSpectrogram; }
            set
            {
                _rangeSpectrogram = value;
                AdjustFactorSpectrogram = Defaults.VALUE_34D / value;
            }
        }

        /// <summary>
        /// Gets or sets the range mmode.
        /// </summary>
        /// <value>The range mmode.</value>
        public double RangeMmode
        {
            get { return _rangeMmode; }
            set
            {
                _rangeMmode = value;
                AdjustFactorMmode = Defaults.VALUE_10D / value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotColorMap"/> class.
        /// </summary>
        public SnapshotColorMap()
        {
            RangeMmode = Defaults.VALUE_10;
            RangeSpectrogram = Defaults.VALUE_34;
        }

        /// <summary>
        /// Gets the color of the snap spectrum.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>Color.</returns>
        public Color GetSnapSpectrumColor(int p)
        {
            try
            {
                valueSpectrum = (int)Math.Round((p - _snapshotLowerLimit) * AdjustFactorSpectrogram);
                return ColorPalatte.Dictionaries.colorPalatte1[valueSpectrum];
            }
            catch
            {
                return Colors.Black;
            }
        }

        /// <summary>
        /// Gets the color of the snap mmode.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>Color.</returns>
        public Color GetSnapMmodeColor(int p)
        {
            try
            {
                if (p > ColorCode.VALUE_0)
                {
                    valueMmode = (int)Math.Round((p - _snapshotLowerLimit) * AdjustFactorMmode);
                    return ColorPalatte.Dictionaries.colorPalatteMMmodeBlue[valueMmode];
                }
                else
                {
                    valueMmode = (int)Math.Round((p + _snapshotLowerLimit) * AdjustFactorMmode);
                    return ColorPalatte.Dictionaries.colorPalatteMMmodeRed[valueMmode];
                }
            }
            catch
            {
                return Colors.Black;
            }
        }

        /// <summary>
        /// Sets the gain.
        /// </summary>
        /// <param name="gain">The gain.</param>
        public void SetGain(float gain)
        {
            _snapshotLowerLimit = Defaults.VALUE_1000 - (Defaults.VALUE_10 * gain);
        }
    }
}