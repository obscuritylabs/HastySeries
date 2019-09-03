

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ScreenMe
{
    public class xor
    {
        // https://stackoverflow.com/questions/14971836/xor-ing-strings-in-c-sharp
        // encryptDecrypt XOR 
        public static string encDec(string input)
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
    }
    public class ScreenCapture
    {
        public Image CaptureScreen()
        {
            IntPtr iHandle = IntPtr.Zero;
            iHandle = User32.GetDesktopWindow();
            if (iHandle.ToInt32() == 0)
            {
                // Throws error
                Console.ForegroundColor = ConsoleColor.Red;
                // Console.WriteLine("[!] FAIL: Open DesktopWindow handle error code: " + Marshal.GetLastWin32Error().ToString());
                Console.WriteLine(xor.encDec(xor.bDec("bAhgFnMHChoNcXMOOzYvaxckODg1JCMWIj0lJCRhIzIvLz8kazYzOTwzazAuLzZ7a3Q=")));
                Console.ResetColor();
                System.Environment.Exit(1);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            // Console.WriteLine("[*] SUCCESS: Obtained DesktopWindow Handle.");
            Console.WriteLine(xor.encDec(xor.bDec("bAhrFnMSHhACDgAScXMOKScgIj0kL3MFLiAqPzwxHDovLzw2axsgJTctLn1m")));
            Console.ResetColor();
            return CaptureWindow(iHandle);
        }
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up 
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }
        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
            Console.ForegroundColor = ConsoleColor.Green;
            // Console.WriteLine("[*] SUCCESS: Image file writen to disk.");
            Console.WriteLine(xor.encDec(xor.bDec("bAhrFnMSHhACDgAScXMIJjImLnMnIj8kayQzIickJXM1JHMlIiAqZXQ=")));
            Console.ResetColor();
        }
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
        public static void Screenshot(String filepath, String filename, ImageFormat format)
        {
            ScreenCapture sc = new ScreenCapture();

            string fullpath = filepath + "\\" + filename;

            sc.CaptureScreenToFile(fullpath, format);
        }
    }
}



namespace Action
{
    public class Action
    {
        static int Main(string[] args)
        {
            string OutPath = "";
            if (args.Length < 2 || args.Contains("-help"))
            {
                // Console.WriteLine("Usage:   HastyShot.exe [Special Folder] [File Name] | -help");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aQYyKjQkcXNhaxsgOCc4GDsuP30kMzZhEAAxLjAoKj9hDTwtLzYzFnMaDTotLnMPKj4kFnM9a34pLj8xaQ==")));
                // Console.WriteLine("Example: HastyShot.exe Desktop window_screenshot.jpeg (Sets outpath to user desktop and output file)");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aRY5Kj4xJzZ7axsgOCc4GDsuP30kMzZhDzYyICcuO3M2Ij0lJCQeODAzLjYvODsuP30rOzYma3sSLicyazw0PyMgPzthPzxhPiAkOXMlLiAqPzwxazIvL3MuPicxPidhLTotLnpj")));
                // Console.WriteLine("Current Environment SpecialFolders:");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("bBA0OSEkJSdhDj03IiEuJT4kJSdhGCMkKDogJxUuJzckOSB7bA==")));
                // Console.WriteLine("     * Desktop");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aXNha3NhYXMFLiAqPzwxaQ==")));
                // Console.WriteLine("     * Music");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aXNha3NhYXMMPiAoKHE=")));
                // Console.WriteLine("     * Documents");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aXNha3NhYXMFJDA0JjYvPyBj")));
                // Console.WriteLine("     * Pictures");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aXNha3NhYXMAOyMtIjAgPzouJRcgPzJj")));
                // Console.WriteLine("     * ApplicationData");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aXNha3NhYXMAOyMtIjAgPzouJRcgPzJj")));
                // Console.WriteLine("     * CommonApplicationData");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aXNha3NhYXMCJD4sJD0AOyMtIjAgPzouJRcgPzJj")));
                // Console.WriteLine("     * LocalApplicationData");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aXNha3NhYXMNJDAgJxIxOz8oKDI1IjwvDzI1KnE=")));
                // Console.WriteLine("HastyShot.exe -help (Returns this Help Message)");
                Console.WriteLine(ScreenMe.xor.encDec(ScreenMe.xor.bDec("aRsgOCc4GDsuP30kMzZhZjskJyNhYwEkPyYzJSBhPzsoOHMJLj8xax4kOCAgLDZoaQ==")));
                return -1;
            }
            switch (args[0].ToLower())
            {
                case "desktop":
                    OutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    break;
                case "music":
                    OutPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                    break;
                case "documents":
                    OutPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    break;
                case "pictures":
                    OutPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    break;
                case "applicationdata":
                    OutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    break;
                case "commonapplicationdata":
                    OutPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    break;
                case "localapplicationdata":
                    OutPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[*] IMAGE TARGET DETAILS:");
            Console.WriteLine("\t" + "OutPath: " + OutPath);
            Console.WriteLine("\t" + "OutFile: " + args[1]);
            Console.ResetColor();
            ScreenMe.ScreenCapture.Screenshot(OutPath, args[1], ImageFormat.Jpeg);
            // Use this version to capture the full extended desktop (i.e. multiple screens)
            return 0;
        }
    }
}





