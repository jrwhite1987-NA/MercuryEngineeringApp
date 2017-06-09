using Core.Common;
using Core.Constants;
using System;
using System.Windows.Media.Imaging;
using UsbTcdLibrary.PacketFormats;
using log4net;
namespace PlottingLib
{
    /// <summary>
    ///
    /// </summary>
    public enum SpectrumXScale : int
    {
        FourSecond = 4,
        EightSecond = 8,
        TwelveSecond = 12,
        None = 0
    }

    /// <summary>
    /// List of bitmaps for graphs
    /// </summary>
    public class BitmapList
    {
        public WriteableBitmap LeftMmodeBitmap { get; set; }

        public WriteableBitmap RightMmodeBitmap { get; set; }

        public WriteableBitmap LeftSpectrumBitmap { get; set; }

        public WriteableBitmap RightSpectrumBitmap { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class NaGraph : IDisposable
    {
        #region constants

        private const int INDEX_0 = 0;
        private const int INDEX_1 = 1;

        #endregion constants

        /// <summary>
        /// Gets or sets the left spectrogram.
        /// </summary>
        /// <value>
        /// The left spectrogram.
        /// </value>
        public Spectrogram LeftSpectrogram { get; set; }

        /// <summary>
        /// Gets or sets the right spectrogram.
        /// </summary>
        /// <value>
        /// The right spectrogram.
        /// </value>
        public Spectrogram RightSpectrogram { get; set; }

        /// <summary>
        /// Gets or sets the left mmode.
        /// </summary>
        /// <value>
        /// The left mmode.
        /// </value>
        public Mmode LeftMmode { get; set; }

        /// <summary>
        /// Gets or sets the right mmode.
        /// </summary>
        /// <value>
        /// The right mmode.
        /// </value>
        public Mmode RightMmode { get; set; }

        /// <summary>
        /// Gets or sets the x start.
        /// </summary>
        /// <value>
        /// The x start.
        /// </value>
        public int XStart { get; set; }

        /// <summary>
        /// Gets or sets the size of queue.
        /// </summary>
        /// <value>
        /// The size of queue.
        /// </value>
        public int SizeOfQueue { get; set; }

        

        /// <summary>
        /// Initializes a new instance of the <see cref="NaGraph"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public NaGraph(BitmapList list)
        {
            LeftSpectrogram = new Spectrogram(list.LeftSpectrumBitmap);
            RightSpectrogram = new Spectrogram(list.RightSpectrumBitmap);
            LeftMmode = new Mmode(list.LeftMmodeBitmap);
            RightMmode = new Mmode(list.RightMmodeBitmap);            
        }

        /// <summary>
        /// This method will initialize required parameters for before plotting.
        /// </summary>
        /// <param name="xScale"></param>
        /// <param name="examProcedureId"></param>
        public void Initialize(SpectrumXScale xScale, float gain = -Constants.VALUE_40)
        {
            Helper.logger.Debug("++");
            switch (xScale)
            {
                case SpectrumXScale.FourSecond:
                    SizeOfQueue = Constants.SpectrumXScale_500;
                    break;

                case SpectrumXScale.EightSecond:
                    SizeOfQueue = Constants.SpectrumXScale_1000;
                    break;

                case SpectrumXScale.TwelveSecond:
                    SizeOfQueue = Constants.SpectrumXScale_1500;
                    break;

                default:
                    SizeOfQueue = Constants.SpectrumXScale_500;
                    break;
            }
            if ((int)gain != -Constants.VALUE_40)
            {
                BaseGraph.ColorMap.SetLowerLimitFromGain(gain);
            }
            Helper.logger.Debug("--");
        }

        public void SetGain(float gain)
        {
            BaseGraph.ColorMap.SetLowerLimitFromGain(gain);
        }

        /// <summary>
        /// Process DMI Packet
        /// </summary>
        /// <param name="arrayDataPackets">The array data packets.</param>
        /// <param name="isCld">if set to <c>true</c> [is CLD].</param>
        /// <param name="currentChannel">The current channel.</param>
        public void ProcessPacket(DMIPmdDataPacket[] arrayDataPackets, bool isCld, int currentChannel)
        {           
            try
            {              
                XStart = (++XStart % SizeOfQueue);
                if (XStart == INDEX_0)
                {
                    LeftSpectrogram.SpectrumEnvolope.EnvelopDataPoints.Clear();
                    RightSpectrogram.SpectrumEnvolope.EnvelopDataPoints.Clear();
                }
                LeftSpectrogram.XStart = XStart;
                RightSpectrogram.XStart = XStart;
                LeftMmode.XStart = XStart;
                RightMmode.XStart = XStart;

                if (arrayDataPackets[INDEX_0] != null)
                {
                    LeftSpectrogram.IsLeftChannel = true;
                    LeftMmode.IsLeftChannel = true;
                    if (LeftSpectrogram.DisplayGraph)
                    {
                        LeftSpectrogram.RenderGraph(arrayDataPackets[INDEX_0]);
                    }

                    if (LeftMmode.DisplayGraph)
                    {
                        LeftMmode.RenderGraph(arrayDataPackets[INDEX_0]);
                    }
                }
                if (arrayDataPackets[INDEX_1] != null)
                {
                    if (!isCld || currentChannel == Constants.VALUE_2)
                    {
                        if (RightSpectrogram.DisplayGraph)
                        {
                            RightSpectrogram.RenderGraph(arrayDataPackets[INDEX_1]);
                        }
                        if (RightMmode.DisplayGraph)
                        {
                            RightMmode.RenderGraph(arrayDataPackets[INDEX_1]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw ex;
            }          
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            LeftSpectrogram.Dispose();
            RightSpectrogram.Dispose();
            LeftMmode.Dispose();
            RightMmode.Dispose();
        }
    }
}