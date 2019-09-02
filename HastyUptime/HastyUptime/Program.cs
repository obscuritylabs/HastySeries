using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;

namespace HastyUptime
{
    class Program
    {
        [DllImport("kernel32")]
        extern static UInt64 GetTickCount64();

        public static TimeSpan GetUpTime()
        {
            return TimeSpan.FromMilliseconds(GetTickCount64());
        }

        public static string WmiLocalUptime()
        {
            ManagementScope scope = new ManagementScope(@"\\.\root\CIMV2");
            scope.Connect();
     
            return "";
        }


        static void Main(string[] args)
        {

            Console.WriteLine(GetUpTime());
            Console.ReadKey();
        }
    }
}
