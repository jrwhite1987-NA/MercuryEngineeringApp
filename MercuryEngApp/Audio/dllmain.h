// dllmain.h : Declaration of module class.

class CAudioModule : public ATL::CAtlDllModuleT< CAudioModule >
{
public :
	DECLARE_LIBID(LIBID_AudioLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_AUDIO, "{0A20C0EC-2BCC-4E0D-BC9D-C73E2F6C8E59}")
};

extern class CAudioModule _AtlModule;
