﻿using System;
using System.Threading;
using PDRPC.Core.Managers;

namespace PDRPC.Core
{
    public class Mod
    {
        [DllExport]
        public static void OnInit(int pid)
        {
            // Global
            Settings.ProcessId = pid;
            Settings.CurrentDirectory = Environment.CurrentDirectory;

            // Running everything in a separate thread to avoid any blocking
            new Thread(() =>
            {
                // Load Settings
                DatabaseManager.LoadSettings();

                // Load Database
                if (DatabaseManager.LoadDatabase())
                {
                    // Init Discord RPC
                    DiscordManager.Init();
                }
            }).Start();
        }

        [DllExport]
        public static void OnSongUpdate(int songId)
        {
            // Update Activity
            DiscordManager.CheckUpdates(songId);
        }

        [DllExport]
        public static void OnDispose()
        {
            // Dispose things here
            DiscordManager.Dispose();

            // Terminate .NET stuff
            Environment.Exit(Environment.ExitCode);
        }
    }
}
