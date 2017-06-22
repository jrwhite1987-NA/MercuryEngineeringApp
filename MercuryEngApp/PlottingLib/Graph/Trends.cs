using System.Collections.Generic;
using System.Linq;
using System.IO;
using Core.Constants;
using System.Runtime.InteropServices.WindowsRuntime;
using Core.Common;
using System;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using System.Windows.Media;
using UsbTcdLibrary.PacketFormats;

namespace PlottingLib
{
    /// <summary>
    /// Enum for trend x scale
    /// </summary>
    public enum TrendXScale : int
    {
        FiveMinutes = 5,
        FifteenMinutes = 15,
        ThirtyMinutes = 30,
        None = Constants.VALUE_0
    }

    /// <summary>
    /// Trending data Collection
    /// </summary>
    public class TrendCollection : IDisposable
    {
        /// <summary>
        /// LeftPosMeanList
        /// </summary>
        public List<int> LeftPosMeanList { get; set; }
        /// <summary>
        /// LeftPosPiList
        /// </summary>
        public List<int> LeftPosPiList { get; set; }
        /// <summary>
        /// /LeftPosPiList
        /// </summary>
        public List<int> RightPosMeanList { get; set; }
        /// <summary>
        /// RightPosPiList
        /// </summary>
        public List<int> RightPosPiList { get; set; }
        /// <summary>
        /// LeftPosMinList
        /// </summary>
        public List<int> LeftPosMinList { get; set; }
        /// <summary>
        /// RightPosMinList
        /// </summary>
        public List<int> RightPosMinList { get; set; }
        /// <summary>
        /// LeftPosMaxList
        /// </summary>
        public List<int> LeftPosMaxList { get; set; }
        /// <summary>
        /// RightPosMaxList
        /// </summary>
        public List<int> RightPosMaxList { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public double Count { get; set; }

        public void Dispose()
        {
            ClearList(LeftPosMeanList);           
            ClearList(LeftPosMaxList);            
            ClearList(LeftPosMinList);
            ClearList(LeftPosPiList);            
        }
        private void ClearList(List<int> list)
        {
            list.Clear();
        }
    }

    /// <summary>
    /// This will render trend graph.
    /// </summary>
    public class Trends : IDisposable
    {
        #region Property

        /// <summary>
        /// writeable bitmap for trend.
        /// </summary>
        public WriteableBitmap TrendBitmap { get; set; }

        public Panel HorizontalScalePanel { get; set; }

        public TrendXScale SelectedTime { get; set; }

        private int readHeadLeftMean;
        private int readHeadLeftPi;
        private int readHeadRightMean;
        private int readHeadRightPi;

        /// <summary>
        /// The trends list collection
        /// </summary>
        /// static TrendCollection
        ///
        private static TrendCollection TrendsListCollection = new TrendCollection
        {
            LeftPosMeanList = new List<int>(),
            LeftPosPiList = new List<int>(),
            LeftPosMaxList = new List<int>(),
            LeftPosMinList = new List<int>(),
            RightPosMaxList = new List<int>(),
            RightPosMinList = new List<int>(),
            RightPosMeanList = new List<int>(),
            RightPosPiList = new List<int>(),
            Count = Constants.VALUE_0
        };

        public static uint PRF { get; set; }

        public static bool PlotLeftMean { get; set; }

        public static bool PlotLeftPi { get; set; }

        public static bool PlotRightMean { get; set; }

        public static bool PlotRightPi { get; set; }

        public static bool PlotLeftMin { get; set; }

        public static bool PlotRightMin { get; set; }

        public static bool PlotLeftMax { get; set; }

        public static bool PlotRightMax { get; set; }

        #endregion Property

        #region Variables

        private const int TWO_VALUE = 2;

        private const int MaxValue = 4500;

        private const int BlockSize = 750;

        public const int PacketCount = 50;

        private int pixelsFactor = Constants.VALUE_0;
        public const string Time5Min = "time 5 min";
        public const string Time30Min = "time 30 min";
        public const string Time15Min = "time 15 min";
        private int SizeOfQueue { get; set; }

        public Stream BitmapStream { get; set; }

        public byte[] ArrayPixel { get; set; }
        public static int trendingCounter = Constants.VALUE_0;

        #endregion Variables

        #region Constants

        /// <summary>
        /// The PRF value 5000
        /// </summary>
        private const int PRF_VALUE_5000 = 5000;

        /// <summary>
        /// The PRF value 6250
        /// </summary>
        private const int PRF_VALUE_6250 = 6250;

        /// <summary>
        /// The PRF value 8000
        /// </summary>
        private const int PRF_VALUE_8000 = 8000;

        /// <summary>
        /// The PRF value 10000
        /// </summary>
        private const int PRF_VALUE_10000 = 10000;

        /// <summary>
        /// The vertscale 1 decimal 6
        /// </summary>
        private const double VERTSCALE_1_DEC_6 = 1.6;

        /// <summary>
        /// The vertscale 1 decimal 28
        /// </summary>
        private const double VERTSCALE_1_DEC_28 = 1.28;

        /// <summary>
        /// The vertscale 1 decimal 0
        /// </summary>
        private const double VERTSCALE_1_DEC_0 = 1.0;

        /// <summary>
        /// The vertscale 0 decimal 8
        /// </summary>
        private const double VERTSCALE_0_DEC_8 = 0.8;

        /// <summary>
        /// The vertscale 0 decimal 642
        /// </summary>
        private const double VERTSCALE_0_DEC_642 = 0.642;

        /// <summary>
        /// The default sizeofqueue
        /// </summary>
        private const int DEFAULT_SIZEOFQUEUE = 375;

        /// <summary>
        /// The value 3
        /// </summary>
        private const int VALUE_3 = 3;

        /// <summary>
        /// The value 5
        /// </summary>
        private const int VALUE_5 = 5;

        /// <summary>
        /// The value 6
        /// </summary>
        private const int VALUE_6 = 6;

        /// <summary>
        /// The PRF 6250
        /// </summary>
        private const int PRF_6250 = 6250;

        /// <summary>
        /// The PRF 5000
        /// </summary>
        private const int PRF_5000 = 5000;

        /// <summary>
        /// The PRF 8000
        /// </summary>
        private const int PRF_8000 = 8000;

        /// <summary>
        /// The PRF 10000
        /// </summary>
        private const int PRF_10000 = 10000;

        #endregion Constants

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="xScale"></param>
        /// <param name="horScalepanel"></param>
        public Trends(WriteableBitmap bitmap, TrendXScale xScale, Panel horScalepanel)
        {
            TrendBitmap = bitmap;
            SelectedTime = xScale;
            HorizontalScalePanel = horScalepanel;
            SizeOfQueue = DEFAULT_SIZEOFQUEUE;
            readHeadLeftMean = Constants.VALUE_0;
            readHeadLeftPi = Constants.VALUE_0;
            readHeadRightMean = Constants.VALUE_0;
            readHeadRightPi = Constants.VALUE_0;
            CreateHorizontalScale();
            BitmapStream = TrendBitmap.PixelBuffer.AsStream();
            ArrayPixel = new byte[TrendBitmap.PixelHeight * TrendBitmap.PixelWidth * Constants.VALUE_4];
            pixelsFactor = TrendBitmap.PixelWidth * Constants.BytesForColor;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Renders the graph.
        /// </summary>
        public void RenderGraph()
        {
            Helper.logger.Debug("++");
            try
            {           
                double vertScale = GetVerticalScale(PRF);

                Array.Clear(ArrayPixel, 0, ArrayPixel.Length);

                DrawGraph(TrendsListCollection.LeftPosMaxList, PlotLeftMax, vertScale, Colors.LightGreen, ref readHeadLeftMean, TrendBitmap.PixelHeight / TWO_VALUE);
                DrawGraph(TrendsListCollection.LeftPosMinList, PlotLeftMin, vertScale, Colors.Yellow, ref readHeadLeftMean, TrendBitmap.PixelHeight / TWO_VALUE);
                DrawGraph(TrendsListCollection.LeftPosMeanList, PlotLeftMean, vertScale, Colors.DarkGreen, ref readHeadLeftMean, TrendBitmap.PixelHeight / TWO_VALUE);
                DrawGraph(TrendsListCollection.LeftPosPiList, PlotLeftPi, vertScale, Colors.GreenYellow, ref readHeadLeftPi, TrendBitmap.PixelHeight - Constants.VALUE_10, true);

                DrawGraph(TrendsListCollection.RightPosMaxList, PlotRightMax, vertScale, Colors.Red, ref readHeadRightMean, TrendBitmap.PixelHeight / TWO_VALUE);
                DrawGraph(TrendsListCollection.RightPosMinList, PlotRightMin, vertScale, Colors.Orange, ref readHeadRightMean, TrendBitmap.PixelHeight / TWO_VALUE);
                DrawGraph(TrendsListCollection.RightPosMeanList, PlotRightMean, vertScale, Colors.DarkOrange, ref readHeadRightMean, TrendBitmap.PixelHeight / TWO_VALUE);
                DrawGraph(TrendsListCollection.RightPosPiList, PlotRightPi, vertScale, Colors.OrangeRed, ref readHeadRightPi, TrendBitmap.PixelHeight - Constants.VALUE_10, true);

                BitmapStream.Seek(Constants.VALUE_0, SeekOrigin.Begin);
                BitmapStream.Write(ArrayPixel, Constants.VALUE_0, ArrayPixel.Length);

                TrendBitmap.Invalidate();                
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw ex;
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Draws the graph.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="shouldPlot">if set to <c>true</c> [should plot].</param>
        /// <param name="vertScale">The vert scale.</param>
        /// <param name="color">The color.</param>
        /// <param name="readHead">The read head.</param>
        /// <param name="baselineCorrection">The baseline correction.</param>
        /// <param name="isPI">if set to <c>true</c> [is pi].</param>
        protected void DrawGraph(List<int> collection, bool shouldPlot, double vertScale, Color color, ref int readHead, int baselineCorrection, bool isPI = false)
        {
            Helper.logger.Debug("++");
            try
            {
                if (shouldPlot && collection != null && collection.Count > Constants.VALUE_0)
                {
                    if (collection.Count > MaxValue)
                    {             
                        collection.RemoveRange(0, (collection.Count - MaxValue) - 1);
                    }

                    //start point is zero if buffer isn't full or the starting point is max length minus block size
                    int interval = (int)SelectedTime / VALUE_5;
                    int start = collection.Count < interval * BlockSize
                        ? readHead
                        : collection.Count - BlockSize * interval;

                    int columnOffset = Constants.VALUE_0;
                    int currentPoint = 0;
                    int prevPoint = 0;
                    int begin = 0;
                    int end = 0;
                    
                    for (int i = start; i < collection.Count; i += interval)
                    {
                        if (isPI)
                        {
                            currentPoint = collection[i];
                            prevPoint = i == start ? currentPoint : collection[i - interval];
                        }
                        else
                        {
                            currentPoint = (int)(vertScale * collection[i]);
                            prevPoint = i == start ? currentPoint : (int)(vertScale * collection[i - interval]);
                        }

                        currentPoint = baselineCorrection - currentPoint;
                        prevPoint = baselineCorrection - prevPoint;
                        begin = currentPoint >= prevPoint ? prevPoint : currentPoint;
                        end = currentPoint >= prevPoint ? currentPoint : prevPoint;
                        for (int j = begin; j <= end; j++)
                        {
                            if (j < TrendBitmap.PixelHeight && j >= Constants.VALUE_0)
                            {
                                ArrayPixel[columnOffset + j * pixelsFactor] = color.B; // B Value
                                ArrayPixel[columnOffset + j * pixelsFactor + Constants.VALUE_1] = color.G; // G Value
                                ArrayPixel[columnOffset + j * pixelsFactor + Constants.VALUE_2] = color.R; // R Value
                                ArrayPixel[columnOffset + j * pixelsFactor + VALUE_3] = color.A;// A Value
                            }
                        }
                        columnOffset += Constants.BytesForColor;
                    }                    
                }                
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw ex;            
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Sets the trending data.
        /// </summary>
        /// <param name="pmdDataPackets">The PMD data packets.</param>
        public void SetTrendingData(DMIPmdDataPacket[] pmdDataPackets)
        {
            Helper.logger.Debug("++");
            try
            {                
                trendingCounter++;
                if (trendingCounter == PacketCount)
                {
                    if (pmdDataPackets[Constants.VALUE_0] != null) // left channel
                    {
                        TrendsListCollection.LeftPosMeanList.Add(pmdDataPackets[Constants.VALUE_0].envelope.posMEAN / Constants.VALUE_10);
                        TrendsListCollection.LeftPosPiList.Add(pmdDataPackets[Constants.VALUE_0].envelope.posPI / Constants.VALUE_10);
                        TrendsListCollection.LeftPosMinList.Add(pmdDataPackets[Constants.VALUE_0].envelope.posDIAS / Constants.VALUE_10);
                        TrendsListCollection.LeftPosMaxList.Add(pmdDataPackets[Constants.VALUE_0].envelope.posPEAK / Constants.VALUE_10);
                    }

                    if (pmdDataPackets[Constants.VALUE_1] != null) // right channel
                    {
                        TrendsListCollection.RightPosMeanList.Add(pmdDataPackets[Constants.VALUE_1].envelope.posMEAN / Constants.VALUE_10);
                        TrendsListCollection.RightPosPiList.Add(pmdDataPackets[Constants.VALUE_1].envelope.posPI / Constants.VALUE_10);
                        TrendsListCollection.RightPosMinList.Add(pmdDataPackets[Constants.VALUE_1].envelope.posDIAS / Constants.VALUE_10);
                        TrendsListCollection.RightPosMaxList.Add(pmdDataPackets[Constants.VALUE_1].envelope.posPEAK / Constants.VALUE_10);
                    }
                    TrendsListCollection.Count++;
                    trendingCounter = Constants.VALUE_0;
                }             
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw;
            }
            Helper.logger.Debug("--");
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public static void Clear()
        {
            Helper.logger.Debug("++");
            if (TrendsListCollection != null)
            {
                ClearCollection(TrendsListCollection.LeftPosMeanList);
                ClearCollection(TrendsListCollection.LeftPosPiList);
                ClearCollection(TrendsListCollection.RightPosMeanList);
                ClearCollection(TrendsListCollection.RightPosPiList);
                ClearCollection(TrendsListCollection.LeftPosMinList);
                ClearCollection(TrendsListCollection.RightPosMinList);
                ClearCollection(TrendsListCollection.LeftPosMaxList);
                ClearCollection(TrendsListCollection.RightPosMaxList);
            }
            Helper.logger.Debug("--");
        }

        private static void ClearCollection(List<int> collection)
        {
            Helper.logger.Debug("++");
            if(collection != null)
            {
                collection.Clear();
            }
            Helper.logger.Debug("--");
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Get vertical scale
        /// </summary>
        /// <param name="prf">The PRF.</param>
        /// <returns></returns>
        private double GetVerticalScale(uint prf)
        {
            Helper.logger.Debug("++");
            try
            {
                double vertScale = VERTSCALE_1_DEC_0;
                if (prf == PRF_5000)
                {
                    vertScale = VERTSCALE_1_DEC_6;
                }
                else if (prf == PRF_6250)
                {
                    vertScale = VERTSCALE_1_DEC_28;
                }
                else if (prf == PRF_8000)
                {
                    vertScale = VERTSCALE_1_DEC_0;
                }
                else if (prf == PRF_10000)
                {
                    vertScale = VERTSCALE_0_DEC_8;
                }
                else
                {
                    vertScale = VERTSCALE_0_DEC_642;
                }
                Helper.logger.Debug("--");
                return vertScale;
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                return VERTSCALE_1_DEC_0;
            }
        }

        /// <summary>
        /// Create horizonatal Scale.
        /// </summary>
        public void CreateHorizontalScale()
        {
            Helper.logger.Debug("++");
            var interval = Constants.VALUE_0;
            switch (SelectedTime)
            {
                case TrendXScale.FiveMinutes:
                    interval = Constants.VALUE_1;
                    break;

                case TrendXScale.ThirtyMinutes:
                    interval = VALUE_6;
                    break;

                case TrendXScale.FifteenMinutes:
                    interval = VALUE_3;
                    break;

                default:
                    interval = Constants.VALUE_1;
                    break;
            }
            Helper.logger.Debug("--");            
        }

        #endregion

        public void Dispose()
        {
            TrendBitmap = null;
            TrendsListCollection.Dispose();
            BitmapStream.Dispose();
            ArrayPixel = null;
        }
    }
}