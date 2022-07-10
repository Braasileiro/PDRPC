using System;

internal static class Logger
{
    private static readonly string PREFIX = "[PDRPC]";

    public static void Info(string message)
    {
        Console.WriteLine($"{PREFIX}: {message}");
    }

    public static void Warning(string message)
    {
        Console.WriteLine($"{PREFIX}[WARN]: {message}");
    }

    public static void Error(string message)
    {
        Console.WriteLine($"{PREFIX}[ERROR]: {message}");
    }

    public static void Error(Exception exception)
    {
        Console.WriteLine($"{PREFIX}[ERROR]: {exception.Message}");
    }
}
