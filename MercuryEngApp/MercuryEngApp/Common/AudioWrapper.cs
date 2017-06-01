
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using UsbTcdLibrary.PacketFormats;
using System.Linq;
using Windows.Storage;
using UsbTcdLibrary;
using Core.Constants;
using AudioLib;
using log4net;

namespace MercuryEngApp.Common
{
    public class AudioWrapper : IDisposable
    {

        #region constants

        /// <summary>
        /// The value 25
        /// </summary>
        const int VALUE_25 = 25;
        /// <summary>
        /// The value 10
        /// </summary>
        const int VALUE_10 = 10;

        /// <summary>
        /// The value 1000
        /// </summary>
        const int VALUE_1000 = 1000;

        /// <summary>
        /// The value 16000
        /// </summary>
        const int maxSampleRate = 16000;
        /// <summary>
        /// The value 5
        /// </summary>
        const int VALUE_5 = 5;

        #endregion

        static ILog logger = LogManager.GetLogger("EnggAppAppender");

        /// <summary>
        /// The audio collection
        /// </summary>
        public ObservableCollection<DMIPmdDataPacket> AudioCollection;

        private AudioLib.IAudioWrapper wrapper;

        private bool isAudioFirst = true;

        public AudioWrapper()
        {
            wrapper = new AudioLib.AudioWrapper();
            AudioCollection = new ObservableCollection<DMIPmdDataPacket>();

            wrapper.Initialize();
        }


        #region Public methods

        /// <summary>
        /// Audioes the collection collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        public void AudioCollectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            logger.Debug("++");
            try
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    List<short> away = new List<short>();
                    List<short> toward = new List<short>();

                    if (AudioCollection.Count >= 1)
                    {
                        if (isAudioFirst)
                        {
                            isAudioFirst = false;
                            wrapper.Start();
                        }

                        var bufferSize = GetBufferSizeFromPRF(AudioCollection[0].audio.sampleRate);

                        foreach (var packet in AudioCollection)
                        {
                            away.AddRange(packet.audio.away.Take(bufferSize));
                            toward.AddRange(packet.audio.toward.Take(bufferSize));
                        }

                        wrapper.SendData(away.ToArray(), toward.ToArray(), (short)AudioCollection[0].audio.sampleRate);

                        AudioCollection.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        /// <summary>
        /// Returns buffersize based on PRF.
        /// </summary>
        /// <returns></returns>
        private int GetBufferSizeFromPRF(int prf)
        {
            var bufferSize = 64; //default for 8khz prf
            int Value_125 = 125;
            if (PRF != 0)
            {
                bufferSize = prf / Value_125;
            }

            return bufferSize;
        }

        /// <summary>
        /// Set the volume for TCD audio
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(uint volume)
        {
            wrapper.SetVolume(volume);
        }

        /// <summary>
        /// Stop the audio
        /// </summary>
        public void Stop()
        {
            wrapper.Stop();
        }

        /// <summary>
        /// Dispose audio wrapper
        /// </summary>
        public void Dispose()
        {
            wrapper.Stop();
        }
        #endregion


        /// <summary>
        /// The sound buffer
        /// </summary>
        public byte[] soundBuffer;
        /// <summary>
        /// The PRF
        /// </summary>
        public int PRF = 0;

        /// <summary>
        /// Deletes if the given file exists.
        /// </summary>
        /// <param name="fileName">Name of the file to delete.</param>
        /// <returns></returns>
        private static async Task DeleteIfFileExists(StorageFolder folder, string fileName)
        {
            logger.Debug("++");
            try
            {
                StorageFile audioFile = await folder.GetFileAsync(fileName);
                await audioFile.DeleteAsync();
            }
            catch (Exception)
            {
                // Ignore. File does not exist.
            }
            logger.Debug("--");
        }

        /// <summary>
        /// Generates the wav file.
        /// </summary>
        /// <param name="bytes">Binary data for audio.</param>
        /// <param name="sampleRate">The sample rate.</param>
        /// <param name="durationInSeconds">The duration of audio in seconds.</param>
        /// <param name="fileName">Name of the WAV file to generate.</param>
        /// <returns></returns>
        private static async Task GenerateWavFile(byte[] bytes, int sampleRate, int durationInSeconds,
            StorageFolder folder, string fileName)
        {
            logger.Debug("++");
            const short tracks = 2;
            const short bitsPerSample = 16;
            const int waveSize = 4;
            const int headerSize = 8;
            const int formatChunkSize = 16;

            try
            {
                // Delete if the file already exists.
                // This is to ensure that the filewatcher receives the file create event.
                await DeleteIfFileExists(folder, fileName);

                StorageFile audioFile = await folder.CreateFileAsync(
                    fileName, CreationCollisionOption.ReplaceExisting);

                using (System.IO.Stream stream = await audioFile.OpenStreamForWriteAsync())
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        //generate the .wav header and dump the data
                        //see http://www.topherlee.com/software/pcm-tut-wavformat.html for reference
                        short frameSize = tracks * bitsPerSample / 8;
                        int bytesPerSecond = sampleRate * frameSize;
                        int samples = (int)((decimal)sampleRate * durationInSeconds);
                        int dataChunkSize = samples * frameSize;
                        int fileSize = (2 * headerSize) + waveSize + formatChunkSize + dataChunkSize;

                        writer.Write("RIFF".ToArray());
                        writer.Write(fileSize);
                        writer.Write("WAVE".ToArray());
                        writer.Write("fmt ".ToArray());
                        writer.Write(formatChunkSize);
                        writer.Write((short)1); // format type
                        writer.Write(tracks);
                        writer.Write(sampleRate);
                        writer.Write(bytesPerSecond);
                        writer.Write(frameSize);
                        writer.Write(bitsPerSample);
                        writer.Write("data".ToArray());
                        writer.Write(dataChunkSize);
                        writer.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Warn("Exception: ", ex);
            }
            logger.Debug("--");
        }

        /// <summary>
        /// Creates the audio file.
        /// Note that for sampling frequencies BELOW 8 kHz, we have to double the sample rate
        /// and double the number of samples. The reason for this is that the AAC codec will
        /// not work for sampling rates below 8 kHz
        /// </summary>
        /// <param name="dataPackets">List of data packets.</param>
        /// <param name="fileName">Name of the file to create.</param>
        /// <returns></returns>
        public static async Task CreateAudioFile(List<DMIPmdDataPacket> dataPackets, StorageFolder folder, string fileName)
        {
            logger.Debug("++");
            int prf = dataPackets[0].audio.sampleRate;
            int numSamples = (int)prf / Constants.PACKETS_PER_SEC;

            if (dataPackets != null)
            {
                try
                {
                    var numPackets = Constants.VALUE_0;
                    int FMT_STEREO = Constants.VALUE_2;

                    int buffSize = (dataPackets.Count / Constants.PACKETS_PER_SEC) * prf * FMT_STEREO * sizeof(short);

                    if (prf == Constants.VALUE_6250 || prf == Constants.VALUE_5000)     //we must interpolate data below 8 kHz
                    {
                        buffSize *= Constants.VALUE_2;
                    }

                    byte[] soundBuffer = new byte[buffSize];

                    foreach (DMIPmdDataPacket packet in dataPackets)
                    {
                        //handle combining channels here 
                        short[] toward = packet.audio.toward;
                        short[] away = packet.audio.away;
                        //copy buffer into sound array
                        short[] sliceToward = toward.Take(numSamples).ToArray();
                        short[] sliceAway = away.Take(numSamples).ToArray();
                        short[] slice;

                        if (prf == Constants.VALUE_6250 || prf == Constants.VALUE_5000)
                        {
                            slice = new short[Constants.VALUE_4 * numSamples];
                        }
                        else
                        {
                            slice = new short[Constants.VALUE_2 * numSamples];
                        }

                        for (int i = 0; i < numSamples; i++)
                        {
                            if (prf == Constants.VALUE_6250 || prf == Constants.VALUE_5000)
                            {
                                slice[Constants.VALUE_4 * i] = sliceAway[i];
                                slice[Constants.VALUE_4 * i + Constants.VALUE_1] = sliceToward[i];
                                slice[Constants.VALUE_4 * i + Constants.VALUE_2] = i < numSamples - Constants.VALUE_1 ?
                                    (short)((sliceAway[i + Constants.VALUE_1] + sliceAway[i]) / Constants.VALUE_2) : sliceAway[i];
                                slice[Constants.VALUE_4 * i + Constants.VALUE_3] = i < numSamples - Constants.VALUE_1 ?
                                    (short)((sliceToward[i + Constants.VALUE_1] + sliceToward[i]) / Constants.VALUE_2) : sliceToward[i];
                            }
                            else
                            {
                                slice[Constants.VALUE_2 * i] = sliceAway[i];
                                slice[Constants.VALUE_2 * i + Constants.VALUE_1] = sliceToward[i];
                            }
                        }
                        if (prf == Constants.VALUE_6250 || prf == Constants.VALUE_5000)
                        {
                            int offset = sizeof(short) * numPackets * numSamples * Constants.VALUE_4;

                            if (offset >= buffSize)
                            {
                                break;
                            }

                            System.Buffer.BlockCopy(slice, Constants.VALUE_0, soundBuffer, offset, Constants.VALUE_4 * numSamples * sizeof(short));
                        }
                        else
                        {
                            int offset = sizeof(short) * numPackets * numSamples * Constants.VALUE_2;

                            if (offset >= buffSize)
                            {
                                break;
                            }
                            System.Buffer.BlockCopy(slice, Constants.VALUE_0, soundBuffer, offset, Constants.VALUE_2 * numSamples * sizeof(short));
                        }
                        numPackets++;
                    }

                    int durationInSeconds = dataPackets.Count / Constants.PACKETS_PER_SEC;

                    if (prf == Constants.VALUE_6250 || prf == Constants.VALUE_5000)
                    {
                        await GenerateWavFile(soundBuffer, prf * Constants.VALUE_2, durationInSeconds, folder, fileName);
                    }
                    else
                    {
                        await GenerateWavFile(soundBuffer, prf, durationInSeconds, folder, fileName);
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn("Exception: ", ex);
                }
            }
            logger.Debug("--");
        }
    }
}
