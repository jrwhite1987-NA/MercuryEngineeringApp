using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Core.Common;
namespace MercuryEngApp.Common
{
    public class Scale
    {
        private int Interval { get; set; }

        private double TickPosition { get; set; }

        public  void CreateScale(ScaleParameters param)
        {
            Helper.logger.Debug("++");
            switch (param.ScaleType)
            {
                case ScaleTypeEnum.MMode:
                    break;
                case ScaleTypeEnum.Spectrogram:
                    CreateScaleForSpectrogram(param);
                    break;
                case ScaleTypeEnum.None:
                    break;
                default:
                    break;
            }
            Helper.logger.Debug("--");
        }

        private void CreateScaleForSpectrogram(ScaleParameters param)
        {
            Helper.logger.Debug("++");
            try
            {
                param.ParentControl.Children.Clear();
                var interval = GetScaleInterval(param.VelocityRange);
                var tickPosition = GetTickPosition((double)param.VelocityRange,(double)interval,param.BitmapHeight);
                TextBlock txtBlock;
                int counter = 0;
                double left = 0;
                double top = param.ScreenCoords.Y - Constants.VALUE_10;
                int posvalue = (int)((param.ScreenCoords.Y) / tickPosition);
                int negValue = Constants.VALUE_11 - posvalue;

                for (int i = Constants.VALUE_0; i <= posvalue; i++)
                {
                    txtBlock = new TextBlock();

                    txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
                    txtBlock.Margin = new Thickness(left, top, 0, 0);
                    txtBlock.Text = counter.ToString() + " -";
                    txtBlock.Foreground = new SolidColorBrush(Colors.White);
                    param.ParentControl.Children.Add(txtBlock);
                    top = top - tickPosition;
                    counter = counter + interval;
                }

                counter = Constants.VALUE_0;
                top = param.ScreenCoords.Y - Constants.VALUE_10;

                for (int i = Constants.VALUE_0; i <= negValue; i++)
                {
                    top = top + tickPosition;
                    counter = counter - interval;
                    txtBlock = new TextBlock();
                    txtBlock.Foreground = new SolidColorBrush(Colors.White);
                    txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
                    txtBlock.Margin = new Thickness(left, top, 0, 0);
                    txtBlock.Text = counter.ToString() + " -";
                    param.ParentControl.Children.Add(txtBlock);
                }
                txtBlock = null;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw new Exception("Unable to create scale for spectrogram.");
            }
            Helper.logger.Debug("--");
        }

        public void CreateMmodeScale(Grid parentGrid, double minimum, double maximum)
        {
            try
            {
                Helper.logger.Debug("++");
                parentGrid.Children.Clear();
                int widthAdjustment = 10;
                double parentHeight = parentGrid.Height - widthAdjustment;

                double high = RoundOffFloor(maximum);
                double low = RoundOffCeiling(minimum);
                double highDiff = maximum - high;
                double lowDiff = low - minimum;

                double totalRange = maximum - minimum;
                double scaleRange = high - low;
                double higherPixels = (parentHeight * highDiff) / totalRange;
                double lowerPixels = (parentHeight * lowDiff) / totalRange;

                double noOfTicks = ((high - low) / 10) + 1;
                double availableHeight = parentHeight - (higherPixels + lowerPixels);

                double rawTickPosition = (scaleRange * availableHeight / noOfTicks) / availableHeight;
                var tickPosition = ((int)Math.Round(rawTickPosition / 10.0)) * 10;
                double interval = tickPosition * (availableHeight / noOfTicks) / rawTickPosition;

                double top = parentHeight - lowerPixels-5;
                double counter = (int)low;
                TextBlock txtBlock;
                for (int i = 0; i < noOfTicks; i++)
                {
                    txtBlock = new TextBlock();
                    txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
                    txtBlock.Margin = new Thickness(-20, top, 0, 0);
                    txtBlock.Foreground = new SolidColorBrush(Colors.White);
                    txtBlock.Text = counter.ToString() + " -";
                    Grid.SetColumn(txtBlock, 0);
                    parentGrid.Children.Add(txtBlock);
                    top = top - interval;
                    counter = counter + tickPosition;
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw new Exception("Unable to create scale for mmode.");
            }
            Helper.logger.Debug("--");
        }

        private double RoundOffCeiling(double maximum)
        {
            return (Math.Ceiling(maximum / Constants.VALUE_DOUBLE_10) * Constants.VALUE_10);
        }

        private double RoundOffFloor(double minimum)
        {
            return (Math.Floor(minimum / Constants.VALUE_DOUBLE_10) * Constants.VALUE_10);
        }
        /// <summary>
        /// Gets the tick position.
        /// </summary>
        /// <param name="velocityRange">The velocity range.</param>
        /// <param name="interval">The interval.</param>
        /// <returns>System.Double.</returns>
        private double GetTickPosition(double velocityRange, double interval, double spectumImgHeight)
        {
            double height = spectumImgHeight / Constants.VALUE_2;
            double tickPosition = (height * interval) / (velocityRange / Constants.VALUE_2);
            return tickPosition;
        }

        /// <summary>
        /// Gets the scale interval.
        /// </summary>
        /// <param name="velocityRange">The velocity range.</param>
        /// <returns>System.Int32.</returns>
        private int GetScaleInterval(int velocityRange)
        {
            Helper.logger.Debug("++");
            var interval = Constants.VALUE_0;

            switch (velocityRange)
            {
                case Constants.VALUE_192:
                    interval = 30;
                    break;
                case Constants.VALUE_240:
                    interval = 40;
                    break;
                case Constants.VALUE_308:
                    interval = 50;
                    break;
                case Constants.VALUE_385:
                    interval = 60;
                    break;
                case Constants.VALUE_480:
                    interval = 70;
                    break;
                default:
                    break;
            }

            Helper.logger.Debug("--");
            return interval;
        }
    }
}
