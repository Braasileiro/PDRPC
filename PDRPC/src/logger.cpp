#include "logger.h"

void Logger::Redirect()
{
	FILE* dummy;
	freopen_s(&dummy, "CONIN$", "r", stdin);
	freopen_s(&dummy, "CONOUT$", "w", stderr);
	freopen_s(&dummy, "CONOUT$", "w", stdout);
}

void Logger::Info(std::string message)
{
	std::cout << "[PDRPC]: " << message << std::endl;
}

void Logger::Warning(std::string message)
{
	std::cout << "[PDRPC][WARN]: " << message << std::endl;
}

void Logger::Error(std::string message)
{
	std::cout << "[PDRPC][ERROR]: " << message << std::endl;
}
