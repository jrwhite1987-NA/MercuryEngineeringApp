// ***********************************************************************
// Assembly         : UsbTcdLibrary
// Author           : Jagtap_R
// Created          : 03-09-2017
//
// Last Modified By : Jagtap_R
// Last Modified On : 04-06-2017
// ***********************************************************************
// <copyright file="DopplerModule.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using Core.Common;
using Core.Constants;
using Core.Models.ReportModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UsbTcdLibrary.PacketFormats;
using UsbTcdLibrary.StatusClasses;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace UsbTcdLibrary
{
    /// <summary>
    /// Class DopplerModule.
    /// </summary>
    public class DopplerModule
    {
        #region Variables

        /// <summary>
        /// The check probe
        /// </summary>
        internal bool checkProbe;

        /// <summary>
        /// Occurs when [on probe plugged].
        /// </summary>
        internal event ProbePlugUnplug OnProbePlugged;

        /// <summary>
        /// Occurs when [on probe unplugged].
        /// </summary>
        internal event ProbePlugUnplug OnProbeUnplugged;

        /// <summary>
        /// The check probe timer
        /// </summary>
        internal ThreadPoolTimer CheckProbeTimer;

        byte[] leftPacket;
        byte[] rightPacket;

        /// <summary>
        /// The packet queue channel1
        /// </summary>
        static internal List<DMIPmdDataPacket> packetQueueChannel1 = new List<DMIPmdDataPacket>();

        /// <summary>
        /// The packet queue channel2
        /// </summary>
        static internal List<DMIPmdDataPacket> packetQueueChannel2 = new List<DMIPmdDataPacket>();

        /// <summary>
        /// The CVR data channel1
        /// </summary>
        static internal List<short> cvrDataChannel1 = new List<short>();
        /// <summary>
        /// The CVR data channel2
        /// </summary>
        static internal List<short> cvrDataChannel2 = new List<short>();

        /// <summary>
        /// The packet queue
        /// </summary>
        internal Dictionary<int, List<DMIPmdDataPacket>> packetQueue = new Dictionary<int, List<DMIPmdDataPacket>>();

        /// <summary>
        /// The read file buffer
        /// </summary>
        static internal ConcurrentQueue<byte> readFileBuffer = new ConcurrentQueue<byte>();

        /// <summary>
        /// The counter packet form
        /// </summary>
        private uint counterPacketForm = 0;

        /// <summary>
        /// The counter packet write
        /// </summary>
        private static int counterPacketWrite = 0;
        internal int exportPacketCount = 0;
        /// <summary>
        /// The sequence left packet
        /// </summary>
        private ushort SequenceLeftPacket = 0;

        /// <summary>
        /// The sequence right packet
        /// </summary>
        private ushort SequenceRightPacket = 0;


        /// <summary>
        /// The MSG timer
        /// </summary>
        //private DispatcherTimer msgTimer = new DispatcherTimer();

        /// <summary>
        /// The left buffer
        /// </summary>
        private Queue<byte[]> leftBuffer = new Queue<byte[]>();

        /// <summary>
        /// The right buffer
        /// </summary>
        private Queue<byte[]> rightBuffer = new Queue<byte[]>();

        /// <summary>
        /// The t1
        /// </summary>
        public static Stopwatch t1 = new Stopwatch();

        /// <summary>
        /// Occurs when [on packet formation dual].
        /// </summary>
        internal event TCDPacketFormed OnPacketFormationDual;

        /// <summary>
        /// Occurs when [on recording enabled].
        /// </summary>
        internal event RecordPacketDelegate OnRecordingEnabled;

        /// <summary>
        /// The packets
        /// </summary>
        private DMIPmdDataPacket[] packets = new DMIPmdDataPacket[Constants.VALUE_2];

        /// <summary>
        /// The file write stream1
        /// </summary>
        private FileRandomAccessStream fileWriteStream1;

        /// <summary>
        /// The file write stream2
        /// </summary>
        private FileRandomAccessStream fileWriteStream2;

        /// <summary>
        /// The write file1
        /// </summary>
        private StorageFile writeFile1;

        /// <summary>
        /// The write file2
        /// </summary>
        private StorageFile writeFile2;



        /// <summary>
        /// The packet arr
        /// </summary>
        private DMIPmdDataPacket[] packetArr;

        /// <summary>
        /// The write data lock
        /// </summary>
        private static object writeDataLock = new object();

        /// <summary>
        /// Gets the active channel.
        /// </summary>
        /// <value>
        /// The active channel.
        /// </value>
        public ActiveChannels ActiveChannel = ActiveChannels.None;


        #endregion Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DopplerModule" /> class.
        /// </summary>
        public DopplerModule()
        {
            packets[0] = new DMIPmdDataPacket();
            packets[1] = new DMIPmdDataPacket();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> ReadFile(int examId, int channelId)
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("Get file for examId:" + examId.ToString() + " channelId: " + channelId.ToString(), "ReadFile", Severity.Debug);

                string fileName = String.Format("{0}-Channel{1}.txt", examId, channelId);

                if (channelId == DMIProtocol.DMI_CHANNEL_ONE)
                {
                    packetQueueChannel1.Clear();
                }
                else if (channelId == DMIProtocol.DMI_CHANNEL_TWO)
                {
                    packetQueueChannel2.Clear();
                }
                else
                {
                    //Logs.Instance.ErrorLog<DopplerModule>("Recieved garbage value for Channel|" + channelId.ToString() + "|"
                    //  + ExecutionConstants.MethodExecutionFail, "ReadFile", Severity.Warning);
                }

                StorageFile readFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

                if (readFile != null)
                {
                    //Logs.Instance.ErrorLog<DopplerModule>("Got storage file with file name:" + readFile.Name, "ReadFile", Severity.Debug);
                    byte[] fileBytes = await LoadFileData(channelId, readFile);

                    if (fileBytes != null)
                    {
                        ConvertToPacket(channelId, fileBytes);
                    }
                }
                else
                {
                    //Logs.Instance.ErrorLog<DopplerModule>("Got storage file with file name:" + (readFile.Name != null ? readFile.Name : string.Empty), "ReadFile", Severity.Debug);
                }
                return true;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFile", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Reads the file CVR.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>Task&lt;System.UInt64&gt;.</returns>
        /// <exception cref="Exception">Synchronization error</exception>
        internal async Task<ulong> ReadFileCVR(int examId, int channelId)
        {
            ulong numPackets = 0;
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("Get file for examId:" + examId.ToString() + " channelId: " + channelId.ToString(), "ReadFile", Severity.Debug);

                string fileName = String.Format("{0}-Channel{1}.txt", examId, channelId);

                if (channelId == DMIProtocol.DMI_CHANNEL_ONE)
                {
                    cvrDataChannel1.Clear();
                }
                else if (channelId == DMIProtocol.DMI_CHANNEL_TWO)
                {
                    cvrDataChannel2.Clear();
                }
                else
                {
                    //Logs.Instance.ErrorLog<DopplerModule>("Recieved garbage value for Channel|" + channelId.ToString() + "|"
                    //   + ExecutionConstants.MethodExecutionFail, "ReadFile", Severity.Warning);
                }

                StorageFile readFile = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                List<short> cvrData = new List<short>();

                if (readFile != null)
                {
                    //Logs.Instance.ErrorLog<DopplerModule>("Got storage file with file name:" + readFile.Name, "ReadFile", Severity.Debug);
                    using (var stream = await readFile.OpenReadAsync())
                    {
                        //read a single packet at a time and append the positive mean velocity to the list
                        numPackets = stream.Size / ((ulong)DMIProtocol.FilePacketSize);
                        int j = 0;
                        int interval = (int)(Math.Ceiling(numPackets / (double)Constants.CVRBitmapWidth));

                        for (ulong i = 0; i < numPackets; i++)
                        {
                            if (j == interval)
                            {
                                var packetStream = stream.GetInputStreamAt(i * ((ulong)DMIProtocol.FilePacketSize));
                                byte[] packetBytes = new byte[DMIProtocol.FilePacketSize];
                                using (var reader = new DataReader(packetStream))
                                {
                                    await reader.LoadAsync((uint)DMIProtocol.FilePacketSize);
                                    reader.ReadBytes(packetBytes);
                                    ulong syncId = BitConverter.ToUInt64(packetBytes, Constants.VALUE_0);

                                    //if we're out of sync, the data is corrupt and we should ignore it
                                    if (syncId != DMIProtocol.DMI_PACKET_SYNC)
                                    {
                                        throw new Exception("Synchronization error");
                                    }
                                }

                                if (packetBytes != null)
                                {
                                    cvrData.Add(BitConverter.ToInt16(packetBytes, Envelop.PosMean));
                                }

                                j = 0;
                            }
                            j++;
                        }
                    }
                    //copy the list to the corresponding data structure
                    if (channelId == DMIProtocol.DMI_CHANNEL_ONE)
                    {
                        cvrDataChannel1 = cvrData;
                    }
                    if (channelId == DMIProtocol.DMI_CHANNEL_TWO)
                    {
                        cvrDataChannel2 = cvrData;
                    }
                }
                else
                {
                    //Logs.Instance.ErrorLog<DopplerModule>("Got storage file with file name:" + (readFile.Name != null ? readFile.Name : string.Empty), "ReadFile", Severity.Debug);
                }
                return numPackets;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFile", Severity.Warning);
                return numPackets;
            }
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <param name="ReadPointerListCh1">The read pointer list CH1.</param>
        /// <param name="ReadPointerListCh2">The read pointer list CH2.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> ReadFile(int examId, Dictionary<int, int> ReadPointerListCh1, Dictionary<int, int> ReadPointerListCh2)
        {
            bool result = false;
            try
            {
                if (ReadPointerListCh1.Count > 0)
                {
                    packetQueueChannel1.Clear();
                    string fileNameChannel1 = String.Format("{0}-Channel{1}.txt", examId, Constants.VALUE_1);
                    StorageFile readFileCh1 = await ApplicationData.Current.LocalFolder.GetFileAsync(fileNameChannel1);
                    result = await LoadFileData(Constants.VALUE_1, readFileCh1, ReadPointerListCh1);
                }
                if (ReadPointerListCh2.Count > 0)
                {
                    packetQueueChannel2.Clear();
                    string fileNameChannel2 = String.Format("{0}-Channel{1}.txt", examId, Constants.VALUE_2);
                    StorageFile readFileCh2 = await ApplicationData.Current.LocalFolder.GetFileAsync(fileNameChannel2);
                    result = await LoadFileData(Constants.VALUE_2, readFileCh2, ReadPointerListCh2);
                }
                return result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFile", Severity.Warning);
                return result;
            }
        }

        /// <summary>
        /// Read Binary file for exam with Range for 4/8/12 Sec
        /// </summary>
        /// <param name="examId">exam Id</param>
        /// <param name="ListReadPointerModelCh1">Exam screen shots parameter for ch1</param>
        /// <param name="ListReadPointerModelCh2">Exam screen shots parameter for ch2</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> ReadFileWithRange(int examId, List<ReadPointerModel> ListReadPointerModelCh1, List<ReadPointerModel> ListReadPointerModelCh2)
        {
            bool result = false;
            try
            {
                if (ListReadPointerModelCh1.Count > 0)
                {
                    packetQueueChannel1.Clear();
                    string fileNameChannel1 = String.Format("{0}-Channel{1}.txt", examId, Constants.VALUE_1);
                    StorageFile readFileCh1 = await ApplicationData.Current.LocalFolder.GetFileAsync(fileNameChannel1);
                    result = await LoadFileData(Constants.VALUE_1, readFileCh1, ListReadPointerModelCh1);
                }
                if (ListReadPointerModelCh2.Count > 0)
                {
                    packetQueueChannel2.Clear();
                    string fileNameChannel2 = String.Format("{0}-Channel{1}.txt", examId, Constants.VALUE_2);
                    StorageFile readFileCh2 = await ApplicationData.Current.LocalFolder.GetFileAsync(fileNameChannel2);
                    result = await LoadFileData(Constants.VALUE_2, readFileCh2, ListReadPointerModelCh2);
                }
                return result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFile", Severity.Warning);
                return result;
            }
        }



        /// <summary>
        /// Read Binary file for exam with Range for 4/8/12 Sec
        /// </summary>
        /// <param name="examId">exam Id</param>
        /// <param name="ListReadPointerModelCh1">Exam screen shots parameter for ch1</param>
        /// <param name="ListReadPointerModelCh2">Exam screen shots parameter for ch2</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> ReadFileWithRange(int examId, List<ReadPointerModel> ListReadPointerModelCh1)
        {
            bool result = false;
            try
            {
                if (ListReadPointerModelCh1.Count > 0)
                {
                    packetQueueChannel1.Clear();
                    string fileNameChannel1 = String.Format("{0}-Channel{1}.txt", examId, Constants.VALUE_1);
                    string filePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"LocalFolder\" + fileNameChannel1);
                    result = await LoadFileData(Constants.VALUE_1, filePath, ListReadPointerModelCh1);
                }
                return result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFile", Severity.Warning);
                return result;
            }
        }

        internal void ReadPacketFromFile(string fileName, int offsetByte, ref byte[] byteArray)
        {
            try
            {
                LoadDataFromFile(fileName, offsetByte, ref byteArray);
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Converts to packet.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="fileBytes">The file bytes.</param>
        private void ConvertToPacket(int channelId, byte[] fileBytes)
        {
            int i = 0;
            byte dump;
            byte[] tempArray = new byte[DMIProtocol.PACKET_SIZE];
            //Logs.Instance.ErrorLog<DopplerModule>("Enqueue all bytes to File Buffer for Channel|" + channelId.ToString(), "ConvertToPacket", Severity.Debug);
            for (i = Constants.VALUE_0; i < fileBytes.Length; i++)
            {
                DopplerModule.readFileBuffer.Enqueue(fileBytes[i]);
            }
            //Logs.Instance.ErrorLog<DopplerModule>("Packet conversion started for channel:" + channelId.ToString(), "ConvertToPacket", Severity.Debug);
            while (readFileBuffer.Count() >= DMIProtocol.FilePacketSize)
            {
                byte[] arr = readFileBuffer.Take<byte>(DMIProtocol.SYNC_SIZE).ToArray<byte>();
                ulong syncId = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                dump = ReadNextSync(ref arr, ref syncId);
                if (syncId == DMIProtocol.DMI_PACKET_SYNC)
                {
                    PacketEnqueue(channelId, ref dump, ref tempArray);
                }
            }
            //Logs.Instance.ErrorLog<DopplerModule>("Packet conversion completed for Channel|" + channelId.ToString(), "ConvertToPacket", Severity.Debug);
        }

        /// <summary>
        /// Extracts the CVR information.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="fileBytes">The file bytes.</param>
        private void ExtractCVRInfo(int channelId, byte[] fileBytes)
        {
            int i = 0;
            byte dump;
            byte[] tempArray = new byte[DMIProtocol.PACKET_SIZE];
            //Logs.Instance.ErrorLog<DopplerModule>("Enqueue all bytes to File Buffer for Channel|" + channelId.ToString(), "ConvertToPacket", Severity.Debug);
            for (i = Constants.VALUE_0; i < fileBytes.Length; i++)
            {
                readFileBuffer.Enqueue(fileBytes[i]);
            }
            //Logs.Instance.ErrorLog<DopplerModule>("Packet conversion started for channel:" + channelId.ToString(), "ConvertToPacket", Severity.Debug);
            while (readFileBuffer.Count() >= DMIProtocol.FilePacketSize)
            {
                byte[] arr = readFileBuffer.Take<byte>(DMIProtocol.SYNC_SIZE).ToArray<byte>();
                ulong syncId = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                dump = ReadNextSync(ref arr, ref syncId);
                if (syncId == DMIProtocol.DMI_PACKET_SYNC)
                {
                    CVREnqueue(channelId, ref dump, ref tempArray);
                }
            }
            //Logs.Instance.ErrorLog<DopplerModule>("Packet conversion completed for Channel|" + channelId.ToString(), "ConvertToPacket", Severity.Debug);
        }

        /// <summary>
        /// Packets the enqueue.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="dump">The dump.</param>
        /// <param name="tempArray">The temporary array.</param>
        private void PacketEnqueue(int channelId, ref byte dump, ref byte[] tempArray)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("create packetQueue for channelId| " + channelId.ToString(), "PacketEnqueue", Severity.Debug);

            tempArray = readFileBuffer.Take<byte>(DMIProtocol.FilePacketSize).ToArray<byte>();
            for (int l = Constants.VALUE_0; l < DMIProtocol.FilePacketSize; l++)
            {
                readFileBuffer.TryDequeue(out dump);
            }

            if (tempArray.Length >= DMIProtocol.FilePacketSize)
            {
                if (channelId == DMIProtocol.DMI_CHANNEL_ONE)
                {
                    packetQueueChannel1.Add(GetShortCVRPacket(tempArray));
                }
                if (channelId == DMIProtocol.DMI_CHANNEL_TWO)
                {
                    packetQueueChannel2.Add(GetShortCVRPacket(tempArray));
                }
            }

            //Logs.Instance.ErrorLog<DopplerModule>("packetQueue creation completed for channelId| " + channelId.ToString(), "PacketEnqueue", Severity.Debug);
        }

        /// <summary>
        /// CVRs the enqueue.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="dump">The dump.</param>
        /// <param name="tempArray">The temporary array.</param>
        private void CVREnqueue(int channelId, ref byte dump, ref byte[] tempArray)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("create packetQueue for channelId| " + channelId.ToString(), "PacketEnqueue", Severity.Debug);

            tempArray = readFileBuffer.Take<byte>(DMIProtocol.FilePacketSize).ToArray<byte>();
            for (int l = Constants.VALUE_0; l < DMIProtocol.FilePacketSize; l++)
            {
                readFileBuffer.TryDequeue(out dump);
            }

            if (tempArray.Length >= DMIProtocol.FilePacketSize)
            {
                if (channelId == DMIProtocol.DMI_CHANNEL_ONE)
                {
                    cvrDataChannel1.Add(BitConverter.ToInt16(tempArray, Envelop.PosMean));
                }
                if (channelId == DMIProtocol.DMI_CHANNEL_TWO)
                {
                    cvrDataChannel2.Add(BitConverter.ToInt16(tempArray, Envelop.PosMean));
                }
            }

            //Logs.Instance.ErrorLog<DopplerModule>("packetQueue creation completed for channelId| " + channelId.ToString(), "PacketEnqueue", Severity.Debug);
        }

        /// <summary>
        /// Reads the next synchronize.
        /// </summary>
        /// <param name="arr">The arr.</param>
        /// <param name="syncId">The synchronize identifier.</param>
        /// <returns>System.Byte.</returns>
        private static byte ReadNextSync(ref byte[] arr, ref ulong syncId)
        {
            byte dump = 0;
            //Logs.Instance.ErrorLog<DopplerModule>("Read Next Sync begins for syncId| " + syncId.ToString(), "ReadNextSync", Severity.Debug);
            while (syncId != DMIProtocol.DMI_PACKET_SYNC)
            {
                readFileBuffer.TryDequeue(out dump);
                while (readFileBuffer.First() != DMIProtocol.SYNCID_FIRSTBYTE)
                {
                    readFileBuffer.TryDequeue(out dump);
                }
                if (readFileBuffer.Count() >= DMIProtocol.SYNC_SIZE)
                {
                    arr = readFileBuffer.Take<byte>(DMIProtocol.SYNC_SIZE).ToArray<byte>();
                    syncId = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                }
            }
            //Logs.Instance.ErrorLog<DopplerModule>("Read Next Sync ends for syncId| " + syncId.ToString(), "ReadNextSync", Severity.Debug);
            return dump;
        }

        /// <summary>
        /// Loads the file data.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="readFile">The read file.</param>
        /// <returns>Task&lt;System.Byte[]&gt;.</returns>
        private static async Task<byte[]> LoadFileData(int channelId, StorageFile readFile)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("await to load file data- file name:" +
            //  (readFile.Name != null ? readFile.Name : string.Empty), "ReadFile", Severity.Debug);
            byte[] fileBytes = null;
            try
            {
                //Reads the entire file
                using (var stream = await readFile.OpenReadAsync())
                {
                    fileBytes = new byte[stream.Size];
                    using (var reader = new DataReader(stream))
                    {
                        //Logs.Instance.ErrorLog<DopplerModule>("reader.LoadAsync for Channel|" + channelId.ToString() +
                        // "|" + ExecutionConstants.MethodExecutionFail, "LoadFileData",
                        //Severity.Debug);
                        await reader.LoadAsync((uint)stream.Size);
                        reader.ReadBytes(fileBytes);
                    }
                }

                //Logs.Instance.ErrorLog<DopplerModule>("Loaded data of size: " +
                //  (fileBytes != null ? fileBytes.Length.ToString() : string.Empty), "LoadFileData", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "LoadFileData", Severity.Critical);
            }
            return fileBytes;
        }

        /// <summary>
        /// Loads the file data.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="readFile">The read file.</param>
        /// <param name="readFromByte">The read from byte.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> LoadFileData(int channelId, StorageFile readFile, Dictionary<int, int> readFromByte)
        {
            byte[] fileBytes = null;
            try
            {
                //Reads required bytes from given position
                using (Stream stream = (await readFile.OpenReadAsync()).AsStreamForRead())
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        foreach (int snapshotId in readFromByte.Keys)
                        {
                            reader.BaseStream.Seek(readFromByte[snapshotId], SeekOrigin.Begin);
                            fileBytes = reader.ReadBytes(DMIProtocol.FilePacketSize * Constants.VALUE_500);
                            if (fileBytes != null)
                            {
                                if (channelId == Constants.VALUE_1)
                                {
                                    ConvertToPacket(Constants.VALUE_1, fileBytes);
                                    packetQueue.Add(snapshotId, packetQueueChannel1.ToList());
                                    packetQueueChannel1.Clear();
                                }
                                else
                                {
                                    ConvertToPacket(Constants.VALUE_2, fileBytes);
                                    packetQueue.Add(snapshotId, packetQueueChannel2.ToList());
                                    packetQueueChannel2.Clear();
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "LoadFileData", Severity.Critical);
                return false;
            }
            finally
            {
                packetQueueChannel1.Clear();
                packetQueueChannel2.Clear();
                fileBytes = null;
            }
        }

        /// <summary>
        /// Load bytes for snapshot seleted
        /// </summary>
        /// <param name="channelId">channel Id</param>
        /// <param name="readFile">exam binary file</param>
        /// <param name="readPointerList">snap shot paramter</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> LoadFileData(int channelId, StorageFile readFile, List<ReadPointerModel> readPointerList)
        {
            byte[] fileBytes = null;
            try
            {
                //Reads required bytes from given position
                using (Stream stream = (await readFile.OpenReadAsync()).AsStreamForRead())
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        foreach (ReadPointerModel readPointer in readPointerList)
                        {
                            reader.BaseStream.Seek(readPointer.OffsetByte, SeekOrigin.Begin);

                            if (readPointer.RangeOffsetByte > stream.Length)
                            {
                                fileBytes = reader.ReadBytes((int)stream.Length - 1);
                            }
                            else
                            {
                                fileBytes = reader.ReadBytes(readPointer.RangeOffsetByte);
                            }

                            if (fileBytes != null)
                            {
                                if (channelId == Constants.VALUE_1)
                                {
                                    ConvertToPacket(Constants.VALUE_1, fileBytes);
                                    packetQueue.Add(readPointer.ExamSnapShotId, packetQueueChannel1.ToList());
                                    packetQueueChannel1.Clear();
                                }
                                else
                                {
                                    ConvertToPacket(Constants.VALUE_2, fileBytes);
                                    packetQueue.Add(readPointer.ExamSnapShotId, packetQueueChannel2.ToList());
                                    packetQueueChannel2.Clear();
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "LoadFileData", Severity.Critical);
                return false;
            }
            finally
            {
                packetQueueChannel1.Clear();
                packetQueueChannel2.Clear();
                fileBytes = null;
            }
        }

        /// <summary>
        /// Load bytes for snapshot seleted
        /// </summary>
        /// <param name="channelId">channel Id</param>
        /// <param name="readFile">exam binary file</param>
        /// <param name="readPointerList">snap shot paramter</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        private async Task<bool> LoadFileData(int channelId, string FilePath, List<ReadPointerModel> readPointerList)
        {
            byte[] fileBytes = null;
            try
            {
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                //Reads required bytes from given position
                using (Stream stream = fs)
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        foreach (ReadPointerModel readPointer in readPointerList)
                        {
                            reader.BaseStream.Seek(readPointer.OffsetByte, SeekOrigin.Begin);

                            if (readPointer.RangeOffsetByte > stream.Length)
                            {
                                fileBytes = reader.ReadBytes((int)stream.Length - 1);
                            }
                            else
                            {
                                fileBytes = reader.ReadBytes(readPointer.RangeOffsetByte);
                            }

                            if (fileBytes != null)
                            {
                                if (channelId == Constants.VALUE_1)
                                {
                                    ConvertToPacket(Constants.VALUE_1, fileBytes);
                                    packetQueue.Add(readPointer.ExamSnapShotId, packetQueueChannel1.ToList());
                                    packetQueueChannel1.Clear();
                                }
                                else
                                {
                                    ConvertToPacket(Constants.VALUE_2, fileBytes);
                                    packetQueue.Add(readPointer.ExamSnapShotId, packetQueueChannel2.ToList());
                                    packetQueueChannel2.Clear();
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "LoadFileData", Severity.Critical);
                return false;
            }
            finally
            {
                packetQueueChannel1.Clear();
                packetQueueChannel2.Clear();
                fileBytes = null;
            }
        }

        private void LoadDataFromFile(string filePath, int offsetByte, ref byte[] byteArray)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //Reads required bytes from given position
                using (Stream stream = fs)
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        reader.BaseStream.Seek(offsetByte, SeekOrigin.Begin);

                        if (offsetByte > stream.Length)
                        {
                            byteArray = reader.ReadBytes((int)stream.Length - 1);
                        }
                        else
                        {
                            byteArray = reader.ReadBytes(1132);
                        }

                        if (byteArray != null)
                        {
                            ConvertToPacket(Constants.VALUE_1, byteArray);
                            packetQueue.Add(0, packetQueueChannel1.ToList());
                            packetQueueChannel1.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "LoadFileData", Severity.Critical);               
            }
            finally
            {
                packetQueueChannel1.Clear();
                packetQueueChannel2.Clear();
            }
        }

        public void GetPacketDetails(byte[] byteArray)
        {
            if (byteArray != null)
            {
                ConvertToPacket(Constants.VALUE_1, byteArray);
                packetQueue.Add(0, packetQueueChannel1.ToList());
                packetQueueChannel1.Clear();
            }
        }

        /// <summary>
        /// Releases the TCD handle
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool ReleaseTCDHandle()
        {
            //Logs.Instance.ErrorLog<DopplerModule>("Release TCD Handle begins", "ReleaseTCDHandle", Severity.Debug);

            string RELEASE_READ_EVENT_HANDLER = "Released 'FormPacketsFromArray' and 'TCDHandler.Current.ReadTCDData' events handler.";
            try
            {
                OnPacketFormationDual = null;
                TCDHandler.OnTCDReadDual -= FormPacketsFromArray;
                TCDHandler.OnTCDReadDual -= TCDHandler.Current.ReadTCDData;
                //Logs.Instance.ErrorLog<DopplerModule>(RELEASE_READ_EVENT_HANDLER, "ReleaseTCDHandle", Severity.Debug);
                //Resting the packet form counter.
                counterPacketForm = 0;
                //Logs.Instance.ErrorLog<DopplerModule>(MessageConstants.ExamTCDOff, "ReleaseTCDHandle", Severity.Error);
                TCDHandler.Current.CloseDevice();
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReleaseTCDHandle", Severity.Warning);
            }
            finally
            {
                RELEASE_READ_EVENT_HANDLER = null;

                //Logs.Instance.ErrorLog<DopplerModule>("Release TCD Handle ends", "ReleaseTCDHandle", Severity.Debug);
            }
            return !TCDHandler.Current.isTCDWorking;
        }

        /// <summary>
        /// Initializes the probe events.
        /// </summary>
        /// <returns>Task.</returns>
        internal async Task InitializeProbeEvents()
        {
            await SendClearBufferCommand(TCDHandles.Channel1, EndpointNumber.EventMessage);
            await SendClearBufferCommand(TCDHandles.Channel2, EndpointNumber.EventMessage);
            InitializeProbeEvents(null);
            //CheckProbeTimer =
            //   ThreadPoolTimer.CreatePeriodicTimer(InitializeProbeEvents, TimeSpan.FromMinutes(Constants.VALUE_50));
        }

        /// <summary>
        /// Initializes the probe events.
        /// </summary>
        /// <param name="timer">The timer.</param>
        private async void InitializeProbeEvents(ThreadPoolTimer timer)
        //private async void InitializeProbeEvents1()
        {
            try
            {
                checkProbe = true;
                if (checkProbe)
                {
                    if (TCDHandler.Current.Channel1.TCDHandleChannel != null)
                    {
                        uint eventCodeCh1 = GetLogFromArray(await TCDHandler.Current.ReadServiceLog(TCDHandles.Channel1, Constants.VALUE_1))[0].Message.MessageCode;

                        if (eventCodeCh1 == DMIProtocol.DMI_EVENTCODE_PROBE_CONNECT)
                        {
                            await TCDHandler.Current.AssignDeviceInfo(TCDHandler.Current.Channel1.TCDHandleChannel);
                            OnProbePlugged(TCDHandles.Channel1);
                        }

                        if (eventCodeCh1 == DMIProtocol.DMI_EVENTCODE_PROBE_DISCONNECT)
                        {
                            TCDHandler.Current.DisableChannel(TCDHandles.Channel1);
                            OnProbeUnplugged(TCDHandles.Channel1);
                        }
                    }

                    if (TCDHandler.Current.Channel2.TCDHandleChannel != null)
                    {
                        uint eventCodeCh2 = GetLogFromArray(await TCDHandler.Current.ReadServiceLog(TCDHandles.Channel2, 1))[0].Message.MessageCode;

                        if (eventCodeCh2 == DMIProtocol.DMI_EVENTCODE_PROBE_CONNECT)
                        {
                            await TCDHandler.Current.AssignDeviceInfo(TCDHandler.Current.Channel2.TCDHandleChannel);
                            OnProbePlugged(TCDHandles.Channel2);
                        }

                        if (eventCodeCh2 == DMIProtocol.DMI_EVENTCODE_PROBE_DISCONNECT)
                        {
                            TCDHandler.Current.Channel2.ResetForSensing();
                            OnProbeUnplugged(TCDHandles.Channel2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "InitializeProbeEvents", Severity.Warning);
            }
        }

        /// <summary>
        /// Sends doppler command for which no data is required
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="content">The content.</param>
        /// <returns>System.UInt32.</returns>
        internal async Task<uint> SendDopplerCommand(TCDHandles channel, DopplerParameters parameter, uint content)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("SendDopplerCommand begins for channel:" + channel.ToString() + " Doppler parameter: " + parameter.ToString() + " content: "
            //  + content.ToString(),
            //"SendDopplerCommand", Severity.Debug);
            try
            {
                return await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_DOPPLER, (uint)parameter, content);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "SendDopplerCommand", Severity.Warning);
                return 0;
            }
        }

        /// <summary>
        /// Sets the PRF.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="PRF">The PRF.</param>
        /// <param name="startDepth">The start depth.</param>
        /// <returns>System.UInt32.</returns>
        internal async Task<uint> SetPRF(TCDHandles channel, uint PRF, byte startDepth)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("Set PRF begins for channel:" + channel.ToString() + " PRF : " + PRF.ToString() + " startDepth: " + startDepth.ToString(),
            //  "SetPRF", Severity.Debug);
            try
            {
                return await TCDHandler.Current.SetPRFAsync(channel, PRF, startDepth);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "SetPRF", Severity.Warning);
                return 0;
            }
        }

        /// <summary>
        /// Sets the envelope range.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="posMaxVelocity">The position maximum velocity.</param>
        /// <param name="negMaxVelocity">The neg maximum velocity.</param>
        /// <returns>System.UInt32.</returns>
        internal uint SetEnvelopeRange(TCDHandles channel, short posMaxVelocity, short negMaxVelocity)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("SetEnvelopeRange begins for channel:" + channel.ToString() + " posMaxVelocity : " + posMaxVelocity.ToString()
            // + " negMaxVelocity: " + negMaxVelocity.ToString(),
            //    "SetEnvelopeRange", Severity.Debug);

            try
            {
                return (TCDHandler.Current.SetEnvelopeRangeAsync(channel, posMaxVelocity, negMaxVelocity).Result);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "SetEnvelopeRange", Severity.Warning);
                return 0;
            }
        }

        /// <summary>
        /// Clears the buffer of the selected channel and endpoint
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.UInt32.</returns>
        internal async Task<uint> SendClearBufferCommand(TCDHandles channel, EndpointNumber endpoint)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("SendClearBufferCommand begins for channel:" + channel.ToString() + " endpoint : " + endpoint.ToString(),
            //   "SendClearBufferCommand", Severity.Debug);
            try
            {
                return await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_CLEAR_BUFFER, (uint)endpoint, 0);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "SendClearBufferCommand", Severity.Warning);
                return 0;
            }
        }

        /// <summary>
        /// Initialize the TCD channel by clearing all buffers and setting mode to operate
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        internal bool StartDopplerData()
        {
            //Logs.Instance.ErrorLog<DopplerModule>("StartDopplerData begins ", "StartDopplerData", Severity.Debug);

            try
            {
                TCDHandler.Current.SealProbes();
                if (TCDHandler.Current.Channel1.IsChannelEnabled)
                {
                    SendCommandToDoppler(TCDHandles.Channel1);
                }

                if (TCDHandler.Current.Channel2.IsChannelEnabled)
                {
                    SendCommandToDoppler(TCDHandles.Channel2);
                }
                return true;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "StartDopplerData", Severity.Warning);
                return false;
            }
        }

        /// <summary>
        /// Sends the command to doppler.
        /// </summary>
        /// <param name="channel">The channel.</param>
        private async void SendCommandToDoppler(TCDHandles channel)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("SendCommandToDoppler begins for channel:" + channel.ToString(), "SendCommandToDoppler", Severity.Debug);
            try
            {
                //Clear Buffer for all endpoints
                var result = await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_CLEAR_BUFFER, (uint)EndpointNumber.DopplerData, 0);
                result = await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_CLEAR_BUFFER, (uint)EndpointNumber.Bulk, 0);
                result = await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_CLEAR_BUFFER, (uint)EndpointNumber.EventMessage, 0);

                // Set mode of TCD to operate
                result = await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_SET_MODE, (uint)TCDModes.Operate, 0);

                //reset the timestamp
                result = await TCDHandler.Current.ResetTimeStampAsync(TCDHandles.Channel1, DateTime.Now);

                //Clear Doppler data buffer
                result = await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_CLEAR_BUFFER, (uint)EndpointNumber.DopplerData, 0);

                if (DMIProtocol.FFTSize == DMIProtocol.FFT256_POINTS)
                {
                    //Set the spectrogram bins to the appropriate value
                    result = await TCDHandler.Current.SendControlCommandAsync(channel, DMIProtocol.DMI_CMD_DOPPLER, (uint)DopplerParameters.SpectrumBins, (uint)DMIProtocol.FFTSize);
                    result = await TCDHandler.Current.SetEnvelopeRangeAsync(channel, Constants.VALUE_1540, -(Constants.VALUE_1540));
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "SendCommandToDoppler", Severity.Warning);
            }

            //Logs.Instance.ErrorLog<DopplerModule>("SendCommandToDoppler ends ", "SendCommandToDoppler", Severity.Debug);
        }

        /// <summary>
        /// Add event listeners and start reading from TCD
        /// </summary>
        internal void PacketsFromTCD()
        {
            //Logs.Instance.ErrorLog<DopplerModule>("async PacketsFromTCD begins", "PacketsFromTCD", Severity.Debug);
            try
            {
                if (TCDHandler.Current.Channel1.IsChannelEnabled | TCDHandler.Current.Channel2.IsChannelEnabled)
                {
                    TCDHandler.OnTCDReadDual += TCDHandler.Current.ReadTCDData;
                    TCDHandler.OnTCDReadDual += FormPacketsFromArray;
                    //handle messages
                    //msgTimer.Tick += MessageTimerTick;
                    //msgTimer.Interval = new TimeSpan(0, 0, 1);
                    TCDHandler.ChannelMessage += DecodeMessage;
                    t1.Start();

                    TCDHandler.Current.ReadTCDData().Start();
                    //msgTimer.Start();
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "PacketsFromTCD", Severity.Warning);
            }
            //Logs.Instance.ErrorLog<DopplerModule>("async PacketsFromTCD ends", "PacketsFromTCD", Severity.Debug);
        }

        /// <summary>
        /// Messages the timer tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private async void MessageTimerTick(object sender, object e)
        {
            await TCDHandler.Current.ReadMessageData();
        }

        /// <summary>
        /// Forms the packets from array.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        private Task<int> FormPacketsFromArray()
        {
            int result = Constants.VALUE_1;
            //checks if there is enough data to form a packet
            ulong syncId = Constants.VALUE_0;
            ulong syncIdR = Constants.VALUE_0;
            //Logs.Instance.ErrorLog<DopplerModule>("Packet formation from array begins", "FormPacketsFromArray", Severity.Debug);
            try
            {
                syncId = SyncLeftChannelData();

                syncIdR = SyncRightChannelData();

                if (syncId == DMIProtocol.DMI_PACKET_SYNC && syncIdR == DMIProtocol.DMI_PACKET_SYNC)
                {
                    if (CircularQueueChannel1.Channel1Queue.Count >= DMIProtocol.PACKET_SIZE
                        && CircularQueueChannel2.Channel2Queue.Count >= DMIProtocol.PACKET_SIZE)
                    {
                        PacketBackgroundThread(CircularQueueChannel1.Channel1Queue.DequeuePacket(),
                            CircularQueueChannel2.Channel2Queue.DequeuePacket());
                    }
                }
                else if (syncId == DMIProtocol.DMI_PACKET_SYNC && CircularQueueChannel1.Channel1Queue.Count >= DMIProtocol.PACKET_SIZE)
                {
                    PacketBackgroundThread(CircularQueueChannel1.Channel1Queue.DequeuePacket(), null);
                }
                else if (syncIdR == DMIProtocol.DMI_PACKET_SYNC && CircularQueueChannel2.Channel2Queue.Count >= DMIProtocol.PACKET_SIZE)
                {
                    PacketBackgroundThread(null, CircularQueueChannel2.Channel2Queue.DequeuePacket());
                }
                else
                {
                    PacketBackgroundThread(null, null);
                }
                //Logs.Instance.ErrorLog<DopplerModule>("Packet formation from array ends", "FormPacketsFromArray", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "FormPacketsFromArray", Severity.Warning);
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// Decodes the message.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="channel">The channel.</param>
        private void DecodeMessage(byte[] data, int channel)
        {
            if (channel == Constants.VALUE_1 || channel == Constants.VALUE_2)
            {
                List<ServiceLogPacket> msgPackets = GetLogFromArray(data);
                foreach (var msgPacket in msgPackets)
                {
                    System.Diagnostics.Debug.WriteLine(msgPacket.Message.MessageCode.ToString());
                }
            }
        }

        /// <summary>
        /// Synchronizes the right channel data.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        private static ulong SyncRightChannelData()
        {
            ulong syncIdR = Constants.VALUE_0;
            //Logs.Instance.ErrorLog<DopplerModule>("SyncRightChannelData begins", "SyncRightChannelData", Severity.Debug);
            try
            {
                if (CircularQueueChannel2.Channel2Queue.Count >= DMIProtocol.PACKET_SIZE)
                {
                    byte[] arr = CircularQueueChannel2.Channel2Queue.PeekSyncCode();
                    syncIdR = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                    while (syncIdR != DMIProtocol.DMI_PACKET_SYNC)
                    {
                        CircularQueueChannel2.Channel2Queue.Dequeue();
                        while (CircularQueueChannel2.Channel2Queue.First() != DMIProtocol.SYNCID_FIRSTBYTE)
                        {
                            CircularQueueChannel2.Channel2Queue.Dequeue();
                        }
                        arr = CircularQueueChannel2.Channel2Queue.PeekSyncCode();
                        syncIdR = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "SyncRightChannelData", Severity.Warning);
            }

            //Logs.Instance.ErrorLog<DopplerModule>("SyncRightChannelData ends with return sync id: " + syncIdR.ToString(), "SyncRightChannelData", Severity.Debug);
            return syncIdR;
        }

        /// <summary>
        /// Gets the log from array.
        /// </summary>
        /// <param name="tempArr">The temporary arr.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>ServiceLogPacket.</returns>
        internal List<ServiceLogPacket> GetLogFromArray(byte[] tempArr)
        {
            List<ServiceLogPacket> ServicePackets = new List<ServiceLogPacket>();

            try
            {
                if (tempArr != null)
                {
                    int offset = 0;
                    //Logs.Instance.ErrorLog<DopplerModule>("GetLogFromArray begins for tempArray of size:" + tempArr.Length.ToString(), "GetLogFromArray", Severity.Debug);
                    while (tempArr.Length > offset)
                    {
                        ServiceLogPacket ServicePacket = new ServiceLogPacket();
                        #region ServiceHeader

                        ServicePacket.PacketHeader.SyncId = BitConverter.ToInt64(tempArr, ServiceHeader.sync + offset);
                        ServicePacket.PacketHeader.SystemId = tempArr[ServiceHeader.systemId + offset];
                        ServicePacket.PacketHeader.DataSource = tempArr[ServiceHeader.dataSource + offset];
                        ServicePacket.PacketHeader.MessageType = tempArr[ServiceHeader.messageType + offset];
                        ServicePacket.PacketHeader.MessageSubType = tempArr[ServiceHeader.messageSubType + offset];
                        ServicePacket.PacketHeader.DataLength = BitConverter.ToUInt16(tempArr, ServiceHeader.dataLength + offset);
                        ServicePacket.PacketHeader.Sequence = BitConverter.ToUInt16(tempArr, ServiceHeader.sequence + offset);

                        #endregion ServiceHeader

                        #region ServiceMessage

                        ServicePacket.Message.MessageCode = BitConverter.ToUInt32(tempArr, ServiceMessage.messageCode + offset);
                        ServicePacket.Message.MessageText = Helper.ConvertBytesToString(tempArr, ServiceMessage.messageText + offset, ServicePacket.PacketHeader.DataLength - 4);
                        ServicePacket.Message.MessageText = ServicePacket.Message.MessageText.Substring(0, ServicePacket.Message.MessageText.IndexOf('\0'));
                        ServicePackets.Add(ServicePacket);
                        offset = DMIProtocol.DMI_SERVICELOG_HEADER_SIZE + ServicePacket.PacketHeader.DataLength + offset; ;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "GetLogFromArray", Severity.Warning);
            }
            //Logs.Instance.ErrorLog<DopplerModule>("GetLogFromArray ends with ServicePacket: " + ServicePacket.ToString(), "GetLogFromArray", Severity.Debug);
            return ServicePackets;
        }

        /// <summary>
        /// Synchronizes the left channel data.
        /// </summary>
        /// <returns>System.UInt64.</returns>
        private static ulong SyncLeftChannelData()
        {
            ulong syncId = Constants.VALUE_0;
            //Logs.Instance.ErrorLog<DopplerModule>("SyncLeftChannelData begins", "SyncRightChannelData", Severity.Debug);
            try
            {
                if (CircularQueueChannel1.Channel1Queue.Count >= DMIProtocol.PACKET_SIZE)
                {
                    byte[] arr = CircularQueueChannel1.Channel1Queue.PeekSyncCode();
                    syncId = BitConverter.ToUInt64(arr, Constants.VALUE_0);

                    while (syncId != DMIProtocol.DMI_PACKET_SYNC)
                    {
                        CircularQueueChannel1.Channel1Queue.Dequeue();
                        while (CircularQueueChannel1.Channel1Queue.First() != DMIProtocol.SYNCID_FIRSTBYTE)
                        {
                            CircularQueueChannel1.Channel1Queue.Dequeue();
                        }
                        arr = CircularQueueChannel1.Channel1Queue.PeekSyncCode();
                        syncId = BitConverter.ToUInt64(arr, Constants.VALUE_0);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "SyncLeftChannelData", Severity.Warning);
            }

            //Logs.Instance.ErrorLog<DopplerModule>("SyncLeftChannelData ends with return sync id: " + syncId.ToString(), "SyncRightChannelData", Severity.Debug);

            return syncId;
        }

        /// <summary>
        /// Tries to make packets on the background thread
        /// </summary>
        /// <param name="tempArray">The temporary array.</param>
        /// <param name="tempArrayRight">The temporary array right.</param>
        private void PacketBackgroundThread(byte[] tempArray, byte[] tempArrayRight)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("PacketBackgroundThread begins for leftArray length:" +
            //   (tempArray != null ? tempArray.Length.ToString() : "Zero") + " rightArray length: "
            //      + (tempArrayRight != null ? tempArrayRight.Length.ToString() : "Zero"), "PacketBackgroundThread", Severity.Debug);
            try
            {
                packets = (ConvertToPacket(tempArray, tempArrayRight));
                leftPacket = tempArray;
                rightPacket = tempArrayRight;
                counterPacketForm++;
                if (OnPacketFormationDual != null)
                {
                    OnPacketFormationDual(packets);
                }
                if (OnRecordingEnabled != null)
                {
                    OnRecordingEnabled(tempArray, tempArrayRight);
                }

                //Logs.Instance.ErrorLog<DopplerModule>("PacketBackgroundThread ends", "PacketBackgroundThread", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "PacketBackgroundThread", Severity.Warning);
            }
        }

        /// <summary>
        /// Convert the byte array to the packet for both the channels
        /// </summary>
        /// <param name="tempArr">The temporary arr.</param>
        /// <param name="tempArrRight">The temporary arr right.</param>
        /// <returns>DMIPmdDataPacket[].</returns>
        private DMIPmdDataPacket[] ConvertToPacket(byte[] tempArr, byte[] tempArrRight)
        {
            packetArr = new DMIPmdDataPacket[Constants.VALUE_2];
            //Logs.Instance.ErrorLog<DopplerModule>("ConvertToPacket begins for leftArray length:" + (tempArr != null ? tempArr.Length.ToString() : "Zero") +
            // " rightArray length: " + (tempArrRight != null ? tempArrRight.Length.ToString() : "Zero"), "PacketBackgroundThread", Severity.Debug);
            try
            {
                if (tempArr != null)
                {
                    packetArr[Constants.VALUE_0] = GetSinglePacket(tempArr, Constants.VALUE_1);
                }
                if (tempArrRight != null)
                {
                    packetArr[Constants.VALUE_1] = GetSinglePacket(tempArrRight, Constants.VALUE_2);
                }

                //Logs.Instance.ErrorLog<DopplerModule>("ConvertToPacket ends with array of packest in return of length:" + packetArr.Length.ToString(), "ConvertToPacket",
                //    Severity.Debug);
                return packetArr;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "ConvertToPacket", Severity.Warning);
                return packetArr;
            }
        }

        /// <summary>
        /// Gets the single packet.
        /// </summary>
        /// <param name="tempArr">The temporary arr.</param>
        /// <param name="channel">The channel.</param>
        /// <returns>DMIPmdDataPacket.</returns>
        private DMIPmdDataPacket GetSinglePacket(byte[] tempArr, int channel)
        {
            //Logs.Instance.ErrorLog<DopplerModule>("GetSinglePacket begins for tempArray length:" +
            // (tempArr != null ? tempArr.Length.ToString() : "Zero") + " ChannelId :" + channel.ToString(),
            //  "GetSinglePacket", Severity.Debug);
            try
            {
                /// <summary>
                /// The single doppler packet
                /// </summary>
                DMIPmdDataPacket singleDopplerPacket = null;
                StringBuilder Logstring = new StringBuilder();
                bool logflag = false;
                int checksum = Constants.VALUE_0;
                if (tempArr != null)
                {
                    singleDopplerPacket = new DMIPmdDataPacket();
                    byte[] byteArr = new byte[DMIProtocol.PACKET_SIZE];
                    Array.ConstrainedCopy(tempArr, Constants.VALUE_0, byteArr, Constants.VALUE_0, DMIProtocol.PACKET_SIZE);
                    singleDopplerPacket.archive = new DMIArchive();

                    CreateHeader(ref byteArr, ref singleDopplerPacket);

                    CreateParameter(ref byteArr, ref singleDopplerPacket);

                    Logstring.Append(CheckEmboli(channel, ref logflag, ref singleDopplerPacket));

                    if (logflag)
                    {
                        logflag = false;
                        //Logs.Instance.ErrorLog<DopplerModule>(Logstring.ToString(), "GetSinglePacket", Severity.Warning);
                        return null;
                    }

                    checksum = CreateRemainingPacketSection(ref byteArr, ref singleDopplerPacket);

                    if (checksum == 0)
                    {
                        return singleDopplerPacket;
                    }
                    else
                    {
                        //Logs.Instance.ErrorLog<DopplerModule>(MessageConstants.InvalidChecksum + " " + singleDopplerPacket.header.sequence, "GetSinglePacket",
                        //     Severity.Warning);
                    }
                }
                //Logs.Instance.ErrorLog<DopplerModule>("GetSinglePacket ends ", "GetSinglePacket", Severity.Debug);
                return null;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "GetSinglePacket", Severity.Warning);
                return null;
            }
        }

        /// <summary>
        /// Creates the remaining packet section.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="packetObj">The packet object.</param>
        /// <returns>System.Int32.</returns>
        private int CreateRemainingPacketSection(ref byte[] byteArr, ref DMIPmdDataPacket packetObj)
        {
            int checksum = 0;
            //Logs.Instance.ErrorLog<DopplerModule>("CreateRemainingPacketSection begins for byteArr of length:" +
            //   (byteArr != null ? byteArr.Length.ToString() : "Zero"), "CreateRemainingPacketSection",
            //       Severity.Debug);
            try
            {
                CreateEnvelop(ref byteArr, ref packetObj);

                CreateSpectrum(ref byteArr, ref packetObj);

                CreateMMode(ref byteArr, ref packetObj);

                CreateAudio(ref byteArr, ref packetObj);

                CreateEDetect(ref byteArr, ref packetObj);

                CreateArchive(ref byteArr, ref packetObj);

                packetObj.checksum = BitConverter.ToInt32(byteArr, Checksum.ChecksumPos);
                int incrementFactor = sizeof(Int32);
                for (int ii = Constants.VALUE_0; ii < DMIProtocol.PACKET_SIZE; ii += incrementFactor)
                {
                    checksum += BitConverter.ToInt32(byteArr, ii);
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CreateRemainingPacketSection ends ", "CreateRemainingPacketSection", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CreateRemainingPacketSection", Severity.Warning);
            }
            return checksum;
        }

        /// <summary>
        /// Writes the value to FPGA register
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="fpgaValue">The fpga value.</param>
        /// <param name="registerAddress">The register address.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> WriteFPGAValue(TCDHandles channelID, int fpgaValue, uint registerAddress)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_ACCESS_FPGA_REG,
                        registerAddress,
                        Constants.VALUE_0,
                        Constants.VALUE_4,
                        BitConverter.GetBytes(fpgaValue)) == Constants.VALUE_4)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "WriteFPGAValue", Severity.Warning);
                throw;
            }
            return result;
        }

        /// <summary>
        /// Sets the mode.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="modeToSet">The mode to set.</param>
        /// <returns>Task&lt;System.UInt32&gt;.</returns>
        internal async Task<uint> SetMode(TCDHandles channelID, TCDModes modeToSet)
        {
            uint bytesTransferred = 0;
            if (TCDHandler.Current.GetCurrentDevice(channelID).CurrentMode != modeToSet)
            {
                bytesTransferred = await TCDHandler.Current.SendControlCommandAsync
                    (channelID,
                    DMIProtocol.DMI_CMD_SET_MODE,
                    (uint)modeToSet,
                    0);
            }
            return bytesTransferred;
        }

        /// <summary>
        /// read fpga value as an asynchronous operation.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="fpgaRegisterAddress">The fpga register address.</param>
        /// <returns>Task&lt;System.UInt32&gt;.</returns>
        internal async Task<uint> ReadFPGAValueAsync(TCDHandles channelID, uint fpgaRegisterAddress)
        {
            uint result = 0;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    byte[] tempArray = await TCDHandler.Current.SendControlRequestAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_ACCESS_FPGA_REG,
                        fpgaRegisterAddress, 0, 4);
                    result = BitConverter.ToUInt32(tempArray, 0);
                }
                return result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFPGAValueAsync", Severity.Warning);
                throw;
            }
        }

        /// <summary>
        /// start update as an asynchronous operation.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.Byte&gt;.</returns>
        internal async Task<byte> StartUpdateAsync(TCDHandles channelID)
        {
            byte result = 0;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Update);
                if (bytesTransferred == 0)
                {
                    byte[] tempArray = await TCDHandler.Current.SendControlRequestAsync
                        (channelID,
                        DMIProtocol.DMI_REQ_UPDATE_ACTION,
                        1, 0, 1);
                    result = tempArray[0];
                }
                return result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFPGAValueAsync", Severity.Warning);
                throw;
            }
        }

        /// <summary>
        /// end update as an asynchronous operation.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.Byte&gt;.</returns>
        internal async Task<byte> EndUpdateAsync(TCDHandles channelID)
        {
            byte result = 0;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Update);
                if (bytesTransferred == 0)
                {
                    byte[] tempArray = await TCDHandler.Current.SendControlRequestAsync
                        (channelID,
                        DMIProtocol.DMI_REQ_UPDATE_ACTION,
                        2, 0, 1);
                    result = tempArray[0];
                }
                return result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadFPGAValueAsync", Severity.Warning);
                throw;
            }
        }

        /// <summary>
        /// get update progress as an asynchronous operation.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;UpdateProgress&gt;.</returns>
        internal async Task<UpdateProgress> GetUpdateProgressAsync(TCDHandles channelID)
        {
            UpdateProgress updateProgress = new UpdateProgress();

            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Update);

                if (bytesTransferred == 0)
                {
                    uint bufferLength = 12;
                    byte[] dataArray = await TCDHandler.Current.SendControlRequestAsync
                        (channelID,
                        DMIProtocol.DMI_REQ_UPDATE_PROGRESS,
                        0, 0, bufferLength);

                    if (dataArray != null && dataArray.Length == bufferLength)
                    {
                        updateProgress = UpdateProgress.ConvertArrayToInfo(ref dataArray);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadCalibrationInfo", Severity.Warning);
                throw;
            }

            return updateProgress;
        }

        /// <summary>
        /// Reads the calibration information of the module specified
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;CalibrationInfo&gt;.</returns>
        internal async Task<CalibrationInfo> ReadCalibrationInfo(TCDHandles channelID)
        {
            CalibrationInfo calibration = new CalibrationInfo();

            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);

                if (bytesTransferred == 0)
                {
                    uint index = 3;
                    uint bufferLength = 8;
                    byte[] dataArray = await TCDHandler.Current.SendControlRequestAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_ACCESS_INFO,
                        index, 0, bufferLength);

                    if (dataArray != null && dataArray.Length == bufferLength)
                    {
                        calibration = CalibrationInfo.ConvertArrayToInfo(ref dataArray);
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadCalibrationInfo", Severity.Warning);
                throw;
            }

            return calibration;
        }

        /// <summary>
        /// Provides the count of minutes the specified module has been on.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.UInt32&gt;.</returns>
        internal async Task<uint> ReadOperatingMinutes(TCDHandles channelID)
        {
            uint result = 0;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 5;
                    uint bufferLength = 4;

                    byte[] tempArray = await TCDHandler.Current.SendControlRequestAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_ACCESS_INFO,
                        index, 0, bufferLength);

                    if (tempArray != null && tempArray.Length == bufferLength)
                    {
                        result = BitConverter.ToUInt32(tempArray, 0);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ReadOperatingMinutes", Severity.Warning);
                throw ex;
            }
        }

        /// <summary>
        /// Reset the value to FPGA register
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="resetValue">The reset value.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> ResetFPGAAsync(TCDHandles channelId, int resetValue)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelId, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelId,
                        DMIProtocol.DMI_CMD_SERVICE_ACCESS_FPGA_REG,
                        0, (uint)resetValue) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ResetFPGAAsync", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Cancel the currently active service activity.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> AbortServiceAsync(TCDHandles channelID)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_ABORT, 0, 0) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "AbortServiceAsync", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Assigns channel number to the module specified.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="channelNumber">The channel number.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> AssignChannelAsync(TCDHandles channelID, byte channelNumber)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 6;
                    uint value = 0x5AFE;
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_ACCESS_INFO,
                        index,
                        value, 1, BitConverter.GetBytes(channelNumber)) == 1)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "AssignChannelAsync", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Writes the board information to the module specified.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="boardInfo">The board information.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> WriteBoardInfoAsync(TCDHandles channelID, BoardInfo boardInfo)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 1;
                    uint value = 0x5AFE;
                    byte[] bufferData = BoardInfo.ConvertBoardInfoToArray(boardInfo);

                    await TCDHandler.Current.SendControlCommandAsync
                        (channelID, DMIProtocol.DMI_CMD_SERVICE_ACCESS_INFO,
                        index, value, DMIProtocol.BOARD_INFO_REQUEST_LENGTH, bufferData);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "WriteBoardInfoAsync", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Writes the probe information to the module specified.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="probeInfo">The probe information.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> WriteProbeInfoAsync(TCDHandles channelID, ProbeInfo probeInfo)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 7;
                    uint value = 0x5AFE;
                    byte[] bufferData = ProbeInfo.ConvertProbeInfoToArray(probeInfo);

                    if (await TCDHandler.Current.SendControlCommandAsync(channelID, DMIProtocol.DMI_CMD_SERVICE_ACCESS_INFO, index, value, DMIProtocol.PROBE_INFO_REQUEST_LENGTH, bufferData) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "WriteProbeInfoAsync", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Measurement start for calibration of Doppler board.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> StartMeasurementOfBoard(TCDHandles channelID)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 30;
                    uint value = 1;
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_CALIBRATE,
                        index, value) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "StartMeasurementOfBoard", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Applies measurement while performing calibration of Doppler board.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="m1">The m1.</param>
        /// <param name="m2">The m2.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> ApplyMeasurementToBoard(TCDHandles channelID, uint m1, uint m2)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 35;
                    uint value = 0;
                    uint bufferLength = 4;

                    List<byte> bufferData = new List<byte>(4);
                    bufferData.AddRange(BitConverter.GetBytes(m1));
                    bufferData.AddRange(BitConverter.GetBytes(m2));

                    await TCDHandler.Current.SendControlCommandAsync
                        (channelID, DMIProtocol.DMI_CMD_SERVICE_CALIBRATE,
                        index, value, bufferLength, bufferData.ToArray());
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "ApplyMeasurementToBoard", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Writes the calibration information to the board directly.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="calibration">The calibration.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> CalibrateBoard(TCDHandles channelID, CalibrationInfo calibration)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 40;
                    uint value = 0;
                    uint bufferLength = 8;
                    byte[] bufferData = CalibrationInfo.ConvertCalibrationInfoToArray(calibration);
                    await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_CALIBRATE,
                        index, value, bufferLength, bufferData.ToArray());
                    result = true;
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "CalibrateBoard", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Enables the transmit testing.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> EnableTransmitTestControl(TCDHandles channelID)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 1;
                    uint value = 0;

                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID, DMIProtocol.DMI_CMD_SERVICE_TX_CTRL,
                        index, value) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "EnableTransmitTestControl", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Disables the transmit testing.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> DisableTransmitTestControl(TCDHandles channelID)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 1;
                    uint value = 1;

                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID, DMIProtocol.DMI_CMD_SERVICE_TX_CTRL,
                        index, value) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "DisableTransmitTestControl", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Runs the transmitted without performing acquisition to enable transmit testing.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="power">The power.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> TransmitTestPower(TCDHandles channelID, uint power)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 2;
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_TX_CTRL,
                        index, power) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "TransmitTestPower", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Runs the transmitted without performing acquisition to enable transmit testing
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="sampleLength">Length of the sample.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> TransmitSampleLength(TCDHandles channelID, uint sampleLength)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 3;
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_TX_CTRL,
                        index, sampleLength) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "TransmitSampleLength", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Runs the transmitted without performing acquisition to enable transmit testing.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="prf">The PRF.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> TransmitPRF(TCDHandles channelID, uint prf)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    uint index = 4;
                    if (await TCDHandler.Current.SendControlCommandAsync
                        (channelID,
                        DMIProtocol.DMI_CMD_SERVICE_TX_CTRL,
                        index, prf) == 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "TransmitPRF", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Gets the service //Logs.
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="count">The count.</param>
        /// <returns>Task&lt;List&lt;PacketFormats.ServiceLogPacket&gt;&gt;.</returns>
        internal async Task<List<PacketFormats.ServiceLogPacket>> GetServiceLogs(TCDHandles channelID, int count)
        {
            try
            {
                List<PacketFormats.ServiceLogPacket> Logs = null;
                byte[] packetData = await TCDHandler.Current.ReadServiceLog(channelID, count);
                if (packetData.Length >= DMIProtocol.DMI_SERVICE_LOG_PACKET_SIZE)
                {
                    Logs = (GetLogFromArray(packetData));
                }
                return Logs;
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "GetServiceLogs", Severity.Warning);
                throw ex;
            }
        }

        /// <summary>
        /// Determines whether [is service active] [the specified channel identifier].
        /// </summary>
        /// <param name="channelID">The channel identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> IsServiceActive(TCDHandles channelID)
        {
            bool result = false;
            uint bytesTransferred = 0;
            try
            {
                bytesTransferred = await SetMode(channelID, TCDModes.Service);
                if (bytesTransferred == 0)
                {
                    byte[] data = await TCDHandler.Current.SendControlRequestAsync
                        (channelID, DMIProtocol.DMI_REQ_SERVICE_GET_STATE,
                        0, 0, 1);
                    if (data[0] == 0)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "TransmitPRF", Severity.Warning);
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Checks the emboli.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="logflag">if set to <c>true</c> [logflag].</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        private string CheckEmboli(int channel, ref bool logflag, ref DMIPmdDataPacket singleDopplerPacket)
        {
            StringBuilder Logstring = null;
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("CheckEmboli begins for channel:" + channel.ToString(), "CheckEmboli", Severity.Debug);
                switch (channel)
                {
                    case Constants.VALUE_1:
                        CheckLeftEmboli(ref logflag, ref Logstring, ref singleDopplerPacket);
                        TCDHandler.Current.Channel1.EmbCount = singleDopplerPacket.embCount;
                        break;

                    case Constants.VALUE_2:
                        CheckRightEmboli(ref logflag, ref Logstring, ref singleDopplerPacket);
                        TCDHandler.Current.Channel2.EmbCount = singleDopplerPacket.embCount;
                        break;

                    default:
                        //Logs.Instance.ErrorLog<DopplerModule>(ExecutionConstants.DopplerModule + ExecutionConstants.MethodName,
                        //     "GetSinglePacket:Recieved garbage value for Channel|" + channel.ToString() + "|" + ExecutionConstants.MethodExecutionFail, Severity.Warning);
                        break;
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CheckEmboli ends", "CheckEmboli", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CheckEmboli", Severity.Warning);
            }
            return (Logstring != null ? Logstring.ToString() : string.Empty);
        }

        /// <summary>
        /// Checks the right emboli.
        /// </summary>
        /// <param name="logflag">if set to <c>true</c> [logflag].</param>
        /// <param name="Logstring">The log string.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        private void CheckRightEmboli(ref bool logflag, ref StringBuilder Logstring, ref DMIPmdDataPacket singleDopplerPacket)
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("CheckRightEmboli begins", "CheckEmboli", Severity.Debug);
                if (counterPacketForm > 0)
                {
                    if (singleDopplerPacket.header.sequence != ++SequenceRightPacket)
                    {
                        Logstring = new StringBuilder(MessageConstants.InvalidSequence + SequenceRightPacket + "For Channel 2");
                        SequenceRightPacket = singleDopplerPacket.header.sequence;
                        logflag = true;
                        bool embDetected = (singleDopplerPacket.parameter.eventFlags & 2) != 0;
                        if (singleDopplerPacket.embCount > TCDHandler.Current.Channel2.EmbCount && !embDetected)
                        {
                            Logstring.Append(" And contained emboli");
                        }
                    }
                }
                else
                {
                    SequenceRightPacket = singleDopplerPacket.header.sequence;
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CheckRightEmboli ends", "CheckEmboli", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CheckRightEmboli", Severity.Warning);
            }
        }

        /// <summary>
        /// Checks the left emboli.
        /// </summary>
        /// <param name="logflag">if set to <c>true</c> [logflag].</param>
        /// <param name="Logstring">The log string.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        private void CheckLeftEmboli(ref bool logflag, ref StringBuilder Logstring, ref DMIPmdDataPacket singleDopplerPacket)
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("CheckLeftEmboli begins", "CheckEmboli", Severity.Debug);
                if (counterPacketForm > 0)
                {
                    if (singleDopplerPacket.header.sequence != ++SequenceLeftPacket)
                    {
                        Logstring = new StringBuilder(MessageConstants.InvalidSequence + SequenceLeftPacket + " For Channel 1");
                        SequenceLeftPacket = singleDopplerPacket.header.sequence;
                        logflag = true;
                        bool embDetected = (singleDopplerPacket.parameter.eventFlags & 2) != 0;
                        if (singleDopplerPacket.embCount > TCDHandler.Current.Channel1.EmbCount && !embDetected)
                        {
                            Logstring.Append(" And contained emboli");
                        }
                    }
                }
                else
                {
                    SequenceLeftPacket = singleDopplerPacket.header.sequence;
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CheckLeftEmboli ends", "CheckEmboli", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CheckRightEmboli", Severity.Warning);
            }
        }

        /// <summary>
        /// Gets the short CVR packet.
        /// </summary>
        /// <param name="tempArr">The temporary arr.</param>
        /// <returns>DMIPmdDataPacket.</returns>
        internal DMIPmdDataPacket GetShortCVRPacket(byte[] tempArr)
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("CheckLeftEmboli begins for tempArr of length:" + (tempArr != null ? tempArr.Length.ToString() : "Zero"),
                //   "GetShortCVRPacket", Severity.Debug);
                DMIPmdDataPacket singleDopplerPacket = null;
                if (tempArr != null)
                {
                    singleDopplerPacket = new DMIPmdDataPacket();
                    byte[] byteArr = new byte[DMIProtocol.FilePacketSize];
                    Array.ConstrainedCopy(tempArr, Constants.VALUE_0, byteArr, Constants.VALUE_0, DMIProtocol.FilePacketSize);

                    CreateHeader(ref byteArr, ref singleDopplerPacket);

                    CreateParameter(ref byteArr, ref singleDopplerPacket);

                    CreateEnvelop(ref byteArr, ref singleDopplerPacket);

                    CreateSpectrumCVR(ref byteArr, ref singleDopplerPacket);

                    CreateMMode(ref byteArr, ref singleDopplerPacket);

                    CreateAudio(ref byteArr, ref singleDopplerPacket);

                    singleDopplerPacket.checksum = BitConverter.ToInt32(byteArr, EDetect.PhaseA_MQ);
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CheckLeftEmboli ends", "GetShortCVRPacket", Severity.Debug);
                return singleDopplerPacket;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "GetShortCVRPacket", Severity.Warning);
                return null;
            }
        }

        #region Create Subpackets

        /// <summary>
        /// Creates the archive.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        private void CreateArchive(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Archive

            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("CreateArchive begins for byteArr of length:" + (byteArr != null ? byteArr.Length.ToString() : "Zero"), "CreateArchive", Severity.Debug);
                singleDopplerPacket.archive.timeseriesDepth = BitConverter.ToUInt16(byteArr, Archive.TimeSeriesDepth);
                singleDopplerPacket.archive.rfu = BitConverter.ToUInt16(byteArr, Archive.RFU);

                singleDopplerPacket.archive.timeseriesDepth = BitConverter.ToUInt16(byteArr, Archive.TimeSeriesDepth);
                singleDopplerPacket.archive.rfu = BitConverter.ToUInt16(byteArr, Archive.RFU);
                int i = Constants.VALUE_0;
                while (i < DMIProtocol.DMI_ARCHIVE_MMODE_GATES)
                {
                    singleDopplerPacket.archive.mmodeData.mmodePhaseA[i] = BitConverter.ToSingle(byteArr, Archive.MmodePhaseA + (i * DMIProtocol.FLOAT_SIZE));
                    singleDopplerPacket.archive.mmodeData.mmodePhaseB[i] = BitConverter.ToSingle(byteArr, Archive.mmodePhaseB + (i * DMIProtocol.FLOAT_SIZE));
                    singleDopplerPacket.archive.mmodeData.mmodePowerA[i] = BitConverter.ToSingle(byteArr, Archive.mmodePowerA + (i * DMIProtocol.FLOAT_SIZE));
                    singleDopplerPacket.archive.mmodeData.mmodePowerB[i] = BitConverter.ToSingle(byteArr, Archive.mmodePowerB + (i * DMIProtocol.FLOAT_SIZE));
                    i++;
                }
                //get time series data
                for (i = 0; i < DMIProtocol.DMI_ARCHIVE_TIMESERIES_SIZE; i++)
                {
                    singleDopplerPacket.archive.timeseries[i].I = BitConverter.ToSingle(byteArr, Archive.TimeseriesI + (i * Constants.VALUE_2 * DMIProtocol.FLOAT_SIZE));
                    singleDopplerPacket.archive.timeseries[i].Q = BitConverter.ToSingle(byteArr, Archive.TimeseriesQ + (i * Constants.VALUE_2 * DMIProtocol.FLOAT_SIZE));
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CreateArchive ends ", "CreateArchive", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CreateArchive", Severity.Warning);
            }

            #endregion Archive
        }

        /// <summary>
        /// Creates the e detect.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// Debug statements not required
        private void CreateEDetect(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Edetect

            singleDopplerPacket.edetectResults = new DMIEDetectResults();
            singleDopplerPacket.edetectResults.phaseA.MQ = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_MQ);
            singleDopplerPacket.edetectResults.phaseA.ClutCount = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_ClutCount);
            singleDopplerPacket.edetectResults.phaseA.MEPosition = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_MEPosition);
            singleDopplerPacket.edetectResults.phaseA.MSum = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_MSum);
            singleDopplerPacket.edetectResults.phaseA.MPLocal = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_MPLocal);
            singleDopplerPacket.edetectResults.phaseA.AEFlag = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_AEFlag);
            singleDopplerPacket.edetectResults.phaseA.AEDownCount = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_AEDownCount);
            singleDopplerPacket.edetectResults.phaseA.AEDetect = BitConverter.ToUInt32(byteArr, EDetect.PhaseA_AEDetect);
            singleDopplerPacket.edetectResults.phaseB.MQ = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_MQ);
            singleDopplerPacket.edetectResults.phaseB.ClutCount = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_ClutCount);
            singleDopplerPacket.edetectResults.phaseB.MEPosition = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_MEPosition);
            singleDopplerPacket.edetectResults.phaseB.MSum = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_MSum);
            singleDopplerPacket.edetectResults.phaseB.MPLocal = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_MPLocal);
            singleDopplerPacket.edetectResults.phaseB.AEFlag = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_AEFlag);
            singleDopplerPacket.edetectResults.phaseB.AEDownCount = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_AEDownCount);
            singleDopplerPacket.edetectResults.phaseB.AEDetect = BitConverter.ToUInt32(byteArr, EDetect.PhaseB_AEDetect);
            singleDopplerPacket.edetectResults.edetect = BitConverter.ToInt32(byteArr, EDetect.EDetectValue);

            #endregion Edetect
        }

        /// <summary>
        /// Creates the audio.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// Debug statements not required
        private void CreateAudio(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Audio

            singleDopplerPacket.audio.depth = BitConverter.ToUInt16(byteArr, Audio.Depth);
            singleDopplerPacket.audio.rfu = BitConverter.ToUInt16(byteArr, Audio.RFU);
            singleDopplerPacket.audio.sampleRate = BitConverter.ToUInt16(byteArr, Audio.SampleRate);
            singleDopplerPacket.audio.maxAmplitude = BitConverter.ToInt16(byteArr, Audio.MaxAmplitude);

            #endregion Audio
        }

        /// <summary>
        /// Creates the m mode.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// Debug statements not required
        private void CreateMMode(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region MMode

            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("CreateMMode begins for byteArr of length:" + (byteArr != null ? byteArr.Length.ToString() : "Zero"),
                // "CreateSpectrum", Severity.Debug);
                singleDopplerPacket.mmode.autoGainOffset = BitConverter.ToInt16(byteArr, MMode.AutoGainOffset);
                singleDopplerPacket.mmode.startDepth = BitConverter.ToUInt16(byteArr, MMode.StartDepth);
                singleDopplerPacket.mmode.endDepth = BitConverter.ToUInt16(byteArr, MMode.EndDepth);
                singleDopplerPacket.mmode.pointsPerColumn = BitConverter.ToUInt16(byteArr, MMode.PointsPerColumn);
                int i = Constants.VALUE_0;
                while (i < DMIProtocol.DMI_PKT_MMODE_PTS)
                {
                    singleDopplerPacket.mmode.power[i] = BitConverter.ToInt16(byteArr, MMode.Power + (i * Constants.VALUE_2));
                    singleDopplerPacket.mmode.velocity[i] = BitConverter.ToInt16(byteArr, MMode.Velocity + (i * Constants.VALUE_2));
                    i++;
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CreateMMode ends ", "CreateMMode", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CreateMMode", Severity.Warning);
            }

            #endregion MMode
        }

        /// <summary>
        /// Creates the spectrum.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// Debug statements not required
        private void CreateSpectrum(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Spectrum

            try
            {
                ///Logs.Instance.ErrorLog<DopplerModule>("CreateSpectrum begins for byteArr of length:" + (byteArr != null ? byteArr.Length.ToString() : "Zero"),
                //   "CreateSpectrum", Severity.Debug);
                singleDopplerPacket.spectrum.depth = BitConverter.ToUInt16(byteArr, Spectrum.Depth);
                singleDopplerPacket.spectrum.clutterFilter = BitConverter.ToUInt16(byteArr, Spectrum.ClutterFilter);
                singleDopplerPacket.spectrum.autoGainOffset = BitConverter.ToInt16(byteArr, Spectrum.AutoGainOffset);
                singleDopplerPacket.spectrum.startVelocity = BitConverter.ToInt16(byteArr, Spectrum.StartVelocity);
                singleDopplerPacket.spectrum.endVelocity = BitConverter.ToInt16(byteArr, Spectrum.EndVelocity);
                singleDopplerPacket.spectrum.pointsPerColumn = BitConverter.ToUInt16(byteArr, Spectrum.PointsPerColumn);
                int i = Constants.VALUE_0;
                while (i < DMIProtocol.SpectrumPointsCount)
                {
                    singleDopplerPacket.spectrum.points[i] = BitConverter.ToInt16(byteArr, Spectrum.Points + (i * Constants.VALUE_2));
                    i++;
                }
                i = Constants.VALUE_0;
                while (i < DMIProtocol.DMI_AUDIO_ARRAY_SIZE)
                {
                    singleDopplerPacket.audio.toward[i] = BitConverter.ToInt16(byteArr, Audio.Toward + (i * Constants.VALUE_2));
                    singleDopplerPacket.audio.away[i] = BitConverter.ToInt16(byteArr, Audio.Away + (i * Constants.VALUE_2));
                    singleDopplerPacket.archive.timeseries[i].I = BitConverter.ToSingle(byteArr, Archive.TimeseriesI + (i * DMIProtocol.FLOATIQ_SIZE));
                    singleDopplerPacket.archive.timeseries[i].Q = BitConverter.ToSingle(byteArr, Archive.TimeseriesQ + (i * DMIProtocol.FLOATIQ_SIZE));
                    i++;
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CreateSpectrum ends ", "CreateSpectrum", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CreateSpectrum", Severity.Warning);
            }

            #endregion Spectrum
        }

        /// <summary>
        /// Creates the spectrum CVR.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        private void CreateSpectrumCVR(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Spectrum

            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("CreateSpectrumCVR begins for byteArr of length:" + (byteArr != null ? byteArr.Length.ToString() : "Zero"),
                //  "CreateSpectrumCVR", Severity.Debug);
                singleDopplerPacket.spectrum.depth = BitConverter.ToUInt16(byteArr, Spectrum.Depth);
                singleDopplerPacket.spectrum.clutterFilter = BitConverter.ToUInt16(byteArr, Spectrum.ClutterFilter);
                singleDopplerPacket.spectrum.autoGainOffset = BitConverter.ToInt16(byteArr, Spectrum.AutoGainOffset);
                singleDopplerPacket.spectrum.startVelocity = BitConverter.ToInt16(byteArr, Spectrum.StartVelocity);
                singleDopplerPacket.spectrum.endVelocity = BitConverter.ToInt16(byteArr, Spectrum.EndVelocity);
                singleDopplerPacket.spectrum.pointsPerColumn = BitConverter.ToUInt16(byteArr, Spectrum.PointsPerColumn);
                int i = Constants.VALUE_0;
                while (i < DMIProtocol.SpectrumPointsCount)
                {
                    singleDopplerPacket.spectrum.points[i] = BitConverter.ToInt16(byteArr, Spectrum.Points + (i * Constants.VALUE_2));
                    i++;
                }
                i = Constants.VALUE_0;
                while (i < DMIProtocol.DMI_AUDIO_ARRAY_SIZE)
                {
                    singleDopplerPacket.audio.toward[i] = BitConverter.ToInt16(byteArr, Audio.Toward + (i * Constants.VALUE_2));
                    singleDopplerPacket.audio.away[i] = BitConverter.ToInt16(byteArr, Audio.Away + (i * Constants.VALUE_2));
                    i++;
                }
                //Logs.Instance.ErrorLog<DopplerModule>("CreateSpectrumCVR ends ", "CreateSpectrumCVR", Severity.Debug);
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(e, "CreateSpectrumCVR", Severity.Warning);
            }

            #endregion Spectrum
        }

        /// <summary>
        /// Creates the envelop.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// Debug statements not required
        private void CreateEnvelop(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Envelope

            singleDopplerPacket.envelope.depth = BitConverter.ToUInt16(byteArr, Envelop.Depth);
            singleDopplerPacket.envelope.velocityUnits = BitConverter.ToUInt16(byteArr, Envelop.VelocityUnit);
            singleDopplerPacket.envelope.colIndexPos = BitConverter.ToUInt16(byteArr, Envelop.ColIndexPos);
            singleDopplerPacket.envelope.posVelocity = BitConverter.ToInt16(byteArr, Envelop.PosVelocity);
            singleDopplerPacket.envelope.posPEAK = BitConverter.ToInt16(byteArr, Envelop.PosPeak);
            singleDopplerPacket.envelope.posMEAN = BitConverter.ToInt16(byteArr, Envelop.PosMean);
            singleDopplerPacket.envelope.posDIAS = BitConverter.ToInt16(byteArr, Envelop.PosDias);
            singleDopplerPacket.envelope.posPI = BitConverter.ToUInt16(byteArr, Envelop.PosPI);
            singleDopplerPacket.envelope.posRI = BitConverter.ToUInt16(byteArr, Envelop.PosRI);
            singleDopplerPacket.envelope.colIndexNeg = BitConverter.ToUInt16(byteArr, Envelop.ColIndexNeg);
            singleDopplerPacket.envelope.negVelocity = BitConverter.ToInt16(byteArr, Envelop.NegVelocity);
            singleDopplerPacket.envelope.negPEAK = BitConverter.ToInt16(byteArr, Envelop.NegPeak);
            singleDopplerPacket.envelope.negMEAN = BitConverter.ToInt16(byteArr, Envelop.NegMean);
            singleDopplerPacket.envelope.negDIAS = BitConverter.ToInt16(byteArr, Envelop.NegDias);
            singleDopplerPacket.envelope.negPI = BitConverter.ToUInt16(byteArr, Envelop.NegPI);
            singleDopplerPacket.envelope.negRI = BitConverter.ToUInt16(byteArr, Envelop.NegRI);

            #endregion Envelope
        }

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// Debug statements not required
        private void CreateParameter(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Parameter

            singleDopplerPacket.parameter.timestampL = BitConverter.ToUInt32(byteArr, Parameter.LeftTimeStamp);
            singleDopplerPacket.parameter.timestampH = BitConverter.ToUInt32(byteArr, Parameter.RightTimeStamp);
            singleDopplerPacket.parameter.eventFlags = BitConverter.ToUInt16(byteArr, Parameter.EventFlags);
            singleDopplerPacket.parameter.operatingState = byteArr[Parameter.OperatingState];
            singleDopplerPacket.parameter.acousticPower = byteArr[Parameter.AcousingPower];
            singleDopplerPacket.parameter.sampleLength = byteArr[Parameter.SampleLength];
            singleDopplerPacket.parameter.userDepth = byteArr[Parameter.UserDepth];
            singleDopplerPacket.parameter.PRF = BitConverter.ToUInt16(byteArr, Parameter.PRF);
            singleDopplerPacket.parameter.TIC = BitConverter.ToUInt16(byteArr, Parameter.TIC);
            singleDopplerPacket.parameter.rfu = BitConverter.ToUInt16(byteArr, Parameter.RFU);
            singleDopplerPacket.embCount = BitConverter.ToUInt32(byteArr, Parameter.EmboliCount);

            #endregion Parameter
        }

        /// <summary>
        /// Creates the header.
        /// </summary>
        /// <param name="byteArr">The byte arr.</param>
        /// <param name="singleDopplerPacket">The single doppler packet.</param>
        /// Debug statements not required
        private void CreateHeader(ref byte[] byteArr, ref DMIPmdDataPacket singleDopplerPacket)
        {
            #region Header

            singleDopplerPacket.header.sync = BitConverter.ToInt64(byteArr, Header.Sync);
            singleDopplerPacket.header.systemID = byteArr[Header.SystemId];
            singleDopplerPacket.header.dataSource = byteArr[Header.DataSource];
            singleDopplerPacket.header.messageType = byteArr[Header.MessageType];
            singleDopplerPacket.header.messageSubType = byteArr[Header.MessageSubType];
            singleDopplerPacket.header.dataLength = BitConverter.ToUInt16(byteArr, Header.DataLength);
            singleDopplerPacket.header.sequence = BitConverter.ToUInt16(byteArr, Header.Sequence);
            singleDopplerPacket.reserved = BitConverter.ToUInt16(byteArr, Header.Reserved);
            singleDopplerPacket.dataFormatRev = BitConverter.ToUInt16(byteArr, Header.DataFormatREV);

            #endregion Header
        }

        #endregion Create Subpackets

        internal List<byte[]> GrabSinglePacket()
        {
            List<byte[]> SinglePacket = null;
            try
            {
                SinglePacket = new List<byte[]>();
                SinglePacket.Add(leftPacket);
                SinglePacket.Add(rightPacket);
            }
            catch (Exception ex)
            {

            }
            return SinglePacket;
        }

        /// <summary>
        /// Writes to file.
        /// </summary>
        /// <param name="leftChannelData">The left channel data.</param>
        /// <param name="rightChannelData">The right channel data.</param>
        internal async void WriteToFile(byte[] leftChannelData, byte[] rightChannelData)
        {
            try
            {
                counterPacketWrite++;
                //Logs.Instance.ErrorLog<DopplerModule>("async WriteToFile begins ", "WriteToFile", Severity.Debug);
                if (leftChannelData != null)
                {
                    leftBuffer.Enqueue(leftChannelData);
                    if (leftBuffer.Count >= Constants.PACKETS_PER_SEC)
                    {
                        var packets = leftBuffer.Take(Constants.PACKETS_PER_SEC);
                        int offset = 0;
                        byte[] writeArr = new byte[Constants.PACKETS_PER_SEC * DMIProtocol.FilePacketSize];
                        foreach (byte[] p in packets)
                        {
                            System.Buffer.BlockCopy(p, 0, writeArr, offset, EDetect.PhaseA_MQ);
                            System.Buffer.BlockCopy(p, Checksum.ChecksumPos, writeArr, offset + EDetect.PhaseA_MQ, DMIProtocol.CHECKSUM_SIZE);
                            offset += DMIProtocol.FilePacketSize;
                        }
                        for (int i = 0; i < Constants.PACKETS_PER_SEC; i++)
                        { leftBuffer.Dequeue(); }
                        if (fileWriteStream1 != null)
                        { await Task.Factory.StartNew(() => WriteAsync(writeArr, fileWriteStream1)); }
                    }
                }

                if (rightChannelData != null)
                {
                    rightBuffer.Enqueue(rightChannelData);
                    if (rightBuffer.Count >= Constants.PACKETS_PER_SEC)
                    {
                        var packets = rightBuffer.Take(Constants.PACKETS_PER_SEC);
                        int offset = 0;
                        byte[] writeArr = new byte[Constants.PACKETS_PER_SEC * DMIProtocol.FilePacketSize];
                        foreach (byte[] p in packets)
                        {
                            System.Buffer.BlockCopy(p, 0, writeArr, offset, EDetect.PhaseA_MQ);
                            System.Buffer.BlockCopy(p, Checksum.ChecksumPos, writeArr, offset + EDetect.PhaseA_MQ, DMIProtocol.CHECKSUM_SIZE);
                            offset += DMIProtocol.FilePacketSize;
                        }
                        for (int i = 0; i < Constants.PACKETS_PER_SEC; i++)
                        { rightBuffer.Dequeue(); }
                        if (fileWriteStream2 != null)
                        { await Task.Factory.StartNew(() => WriteAsync(writeArr, fileWriteStream2)); }
                    }
                }
                if (counterPacketWrite == exportPacketCount)
                {
                    counterPacketWrite = 0;
                    OnRecordingEnabled -= WriteToFile;
                    await ReleaseFileWritingResources();
                }
                //Logs.Instance.ErrorLog<DopplerModule>("async WriteToFile ends", "WriteToFile", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<DopplerModule>(ex, "WriteToFile", Severity.Warning);
            }
        }

        /// <summary>
        /// write as an asynchronous operation.
        /// </summary>
        /// <param name="tempArr">The temporary arr.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Task.</returns>
        private static async Task WriteAsync(byte[] tempArr, FileRandomAccessStream stream)
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("async WriteAsync begins", "WriteAsync", Severity.Debug);

                stream.Seek(stream.Size);
                await stream.WriteAsync(tempArr.AsBuffer());
                //Logs.Instance.ErrorLog<DopplerModule>("async WriteAsync ends", "WriteAsync", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "WriteAsync", Severity.Warning);
            }
        }

        /// <summary>
        /// Initializes the streams.
        /// </summary>
        /// <returns>Task.</returns>
        internal async Task InitializeStreams()
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("async InitializeStreams begins", "InitializeStreams", Severity.Debug);
                if (TCDHandler.Current.Channel1.IsChannelEnabled)
                {
                    fileWriteStream1 = (FileRandomAccessStream)await writeFile1.OpenAsync(FileAccessMode.ReadWrite);
                }

                if (TCDHandler.Current.Channel2.IsChannelEnabled)
                {
                    fileWriteStream2 = (FileRandomAccessStream)await writeFile2.OpenAsync(FileAccessMode.ReadWrite);
                }

                //Logs.Instance.ErrorLog<DopplerModule>("async InitializeStreams ends", "InitializeStreams", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "InitializeStreams", Severity.Warning);
            }
        }

        /// <summary>
        /// Releases the file writing resources.
        /// </summary>
        /// <returns>Task.</returns>
        internal async Task ReleaseFileWritingResources()
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("ReleaseFileWritingResources begins ", "ReleaseFileWritingResources", Severity.Debug);
                OnRecordingEnabled = null;
                if (fileWriteStream1 != null)
                {
                    await fileWriteStream1.FlushAsync();
                    fileWriteStream1.Dispose();
                    fileWriteStream1 = null;
                }
                if (fileWriteStream2 != null)
                {
                    await fileWriteStream2.FlushAsync();
                    fileWriteStream2.Dispose();
                    fileWriteStream2 = null;
                }

                //Logs.Instance.ErrorLog<DopplerModule>("ReleaseFileWritingResources ends ", "ReleaseFileWritingResources", Severity.Debug);
            }
            catch (Exception ex)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(ex, "ReleaseFileWritingResources", Severity.Warning);
            }
        }

        /// <summary>
        /// Creates the binary file of exam.
        /// </summary>
        /// <param name="examId">The exam identifier.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal async Task<bool> CreateBinaryFileOfExam(int examId)
        {
            try
            {
                //Logs.Instance.ErrorLog<DopplerModule>("async CreateBinaryFileOfExam begins for examId:" + examId.ToString(), "CreateBinaryFileOfExam", Severity.Debug);
                if (TCDHandler.Current.Channel1.IsChannelEnabled)
                {
                    string binaryFileChannel1 = String.Format("{0}-Channel{1}.txt", examId, Constants.VALUE_1);
                    writeFile1 = await ApplicationData.Current.LocalFolder.CreateFileAsync(binaryFileChannel1, CreationCollisionOption.OpenIfExists);
                }

                if (TCDHandler.Current.Channel2.IsChannelEnabled)
                {
                    string binaryFileChannel2 = String.Format("{0}-Channel{1}.txt", examId, Constants.VALUE_2);
                    writeFile2 = await ApplicationData.Current.LocalFolder.CreateFileAsync(binaryFileChannel2, CreationCollisionOption.OpenIfExists);
                }
                //Logs.Instance.ErrorLog<DopplerModule>("async CreateBinaryFileOfExam ends for examId:" + examId.ToString(), "CreateBinaryFileOfExam", Severity.Debug);
                return true;
            }
            catch (Exception e)
            {
                //Logs.Instance.ErrorLog<UsbTcdDll>(e, "CreateBinaryFileOfExam", Severity.Warning);
                return false;
            }
        }

                        #endregion ServiceMessage

        #endregion Methods
    }
}