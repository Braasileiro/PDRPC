using System.Threading;
using PDRPC.Core.Managers;

namespace PDRPC.Core
{
    public class Mod
    {
        [DllExport]
        public static void OnInit()
        {
            // Running everything in a separate thread to avoid any blocking
            new Thread(() =>
            {
                // Attempt to attach to the game process
                if (ProcessManager.Attach(GameInfo.Process))
                {
                    // Load Database
                    if (DatabaseManager.Load())
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
