using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace HastyArp
{
    class Program
    {
        // https://stackoverflow.com/questions/1148778/how-do-i-access-arp-protocol-information-through-net
        // The max number of physical addresses.
        const int MAXLEN_PHYSADDR = 8;

        // The insufficient buffer error.
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        // Define the MIB_IPNETROW structure.      
        [StructLayout(LayoutKind.Sequential)]
        struct MIB_IPNETROW
        {
            // https://docs.microsoft.com/en-us/openspecs/windows_protocols/ms-rrasm/1dab4bfb-a5dc-4763-afdb-5ea211f42ce1
            [MarshalAs(UnmanagedType.U4)]
            public uint dwIndex;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwPhysAddrLen;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac0;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac1;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac2;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac3;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac4;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac5;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac6;
            [MarshalAs(UnmanagedType.U1)]
            public byte mac7;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwAddr;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwType;
        }

        // Declare the GetIpNetTable function.
        [DllImport("IpHlpApi.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern int GetIpNetTable(IntPtr pIpNetTable,[MarshalAs(UnmanagedType.U4)] ref int pdwSize, bool bOrder);


        [DllImport("IpHlpApi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int FreeMibTable(IntPtr plpNetTable);

        static void Main(string[] args)
        {
            // The number of bytes needed.
            int bytesNeeded = 0;
            // The result from the API call.
            int result = GetIpNetTable(IntPtr.Zero, ref bytesNeeded, false);
            // Call the function, expecting an insufficient buffer.
            if (result != ERROR_INSUFFICIENT_BUFFER)
            {
                // Throw an exception.
                throw new Win32Exception(result);
            }
            // Allocate the memory, do it in a try/finally block, to ensure
            // that it is released.
            IntPtr buffer = IntPtr.Zero;
            // Try/finally.
            try
            {
                // Allocate the memory.
                buffer = Marshal.AllocCoTaskMem(bytesNeeded);

                // Make the call again. If it did not succeed, then
                // raise an error.
                result = GetIpNetTable(buffer, ref bytesNeeded, false);

                // If the result is not 0 (no error), then throw an exception.
                if (result != 0)
                {
                    // Throw an exception.
                    throw new Win32Exception(result);
                }

                // Now we have the buffer, we have to marshal it. We can read
                // the first 4 bytes to get the length of the buffer.
                int entries = Marshal.ReadInt32(buffer);

                // Increment the memory pointer by the size of the int.
                IntPtr currentBuffer = new IntPtr(buffer.ToInt64() +
                   Marshal.SizeOf(typeof(int)));

                // Allocate an array of entries.
                MIB_IPNETROW[] table = new MIB_IPNETROW[entries];

                // Cycle through the entries.
                for (int index = 0; index < entries; index++)
                {
                    // Call PtrToStructure, getting the structure information.
                    table[index] = (MIB_IPNETROW)Marshal.PtrToStructure(new
                       IntPtr(currentBuffer.ToInt64() + (index *
                       Marshal.SizeOf(typeof(MIB_IPNETROW)))), typeof(MIB_IPNETROW));
                }

                uint _arpInt = 0;
                for (int index = 0; index < entries; index++)
                {
                    MIB_IPNETROW row = table[index];
                    IPAddress ip = new IPAddress(BitConverter.GetBytes(row.dwAddr));
                    // check to see if we printed a new interface
                    // if so print like `arp -av` output
                    if (_arpInt != row.dwIndex)
                    {
                        // prior row dwindex does NOT match so its same interface
                        Console.WriteLine($"Interface: INT-{row.dwIndex.ToString()} {row.dwType} ---- IP-{ip.ToString()}");
                    }
                    Console.Write("\tIP:" + ip.ToString() + "\t\tMAC:");
                    // write mac to console
                    Console.Write(row.mac0.ToString("X2") + '-');
                    Console.Write(row.mac1.ToString("X2") + '-');
                    Console.Write(row.mac2.ToString("X2") + '-');
                    Console.Write(row.mac3.ToString("X2") + '-');
                    Console.Write(row.mac4.ToString("X2") + '-');
                    Console.WriteLine(row.mac5.ToString("X2"));
                    _arpInt = row.dwIndex;
                }
            }
            finally
            {
                // Release the memory.
                FreeMibTable(buffer);
            }
        }
    }
}
