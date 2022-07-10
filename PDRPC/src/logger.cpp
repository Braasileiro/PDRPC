#include "logger.h"

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
