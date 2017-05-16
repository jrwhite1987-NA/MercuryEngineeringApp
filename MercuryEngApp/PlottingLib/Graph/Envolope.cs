// ***********************************************************************
// Assembly         : Mercury
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="Envolope.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using UsbTcdLibrary;
using UsbTcdLibrary.PacketFormats;

namespace PlottingLib
{
    /// <summary>
    /// Class Envolope.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class Envolope : IDisposable
    {
        #region Constants

        /// <summary>
        /// The current velocity range 3080
        /// </summary>
        private const int CURRENT_VELOCITY_RANGE_3080 = 3080;
        /// <summary>
        /// The negative factor
        /// </summary>
        private const int NEGATIVE_FACTOR = -1;
        /// <summary>
        /// The offset 1
        /// </summary>
        private const int OFFSET_1 = 1;
        /// <summary>
        /// The offset 2
        /// </summary>
        private const int OFFSET_2 = 2;
        /// <summary>
        /// The offset 3
        /// </summary>
        private const int OFFSET_3 = 3;

        #endregion Constants

        /// <summary>
        /// Gets or sets the envelop data points.
        /// </summary>
        /// <value>The envelop data points.</value>
        public List<int> EnvelopDataPoints { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [positive flow visible].
        /// </summary>
        /// <value><c>true</c> if [positive flow visible]; otherwise, <c>false</c>.</value>
        public bool PositiveFlowVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [negative flow visible].
        /// </summary>
        /// <value><c>true</c> if [negative flow visible]; otherwise, <c>false</c>.</value>
        public bool NegativeFlowVisible { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Envolope"/> class.
        /// </summary>
        public Envolope()
        {
            EnvelopDataPoints = new List<int>();
            NegativeFlowVisible = true;
            PositiveFlowVisible = true;
        }

        /// <summary>
        /// Plots the envolope.
        /// </summary>
        /// <param name="spectrum">The spectrum.</param>
        /// <param name="packet">The packet.</param>
        /// <param name="columnOffset">The column offset.</param>
        /// <param name="pixelsFactor">The pixels factor.</param>
        public void PlotEnvolope(Spectrogram spectrum, DMIPmdDataPacket packet, int columnOffset, int pixelsFactor)
        {
            ///*
            // * PRF needs to be checked here. Please use the following as a reference:
            // * 5.0KHz -> 1920.0
            // * 6.25KHz -> 2400.0
            // * 8.0KHz -> 3080.0
            // * 10.0KHz -> 3850.0
            // * 12.5KHz -> 4800.0
            // */
            spectrum.CurrentVelocityRange = (spectrum.CurrentVelocityRange == 0 ? CURRENT_VELOCITY_RANGE_3080 : spectrum.CurrentVelocityRange);

            ShowPositiveEnvelope(spectrum, packet, pixelsFactor);
            ShowNegativeEnvelope(spectrum, packet, pixelsFactor);
        }

        /// <summary>
        /// Shows the negative envelope.
        /// </summary>
        /// <param name="spectrum">The spectrum.</param>
        /// <param name="packet">The packet.</param>
        /// <param name="pixelsFactor">The pixels factor.</param>
        private void ShowNegativeEnvelope(Spectrogram spectrum, DMIPmdDataPacket packet, int pixelsFactor)
        {
            var baseLineOffset = spectrum.BaseLinePosition == DMIProtocol.FFTSize ? 0 : spectrum.BaseLinePosition;
            int currentPoint = 0;
            int prevPoint = 0;
            int start = 0;
            int end = 0;
            if (NegativeFlowVisible)
            {
                //calculate negative envelope position
                currentPoint = (int)((packet.envelope.negVelocity * NEGATIVE_FACTOR / spectrum.CurrentVelocityRange) * Constants.FFTSize);
                currentPoint = (Constants.FFTSize / OFFSET_2) + currentPoint;
                currentPoint = (currentPoint < 0 ? currentPoint * NEGATIVE_FACTOR : currentPoint) - baseLineOffset;
                currentPoint = (currentPoint > Constants.FFTSize) ? currentPoint - Constants.FFTSize : currentPoint;

                prevPoint = EnvelopDataPoints.Count <= OFFSET_1 ? currentPoint : EnvelopDataPoints[EnvelopDataPoints.Count - OFFSET_2];

                start = currentPoint >= prevPoint ? prevPoint : currentPoint;
                end = currentPoint >= prevPoint ? currentPoint : prevPoint;
                int columnOffset = spectrum.XStart * Constants.BytesForColor;
                //draw negative envelope
                if (ShouldDrawLine(start, end, spectrum.BaseLinePosition))
                {
                    for (int i = start; i < end + 1; i++)
                    {
                        if (i < Constants.FFTSize && i >= 0)
                        {
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor] = Colors.White.B; // B Value
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor + OFFSET_1] = Colors.White.G; // G Value
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor + OFFSET_2] = Colors.White.R; // R Value
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor + OFFSET_3] = Colors.White.A;// A Value
                        }
                    }
                }
            }
            EnvelopDataPoints.Add(currentPoint);
        }

        /// <summary>
        /// Shows the positive envelope.
        /// </summary>
        /// <param name="spectrum">The spectrum.</param>
        /// <param name="packet">The packet.</param>
        /// <param name="pixelsFactor">The pixels factor.</param>
        private void ShowPositiveEnvelope(Spectrogram spectrum, DMIPmdDataPacket packet, int pixelsFactor)
        {
            var baseLineOffset = spectrum.BaseLinePosition == DMIProtocol.FFTSize ? 0 : spectrum.BaseLinePosition;
            int currentPoint = 0;
            int prevPoint = 0;
            int start = 0;
            int end = 0;
            if (PositiveFlowVisible)
            {
                //calculate positive envelope position
                currentPoint = (int)((packet.envelope.posVelocity / spectrum.CurrentVelocityRange) * Constants.FFTSize);
                currentPoint = (Constants.FFTSize / Constants.VALUE_2) - currentPoint;

                currentPoint -= baseLineOffset;
                currentPoint = (currentPoint < 0) ? currentPoint + Constants.FFTSize : currentPoint;

                prevPoint = EnvelopDataPoints.Count <= 1 ? currentPoint : EnvelopDataPoints[EnvelopDataPoints.Count - Constants.VALUE_2];
                start = currentPoint >= prevPoint ? prevPoint : currentPoint;
                end = currentPoint >= prevPoint ? currentPoint : prevPoint;
                //reset offset and plot positive envelope
                int columnOffset = spectrum.XStart * Constants.BytesForColor;

                if (ShouldDrawLine(start, end, spectrum.BaseLinePosition))
                {
                    for (int i = start; i < end + OFFSET_1; i++)
                    {
                        if (i < Constants.FFTSize && i >= 0)
                        {
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor] = Colors.White.B; // B Value
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor + OFFSET_1] = Colors.White.G; // G Value
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor + OFFSET_2] = Colors.White.R; // R Value
                            spectrum.ArrayPixel[columnOffset + i * pixelsFactor + OFFSET_3] = Colors.White.A;// A Value
                        }
                    }
                }
            }
            EnvelopDataPoints.Add(currentPoint);
        }

        /// <summary>
        /// Shoulds the draw line.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="baseLinePosition">The base line position.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ShouldDrawLine(int start, int end, int baseLinePosition)
        {
            bool result = true;
            var halfFFTSize = Constants.FFTSize / 2;
            var diff = start - end;

            if ((diff > halfFFTSize) ||
                (diff < -halfFFTSize) ||
                ((halfFFTSize - baseLinePosition == start) || (halfFFTSize - baseLinePosition == end)))
            {
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Clears EnvelopDatapoints and set to null
        /// </summary>
        public void Dispose()
        {
            EnvelopDataPoints.Clear();
            EnvelopDataPoints = null;
        }
    }
}