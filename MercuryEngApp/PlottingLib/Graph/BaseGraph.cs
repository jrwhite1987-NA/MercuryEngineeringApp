using Core.Constants;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Media.Imaging;
using UsbTcdLibrary.PacketFormats;
using Windows.UI;

namespace PlottingLib
{
    public abstract class BaseGraph : IDisposable
    {
        public static ExamColorMap ColorMap = new ExamColorMap();
        
        public Color CurrentColor { get; set; }

        public int YStart { get; set; }

        public int XStart { get; set; }

        public int SizeOfQueue { get; set; }

        public int ExamProcedureId { get; set; }

        public bool IsLeftChannel { get; set; }

        private bool displayGraph = true;

        /// <summary>
        /// Gets or sets a value indicating whether [display graph].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display graph]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayGraph
        {
            get { return displayGraph; }
            set { displayGraph = value; }
        }

        /// <summary>
        /// Gets or sets the bitmap stream.
        /// </summary>
        /// <value>
        /// The bitmap stream.
        /// </value>
        public Stream BitmapStream { get; set; }

        /// <summary>
        /// Gets or sets the array pixel.
        /// </summary>
        /// <value>
        /// The array pixel.
        /// </value>
        public byte[] ArrayPixel { get; set; }

        /// <summary>
        /// Gets or sets the graph bitmap.
        /// </summary>
        /// <value>
        /// The graph bitmap.
        /// </value>
        public WriteableBitmap GraphBitmap { get; set; }

        /// <summary>
        /// Renders the graph.
        /// </summary>
        /// <param name="packet">The packet.</param>
        public abstract void RenderGraph(DMIPmdDataPacket packet);

        /// <summary>
        /// Gets or sets the current velocity range.
        /// </summary>
        /// <value>
        /// The current velocity range.
        /// </value>
        public double CurrentVelocityRange { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGraph" /> class.
        /// </summary>
        /// <param name="writeableBitmap">The writeable bitmap.</param>
        public BaseGraph(WriteableBitmap writeableBitmap)
        {            
            GraphBitmap = writeableBitmap;            
            ArrayPixel = new byte[GraphBitmap.PixelHeight * GraphBitmap.PixelWidth * Constants.BytesForColor];
        }

        protected BaseGraph()
        {
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            ArrayPixel = new byte[GraphBitmap.PixelHeight * GraphBitmap.PixelWidth * Constants.BytesForColor];
        }

        public virtual void Dispose()
        {       
            
            BitmapStream.Dispose();                      
            GraphBitmap = null;
            ArrayPixel = null;
        }
    }
}