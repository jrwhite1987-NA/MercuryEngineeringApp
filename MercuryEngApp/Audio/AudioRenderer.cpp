#include "stdafx.h"
#include "AudioRenderer.h"

AudioRenderer::AudioRenderer()
{
	pXAudio2 = NULL;
	pMasterVoice = NULL;
	pSourceVoice = NULL;
}

static WAVEFORMATEX WaveFormatFromSampleRate(int sampleRate)
{
	// for 16-bit, dual channel
	// add additional parameters to function as desired
	WAVEFORMATEX wfx;
	wfx.wFormatTag = WAVE_FORMAT_PCM;
	wfx.nChannels = 2;
	wfx.nSamplesPerSec = sampleRate;
	wfx.nAvgBytesPerSec = sampleRate * wfx.nChannels * sizeof(short);
	wfx.wBitsPerSample = 8 * sizeof(short);
	wfx.nBlockAlign = wfx.wBitsPerSample*wfx.nChannels / 8;
	wfx.cbSize = 0;

	return wfx;
}

AudioRenderer::~AudioRenderer()
{
	pSourceVoice->FlushSourceBuffers();
	pSourceVoice->DestroyVoice();
	pMasterVoice->DestroyVoice();
	pXAudio2->Release();
}

HRESULT AudioRenderer::InitializeXAudio2()
{
	HRESULT hr = S_OK;
	/*
	Create an instance of the XAudio2 engine

	Out: Returns pointer to IXAudio2 object
	In: Flags (must be zero)
	In: The type of CPU to use
	Return: S_OK or XAudio2 error code.
	https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2create(v=vs.85).aspx
	*/

	
	hr = XAudio2Create(&pXAudio2, 0, XAUDIO2_DEFAULT_PROCESSOR);
	if FAILED(hr)
		return hr;

	/*
	Create the master voice.

	Out: Returns a pointer to the master voice object.
	In: number of input channels (default channels)
	In: Sample rate (default sample rate)
	In: Flags (must be zero)
	In: Device id for audio output (global default audio device)
	Return: S_OK or XAudio2 error code.
	https://msdn.microsoft.com/en-us/library/windows/desktop/hh405048(v=vs.85).aspx
	*/
	hr = pXAudio2->CreateMasteringVoice(&pMasterVoice);
	if (FAILED(hr))
		return hr;
	/*
	Create a source voice. Default to 8 kHz sample rate.
	https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2.ixaudio2.createsourcevoice(v=vs.85).aspx
	*/

	WAVEFORMATEX wfx = WaveFormatFromSampleRate(curSampleRate);
	pXAudio2->CreateSourceVoice(&pSourceVoice, &wfx);
	return hr;
}

void AudioRenderer::SubmitBuffers(void)
{
	for (auto it = m_Buffs.begin(); it != m_Buffs.end(); ++it)
	{
		if (!it->submitted)
		{
			/*
			Create an xaudio buffer
			https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.xaudio2.xaudio2_buffer(v=vs.85).aspx
			*/
			XAUDIO2_BUFFER buf = { 0 };
			buf.AudioBytes = it->data.size()*sizeof(short);
			buf.pAudioData = (BYTE*)it->data.data();
			/*
			Submit a buffer to the source voice.
			https://msdn.microsoft.com/en-us/library/windows/desktop/microsoft.directx_sdk.ixaudio2sourcevoice.ixaudio2sourcevoice.submitsourcebuffer(v=vs.85).aspx
			*/
			pSourceVoice->SubmitSourceBuffer(&buf);
			it->submitted = true;
		}
	}
}

void AudioRenderer::ReleasePlayedBuffers(void)
{
	if (pSourceVoice == NULL)
		return;

	// Determine Number active in voice
	XAUDIO2_VOICE_STATE state;
	pSourceVoice->GetState(&state);
	UINT numInVoice = state.BuffersQueued;

	// Determine number submitted to voice
	UINT numSubmitted = 0;
	for (auto it = m_Buffs.begin(); it != m_Buffs.end(); ++it)
	{
		if (it->submitted)
			numSubmitted++;
	}

	// Pop used - this takes for granted that used buffers are in front of the line
	if (numInVoice >= numSubmitted)
		return;

	UINT numPop = numSubmitted - numInVoice;
	for (size_t i = 0; i < numPop; ++i)
		m_Buffs.pop_front();
}

void AudioRenderer::ReleaseAllBuffers(void)
{
	m_Buffs.clear();
}

void AudioRenderer::AddBuffer(std::vector<short> away, std::vector<short> towards)
{
	// Make buffer large enough to fit larger of the two arrays
	// Fill buffer { away[0], to[0], away[1], to[1], .....
	// fill with zeros if buffers have different sizes

	size_t count = max(away.size(), towards.size());
	if (count == 0)
		return; // error condition?

	m_Buffs.push_back(Bundle());
	std::vector<short>& data = m_Buffs.back().data;

	data.resize(count * 2);

	for (size_t i = 0; i < away.size(); i++)
	{
		data[2 * i] = away.at(i);
	}

	for (size_t i = 0; i < towards.size(); i++)
	{
		data[2 * i + 1] = towards.at(i);
	}
}

void AudioRenderer::CheckBuildupMode(void)
{
	if (m_BuildupMode == false )
		return;

	// Do not update while a rate is pending
	if (pendingSampleRate)
		return;
	
	// Count non submitted buffers
	UINT n = 0;
	for (auto it = m_Buffs.begin(); it != m_Buffs.end(); ++it)
	{
		if (!it->submitted)
			n++;
	}

	if ( n >= BUILDUP_SIZE)
		m_BuildupMode = false;
}

bool AudioRenderer::LoadTCD(std::vector<short> away, std::vector<short> toward, short sRate)
{
	//if the packet contains a new sample rate, clear the queue and set the new sample rate
	if (sRate != curSampleRate)
	{
		// This is tricky
		// FlushSourceBuffers removes buffers waiting, but not the buffer currently playing
		// We can't clear the memory because the deleting the currently used data is disastrous
		// On top of this, the play order in the voice and in our data structures is a little confused
		// A proper approach would be to delete the processed buffers starting from the tail
		// and leaving N in the list
		// Better to just leave it be while we repeat the buildup. There is no rush to free this memory
		// It will get deleted once buildup is complete and processing resumes
		pSourceVoice->FlushSourceBuffers();
		m_BuildupMode = true;
		pendingSampleRate = sRate;
	}

	if (pendingSampleRate)
	{
		HRESULT hr = pSourceVoice->SetSourceSampleRate(pendingSampleRate);
		if (SUCCEEDED(hr))
		{
			curSampleRate = pendingSampleRate;
			pendingSampleRate = 0;
		}
	}

	AddBuffer(away, toward);

	CheckBuildupMode();

	if (!m_BuildupMode)
	{
		SubmitBuffers();

		// Do not release during buildup
		ReleasePlayedBuffers();
	}

	return true;
}

/*
Stops the Audio
*/
bool AudioRenderer::StopAudio()
{
	if (pSourceVoice){
		pSourceVoice->Stop();
	}
	return true;
}

/*
Starts the Audio
*/
bool AudioRenderer::StartAudio()
{
	//if (!bufferReady)
	//	return false;
	if (pSourceVoice){
		pSourceVoice->Start(0);
	}
	return true;
}

//
//  SetVolumeOnSession()
//
HRESULT AudioRenderer::SetVolumeOnSession(UINT32 volume)
{
	HRESULT hr = S_OK;
	hr = pMasterVoice->SetVolume(volume / 10.0);
	return hr;
}
