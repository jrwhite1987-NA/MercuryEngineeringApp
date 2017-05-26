#include <Windows.h>
#include <mfapi.h>
#include <AudioClient.h>
#include <audioPolicy.h>
#include <mmdeviceapi.h>
#include <xaudio2.h>
#include <mftransform.h>
#include <mediaobj.h>
#include <robuffer.h>
#include <math.h>
#include <vector>
#include <list>

#define NUM_RATES 5

class AudioRenderer
{
public:
	AudioRenderer();
	static const int sampleRates[NUM_RATES];
	HRESULT InitializeXAudio2();

	HRESULT SetVolumeOnSession(UINT32 volume);
	~AudioRenderer();
	bool LoadTCD(std::vector<short> away, std::vector<short> towards, short sRate);
	bool StopAudio();
	bool StartAudio();
	unsigned short sampleRate = 0;
	unsigned short pendingSampleRate = 0;
	//used for debugging purposes
	std::vector<short> towardsData;
	std::vector<short> awayData;

private:
	void SubmitBuffers(void);
	void ReleasePlayedBuffers(void);
	void ReleaseAllBuffers(void);
	void AddBuffer(std::vector<short> away, std::vector<short> towards);
	void CheckBuildupMode(void);

private:
	enum { DEFAULT_SAMPLE_RATE = 8000 };

	//XAudio2 objects
	IXAudio2* pXAudio2;						//XAudio2 engine instance
	IXAudio2MasteringVoice* pMasterVoice;	//Master voice
	IXAudio2SourceVoice* pSourceVoice;		//Source voice
	int curSampleRate = DEFAULT_SAMPLE_RATE;

	//holds TCD data
	enum { BUILDUP_SIZE = 3 };

	struct Bundle
	{
		std::vector<short> data;
		bool submitted;

		Bundle()
		{
			submitted = false;
		}
	};

	std::list< Bundle > m_Buffs;
	bool m_BuildupMode = true;
};

