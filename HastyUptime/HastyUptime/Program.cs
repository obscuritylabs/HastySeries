using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Management;
using System.Diagnostics;

namespace HastyUptime
{
    class Program
    {
        public static void GetTimeStamp()
        {
            var start = Stopwatch.StartNew();
            var ticks = Stopwatch.GetTimestamp();
            var uptime = ((double)ticks) / Stopwatch.Frequency;
            var uptimeTimeSpan = TimeSpan.FromSeconds(uptime);
            Console.WriteLine("[*] Current uptime using GetTimeStamp(): ");
            Console.WriteLine("\t* Days: " + uptimeTimeSpan.Days.ToString());
            Console.WriteLine("\t* Hours: " + uptimeTimeSpan.Hours.ToString());
            Console.WriteLine("\t* Minutes: " + uptimeTimeSpan.Minutes.ToString());
            Console.WriteLine("\t* Seconds: " + uptimeTimeSpan.Seconds.ToString());
        }

        static void Main(string[] args)
        {
            GetTimeStamp();
        }
    }
}
