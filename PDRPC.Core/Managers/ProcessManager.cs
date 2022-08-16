using System;
using System.Text;
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

        private static UIntPtr GetAddress(long address, bool withBase)
        {
            if (withBase)
            {
                return (UIntPtr)((long)mProcess.MainModule.BaseAddress + address);
            }
            else
            {
                return (UIntPtr)address;
            }
        }

        public static int ReadInt32(long address, bool withBase = true)
        {
            byte[] buffer = new byte[4];

            ReadProcessMemory(
                mProcessHandle,
                GetAddress(address, withBase),
                buffer,
                (UIntPtr)4,
                IntPtr.Zero
            );

            return BitConverter.ToInt32(buffer, 0);
        }

        public static long ReadInt64(long address, bool withBase = true)
        {
            byte[] buffer = new byte[8];

            ReadProcessMemory(
                mProcessHandle,
                GetAddress(address, withBase),
                buffer,
                (UIntPtr)8,
                IntPtr.Zero
            );

            return BitConverter.ToInt64(buffer, 0);
        }

        public static string ReadString(long address, int size = 128, bool withBase = true)
        {
            byte[] buffer = new byte[size];

            ReadProcessMemory(
                mProcessHandle,
                GetAddress(address, withBase),
                buffer,
                (UIntPtr)buffer.Length,
                IntPtr.Zero
            );

            return Encoding.UTF8.GetString(buffer).Split('\0')[0];
        }
    }
}
