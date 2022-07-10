using System;
using PDRPC.Core;
using PDRPC.Core.Managers;

namespace PDRPC
{
    public class Mod
    {
        public static int Init(string assemblyDir)
        {
            // Set current directory
            Environment.CurrentDirectory = assemblyDir;

            // Attempt to attach to the game process
            if (!ProcessManager.Attach(GameInfo.Process)) return 1;

            // Load Database
            if (!DatabaseManager.Load()) return 1;

            // Init Discord RPC
            if (!DiscordManager.Init()) return 1;

            return 0;
        }
    }
}
