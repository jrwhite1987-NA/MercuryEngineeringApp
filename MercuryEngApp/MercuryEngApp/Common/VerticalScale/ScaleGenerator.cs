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
    /// Class ScaleGenerator.
    /// </summary>
    //public class ScaleGenerator
    //{
    //    /// <summary>
    //    /// Class ScaleValue.
    //    /// </summary>
    //    private static class ScaleValue
    //    {
    //        /// <summary>
    //        /// The value 102
    //        /// </summary>
    //        public const int VALUE_102 = 102;

    //        /// <summary>
    //        /// The value 90
    //        /// </summary>
    //        public const int VALUE_90 = 90;

    //        /// <summary>
    //        /// The value 70
    //        /// </summary>
    //        public const int VALUE_70 = 70;

    //        /// <summary>
    //        /// The value 60
    //        /// </summary>
    //        public const int VALUE_60 = 60;

    //        /// <summary>
    //        /// The value 0
    //        /// </summary>
    //        public const int VALUE_0 = 0;

    //        /// <summary>
    //        /// The value 30
    //        /// </summary>
    //        public const int VALUE_30 = 30;

    //        /// <summary>
    //        /// The value 40
    //        /// </summary>
    //        public const int VALUE_40 = 40;

    //        /// <summary>
    //        /// The value 10
    //        /// </summary>
    //        public const int VALUE_10 = 10;

    //        /// <summary>
    //        /// The value 11
    //        /// </summary>
    //        public const int VALUE_11 = 11;

    //        /// <summary>
    //        /// The value 20
    //        /// </summary>
    //        public const int VALUE_20 = 20;

    //        /// <summary>
    //        /// The value 151
    //        /// </summary>
    //        public const int VALUE_151 = 151;

    //        /// <summary>
    //        /// The value 190
    //        /// </summary>
    //        public const int VALUE_200 = 200;

    //        /// <summary>
    //        /// The value 23
    //        /// </summary>
    //        public const int VALUE_23 = 23;

    //        /// <summary>
    //        /// The value 203
    //        /// </summary>
    //        public const int VALUE_203 = 203;

    //        /// <summary>
    //        /// The value 15
    //        /// </summary>
    //        public const int VALUE_15 = 15;

    //        /// <summary>
    //        /// The value 196
    //        /// </summary>
    //        public const int VALUE_176 = 176;

    //        /// <summary>
    //        /// The value 3
    //        /// </summary>
    //        public const int VALUE_3 = 3;

    //        /// <summary>
    //        /// The value 250
    //        /// </summary>
    //        public const int VALUE_250 = 250;

    //        /// <summary>
    //        /// The value 50
    //        /// </summary>
    //        public const int VALUE_50 = 50;

    //        /// <summary>
    //        /// The value 25
    //        /// </summary>
    //        public const int VALUE_25 = 25;

    //        /// <summary>
    //        /// The value 220
    //        /// </summary>
    //        public const int VALUE_220 = 220;

    //        /// <summary>
    //        /// The value 436
    //        /// </summary>
    //        public const int VALUE_436 = 436;

    //        /// <summary>
    //        /// The value 448
    //        /// </summary>
    //        public const int VALUE_448 = 448;

    //        /// <summary>
    //        /// The value 251
    //        /// </summary>
    //        public const int VALUE_251 = 251;

    //        /// <summary>
    //        /// The value 26 5 d
    //        /// </summary>
    //        public const double VALUE_20_5D = 20.5d;

    //        /// <summary>
    //        /// The value 465
    //        /// </summary>
    //        public const double VALUE_465 = 465;

    //        /// <summary>
    //        /// The value 41
    //        /// </summary>
    //        public const int VALUE_41 = 41;

    //        /// <summary>
    //        /// The value double 10
    //        /// </summary>
    //        public const double VALUE_DOUBLE_10 = 10.0;

    //        /// <summary>
    //        /// The value 2
    //        /// </summary>
    //        public const int VALUE_2 = 2;

    //        /// <summary>
    //        /// The value 0.0
    //        /// </summary>
    //        public const double VALUE_DECIMAL_0 = 0.0;

    //        /// <summary>
    //        /// The value 0.5
    //        /// </summary>
    //        public const double VALUE_DECIMAL_HALF = 0.5;

    //        /// <summary>
    //        /// The value 30.5
    //        /// </summary>
    //        public const double VALUE_DOUBLE_32HALF = 32.5;

    //        /// <summary>
    //        /// The value 35.5
    //        /// </summary>
    //        public const double VALUE_DOUBLE_35HALF = 35.5;

    //        /// <summary>
    //        /// The value 35.5
    //        /// </summary>
    //        public const double VALUE_DOUBLE_31HALF = 31.5;

    //        /// <summary>
    //        /// The value 5
    //        /// </summary>
    //        public const int VALUE_5 = 5;

    //        /// <summary>
    //        /// The value 440
    //        /// </summary>
    //        public const double VALUE_440 = 440;

    //        /// <summary>
    //        /// The value 500
    //        /// </summary>
    //        public const int VALUE_500 = 500;

    //        /// <summary>
    //        /// The value 33
    //        /// </summary>
    //        public const int VALUE_33 = 33;

    //        public const int VALUE_1 = 1;

    //        /// <summary>
    //        /// The value 235
    //        /// </summary>
    //        public const int VALUE_235 = 235;

    //        public const int VALUE_140 = 140;

    //        public const int VALUE_17 = 17;

    //        public const int VALUE_53 = 53;

    //        public const int VALUE_160 = 160;

    //        public const int VALUE_163 = 163;

    //        public const int VALUE_153 = 153;

    //        public const int VALUE_32 = 32;

    //        public const int VALUE_195 = 195;

    //        public const int VALUE_212 = 212;
    //        public const int VALUE_210 = 210;

    //        public const string DEFAULT_TIME_STRING = "0:0:0";

    //        //125*60*60=450000
    //        public const int EXAM_HOUR_DATAPOINTS = 450000;

    //        //125*60=7500
    //        public const int EXAM_MINUTE_DATAPOINTS = 7500;

    //        public const string COLON = ":";

    //        public const int PRF_VALUE_192 = 192;
    //        public const int PRF_VALUE_240 = 240;
    //        public const int PRF_VALUE_308 = 308;
    //        public const int PRF_VALUE_385 = 385;
    //        public const int PRF_VALUE_480 = 480;

    //        public const int TOP_VALUE_175 = 175;
    //        public const int TOP_VALUE_173 = 173;
    //        public const int TOP_VALUE_178 = 178;
    //        public const int VALUE_410 = 410;
    //        public const double VALUE_41_DEC_5 = 41.5;
    //    }

    //    /// <summary>
    //    /// Key:Indicates customslider values
    //    /// Value: starting value for scale
    //    /// </summary>
    //    private static Dictionary<int, int> mModeScaleChangeDict = new Dictionary<int, int>()
    //    {
    //        {
    //            ScaleValue.VALUE_102,
    //            ScaleValue.VALUE_90
    //        },
    //        {
    //            ScaleValue.VALUE_70,
    //            ScaleValue.VALUE_60
    //        },
    //        {
    //            ScaleValue.VALUE_0,
    //            ScaleValue.VALUE_30
    //        }
    //    };

    //    public static int GetScaleInterval(int velocityRange)
    //    {
    //        var interval = 0;
    //        switch (velocityRange)
    //        {
    //            case ScaleValue.PRF_VALUE_192:
    //                interval = ScaleValue.VALUE_30;
    //                break;

    //            case ScaleValue.PRF_VALUE_240:
    //                interval = ScaleValue.VALUE_40;
    //                break;

    //            case ScaleValue.PRF_VALUE_308:
    //                interval = ScaleValue.VALUE_50;
    //                break;

    //            case ScaleValue.PRF_VALUE_385:
    //                interval = ScaleValue.VALUE_60;
    //                break;

    //            case ScaleValue.PRF_VALUE_480:
    //                interval = ScaleValue.VALUE_70;
    //                break;

    //            default:
    //                interval = ScaleValue.VALUE_50;
    //                break;
    //        }
    //        return interval;
    //    }

    //    /// <summary>
    //    /// Create scale for graph.
    //    /// </summary>
    //    /// <param name="param">The parameter.</param>
    //    public static void CreateScale(ScaleParameters param)
    //    {
    //        switch (param.ScaleType)
    //        {
    //            case ScaleTypeEnum.Spectrogram:
    //                CreateScaleForSpectrogram(param);
    //                break;

    //            case ScaleTypeEnum.MMode:
    //                CreateScaleForMmode(param);
    //                break;

               

    //            default:
    //                break;
    //        }
    //    }

    //    //private static void CreateScaleForEditSpectrogram(ScaleParameters param)
    //    //{
    //    //    try
    //    //    {
    //    //        Grid scaleGrid = new Grid();
    //    //        scaleGrid.Height = param.ParentControl.Height - Constants.VALUE_50;
    //    //        scaleGrid.Margin = new Thickness(Constants.VALUE_0, Constants.VALUE_25, Constants.VALUE_0, Constants.VALUE_25);
    //    //        Grid.SetColumn(scaleGrid, Constants.VALUE_0);
    //    //        //List<UIElement> list = param.ParentControl.Children.Where(x => x is Grid).ToList();

    //    //        //foreach (UIElement item in list)
    //    //        //{
    //    //        //    param.ParentControl.Children.Remove(item);
    //    //        //}

    //    //        TextBlock txtBlock;

    //    //        int counter = Constants.VALUE_0;
    //    //        double left = - Constants.VALUE_15;
    //    //        double top = param.ScreenCoords.Y - Constants.VALUE_10;
    //    //        int posvalue = (int)((param.ScreenCoords.Y) / param.TickPosition);
    //    //        int negValue = Constants.VALUE_11 - posvalue;

    //    //        for (int i = Constants.VALUE_0; i <= posvalue; i++)
    //    //        {
    //    //            txtBlock = new TextBlock();
    //    //            txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //    //            txtBlock.Margin = new Thickness(left, top, Constants.VALUE_0, Constants.VALUE_0);
    //    //            txtBlock.Text = counter.ToString() + " -";
    //    //            scaleGrid.Children.Add(txtBlock);
    //    //            top = top - param.TickPosition;
    //    //            counter = counter + param.Interval;
    //    //        }

    //    //        counter = Constants.VALUE_0;
    //    //        top = param.ScreenCoords.Y - Constants.VALUE_10;

    //    //        for (int i = Constants.VALUE_0; i <= negValue; i++)
    //    //        {
    //    //            top = top + param.TickPosition;
    //    //            counter = counter - param.Interval;
    //    //            txtBlock = new TextBlock();
    //    //            txtBlock.HorizontalAlignment =HorizontalAlignment.Right; 
    //    //            txtBlock.Margin = new Thickness(left, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //    //            txtBlock.Text = counter.ToString() + " -";
    //    //            scaleGrid.Children.Add(txtBlock);
    //    //        }
    //    //        txtBlock = null;
    //    //        param.ParentControl.Children.Add(scaleGrid);
    //    //    }
    //    //    catch (Exception)
    //    //    {
    //    //        throw new Exception("Unable to create scale for spectrogram.");
    //    //    }
    //    //}

    //    /// <summary>
    //    /// Create scale for spectrogram.
    //    /// </summary>
    //    /// <param name="screenCoords">This is point where we need to draw 0 value</param>
    //    /// <param name="parentControl">Parent for scale</param>
    //    /// <param name="Interval">Interval for scale</param>
    //    /// <exception cref="Exception">Unable to create scale for spectrogram.</exception>
    //    private static void CreateScaleForSpectrogram(ScaleParameters param)
    //    {
    //        try
    //        {
    //            //List<UIElement> list = param.ParentControl.Children.Where(x => x is TextBlock).ToList();
    //            //foreach (UIElement item in list)
    //            //{
    //            //    param.ParentControl.Children.Remove(item);
    //            //}
    //            TextBlock txtBlock;
    //            int counter = ScaleValue.VALUE_0;

    //            double left = -ScaleValue.VALUE_15;
    //            double top = param.ScreenCoords.Y + ScaleValue.VALUE_17;

    //            for (double i = top; counter < ScaleValue.VALUE_500 && top > ScaleValue.VALUE_40; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(left, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter.ToString() + " -";
    //                Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //                param.ParentControl.Children.Add(txtBlock);
    //                top = top - param.TickPosition;
    //                counter = counter + param.Interval;
    //            }

    //            counter = ScaleValue.VALUE_0;
    //            top = param.ScreenCoords.Y + ScaleValue.VALUE_15;
    //            for (int i = ScaleValue.VALUE_0; top < ScaleValue.VALUE_235; i++)
    //            {
    //                top = top + param.TickPosition;
    //                counter = counter - param.Interval;
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(left, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter.ToString() + " -";
    //                Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //                param.ParentControl.Children.Add(txtBlock);
    //            }
    //            txtBlock = null;
    //        }
    //        catch (Exception)
    //        {
    //            throw new Exception("Unable to create scale for spectrogram.");
    //        }
    //    }

    //    /// <summary>
    //    /// Creates the scale for mmode.
    //    /// </summary>
    //    /// <param name="parentControl">The parent control.</param>
    //    /// <param name="customSliderValue">The custom slider value.</param>
    //    private static void CreateScaleForMmode(ScaleParameters param)
    //    {
    //        try
    //        {
    //            //List<UIElement> list = param.ParentControl.Children.Where(x => x is TextBlock).ToList();

    //            //foreach (UIElement item in list)
    //            //{
    //            //    param.ParentControl.Children.Remove(item);
    //            //}

    //            int counter = 0;
    //            double top;

    //            counter = mModeScaleChangeDict.Where(x => param.CustomSliderValue > x.Key).OrderByDescending(x => x.Key).First().Value;

    //            if (counter == ScaleValue.VALUE_90)
    //            {
    //                top = ScaleValue.TOP_VALUE_175;
    //            }
    //            else if (counter == ScaleValue.VALUE_60)
    //            {
    //                top = ScaleValue.TOP_VALUE_178;
    //            }
    //            else
    //            {
    //                top = ScaleValue.TOP_VALUE_173;
    //            }
    //            TextBlock txtBlock;
    //            for (double i = ScaleValue.VALUE_0; counter < ScaleValue.VALUE_151 && top > ScaleValue.VALUE_23; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(-ScaleValue.VALUE_15, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter.ToString() + " -";
    //                Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //                param.ParentControl.Children.Add(txtBlock);
    //                top = top - param.TickPosition;
    //                counter = counter + ScaleValue.VALUE_10;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //           // Logs.Instance.ErrorLog<ScaleGenerator>(ex, "CreateScaleForMmode", Severity.Critical);
    //        }
    //    }

    //    /// <summary>
    //    /// Creates the scale for mmode.
    //    /// </summary>
    //    /// <param name="parentControl">The parent control.</param>
    //    /// <param name="customSliderValue">The custom slider value.</param>
    //    private static void CreateScaleForMmodeTestReview(ScaleParameters param)
    //    {
    //        try
    //        {
    //            //List<UIElement> list = param.ParentControl.Children.Where(x => x is TextBlock).ToList();

    //            //foreach (UIElement item in list)
    //            //{
    //            //    param.ParentControl.Children.Remove(item);
    //            //}

    //            int counter = 0;
    //            double top;

    //            counter = mModeScaleChangeDict.Where(x => param.CustomSliderValue > x.Key).OrderByDescending(x => x.Key).First().Value;

    //            if (counter == ScaleValue.VALUE_90)
    //            {
    //                top = ScaleValue.VALUE_212;
    //            }
    //            else if (counter == ScaleValue.VALUE_60)
    //            {
    //                top = ScaleValue.VALUE_210;
    //            }
    //            else
    //            {
    //                top = ScaleValue.VALUE_200;
    //            }
    //            TextBlock txtBlock;
    //            for (double i = ScaleValue.VALUE_0; counter < ScaleValue.VALUE_151 && top > ScaleValue.VALUE_23; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(-ScaleValue.VALUE_15, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter.ToString() + " -";
    //                Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //                param.ParentControl.Children.Add(txtBlock);
    //                top = top - param.TickPosition;
    //                counter = counter + ScaleValue.VALUE_10;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Logs.Instance.ErrorLog<ScaleGenerator>(ex, "CreateScaleForMmodeTestReview", Severity.Critical);
    //        }
    //    }

    //    /// <summary>
    //    /// Create scale for Trending Mean
    //    /// </summary>
    //    /// <param name="parentControl">The parent control.</param>
    //    private static void CreateScaleTrendingMean(System.Windows.Point screenCoords, Grid parentControl, int Interval)
    //    {
    //        try
    //        {
    //            //List<UIElement> list = parentControl.Children.Where(x => x.GetType() == typeof(TextBlock)).ToList();
    //            //foreach (UIElement item in list)
    //            //{
    //            //    parentControl.Children.Remove(item);
    //            //}
    //            TextBlock txtBlock;
    //            var counter = ScaleValue.VALUE_0;

    //            double left = -ScaleValue.VALUE_15;
    //            double top = screenCoords.Y + ScaleValue.VALUE_15;

    //            for (double i = top; counter < ScaleValue.VALUE_500 && top > ScaleValue.VALUE_40; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(left, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter.ToString() + " -";
    //                Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //                parentControl.Children.Add(txtBlock);
    //                top = top - ScaleValue.VALUE_DOUBLE_31HALF;
    //                counter = counter + Interval;
    //            }

    //            counter = ScaleValue.VALUE_0;
    //            top = screenCoords.Y + ScaleValue.VALUE_15;
    //            for (int i = ScaleValue.VALUE_0; top < ScaleValue.VALUE_235; i++)
    //            {
    //                top = top + ScaleValue.VALUE_DOUBLE_31HALF;
    //                counter = counter - Interval;
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(left, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter.ToString() + " -";
    //                Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //                parentControl.Children.Add(txtBlock);
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            throw new Exception("Unable to create scale for spectrogram.");
    //        }
    //    }

    //    /// <summary>
    //    /// Create scale for Trending PI
    //    /// </summary>
    //    /// <param name="parentControl">The parent control.</param>
    //    private static void CreateScaleTrendingPI(Grid parentControl)
    //    {
    //        try
    //        {
    //            //List<UIElement> list = parentControl.Children.Where(x => x is TextBlock).ToList();

    //            //foreach (UIElement item in list)
    //            //{
    //            //    parentControl.Children.Remove(item);
    //            //}

    //            double counter = ScaleValue.VALUE_DECIMAL_0;
    //            double top = ScaleValue.VALUE_195;

    //            TextBlock txtBlock;
    //            for (double i = ScaleValue.VALUE_0; counter < ScaleValue.VALUE_151 && top > ScaleValue.VALUE_0; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(-ScaleValue.VALUE_15, top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter + " -";
    //                Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //                parentControl.Children.Add(txtBlock);
    //                top = top - ScaleValue.VALUE_30;
    //                counter = counter + ScaleValue.VALUE_DECIMAL_HALF;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Logs.Instance.ErrorLog<ScaleGenerator>(ex, "CreateScaleTrendingPI", Severity.Critical);
    //        }
    //    }

    //    /// <summary>
    //    /// Create scale for Trending Horizontal- Time axis
    //    /// </summary>
    //    /// <param name="parentControl">The parent control.</param>
    //    /// <param name="interval">Interval for scale</param>
    //    public static void CreateScaleTrendingHorizontal(Panel parentControl, int interval)
    //    {
    //        try
    //        {
    //            //List<UIElement> list = parentControl.Children.Where(x => x is TextBlock).ToList();

    //            //foreach (UIElement item in list)
    //            //{
    //            //    parentControl.Children.Remove(item);
    //            //}

    //            int counter = ScaleValue.VALUE_0;
    //            double left = ScaleValue.VALUE_0;

    //            TextBlock txtBlock;
    //            for (double i = ScaleValue.VALUE_0; i < ScaleValue.VALUE_5; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Left;
    //                txtBlock.Margin = new Thickness(left, ScaleValue.VALUE_200, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                txtBlock.Text = counter.ToString();
    //                txtBlock.TextWrapping = TextWrapping.Wrap;
    //                Grid.SetRow(txtBlock, ScaleValue.VALUE_0);
    //                parentControl.Children.Add(txtBlock);

    //                left = left + ScaleValue.VALUE_176;
    //                counter = counter + interval;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //           // Logs.Instance.ErrorLog<ScaleGenerator>(ex, "CreateScaleTrendingHorizontal", Severity.Critical);
    //        }
    //    }

    //    /// <summary>
    //    /// Create scale for Trending Horizontal- Time axis
    //    /// </summary>
    //    /// <param name="parentControl">The parent control.</param>
    //    /// <param name="interval">Interval for scale</param>
    //    public static void CreateScaleCVRHorizontal(Panel parentControl, ulong dataCount)
    //    {
    //        try
    //        {
    //            int seconds = ScaleValue.VALUE_0;
    //            int interval = ScaleValue.VALUE_0;
    //            int numofTicks = ScaleValue.VALUE_10;

    //            seconds = (int)(Math.Ceiling(dataCount /(double) Constants.PACKETS_PER_SEC));

    //            if (seconds < Constants.VALUE_10)
    //            {
    //                numofTicks = seconds;
    //            }

    //            interval = seconds / numofTicks;
    //            int modSeconds = seconds % numofTicks;
    //            double oneSecWidth = parentControl.Width / seconds;
    //            double availableWidth = parentControl.Width - (oneSecWidth * modSeconds);

    //            if (interval == Constants.VALUE_0)
    //            {
    //                return;
    //            }

    //            parentControl.Children.Clear();

    //            int counter = ScaleValue.VALUE_0;
    //            double left = ScaleValue.VALUE_0;
    //            string displayValue = ScaleValue.DEFAULT_TIME_STRING;
    //            double leftMarginAdjustment = availableWidth / numofTicks;
    //            TextBlock txtBlock;
    //            for (double i = ScaleValue.VALUE_0; i < ScaleValue.VALUE_11; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Left;
    //                txtBlock.Margin = new Thickness(left, ScaleValue.VALUE_448, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //                displayValue = CalculateTimeAxis(counter);
    //                txtBlock.Text = displayValue;
    //                txtBlock.TextWrapping = TextWrapping.Wrap;
    //                Grid.SetRow(txtBlock, ScaleValue.VALUE_0);
    //                parentControl.Children.Add(txtBlock);
    //                left = left + leftMarginAdjustment;
    //                counter = counter + interval;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Logs.Instance.ErrorLog<ScaleGenerator>(MessageConstants.CreateScaleCVRHorizontalFailed, "CreateScaleCVRHorizontal", Severity.Warning);
    //            //Logs.Instance.ErrorLog<ScaleGenerator>(ex, "CreateScaleCVRHorizontal", Severity.Critical);
    //        }
    //    }

    //    /// <summary>
    //    /// Convert time in seconds to time in hh:mm:ss format
    //    /// </summary>
    //    /// <param name="counter">value is seconds</param>
    //    private static string CalculateTimeAxis(int counter)
    //    {
    //        int examTimeInHours = 0;
    //        int examTimeInMinutes = 0;
    //        int examTimeInSeconds = 0;
    //        int tempRemainingPacketCount = 0;

    //        string stringTimeToDisplay = "";
    //        try
    //        {
    //            if (ScaleValue.EXAM_HOUR_DATAPOINTS < counter)
    //            {
    //                examTimeInHours = counter / ScaleValue.EXAM_HOUR_DATAPOINTS;
    //                tempRemainingPacketCount = counter - (ScaleValue.EXAM_HOUR_DATAPOINTS * examTimeInHours);
    //                examTimeInMinutes = tempRemainingPacketCount / ScaleValue.EXAM_MINUTE_DATAPOINTS;
    //                tempRemainingPacketCount = 0;
    //                tempRemainingPacketCount = counter - (ScaleValue.EXAM_MINUTE_DATAPOINTS * examTimeInMinutes);
    //                examTimeInSeconds = tempRemainingPacketCount;
    //            }
    //            else if ((ScaleValue.EXAM_HOUR_DATAPOINTS > counter) && (ScaleValue.EXAM_MINUTE_DATAPOINTS < counter))
    //            {
    //                examTimeInMinutes = counter / ScaleValue.EXAM_MINUTE_DATAPOINTS;
    //                tempRemainingPacketCount = counter - (ScaleValue.EXAM_MINUTE_DATAPOINTS * examTimeInMinutes);
    //                examTimeInSeconds = tempRemainingPacketCount;
    //            }
    //            else if (counter > ScaleValue.VALUE_60 && ScaleValue.EXAM_MINUTE_DATAPOINTS > counter)
    //            {
    //                examTimeInMinutes = counter / ScaleValue.VALUE_60;
    //                tempRemainingPacketCount = counter - (ScaleValue.VALUE_60 * examTimeInMinutes);
    //                examTimeInSeconds = tempRemainingPacketCount;
    //            }
    //            else
    //            {
    //                examTimeInSeconds = counter;
    //            }

    //            stringTimeToDisplay = examTimeInHours + ScaleValue.COLON + examTimeInMinutes + ScaleValue.COLON + examTimeInSeconds;
    //        }
    //        catch (Exception ex)
    //        {
    //            //Logs.Instance.ErrorLog<ScaleGenerator>(ex, "CalculateTimeAxis", Severity.Critical);
    //        }
    //        return stringTimeToDisplay;
    //    }

    //    public static void CreateMmodeScale(Grid parentGrid, double minimum, double maximum)
    //    {
    //        //var list = parentGrid.Children.Where(x => x is TextBlock).ToList();
    //        //foreach (TextBlock child in list)
    //        //{
    //        //    parentGrid.Children.Remove(child);
    //        //}

    //        int widthAdjustment = 10;
    //        double parentHeight = parentGrid.Height - widthAdjustment;

    //        double high = RoundOffFloor(maximum);
    //        double low = RoundOffCeiling(minimum);
    //        double highDiff = maximum - high;
    //        double lowDiff = low - minimum;

    //        double totalRange = maximum - minimum;
    //        double scaleRange = high - low;
    //        double higherPixels = (parentHeight * highDiff) / totalRange;
    //        double lowerPixels = (parentHeight * lowDiff) / totalRange;

    //        double noOfTicks = ((high - low) / 10) + 1;
    //        double availableHeight = parentHeight - (higherPixels + lowerPixels);

    //        double rawTickPosition = (scaleRange * availableHeight / noOfTicks) / availableHeight;
    //        var tickPosition = ((int)Math.Round(rawTickPosition / 10.0)) * 10;
    //        double interval = tickPosition * (availableHeight / noOfTicks) / rawTickPosition;

    //        double top = parentHeight - lowerPixels;
    //        double counter = (int)low;
    //        TextBlock txtBlock;
    //        for (int i = 0; i < noOfTicks; i++)
    //        {
    //            txtBlock = new TextBlock();
    //            txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //            txtBlock.Margin = new Thickness(-(ScaleValue.VALUE_20), top, ScaleValue.VALUE_0, ScaleValue.VALUE_0);
    //            txtBlock.Text = counter.ToString() + " -";
    //            Grid.SetColumn(txtBlock, ScaleValue.VALUE_0);
    //            parentGrid.Children.Add(txtBlock);
    //            top = top - interval;
    //            counter = counter + tickPosition;
    //        }
    //    }

    //    /// Create scale for CVR- Vertical
    //    /// </summary>
    //    /// <param name="parentControl">The parent control.</param>
    //    /// <param name="maximum">The maximum.</param>
    //    /// <param name="minimum">The minimum.</param>
    //    public static void CreateScaleForCVR(Grid parentControl, double maximum, double minimum)
    //    {
    //        try
    //        {
    //            parentControl.Children.Clear();
    //            double maximumRange = maximum / ScaleValue.VALUE_10;
    //            double minimumRange = minimum / ScaleValue.VALUE_10;
    //            double parentHeight = parentControl.Height - Constants.VALUE_50;
    //            var noOfTicks = 8;
    //            double high = RoundOffFloor(maximumRange);
    //            double low = RoundOffCeiling(minimumRange);

    //            double highDiff = maximumRange - high;
    //            double lowDiff = low - minimumRange;

    //            double totalRange = maximumRange - minimumRange;
    //            double scaleRange = high - low;

    //            double higherPixels = (parentHeight * highDiff) / totalRange;
    //            double lowerPixels = (parentHeight * lowDiff) / totalRange;

    //            double availableHeight = parentHeight - (higherPixels + lowerPixels);

    //            double rawTickPosition = (scaleRange * availableHeight / noOfTicks) / availableHeight;
    //            var tickPosition = ((int)Math.Round(rawTickPosition / ScaleValue.VALUE_DOUBLE_10)) * ScaleValue.VALUE_10;

    //            if (tickPosition == Constants.VALUE_0)
    //            {
    //                tickPosition = (int)maximumRange / noOfTicks;
    //            }

    //            double interval = tickPosition * (availableHeight / noOfTicks) / rawTickPosition;

    //            TextBlock txtBlock;
    //            double top = parentHeight - lowerPixels + Constants.VALUE_25;
    //            int counter = (int)low;

    //            for (int i = 0; top > minimumRange; i++)
    //            {
    //                txtBlock = new TextBlock();
    //                txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
    //                txtBlock.Margin = new Thickness(-(Constants.VALUE_50), top, Constants.VALUE_0, Constants.VALUE_0);
    //                txtBlock.Text = counter + " -";
    //                Grid.SetColumn(txtBlock, Constants.VALUE_0);
    //                parentControl.Children.Add(txtBlock);
    //                top = top - interval;
    //                counter = counter + tickPosition;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Logs.Instance.ErrorLog<ScaleGenerator>(ex, "CreateScaleForCVR", Severity.Critical);
    //        }
    //    }

    //    private static double RoundOffCeiling(double maximum)
    //    {
    //        return (Math.Ceiling(maximum / Constants.VALUE_DOUBLE_10) * Constants.VALUE_10);
    //    }

    //    private static double RoundOffFloor(double minimum)
    //    {
    //        return (Math.Floor(minimum / Constants.VALUE_DOUBLE_10) * Constants.VALUE_10);
    //    }
    //}

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