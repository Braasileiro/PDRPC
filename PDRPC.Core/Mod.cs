using System;
using System.Threading;
using PDRPC.Core.Managers;
using System.Threading.Tasks;

namespace PDRPC.Core
{
    public class Mod
    {
        [DllExport]
        public static void OnInit(int pid)
        {
            // Initial Settings
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
            // Async update to avoid any blocking
            Task.Run(() =>
            {
                DiscordManager.CheckUpdates(songId);
            });
        }

        [DllExport]
        public static void OnDispose()
        {
            // Dispose things here
            DiscordManager.Dispose();
        }
    }
}
