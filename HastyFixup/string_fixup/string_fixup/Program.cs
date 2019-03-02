using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace string_fixup
{
    class Program
    {
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
            if (args.Count() < 1)
            {
                // No args passed show some help options
                Console.WriteLine("[>] Ussage: string_fixup.exe '<STRING_TO_XOR>'");
                Console.WriteLine("    Example: string_fixup.exe 'help me fix this xor'");
                System.Environment.Exit(1);
            }
            string cli = Environment.GetCommandLineArgs()[0];
            string encrypted = encryptDecrypt(Environment.CommandLine.Replace(cli, "").TrimStart(' '));
            string encoded = Base64Encode(encrypted);
            Console.WriteLine("[*] Encrypted and encoded: " + encoded);

            string decoded = Base64Decode(encoded);
            string decrypted = encryptDecrypt(decoded);
            Console.WriteLine("[*] Decrypted and decoded: " + decrypted);
        }
    }
}
