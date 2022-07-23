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

            // Running everything in a separate thread to avoid any blocking
            new Thread(() =>
            {
                // Attempt to attach to the game process
                if (ProcessManager.Attach(Settings.ProcessId))
                {
                    // Load Settings
                    DatabaseManager.LoadSettings();

                    // Load Database
                    if (DatabaseManager.LoadDatabase())
                    {
                        // Init Discord RPC
                        DiscordManager.Init();
                    }
                }
            }).Start();
        }

        [DllExport]
        public static void OnDispose()
        {
            // Dispose things here
            DiscordManager.Dispose();
        }
    }
}
