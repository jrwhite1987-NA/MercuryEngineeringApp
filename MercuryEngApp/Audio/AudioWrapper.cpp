// AudioWrapper.cpp : Implementation of CAudioWrapper

#include "stdafx.h"
#include "AudioRenderer.h"
#include "AudioWrapper.h"
#include <vector>

// CAudioWrapper

STDMETHODIMP CAudioWrapper::Initialize(VARIANT_BOOL* bReturn)
{
	HRESULT hr = m_pRenderer->InitializeXAudio2();
	*bReturn = FAILED(hr) ? VARIANT_FALSE : VARIANT_TRUE;
	return hr;
}

STDMETHODIMP CAudioWrapper::Start(VARIANT_BOOL* bReturn)
{
	*bReturn = (m_pRenderer->StartAudio() == true) ? VARIANT_TRUE : VARIANT_FALSE;
	return S_OK;
}

STDMETHODIMP CAudioWrapper::SetVolume(UINT32 volume, VARIANT_BOOL* bReturn)
{
	HRESULT hr = m_pRenderer->SetVolumeOnSession(volume);
	*bReturn = FAILED(hr) ? VARIANT_FALSE : VARIANT_TRUE;
	return hr;
}

STDMETHODIMP CAudioWrapper::SendData(SAFEARRAY* away, SAFEARRAY* towards, short sRate, VARIANT_BOOL* bReturn)
{
	std::vector<short> vecAway;
	std::vector<short> vecTowards;

	FillVector(away, vecAway);
	FillVector(towards, vecTowards);
	
	*bReturn = (m_pRenderer->LoadTCD(vecAway, vecTowards, sRate) == true) ?
	VARIANT_TRUE : VARIANT_FALSE;

	wchar_t sval[80];
	
	for (auto it = vecAway.begin(); it != vecAway.end(); ++it)
	{
		wsprintf(sval, _TEXT("Away: %d"), *it);
		OutputDebugStringW(sval);
	}

	for (auto it = vecTowards.begin(); it != vecTowards.end(); ++it)
	{
		wsprintf(sval, _TEXT("vecTowards: %d"), *it);
		OutputDebugStringW(sval);
	}

	return S_OK;
}

STDMETHODIMP CAudioWrapper::Stop(VARIANT_BOOL* bReturn)
{
	*bReturn = (m_pRenderer->StartAudio() == true) ? VARIANT_TRUE : VARIANT_FALSE;
	return S_OK;
}

STDMETHODIMP CAudioWrapper::Destroy(VARIANT_BOOL* bReturn)
{
	*bReturn = VARIANT_TRUE;
	return S_OK;
}

HRESULT CAudioWrapper::FillVector(SAFEARRAY* saData, std::vector<short>& refContents)
{
	void *pVoid = 0;

	HRESULT hr = ::SafeArrayAccessData(saData, &pVoid);
	const short *pData = reinterpret_cast<short *>(pVoid);

	refContents.clear();
	refContents.assign(pData, pData + saData->rgsabound[0].cElements);

	hr = ::SafeArrayUnaccessData(saData);

	return S_OK;
}