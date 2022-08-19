#include "pch.h"

// Mod Library
HMODULE m_Library;

// Mod Types
typedef void(__cdecl* _OnInit)(int pid, uintptr_t pSongData);
typedef void(__cdecl* _OnDispose)();
typedef void(__cdecl* _OnSongUpdate)(int songId, bool isPractice);

// Mod Functions
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
 * Hooks
 */

 // v1.02: 0x14040B270 (ActualMandM), 0x14040B260 (Me)
SIG_SCAN
(
	sigSongData,
	0x14040B260,
	"\x48\x8D\x05\xCC\xCC\xCC\xCC\xC3\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\x48\x89\x5C\x24\x08\x48\x89\x6C\x24\x10\x48\x89\x74\x24\x18\x57\x48\x83\xEC\x20",
	"xxx????x????????xxxxxxxxxxxxxxxxxxxx"
)

// 1.02: 0x14043B2D0
SIG_SCAN
(
	sigSongStart,
	0x14043B2D0,
	"\x8B\xD1\xE9\xA9\xE8\xFF\xFF\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xE9",
	"xxxxxxx?????????x"
);

// 1.02: 0x14043B2D0
SIG_SCAN
(
	sigSongPracticeStart,
	0x1401E7A60,
	"\xE9\x00\x00\x00\x00\xA3\xF6\x42\xF3\xF8\x58\xFD\x35\x1D",
	"x????xxxxxxxxx"
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
	if (m_Library)
	{
		// Playing
		p_OnSongUpdate(songId, false);
	}

	original_SongStart(songId);
}

HOOK(__int64, __fastcall, _SongPracticeStart, sigSongPracticeStart(), __int64 a1, __int64 a2)
{
	if (m_Library)
	{
		// Practicing
		p_OnSongUpdate(0, true);
	}

	return original_SongPracticeStart(a1, a2);
}

HOOK(__int64, __stdcall, _SongEnd, sigSongEnd())
{
	if (m_Library)
	{
		// In Menu
		p_OnSongUpdate(0, false);
	}

	return original_SongEnd();
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
		INSTALL_HOOK(_SongPracticeStart);

		// Current PID
		auto pid = GetCurrentProcessId();

		// Mod Entry Point
		auto addr = (uint8_t*)sigSongData();
		auto pointer = (uintptr_t)(addr + ReadUnalignedU32(addr + 0x3));

		p_OnInit(pid, pointer);
	}
}
