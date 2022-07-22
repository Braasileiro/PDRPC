using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PDRPC.Core.Managers
{
    internal class ProcessManager
    {
        /*
         * Flags
         */
        private const int PROCESS_VM_READ = 0x0010;

        /*
         * Imports
         */
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, bool bInheritHandle, Int32 dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, UIntPtr lpBaseAddress, [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        /*
         * Interesting Stuff
         */
        private static Process mProcess;
        private static IntPtr mProcessHandle;
        
        public static bool Attach(int id)
        {
            try
            {
                // Current Process
                mProcess = Process.GetProcessById(id);

                if (mProcess != null)
                {
                    // Handle
                    mProcessHandle = OpenProcess(PROCESS_VM_READ, false, mProcess.Id);

                    Logger.Info("Attached to the game process.");

                    return true;
                }
                else
                {
                    Logger.Error("Game process not found.");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }

            return false;
        }

        public static int Read2Byte(long address)
        {
            byte[] buffer = new byte[4];

            ReadProcessMemory(
                mProcessHandle,
                (UIntPtr)((long)mProcess.MainModule.BaseAddress + address),
                buffer,
                (UIntPtr)2,
                IntPtr.Zero
            );

            return BitConverter.ToInt16(buffer, 0);
        }
    }
}
