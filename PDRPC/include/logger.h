#pragma once

#include <iostream>

class Logger
{
	public:
		static void Info(std::string message);
		static void Warning(std::string message);
		static void Error(std::string message);
};
