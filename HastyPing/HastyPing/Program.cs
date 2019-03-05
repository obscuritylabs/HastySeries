using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Action
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1 || args[0].Equals("-help"))
            {
                Console.WriteLine("Usage is: ping [Host Name] | [Host IP] | -help");
                Console.WriteLine("HastyPing foo.bar.com");
                Console.WriteLine("HastyPing 123.123.123.123");
                Console.WriteLine("HastyPing -help (Returns this Help Message)");
                System.Environment.Exit(1);
            }
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.networkinformation.ping?view=netframework-4.7.2
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.Ttl = 128;
            options.DontFragment = true;
            // Create a buffer of 32 bytes of data to be transmitted.
            // Windows by default send 32 bytes so lets keep it the same
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = pingSender.Send(args[0], timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
            else
            {
                Console.WriteLine("Error: " + reply.Status);
            }
        }
    }
}
