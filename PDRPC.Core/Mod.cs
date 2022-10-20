using System;
using System.Threading;
using PDRPC.Core.Managers;
using System.Runtime.InteropServices;

namespace PDRPC.Core
{
    public class Mod
    {
        [DllExport]
        public static void OnInit([MarshalAs(UnmanagedType.I4)] int pid, [MarshalAs(UnmanagedType.U8)] ulong pSongData)
        {
            // Initial Settings
            Settings.ProcessId = pid;
            Settings.CurrentDirectory = Environment.CurrentDirectory;

            // Addresses
            Settings.Addr.SongId = pSongData + 0x27;
            Settings.Addr.SongDifficulty = pSongData + 0x7;
            Settings.Addr.SongExtraFlag = pSongData + 0xB;
            Settings.Addr.SongPracticeFlag = pSongData + 0x1F;
            Settings.Addr.SongPvFlag = pSongData + 0x20;

            // Running everything in a separate thread to avoid any blocking
            new Thread(() =>
            {
                // Attach
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
        public static void OnSongUpdate([MarshalAs(UnmanagedType.I4)] int songId, [MarshalAs(UnmanagedType.I1)] bool isPractice)
        {
            DiscordManager.CheckUpdates(songId, isPractice);
        }

        [DllExport]
        public static void OnDispose()
        {
            // Dispose things here
            DiscordManager.Dispose();

            Logger.Info("Discord RPC Client disposed.");
        }
    }
}
