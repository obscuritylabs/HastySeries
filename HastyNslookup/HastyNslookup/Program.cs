using System;
using System.Net;

namespace Action
{
    /// <summary>
    /// 
    /// <title>nslookup</title>
    /// <description>
    /// *nix simple nslookup clone for the Win32 platform (Console Application)
    /// Does A DNS lookup by Host Name or IP. Host Name lookups can return
    /// multiple IP Ranges.
    /// </description>
    /// <author>Doug Bell</author>
    /// <version>1.0</version>
    /// <date>March 23 2002</date>
    /// 
    /// </summary>
    class NSLookup
    {
        /// <summary>
        /// 
        /// <description>Application entry point.</description>
        /// <param name="args">Host Address, IP Address or -help command line options</param>
        /// <return>int Return code 0 for success -1 for failure or error</return>
        /// 
        /// </summary>

        static int Main(string[] args)
        {
            //Make sure we were passed something, otherwise return help.
            if (args.Length < 1 || args[0].Equals("-help"))
            {
                Console.WriteLine("Usage is: nslookup [Host Name] | [Host IP] | -help");
                Console.WriteLine("nslookup foo.bar.com (Returns IP Address for Host Name)");
                Console.WriteLine("nslookup 123.123.123.123 (Returns Host Name for Address)");
                Console.WriteLine("nslookup -help (Returns this Help Message)");
                return -1;
            }
            else
            {
                //We have something, try to look it up....
                try
                {
                    //The IP or Host Entry to lookup
                    IPHostEntry ipEntry;
                    //The IP Address Array. Holds an array of resolved Host Names.
                    IPAddress[] ipAddr;
                    //Value of alpha characters
                    char[] alpha = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ-".ToCharArray();
                    //If alpha characters exist we know we are doing a forward lookup
                    if (args[0].IndexOfAny(alpha) != -1)
                    {
                        ipEntry = Dns.GetHostByName(args[0]);
                        ipAddr = ipEntry.AddressList;
                        Console.WriteLine("\nHost Name : " + args[0]);
                        int i = 0;
                        int len = ipAddr.Length;
                        for (i = 0; i < len; i++)
                        {
                            Console.WriteLine("Address {0} : {1} ", i, ipAddr[i].ToString());
                        }
                        return 0;
                    }
                    //If no alpha characters exist we do a reverse lookup
                    else
                    {
                        ipEntry = Dns.Resolve(args[0]);
                        Console.WriteLine("Address : " + args[0]);
                        Console.WriteLine("Host Name : " + ipEntry.HostName);
                        return 0;
                    }
                }
                catch (System.Net.Sockets.SocketException se)
                {
                    // The system had problems resolving the address passed
                    Console.WriteLine(se.Message.ToString());
                    return -1;
                }
                catch (System.FormatException fe)
                {
                    // Non unicode chars were probably passed 
                    Console.WriteLine(fe.Message.ToString());
                    return -1;
                }
            }
        }//End Main(string[])
    }//End NSLookup Class
}//End nslookup namespace