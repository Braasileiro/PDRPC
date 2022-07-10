#include "main.h"

// State
static bool isHosting{ true };
static bool isAssembly{ true };

// CLR
static ICLRMetaHost* pMetaHost{ NULL };
static ICLRRuntimeInfo* pRuntimeInfo{ NULL };
static ICLRRuntimeHost* pRuntimeHost{ NULL };

// Assembly
static std::wstring assemblyDir;
static std::wstring assemblyPath;
static const wchar_t* assemblyMethod{ L"Init" };
static const wchar_t* assemblyType{ L"PDRPC.Mod" };
static const wchar_t* assemblyName{ L"PDRPC.Core.dll" };


/*
 * Entry
 */
BOOL APIENTRY DllMain(HMODULE hModule, DWORD reason, LPVOID lpReserved)
{	
    switch (reason)
    {
		case DLL_PROCESS_ATTACH:
			Logger::Redirect();
			CLR_Assembly(hModule);
			break;
		case DLL_THREAD_ATTACH: break;
		case DLL_THREAD_DETACH: break;
		case DLL_PROCESS_DETACH:
			CLR_Dispose();
			break;
	}

    return TRUE;
}


/*
 * CLR
 */
void CLR_Assembly(HMODULE& hModule)
{
	wchar_t buffer[MAX_PATH + 1];
	GetModuleFileName(hModule, buffer, _countof(buffer));

	auto me = std::wstring(buffer);
	assemblyDir = me.substr(0, me.find_last_of(L"\\/"));
	assemblyPath = assemblyDir + L"\\" + assemblyName;

	// Exists (?)
	if (!std::filesystem::exists(assemblyPath))
	{
		isAssembly = false;

		Logger::Error("Core library not found. Please check your mod folder.");
	}
}

void CLR_Start()
{
	if (CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&pMetaHost) == S_OK)
	{
		if (pMetaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (LPVOID*)&pRuntimeInfo) == S_OK)
		{
			if (pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (LPVOID*)&pRuntimeHost) == S_OK)
			{
				if (pRuntimeHost->Start() == S_OK)
				{
					Logger::Info("CLR started.");

					DWORD pReturnValue;

					pRuntimeHost->ExecuteInDefaultAppDomain(
						assemblyPath.c_str(),
						assemblyType,
						assemblyMethod,
						assemblyDir.c_str(),
						&pReturnValue
					);

					if (pReturnValue == 1)
					{
						Logger::Error("Error reported from the core library. Disposing the CLR.");

						CLR_Dispose();
					}

					return;
				}
			}
		}
	}

	isHosting = false;

	Logger::Error("Failed to start CLR. Check if your .NET Framework is functioning properly.");
}

void CLR_Dispose()
{
	if (isAssembly && isHosting)
	{
		if (pRuntimeInfo != NULL && pMetaHost != NULL && pRuntimeHost != NULL)
		{
			pRuntimeInfo->Release();
			pMetaHost->Release();
			pRuntimeHost->Stop();
			isHosting = false;

			Logger::Info("CLR disposed.");
		}
	}
}


/*
 * ModLoader
 */
extern "C" __declspec(dllexport) void Init()
{
	if (isAssembly)
	{
		// Start CLR
		CLR_Start();
	}
}
