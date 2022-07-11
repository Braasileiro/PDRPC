using System;

internal static class Logger
{
    private static readonly string PREFIX = "[PDRPC]";

    public static void Info(object message)
    {
        Console.WriteLine($"{PREFIX}: {message}");
    }

    public static void Warning(object message)
    {
        Console.WriteLine($"{PREFIX}[WARN]: {message}");
    }

    public static void Error(object message)
    {
        Console.WriteLine($"{PREFIX}[ERROR]: {message}");
    }

    public static void Error(Exception exception)
    {
        Console.WriteLine($"{PREFIX}[ERROR]: {exception.Message}");
    }
}
