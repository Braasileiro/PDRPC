// Source: https://github.com/blueskythlikesclouds/DivaModLoader/blob/master/Source/DivaModLoader/SigScan.cpp

#pragma once

#include <Psapi.h>

inline bool sigValid = true;

inline void* sigScan(const char* signature, const char* mask, size_t sigSize, void* memory, const size_t memorySize)
{
    if (sigSize == 0)
        sigSize = strlen(mask);

    for (size_t i = 0; i < memorySize; i++)
    {
        char* currMemory = (char*)memory + i;

        size_t j;
        for (j = 0; j < sigSize; j++)
        {
            if (mask[j] != '?' && signature[j] != currMemory[j])
                break;
        }

        if (j == sigSize)
            return currMemory;
    }

    return nullptr;
}

inline MODULEINFO moduleInfo;

inline const MODULEINFO& getModuleInfo()
{
    if (moduleInfo.SizeOfImage)
        return moduleInfo;

    ZeroMemory(&moduleInfo, sizeof(MODULEINFO));
    GetModuleInformation(GetCurrentProcess(), GetModuleHandle(nullptr), &moduleInfo, sizeof(MODULEINFO));

    return moduleInfo;
}

inline void* sigScan(const char* signature, const char* mask, void* hint)
{
    const MODULEINFO& info = getModuleInfo();
    const size_t sigSize = strlen(mask);

    // Ensure hint address is within the process memory region so there are no crashes.
    if ((hint >= info.lpBaseOfDll) && ((char*)hint + sigSize <= (char*)info.lpBaseOfDll + info.SizeOfImage))
    {
        void* result = sigScan(signature, mask, sigSize, hint, sigSize);

        if (result)
            return result;
    }

    return sigScan(signature, mask, sigSize, info.lpBaseOfDll, info.SizeOfImage);
}

#define SIG_SCAN(x, y, ...) \
    void* x(); \
    void* x##Addr = x(); \
    void* x() \
    { \
        constexpr const char* x##Data[] = { __VA_ARGS__ }; \
        constexpr size_t x##Size = _countof(x##Data); \
        if (!x##Addr) \
        { \
            if constexpr (x##Size == 2) \
            { \
                x##Addr = sigScan(x##Data[0], x##Data[1], (void*)(y)); \
                if (x##Addr) \
                    return x##Addr; \
            } \
            else \
            { \
                for (int i = 0; i < x##Size; i += 2) \
                { \
                    x##Addr = sigScan(x##Data[i], x##Data[i + 1], (void*)(y)); \
                    if (x##Addr) \
                        return x##Addr; \
                } \
            } \
            sigValid = false; \
        } \
        return x##Addr; \
    }
