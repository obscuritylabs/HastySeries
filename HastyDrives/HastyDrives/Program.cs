using System;
using System.IO;

namespace Action
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Lists drives on target system
             * Can be awesome to list locations, sizes and
             * location places user has normal access to
             */
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            string result = new String('-', 25);
            Console.WriteLine("*" + result + "HastyDrives" + result + "*");
            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("|Drive {0}", d.Name);
                Console.WriteLine("|  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("|  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("|  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "|  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "|  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "|  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
                Console.WriteLine("*" + result + "------------" + result + "*");
            }
        }
    }
}
