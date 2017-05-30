using Core.Common;
using Core.Constants;
using System;
using Windows.UI;
using System.IO;
using UsbTcdLibrary.PacketFormats;
using System.Windows.Media.Imaging;


namespace PlottingLib
{
    public class Mmode : BaseGraph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Mmode"/> class.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        public Mmode(WriteableBitmap bitmap)
            : base(bitmap)
        {
            MmodeHeight = Constants.MMODE_BITMAP_HEIGHT - (Constants.VALUE_5);
        }

        public int MmodeHeight { get; set; }

        /// <summary>
        /// Render graph for mmode.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="packet">DMI packet</param>
        public override void RenderGraph(DMIPmdDataPacket packet)
        {
           
            try
            {
                int pixelsFactor = GraphBitmap.PixelWidth * Constants.BytesForColor;
                int columnsIncrementFactor = XStart * Constants.BytesForColor;             

                for (int i = 0; i < MmodeHeight; i++)
                {
                    CurrentColor = IsLeftChannel
                                         ? ColorMap.GetMmodeColorChannel1(packet.mmode.power[MmodeHeight - i - 1])
                                         : ColorMap.GetMmodeColorChannel2(packet.mmode.power[MmodeHeight - i - 1]);

                    ArrayPixel[columnsIncrementFactor] = CurrentColor.B; // B value
                    ArrayPixel[++columnsIncrementFactor] = CurrentColor.G; //G value
                    ArrayPixel[++columnsIncrementFactor] = CurrentColor.R; //R value
                    ArrayPixel[++columnsIncrementFactor] = CurrentColor.A; //A Value

                    if (i != MmodeHeight - 1)
                    {
                        ArrayPixel[++columnsIncrementFactor] = Colors.White.B; //B value
                        ArrayPixel[++columnsIncrementFactor] = Colors.White.G; //G value
                        ArrayPixel[++columnsIncrementFactor] = Colors.White.R; //R value
                        ArrayPixel[++columnsIncrementFactor] = Colors.White.A; //A value

                        ArrayPixel[++columnsIncrementFactor] = Colors.White.B;  //B value
                        ArrayPixel[++columnsIncrementFactor] = Colors.White.G; //G value
                        ArrayPixel[++columnsIncrementFactor] = Colors.White.R; //R value
                        ArrayPixel[++columnsIncrementFactor] = Colors.White.A; //A value
                    }
                    columnsIncrementFactor += pixelsFactor - Constants.DecrementFactor;
                }
                GraphBitmap.FromByteArray(ArrayPixel);               
            }
            catch (Exception ex)
            {
                Helper.logger.Warn("Exception: ", ex);
                throw;
            }           
        }
    }
}