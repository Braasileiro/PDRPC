#pragma once
#pragma comment(lib, "mscoree.lib")

#include <iostream>
#include <Windows.h>
#include <filesystem>
#include <metahost.h>

#include "logger.h"

void CLR_Assembly(HMODULE& hModule);
void CLR_Start();
void CLR_Dispose();
