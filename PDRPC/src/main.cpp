#include "pch.h"

// Mod Library
int m_ProcessId;
HMODULE m_Library;

// Mod Types
typedef void(__cdecl* _OnInit)(int processId);
typedef void(__cdecl* _OnDispose)();

// Mod Pointers
_OnInit p_OnInit;
_OnDispose p_OnDispose;


/*
 * Entry
 */
BOOL APIENTRY DllMain(HMODULE hModule, DWORD reason, LPVOID lpReserved)
{
	switch (reason)
	{
	case DLL_PROCESS_ATTACH:
		break;
	case DLL_THREAD_ATTACH:
		break;
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		if (m_Library)
		{
			// Mod Dispose
			p_OnDispose();
		}
		break;
	}

	return TRUE;
}


/*
 * Core Library
 */
bool LoadModLibrary()
{
	m_Library = LoadLibraryA("PDRPC.Core.dll");

	if (m_Library)
	{
		p_OnInit = (_OnInit)GetProcAddress(m_Library, "OnInit");
		p_OnDispose = (_OnDispose)GetProcAddress(m_Library, "OnDispose");

		// Current Process
		m_ProcessId = GetCurrentProcessId();

		return true;
	}

	return false;
}


/*
 * ModLoader
 */
extern "C" __declspec(dllexport) void Init()
{
	// Load Mod Library
	if (LoadModLibrary())
	{
		// Mod Entry Point
		p_OnInit(m_ProcessId);
	}
}
