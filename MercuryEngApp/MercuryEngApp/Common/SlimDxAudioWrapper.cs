using System;
using System.Collections.Generic;
using System.Linq;
using SlimDX.XAudio2;
using SlimDX.Multimedia;
using SlimDX;
using System.Runtime.InteropServices;

namespace AudioLib
{
    internal class SlimDxAudioWrapper : IDisposable
    {
        const int NUM_RATES = 5;
        const int DEFAULT_SAMPLE_RATE = 8000;
        static int[] sampleRates = new int[NUM_RATES];
        const int BUILDUP_SIZE = 3;

        ushort pendingSampleRate = 0;

        XAudio2 pXAudio2; //XAudio2 engine instance
        SlimDX.XAudio2.MasteringVoice pMasterVoice; //Master voice
        SlimDX.XAudio2.SourceVoice pSourceVoice; //Source voice
        int curSampleRate = DEFAULT_SAMPLE_RATE;

        private class Bundle
        {
            public List<short> data;
            public bool submitted;
        }
        List<Bundle> m_Buffs = new List<Bundle>();
        bool m_BuildupMode = true;

        public void Initialize()
        {
            pXAudio2 = new XAudio2(0, ProcessorSpecifier.DefaultProcessor);
            pMasterVoice = new MasteringVoice(pXAudio2);
            pSourceVoice = new SourceVoice(pXAudio2, WaveFormatFromSampleRate(curSampleRate));
        }

        public bool Start()
        {
            if (pSourceVoice != null)
            {
                pSourceVoice.Start(PlayFlags.None);
            }

            return true;
        }

        public void SetVolume(UInt32 volume)
        {
            pMasterVoice.Volume = (float)(volume/10.0f);
        }

        public bool SendData(List<short> away, List<short> towards, short sRate)
        {
            return LoadTCD(away, towards, sRate);
        }

        public bool Stop()
        {
            if (pSourceVoice != null)
            {
                pSourceVoice.Stop();
            }

            return true;
        }

        public void ReleaseAllBuffers()
        {
            m_Buffs.Clear();
        }

        public void Dispose()
        {
            pSourceVoice.FlushSourceBuffers();
            pSourceVoice.Dispose();

            pMasterVoice.Dispose();
            pXAudio2.Dispose();
        }

        private bool LoadTCD(List<short> away, List<short> towards, short sRate)
        {
            if (sRate != curSampleRate)
            {
                pSourceVoice.FlushSourceBuffers();
                m_BuildupMode = true;
                pendingSampleRate = (ushort)sRate;
            }

            if (pendingSampleRate != 0)
            {
                SlimDX.Result result  = pSourceVoice.SetSourceSampleRate(pendingSampleRate);
                if(result.IsSuccess)
                {
                    curSampleRate = pendingSampleRate;
                    pendingSampleRate = 0;
                }
            }

            AddBuffer(away, towards);

            CheckBuildupMode();
            if(!m_BuildupMode)
            {
                SubmitBuffers();
                // Do not release during buildup
                ReleasePlayedBuffers();
            }

            return true;
        }
        
        private byte[] GetBytes(List<short> lstData)
        {
            byte[] allBytes = new byte[lstData.Count * sizeof(short)];
            for(int i = 0; i < lstData.Count;i++)
            {
                byte[] bytes = System.BitConverter.GetBytes(lstData[i]);
                bytes.CopyTo(allBytes, i * sizeof(short));
            }

            return allBytes;
        }

        private void SubmitBuffers()
        {
            try
            {
                for (int i = 0; i < m_Buffs.Count; i++)
                {
                    var bundle = m_Buffs[i];
                    if (!bundle.submitted)
                    {
                        using (AudioBuffer buf = new AudioBuffer())
                        {
                            byte[] bytes = GetBytes(bundle.data);
                            buf.AudioBytes = bytes.Length;
                            
                            buf.AudioData = new System.IO.MemoryStream(); 
                            buf.AudioData.Write(bytes, 0, bytes.Length);
                            buf.AudioData.Position = 0;

                            pSourceVoice.SubmitSourceBuffer(buf);
                        }
                        bundle.submitted = true;
                    }
                }
            }
            catch(Exception ex)
            {
                string str = ex.Message;
            }
        }

        private void ReleasePlayedBuffers()
        {
            if(pSourceVoice == null)
            {
                return;
            }

            VoiceState state = pSourceVoice.State;
            int numInVoice = state.BuffersQueued;

            uint numSubmitted = 0;
            foreach (var bundle in m_Buffs)
            {
                if (bundle.submitted)
                {
                    numSubmitted++;
                }
            }

            if (numInVoice >= numSubmitted)
            {
                return;
            }

            uint numPop = (uint)(numSubmitted - numInVoice);

            for(int i = 0 ; i < numPop; i++)
            {
                m_Buffs.RemoveAt(i);
            }
        }

        

        private void AddBuffer(List<short> away, List<short> towards)
        {
            int count = Math.Max(away.Count, towards.Count);
            if (count == 0)
            {
                return;
            }

            short[] arrData = new short[count * 2];
            for(int i = 0 ; i < away.Count ; i++)
            {
                arrData[2 * i] = away.ElementAt(i);
            }
            for (int j = 0; j < towards.Count; j++)
            {
                arrData[2 * j + 1] = towards.ElementAt(j);
            }
            
            m_Buffs.Add(new Bundle());
            m_Buffs.Last().data = arrData.ToList();
        }

        private void CheckBuildupMode()
        {
            if (m_BuildupMode == false)
            {
                return;
            }

            if (pendingSampleRate != 0)
                return;

            uint n = 0;
            foreach(var bundle in m_Buffs)
            {
                if(!bundle.submitted)
                {
                    n++;
                }
            }

            if(n > BUILDUP_SIZE)
            {
                m_BuildupMode = false;
            }
        }

        

        private WaveFormatExtensible WaveFormatFromSampleRate(int sampleRate)
        {
            WaveFormatExtensible wfx = new WaveFormatExtensible();
            wfx.FormatTag = WaveFormatTag.Pcm;
            wfx.Channels = 2;
            wfx.SamplesPerSecond = sampleRate;
            wfx.AverageBytesPerSecond = sampleRate * wfx.Channels * sizeof(short);
            wfx.BitsPerSample = 8 * sizeof(short);
            wfx.BlockAlignment = (short)(wfx.BitsPerSample * wfx.Channels / 8);

            return wfx;
        }
    }
}
