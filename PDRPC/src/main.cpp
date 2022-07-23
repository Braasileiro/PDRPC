#include "pch.h"

// Mod Library
HMODULE m_Library;

// Mod Types
typedef void(__cdecl* _OnInit)(int processId);
typedef void(__cdecl* _OnDispose)();

// Mod Functions
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
		if (m_Library) p_OnDispose();
		break;
	}

	return TRUE;
}

bool LoadModLibrary()
{
	m_Library = LoadLibraryA("PDRPC.Core.dll");

	if (m_Library)
	{
		p_OnInit = (_OnInit)GetProcAddress(m_Library, "OnInit");
		p_OnDispose = (_OnDispose)GetProcAddress(m_Library, "OnDispose");

		return true;
	}

	return false;
}


/*
 * Hooks
 */

// 1.02: 0x14043B2D0
SIG_SCAN
(
	sigSongStart,
	0x14043B2D0,
	"\x8B\xD1\xE9\xA9\xE8\xFF\xFF\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xE9",
	"xxxxxxx?????????x"
);

// 1.02: 0x14043B000
SIG_SCAN
(
	sigSongEnd,
	0x14043B000,
	"\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x20\x48\x8D\x0D\xCC\xCC\xCC\xCC\xE8\xCC\xCC\xCC\xCC\x48\x8B\x3D\xCC\xCC\xCC\xCC\x48\x8B\x1F\x48\x3B\xDF",
	"xxxxxxxxxxxxx????x????xxx????xxxxxx"
);

HOOK(void, __fastcall, _SongStart, sigSongStart(), int songId)
{
	original_SongStart(songId);
}

HOOK(__int64, __cdecl, _SongEnd, sigSongEnd())
{
	original_SongEnd();
}


/*
 * ModLoader
 */
extern "C" __declspec(dllexport) void Init()
{
	// Load Mod Library
	if (LoadModLibrary())
	{
		// Install Hooks
		INSTALL_HOOK(_SongStart);
		INSTALL_HOOK(_SongEnd);

		// Mod Entry Point
		p_OnInit(GetCurrentProcessId());
	}
}
