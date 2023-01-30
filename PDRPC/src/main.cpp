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

// 1.02: 0x1401E7A60 [0x1401E7990 + 0xD0]
// 1.03: 0x1401E7A70 [0x1401E79A0 + 0xD0]
SIG_SCAN
(
	sigSongPracticeStart,
	0x1401E79A0,
	"\x48\x89\x5C\x24\x18\x56\x57\x41\x54\x41\x56\x41\x57\x48\x83\xEC\x40\x41\x0F\xB6\xD9\x4D\x8B\xE0\x4C\x8B\xFA\x48\x8B\xF1\x48\x89\x4C\x24\x30\x4C\x8B\x31\x80\x7A\x19\x00\x0F\x85\x87\x00\x00\x00\x48\x89\x4C\x24\x20\x48\xC7\x44\x24\x28\x00\x00\x00\x00\xB9\x38",
	"xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
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
HOOK(void, __fastcall, _SongStart, sigSongStart(), int songId)
{
	if (m_Library)
	{
		// Playing
		p_OnSongUpdate(songId, false);
	}

	original_SongStart(songId);
}

HOOK(__int64, __fastcall, _SongPracticeStart, (uintptr_t)sigSongPracticeStart() + 0xD0)
{
	if (m_Library)
	{
		// Practicing
		p_OnSongUpdate(0, true);
	}

	return original_SongPracticeStart();
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
