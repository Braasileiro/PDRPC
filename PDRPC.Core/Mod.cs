using System;
using System.Threading;
using PDRPC.Core.Managers;

namespace PDRPC.Core
{
    public class Mod
    {
        [DllExport]
        public static void OnInit(int processId)
        {
            // Global
            Settings.ProcessId = processId;
            Settings.CurrentDirectory = Environment.CurrentDirectory;

            Logger.Info($"Current PID is {Settings.ProcessId}.");

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
            Logger.Info($"OnSongUpdate");

            // Update Presence
            DiscordManager.OnUpdateActivity(songId);
        }

        [DllExport]
        public static void OnDispose()
        {
            // Dispose things here
            DiscordManager.Dispose();
        }
    }
}
