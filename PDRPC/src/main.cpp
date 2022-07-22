#include "pch.h"

// Mod Library
HMODULE m_Library;

// Mod Types
typedef void(__cdecl* _OnInit)();
typedef void(__cdecl* _OnDispose)();
typedef void(__cdecl* _OnSongUpdate)(int songId);

// Mod Pointers
_OnInit p_OnInit;
_OnDispose p_OnDispose;
_OnSongUpdate p_OnSongUpdate;


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
		p_OnSongUpdate = (_OnSongUpdate)GetProcAddress(m_Library, "OnSongUpdate");

		return true;
	}

	return false;
}


/*
 * Signatures
 */
SIG_SCAN
(
	sigSongStart,
	0x14043A5C0,
	"\xe8\x00\x00\x00\x00\x41\xb8\x00\x00\x00\x00\x48\x8d\x55\x00\x48\x8d\x0d\x00\x00\x00\x00\xe8\x00\x00\x00\x00\x84\xc0\x74\x00\xc7\x05\x00\x00\x00\x00\x00\x00\x00\x00\x41\xbe\x00\x00\x00\x00\xe9\x00\x00\x00\x00\x48\x8d\x0d\x00\x00\x00\x00\xe8\x00\x00\x00\x00\xc7\x05",
	"x????xx????xxx?xxx????x????xxx?xx????????xx????x????xxx????x????xx"
);

SIG_SCAN
(
	sigSongEnd,
	0x14043AFF8,
	"\x48\x89\x5c\x24\x00\x57\x48\x83\xec\x00\x48\x8d\x0d\x00\x00\x00\x00\xe8\x00\x00\x00\x00\x48\x8b\x3d",
	"xxxx?xxxx?xxx????x????xxx"
);


/*
 * Hooks
 */
HOOK(__int64, __fastcall, _SongStart, sigSongStart(), char* unknown1, __int64 unknown2, char* lightParam, int songId)
{
	if (m_Library)
	{
		// Song Update Event
		p_OnSongUpdate(songId);
	}

	return original_SongStart(unknown1, unknown2, lightParam, songId);
};

HOOK(__int64, __fastcall, _SongEnd, sigSongEnd())
{
	if (m_Library)
	{
		// Menus
		p_OnSongUpdate(0);
	}

	return original_SongEnd();
};


/*
 * ModLoader
 */
extern "C" __declspec(dllexport) void Init()
{
	// Load Mod Library
	if (LoadModLibrary())
	{
		// Hooks
		INSTALL_HOOK(_SongStart);
		INSTALL_HOOK(_SongEnd);

		// Mod Entry Point
		p_OnInit();
	}
}
