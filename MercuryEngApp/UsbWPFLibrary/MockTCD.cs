// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="MockTCD.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UsbTcdLibrary
{
    /// <summary>
    /// Class MockTCD.
    /// </summary>
    internal class MockTCD
    {
        /// <summary>
        /// The is mock active
        /// </summary>
        internal bool isMockActive;
        /// <summary>
        /// The single packet data
        /// </summary>
        public PacketFormats.DMIPmdDataPacket singlePacketData = null;

        /// <summary>
        /// Occurs when [on packet formation dual].
        /// </summary>
        internal event TCDPacketFormed OnPacketFormationDual;

        /// <summary>
        /// The buffer
        /// </summary>
        private List<byte> buffer = null;
        /// <summary>
        /// The doppler object
        /// </summary>
        private DopplerModule dopplerObject = new DopplerModule();

        /// <summary>
        /// Reads the next synchronize.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="syncId">The synchronize identifier.</param>
        /// <returns>System.Byte.</returns>
        private byte ReadNextSync(ref byte[] arr, ref ulong syncId)
        {
            byte dump = 0;
            while (syncId != DMIProtocol.DMI_PACKET_SYNC)
            {
                buffer.RemoveAt(0);
                while (buffer.First() != DMIProtocol.SYNCID_FIRSTBYTE)
                {
                    buffer.RemoveAt(0);
                }
                if (buffer.Count() >= DMIProtocol.SYNC_SIZE)
                {
                    arr = buffer.Take<byte>(DMIProtocol.SYNC_SIZE).ToArray<byte>();
                    syncId = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                }
            }
            return dump;
        }

        /// <summary>
        /// Sends the packets from file.
        /// </summary>
        public async void SendPacketsFromFile()
        {
            string fileName = "5kHz.txt";
            StorageFile readFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
            byte[] incomingArray = null;
            if (readFile != null)
            {
                using (var stream = await readFile.OpenReadAsync())
                {
                    incomingArray = new byte[stream.Size];
                    using (var reader = new DataReader(stream))
                    {
                        await reader.LoadAsync((uint)stream.Size);
                        reader.ReadBytes(incomingArray);
                    }
                }
            }

            buffer = new List<byte>(incomingArray);

            while (buffer.Count() >= DMIProtocol.FilePacketSize)
            {
                if (isMockActive)
                {
                    byte[] arr = buffer.Take<byte>(DMIProtocol.SYNC_SIZE).ToArray<byte>();
                    PacketFormats.DMIPmdDataPacket[] packets = new PacketFormats.DMIPmdDataPacket[2];
                    ulong syncId = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                    ReadNextSync(ref arr, ref syncId);

                    if (syncId == DMIProtocol.DMI_PACKET_SYNC)
                    {
                        singlePacketData = dopplerObject.GetShortCVRPacket(buffer.Take<byte>(DMIProtocol.FilePacketSize).ToArray<byte>());
                        packets[0] = singlePacketData;
                        packets[1] = singlePacketData;
                        OnPacketFormationDual(packets);
                        await Task.Delay(Constants.VALUE_8);
                    }
                    buffer.RemoveRange(Constants.VALUE_0, DMIProtocol.FilePacketSize);
                    if (buffer.Count < DMIProtocol.FilePacketSize)
                    {
                        // check if there is any residual
                        buffer = new List<byte>(incomingArray);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}