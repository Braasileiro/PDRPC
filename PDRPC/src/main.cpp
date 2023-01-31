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


/*
 * Signatures
 */

// 1.02: 0x14040B260
// 1.03: 0x14040B2A0
SIG_SCAN
(
	sigSongData,
	0x14040B2A0,
	"\x48\x8D\x05\xCC\xCC\xCC\xCC\xC3\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\x48\x89\x5C\x24\x08\x48\x89\x6C\x24\x10\x48\x89\x74\x24\x18\x57\x48\x83\xEC\x20",
	"xxx????x????????xxxxxxxxxxxxxxxxxxxx"
)

// 1.02: 0x14043B2D0
// 1.03: 0x14043B310
SIG_SCAN
(
	sigSongStart,
	0x14043B310,
	"\x8B\xD1\xE9\xA9\xE8\xFF\xFF\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xCC\xE9",
	"xxxxxxx?????????x"
);

// 1.02: 0x1401E7A60
// 1.03: 0x1401E7A70
SIG_SCAN
(
	sigSongPracticeStart,
	0x1401E7A70,
	"\xE9\x00\x00\x00\x00\x58\x3C\xB4",
	"x????xxx"
);

// 1.02: 0x14043B000
// 1.03: 0x14043B040
SIG_SCAN
(
	sigSongEnd,
	0x14043B040,
	"\x48\x89\x5C\x24\x08\x57\x48\x83\xEC\x20\x48\x8D\x0D\xCC\xCC\xCC\xCC\xE8\xCC\xCC\xCC\xCC\x48\x8B\x3D\xCC\xCC\xCC\xCC\x48\x8B\x1F\x48\x3B\xDF",
	"xxxxxxxxxxxxx????x????xxx????xxxxxx"
);


/*
 * Hooks
 */
static void Update(int songId, bool isPractice)
{
	if (m_Library)
	{
		p_OnSongUpdate(songId, isPractice);
	}
}

HOOK(void, __fastcall, _SongStart, sigSongStart(), int songId)
{
	// Playing
	Update(songId, false);

	original_SongStart(songId);
}

HOOK(void, __fastcall, _SongPracticeStart, sigSongPracticeStart(), int a1, int *a2)
{
	// Practicing
	Update(0, true);

	original_SongPracticeStart(a1, a2);
}

HOOK(__int64, __stdcall, _SongEnd, sigSongEnd())
{
	// In Menu
	Update(0, false);

	return original_SongEnd();
}


/*
 * ModLoader
 */
extern "C" __declspec(dllexport) void Init()
{
	if (!sigValid)
	{
		MessageBox(
			nullptr,
			TEXT("Incompatible game version. Check if DivaMegaMix.exe is updated to 1.03 version."),
			L"PDRPC",
			MB_ICONERROR
		);
	}
	else
	{
		// Load Mod Library
		m_Library = LoadLibraryA("PDRPC.Core.dll");

		if (m_Library)
		{
			// Mod Function Pointers
			p_OnInit = (_OnInit)GetProcAddress(m_Library, "OnInit");
			p_OnDispose = (_OnDispose)GetProcAddress(m_Library, "OnDispose");
			p_OnSongUpdate = (_OnSongUpdate)GetProcAddress(m_Library, "OnSongUpdate");

			// Install Hooks
			INSTALL_HOOK(_SongStart);
			INSTALL_HOOK(_SongEnd);
			INSTALL_HOOK(_SongPracticeStart);

			// Current PID
			auto pid = GetCurrentProcessId();

			// 1.02: 0x14040B260
			// 1.03: 0x14040B2A0
			auto addr = (uint8_t*)sigSongData();

			// 1.02, 1.03: 0x1416E2B89
			auto pointer = (uintptr_t)(addr + ReadUnalignedU32(addr + 0x3));

			// Mod Entry Point
			p_OnInit(pid, pointer);
		}
	}
}
