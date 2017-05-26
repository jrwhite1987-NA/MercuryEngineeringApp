// AudioWrapper.h : Declaration of the CAudioWrapper

#pragma once
#include "resource.h"       // main symbols



#include "Audio_i.h"



#if defined(_WIN32_WCE) && !defined(_CE_DCOM) && !defined(_CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA)
#error "Single-threaded COM objects are not properly supported on Windows CE platform, such as the Windows Mobile platforms that do not include full DCOM support. Define _CE_ALLOW_SINGLE_THREADED_OBJECTS_IN_MTA to force ATL to support creating single-thread COM object's and allow use of it's single-threaded COM object implementations. The threading model in your rgs file was set to 'Free' as that is the only threading model supported in non DCOM Windows CE platforms."
#endif

using namespace ATL;


// CAudioWrapper

class ATL_NO_VTABLE CAudioWrapper :
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CAudioWrapper, &CLSID_AudioWrapper>,
	public IDispatchImpl<IAudioWrapper, &IID_IAudioWrapper, &LIBID_AudioLib, /*wMajor =*/ 1, /*wMinor =*/ 0>
{
public:
	CAudioWrapper()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_AUDIOWRAPPER)

DECLARE_NOT_AGGREGATABLE(CAudioWrapper)

BEGIN_COM_MAP(CAudioWrapper)
	COM_INTERFACE_ENTRY(IAudioWrapper)
	COM_INTERFACE_ENTRY(IDispatch)
END_COM_MAP()



	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{
		m_pRenderer = new AudioRenderer;
		return S_OK;
	}

	void FinalRelease()
	{
		if (m_pRenderer != NULL)
		{
			delete m_pRenderer;
			m_pRenderer = NULL;
		}
	}

public:

	STDMETHOD(Initialize)(VARIANT_BOOL* bReturn);
	STDMETHOD(Start)(VARIANT_BOOL* bReturn);
	STDMETHOD(SetVolume)(UINT32 volume, VARIANT_BOOL* bReturn);
	STDMETHOD(SendData)(SAFEARRAY* away, SAFEARRAY* towards, short sRate, VARIANT_BOOL* bReturn);
	STDMETHOD(Stop)(VARIANT_BOOL* bReturn);
	STDMETHOD(Destroy)(VARIANT_BOOL* bReturn);

private:
	AudioRenderer* m_pRenderer;

	HRESULT FillVector(SAFEARRAY* saData, std::vector<short>& refContents);
};

OBJECT_ENTRY_AUTO(__uuidof(AudioWrapper), CAudioWrapper)
