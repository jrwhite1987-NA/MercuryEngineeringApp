using Core.Common;
using Core.Constants;
using System;
using System.IO;
using Windows.UI;
using UsbTcdLibrary.PacketFormats;
using System.Windows.Media.Imaging;
using UsbTcdLibrary;
//using Windows.UI.Xaml.Media.Imaging;

namespace PlottingLib
{
    public class Spectrogram : BaseGraph
    {
        #region Constants

        private const int VALUE_0 = 0;
        private const int OFFSET_1 = 1;

        #endregion Constants

        private int _baselinePos;

        /// <summary>
        /// Gets or sets the base line position.
        /// </summary>
        /// <value>
        /// The base line position.
        /// </value>
        public int BaseLinePosition
        {
            get
            {
                return _baselinePos;
            }
            set
            {
                _baselinePos = value;
            }
        }

        public Envolope SpectrumEnvolope { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Spectrogram"/> class.
        /// </summary>
        /// <param name="writeableBitmap">The writeable bitmap.</param>
        public Spectrogram(WriteableBitmap writeableBitmap)
            : base(writeableBitmap)
        {
            SpectrumEnvolope = new Envolope();
        }

        protected Spectrogram() { }
       
        /// <summary>
        /// This method render graph for spectum.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="packet"></param>
        public override void RenderGraph(DMIPmdDataPacket packet)
        {
            try
            {
                var pixelsFactor = GraphBitmap.PixelWidth * Constants.BytesForColor;
                var columnOffset = XStart * Constants.BytesForColor;
                var index =  GetIndexforBaseline(BaseLinePosition);
                
                for (int i = VALUE_0; i < Constants.FFTSize; i++)
                {
                    if (index == Constants.FFTSize)
                    {
                        index = VALUE_0;
                    }

                    CurrentColor = IsLeftChannel
                        ? ColorMap.GetSpectrogramColorChannel1(packet.spectrum.points[index])
                        : ColorMap.GetSpectrogramColorChannel2(packet.spectrum.points[index]);

                    ArrayPixel[columnOffset] = CurrentColor.B; // B Value
                    ArrayPixel[++columnOffset] = CurrentColor.G; // G Value
                    ArrayPixel[++columnOffset] = CurrentColor.R; // R Value
                    ArrayPixel[++columnOffset] = CurrentColor.A; // A value

                    if (i != Constants.FFTSize - OFFSET_1)
                    {
                        ArrayPixel[++columnOffset] = Colors.White.B; // B Value
                        ArrayPixel[++columnOffset] = Colors.White.G; // G Value
                        ArrayPixel[++columnOffset] = Colors.White.R; // R Value
                        ArrayPixel[++columnOffset] = Colors.White.A;// A Value

                        ArrayPixel[++columnOffset] = Colors.White.B; // B Value
                        ArrayPixel[++columnOffset] = Colors.White.G; // G Value
                        ArrayPixel[++columnOffset] = Colors.White.R; // R Value
                        ArrayPixel[++columnOffset] = Colors.White.A;// A Value
                    }
                    columnOffset += pixelsFactor - Constants.DecrementFactor;
                    index++;
                }
                SpectrumEnvolope.PlotEnvolope(this, packet, columnOffset, pixelsFactor);
                GraphBitmap.FromByteArray(ArrayPixel);               
            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
        public static int GetIndexforBaseline(int baseLinePosition)
        {
            const int DEFAULT_BASE_VALUE = 0;
            Constants.DefaultBaseline = 64;

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

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            SpectrumEnvolope.Dispose();
        }
    }
}